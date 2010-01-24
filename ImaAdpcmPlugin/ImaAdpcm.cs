using System;
using System.Collections.Generic;
using System.Text;
using Nanook.TheGhost;
using System.IO;
using System.Diagnostics;

namespace Nanook.TheGhost.Plugins
{
    /// <summary>
    /// Provides methods for converting audio between formats
    /// </summary>
    public class ImaAdpcm : IPluginAudioExport
    {
        public ImaAdpcm()
        {
        }

        #region IPluginAudioExport Members

        string IPluginAudioExport.Convert(string sourceFilename, string destinationFilename, bool forceMono, int forceDownSample)
        {
            WavSingleChunkHeader ws;
            using (FileStream fs = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read))
            {
                ws = WavProcessor.ParseWavSingleChunkHeader(fs);
            }

            if (forceDownSample != 0 && forceDownSample < ws.SamplesPerSec) // && wh.SamplesPerSec != 32000)
            {
                DateTime dt = DateTime.Now;
                string currPath = ((FileInfo)new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).DirectoryName;
                string tmp = string.Format("{0}_rs", destinationFilename);

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = string.Format(@"""{0}\ssrc.exe""", currPath);
                psi.Arguments = string.Format(@" --quiet --rate {2} --profile fast ""{0}"" ""{1}""", sourceFilename, tmp, forceDownSample.ToString());
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(psi).WaitForExit();
                if (File.Exists(tmp))
                    FileHelper.Move(tmp, sourceFilename);
            }


            Ima ima = new Ima();
            ima.Encode(sourceFilename, destinationFilename, forceMono);

            return destinationFilename;

        }

        #endregion

        #region IPlugin Members

        string IPlugin.Title()
        {
            return "IMA ADPCM Plugin";
        }

        string IPlugin.Description()
        {
            return "Export audio from TheGHOST to IMA ADPCM format for the Wii. Very fast and no setup required.";
        }

        float IPlugin.Version()
        {
            return 0.1F;
        }

        #endregion

    }
}
