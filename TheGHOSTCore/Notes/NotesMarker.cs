using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class NotesMarker
    {
        internal NotesMarker()
            : this("", 0)
        {
        }

        internal NotesMarker(string title, int offset)
        {
            this.Title = title;
            this.Offset = offset;
        }

        public string Title { get; set; }
        public int Offset { get; set; }
    }
}
