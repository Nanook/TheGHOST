using System;
using System.Collections.Generic;
using System.Text;
using DirectShowLib;
using System.Runtime.InteropServices;
using Nanook.TheGhost;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace Nanook.TheGhost.Plugins
{
    /// <summary>
    /// Provides methods for converting audio between formats
    /// </summary>
    public class Xbadpcm : Form, IPluginAudioExport
    {
        #region PeekMessage stuff
        //[StructLayout(LayoutKind.Sequential)]
        //public struct POINT
        //{
        //    public int X;
        //    public int Y;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct MSG
        //{
        //    public IntPtr hwnd;
        //    public uint message;
        //    public IntPtr wParam;
        //    public IntPtr lParam;
        //    public uint time;
        //    public POINT pt;
        //}

        //[DllImport("user32.dll")]
        //static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint MsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        //[DllImport("user32.dll")]
        //static extern bool TranslateMessage([In] ref MSG lpMsg);

        //[DllImport("user32.dll")]
        //static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        //private const int PM_REMOVE = 0x1;
        #endregion;

        private bool _complete;

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
        // media control used to render the filter graph
        private IMediaControl _mediaControl = null;
        // object used to check for DirectShow events
        private IMediaEventEx _mediaEvent = null;
        // int used to store hresult from COM operations
        private int hr;
        // required filters
        private IBaseFilter _xboxFilter = null;
        private IBaseFilter _acmWrapper = null;
        private IBaseFilter _wavParser = null;
        private IBaseFilter _wavDest = null;
        private IBaseFilter _source = null;
        private FileWriter _fileWriter = null;
        #endregion

        public Xbadpcm()
        {
        }


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

        private void InitFilters(string sourceFileName, string outputFileName)
        {
            try
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
                                // Add the file source
                                hr = _graphBuilder.AddSourceFilter(sourceFileName, "source", out _source);
                                DsError.ThrowExceptionForHR(hr);

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
                            else
                            {
                                throw new Exception("Could not load Wav Dest filter. Is it installed?");
                            }
                        }
                        else
                        {
                            throw new Exception("Could not load wave parser");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not load ACM Wrapper");
                    }
                }
                else
                {
                    throw new Exception("Could not load Xbox Filter. Is it installed?");
                }
            }
            catch (Exception ex)
            {
               throw new ApplicationException("Error initialising filters: " + ex.Message);
            }
        }

        private IBaseFilter CreateAndAddFilterByName(string filterName, IGraphBuilder graphBuilder, Guid category)
        {
            IBaseFilter filter = null;
            try
            {
                // get a list of devices (filters) from the specified category
                int hr = 0;
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


        private void ConnectPins(bool forceMono, int forceDownSample)
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
                if (forceDownSample != 0 || forceMono)
                    ChangePinFormat(acmWrapperOutputPin, forceDownSample, forceMono);

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

            if (samplesPerSec != 0 && samplesPerSec < formatEx.nSamplesPerSec)
                formatEx.nSamplesPerSec = samplesPerSec;

            if (forceMono)
                formatEx.nChannels = 1;

            formatEx.nBlockAlign = (short)(formatEx.nChannels * 2); // * 2 for 16 bit
            formatEx.nAvgBytesPerSec = formatEx.nSamplesPerSec * formatEx.nBlockAlign;

            Marshal.StructureToPtr(formatEx, outputMediaType.formatPtr, true);
            IAMStreamConfig streamConfig = (IAMStreamConfig)pin;
            streamConfig.SetFormat(outputMediaType);
        }


        private void DisposeObjects()
        {
            ReleaseCOMObject(_source);
            ReleaseCOMObject(_graphBuilder);
            ReleaseCOMObject(_graph);
            ReleaseCOMObject(_acmWrapper);
            ReleaseCOMObject(_wavParser);
            ReleaseCOMObject(_xboxFilter);
            ReleaseCOMObject(_mediaControl);
            ReleaseCOMObject(_wavDest);
            ReleaseCOMObject(_fileWriter);
            ReleaseCOMObject(_mediaEvent);
        }

        private void ReleaseCOMObject(object obj)
        {
            // release COM object
            if (obj != null)
                Marshal.ReleaseComObject(obj);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                EventCode lEventCode;
                IntPtr lParam1, lParam2;

                try
                {
                    _mediaEvent.GetEvent(out lEventCode, out lParam1, out lParam2, 0);
                    _mediaEvent.FreeEventParams(lEventCode, lParam1, lParam2);

                    if (lEventCode == EventCode.Complete)
                    {
                        _mediaControl.StopWhenReady();
                        _complete = true;
                    }
                }
                catch
                {
                    _complete = true; //let wait loop drop out
                    throw;
                }
            }
            else
                base.WndProc(ref m);
        }


        #region IPluginAudioExport Members

        string IPluginAudioExport.Convert(string sourceFilename, string destinationFilename, bool forceMono, int forceDownSample)
        {
            string currPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = ((FileInfo)new FileInfo(Assembly.GetEntryAssembly().Location)).DirectoryName;

            InitInterfaces();
            InitFilters(sourceFilename, destinationFilename);
            ConnectPins(forceMono, forceDownSample);
            try
            {
                // route DirectShow messges to this form
                _mediaEvent.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);

                _complete = false;
                _mediaControl.Run();

                while (!_complete)
                    Application.DoEvents();

                if (Directory.Exists(currPath))
                    Environment.CurrentDirectory = currPath;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error converting: " + ex.Message);
            }
            finally
            {
                DisposeObjects();
            }


            //don't return until a full file lock can be gained or 5 seconds.
            DateTime dt = DateTime.Now.AddSeconds(5);
            while (DateTime.Now <= dt)
            {
                try
                {
                    using (FileStream fs = new FileStream(destinationFilename, FileMode.Open, FileAccess.ReadWrite)) { }
                    //System.Diagnostics.Debug.WriteLine("No Lock: " + DateTime.Now.ToString("s"));
                    break;
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine("Lock: " + DateTime.Now.ToString("s"));
                }
            }

            return destinationFilename;

        }

        
        #endregion

        #region IPlugin Members

        string IPlugin.Title()
        {
            return "XBADPCM Plugin";
        }

        string IPlugin.Description()
        {
            return "Export audio from TheGHOST to XBADPCM format for the Wii. See the guide for setup instructions.";
        }

        float IPlugin.Version()
        {
            return 0.2F;
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WindowsAudio
            // 
            this.Name = "Xbadpcm";
            this.ResumeLayout(false);

        }

    }
}
