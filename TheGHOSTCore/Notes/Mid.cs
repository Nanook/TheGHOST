using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    internal class Mid : INotesParser
    {


        internal Mid()
        {

        }

        private void adjustTimeSig()
        {
            if (_timeSig.Count == 0)
            {
                _timeSig.Add(0);
                _timeSig.Add(4);
                _timeSig.Add(4);
            }

            for (int i = 0; i < _timeSig.Count; i += 3)
            {
                _timeSig[i] = offsetToMicroseconds(_timeSig[i]);
                _timeSig[i + 2] = (int)Math.Pow(2, _timeSig[i + 2]);  //done when converting to charts, without it frets often look too spaced out.
            }
        }

        private void adjustNotes()
        {
            if (_sustainedTrigger == 0)
                ((INotesParser)this).GetSustainTrigger(); //ensure the value is set

            _maxNoteParsed = 0;

            List<int> n;
            foreach (string key in _noteSets.Keys)
            {
                n = _noteSets[key];
                for (int i = 0; i < n.Count; i += 3)
                {
                    n[i + 1] = lengthToMicroseconds(n[i], n[i + 1], true);
                    n[i] = offsetToMicroseconds(n[i]);
                    if (n[i] + n[i + 1] > _maxNoteParsed)
                        _maxNoteParsed = n[i] + n[i + 1];
                }
            }
        }


        private void adjustStarPower()
        {
            List<int> sp;
            foreach (string key in _spSets.Keys)
            {
                sp = _spSets[key];
                for (int i = 0; i < sp.Count; i += 3)
                {
                    //do length before adjusting the offset
                    sp[i + 1] = lengthToMicroseconds(sp[i], sp[i + 1], false);
                    sp[i] = offsetToMicroseconds(sp[i]);

                    //count notes used in sp
                    sp[i + 2] = 0;
                    List<int> notes = _noteSets[key];
                    for (int c = 0; c < notes.Count; c += 3)
                    {
                        if (notes[c] >= sp[i] + sp[i + 1]) //not is >= to end of sp
                            break;
                        else if (notes[c] > sp[i]) //note is > than start of sp
                            sp[i + 2]++;
                    }
                }

            }
        }

        private void adjustFaceOffP1()
        {
            List<int> fo;
            foreach (string key in _fo1Sets.Keys)
            {
                fo = _fo1Sets[key];
                for (int i = fo.Count - 3; i >= 0; i -= 3)
                {
                    //do length before adjusting the offset
                    fo[i + 1] = lengthToMicroseconds(fo[i], fo[i + 1], false);
                    fo[i] = offsetToMicroseconds(fo[i]);
                    fo.RemoveAt(i + 2); //remove notes count;
                }

            }
        }

        private void adjustFaceOffP2()
        {
            List<int> fo;
            foreach (string key in _fo2Sets.Keys)
            {
                fo = _fo2Sets[key];
                for (int i = fo.Count - 3; i >= 0; i -= 3)
                {
                    //do length before adjusting the offset
                    fo[i + 1] = lengthToMicroseconds(fo[i], fo[i + 1], false);
                    fo[i] = offsetToMicroseconds(fo[i]);
                    fo.RemoveAt(i + 2); //remove notes count;
                }

            }
        }

        private void adjustMarkers()
        {
            string secPrefix = "section ";


            for (int i = _markers.Count - 1; i >= 0; i--)
            {
                NotesMarker m = _markers[i];
                m.Offset = offsetToMicroseconds(m.Offset);

                m.Title = m.Title.TrimStart('[').TrimEnd(']'); //trim GH2 []
                m.Title = m.Title.TrimStart('"').TrimEnd('"');
                m.Title = m.Title.Replace('_', ' ');

                if (m.Title.ToLower().StartsWith(secPrefix))
                    m.Title = m.Title.Substring(secPrefix.Length, m.Title.Length - secPrefix.Length);
                else if (m.Title.ToLower() == "end")
                {
                    _endEvent = offsetToMicroseconds(m.Offset);
                    _markers.RemoveAt(i);
                }
                else
                {
                    _markers.RemoveAt(i);
                }
            }

            if (_markers.Count <= 1)
                _markers.Clear();
        }

        private int offsetToMicroseconds(int noteOffset)
        {
            int i = 1;
            double ms = 0;
            int bpmOffset = 0;
            double bpmUnit = 0;

            while (i < _bpmOffset.Count && _bpmOffset[i] <= noteOffset)
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

            while (i < _bpmOffset.Count && _bpmOffset[i] <= noteOffset)
                bpmUnit = _bpmUnit[i++];

            double noteLen = 0;
            if (addNoteLen)
                noteLen = (int)(_sustainedTrigger / 2F);

            if (length == 0)
                return (int)(bpmUnit * (double)(_resolution >> 2));
            else
                return (int)Math.Round(noteLen + (bpmUnit * length));
        }

        private void readNote(BinaryEndianReader br, int offset)
        {
            byte note = 0;
            byte speed = 0;

            if (_currNoteType == NoteType.NoteOn || _currNoteType == NoteType.NoteOff)
            {
                note = br.ReadByte();
                speed = br.ReadByte();

                _notes.Add(new MidNote(_currNoteType, _trackName, offset, note));
            }
            else if (_currNoteType == NoteType.Parameter || _currNoteType == NoteType.Pitch || _currNoteType == NoteType.NoteAftertouch)
            {
                note = br.ReadByte();
                speed = br.ReadByte();
            }
            else if (_currNoteType == NoteType.Program || _currNoteType == NoteType.ChannelAftertouch)
            {
                note = br.ReadByte();
            }
            else
            {
                throw new ApplicationException("Unknown Read Note Type");
            }
        }

        private string readText(BinaryEndianReader br, int length)
        {
            byte[] buff = br.ReadBytes(length);
            return Encoding.Default.GetString(buff);
        }

        private int read3ByteNum(BinaryEndianReader br, EndianType endian)
        {
            byte[] b = br.ReadBytes(3);
            if (endian == EndianType.Little)
                Array.Reverse(b);

            int val = (b[0] * 0x10000) + (b[1] * 0x100) + b[2];

            return val;
        }

        private ulong readVarLen(BinaryEndianReader br)
        {
            byte c = 0;
            ulong value = 0;

            if (((value = (ulong)br.ReadByte()) & 0x80) == 0x80)
            {
                value &= 0x7F;
                do
                {
                    value = (value << 7) + ((c = br.ReadByte()) & (ulong)0x7F);
                }
                while ((c & 0x80) == 0x80);
            }

            return value;
        }

        private void setNotes()
        {
            int noteLen;
            MidNote n;
            string key;
            string section;
            List<int> ints;

            //bool hasNoteOffs = false;
            //for (int i = 0; i < _notes.Count; i++)
            //{
            //    if (_notes[i].Type == NoteType.NoteOff)
            //    {
            //        hasNoteOffs = true;
            //        break;
            //    }
            //}

            //set note offs
            //if (!hasNoteOffs)
            //{
                for (int i = 0; i < _notes.Count; i++)
                {
                    n = _notes[i];
                    if (n.Type == NoteType.NoteOn)
                    {
                        for (int j = i + 1; j < _notes.Count; j++)
                        {
                            if ((_notes[j].Type == NoteType.NoteOff || _notes[j].Type == NoteType.NoteOn) && n.Note == _notes[j].Note)
                            {
                                _notes[j].Type = NoteType.NoteOff;
                                break;
                            }
                        }
                    }
                }
            //}


            for (int i = 0; i < _notes.Count; i++)
            {
                n = _notes[i];

                if (n.Type == NoteType.NoteOn)
                {
                    if (n.Difficulty == (NotesDifficulty)(-1))
                        continue;

                    section = n.Section;
                    key = string.Format("{0} : {1}", section, n.Difficulty.ToString());
                    if (n.IsStarPower)
                    {
                        if (!_spSets.ContainsKey(key))
                            _spSets.Add(key, new List<int>());
                        ints = _spSets[key];
                    }
                    else if (n.IsFaceOffP1)
                    {
                        if (!_fo1Sets.ContainsKey(key))
                            _fo1Sets.Add(key, new List<int>());
                        ints = _fo1Sets[key];
                    }
                    else if (n.IsFaceOffP2)
                    {
                        if (!_fo2Sets.ContainsKey(key))
                            _fo2Sets.Add(key, new List<int>());
                        ints = _fo2Sets[key];
                    }
                    else
                    {
                        if (!_noteSets.ContainsKey(key))
                            _noteSets.Add(key, new List<int>());
                        ints = _noteSets[key];
                    }

                    //if the offset is the same as the last offset for this notes set then just add the note to the GH3 mask
                    if (ints.Count >= 3 && ints[ints.Count - 3] == n.Offset)
                    {
                        ints[ints.Count - 1] |= n.GhNote;
                    }
                    else
                    {
                        //find note off (if present)
                        noteLen = 0;
                        for (int j = i + 1; j < _notes.Count; j++)
                        {
                            if (_notes[j].Type == NoteType.NoteOff && n.Note == _notes[j].Note)
                            {
                                noteLen = _notes[j].Offset - n.Offset;
                                break;
                            }
                        }

                        if (!n.IsStarPower && noteLen < 160)
                            noteLen = 0;

                        ints.Add(n.Offset);
                        ints.Add(noteLen);
                        if (!n.IsStarPower)
                            ints.Add(n.GhNote);
                        else
                            ints.Add(0); //calculate the sp note count later
                    }
                }
            }

            string[] headers = ((INotesParser)this).HeaderNames();
            //remove blank sets
            foreach (string h in headers)
            {
                if (_noteSets[h].Count == 0)
                {
                    _noteSets.Remove(h);
                    _spSets.Remove(h);
                    _fo1Sets.Remove(h);
                    _fo2Sets.Remove(h);
                }
            }
        }

        #region INotesParser Members

        void INotesParser.Parse(string filename)
        {
            EndianType endian = EndianType.Big;
            _maxNoteParsed = 0;
            _timeSig = new List<int>();
            _tempo = new List<int>();
            _notes = new List<MidNote>();
            _trackName = string.Empty;
            _noteSets = new Dictionary<string, List<int>>();
            _spSets = new Dictionary<string, List<int>>();
            _fo1Sets = new Dictionary<string, List<int>>();
            _fo2Sets = new Dictionary<string, List<int>>();

            _bpmUnit = new List<double>();
            _bpmOffset = new List<int>();

            _markers = new List<NotesMarker>();

            uint mthdLength;
            uint mthdFormat;
            uint mthdTracks;

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                BinaryEndianReader br = new BinaryEndianReader(fs);

                if (readText(br, 4) == "MThd")
                {
                    mthdLength = br.ReadUInt32(endian);
                    mthdFormat = br.ReadUInt16(endian);
                    mthdTracks = br.ReadUInt16(endian);
                    _resolution = br.ReadUInt16(endian); //mthdDivision

                    for (int t = 0; t < mthdTracks; t++)
                    {
                        int totalTime = 0;

                        if (readText(br, 4) == "MTrk")
                        {
                            uint mtrkLength = br.ReadUInt32(endian);
                            long mtrkStartPos = fs.Position;

                            while (fs.Position < mtrkStartPos + (long)mtrkLength)
                            {
                                int len = 0;
                                int time = (int)readVarLen(br);
                                byte type = br.ReadByte();

                                totalTime += time;

                                if (type == 0xFF)
                                {
                                    byte typeFF = br.ReadByte();
                                    switch (typeFF)
                                    {
                                        case 0x03: //track name
                                            len = (int)readVarLen(br);
                                            _trackName = readText(br, len);

                                            if (t == 0) //main track
                                                _songName = _trackName;
                                            break;
                                        case 0x04: //?
                                            len = (int)readVarLen(br);
                                            string unknown = readText(br, len);
                                            break;
                                        case 0x01: //Text
                                            len = (int)readVarLen(br);
                                            string text = readText(br, len);

                                            if (_trackName == "EVENTS")
                                                _markers.Add(new NotesMarker(text, totalTime));

                                            break;
                                        case 0x58: //Timesig
                                            len = (int)readVarLen(br);
                                            if (len != 4)
                                                throw new ApplicationException("Invalid Time Signature");

                                            byte num = br.ReadByte();
                                            byte denum = br.ReadByte();
                                            byte clocks = br.ReadByte();
                                            byte notated32 = br.ReadByte();

                                            if (t == 0) //main track
                                            {
                                                _timeSig.Add(totalTime);
                                                _timeSig.Add(num);
                                                _timeSig.Add(denum);
                                            }
                                            break;
                                        case 0x51:  //BPM
                                            len = (int)readVarLen(br);
                                            if (len != 3)
                                                throw new ApplicationException("Invalid Tempo");

                                            int tempo = read3ByteNum(br, endian);


                                            if (t == 0) //main track
                                            {
                                                int bpm = (int)Math.Round(60000000000.0 / (double)tempo); //convert to value used in chart

                                                _bpmOffset.Add(totalTime);
                                                _bpmUnit.Add((double)60000 / ((double)bpm / (double)1000) / (double)_resolution); //value of a single unit at this bpm
                                            }

                                            break;
                                        case 0x2F: //track end
                                            len = (int)br.ReadByte();
                                            if (len != 0)
                                                throw new ApplicationException("Invalid Track End");
                                            break;
                                        case 0x7F: //Propietary event
                                            len = (int)br.ReadByte();
                                            fs.Seek(len, SeekOrigin.Current);
                                            break;
                                        case 0x20: //channel
                                            byte channel = br.ReadByte();
                                            break;
                                        case 0x05: //lyric
                                            len = (int)readVarLen(br);
                                            string lyric = readText(br, len);
                                            break;
                                        case 0x06: //Marker
                                            len = (int)readVarLen(br);
                                            string marker = readText(br, len);
                                            _markers.Add(new NotesMarker(string.Format("[section {0}]", marker), totalTime));
                                            break;
                                        case 0x59:
                                            len = (int)readVarLen(br);
                                            fs.Seek(len, SeekOrigin.Current);
                                            break;
                                        case 0x54:
                                            len = (int)readVarLen(br);
                                            fs.Seek(len, SeekOrigin.Current);
                                            break;
                                        case 0x21:
                                            byte startEvent = br.ReadByte();
                                            len = (int)readVarLen(br);
                                            break;
                                        default:
                                            throw new ApplicationException("Unknown FF Type");
                                        //break;
                                    }
                                }
                                else if (type < 0x80) //repeat last type
                                {
                                    //_noteType = //use last type
                                    fs.Seek(-1, SeekOrigin.Current);
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0x80 && type < 0x90) //note off
                                {
                                    _currNoteType = NoteType.NoteOff;
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0x90 && type < 0xA0) //note on
                                {
                                    _currNoteType = NoteType.NoteOn;
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0xA0 && type < 0xB0) //note aftertouch
                                {
                                    _currNoteType = NoteType.NoteAftertouch;
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0xB0 && type < 0xC0) //parameter
                                {
                                    _currNoteType = NoteType.Parameter;
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0xC0 && type < 0xD0) //program
                                {
                                    _currNoteType = NoteType.Program;
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0xD0 && type < 0xE0) //channel aftertouch
                                {
                                    _currNoteType = NoteType.ChannelAftertouch;
                                    readNote(br, totalTime);
                                }
                                else if (type >= 0xE0 && type < 0xF0) //pitch
                                {
                                    _currNoteType = NoteType.Pitch;
                                    readNote(br, totalTime);
                                }
                                else
                                {
                                    throw new ApplicationException("Unknown Note Type");
                                }

                            }
                        }
                    }
                }
            }

            setNotes();
            adjustTimeSig(); //convert offsets to ms
            adjustMarkers(); //convert offsets to ms
            adjustNotes();
            adjustStarPower();
            adjustFaceOffP1();
            adjustFaceOffP2();
            //free up some memory
            _notes.Clear();
        }


        string[] INotesParser.HeaderNames()
        {
            string[] keys = new string[_noteSets.Count];
            _noteSets.Keys.CopyTo(keys, 0);
            return keys;
        }

        int[] INotesParser.GetNotes(string headerName)
        {
            if (_noteSets.ContainsKey(headerName))
                return _noteSets[headerName].ToArray();
            else
                return new int[0];
        }

        int[] INotesParser.GetStarPower(string headerName)
        {
            if (_spSets.ContainsKey(headerName))
                return _spSets[headerName].ToArray();
            else
                return new int[0];
        }

        int[] INotesParser.GetFaceOffP1(string headerName)
        {
            if (_fo1Sets.ContainsKey(headerName))
                return _fo1Sets[headerName].ToArray();
            else
                return new int[0];
        }

        int[] INotesParser.GetFaceOffP2(string headerName)
        {
            if (_fo2Sets.ContainsKey(headerName))
                return _fo2Sets[headerName].ToArray();
            else
                return new int[0];
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
            while (v <= len); //last fret can be after song ends

            return l.ToArray();
        }

        int[] INotesParser.GetTimeSig()
        {
            return _timeSig.ToArray();
        }

        int INotesParser.GetSustainTrigger()
        {
            if (_sustainedTrigger == 0)
            {
                int[] frets = ((INotesParser)this).GetFretBars(10000); //bit of a hack, just calculate 10 seconds of markers
                _sustainedTrigger = ((float)(frets[1] - frets[0]) / 2F);
            }
            return (int)_sustainedTrigger;
        }

        NotesMarker[] INotesParser.GetNotesMarkers()
        {
            return _markers.ToArray();
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
                        t = "PART GUITAR";
                        break;
                    case NotesType.Rhythm:
                        t = "PART BASS";
                        break;
                    case NotesType.GuitarCoop:
                        t = "PART GUITAR COOP";
                        break;
                    case NotesType.RhythmCoop:
                        t = "PART RHYTHM";
                        break;
                }

                string t1 = string.Format("{0} : {1}", t, diff.ToString());
                string tGems = string.Format("T1 GEMS : {0}", diff.ToString());
                if (_noteSets.ContainsKey(t1))
                    val = t1;
                else if (_noteSets.ContainsKey(tGems))
                    val = tGems;
                else if (type == NotesType.Guitar)
                {
                    string t2 = string.Format("{0} : {1}", _songName, diff.ToString());
                    if (_noteSets.ContainsKey(t2))
                        val = t2;
                    else
                    {
                        t2 = string.Format("{0} : {1}", _trackName, diff.ToString());
                        if (_noteSets.ContainsKey(t2))
                            val = t2;
                    }
                }
                else if (type == NotesType.Rhythm)
                {
                    string t2 = string.Format("PART RHYTHM : {0}", diff.ToString());
                    if (_noteSets.ContainsKey(t2))
                        val = t2;
                }


            }

            return val;
        }

        #endregion

        private NoteType _currNoteType;

        private string _songName;

        private string _trackName;
        private List<MidNote> _notes;

        private Dictionary<string, List<int>> _noteSets;
        private Dictionary<string, List<int>> _spSets;
        private Dictionary<string, List<int>> _fo1Sets;
        private Dictionary<string, List<int>> _fo2Sets;

        private List<int> _timeSig;
        private List<int> _tempo;

        private List<NotesMarker> _markers;

        private int _resolution;
        private List<double> _bpmUnit;
        private List<int> _bpmOffset;
        private int _endEvent;
        private int _maxNoteParsed;
        private float _sustainedTrigger;


    }
}
