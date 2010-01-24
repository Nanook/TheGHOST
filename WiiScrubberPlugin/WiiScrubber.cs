using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace Nanook.TheGhost.Plugins
{
    public partial class WiiScrubberPlugin : Form, IPluginFileCopy
    {
        [DllImport("WiiScrubber.dll")]
        static extern void ExtractFiles([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string[] files, string[] dests, int fileCount, bool caseSensitive, int partNo, IntPtr hwnd);

//        [DllImport("WiiScrubber.dll")]
//        static extern void ExtractFiles([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string[] files, string[] dests, int fileCount, bool caseSensitive, IntPtr hwnd);

        [DllImport("WiiScrubber.dll")]
        static extern void ReplaceFiles([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string[] files, string[] paths, int fileCount, bool caseSensitive, bool appendDiffSizeFiles, int partNo, IntPtr hwnd);

//        [DllImport("WiiScrubber.dll")]
//        static extern void ReplaceFiles([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string[] files, string[] paths, int fileCount, bool caseSensitive, IntPtr hwnd);

        [DllImport("WiiScrubber.dll")]
        static extern IntPtr GetDiscId([MarshalAs(UnmanagedType.LPStr)]string isoPath);

        [DllImport("WiiScrubber.dll")]
        static extern bool FileExists([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string findFileName, int partNo, bool caseSensitive);

        const int WM_USER = 0x0400;

        public WiiScrubberPlugin()
        {
            InitializeComponent();
            _currPath = ((FileInfo)new FileInfo(Assembly.GetEntryAssembly().Location)).DirectoryName;
            _partitionNo = _DataPartition;
            _partitionProven = false;
            _handle = this.Handle;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_USER + 666)
            {
                if (_callback != null)
                {
                    int i = m.WParam.ToInt32();
                    _callback(_gameFiles[i], _diskFiles[i], i, _gameFiles.Length, 1F, m.LParam.ToInt32() == 1);
                }
            }

            base.WndProc(ref m);
        }

        #region IPlugin Members

        string IPlugin.Title()
        {
            return "Wii Scrubber";
        }

        string IPlugin.Description()
        {
            return "This plugin uses WiiScrubber to import and export files to an ISO image. WiiScrubber.dll must be in the same folder as this plugin.  Set the Game Location to the ISO.";
        }

        float IPlugin.Version()
        {
            return 0.1F;
        }

        #endregion

        #region IPluginFileCopy Members

        void IPluginFileCopy.SetSourcePath(string path)
        {
            _isoPath = path;
        }

        private void findMainPartition()
        {
            if (!_partitionProven)
            {
                int p = _partitionNo;

                while (p >= 0 && !_partitionProven)
                {
                    if (FileExists(_isoPath, "opening.bnr", p, true))
                    {
                        _partitionProven = true;
                        _partitionNo = p;
                        break;
                    }
                    p--;
                }

            }

            if (!_partitionProven)
                _partitionNo = _DataPartition;
        }

        private void extract(string[] gameFilenames, string[] diskFilenames, FileCopyProgress callback)
        {
            if (gameFilenames.Length == 0)
                return;

            findMainPartition();

            FileInfo f;
            foreach (string s in diskFilenames)
            {
                //create all folders
                f = new FileInfo(s);
                if (!Directory.Exists(f.DirectoryName))
                    Directory.CreateDirectory(f.DirectoryName);
                else if (File.Exists(s))
                    FileHelper.Delete(s);
            }

            _gameFiles = gameFilenames;
            _diskFiles = diskFilenames;
            _callback = callback;

            //MessageBox.Show("Current Path: " + Environment.CurrentDirectory);

            string currPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = _currPath;
            //MessageBox.Show("Path Before Extract: " + Environment.CurrentDirectory);
            ExtractFiles(_isoPath, gameFilenames, diskFilenames, diskFilenames.Length, false, _partitionNo, _handle);
            if (Directory.Exists(currPath))
                Environment.CurrentDirectory = currPath;
            //MessageBox.Show("Path After Extract: " + Environment.CurrentDirectory);

        }

        private void replace(string[] gameFilenames, string[] diskFilenames, FileCopyProgress callback)
        {
            if (gameFilenames.Length == 0)
                return;

            findMainPartition();

            _gameFiles = gameFilenames;
            _diskFiles = diskFilenames;
            _callback = callback;

            string currPath = Environment.CurrentDirectory;
            Environment.CurrentDirectory = _currPath;
            ReplaceFiles(_isoPath, gameFilenames, diskFilenames, diskFilenames.Length, false, false, _partitionNo, _handle);
            if (Directory.Exists(currPath))
                Environment.CurrentDirectory = currPath;
        }

        /// <summary>
        /// Import a single file in to TheGHOST from a Wii ISO
        /// </summary>
        /// <param name="gameFilename">Lowercase path and filename seperated by \</param>
        /// <param name="diskFilename">Local disk path and filename</param>
        void IPluginFileCopy.ImportFile(string gameFilename, string diskFilename, FileCopyProgress callback)
        {
            extract(new string[] { gameFilename }, new string[] { diskFilename }, callback);
        }

        /// <summary>
        /// Import files in to TheGHOST from a Wii iso
        /// </summary>
        /// <param name="gameFilenames">Lowercase path and filenames seperated by \</param>
        /// <param name="diskFilenames">local disk path and filenames</param>
        void IPluginFileCopy.ImportFiles(string[] gameFilenames, string[] diskFilenames, FileCopyProgress callback)
        {
            extract(gameFilenames, diskFilenames, callback);
        }

        /// <summary>
        /// Export a single file from TheGHOST to a Wii ISO
        /// </summary>
        /// <param name="diskFilename">Local disk path and filename</param>
        /// <param name="gameFilename">Lowercase path and filename seperated by \</param>
        void IPluginFileCopy.ExportFile(string diskFilename, string gameFilename, FileCopyProgress callback)
        {
            replace(new string[] { gameFilename }, new string[] { diskFilename }, callback);
        }

        /// <summary>
        /// Export a files from TheGHOST to a Wii ISO
        /// </summary>
        /// <param name="diskFilenames">Local disk path and filenames</param>
        /// <param name="gameFilenames">Lowercase path and filenames seperated by \</param>
        void IPluginFileCopy.ExportFiles(string[] diskFilenames, string[] gameFilenames, FileCopyProgress callback)
        {
            replace(gameFilenames, diskFilenames, callback);
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WiiScrubberPlugin
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "WiiScrubberPlugin";
            this.ResumeLayout(false);

        }

        private const int _DataPartition = 1;
        private int _partitionNo;
        private bool _partitionProven;

        private IntPtr _handle; //for cross threaded access

        private string[] _gameFiles;
        private string[] _diskFiles;
        private FileCopyProgress _callback;
        private string _isoPath;
        private string _currPath;
    }
}
