using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class PreviewSettingsChangedEventArgs
    {
        /// <summary>
        /// Indicates that a notes file has been added or removed
        /// </summary>
        /// <param name="notesFile">Notes File</param>
        /// <param name="addedRemoved">true=Added, false=Removed</param>
        internal PreviewSettingsChangedEventArgs(int previewStart, int previewLength, int previewFadeLength, int previewVolume, bool previewIncludeGuitar, bool previewIncludeRhythm)
        {
            this.PreviewStart = previewStart;
            this.PreviewLength = previewLength;
            this.PreviewFadeLength = previewFadeLength;
            this.PreviewVolume = previewVolume;
            this.PreviewIncludeGuitar = previewIncludeGuitar;
            this.PreviewIncludeRhythm = previewIncludeRhythm;
        }

        public readonly int PreviewStart;
        public readonly int PreviewLength;
        public readonly int PreviewFadeLength;
        public readonly int PreviewVolume;
        public readonly bool PreviewIncludeGuitar;
        public readonly bool PreviewIncludeRhythm;
    }
}
