// C# IMA ADPCM decoder
// This source code is in the public domain.
// usage: new SoundPlayer(new IMA_ADPCM("imaadpcm.wav")).Play();

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Nanook.TheGhost.Plugins
{
    internal enum WAVE_FORMAT
    {
        UNKNOWN,
        PCM,
        ADPCM,
        IMA_ADPCM = 0x11,
        XBADPCM = 0x69  //This is not 1:1 XBADPCM as it has different StepTable (I don't know what it is though)
    }

    internal class Ima
    {

        private int readHeader(Stream stream, bool forDecode, bool forceMonoEncode)
        {
            bool xbadpcm = false; //for debugging purposes
            int riffLen = 0;

            if (ReadID(stream) != "RIFF")
                throw new ApplicationException("Invalid RIFF header");
            riffLen = ReadInt32(stream);
            if (ReadID(stream) != "WAVE")
                throw new ApplicationException("Wave type is expected");
            int fmtsize = 0;
            _dataSize = 0;
            while (stream.Position < stream.Length)
            {
                switch (ReadID(stream))
                {
                    case "fmt ":
                        fmtsize = ReadInt32(stream);
                        if (forDecode)
                        {
                            if (!xbadpcm)
                            {
                                if (ReadUInt16(stream) != (ushort)WAVE_FORMAT.IMA_ADPCM)
                                    throw new ApplicationException("Not IMA ADPCM");
                            }
                            else
                            {
                                if (ReadUInt16(stream) != (ushort)WAVE_FORMAT.XBADPCM)
                                    throw new ApplicationException("Not XBADPCM");
                            }
                        }
                        else
                        {
                            if (ReadUInt16(stream) != (ushort)WAVE_FORMAT.PCM)
                                throw new ApplicationException("Not PCM");
                        }
                        _inChannels = ReadUInt16(stream);
                        _samplesPerSec = ReadInt32(stream);
                        ReadInt32(stream);
                        _blockAlign = ReadUInt16(stream);
                        if (forDecode)
                        {
                            if (ReadUInt16(stream) != 4)
                                throw new ApplicationException("Not 4-bit format");
                        }
                        else
                        {
                            if (ReadUInt16(stream) != 16)
                                throw new ApplicationException("Not 16-bit format");
                        }
                        ReadBytes(stream, fmtsize - 16);
                        break;
                    case "data":
                        _dataSize = ReadInt32(stream);
                        _offset = (int)stream.Position;
                        stream.Position += _dataSize;
                        break;
                    default:
                        int size = ReadInt32(stream);
                        stream.Position += size;
                        break;
                }
            }
            if (fmtsize == 0)
                throw new ApplicationException("No format information");
            else if (_dataSize == 0)
                throw new ApplicationException("No data");

            int blocks = (int)(_dataSize / _blockAlign);
            int blocklen;
            int datalen;
            int bytesPerSec;

            _outChannels = (forceMonoEncode && !forDecode ? (ushort)1 : _inChannels);

            if (forDecode)
            {
                blocklen = ((_blockAlign - (_inChannels * 4)) * 4) + (_inChannels * 2); //4=bits 2 = 16bit (2 bytes)  - How much to pull from source stream
                datalen = blocks * blocklen;
                bytesPerSec = _samplesPerSec * _outChannels * 2;
            }
            else
            {
                _imaBlockAlign = 36 * _outChannels;
                int imaDataOnly = (_imaBlockAlign - (_outChannels * 4)); //compressed data without header (4 is header per channel)
                datalen = ((_dataSize / (imaDataOnly * 4)) * imaDataOnly) + ((_dataSize / (imaDataOnly * 4)) * (_outChannels * 4)); //(How many uncompressed samples fit in a block) + (how many headers)
                bytesPerSec = (int)(_samplesPerSec * 0.5625 * _outChannels); //crop off any decimal points.  Each channel will shrink by 1 quarter + 4 bytes per block + channel
                _predictedValues = new short[_outChannels];
                _stepIndexes = new int[_outChannels];

            }
            if (_inChannels > _outChannels)
                datalen /= 2;

            _length = datalen + (!forDecode && xbadpcm ? 48 : 44);
           
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(Encoding.UTF8.GetBytes("RIFF"));
            bw.Write(_length - 8);
            bw.Write(Encoding.UTF8.GetBytes("WAVE"));
            bw.Write(Encoding.UTF8.GetBytes("fmt "));
            bw.Write((!forDecode && xbadpcm ? 20 : 16));
            bw.Write((ushort)(forDecode ? WAVE_FORMAT.PCM : (!xbadpcm ? WAVE_FORMAT.IMA_ADPCM : WAVE_FORMAT.XBADPCM))); // FormatTag
            bw.Write(_outChannels);
            bw.Write(_samplesPerSec);
            bw.Write(bytesPerSec); // AvgBytesPerSec
            bw.Write((ushort)(forDecode ? (_outChannels * 2) : _imaBlockAlign)); // BlockAlign
            bw.Write((ushort)(forDecode ? 16 : 4)); // BitsPerSample
            if (!forDecode && xbadpcm)  //for Xbox compatibility
            {
                bw.Write((short)0x2);  //?? 
                bw.Write((short)0x40); //??
            }
            bw.Write(Encoding.UTF8.GetBytes("data"));
            bw.Write(datalen);
            _header = ms.ToArray();
            bw.Close();
            ms.Close();
            return _header.Length;
        }

        public void Encode(string srcPath, string dstPath, bool forceMono)
        {
            using (FileStream s = new FileStream(srcPath, FileMode.Open, FileAccess.Read))
            {
                readHeader(s, false, forceMono);
                using (FileStream fs = new FileStream(dstPath, FileMode.Create))
                {
                    s.Position = _offset; //start of the data
                    fs.Write(_header, 0, _header.Length);
                    byte[] block;
                    while ((block = EncodeBlock(s)) != null)
                        fs.Write(block, 0, block.Length);
                }
            }
            _cache = null;
            _header = null;
            _predictedValues = null;
            _stepIndexes = null;
        }

        public void Decode(string srcPath, string dstPath)
        {
            using (FileStream s = new FileStream(srcPath, FileMode.Open, FileAccess.Read))
            {
                readHeader(s, true, false);
                using (FileStream fs = new FileStream(dstPath, FileMode.Create))
                {
                    fs.Write(_header, 0, _header.Length);
                    int blocks = _dataSize / _blockAlign;
                    for (int i = 0; i < blocks; i++)
                    {
                        byte[] block = DecodeBlock(s, i);
                        fs.Write(block, 0, block.Length);
                    }
                }
            }
            _header = null;
            _predictedValues = null;
            _stepIndexes = null;
        }

        private byte[] EncodeBlock(Stream s)
        {
            if (s.Position >= s.Length)
                return null;

            byte[] outBuff = new byte[_imaBlockAlign];

            int imaBlockAlign = _imaBlockAlign;
            if (_inChannels > _outChannels) //convert to mono
                imaBlockAlign *= 2;

            int ch4 = _inChannels * 4;
            byte[] inBuff = new byte[(imaBlockAlign - ch4) * 4];  //*4 for compression ratio
            if (s.Read(inBuff, 0, inBuff.Length) < inBuff.Length)
                return null; //work like xbadpcm and copy end

            using (MemoryStream ms = new MemoryStream(inBuff))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {

                    if (_inChannels > _outChannels) //convert to mono
                    {
                        ch4 = _outChannels * 4;
                        MemoryStream mso = new MemoryStream(inBuff);
                        BinaryWriter bw = new BinaryWriter(mso);
                        for (int i = 0; i < inBuff.Length / 4; i++)
                            bw.Write((short)(((int)br.ReadInt16() + (int)br.ReadInt16()) / 2));
                        mso.Close();
                        bw.Close();
                        br.BaseStream.Position = 0;
                    }


                    SampleValue[] v = new SampleValue[_outChannels];
                    for (int ch = 0; ch < _outChannels; ch++)
                    {
                        //write last stored.  This is a guess
                        BitConverter.GetBytes(_predictedValues[ch]).CopyTo(outBuff, (ch << 2));
                        outBuff[(ch << 2) + 2] = (byte)_stepIndexes[ch];

                        v[ch] = new SampleValue(_predictedValues[ch], _stepIndexes[ch]);
                    }

                    if (_outChannels == 1) //mono
                    {
                        for (int i = ch4; i < _imaBlockAlign; i++)
                            outBuff[i] = (byte)((v[0].EncodeNext(br.ReadInt16()) & 0xf) | (v[0].EncodeNext(br.ReadInt16()) << 0x4));
                    }
                    else
                    {
                        int opt;
                        for (int i = ch4; i < _imaBlockAlign; i += ch4)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                opt = i + j;
                                for (int ch = 0; ch < _outChannels; ch++)
                                    outBuff[opt + (ch << 2)] = (byte)(v[ch].EncodeNext(br.ReadInt16()) & 0xf);
                                for (int ch = 0; ch < _outChannels; ch++)
                                    outBuff[opt + (ch << 2)] |= (byte)(v[ch].EncodeNext(br.ReadInt16()) << 0x4);
                            }
                        }
                    }

                    //Store last values, Who knows if this is correct?
                    for (int ch = 0; ch < _outChannels; ch++)
                    {
                        _predictedValues[ch] = v[ch].PredictedValue;
                        _stepIndexes[ch] = v[ch].StepIndex;
                    }
                }
            }
            return outBuff;
        }

        private byte[] DecodeBlock(Stream s, int src)
        {
            if (src >= _dataSize / _blockAlign) return null;
            if (_cacheNo == src) return _cache;

            int pos = _offset + (src * _blockAlign); //4 = compression ratio
            if (pos >= s.Length) return null;

            s.Position = pos;
            byte[] data = ReadBytes(s, _blockAlign);

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    SampleValue[] v = new SampleValue[_outChannels];
                    for (int ch = 0; ch < _outChannels; ch++)
                    {
                        v[ch] = new SampleValue(data, ch * 4);
                        //bw.Write(v[ch].PredictedValue);
                    }
                    int ch4 = _outChannels * 4;

                    if (_outChannels == 1) //mono
                    {
                        for (int i = ch4; i < _blockAlign; i++)
                        {
                            bw.Write(v[0].DecodeNext(data[i] & 0xf));
                            bw.Write(v[0].DecodeNext(data[i] >> 4));
                        }
                    }
                    else
                    {
                        for (int i = ch4; i < _blockAlign; i += ch4)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int ch = 0; ch < _outChannels; ch++)
                                    bw.Write(v[ch].DecodeNext(data[i + j + ch * 4] & 0xf));
                                for (int ch = 0; ch < _outChannels; ch++)
                                    bw.Write(v[ch].DecodeNext(data[i + j + ch * 4] >> 4));
                            }
                        }
                    }
                    _cacheNo = src;
                    _cache = ms.ToArray();
                }
            }
            return _cache;
        }

        private struct SampleValue
        {
            public short PredictedValue;
            public int StepIndex;

            public SampleValue(short predictedValue, int stepIndex)
            {
                this.PredictedValue = predictedValue;
                this.StepIndex = stepIndex;
            }

            public SampleValue(byte[] value, int stepIndex)
            {
                PredictedValue = BitConverter.ToInt16(value, stepIndex);
                StepIndex = value[stepIndex + 2];
            }

            private static int[] StepTable = new[]
            {
                7, 8, 9, 10, 11, 12, 13, 14,
                16, 17, 19, 21, 23, 25, 28, 31,
                34, 37, 41, 45, 50, 55, 60, 66,
                73, 80, 88, 97, 107, 118, 130, 143,
                157, 173, 190, 209, 230, 253, 279, 307,
                337, 371, 408, 449, 494, 544, 598, 658,
                724, 796, 876, 963, 1060, 1166, 1282, 1411,
                1552, 1707, 1878, 2066, 2272, 2499, 2749, 3024,
                3327, 3660, 4026, 4428, 4871, 5358, 5894, 6484,
                7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
                15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794,
                32767
            };

            private static int[] IndexTable = new[]
            {
                -1, -1, -1, -1, 2, 4, 6, 8,
                -1, -1, -1, -1, 2, 4, 6, 8
            };

            public byte EncodeNext(int pcm16)
            {
                //if (pcm16 > 7 || pcm16 < -7)
                //    System.Diagnostics.Debug.WriteLine("Hello");

                int predictedValue = PredictedValue;
                int stepIndex = StepIndex;

                int delta = pcm16 - predictedValue;
                uint value;
                if (delta >= 0)
                    value = 0;
                else
                {
                    value = 8;
                    delta = -delta;
                }

                int step = StepTable[stepIndex];
                int diff = step >> 3;
                if (delta > step)
                {
                    value |= 4;
                    delta -= step;
                    diff += step;
                }
                step >>= 1;
                if (delta > step)
                {
                    value |= 2;
                    delta -= step;
                    diff += step;
                }
                step >>= 1;
                if (delta > step)
                {
                    value |= 1;
                    diff += step;
                }

                if ((value & 8) != 0)
                    predictedValue -= diff;
                else
                    predictedValue += diff;
                if (predictedValue < short.MinValue)
                    predictedValue = short.MinValue;
                else if (predictedValue > short.MaxValue)
                    predictedValue = short.MaxValue;
                PredictedValue = (short)predictedValue;

                stepIndex += IndexTable[value & 7];
                if (stepIndex < 0)
                    stepIndex = 0;
                else if (stepIndex > 88)
                    stepIndex = 88;
                StepIndex = stepIndex;

                return (byte)value;
            }

            public short DecodeNext(int adpcm)
            {
                int step = StepTable[StepIndex];
                int diff = ((((adpcm & 7) << 1) + 1) * step) >> 3;

                if ((adpcm & 8) != 0)
                    diff = -diff;
                int predictedValue = ((int)PredictedValue) + diff;
                if (predictedValue > short.MaxValue)
                    predictedValue = short.MaxValue;
                if (predictedValue < short.MinValue)
                    predictedValue = short.MinValue;

                int idx = StepIndex + IndexTable[adpcm];
                if (idx >= StepTable.Length) idx = StepTable.Length - 1;
                if (idx < 0) idx = 0;

                PredictedValue = (short)predictedValue;
                StepIndex = idx;
                return PredictedValue;
            }
        }

        private byte[] ReadBytes(Stream s, int length)
        {
            byte[] ret = new byte[length];
            if (length > 0) s.Read(ret, 0, length);
            return ret;
        }

        private string ReadID(Stream s) { return Encoding.UTF8.GetString(ReadBytes(s, 4), 0, 4); }
        private int ReadInt32(Stream s) { return BitConverter.ToInt32(ReadBytes(s, 4), 0); }
        private uint ReadUInt32(Stream s) { return BitConverter.ToUInt32(ReadBytes(s, 4), 0); }
        private ushort ReadUInt16(Stream s) { return BitConverter.ToUInt16(ReadBytes(s, 2), 0); }
        private short ReadInt16(Stream s) { return BitConverter.ToInt16(ReadBytes(s, 2), 0); }

        private int _length;
        private ushort _inChannels;
        private ushort _outChannels;
        private int _samplesPerSec;
        private ushort _blockAlign;
        private int _offset;
        private int _dataSize;
        private byte[] _header;
        private int _cacheNo = -1;
        private byte[] _cache;

        private int _imaBlockAlign;
        private short[] _predictedValues;
        private int[] _stepIndexes;

    }
}