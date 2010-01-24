using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Nanook.TheGhost
{
    [Flags]
    public enum SoundFlags : int
    {
        //  flag values for SoundFlags argument on PlaySound
        SND_SYNC = 0x0000,      // play synchronously
        // (default)
        SND_ASYNC = 0x0001,      // play asynchronously
        SND_NODEFAULT = 0x0002,      // silence (!default)
        // if sound not found
        SND_MEMORY = 0x0004,     // pszSound points to
        // a memory file
        SND_LOOP = 0x0008,      // loop the sound until
        // next sndPlaySound
        SND_NOSTOP = 0x0010,      // don't stop any
        // currently playing
        // sound

        SND_NOWAIT = 0x00002000, // don't wait if the
        // driver is busy
        SND_ALIAS = 0x00010000, // name is a Registry
        // alias
        SND_ALIAS_ID = 0x00110000, // alias is a predefined
        // ID
        SND_FILENAME = 0x00020000, // name is file name
        SND_RESOURCE = 0x00040004, // name is resource name
        // or atom
        SND_PURGE = 0x0040,     // purge non-static
        // events for task
        SND_APPLICATION = 0x0080     // look for application-
        // specific association
    }


    public class WavProcessor
    {
        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        private static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf);

        public static WavSingleChunkHeader FixWavHeader(Stream wavStream)
        {
            WavSingleChunkHeader wh = WavProcessor.ParseWavSingleChunkHeader(wavStream);
            //set to whatever is the shortest (filelength or header)
            if ((uint)wavStream.Length - (uint)wh.DataOffset < wh.ChunkLength)
                wh.ChunkLength = (uint)wavStream.Length - (uint)wh.DataOffset;
            wh.ChunkLength -= wh.ChunkLength % wh.BlockAlign;
            wh.FormatTag = 1; //fix issue on vista where FFFE appears for some reason.
            wh.FileLength = (wh.ChunkLength + (uint)wh.DataOffset) - 8;

            //write back corrected header.
            wavStream.Seek(0, SeekOrigin.Begin);
            WavProcessor.WriteSingleChunkHeader(wh, wavStream);
            wavStream.SetLength(wh.FileLength + 8);

            return wh;
        }

        public static WavSingleChunkHeader ParseWavSingleChunkHeader(Stream wavStream)
        {
            WavSingleChunkHeader w = new WavSingleChunkHeader();
            byte[] b;

            BinaryEndianReader r = new BinaryEndianReader(wavStream);

            b = r.ReadBytes(4);
            w.FileId = Encoding.Default.GetString(b);
            w.FileLength = r.ReadUInt32(EndianType.Little);
            b = r.ReadBytes(4);
            w.RiffType = Encoding.Default.GetString(b);

            b = r.ReadBytes(4);
            w.ChunkHeaderId = Encoding.Default.GetString(b);
            w.ChunkHeaderLength = r.ReadUInt32(EndianType.Little);
            long p = wavStream.Position;
            w.FormatTag = r.ReadUInt16(EndianType.Little);
            w.Channels = r.ReadUInt16(EndianType.Little);
            w.SamplesPerSec = r.ReadUInt32(EndianType.Little);
            w.AvgBytesPerSec = r.ReadUInt32(EndianType.Little);
            w.BlockAlign = r.ReadUInt16(EndianType.Little);
            w.BitsPerSample = r.ReadUInt16(EndianType.Little);

            if (wavStream.Position - p != w.ChunkHeaderLength)
                w.ExtraBytes = r.ReadBytes((int)(w.ChunkHeaderLength - (wavStream.Position - p)));

            b = r.ReadBytes(4);
            w.ChunkId = Encoding.Default.GetString(b);
            w.ChunkLength = r.ReadUInt32(EndianType.Little);

            w.DataOffset = (int)wavStream.Position;
            return w;
        }

        /// <summary>
        /// test 10 1k chunk from random parts of the files
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static bool FilesEqual(string fileA, string fileB)
        {
            if (fileA.ToLower() == fileB.ToLower())
                return true;

            int l = 1024;  //meg
            int c;
            int maxPos;
            int pos;
            byte[] a = new byte[l];
            byte[] b = new byte[l];
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            using (FileStream fsA = new FileStream(fileA, FileMode.Open, FileAccess.Read))
            {
                maxPos = (int)(fsA.Length - l);

                using (FileStream fsB = new FileStream(fileB, FileMode.Open, FileAccess.Read))
                {
                    if (fsA.Length != fsB.Length)
                        return false;

                    for (int i = 0; i < 10; i++)
                    {
                        pos = rnd.Next(0, maxPos);
                        fsA.Seek((long)pos, SeekOrigin.Begin);
                        fsB.Seek((long)pos, SeekOrigin.Begin);

                        c = fsA.Read(a, 0, l);
                        fsB.Read(b, 0, l);

                        for (int j = 0; j < c; j++)
                        {
                            if (a[j] != b[j])
                                return false;
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Test for silent wavs
        /// </summary>
        /// <param name="filename">input raw wav</param>
        /// <param name="tolerence">0=0% to 1=1%</param>
        /// <returns></returns>
        public static bool IsWavSilent(string filename, float tolerence)
        {
            long tested = 0;
            long silent = 0;
            int pos;

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                WavSingleChunkHeader h = WavProcessor.ParseWavSingleChunkHeader(fs);

                BinaryEndianReader br = new BinaryEndianReader(fs);

                for (int i = h.DataOffset; i < ((int)h.ChunkLength - 1024); i += (1024 * 1024)) //check at each meg
                {
                    int block = 0;

                    pos = i;
                    fs.Seek((long)pos, SeekOrigin.Begin);
                    while (block != 1024) //length is block aligned so don't worry about channel count
                    {
                        if (br.ReadInt16(EndianType.Little) == 0)
                            silent += 2;
                        tested += 2;
                        block++;
                    }
                }
            }
            if (((double)silent / (double)tested) >= (double)tolerence)
                return true;
            else
                return false;
        }


        public static void WriteSingleChunkHeader(WavSingleChunkHeader header, Stream wavStream)
        {
            BinaryEndianWriter w = new BinaryEndianWriter(wavStream);

            w.Write(Encoding.Default.GetBytes(header.FileId));
            w.Write(header.FileLength);
            w.Write(Encoding.Default.GetBytes(header.RiffType));

            w.Write(Encoding.Default.GetBytes(header.ChunkHeaderId));
            w.Write(header.ChunkHeaderLength);
            w.Write(header.FormatTag);
            w.Write(header.Channels);
            w.Write(header.SamplesPerSec);
            w.Write(header.AvgBytesPerSec);
            w.Write(header.BlockAlign);
            w.Write(header.BitsPerSample);
            w.Write(header.ExtraBytes);
            w.Write(Encoding.Default.GetBytes(header.ChunkId));
            w.Write(header.ChunkLength);
        }

        /// <summary>
        /// Converts all found wav files for the songQbKey passed on contruction to FSB format.
        /// </summary>
        //public void PackageInFsb(params QbKey[] filenames)
        //{
        //    if (AppState.Project.GameInfo.Game == Game.GH3_Wii || AppState.Project.GameInfo.Game == Game.GHA_Wii)
        //    {
        //        string fsLst = string.Format(@"{0}\fsbank.lst", AppState.Project.WorkingPath.TrimEnd('\\'));
        //        string f;

        //        foreach (QbKey fn in filenames)
        //        {
        //            f = string.Format(@"{0}\{1}.wav", AppState.Project.WorkingPath.TrimEnd('\\'), fn.Text);

        //            if (File.Exists(f))
        //            {
        //                string fsbH = string.Format("{0}.h", f);

        //                File.WriteAllText(fsLst, f);

        //                Process p = new Process();
        //                ProcessStartInfo psi = p.StartInfo;
        //                psi.FileName = string.Format(@"{0}\fsbank.exe", _currPath);
        //                psi.Arguments = string.Format(@"-o ""{0}.fsb"" ""{1}"" -p cross -f adpcm -h", f, fsLst);
        //                p.Start();
        //                p.WaitForExit();

        //                //TODO: Test FSB version and header sizes, then AND FSOUND_LOADMEMORYIOP properly
        //                using (FileStream fs = File.OpenWrite(string.Format("{0}.fsb", f)))
        //                {
        //                    fs.Seek(0x4B, SeekOrigin.Begin);
        //                    fs.WriteByte(0x20);  //set FSOUND_LOADMEMORYIOP
        //                }

        //                FileHelper.Delete(fsbH);
        //            }
        //        }

        //        FileHelper.Delete(fsLst);
        //    }
        //    else
        //        throw new ApplicationException(string.Format("'{0}' is not a supported Game type", AppState.Project.GameInfo.Game.ToString()));

        //}

        public void ChangeFsbInternalFilename(string fsbFilename, string internalFilename)
        {
            int len = 30;
            byte[] fn = Encoding.Default.GetBytes(internalFilename.ToLower().PadRight(len, '\0').Substring(0, len));

            using (FileStream fs = File.OpenWrite(fsbFilename))
            {
                fs.Seek(26, SeekOrigin.Begin);
                fs.Write(fn, 0, len);
                fs.Flush();
            }
        }

        public static void PlayWav(string wavFilename, SoundFlags soundFlags)
        {
            byte[] bname = new Byte[2000];    //Max path length
            PlaySound(wavFilename, IntPtr.Zero, soundFlags);
        }

        public static void StopWav()
        {
            PlaySound(null, IntPtr.Zero, SoundFlags.SND_PURGE);
        }

        public static void CombineAudio(float volume, string dstFilename, params AudioFile[] srcFilenames)
        {
            CreatePreview(0, 0, 0, volume, true, dstFilename, srcFilenames);
        }

        public static void CreatePreview(int offset, int length, int fade, float volume, bool volumeApplied, string dstFilename, params AudioFile[] srcFilenames)
        {
            if (srcFilenames == null || srcFilenames.Length == 0 || (srcFilenames.Length == 1 && (srcFilenames[0] == null || srcFilenames[0].Name.Length == 0)))
                return;

            FileHelper.Delete(dstFilename);

            WavSingleChunkHeader[] whi = new WavSingleChunkHeader[srcFilenames.Length];
            BinaryEndianReader[] br = new BinaryEndianReader[srcFilenames.Length];
            float[] vols = new float[srcFilenames.Length];

            int outWav = 0; //use this input wav as the format for the output wav
            int maxLen = 0;

            for (int c = 0; c < srcFilenames.Length; c++)
            {
                br[c] = new BinaryEndianReader(File.OpenRead(srcFilenames[c].Name));
                whi[c] = ParseWavSingleChunkHeader(br[c].BaseStream);
                if (!volumeApplied)
                    vols[c] = srcFilenames[c].Volume / 100F; //get percentage
                else
                    vols[c] = 1; //100%

                //move to the correct point in each wav
                uint wOffset = (uint)(whi[c].AvgBytesPerSec * ((float)offset / 1000));
                wOffset -= wOffset % whi[c].BlockAlign;
                br[c].BaseStream.Seek((long)wOffset, SeekOrigin.Current);

                if (whi[c].AudioLength > maxLen)
                    maxLen = whi[c].AudioLength;

                if (whi[c].Channels == 2)
                    outWav = c;
            }

            WavSingleChunkHeader who = new WavSingleChunkHeader();

            if (length == 0)
                length = maxLen;

            uint wLength = (uint)(whi[outWav].AvgBytesPerSec * ((float)length / 1000));
            wLength -= wLength % whi[outWav].BlockAlign;

            who.FileId = "RIFF";
            who.FileLength = (uint)(br[outWav].BaseStream.Length - whi[outWav].FileLength) + wLength;
            who.RiffType = "WAVE";

            who.ChunkHeaderId = "fmt ";
            who.ChunkHeaderLength = whi[outWav].ChunkHeaderLength;
            who.FormatTag = whi[outWav].FormatTag;
            who.Channels = whi[outWav].Channels;
            who.SamplesPerSec = whi[outWav].SamplesPerSec;
            who.AvgBytesPerSec = whi[outWav].AvgBytesPerSec;
            who.BlockAlign = whi[outWav].BlockAlign;
            who.BitsPerSample = whi[outWav].BitsPerSample;
            who.ExtraBytes = whi[outWav].ExtraBytes;

            who.ChunkId = "data";
            who.ChunkLength = wLength;


            using (FileStream fso = File.OpenWrite(dstFilename))
            {
                WriteSingleChunkHeader(who, fso);

                int loops = (int)((who.AvgBytesPerSec / who.BlockAlign) * ((float)length / 1000));
                int fadeLen = (int)((who.AvgBytesPerSec / who.BlockAlign) * ((float)fade / 1000));

                Double maxVol = (volume / (float)srcFilenames.Length);  //reduce volume Audio in game
                maxVol *= 1F + ((srcFilenames.Length - 1) * 0.33F);  //add an extra third per file to cater for volume loss

                Double vol = 0; //initial volume
                if (fade == 0)
                    vol = maxVol;

                int b32;

                BinaryEndianWriter bw = new BinaryEndianWriter(fso);

                Double step = (double)(maxVol - 0) / fadeLen; //fade in
                if (Double.IsInfinity(step))
                    step = 0;
                int allLeft = 0;
                int allRight = 0;

                long[] streamPos = new long[br.Length]; //fast way to track pos in stream
                for (int c = 0; c < br.Length; c++)
                    streamPos[c] = br[c].BaseStream.Position;
                long[] streamLen = new long[br.Length]; //fast way to get length of stream
                for (int c = 0; c < br.Length; c++)
                    streamLen[c] = br[c].BaseStream.Length;

                for (int i = 0; i != loops; i++)
                {
                    if (i == fadeLen)
                        step = 0; //use max volume
                    if (i == loops - fadeLen) //fade out
                        step = (double)(0 - maxVol) / fadeLen;

                    allLeft = 0;
                    allRight = 0;

                    for (int c = 0; c < br.Length; c++) //each input stream
                    {
                        BinaryEndianReader b = br[c];
                        WavSingleChunkHeader w = whi[c];

                        //Left channel
                        b32 = 0;
                        if (streamPos[c] + 2 < streamLen[c])
                        {
                            b32 = (int)(b.ReadInt16(EndianType.Little) * vols[c]);
                            streamPos[c] += 2;
                        }
                        allLeft += (int)(b32 * vol);
                        allLeft = (short)Math.Max((int)short.MinValue, Math.Min((int)short.MaxValue, allLeft));

                        if (who.Channels > 1)
                        {
                            if (w.Channels > 1)
                            {
                                //Right channel
                                b32 = 0;
                                if (streamPos[c] + 2 < streamLen[c])
                                {
                                    b32 = (int)(b.ReadInt16(EndianType.Little) * vols[c]);
                                    streamPos[c] += 2;
                                }
                            }
                            allRight += (int)(b32 * vol);
                            allRight = (short)Math.Max((int)short.MinValue, Math.Min((int)short.MaxValue, allRight));
                        }
                    }

                    bw.Write((short)allLeft);
                    if (who.Channels > 1)
                        bw.Write((short)allRight);

                    vol += step;
                }


                foreach (BinaryEndianReader b in br)
                    b.Close();

                fso.Flush();
            }
        }

        //public static void DownSample(string src, int sampleRate)
        //{


        //    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
        //    {
        //        using (FileStream fs = new FileStream(@"g:\downsample.wav", FileMode.Create, FileAccess.Write))
        //        {
        //            WavSingleChunkHeader wh = ParseWavSingleChunkHeader(fs);

        //            if (wh.SamplesPerSec <= samplerate)
        //                return;

        //            float block = wh.SamplesPerSec / (wh.SamplesPerSec - sampleRate);

        //        }

        //    }
        //}

        public static void Truncate(string filename, float lengthMs)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                WavSingleChunkHeader wh = ParseWavSingleChunkHeader(fs);

                uint byteLen = (uint)((float)wh.AvgBytesPerSec * (lengthMs / 1000F));
                byteLen -= byteLen % wh.BlockAlign;
                uint headerLen = (uint)fs.Position;

                fs.Seek(0, SeekOrigin.Begin);

                if (fs.Length > headerLen + byteLen)
                {
                    wh.FileLength = (byteLen + headerLen) - 8;
                    wh.ChunkLength = byteLen;

                    WriteSingleChunkHeader(wh, fs);

                    fs.SetLength(headerLen + byteLen);
                }
            }
        }

        public static void Normalize(string filename, bool halfVolume)
        {
            string currPath = ((FileInfo)new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).DirectoryName;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = string.Format(@"""{0}\normalize.exe""", currPath);
            if (halfVolume)
                psi.Arguments = string.Format(@" -a 0.2 -q ""{0}""", filename);
            else
                psi.Arguments = string.Format(@" -q ""{0}""", filename);
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi).WaitForExit();
        }

        /// <summary>
        /// Adds silence to wav files alter volume
        /// </summary>
        /// <param name="silenceInsertLength">Milliseconds to insert at start of wav</param>
        /// <param name="volume">0=silent, 1=100%</param>
        /// <param name="maxLength">If the wav is longer than this then crop (includes silence), if 0 then don't crop</param>
        /// <param name="srcFilenames">Filenames to pad</param>
        public static void SetLengthSilenceAndVolume(float silenceInsertLength, float maxLength, float volume, string fileName)
        {
            FileStream inS;
            FileStream outS;
            WavSingleChunkHeader h;
            uint silenceLen;
            string tmpName;

            //uint origChunkLen;

            int copied;
            byte[] buff = new byte[10000];

            for (int i = 0; i < buff.Length; i++)
                buff[i] = 0;

            tmpName = string.Format("{0}_{1}", fileName, Guid.NewGuid().ToString("N"));

            using (inS = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (outS = new FileStream(tmpName, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    h = ParseWavSingleChunkHeader(inS);

                    //origChunkLen = h.ChunkLength;

                    silenceLen = (uint)((float)h.AvgBytesPerSec * (silenceInsertLength / 1000F));
                    silenceLen -= silenceLen % h.BlockAlign;

                    uint newLenBytes = maxLength != 0 ? (uint)((float)h.AvgBytesPerSec * (maxLength / 1000F)) : h.ChunkLength + silenceLen;
                    newLenBytes -= newLenBytes % h.BlockAlign;

                    h.ChunkLength = (uint)((uint)inS.Length - h.DataOffset) + silenceLen;

                    if (h.ChunkLength > newLenBytes)
                        h.ChunkLength = newLenBytes;

                    h.ChunkLength -= h.ChunkLength % h.BlockAlign;
                    h.FileLength = (h.ChunkLength + (uint)h.DataOffset) - 8;

                    WriteSingleChunkHeader(h, outS);

                    copied = 0;
                    int len = 0;
                    while (copied < silenceLen)
                    {
                        len = buff.Length;
                        if (copied + buff.Length > silenceLen)
                            len = (int)silenceLen - copied;

                        outS.Write(buff, 0, len);
                        copied += len;
                    }

                    //origChunkLen = h.ChunkLength; //we need to copy this much

                    //copy audio and set volume
                    BinaryEndianWriter bw = new BinaryEndianWriter(outS);
                    BinaryEndianReader br = new BinaryEndianReader(inS);
                    int b32;
                    while (copied != h.ChunkLength) //length is block aligned so don't worry about channel count
                    {
                        b32 = (int)br.ReadInt16(EndianType.Little);
                        b32 = (int)((float)b32 * volume);

                        b32 = Math.Max((int)short.MinValue, Math.Min((int)short.MaxValue, b32));

                        bw.Write((short)b32, EndianType.Little);
                        copied += 2;
                    }

                    //copied = (int)origChunkLen;  //we've copied this much (silence included)
                    //while (copied < h.ChunkLength) //pad to the length we said it was in the header
                    //{
                    //    inS.Read(buff, 0, buff.Length);
                    //    if (copied + buff.Length < h.ChunkLength)
                    //        outS.Write(buff, 0, buff.Length);
                    //    else
                    //        outS.Write(buff, 0, (int)h.ChunkLength % - copied);
                    //    copied += buff.Length;
                    //}

                    outS.Flush();
                }
            }
            if (File.Exists(fileName))
            {
                if (File.Exists(tmpName))
                    FileHelper.Move(tmpName, fileName);
            }
        }



        public static void CreateSilentWav(int length, string dstFilename, params string[] srcFilenames)
        {
            if (srcFilenames == null || (srcFilenames.Length == 1 && srcFilenames[0].Length == 0))
                return;

            FileHelper.Delete(dstFilename);

            WavSingleChunkHeader[] whi = new WavSingleChunkHeader[srcFilenames.Length];
            BinaryEndianReader[] br = new BinaryEndianReader[srcFilenames.Length];

            int outWav = 0; //use this input wav as the format for the output wav

            for (int c = 0; c < srcFilenames.Length; c++)
            {
                br[c] = new BinaryEndianReader(File.OpenRead(srcFilenames[c]));
                whi[c] = ParseWavSingleChunkHeader(br[c].BaseStream);

                if (whi[c].Channels == 2)
                    outWav = c;
            }


            WavSingleChunkHeader who = new WavSingleChunkHeader();

            uint wLength = (uint)((float)whi[outWav].AvgBytesPerSec * (float)(length / 1000F));
            wLength -= wLength % whi[outWav].BlockAlign;

            who.FileId = "RIFF";
            who.FileLength = (uint)(whi[outWav].DataOffset + wLength) - 8;
            who.RiffType = "WAVE";

            who.ChunkHeaderId = "fmt ";
            who.ChunkHeaderLength = whi[outWav].ChunkHeaderLength;
            who.FormatTag = whi[outWav].FormatTag;
            who.Channels = whi[outWav].Channels;
            who.SamplesPerSec = whi[outWav].SamplesPerSec;
            who.AvgBytesPerSec = whi[outWav].AvgBytesPerSec;
            who.BlockAlign = whi[outWav].BlockAlign;
            who.BitsPerSample = whi[outWav].BitsPerSample;
            who.ExtraBytes = whi[outWav].ExtraBytes;

            who.ChunkId = "data";
            who.ChunkLength = wLength;

            foreach (BinaryEndianReader b in br)
                b.Close();


            using (FileStream fso = File.OpenWrite(dstFilename))
            {
                WriteSingleChunkHeader(who, fso);

                byte[] buffer = new byte[who.AvgBytesPerSec];
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = 0;
                long written = 0;

                while (written + buffer.Length < wLength)
                {
                    fso.Write(buffer, 0, buffer.Length);
                    written += buffer.Length;
                }

                if (written != wLength)
                    fso.Write(buffer, 0, (int)(wLength - written));

                fso.Flush();
            }
        }

        public static void CreateSilentWav(int length, string dstFilename, bool stereo, int samplesPerSec)
        {

            FileHelper.Delete(dstFilename);

            ushort bits = 16;
            ushort channels = (ushort)(stereo ? 2 : 1);
            ushort blockAlign = (ushort)(channels * 2);

            WavSingleChunkHeader who = new WavSingleChunkHeader();

            uint wLength = (uint)((float)(blockAlign * samplesPerSec) * (float)(length / 1000F));
            wLength -= wLength % blockAlign;

            who.FileId = "RIFF";
            who.FileLength = (uint)(wLength + 46) - 8;
            who.RiffType = "WAVE";

            who.ChunkHeaderId = "fmt ";
            who.ChunkHeaderLength = 18;
            who.FormatTag = 1; //raw wav
            who.Channels = channels;
            who.SamplesPerSec = (uint)samplesPerSec;
            who.AvgBytesPerSec = (uint)(blockAlign * samplesPerSec);
            who.BlockAlign = blockAlign;
            who.BitsPerSample = bits;
            who.ExtraBytes = new byte[2];

            who.ChunkId = "data";
            who.ChunkLength = wLength;


            using (FileStream fso = File.OpenWrite(dstFilename))
            {
                WriteSingleChunkHeader(who, fso);

                byte[] buffer = new byte[who.AvgBytesPerSec];
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = 0;
                long written = 0;

                while (written + buffer.Length < wLength)
                {
                    fso.Write(buffer, 0, buffer.Length);
                    written += buffer.Length;
                }

                if (written != wLength)
                    fso.Write(buffer, 0, (int)(wLength - written));

                fso.Flush();
            }
        }
        
    }


}
