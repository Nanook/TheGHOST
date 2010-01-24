using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class DatWad
    {
        /// <summary>
        /// Open existing DAT/WAD files
        /// </summary>
        /// <param name="datFilename"></param>
        /// <param name="wadFilename"></param>
        /// <param name="endianType"></param>
        public DatWad(string datFilename, string wadFilename, EndianType endianType)
        {
            _datFilename = datFilename;
            _wadFilename = wadFilename;
            _endianType = endianType;

            _datItems = new Dictionary<uint, DatItem>();
            _headerFileSize = 0;
            this.parse();
        }

        public DatItem[] GetBackgroundAudioItems()
        {
            List<DatItem> d = new List<DatItem>();

            string name;
            foreach (DatItem di in this.DatItems.Values)
            {
                name = this.ReadInternalFileName(di);
                if (name.StartsWith("men"))
                    d.Add(di);
            }
            return d.ToArray();

        }

        public string DatFilename
        {
            get { return _datFilename; }
        }

        public string WadFilename
        {
            get { return _wadFilename; }
        }

        public Dictionary<uint, DatItem> DatItems
        {
            get { return _datItems; }
        }

        public static int FileAlignment
        {
            get { return 0x4000; }
        }

        public static byte FileAlignmentPadValue
        {
            get { return 0; }
        }

        private static void copy(Stream from, Stream to, long length)
        {
            long l = 0;
            long size = 10000;
            int c = -1;
            byte[] b = new byte[size];
            while (c != 0 && l < length)
            {
                c = from.Read(b, 0, (int)(l + size < length ? b.Length : length - l));
                l += c;
                to.Write(b, 0, c);
            }

            //pad stream, if input stream was too short
            if (l < length)
            {
                for (int i = 0; i < size; i++)
                    b[i] = 0;

                while (l < length)
                {
                    if (length - l > size)
                        to.Write(b, 0, (int)size);
                    else
                        to.Write(b, 0, (int)(length - l));

                    l += size;
                }
            }
        }

        public static void CreateDatWad(QbKey songQk, EndianType endianType, string datFilename, string wadFilename, string songFilename, string guitarFilename, string rhythmFilename, string previewFilename)
        {
            QbKey[] keys = new QbKey[4];
            int[] offsets = new int[5];
            int[] sizes = new int[4];

            FileHelper.Delete(wadFilename);
            FileHelper.Delete(datFilename);

            using (FileStream fsWad = File.OpenWrite(wadFilename))
            {
                offsets[0] = (int)fsWad.Position;
                keys[0] = QbKey.Create(string.Format("{0}_guitar", songQk.Text));
                sizes[0] = (int)writeFsbToStream(fsWad, guitarFilename, string.Concat(keys[0].Text, ".wav"));
                offsets[1] = (int)fsWad.Position;
                keys[1] = QbKey.Create(string.Format("{0}_preview", songQk.Text));
                sizes[1] = (int)writeFsbToStream(fsWad, previewFilename, string.Concat(keys[1].Text, ".wav"));
                offsets[2] = (int)fsWad.Position;
                keys[2] = QbKey.Create(string.Format("{0}_rhythm", songQk.Text));
                sizes[2] = (int)writeFsbToStream(fsWad, rhythmFilename, string.Concat(keys[2].Text, ".wav"));
                offsets[3] = (int)fsWad.Position;
                keys[3] = QbKey.Create(string.Format("{0}_song", songQk.Text));
                sizes[3] = (int)writeFsbToStream(fsWad, songFilename, string.Concat(keys[3].Text, ".wav"));
                offsets[4] = (int)fsWad.Position;
                fsWad.Flush();
            }


            using (FileStream fsDat = File.OpenWrite(datFilename))
            {
                using (BinaryEndianWriter bw = new BinaryEndianWriter(fsDat))
                {
                    int l = offsets[3] + sizes[3];
                    if (l % 16 != 0)
                        l += 16 - (l % 16);

                    bw.Write((uint)keys.Length, endianType);
                    bw.Write((uint)l, endianType);

                    for (int i = 0; i < offsets.Length - 1; i++)
                    {
                        bw.Write(keys[i].Crc, endianType);
                        bw.Write(offsets[i], endianType);
                        bw.Write(sizes[i], endianType);
                        bw.Write(new byte[] { 0,0,0,0,0,0,0,0 });
                    }
                    fsDat.Flush();
                }
            } 
            
        }

        private static long writeFsbToStream(FileStream fsWrite, string wavFilename, string internalFilename)
        {
            int filenameLen = 30;
            long pos = fsWrite.Position;
            long ret;

            //open XBADPCM file to write as FSB file
            using (FileStream fs = File.OpenRead(wavFilename))
            {
                //not fully correct for XBADPCM 
                WavSingleChunkHeader wh = WavProcessor.ParseWavSingleChunkHeader(fs);

                BinaryEndianWriter bw = new BinaryEndianWriter(fsWrite);

                byte[] fsbFilename = new byte[filenameLen];

                //FileHeader

                bw.Write(Encoding.Default.GetBytes("FSB3"));
                bw.Write((uint)1, EndianType.Little); //1 sample in file
                bw.Write((uint)80, EndianType.Little); //sampleheader length
                bw.Write(wh.ChunkLength, EndianType.Little); //sampleheader length
                bw.Write((uint)0x00030001, EndianType.Little); //header version 3.1
                bw.Write((uint)0, EndianType.Little); //global mode flags

                //SampleHeader (80 byte version)
                bw.Write((UInt16)80, EndianType.Little); //sampleheader length
                bw.Write(Encoding.Default.GetBytes(internalFilename.ToLower().PadRight(filenameLen, '\0').Substring(0, filenameLen))); //write filename

                //sampleheader length
                uint lenSamp = (uint)Math.Round(((double)wh.SamplesPerSec / (double)wh.AvgBytesPerSec) * wh.ChunkLength);
                if (lenSamp < 0xFA00)
                    lenSamp = 0xFA00; //set smallest allowed size? May be a memory allocation thing for FSB

                bw.Write(lenSamp, EndianType.Little); //sampleheader length
                bw.Write(wh.ChunkLength, EndianType.Little); //compressed bytes
                bw.Write((uint)0x0, EndianType.Little); //loop start
                bw.Write(lenSamp - 1, EndianType.Little); //loop end
                bw.Write((uint)0x20400041, EndianType.Little); //sample mode
                bw.Write(wh.SamplesPerSec, EndianType.Little); //frequency
                bw.Write((ushort)0xFF, EndianType.Little); //default volume
                bw.Write((ushort)0xFFFF, EndianType.Little); //default pan
                bw.Write((ushort)0xFF, EndianType.Little); //default pri
                bw.Write(wh.Channels, EndianType.Little); //channels
                bw.Write((float)1, EndianType.Little); //min distance
                bw.Write((float)1000000, EndianType.Little); //max distance
                bw.Write((uint)0x0, EndianType.Little); //varfreq
                bw.Write((ushort)0x0, EndianType.Little); //varvol
                bw.Write((ushort)0x0, EndianType.Little); //varpan

                copy(fs, fsWrite, wh.ChunkLength);
                uint fileSize = (uint)(24 + 80 + wh.ChunkLength);
                byte[] buff;

                ret = fsWrite.Position - pos;

                if (fileSize % DatWad.FileAlignment != 0)
                {
                    buff = new byte[DatWad.FileAlignment - (fileSize % DatWad.FileAlignment)];
                    for (int i = 0; i < buff.Length; i++)
                        buff[i] = DatWad.FileAlignmentPadValue;
                    fsWrite.Write(buff, 0, buff.Length);
                }
            }

            return ret;
        }

        public string ReadInternalFileName(DatItem item)
        {
            using (FileStream fsi = File.OpenRead(this.WadFilename))
            {
                fsi.Seek(item.FileOffset + 0x1a, SeekOrigin.Begin);
                byte[] b = new byte[30];
                fsi.Read(b, 0, 30);
                return Encoding.Default.GetString(b).TrimEnd('\0');
            }
        }

        public void ReplaceFsbFileWithWav(DatItem item, string wavFilename)
        {
            string tempWadFn = string.Format("{0}_{1}", _wadFilename, Guid.NewGuid().ToString("N"));

            byte[] buff;
            int filenameLen = 30;
            uint fileSize = 0;
            int diff;

            //save WAD to temp file
            using (FileStream fso = File.OpenWrite(tempWadFn))
            {
                using (FileStream fsi = File.OpenRead(_wadFilename))
                {
                    if (item.FileOffset != 0)
                        copy(fsi, fso, item.FileOffset);

                    using (FileStream fs = File.OpenRead(wavFilename))
                    {
                        //not fully correct for XBADPCM 
                        WavSingleChunkHeader wh = WavProcessor.ParseWavSingleChunkHeader(fs);

                        long p = fso.Position;

                        BinaryEndianWriter bw = new BinaryEndianWriter(fso);

                        byte[] fsbFilename = new byte[filenameLen];

                        //http://www.fmod.org/forum/viewtopic.php?t=1551

                        //FileHeader

                        bw.Write(Encoding.Default.GetBytes("FSB3"));
                        bw.Write((uint)1, EndianType.Little); //1 sample in file
                        bw.Write((uint)80, EndianType.Little); //sampleheader length
                        bw.Write(wh.ChunkLength, EndianType.Little); //sampleheader length
                        bw.Write((uint)0x00030001, EndianType.Little); //header version 3.1
                        bw.Write((uint)0, EndianType.Little); //global mode flags

                        //p = fso.Position;

                        //SampleHeader (80 byte version)
                        bw.Write((UInt16)80, EndianType.Little); //sampleheader length
                        fsi.Seek(24 + 2, SeekOrigin.Current); //skip the start of the source fsb file name
                        fsi.Read(fsbFilename, 0, filenameLen);  //read filename from file being replaced
                        bw.Write(fsbFilename); //write filename

                        //sampleheader length (What's this about?)
                        uint lenSamp = (uint)Math.Round(((double)wh.SamplesPerSec / (double)wh.AvgBytesPerSec) * wh.ChunkLength);
                        if (lenSamp < 0xFA00)
                            lenSamp = 0xFA00; //set smallest allowed size? May be a memory allocation thing for FSB

                        bw.Write(lenSamp, EndianType.Little); //sampleheader length
                        bw.Write(wh.ChunkLength, EndianType.Little); //compressed bytes
                        bw.Write((uint)0x0, EndianType.Little); //loop start
                        bw.Write(lenSamp - 1, EndianType.Little); //loop end
                        bw.Write((uint)0x20400040, EndianType.Little); //sample mode
                        bw.Write(wh.SamplesPerSec, EndianType.Little); //frequency
                        bw.Write((ushort)0xFF, EndianType.Little); //default volume
                        bw.Write((ushort)0x0080, EndianType.Little); //default pan
                        bw.Write((ushort)0x0080, EndianType.Little); //default pri
                        bw.Write(wh.Channels, EndianType.Little); //channels
                        bw.Write((float)1, EndianType.Little); //min distance
                        bw.Write((float)10000, EndianType.Little); //max distance
                        bw.Write((uint)0x0, EndianType.Little); //varfreq
                        bw.Write((ushort)0x0, EndianType.Little); //varvol
                        bw.Write((ushort)0x0, EndianType.Little); //varpan

                        copy(fs, fso, wh.ChunkLength);
                        fileSize = (uint)(24 + 80 + wh.ChunkLength);

                        if (fileSize % DatWad.FileAlignment != 0)
                        {
                            buff = new byte[DatWad.FileAlignment - (fileSize % DatWad.FileAlignment)];
                            for (int i = 0; i < buff.Length; i++)
                                buff[i] = DatWad.FileAlignmentPadValue;
                            fso.Write(buff, 0, buff.Length);
                        }

                        //move past the rest of this file being replaced
                        long itemSize = item.FileSize;
                        if (item.FileSize % DatWad.FileAlignment != 0)
                            itemSize += (DatWad.FileAlignment - (item.FileSize % DatWad.FileAlignment));

                        if (itemSize < fsi.Length)
                            fsi.Seek(itemSize - (24 + 2 + filenameLen), SeekOrigin.Current);

                    }

                    diff = (int)(fso.Position - fsi.Position);

                    //copy the rest of the file
                    copy(fsi, fso, fsi.Length - fsi.Position);
                }
                fso.Flush();
            }

            //rename the temp wad file
            FileHelper.Delete(_wadFilename);

            //save dat headers
            //int diff = (int)fileSize - (int)item.FileSize;
            foreach (DatItem di in _datItems.Values)
            {
                if (di == item)
                    di.FileSize = fileSize;
                else
                    di.FileOffset = (uint)((int)di.FileOffset + (di.FileOffset > item.FileOffset ? diff : 0));
            }
            _headerFileSize = (uint)((int)_headerFileSize + diff);
            this.save();


            File.Move(tempWadFn, _wadFilename);
        }

        private void parse()
        {
            _datItems.Clear();

            using (FileStream fs = File.OpenRead(_datFilename))
            {
                using (BinaryEndianReader br = new BinaryEndianReader(fs))
                {
                    uint files = br.ReadUInt32(_endianType);
                    _headerFileSize = br.ReadUInt32(_endianType);
                    QbKey songQk;

                    for (int i = 0; i < files; i++)
                    {
                        songQk = QbKey.Create(br.ReadUInt32(_endianType));
                        _datItems.Add(songQk.Crc, new DatItem(songQk, br.ReadUInt32(_endianType), br.ReadUInt32(_endianType), br.ReadBytes(8)));
                    }
                }
            }
        }

        private void save()
        {
            using (FileStream fs = File.OpenWrite(_datFilename))
            {
                using (BinaryEndianWriter bw = new BinaryEndianWriter(fs))
                {
                    bw.Write((uint)_datItems.Count, _endianType);
                    bw.Write(_headerFileSize, _endianType);

                    foreach (DatItem di in _datItems.Values)
                    {
                        bw.Write(di.ItemQbKey.Crc, _endianType);
                        bw.Write(di.FileOffset, _endianType);
                        bw.Write(di.FileSize, _endianType);
                        bw.Write(di.Reserved);
                    }
                    fs.Flush();
                }
            }
        }

        private Dictionary<uint, DatItem> _datItems;

        private EndianType _endianType;
        private uint _headerFileSize;
        private string _datFilename;
        private string _wadFilename;

    }
}
