using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class GhItemMapChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates that a notes file has been added or removed
        /// </summary>
        /// <param name="item">GH notes item</param>
        /// <param name="addedRemoved">true=Added, false=Removed</param>
        internal GhItemMapChangedEventArgs(NotesFile fromFile, uint uniqueId, GhNotesItem toItem, bool isGenerated, bool addedRemoved)
        {
            this.ToGhNotesItem = toItem;
            this.FromFile = fromFile;
            this.UniqueId = uniqueId;
            this.IsGenerated = isGenerated;
            this.AddedRemoved = addedRemoved;
        }

        /// <summary>
        /// GH notes item
        /// </summary>
        public readonly GhNotesItem ToGhNotesItem;

        public readonly NotesFile FromFile;
        public readonly uint UniqueId;
        public readonly bool IsGenerated;

        /// <summary>
        /// true=Added, false=Removed
        /// </summary>
        public readonly bool AddedRemoved;
     }
}
