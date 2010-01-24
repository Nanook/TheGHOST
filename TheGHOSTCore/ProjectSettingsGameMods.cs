using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nanook.TheGhost
{
    public class ProjectSettingsGameMods : ISettingsChange
    {
        internal ProjectSettingsGameMods(Project project)
        {
            _recordChange = false;
            _lastChanged = DateTime.MinValue;

            _tierNames = new List<string>();
            _project = project;

            _unlockSetlists = false;
            _completeTier1Song = false;
            _addNonCareerTracksToBonus = false;
            _setCheats = false;
            _freeStore = false;
            _defaultBonusSongInfo = false;
            _tiersTested = false;
        }

        public bool UnlockSetlists
        {
            get { return _unlockSetlists; }
            set
            {
                if (_unlockSetlists == value)
                    return;
                _unlockSetlists = value;
                this.LastChanged = DateTime.Now;
            }
        }

        public bool CompleteTier1Song
        {
            get { return _completeTier1Song; }
            set
            {
                if (_completeTier1Song == value)
                    return;
                _completeTier1Song = value;
                this.LastChanged = DateTime.Now;
            }
        }

        public bool AddNonCareerTracksToBonus
        {
            get { return _addNonCareerTracksToBonus; }
            set
            {
                if (_addNonCareerTracksToBonus == value)
                    return;
                _addNonCareerTracksToBonus = value;
                this.LastChanged = DateTime.Now;
            }
        }
        public bool SetCheats
        {
            get { return _setCheats; }
            set
            {
                if (_setCheats == value)
                    return;
                _setCheats = value;
                this.LastChanged = DateTime.Now;
            }
        }
        public bool FreeStore
        {
            get { return _freeStore; }
            set
            {
                if (_freeStore == value)
                    return;
                _freeStore = value;
                this.LastChanged = DateTime.Now;
            }
        }
        public bool DefaultBonusSongArt
        {
            get { return _defaultBonusSongArt; }
            set
            {
                if (_defaultBonusSongArt == value)
                    return;
                _defaultBonusSongArt = value;
                this.LastChanged = DateTime.Now;
            }
        }

        public bool DefaultBonusSongInfo
        {
            get { return _defaultBonusSongInfo; }
            set
            {
                if (_defaultBonusSongInfo == value)
                    return;
                _defaultBonusSongInfo = value;
                this.LastChanged = DateTime.Now;
            }
        }

        public string DefaultBonusSongInfoText
        {
            get { return _defaultBonusSongInfoText; }
            set
            {
                if (_defaultBonusSongInfoText == value)
                    return;
                _defaultBonusSongInfoText = value;
                this.LastChanged = DateTime.Now;
            }
        }


        public string[] TierNames
        {
            get
            {

                if (!_tiersTested)
                {
                    List<ProjectTier> pt = _project.Tiers;

                    int c = 0;
                    foreach (ProjectTier p in pt)
                    {
                        if (p.Type == TierType.Career)
                            c++;
                    }

                    List<string> s = new List<string>();

                    int i = 0;
                    while (i < c)
                    {
                        if (i < _tierNames.Count)
                            s.Add(_tierNames[i]);
                        else if (pt[i].Type == TierType.Career)
                            s.Add(pt[i].Name);
                        i++;
                    }

                    _tierNames = s;

                    _tiersTested = true;
                }

                return _tierNames.ToArray();

            }
        }

        public void SetTierName(int index, string name)
        {
            while (index > _tierNames.Count - 1)
                _tierNames.Add(string.Empty);

            if (_tierNames[index] == name)
                return;

            _tierNames[index] = name;
            this.LastChanged = DateTime.Now;
        }


        #region ISettingsChange Members

        public DateTime LastChanged
        {
            get
            {
                return _lastChanged;
            }
            set
            {
                if (_recordChange)
                    _lastChanged = value;
            }
        }

        void ISettingsChange.RecordChange()
        {
            _recordChange = true;
        }

        #endregion

        public DateTime LastApplied
        {
            get { return _lastApplied; }
            set
            {
                if (value < DateTime.Now)
                    _lastApplied = value;
                else
                    _lastApplied = DateTime.Now;
            }
        }



        private bool _recordChange;
        private DateTime _lastChanged;
        private DateTime _lastApplied;

        private List<string> _tierNames;

        private Project _project;

        private bool _unlockSetlists;
        private bool _completeTier1Song;
        private bool _addNonCareerTracksToBonus;
        private bool _setCheats;
        private bool _freeStore;
        private bool _defaultBonusSongArt;
        private bool _defaultBonusSongInfo;
        private string _defaultBonusSongInfoText;
        private bool _tiersTested;
    
    }
}
