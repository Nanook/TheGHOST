using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public delegate void WavStatusChangedEventHandler(object source, WavStatusChangedEventArgs e);

    public class AudioPlayer : IDisposable
    {
        public event WavStatusChangedEventHandler WavStatusChanged;

        public AudioPlayer(params string[] filenames)
        {
            _streams = new List<BinaryEndianReader>();
            _filenames = new List<string>();
            _headers = new List<WavSingleChunkHeader>();
            _maxLength = 0;

            FileStream fs;
            foreach (string fn in filenames)
            {
                if (File.Exists(fn))
                {
                    try
                    {
                        fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                        _headers.Add(WavProcessor.ParseWavSingleChunkHeader(fs));
                        _streams.Add(new BinaryEndianReader(fs));
                        _filenames.Add(fn);
                    }
                    catch
                    {
                        throw;
                    }
                }

            }

            _outHeader = null;
            for (int i = 0; i < _headers.Count; i++)
            {
                if (_headers[i].AudioLength > _maxLength)
                    _maxLength = _headers[i].AudioLength;

                if (_outHeader == null ||
                      (_headers[i].Channels > _outHeader.Channels ||
                          (_headers[i].Channels == _outHeader.Channels && _headers[i].BitsPerSample > _outHeader.BitsPerSample)))
                    _outHeader = _headers[i];
            }

            if (_outHeader != null)
            {
                _player = new WaveOutPlayer(-1, _outHeader, 16384, 3, new BufferFillEventHandler(bufferFiller));
                //_player = new WaveOutPlayer(-1, _outHeader, 2000 * _outHeader.BlockAlign, 2, new BufferFillEventHandler(bufferFiller));
            }
            
            _paused = true;
            _playing = false;
            _lastBufferPaused = true;
            this.OnWavStatusChanged(WavStatus.Initialised);

        }

        /// <summary>
        /// Play wavs combined together
        /// </summary>
        /// <param name="msStart"></param>
        /// <param name="pauseAtEnd">true=pause at end, false=stop</param>
        public void Play(int msStart, bool pauseAtEnd)
        {
            _pauseAtEnd = pauseAtEnd;
            for (int i = 0; i < _headers.Count; i++)
            {
                long pos = (long)(_headers[i].SamplesPerSec * _headers[i].BlockAlign * ((float)msStart / 1000F));

                if (pos % _headers[i].BlockAlign != 0)
                    pos += _headers[i].BlockAlign - (pos % _headers[i].BlockAlign); //align to block


                _streams[i].BaseStream.Seek(_headers[i].DataOffset + pos, SeekOrigin.Begin);
            }

            foreach (WavSingleChunkHeader wh in _headers)
            {
                if (wh.AudioLength > _maxLength)
                    _maxLength = wh.AudioLength;
            }

            this.OnWavStatusChanged(WavStatus.Playing);

            _playing = true;
            _paused = false;

            while (!this.AudioStarted) ;
        }

        protected void OnWavStatusChanged(WavStatus status)
        {
            if (WavStatusChanged != null)
                WavStatusChanged(this, new WavStatusChangedEventArgs(status, _filenames.ToArray()));
        }

        public void Pause()
        {
            _paused = true;
            _startOutput = 0;
            _totalOutput = 0;
            if (_player != null)
                _player.Reset();
            this.OnWavStatusChanged(WavStatus.Resumed);
        }

        public void Resume()
        {
            _paused = false;
            this.OnWavStatusChanged(WavStatus.Resumed);

            while (!this.AudioStarted) ;
        }

        public void Stop()
        {
            _startOutput = 0;
            _totalOutput = 0;
            this.Dispose();
            this.OnWavStatusChanged(WavStatus.Stopped);
        }

        public long Position
        {
            get
            {
                if (_player == null)
                    return 0;
                return _player.Position;
            }
        }

        public bool AudioStarted
        {
            get
            {
                if (_paused || _startOutput == 0)
                    return true;
                return _player.Position > _startOutput;
            }
        }

        public int PlayedMs
        {
            get
            {
                if (_player == null || _paused || _startOutput == 0)
                    return 0;

                return (int)((_player.Position - _startOutput) * 1000 / _outHeader.AvgBytesPerSec);
            }
        }

        public int StartedMs
        {
            get
            {
                if (_paused || _startOutput == 0)
                    return 0;

                return (int)(_startOutput) / _outHeader.BlockAlign;
            }
        }

        /// <summary>
        /// Fill the buffer, 16 bit only. Stereo and Mono supported (runs on background thread)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        private void bufferFiller(IntPtr data, int size)
        {

            byte[] buff = new byte[size];
            short b16;
            BinaryEndianWriter bw = new BinaryEndianWriter(new MemoryStream(buff));

            short allLeft = 0;
            short allRight = 0;

            bool eof = true;

            if (!_paused && _streams.Count != 0)
            {
                if (_lastBufferPaused)
                    _startOutput = _totalOutput; //this byte is where our audio will be placed in the stream

                long[] streamPos = new long[_streams.Count]; //fast way to track pos in stream
                for (int c = 0; c < _streams.Count; c++)
                    streamPos[c] = _streams[c].BaseStream.Position;
                long[] streamLen = new long[_streams.Count]; //fast way to get length of stream
                for (int c = 0; c < _streams.Count; c++)
                    streamLen[c] = _streams[c].BaseStream.Length;

                for (int i = 0; i < size / _outHeader.BlockAlign; i++)
                {
                    allLeft = 0;
                    allRight = 0;

                    for (int c = 0; c < _streams.Count; c++)
                    {
                        BinaryEndianReader b = _streams[c];
                        WavSingleChunkHeader w = _headers[c];

                        //Left channel
                        b16 = 0;
                        if (streamPos[c] + 2 < streamLen[c])
                        {
                            b16 = b.ReadInt16(EndianType.Little);
                            streamPos[c] += 2;
                            eof = false;
                        }
                        allLeft += (short)(b16 / _streams.Count);

                        if (_outHeader.Channels > 1)
                        {
                            if (w.Channels == 1)
                                allRight = allLeft;
                            else
                            {
                                //Right channel
                                b16 = 0;
                                if (streamPos[c] + 2 < streamLen[c])
                                {
                                    b16 = b.ReadInt16(EndianType.Little);
                                    streamPos[c] += 2;
                                    eof = false;
                                }
                                allRight += (short)(b16 / _streams.Count);
                                //System.Diagnostics.Debug.WriteLine(_streams.Count.ToString());
                            }
                        }
                    }

                    bw.Write(allLeft);
                    if (_outHeader.Channels > 1)
                        bw.Write(allRight);

                    _lastBufferPaused = false;
                }

            }
            else
            {
                for (int i = 0; i < buff.Length; i++)
                    buff[i] = 0;
                _lastBufferPaused = true;
            }

            if (eof && !_pauseAtEnd && _playing)
                this.OnWavStatusChanged(WavStatus.End);

            _totalOutput += buff.Length;
            
            System.Runtime.InteropServices.Marshal.Copy(buff, 0, data, size);

        }

        public int Length
        {
            get { return _maxLength; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_player != null)
            {
                _player.Dispose();
                _player = null;
            }

            if (_streams != null)
            {
                for (int i = 0; i < _streams.Count; i++)
                {
                    if (_streams[i] != null)
                    {
                        _streams[i].Close();
                        _streams[i] = null;
                    }
                }
            }
        }

        #endregion


        private List<WavSingleChunkHeader> _headers;
        private List<BinaryEndianReader> _streams;
        private List<string> _filenames;

        private WaveOutPlayer _player;

        private int _maxLength;
        private WavSingleChunkHeader _outHeader;

        private bool _paused;
        private bool _pauseAtEnd;
        private bool _playing;

        private bool _lastBufferPaused;

        private long _totalOutput;
        private long _startOutput;

    }
}
