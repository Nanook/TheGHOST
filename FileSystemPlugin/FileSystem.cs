using System;
using System.Collections.Generic;
using System.Text;
using Nanook.TheGhost;
using System.IO;
using System.Reflection;


namespace Nanook.TheGhost.Plugins
{
    public class FileSystem : IPluginFileCopy
    {
        public FileSystem()
        {
            _currPath = ((FileInfo)new FileInfo(Assembly.GetEntryAssembly().Location)).DirectoryName;
        }

        private void importFile(string gameFilename, string diskFilename, int index, int count, FileCopyProgress callback)
        {
            char s = Path.DirectorySeparatorChar;

            string g = Path.Combine(_srcPath, gameFilename.Replace('/', s));
            string d = diskFilename;

            bool success = File.Exists(g);
            
            File.Copy(g, d, true);

            if (callback != null)
                callback(gameFilename, diskFilename, index, count, 100F, success);
        }

        private void exportFile(string gameFilename, string diskFilename, int index, int count, FileCopyProgress callback)
        {
            char s = Path.DirectorySeparatorChar;

            string g = Path.Combine(_srcPath, gameFilename.Replace('/', s));
            string d = diskFilename;

            bool success = File.Exists(d);

            File.Copy(d, g, true);

            if (callback != null)
                callback(gameFilename, diskFilename, index, count, 100F, success);
        }

        #region IPluginFileCopy Members

        void IPluginFileCopy.SetSourcePath(string path)
        {
            _srcPath = path;
        }

        void IPluginFileCopy.ImportFile(string gameFilename, string diskFilename, FileCopyProgress callback)
        {
            importFile(gameFilename, diskFilename, 1, 1, callback);
        }

        void IPluginFileCopy.ImportFiles(string[] gameFilenames, string[] diskFilenames, FileCopyProgress callback)
        {
            for (int i = 0; i < gameFilenames.Length; i++)
                importFile(gameFilenames[i], diskFilenames[i], i, gameFilenames.Length, callback);
        }

        void IPluginFileCopy.ExportFile(string diskFilename, string gameFilename, FileCopyProgress callback)
        {
            exportFile(gameFilename, diskFilename, 1, 1, callback);
        }

        void IPluginFileCopy.ExportFiles(string[] diskFilenames, string[] gameFilenames, FileCopyProgress callback)
        {
            for (int i = 0; i < gameFilenames.Length; i++)
                exportFile(gameFilenames[i], diskFilenames[i], i, gameFilenames.Length, callback);
        }

        #endregion

        #region IPlugin Members

        string IPlugin.Title()
        {
            return "File System";
        }

        string IPlugin.Description()
        {
            return "This plugin uses the file system. It can be used with TheGHOSTWiiIsoTool and ManualEdit. Set the Game Location to the directory containing the Data Partition files.";
        }

        float IPlugin.Version()
        {
            return 0.1F;
        }

        #endregion

        private string _srcPath;
        private string _currPath;
    }
}
