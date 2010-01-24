using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    /// <summary>
    /// Holds the song QbKey and status ProjectSong (if it's been created)
    /// </summary>
    public class ProjectTierSong
    {
        internal ProjectTierSong(SongQb songQb)
        {
            _songQb = songQb;
            _number = 0;
        }

        public bool IsMappingDisabled
        {
            get { return _isMappingDisabled; }
            set { _isMappingDisabled = value; }
        }

        public bool IsEditSong
        {
            get { return _isEditSong; }
            set { _isEditSong = value; }
        }

        public bool IsAddedWithTheGhost
        {
            get { return _isAddedWithTheGhost; }
            internal set { _isAddedWithTheGhost = value; }
        }

        public bool IsBonusSong
        {
            get { return _isBonusSong; }
            internal set { _isBonusSong = value; }
        }

        public ProjectSong Song
        {
            get { return _song; }
            set { _song = value; }
        }

        public ProjectTier Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        public SongQb SongQb
        {
            get { return _songQb; }
            set { _songQb = value; }
        }

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private int _number;
        private ProjectTier _tier;
        private SongQb _songQb;

        private ProjectSong _song;
        private bool _isMappingDisabled;
        private bool _isEditSong;
        private bool _isAddedWithTheGhost;
        private bool _isBonusSong;
    }
}
