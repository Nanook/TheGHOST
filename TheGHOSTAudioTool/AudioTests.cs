using System;
using DirectShowLib;
using Nanook.TheGhost;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Nanook.TheGhost.AudioTool
{
    public class AudioTests : Form
    {
        //---------------------------------------------------------
        // Note: This plugin can convert any audio format to wav
        // providing a direct show filter is installed on the
        // machine for the source audio format. Usually, if the 
        // machine can play the source file, then this plugin
        // will be able to convert it
        //---------------------------------------------------------

        #region PeekMessage stuff
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [DllImport("user32.dll")]
        static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint MsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        private const int PM_REMOVE = 0x1;
        #endregion;


        #region constants
        private const int WM_APP = 0x8000;
        private const int WM_GRAPHNOTIFY = WM_APP + 1;
        private const int EC_COMPLETE = 0x01;
        #endregion

        #region private fields
        // filter graph object
        private FilterGraph _graph = null;
        // graph builder used to manipulate the filter graph
        private IGraphBuilder _graphBuilder = null;
        // source (input) filter
        private IBaseFilter _source = null;
        // media control used to render the filter graph
        private IMediaControl _mediaControl = null;
        // used for outputting to wav
        private IBaseFilter _wavDest = null;
        // used for saving the file out
        private FileWriter _fileWriter = null;
        // object used to check for DirectShow events
        private IMediaEventEx _mediaEvent = null;
        // h-result used for returning COM exceptions
        private IBaseFilter _xboxFilter = null;
        private IBaseFilter _acmWrapper = null;
        private IBaseFilter _wavParser = null;
        private int hr;
        #endregion


        #region private methods
        private void InitInterfaces()
        {
            try
            {
                // initialise the graph and get the graph builder and media control from it
                _graph = new FilterGraph();
                _mediaEvent = (IMediaEventEx)_graph;
                _graphBuilder = (IGraphBuilder)_graph;
                _mediaControl = (IMediaControl)_graph;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Couldn't initialise filter graph: " + ex.Message);
            }
        }

        /// <summary>
        /// Sets up the filter graph with the required filters
        /// </summary>
        private void InitGraph(string sourceFileName, string destinationFileName)
        {
            try
            {
                // Add the file source
                hr = _graphBuilder.AddSourceFilter(sourceFileName, "source", out _source);
                DsError.ThrowExceptionForHR(hr);

                // get the output pin from the source filter
                IPin sourceOutputPin = DsFindPin.ByDirection(_source, PinDirection.Output, 0);
                hr = _graphBuilder.Render(sourceOutputPin);
                DsError.ThrowExceptionForHR(hr);

                // check for the Default DirectSound Device filter. If it exists we can assume the
                // intelligent connect worked
                IBaseFilter DefaultOutputDevice;
                _graphBuilder.FindFilterByName("Default DirectSound Device", out DefaultOutputDevice);
                DsError.ThrowExceptionForHR(hr);

                // get the previous filter in the chain. (The one that is connected to the default direct sound device)
                // we have to loop through the filters in the graph to do this as we don't know what it might be called
                IEnumFilters filters;
                _graphBuilder.EnumFilters(out filters);
                DsError.ThrowExceptionForHR(hr);

                IBaseFilter[] outputFilter = new IBaseFilter[1];
                IntPtr fetched = IntPtr.Zero;
                int filterNo = 0;
                // filters enum seems to list the filters in reverse order (Starting with the default direct sound device)
                // so the second filter should be the one we're after
                do
                {
                    filters.Next(1, outputFilter, IntPtr.Zero);
                    filterNo += 1;

                } while (filterNo < 2);

                // get the output pin from the filter connected to the default direct sound device
                // we will use this later to connect to wav dest
                IPin outPin = DsFindPin.ByDirection(outputFilter[0], PinDirection.Output, 0);


                // add wav dest filter
                _wavDest = CreateAndAddFilterByName("WAV Dest", _graphBuilder, FilterCategory.LegacyAmFilterCategory);

                // add the file writer and file sink filters
                _fileWriter = new FileWriter();
                // make sure we access the IFileSinkFilter interface to set the file name
                IFileSinkFilter fs = (IFileSinkFilter)_fileWriter;
                hr = fs.SetFileName(destinationFileName, null);
                DsError.ThrowExceptionForHR(hr);
                // Add the file writer to the graph
                hr = _graphBuilder.AddFilter((IBaseFilter)_fileWriter, "output");
                DsError.ThrowExceptionForHR(hr);

                // disconnect and remove the default direct sound device filter (we don't need it)
                _graphBuilder.Disconnect(outPin);
                _graphBuilder.RemoveFilter(DefaultOutputDevice);

                // connect the output filter output pin to the wav dest input pin
                IPin wavDestInputPin = DsFindPin.ByDirection(_wavDest, PinDirection.Input, 0);
                hr = _graphBuilder.ConnectDirect(outPin, wavDestInputPin, new AMMediaType());
                DsError.ThrowExceptionForHR(hr);

                // connect the WAV Dest filter to the File Writer
                IPin wavDestOutputPin = DsFindPin.ByDirection(_wavDest, PinDirection.Output, 0);
                IPin fileWriterInputPin = DsFindPin.ByDirection((IBaseFilter)_fileWriter, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(wavDestOutputPin, fileWriterInputPin);
                DsError.ThrowExceptionForHR(hr);

                // DONE! the graph should be complete
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error initialising filter graph: " + ex.Message);
            }

        }

        private void InitWavConvertGraph(string sourceFileName, string destinationFileName)
        {
            try
            {
                // Add the file source
                hr = _graphBuilder.AddSourceFilter(sourceFileName, "source", out _source);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Unable to create mp3 source filter. Does the test file exist?", ex);
            }

            try
            {
                // get the output pin from the source filter
                IPin sourceOutputPin = DsFindPin.ByDirection(_source, PinDirection.Output, 0);
                hr = _graphBuilder.Render(sourceOutputPin);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Cannot render filter graph", ex);
            }

            IBaseFilter DefaultOutputDevice;

            try
            {
                // check for the Default DirectSound Device filter. If it exists we can assume the
                // intelligent connect worked
                
                _graphBuilder.FindFilterByName("Default DirectSound Device", out DefaultOutputDevice);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Couldn't find default playback device. Is there a sound card installed?", ex);
            }

            IBaseFilter[] outputFilter = new IBaseFilter[1];

            try
            {
                // get the previous filter in the chain. (The one that is connected to the default direct sound device)
                // we have to loop through the filters in the graph to do this as we don't know what it might be called
                IEnumFilters filters;
                _graphBuilder.EnumFilters(out filters);
                DsError.ThrowExceptionForHR(hr);

                
                IntPtr fetched = IntPtr.Zero;
                int filterNo = 0;
                // filters enum seems to list the filters in reverse order (Starting with the default direct sound device)
                // so the second filter should be the one we're after
                do
                {
                    filters.Next(1, outputFilter, IntPtr.Zero);
                    filterNo += 1;

                } while (filterNo < 2);


            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Graph building failed", ex);
            }

            // get the output pin from the filter connected to the default direct sound device
            // we will use this later to connect to wav dest
            IPin outPin = DsFindPin.ByDirection(outputFilter[0], PinDirection.Output, 0);

            try
            {
                // add wav dest filter
                _wavDest = CreateAndAddFilterByName("WAV Dest", _graphBuilder, FilterCategory.LegacyAmFilterCategory);
            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Can't create WAV Dest filter. Is it installed?", ex);
            }

            try
            {
                // add the file writer and file sink filters
                _fileWriter = new FileWriter();
                // make sure we access the IFileSinkFilter interface to set the file name
                IFileSinkFilter fs = (IFileSinkFilter)_fileWriter;
                hr = fs.SetFileName(destinationFileName, null);
                DsError.ThrowExceptionForHR(hr);
                // Add the file writer to the graph
                hr = _graphBuilder.AddFilter((IBaseFilter)_fileWriter, "output");
                DsError.ThrowExceptionForHR(hr);


                // disconnect and remove the default direct sound device filter (we don't need it)
                _graphBuilder.Disconnect(outPin);
                _graphBuilder.RemoveFilter(DefaultOutputDevice);
            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Unable to create FileWriter filters. Is the application path writeable (for output files)?", ex);
            }

            try
            {
                // connect the output filter output pin to the wav dest input pin
                IPin wavDestInputPin = DsFindPin.ByDirection(_wavDest, PinDirection.Input, 0);
                hr = _graphBuilder.ConnectDirect(outPin, wavDestInputPin, new AMMediaType());
                DsError.ThrowExceptionForHR(hr);

                // connect the WAV Dest filter to the File Writer
                IPin wavDestOutputPin = DsFindPin.ByDirection(_wavDest, PinDirection.Output, 0);
                IPin fileWriterInputPin = DsFindPin.ByDirection((IBaseFilter)_fileWriter, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(wavDestOutputPin, fileWriterInputPin);
                DsError.ThrowExceptionForHR(hr);

                // DONE! the graph should be complete
            }
            catch (Exception ex)
            {
                throw new CantConvertMP3Exception("Graph building failed", ex);
            }

        }

        private void InitPlayGraph(string sourceFileName)
        {
            try
            {
                // Add the file source
                hr = _graphBuilder.AddSourceFilter(sourceFileName, "source", out _source);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                throw new CantPlayMp3Exception("Unable to create wav source filter. Does the test file exist?", ex);
            }

            try
            {
                // get the output pin from the source filter
                IPin sourceOutputPin = DsFindPin.ByDirection(_source, PinDirection.Output, 0);
                hr = _graphBuilder.Render(sourceOutputPin);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                if (hr == -2147221231)
                {
                    throw new MissingCodecException("Cannot render filter. Is MP3 and Ogg Codec installed?", ex);
                }
                else
                {
                    throw new CantPlayMp3Exception("Cannot render filter graph", ex);
                }

            }

            try
            {
                // check for the Default DirectSound Device filter. If it exists we can assume the
                // intelligent connect worked
                IBaseFilter DefaultOutputDevice;
                _graphBuilder.FindFilterByName("Default DirectSound Device", out DefaultOutputDevice);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                throw new CantPlayMp3Exception("Couldn't find default playback device. Is there a sound card installed?", ex);
            }
        }

        private void InitXbadpcmConvertGraph(string sourceFileName, string outputFileName)
        {

            // create an instance of the xbox filter and add it to the graph
            _xboxFilter = CreateAndAddFilterByName("Xbox ADPCM", _graphBuilder, FilterCategory.AudioCompressorCategory);
            if (_xboxFilter != null)
            {
                // create an instance of the ACM Wrapper filter
                _acmWrapper = CreateAndAddFilterByName("ACM Wrapper", _graphBuilder, FilterCategory.LegacyAmFilterCategory);
                if (_acmWrapper != null)
                {
                    // Add WavParser
                    _wavParser = CreateAndAddFilterByName("Wave Parser", _graphBuilder, FilterCategory.LegacyAmFilterCategory);
                    if (_wavParser != null)
                    {
                        // Add WavDest filter
                        _wavDest = CreateAndAddFilterByName("WAV Dest", _graphBuilder, FilterCategory.LegacyAmFilterCategory);
                        if (_wavDest != null)
                        {
                            try
                            {
                                // Add the file source
                                hr = _graphBuilder.AddSourceFilter(sourceFileName, "source", out _source);
                                DsError.ThrowExceptionForHR(hr);
                            }
                            catch (Exception ex)
                            {
                                throw new XbadPcmConvertException("Unable to create xbox wav source filter. Does the test file exist?", ex);
                            }

                            try
                            {
                                // we want to add a filewriter filter to the filter graph
                                _fileWriter = new FileWriter();
                                // make sure we access the IFileSinkFilter interface to
                                // set the file name
                                IFileSinkFilter fs = (IFileSinkFilter)_fileWriter;
                                hr = fs.SetFileName(outputFileName, null);
                                DsError.ThrowExceptionForHR(hr);
                                // Add the file writer to the graph
                                hr = _graphBuilder.AddFilter((IBaseFilter)_fileWriter, "output");
                                DsError.ThrowExceptionForHR(hr);
                            }
                            catch (Exception ex)
                            {
                                throw new XbadPcmConvertException("Graph Building Failed", ex);
                            }
                        }
                        else
                        {
                            throw new XbadPcmConvertException("Could not load Wav Dest filter. Is it installed?");
                        }
                    }
                    else
                    {
                        throw new XbadPcmConvertException("Could not load wave parser");
                    }
                }
                else
                {
                    throw new XbadPcmConvertException("Could not load ACM Wrapper");
                }
            }
            else
            {
                throw new XbadPcmConvertException("Could not load Xbox Filter. Is it installed?");
            }


        }

        private void ConnectPins(bool mono)
        {
            try
            {
                // connect the source filter to the wave parser
                IPin sourceOutputPin = DsFindPin.ByDirection(_source, PinDirection.Output, 0);
                IPin wavParserInputPin = DsFindPin.ByDirection(_wavParser, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(sourceOutputPin, wavParserInputPin);
                DsError.ThrowExceptionForHR(hr);

                // connect the wave parser to the ACM Wrapper
                IPin wavParserOutputPin = DsFindPin.ByDirection(_wavParser, PinDirection.Output, 0);
                IPin ACMWrapperInputPin = DsFindPin.ByDirection(_acmWrapper, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(wavParserOutputPin, ACMWrapperInputPin);
                DsError.ThrowExceptionForHR(hr);

                // connect the ACM Wrapper to the Xbox filter
                IPin acmWrapperOutputPin = DsFindPin.ByDirection(_acmWrapper, PinDirection.Output, 0);
                IPin xboxFilterInputPin = DsFindPin.ByDirection(_xboxFilter, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(acmWrapperOutputPin, xboxFilterInputPin);
                DsError.ThrowExceptionForHR(hr);
                ChangePinFormat(acmWrapperOutputPin, 32000, mono);

                // connect the Xbox filter to the WAV Dest filter
                IPin xboxFilterOutputPin = DsFindPin.ByDirection(_xboxFilter, PinDirection.Output, 0);
                IPin wavDestInputPin = DsFindPin.ByDirection(_wavDest, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(xboxFilterOutputPin, wavDestInputPin);
                DsError.ThrowExceptionForHR(hr);
                //ChangePinFormat(xboxFilterOutputPin, 32000, mono);

                // connect the WAV Dest filter to the File Writer
                IPin wavDestOutputPin = DsFindPin.ByDirection(_wavDest, PinDirection.Output, 0);
                IPin fileWriterInputPin = DsFindPin.ByDirection((IBaseFilter)_fileWriter, PinDirection.Input, 0);
                hr = _graphBuilder.Connect(wavDestOutputPin, fileWriterInputPin);
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error connecting pins: " + ex.Message);
            }
        }

        private void ChangePinFormat(IPin pin, int samplesPerSec, bool forceMono)
        {
            AMMediaType outputMediaType = new AMMediaType();
            pin.ConnectionMediaType(outputMediaType);
            var formatEx = Marshal.PtrToStructure(outputMediaType.formatPtr, typeof(WaveFormatEx)) as WaveFormatEx;
            formatEx.nSamplesPerSec = samplesPerSec;

            if (forceMono)
                formatEx.nChannels = 1;

            formatEx.nBlockAlign = (short)(formatEx.nChannels * 2);
            formatEx.nAvgBytesPerSec = formatEx.nSamplesPerSec * formatEx.nBlockAlign;

            Marshal.StructureToPtr(formatEx, outputMediaType.formatPtr, true);
            IAMStreamConfig streamConfig = (IAMStreamConfig)pin;
            streamConfig.SetFormat(outputMediaType);
        }


        private IBaseFilter CreateAndAddFilterByName(string filterName, IGraphBuilder graphBuilder, Guid category)
        {
            IBaseFilter filter = null;
            try
            {
                // get a list of devices (filters) from the specified category
                DsDevice[] devices = DsDevice.GetDevicesOfCat(category);

                // loop through the filters to find the one we're looking for
                for (int i = 0; i < devices.Length; i++)
                {
                    if (!devices[i].Name.Equals(filterName))
                        continue;

                    hr =
                        (graphBuilder as IFilterGraph2).AddSourceFilterForMoniker(devices[i].Mon, null, filterName,
                                                                                  out filter);
                    DsError.ThrowExceptionForHR(hr);

                    break;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating filter: " + ex.Message);
            }
            return filter;
        }

        private void DisposeObjects()
        {
            ReleaseCOMObject(_graphBuilder);
            ReleaseCOMObject(_source);
            ReleaseCOMObject(_mediaControl);
            ReleaseCOMObject(_wavDest);
            ReleaseCOMObject(_fileWriter);
            ReleaseCOMObject(_mediaEvent);
            ReleaseCOMObject(_acmWrapper);
            ReleaseCOMObject(_wavParser);
            ReleaseCOMObject(_xboxFilter);
        }

        private void ReleaseCOMObject(object obj)
        {
            // release COM object
            if (obj != null)
                Marshal.ReleaseComObject(obj);
        }

        #endregion

        public bool PlayTest(string testSource)
        {
            InitInterfaces();
            InitPlayGraph(testSource);
            try
            {
                // route DirectShow messges to this form
                _mediaEvent.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                // start rendering
                _mediaControl.Run();

                MSG m;
                bool complete = false;
                do
                {
                    if (PeekMessage(out m, this.Handle, (uint)WM_GRAPHNOTIFY, (uint)WM_GRAPHNOTIFY, PM_REMOVE))
                    {
                        if (m.message == WM_GRAPHNOTIFY)
                        {
                            EventCode eCode;
                            IntPtr param1, param2;
                            try
                            {
                                // get the event from the queue
                                _mediaEvent.GetEvent(out eCode, out param1, out param2, 0);
                                _mediaEvent.FreeEventParams(eCode, param1, param2);

                                if (eCode == EventCode.Complete)
                                {
                                    _mediaControl.Stop();
                                    complete = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(50);
                }
                while (!complete);

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error converting: " + ex.Message);
            }
            finally
            {
                DisposeObjects(); // clean up
            }
        }



        public string WavConvertTest(string sourceFilename, string destinationFilename)
        {
            InitInterfaces();
            InitWavConvertGraph(sourceFilename, destinationFilename);
            try
            {
                // route DirectShow messges to this form
                _mediaEvent.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                // start rendering
                _mediaControl.Run();

                MSG m;
                bool complete = false;
                do
                {
                    if (PeekMessage(out m, this.Handle, (uint)WM_GRAPHNOTIFY, (uint)WM_GRAPHNOTIFY, PM_REMOVE))
                    {
                        if (m.message == WM_GRAPHNOTIFY)
                        {
                            EventCode eCode;
                            IntPtr param1, param2;
                            try
                            {
                                // get the event from the queue
                                _mediaEvent.GetEvent(out eCode, out param1, out param2, 0);
                                _mediaEvent.FreeEventParams(eCode, param1, param2);

                                if (eCode == EventCode.Complete)
                                {
                                    _mediaControl.Stop();
                                    complete = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(50);
                }
                while (!complete);

                return destinationFilename;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error converting: " + ex.Message);
            }
            finally
            {
                DisposeObjects(); // clean up
            }
        }


        public string XBadPcmConvertTest(string sourceFilename, string destinationFilename)
        {
            InitInterfaces();
            InitXbadpcmConvertGraph(sourceFilename, destinationFilename);
            ConnectPins(false);
            try
            {
                // route DirectShow messges to this form
                _mediaEvent.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                _mediaControl.Run();

                MSG m;
                bool complete = false;
                do
                {
                    if (PeekMessage(out m, this.Handle, (uint)WM_GRAPHNOTIFY, (uint)WM_GRAPHNOTIFY, PM_REMOVE))
                    {
                        if (m.message == WM_GRAPHNOTIFY)
                        {
                            EventCode eCode;
                            IntPtr param1, param2;
                            try
                            {
                                // get the event from the queue
                                _mediaEvent.GetEvent(out eCode, out param1, out param2, 0);
                                _mediaEvent.FreeEventParams(eCode, param1, param2);

                                if (eCode == EventCode.Complete)
                                {
                                    _mediaControl.Stop();
                                    complete = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(50);
                }
                while (!complete);

                return destinationFilename;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error converting: " + ex.Message);
            }
            finally
            {
                DisposeObjects();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WindowsAudio
            // 
            this.Name = "WindowsAudio";
            this.ResumeLayout(false);

        }

    }
}
