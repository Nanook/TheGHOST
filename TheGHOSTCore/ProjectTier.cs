using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public enum TierType { Career, Bonus, NonCareer, None }

    public class ProjectTier
    {
        internal ProjectTier(TierQb tierQb)
        {
            _songs = new List<ProjectTierSong>();
            _type = TierType.None;
            _tierQb = tierQb;
        }

        internal ProjectTier(TierQb tierQb, TierType type, int number) : this(tierQb)
        {
            _type = type;
            _number = number;
        }

        internal void Save(Dictionary<uint, SongQb> songQbs)
        {
            if (_tierQb == null)
                return;

            foreach (QbKey sqk in _tierQb.Songs)
            {
                if (songQbs.ContainsKey(sqk.Crc))
                {
                    ProjectTierSong pt = new ProjectTierSong(songQbs[sqk.Crc]);
                    _songs.Add(pt);
                    pt.Tier = this;
                }
            }
        }

        public List<ProjectTierSong> Songs
        {
            get { return _songs; }
        }

        public string Name
        {
            get { return (_tierQb == null) ? string.Empty : _tierQb.Name; }
            set
            {
                if (_tierQb == null)
                    return;
                _tierQb.Name = value;
            }
        }

        public TierType Type
        {
            get { return _type; }
            internal set { _type = value; }
        }

        public int Number
        {
            get { return _number; }
            internal set { _number = value; }
        }

        public void InsertSong(ProjectTierSong tierSong, int index)
        {
            if (_tierQb != null)
            {
                List<QbKey> projectSongs = new List<QbKey>(_tierQb.Songs);
                projectSongs.Insert(index, QbKey.Create(tierSong.SongQb.Id.Text)); //clone qbkey
                _tierQb.Songs = projectSongs.ToArray();
            }

            _songs.Insert(index, tierSong);
        }

        public void RemoveSong(ProjectTierSong song)
        {
            if (_tierQb != null)
            {
                List<QbKey> projectSongs = new List<QbKey>(_tierQb.Songs);

                for (int c = 0; c < projectSongs.Count; c++)
                {
                    if (projectSongs[c].Crc == song.SongQb.Id.Crc)
                    {
                        projectSongs.RemoveAt(c);
                        break;
                    }
                }
                _tierQb.Songs = projectSongs.ToArray();
            }

            _songs.Remove(song);
        }

        private int _number;
        private TierType _type;
        private TierQb _tierQb;
        private List<ProjectTierSong> _songs;
    }
}
