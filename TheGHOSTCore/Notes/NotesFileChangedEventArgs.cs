using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public enum NotesFileChangeType
    {
        Added,
        Removed,
        Reloaded
    }

    public class NotesFileChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates that a notes file has been added or removed
        /// </summary>
        /// <param name="notesFile">Notes File</param>
        /// <param name="addedRemoved">true=Added, false=Removed</param>
        internal NotesFileChangedEventArgs(NotesFile notesFile, NotesFileChangeType changeType)
        {
            this.NotesFile = notesFile;
            this.ChangeType = changeType;
        }

        /// <summary>
        /// Notes Item
        /// </summary>
        public readonly NotesFile NotesFile;
        /// <summary>
        /// true=Added, false=Removed
        /// </summary>
        public readonly NotesFileChangeType ChangeType;
    }
}
