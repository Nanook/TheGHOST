using System;
using DirectShowLib;
using Nanook.TheGhost;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Nanook.TheGhost.Plugins
{
    public class WindowsAudio : Form, IPluginAudioImport
    {
        //---------------------------------------------------------
        // Note: This plugin can convert any audio format to wav
        // providing a direct show filter is installed on the
        // machine for the source audio format. Usually, if the 
        // machine can play the source file, then this plugin
        // will be able to convert it
        //---------------------------------------------------------

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
        }

        private void ReleaseCOMObject(object obj)
        {
            // release COM object
            if (obj != null)
                Marshal.ReleaseComObject(obj);
        }

        #endregion

        
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
                        _mediaControl.Stop(); // StopWhenReady();
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



        #region IPluginAudioImport Members
        string IPluginAudioImport.Convert(string sourceFilename, string destinationFilename)
        {
            string currPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = ((FileInfo)new FileInfo(Assembly.GetEntryAssembly().Location)).DirectoryName;

            InitInterfaces();
            InitGraph(sourceFilename, destinationFilename);
            try
            {
                // route DirectShow messges to this form
                _mediaEvent.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                // start rendering

                _complete = false;
                _mediaControl.Run();

                while (!_complete)
                    Application.DoEvents();

                if (Directory.Exists(currPath))
                    Environment.CurrentDirectory = currPath;

                //don't return until a full file lock can be gained or 5 seconds.
                DateTime dt = DateTime.Now.AddSeconds(5);
                while (DateTime.Now <= dt)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(destinationFilename, FileMode.Open, FileAccess.ReadWrite)) { }
                        break;
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("s"));
                    }
                }

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

        #endregion

        #region IPlugin Members

        string IPlugin.Description()
        {
            return "Import Windows audio in to TheGHOST";
        }

        string IPlugin.Title()
        {
            return "Windows Audio";
        }

        float IPlugin.Version()
        {
            return 0.1F;
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "WindowsAudio";
            this.ResumeLayout(false);

        }


    }
}
