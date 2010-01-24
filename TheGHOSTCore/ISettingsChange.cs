using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    interface ISettingsChange
    {
        /// <summary>
        /// Used to record the date that state to be save in a config files changed
        /// </summary>
        DateTime LastChanged { get; set; }

        /// <summary>
        /// This is called to tell the classes that they should start allowing LastChange to be called
        /// When called, all sub items with this interface should be called also
        /// </summary>
        void RecordChange();

    }
}
