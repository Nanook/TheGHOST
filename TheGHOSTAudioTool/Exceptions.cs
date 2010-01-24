using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost.AudioTool
{

    public class OggNotInstalledException : Exception
    {
        public OggNotInstalledException() { }
        public OggNotInstalledException(string message) : base(message) { }
        public OggNotInstalledException(string message, Exception inner) : base(message, inner) { }
    }

    public class OggPlaybackException : Exception
    {
        public OggPlaybackException() { }
        public OggPlaybackException(string message) : base(message) { }
        public OggPlaybackException(string message, Exception inner) : base(message, inner) { }
    }

    public class CantPlayMp3Exception : Exception
    {
        public CantPlayMp3Exception() { }
        public CantPlayMp3Exception(string message) : base(message) { }
        public CantPlayMp3Exception(string message, Exception inner) : base(message, inner) { }
    }

    public class MissingCodecException : Exception
    {
        public MissingCodecException() { }
        public MissingCodecException(string message) : base(message) { }
        public MissingCodecException(string message, Exception inner) : base(message, inner) { }
    }

    public class CantConvertMP3Exception : Exception
    {
        public CantConvertMP3Exception() { }
        public CantConvertMP3Exception(string message) : base(message) { }
        public CantConvertMP3Exception(string message, Exception inner) : base(message, inner) { }
    }

    public class XbadPcmConvertException : Exception
    {
        public XbadPcmConvertException() { }
        public XbadPcmConvertException(string message) : base(message) { }
        public XbadPcmConvertException(string message, Exception inner) : base(message, inner) { }
    }
}
