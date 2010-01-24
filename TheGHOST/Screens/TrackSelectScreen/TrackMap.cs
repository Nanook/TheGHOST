using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nanook.TheGhost
{
    internal class TrackMap
    {
        public TrackMap(SongQb songQb, ProjectTierSong tierSong)
        {
            this.SongQb = songQb;
            this.TierSong = tierSong;
            this.MappedDir = null;
        }

        public SongQb SongQb { get; set; }
        public DirectoryInfo MappedDir { get; set; }
        public ProjectTierSong TierSong { get; set; }
    }
}
