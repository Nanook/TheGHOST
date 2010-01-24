using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public enum WavStatus
    {
        Initialised,
        Playing,
        Paused,
        Resumed,
        End,
        Stopped
    }

    public class WavStatusChangedEventArgs : EventArgs
    {
        public WavStatusChangedEventArgs(WavStatus status, params string[] wavFilenames)
        {
            this.Status = status;
            this.WavFilenames = wavFilenames;
        }

        public readonly WavStatus Status;
        public readonly string[] WavFilenames;
    }
}
