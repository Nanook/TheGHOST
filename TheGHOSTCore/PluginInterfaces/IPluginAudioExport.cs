using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public interface IPluginAudioExport : IPlugin
    {
        /// <summary>
        /// Convert a 16bit 44100hz Wav to the format to be used in the game
        /// </summary>
        /// <param name="sourceFilename">Filename and path of input file</param>
        /// <param name="destinationPath">Filename and path of output file - without file extension</param>
        /// <returns>Converted file destinationFilename with file extension</returns>
        /// <exception cref="ApplicationException">Raised exception on error.</exception>
        string Convert(string sourceFilename, string destinationFilename, bool forceMono, int forceDownSample);
    }
}
