using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public delegate void FileCopyProgress(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success);

    public interface IPluginFileCopy : IPlugin
    {
        /// <summary>
        /// Informs the plugin of the source path of game data or ISO.
        /// </summary>
        /// <param name="path">Path of game data or ISO.</param>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        void SetSourcePath(string path);

        /// <summary>
        /// Retrieve a file from the game (file structure or ISO) and copy it to the diskPath.
        /// </summary>
        /// <param name="gameFilename">Path and filename of import file.</param>
        /// <param name="diskPath">Path on the local disk to save file to.</param>
        /// <returns>The path and filename where file was imported to.</returns>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        void ImportFile(string gameFilename, string diskFilename, FileCopyProgress callback);

        /// <summary>
        /// Retrieve an array of files from the game (file structure or ISO) and copy it to the diskPath.
        /// </summary>
        /// <param name="gameFilename">Path and filename of import files.</param>
        /// <param name="diskPath">Path on the local disk to save files to.</param>
        /// <returns>The path and filenames where files were imported to.</returns>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        void ImportFiles(string[] gameFilenames, string[] diskFilenames, FileCopyProgress callback);

        /// <summary>
        /// Write a file from the diskFilename to the game (file structure or ISO).
        /// </summary>
        /// <param name="diskPath">Path on the local disk to read the file from.</param>
        /// <param name="gameFilename">Path and filename of destination game file.</param>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        void ExportFile(string diskFilename, string gameFilename, FileCopyProgress callback);

        /// <summary>
        /// Write a file from the diskFilename to the game (file structure or ISO).
        /// </summary>
        /// <param name="diskPath">Path on the local disk to read the files from.</param>
        /// <param name="gameFilename">Path and filenames of destination game files.</param>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        void ExportFiles(string[] diskFilenames, string[] gameFilenames, FileCopyProgress callback);
    }
}
