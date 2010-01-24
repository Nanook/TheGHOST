using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    internal class ScreenBusyEventArgs : EventArgs
    {
        internal ScreenBusyEventArgs(bool busy)
        {
            this.Busy = busy;
        }

        public readonly bool Busy;
    }
}
