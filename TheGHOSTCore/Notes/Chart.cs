using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    internal class Chart : INotesParser
    {
        internal Chart(/*MidItems midItems*/)
        {
            //_midItems = midItems;
            _chart = new string[0];
            _headerNames = new Dictionary<string, int>();
            _markers = null;
            _resolution = 0;
            _offset = 0;
            _timeSig = new int[0];
            _bpmUnit = new double[0];
            _bpmOffset = new int[0];
            _endEvent = 0;
            _maxNoteParsed = 0;
            _sustainedTrigger = 0; //the length of a note before the sustain kicks in
        }


        #region INotesItems Members

        void INotesParser.Parse(string filename)
        {
            string chart = File.ReadAllText(filename);

            chart = Regex.Replace(chart, "(\n)([\t ] *?)(?=[^\t \n])", "$1"); //remove start of line whitespace 
            chart = Regex.Replace(chart, "([\t ]*?)(?=\r)", ""); //remove end of line whitespace
            chart = Regex.Replace(chart, "[\t ]{2,}", " "); //replace blocks of white space for a single space

            chart = Regex.Replace(chart, "(\r\n){2,}", "\r\n"); //replace multiple line breaks with one

            //save formatted text to lines
            _chart = chart.Replace("\r", "").Split('\n');

            bool headerFound = false;
            string headerName = string.Empty;

            int songPos = -1;
            int syncTrack = -1;
            int events = -1;

            for (int i = 0; i < _chart.Length; i++)
            {
                if (headerFound)
                {
                    if (_chart[i] == "{")
                    {
                        if (string.Compare(headerName, "Song", true) == 0)
                            songPos = i;
                        else if (string.Compare(headerName, "SyncTrack", true) == 0)
                            syncTrack = i;
                        else if (string.Compare(headerName, "Events", true) == 0)
                            events = i;
                        else
                        {
                            if (i + 1 < _chart.Length && _chart[i + 1] != "}")
                                _headerNames.Add(headerName, i);
                        }
                    }
                }

                headerFound = false;

                if (_chart[i].StartsWith("[") && _chart[i].EndsWith("]"))
                {
                    headerName = _chart[i].Substring(1, _chart[i].Length - 2);
                    headerFound = true;
                }
            }

            if (songPos != -1)
                parseSongHeader(songPos);
            if (syncTrack != -1)
                parseSyncTrackHeader(syncTrack);
            if (events != -1)
                parseEventsHeader(events);

            adjustTimeSig();
        }

        string[] INotesParser.HeaderNames()
        {
            string[] h = new string[_headerNames.Count];
            _headerNames.Keys.CopyTo(h, 0);
            return h;
        }

        NotesMarker[] INotesParser.GetNotesMarkers()
        {
            if (_markers == null)
                return new NotesMarker[0];
            else
                return _markers;
        }

        int[] INotesParser.GetNotes(string headerName)
        {
            int i = _headerNames[headerName] + 1;
            List<int> ints = new List<int>();
            string[] sp;

            int last = -1;
            int v;
            int vMs;

            if (_sustainedTrigger == 0)
                ((INotesParser)this).GetSustainTrigger(); //ensure the value is set

            sp = _chart[i++].Split(' ');
            while (sp[0] != "}")
            {
                if (sp[2] == "N" || (sp[2] == "E" && sp[3] == "*"))
                {
                    v = int.Parse(sp[0]);
                    vMs = offsetToMicroseconds(v);

                    if (v != last)
                    {
                        ints.Add(vMs); //time
                        ints.Add(lengthToMicroseconds(v, int.Parse(sp[4]), true));
                        //notes + HOPO
                        ints.Add(1 << int.Parse(sp[3]));

                        vMs += ints[ints.Count - 2]; //add the length to the note pos
                    }
                    else
                    {
                        // HOPO 10008 = E *
                        //add note key press
                        if (sp[2] == "E")
                        {
                            if (sp[3] == "*")
                                ints[ints.Count - 1] |= 1 << 5;
                        }
                        else
                            ints[ints.Count - 1] |= 1 << int.Parse(sp[3]);
                    }

                    if (vMs > _maxNoteParsed)
                        _maxNoteParsed = vMs;

                    last = v;
                }
                sp = _chart[i++].Split(' ');
            }

            return ints.ToArray();
        }

        private List<int> getSpecialNotes(string headerName, string key)
        {
            int i = _headerNames[headerName] + 1;
            List<int> ints = new List<int>();
            string[] sp;

            int v;

            sp = _chart[i++].Split(' ');
            while (sp[0] != "}")
            {
                if (sp[0] != "=") //mid2chart misses the offset when putting in blank notes
                {
                    v = int.Parse(sp[0]);
                    if (sp[2] == "S" && sp[3] == key) //0= face off P1, 1=face off P2, 2=star power
                    {
                        ints.Add(offsetToMicroseconds(v)); //Star power start
                        ints.Add(lengthToMicroseconds(v, int.Parse(sp[4]), false)); //Star power length (end just before the current note)
                        ints.Add(0); //Star power note count
                    }
                    else if (sp[2] == "N" && ints.Count >= 3)
                    {
                        if (offsetToMicroseconds(v) < ints[ints.Count - 3] + ints[ints.Count - 2])
                            ints[ints.Count - 1]++; //Star power note count
                    }
                }
                sp = _chart[i++].Split(' ');
            }

            return ints;
        }


        int[] INotesParser.GetStarPower(string headerName)
        {
            return getSpecialNotes(headerName, "2").ToArray(); //2=star power
        }

        int[] INotesParser.GetFaceOffP1(string headerName)
        {
            List<int> ints = getSpecialNotes(headerName, "0"); //0=FaceOffP1

            for (int i = ints.Count - 3; i >= 0; i -= 3)
                ints.RemoveAt(i + 2); //remove notes count;

            return ints.ToArray();
        }

        int[] INotesParser.GetFaceOffP2(string headerName)
        {
            List<int> ints = getSpecialNotes(headerName, "1"); //0=FaceOffP1

            for (int i = ints.Count - 3; i >= 0; i -= 3)
                ints.RemoveAt(i + 2); //remove notes count;

            return ints.ToArray();
        }


        int[] INotesParser.GetFretBars(int msSongLength)
        {

            int len = msSongLength;
            int i = 0;
            int v;

            //if (_endEvent > len)
            //    len = _endEvent;

            if (_maxNoteParsed > len)
                len = _maxNoteParsed;


            List<int> l = new List<int>();
            do
            {
                v = offsetToMicroseconds(_resolution * i++);
                l.Add(v);
            }
            while (v >= 0 && v <= len); //last fret can be after song ends  (v >= 0 to prevent infinite loop when info is missing

            return l.ToArray();
        }

        string INotesParser.MatchType(NotesType type, NotesDifficulty difficulty)
        {
            string val = string.Empty;
            string diff = string.Empty;
            string t = string.Empty;

            switch (difficulty)
            {
                case NotesDifficulty.Easy:
                    diff = "Easy";
                    break;
                case NotesDifficulty.Medium:
                    diff = "Medium";
                    break;
                case NotesDifficulty.Hard:
                    diff = "Hard";
                    break;
                case NotesDifficulty.Expert:
                    diff = "Expert";
                    break;
            }

            if (diff.Length != 0)
            {
                switch (type)
                {
                    case NotesType.Guitar:
                        t = "Single";
                        break;
                    case NotesType.Rhythm:
                        t = "DoubleBass";
                        break;
                    case NotesType.GuitarCoop:
                        t = "CoopLead";
                        break;
                    case NotesType.RhythmCoop:
                        t = "CoopBass";
                        break;
                }

                t = string.Concat(diff, t);
                if (_headerNames.ContainsKey(t))
                    val = t;
    
            }

            return val;
        }

        int[] INotesParser.GetTimeSig()
        {
            return _timeSig;
        }

        int INotesParser.GetSustainTrigger()
        {
            if (_sustainedTrigger == 0)
            {
                int[] frets = ((INotesParser)this).GetFretBars(10000); //bit of a hack, calculate at least 10 seconds of markers
                _sustainedTrigger = ((float)(frets[1] - frets[0]) / 2F);
            }
            return (int)_sustainedTrigger;
        }
        #endregion

        private void parseSongHeader(int lineNo)
        {
            int i = lineNo + 1;
            string[] sp = _chart[i++].Split(new char[] { '=' }, 2);
            
            while (sp[0] != "}")
            {
                if (string.Compare(sp[0].Trim(), "resolution", true) == 0)
                    _resolution = int.Parse(sp[1].Trim());
                else if (string.Compare(sp[0].Trim(), "offset", true) == 0)
                    _offset = double.Parse(sp[1].Trim());
                sp = _chart[i++].Split('=');
            }
            
        }

        private void parseSyncTrackHeader(int lineNo)
        {
            int i = lineNo + 1;
            string[] sp = _chart[i++].Split(' ');

            List<int> l = new List<int>();
            List<double> u = new List<double>();
            List<int> o = new List<int>();

            int bpm;

            while (sp[0] != "}")
            {
                if (sp[2] == "B")
                {
                    bpm = int.Parse(sp[3]);

                    o.Add(int.Parse(sp[0]));
                    u.Add((double)60000 / ((double)bpm / (double)1000) / (double)_resolution); //value of a single unit at this bpm
                }
                else if (sp[2] == "TS")
                {
                    l.Add(int.Parse(sp[0]));
                    l.Add(int.Parse(sp[3]));
                    l.Add(4); //always 4 for GH3
                }


                sp = _chart[i++].Split(' ');
            }
            _bpmUnit = u.ToArray();
            _bpmOffset = o.ToArray();

            if (l.Count != 0)
                _timeSig = l.ToArray();
            else
            {
                _timeSig = new int[3];
                _timeSig[0] = 0;
                _timeSig[1] = 4;
                _timeSig[2] = 4;
            }


        }

        private void parseEventsHeader(int lineNo)
        {
            List<NotesMarker> mrk = new List<NotesMarker>();
            int i = lineNo + 1;
            string[] sp = _chart[i++].Split(new char[] { ' ' }, 4);

            string secPrefix = "\"section ";

            while (sp[0] != "}")
            {
                if (sp[2] == "E")
                {
                    if (sp[3].ToLower().StartsWith(secPrefix))
                        mrk.Add(new NotesMarker(sp[3].Substring(secPrefix.Length, sp[3].Length - secPrefix.Length - 1).Replace('_', ' '), offsetToMicroseconds(int.Parse(sp[0]))));
                    else if (sp[3].ToLower() == "\"end\"")
                        _endEvent = offsetToMicroseconds(int.Parse(sp[0]));
                }

                sp = _chart[i++].Split(new char[] { ' ' }, 4);
            }

            _markers = mrk.ToArray();
        }

        private int offsetToMicroseconds(int noteOffset)
        {
            int i = 1;
            double ms = 0;
            int bpmOffset = 0;
            double bpmUnit = 0;

            while (i < _bpmOffset.Length && _bpmOffset[i] <= noteOffset)
            {
                ms += (double)(_bpmOffset[i] - _bpmOffset[i - 1]) * _bpmUnit[i - 1]; //get all the milliseconds for that bpm section
                i++;
            }

            //note is in between BPM markers
            if (bpmOffset != noteOffset)
            {
                if (i > 0)
                {
                    bpmOffset = _bpmOffset[i - 1];
                    bpmUnit = _bpmUnit[i - 1];
                }

                ms += (double)(noteOffset - bpmOffset) * bpmUnit; //get all the milliseconds for that bpm section
            }

            return (int)Math.Round(ms);
        }

        private int lengthToMicroseconds(int noteOffset, int length, bool addNoteLen)
        {
            int i = 0;
            double bpmUnit = 0;

            while (i < _bpmOffset.Length && _bpmOffset[i] <= noteOffset)
                bpmUnit = _bpmUnit[i++];

            double noteLen = 0;
            if (addNoteLen)
                noteLen = (int)(_sustainedTrigger / 2F);

            if (length == 0)
                return (int)(bpmUnit * (double)(_resolution >> 2));
            else
                return (int)Math.Round(noteLen + (bpmUnit * length));
        }

        private void adjustTimeSig()
        {
            for (int i = 0; i < _timeSig.Length; i += 3)
            {
                _timeSig[i] = offsetToMicroseconds(_timeSig[i]);
            }
        }

        private Dictionary<string, int> _headerNames;
        private NotesMarker[] _markers;

        private string[] _chart;
        private int _resolution;
        private double _offset;
        private int[] _timeSig;
        private double[] _bpmUnit;
        private int[] _bpmOffset;
        private int _endEvent;
        private int _maxNoteParsed;
        private float _sustainedTrigger;

    }
}
