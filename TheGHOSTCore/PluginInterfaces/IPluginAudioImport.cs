using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public interface IPluginAudioImport : IPlugin
    {
        /// <summary>
        /// Convert an audio file to 16bit 44100hz Wav
        /// </summary>
        /// <param name="sourceFilename">Filename and path of input file</param>
        /// <param name="destinationPath">Filename and path of output file - without file extension</param>
        /// <returns>Converted file destinationFilename with file extension</returns>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        string Convert(string sourceFilename, string destinationFilename);
    }
}
