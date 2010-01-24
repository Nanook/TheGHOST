using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nanook.TheGhost
{
    public class NotesFile : ISettingsChange
    {
        internal NotesFile(INotesParser np, string filename, int songLength, bool gh3SustainClipping)
        {
            _lastChanged = DateTime.MinValue;
            _recordChange = false;

            this.NonNoteSyncOffset = 0;

            _filename = filename;

            _items = new List<NotesFileItem>();

            if (File.Exists(_filename))
            {
                np.Parse(_filename);

                _np = np;

                foreach (string s in np.HeaderNames())
                    _items.Add(new NotesFileItem(s, np.GetNotes(s), np.GetStarPower(s), new int[0], np.GetFaceOffP1(s), np.GetFaceOffP2(s), np.GetSustainTrigger()));

                _frets = np.GetFretBars(songLength);
                _markers = np.GetNotesMarkers();
                _timeSig = np.GetTimeSig();
            }
            else
            {
                _frets = new int[0];
                _markers = new NotesMarker[0];
                _timeSig = new int[0];
            }
        }

        public int[] TimeSig
        {
            get { return _timeSig; }
        }


        public List<NotesFileItem> Items
        {
            get { return _items; }
        }

        public INotesParser Parser
        {
            get { return _np; }
        }

        public string Filename
        {
            get { return _filename; }
            internal set { _filename = value; }
        }

        public int[] Frets
        {
            get { return _frets; }
        }

        public NotesMarker[] Markers
        {
            get { return _markers; }
        }

        public NotesFileItem FindItem(uint uniqueId)
        {
            foreach (NotesFileItem nfi in _items)
            {
                if (nfi.UniqueId == uniqueId)
                    return nfi;
            }
            return null;
        }

        public int NonNoteSyncOffset
        {
            get { return _nonNoteSyncOffset; }
            set
            {
                if (_nonNoteSyncOffset == value)
                    return;
                _nonNoteSyncOffset = value;
                this.LastChanged = DateTime.Now;
            }
        }


        #region ISettingsChange Members

        public DateTime LastChanged
        {
            get
            {
                DateTime dt = _lastChanged;
                foreach (NotesFileItem nfi in _items)
                {
                    if (dt > nfi.LastChanged)
                        dt = nfi.LastChanged;
                }
                return dt;
            }

            set
            {
                if (_recordChange)
                    _lastChanged = value;
            }
        }

        void ISettingsChange.RecordChange()
        {
            foreach (NotesFileItem nfi in _items)
                    ((ISettingsChange)nfi).RecordChange();
            _recordChange = true;
        }

        #endregion

        private int _nonNoteSyncOffset;
        private NotesMarker[] _markers;
        private int[] _frets;
        private int[] _timeSig;
        private List<NotesFileItem> _items;
        private INotesParser _np;
        private string _filename;
        private bool _recordChange;
        private DateTime _lastChanged;


    }
}
