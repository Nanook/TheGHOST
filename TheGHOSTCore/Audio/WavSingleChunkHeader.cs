using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class WavSingleChunkHeader
    {
        public WavSingleChunkHeader()
        {
            this.ExtraBytes = new byte[0];
        }

        public string FileId { get; internal set; } //RIFF
        public uint FileLength { get; internal set; } //-8
        public string RiffType { get; internal set; } //WAVE...

        public string ChunkHeaderId { get; internal set; } //"fmt " ...
        public uint ChunkHeaderLength { get; internal set; } //header length in bytes
        public ushort FormatTag { get; internal set; } //1 Uncompressed PCM, 105 XBADPCM
        public ushort Channels { get; internal set; } //Channel Count
        public uint SamplesPerSec { get; internal set; } //frequency
        public uint AvgBytesPerSec { get; internal set; } //Channels * samples * (BitsPerSample / 8)
        public ushort BlockAlign { get; internal set; } //Channels * (BitsPerSample / 8)
        public ushort BitsPerSample { get; internal set; } //frequency

        public byte[] ExtraBytes;

        public string ChunkId { get; internal set; } //"data" ...
        public uint ChunkLength { get; internal set; } //length of data


        public int DataOffset { get; set; } //byte where audio starts in wav file.

        /// <summary>
        /// Length in ms
        /// </summary>
        public int AudioLength
        {
            get
            {
                return (int)((float)this.ChunkLength / ((float)this.SamplesPerSec / 1000) / (float)this.BlockAlign);

            }
        }
    }
}
