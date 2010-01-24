using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class AudioFileChangedEventArgs
    {
        /// <summary>
        /// Indicates that a notes file has been added or removed
        /// </summary>
        /// <param name="notesFile">Notes File</param>
        /// <param name="addedRemoved">true=Added, false=Removed</param>
        internal AudioFileChangedEventArgs(string filename, string type, bool addedRemoved)
        {
            this.Filename = filename;
            this.Type = type;
            this.AddedRemoved = addedRemoved;
        }

        /// <summary>
        /// Notes Item
        /// </summary>
        public readonly string Filename;
        /// <summary>
        /// Type = song, guitar, rhythm
        /// </summary>
        public readonly string Type;
        /// <summary>
        /// true=Added, false=Removed
        /// </summary>
        public readonly bool AddedRemoved;
    }
}
