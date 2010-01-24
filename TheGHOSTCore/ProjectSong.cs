using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;
using System.Diagnostics;

namespace Nanook.TheGhost
{
    public class ProjectSong : ISettingsChange
    {
        internal ProjectSong(Project project, SongQb songQb)
        {
            _lastChanged = DateTime.MinValue;
            _qbLastChanged = DateTime.MinValue;
            _recordChange = false;

            _project = project;
            _songQb = songQb;
            _artist = this.SongQb.Artist;
            _title = this.SongQb.Title;

            if (_project.Defaults.SongVolumeMode == DefaultSettingMode.Game)
                _year = this.SongQb.Year;
            else
                _year = _project.Defaults.Year;

            if (_project.Defaults.SongVolumeMode == DefaultSettingMode.Game)
                _songVolume = this.SongQb.SongVolume;
            else
                _songVolume = _project.Defaults.SongVolume;

            if (_project.Defaults.GuitarVolumeMode == DefaultSettingMode.Game)
                _guitarVolume = this.SongQb.GuitarVolume;
            else
                _guitarVolume = _project.Defaults.GuitarVolume;

            if (_project.Defaults.SingerMode == DefaultSettingMode.Game)
                _singer = this.SongQb.Singer;
            else
                _singer = _project.Defaults.Singer;

            _originalArtist = true; // this.Song.OriginalArtist;

            _minMsBeforeNotesStart = _project.Defaults.MinMsBeforeNotesStart;
            _startPaddingMs = 0;

            _audio = new ProjectSongAudio(_project, this);
            _notes = new ProjectSongNotes(_project, this);

            _audio.PreviewFadeLength = _project.Defaults.PreviewFadeLength;
            _audio.PreviewLength = _project.Defaults.PreviewLength;
            _audio.PreviewStart = _project.Defaults.PreviewStart;
            _audio.PreviewVolume = _project.Defaults.PreviewVolume;
            _audio.PreviewIncludeGuitar = _project.Defaults.PreviewIncludeGuitar;
            _audio.PreviewIncludeRhythm = _project.Defaults.PreviewIncludeRhythm;

            _notes.HoPoMeasure = _project.Defaults.HoPoMeasure;
            _notes.Gh3SustainClipping = _project.Defaults.Gh3SustainClipping;
            _notes.ForceNoStarPower = _project.Defaults.ForceNoStarPower;
        }

        public bool IsBoss
        {
            get { return _songQb.IsBoss; }
        }

        public string Title
        {
            get { return _title; }
            set 
            {
                if (_title == fixText(value))
                    return;
                _title = fixText(value);
                this.QbLastChanged = DateTime.Now;
            }
        }

        public string Artist
        {
            get { return _artist; }
            set 
            {
                if (_artist == fixText(value))
                    return;
                _artist = fixText(value);
                this.QbLastChanged = DateTime.Now;
            }
        }

        public string Year
        {
            get { return _year; }
            set
            {
                if (_year == value)
                    return;
                _year = value;
                this.QbLastChanged = DateTime.Now;
            }
        }

        private string fixText(string text)
        {
            return text.Replace('’', '\'');
        }

        public float GuitarVolume
        {
            get { return _guitarVolume; }
            set
            {
                if (_guitarVolume == value)
                    return;
                _guitarVolume = value;
                this.QbLastChanged = DateTime.Now;
            }
        }

        public float SongVolume
        {
            get { return _songVolume; }
            set
            {
                if (_songVolume == value)
                    return;
                _songVolume = value;
                this.QbLastChanged = DateTime.Now;
            }
        }


        public Singer Singer
        {
            get { return _singer; }
            set
            {
                if (_singer == value)
                    return;
                _singer = value;
                this.QbLastChanged = DateTime.Now;
            }
        }

        public bool OriginalArtist
        {
            get { return _originalArtist; }
            set
            {
                if (_originalArtist == value)
                    return;
                _originalArtist = value;
                this.QbLastChanged = DateTime.Now;
            }
        }

        public bool AllFilesExist
        {
            get
            {
                return _notes.AllFilesExist && _audio.AllFilesExist;
            }
        }

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public SongQb SongQb
        {
            get { return _songQb; }
            set { _songQb = value; }
        }

        private bool qbChanged
        {
            get
            {
                if (this.Title != this.SongQb.Title ||
                    this.Artist != this.SongQb.Artist ||
                    this.Year != this.SongQb.Year ||
                    this.GuitarVolume != this.SongQb.GuitarVolume ||
                    this.SongVolume != this.SongQb.SongVolume ||
                    this.OriginalArtist != this.SongQb.OriginalArtist ||
                    this.Singer != this.SongQb.Singer)
                {
                    return true;
                }
                return false;


            }
        }

        public int MinMsBeforeNotesStart  //Handle this changing,  may just set LastUpdate on NOTES AND AUDIO
        {
            get { return _minMsBeforeNotesStart; }
            set
            {
                if (_minMsBeforeNotesStart == value)
                    return;
                _minMsBeforeNotesStart = value;
                //apply date to multiple items
                this.Notes.LastChanged = this.Audio.LastChanged = DateTime.Now;
            }
        }

      
        public ProjectSongAudio Audio
        {
            get { return _audio; }
        }

        public ProjectSongNotes Notes
        {
            get { return _notes; }
        }

        /// <summary>
        /// Reads Artis and Title from song ini file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Delay offset</returns>
        public int AutoImportSongIni(string fileName)
        {
            if (!File.Exists(fileName))
                return 0;

            string[] l = File.ReadAllLines(fileName, Encoding.Default);

            bool found = false;

            string n = "name = ";
            string a = "artist = ";
            string y = "year = ";
            string d = "delay = ";
            int delay = 0;

            foreach (string s in l)
            {
                if (s.ToLower() == "[song]")
                {
                    this.Artist = string.Empty;
                    this.Title = string.Empty;
                    this.Year = string.Empty; // string.Empty;
                    if (this.Notes.BaseFile != null)
                        this.Notes.BaseFile.NonNoteSyncOffset = 0;
                    this.GuitarVolume = 0;
                    this.SongVolume = 0;
                    found = true;
                }
                else if (found)
                {
                    if (s.Length > n.Length && s.ToLower().StartsWith(n))
                        this.Title = s.Substring(n.Length);
                    else if (s.Length > a.Length && s.ToLower().StartsWith(a))
                        this.Artist = s.Substring(a.Length);
                    else if (s.Length > y.Length && s.ToLower().StartsWith(y))
                        this.Year = s.Substring(y.Length);
                    else if (s.Length > d.Length && s.ToLower().StartsWith(d))
                        delay = int.Parse(s.Substring(d.Length));
                }

            }
            return delay;
        }

        /// <summary>
        /// Import a directory, map the audio, parse the song.ini, set the notes file
        /// </summary>
        /// <param name="folderName"></param>
        public void AutoImportDirectory(string folderName, bool addCrowd)
        {
            if (!Directory.Exists(folderName))
                return;

            string ghostSettings = string.Format(@"{0}\TheGHOST_Settings.tgs", folderName.TrimEnd('\\'));

            if (File.Exists(ghostSettings))
            {
                //remove existing songs
                for (int i = this.Audio.SongFiles.Count - 1; i >= 0; i--)
                    this.Audio.SongFiles.RemoveAt(i);

                for (int i = this.Notes.Files.Length - 1; i >= 0; i--)
                    this.Notes.RemoveFile(this.Notes.Files[i]);

                _project.Settings.LoadSongXmlSettings(ghostSettings, this);
                this.LastChanged = DateTime.Now;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(folderName);
                string mask = string.Format(@"{0}\{{0}}.{{1}}", di.FullName);

                string[] exts = new string[] { "wav", "flac", "ogg", "mp3" }; //order is important

                List<string> song = new List<string>();
                string guitar = null;
                string rhythm = null;

                string[] songNames = { "song", "song1", "song2", "song3", "song4", "song5", "song6", "song7", "song8", "song9",
                                   "drums", "drums1", "drums2", "drums3" };

                foreach (string ext in exts)
                {
                    //search for all the song types
                    foreach (string s in songNames)
                    {
                        if (File.Exists(string.Format(mask, s, ext)))
                            song.Add(string.Format(mask, s, ext));
                    }

                    //add the crowd if required
                    if (addCrowd && File.Exists(string.Format(mask, "crowd", ext)))
                        song.Add(string.Format(mask, "crowd", ext));

                    if (guitar == null && File.Exists(string.Format(mask, "guitar", ext)))
                        guitar = string.Format(mask, "guitar", ext);

                    if (rhythm == null && File.Exists(string.Format(mask, "rhythm", ext)))
                        rhythm = string.Format(mask, "rhythm", ext);
                }

                //if guitar with no song them use only song - TheGHOST will duplicate it to the guitar when making wads
                if (song.Count == 0 && guitar != null)
                {
                    song.Add(guitar);
                    guitar = null;
                }

                for (int i = this.Notes.Files.Length - 1; i >= 0; i--)
                    this.Notes.RemoveFile(this.Notes.Files[i]);

                foreach (GhNotesItem ghi in this.Notes.GhItems)
                {
                    if (ghi.IsMapped)
                        this.Notes.UnMapGhItem(ghi); //remove generated items
                }

                int delay = 0;

                if (File.Exists(string.Format(mask, "song", "ini")))
                    delay = this.AutoImportSongIni(string.Format(mask, "song", "ini"));

                NotesFile nf = null;

                //if notes.chart exists then import as first mid.
                if (File.Exists(string.Format(mask, "notes", "mid")))
                    nf = this.Notes.ParseFile(string.Format(mask, "notes", "mid"));

                if (nf != null)
                {
                    foreach (NotesFileItem nfi in nf.Items)
                        nfi.SyncOffset = delay;
                    nf.NonNoteSyncOffset = delay;
                    nf = null;
                }

                //if notes.chart exists then import as chart.
                if (File.Exists(string.Format(mask, "notes", "chart")))
                    nf = this.Notes.ParseFile(string.Format(mask, "notes", "chart"));

                if (nf != null)
                {
                    foreach (NotesFileItem nfi in nf.Items)
                        nfi.SyncOffset = delay;
                    nf.NonNoteSyncOffset = delay;
                    nf = null;
                }

                //add other mids
                foreach (FileInfo fi in (new DirectoryInfo(folderName)).GetFiles("*.mid"))
                {
                    if (fi.Name.ToLower() != "notes.mid") //already added
                    {
                        nf = this.Notes.ParseFile(fi.FullName);
                        if (nf != null)
                        {
                            foreach (NotesFileItem nfi in nf.Items)
                                nfi.SyncOffset = delay;
                            nf.NonNoteSyncOffset = delay;
                            nf = null;
                        }
                    }
                }

                //add other charts
                foreach (FileInfo fi in (new DirectoryInfo(folderName)).GetFiles("*.chart"))
                {
                    if (fi.Name.ToLower() != "notes.chart") //already added
                    {
                        nf = this.Notes.ParseFile(fi.FullName);
                        if (nf != null)
                        {
                            foreach (NotesFileItem nfi in nf.Items)
                                nfi.SyncOffset = delay;
                            nf = null;
                        }
                    }
                }


                //set the sync value
                if (this.Notes.BaseFile != null)
                {
                    foreach (GhNotesItem ghi in this.Notes.GhItems)
                    {
                        if (ghi.IsMapped)
                            ghi.MappedFileItem.SyncOffset = this.Notes.BaseFile.NonNoteSyncOffset;
                    }
                }

                //there must be a song / guitar to continue
                if (song.Count != 0)
                {
                    //remove existing songs
                    for (int i = this.Audio.SongFiles.Count - 1; i >= 0; i--)
                        this.Audio.SongFiles.RemoveAt(i);

                    foreach (string s in song)
                        this.Audio.SongFiles.Add(this.Audio.CreateAudioFile(s, _project.Defaults.AudioSongVolume));

                    this.Audio.GuitarFile = guitar == null ? null : this.Audio.CreateAudioFile(guitar, _project.Defaults.AudioGuitarVolume);
                    this.Audio.RhythmFile = rhythm == null ? null : this.Audio.CreateAudioFile(rhythm, _project.Defaults.AudioRhythmVolume);
                }
            }

            for (int i = 0; i < this.Audio.SongFiles.Count; i++)
                this.Audio.Import(this.Audio.SongFiles[i].Name, this.Audio.RawSongFilenames[i]);
            if (this.Audio.GuitarFile != null && this.Audio.GuitarFile.Name.Length != 0)
                this.Audio.Import(this.Audio.GuitarFile.Name, this.Audio.RawGuitarFilename);
            if (this.Audio.RhythmFile != null && this.Audio.RhythmFile.Name.Length != 0)
                this.Audio.Import(this.Audio.RhythmFile.Name, this.Audio.RawRhythmFilename);
        }

        public void UpdateQbChanges()
        {
            if (this.qbChanged)
            {
                //sets data in the QB.PAK
                this.SongQb.Title = this.Title;
                this.SongQb.Artist = this.Artist;
                if (_project.Defaults.YearMode != DefaultSettingModeBlank.Blank)
                    this.SongQb.Year = this.Year;
                else
                    this.SongQb.Year = string.Empty;
                this.SongQb.GuitarVolume = this.GuitarVolume;
                this.SongQb.SongVolume = this.SongVolume;
                this.SongQb.OriginalArtist = this.OriginalArtist;
                this.SongQb.Singer = this.Singer;
                this.SongQb.HopoMeasure = this.Notes.HoPoMeasure;
                this.SongQb.RhythmTrack = false;
                this.SongQb.RemoveUseCoopNoteTracks();
                this.SongQb.ZeroOffsets();
            }
        }


        #region Song Finalisation routines


        /// <summary>
        /// Apply any padding to notes, audio, frets and generate practise markers
        /// </summary>
        public void FinaliseSettings()
        {
            int notesLength = 0;

            List<string> wavs = new List<string>();

            this.UpdateQbChanges();

            bool updateNotes = this.Notes.LastChanged > this.LastApplied || _project.Defaults.ReapplyAll;
            bool updateAudio = this.Audio.LastChanged > this.LastApplied || _project.Defaults.ReapplyAll;

            //if either the notes or the audio have changed then update the notes.

            if (updateNotes || updateAudio)
            {
                //this sets the _startPaddingMs and _fretPadding (these are important as they are required to pad the audio correctly)

                notesLength = replaceNotesItems();
            }


            if (updateAudio || this.Notes.UpdateAffectsAudio)
            {
                //import missing files (was the item green then modified)
                this.Audio.ImportMissingAudioFiles();

                this.Audio.RemoveAndDeleteRedundantFiles();

                float ac = 0;
                if (this.Audio.RhythmFile != null && this.Audio.RhythmFile.Name.Length != 0 && File.Exists(this.Audio.RawRhythmFilename))
                    ac++;
                if (this.Audio.GuitarFile != null && this.Audio.GuitarFile.Name.Length != 0 && File.Exists(this.Audio.RawGuitarFilename))
                    ac++;
                if (this.Audio.SongFiles.Count > 0)
                    ac++;

                if (ac != 0)
                    ac = 0.66F; // 1 / ac;

                //ensure audio is all the same length
                int newAudioLen = this.Audio.AudioLength + _startPaddingMs + _fretPadding; //to keep in sync
                if (newAudioLen < notesLength + 500) //to ensure is greater than notes (last fret)
                    newAudioLen = notesLength + 500;

                if (this.Audio.RhythmFile != null && this.Audio.RhythmFile.Name.Length != 0 && File.Exists(this.Audio.RawRhythmFilename))
                    WavProcessor.SetLengthSilenceAndVolume(_startPaddingMs + _fretPadding, newAudioLen, (this.Audio.RhythmFile.Volume * ac) / 100F, this.Audio.RawRhythmFilename);
                if (this.Audio.GuitarFile != null && this.Audio.GuitarFile.Name.Length != 0 && File.Exists(this.Audio.RawGuitarFilename))
                    WavProcessor.SetLengthSilenceAndVolume(_startPaddingMs + _fretPadding, newAudioLen, (this.Audio.GuitarFile.Volume * ac) / 100F, this.Audio.RawGuitarFilename);

                for (int i = 0; i < this.Audio.SongFiles.Count; i++)
                {
                    if (File.Exists(this.Audio.RawSongFilenames[i]))
                    {
                        float vol = this.Audio.SongFiles[i].Volume / 100F;

                        //if no guitar or rhythm then half the volume of the song as it will be used as the guitar as well (stops the distortion).
                        if ((this.Audio.RhythmFile == null || this.Audio.RhythmFile.Name.Length == 0 || !File.Exists(this.Audio.RawRhythmFilename))
                            && (this.Audio.GuitarFile == null || this.Audio.GuitarFile.Name.Length == 0 || !File.Exists(this.Audio.RawGuitarFilename)))
                        {
                            if (this.Audio.SongFiles.Count == 1)
                                WavProcessor.Normalize(this.Audio.RawSongFilenames[i], true); //don't normalise when there's >1 because all files will be 100% and not the correct levels
                            else
                                vol *= 0.5F; //about half the volume
                        }
                        else
                            vol *= ac; //third if guitar and rhythm exists for example

                        WavProcessor.SetLengthSilenceAndVolume(_startPaddingMs + _fretPadding, newAudioLen, vol, this.Audio.RawSongFilenames[i]);
                    }
                }
            }
        }

        private int replaceNotesItems()
        {
            int notesLength = 0; //position of final notes item (last fret) 
            QbKey song = this.SongQb.Id;
            string songName = song.Text;

            GameFile gf = _project.FileManager.File(_project.GameInfo.GetNotesFilename(song));

            if (gf == null)
                return notesLength; //return 0, this prevents exceptions in the ISO tool which doesn't replace notes

            string notesPak = gf.LocalName;

            PakFormat pf = new PakFormat(notesPak, "", "", _project.GameInfo.PakFormatType);
            PakEditor pak = new PakEditor(pf);
            QbFile qb = pak.ReadQbFile(_project.GameInfo.GetNotesQbFilename(song));

            //modify the mid file
            //clearAllQbItems(qb, this.Notes.GhItems,
            //    QbKey.Create(string.Format("{0}_TimeSig", songName)),
            //    QbKey.Create(string.Format("{0}_FretBars", songName)),
            //    QbKey.Create(string.Format("{0}_Markers", songName)));



            QbFile qbText;
            if (!_project.GameInfo.MidTextInQbPak)
                qbText = pak.ReadQbFile(_project.GameInfo.GetNotesTextQbFilename(song));
            else
            {
                qbText = _project.FileManager.QbPakEditor.ReadQbFile(_project.GameInfo.GetNotesTextQbFilename(song));
            }

            QbItemArray ar;

            int[] ints;
            int offset;


            //calculate ms to add to wav to ensure notes don't start before the given time (in seconds)
            if (this.Notes.MinNoteOffsetSynced < this.MinMsBeforeNotesStart)
                _startPaddingMs = this.MinMsBeforeNotesStart - this.Notes.MinNoteOffsetSynced;


            this.Length = this.Audio.AudioLength + _startPaddingMs;


            offset = this.Notes.BaseFile.NonNoteSyncOffset + _startPaddingMs;

            int[] frets = (int[])this.Notes.BaseFile.Frets.Clone(); //clone in case class is holding reference
            for (int i = 0; i < frets.Length; i++)
                frets[i] += offset;


            //this function sets _fretPadding, another padding value.  It adds padding to allow a properly spaced fret to be at position 0
            frets = adjustFrets(frets);
            notesLength = frets[frets.Length - 1];

            //set the track length to the notes length if it's longer
            if (this.Length < notesLength)
                this.Length = notesLength;
            else
                this.Length += _fretPadding;

            offset += _fretPadding;

            int[] timeSig = (int[])this.Notes.BaseFile.Parser.GetTimeSig().Clone(); //clone in case class is holding reference

            if (timeSig.Length == 0)
                timeSig = new int[] { 0, 4, 4 };

            for (int i = 0; i < timeSig.Length; i += 3)
                timeSig[i] = (int)findNearestFret((uint)(timeSig[i] + offset), frets);

            timeSig[0] = 0; //if the first item is not 0 the song won't load??.

            NotesMarker[] markers = (NotesMarker[])this.Notes.BaseFile.Parser.GetNotesMarkers().Clone(); //clone in case class is holding reference
            for (int i = 0; i < markers.Length; i += 3)
                markers[i] = new NotesMarker(markers[i].Title, markers[i].Offset + offset); //perform deep clone

            int oldSustainTrigger;
            int sustainTrigger;
            int nextNote;
            int[] faceOffP1Ints = new int[0];
            int[] faceOffP2Ints = new int[0];
            GhNotesItem faceOffItem = null;

            //we have a face off item for each mapping, GH3 only has one per song,  find the one with the hardest difficulty (They're always the same anyway)
            foreach (GhNotesItem ghi in this.Notes.GhItems)
            {
                if (ghi.IsMapped && ghi.MappedFileItem.FaceOffP1Count != 0 && ghi.MappedFileItem.FaceOffP2Count != 0)
                {
                    //non generated overrides generated 
                    if (faceOffItem == null || faceOffItem.MappedFileItem.HasGeneratedFaceOff || (ghi.MappedFileItem.HasGeneratedFaceOff == faceOffItem.MappedFileItem.HasGeneratedFaceOff))
                    {
                        if (faceOffItem == null || ((int)faceOffItem.Difficulty < (int)ghi.Difficulty && ghi.Type == NotesType.Guitar)) //guitar is more reliable
                            faceOffItem = ghi;
                    }
                }
            }

            if (faceOffItem != null)
            {
                offset = faceOffItem.MappedFileItem.SyncOffset + _startPaddingMs + _fretPadding;

                faceOffP1Ints = (int[])faceOffItem.MappedFileItem.FaceOffP1.Clone();
                for (int i = 0; i < faceOffP1Ints.Length; i += 2)
                    faceOffP1Ints[i] += offset;

                faceOffP2Ints = (int[])faceOffItem.MappedFileItem.FaceOffP2.Clone();
                for (int i = 0; i < faceOffP2Ints.Length; i += 2)
                    faceOffP2Ints[i] += offset;
            }


            foreach (GhNotesItem ghi in this.Notes.GhItems)
            {
                if (ghi.IsMapped)
                {
                    oldSustainTrigger = ghi.MappedFileItem.SustainTrigger;
                    sustainTrigger = (int)((float)(frets[1] - frets[0]) / 2F);
                    ghi.MappedFileItem.SustainTrigger = sustainTrigger;

                    //Offset notes
                    offset = ghi.MappedFileItem.SyncOffset + _startPaddingMs + _fretPadding;

                    ints = (int[])ghi.MappedFileItem.Notes.Clone(); //don't modify original notes
                    int startFret = 0;
                    for (int i = 0; i < ints.Length; i += 3)
                    {
                        ints[i] += offset;
                        if (i + 3 < ints.Length)
                            nextNote = ints[i + 3] + offset;
                        else
                            nextNote = 0;

                        //loop to find fret length note is in to calculate if note is sustained
                        int fpos;
                        int fretLen = 0;
                        bool isSustained = false;
                        for (int c = startFret; c < frets.Length; c++)
                        {
                            if ((fpos = frets[c]) > ints[i] && c > 0) //careful fpos is assigned here
                            {
                                startFret = c; //next time start from here
                                fretLen = fpos - frets[c - 1];
                                isSustained = ints[i + 1] > ((fretLen / 192.0) * (double)(192 >> 2));  //(fretLen / 192.0 == bpmUnit
                                break;
                            }
                        }
                        //clip sustained notes to GH3 mode
                        if (isSustained)
                            ints[i + 1] = setSustain(ints[i], ints[i + 1], nextNote, oldSustainTrigger / 2, sustainTrigger / 2, this.Notes.Gh3SustainClipping);
                    }
                    int[] notes = ints;

                    //if this is a boss battle then remove notes that are not within the face off sections
                    if (this.IsBoss && faceOffItem != null)
                        ints = convertBossNotesToFaceOff(ints, ghi.Type == NotesType.Guitar ? faceOffP1Ints : faceOffP2Ints);
                    ints = adjustNotes(ints); //merge any notes that are really close together


                    //replace track notes
                    ar = (QbItemArray)qb.FindItem(ghi.SongNotesQbKey, false);
                    replaceQbItems(ar, ints, false);
                    setLength(ar, this.Length, 3, frets, false);

                    //Offset star power
                    ints = (int[])ghi.MappedFileItem.StarPower.Clone(); //don't modify original starpower
                    for (int i = 0; i < ints.Length; i += 3)
                        ints[i] += offset;
                    //if (faceOffItem != null)
                    //    alignBattleStarPowerToFaceOff(ints, ghi.Type == NotesType.Guitar ? faceOffP1Ints : faceOffP2Ints);
                    ints = adjustBattleStarPower(ints, notes);

                    //replace star power
                    ar = (QbItemArray)qb.FindItem(ghi.SongStarPowerQbKey, false);
                    replaceQbItems(ar, this.Notes.ForceNoStarPower ? new int[0] : ints, true); //optionally set to no star power
                    setLength(ar, this.Length, 3, frets, false);

                    int[] sp = ints;

                    //Offset star battle mode
                    ints = (int[])ghi.MappedFileItem.BattlePower.Clone();
                    for (int i = 0; i < ints.Length; i += 3)
                        ints[i] += offset;
                    //if (faceOffItem != null)
                    //    alignBattleStarPowerToFaceOff(ints, ghi.Type == NotesType.Guitar ? faceOffP1Ints : faceOffP2Ints);
                    ints = adjustBattleStarPower(ints, notes);

                    if (ints.Length == 0) //if no ints then use star power
                        ints = (int[])sp.Clone(); //just to ensure there's no issue with 2 references pointing at the same array

                    //replace star battle notes
                    ar = (QbItemArray)qb.FindItem(ghi.SongStarPowerBattleQbKey, false);
                    replaceQbItems(ar, ints, true);
                    setLength(ar, this.Length, 3, frets, false);



                }
                else
                {
                    ar = (QbItemArray)qb.FindItem(ghi.SongNotesQbKey, false);
                    clearQbItems(ar);
                    ar = (QbItemArray)qb.FindItem(ghi.SongStarPowerQbKey, false);
                    clearQbItems(ar);
                    ar = (QbItemArray)qb.FindItem(ghi.SongStarPowerBattleQbKey, false);
                    clearQbItems(ar);
                }
            }


            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_GuitarCoop_{1}", songName, NotesDifficulty.Easy.ToString())), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_GuitarCoop_{1}", songName, NotesDifficulty.Medium.ToString())), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_GuitarCoop_{1}", songName, NotesDifficulty.Hard.ToString())), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_GuitarCoop_{1}", songName, NotesDifficulty.Expert.ToString())), false);
            clearQbItems(ar);

            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_RhythmCoop_{1}", songName, NotesDifficulty.Easy.ToString())), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_RhythmCoop_{1}", songName, NotesDifficulty.Medium.ToString())), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_RhythmCoop_{1}", songName, NotesDifficulty.Hard.ToString())), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Song_RhythmCoop_{1}", songName, NotesDifficulty.Expert.ToString())), false);
            clearQbItems(ar);

            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_BossBattleP1", songName)), false);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_BossBattleP2", songName)), false);
            clearQbItems(ar);


            //start with expert guitar and work down the difficulties to find a GHItem with Face off, copy in to BOss Battle also

            if (faceOffItem != null)
            {
                QbItemArray fo1 = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_FaceOffP1", songName)), false);
                QbItemArray fo2 = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_FaceOffP2", songName)), false);
                if (faceOffItem.IsMapped && faceOffItem.MappedFileItem != null && faceOffItem.MappedFileItem.FaceOffP1Count + faceOffItem.MappedFileItem.FaceOffP2Count != 0)
                {
                    faceOffP1Ints = adjustFaceOff(faceOffP1Ints);
                    replaceQbItems(fo1, faceOffP1Ints, true, 2);

                    //ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_BossBattleP1", songName)), false);
                    //replaceQbItems(ar, faceOffP1Ints, true, 2);

                    faceOffP2Ints = adjustFaceOff(faceOffP2Ints);
                    replaceQbItems(fo2, faceOffP2Ints, true, 2);

                    //ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_BossBattleP2", songName)), false);
                    //replaceQbItems(ar, faceOffP2Ints, true, 2);
                }
                else
                {
                    clearQbItems(fo1);
                    clearQbItems(fo2);
                } 

            }

            
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_TimeSig", songName)), false);
            replaceQbItems(ar, timeSig, true);
            setLength(ar, this.Length, 3, frets, false);

            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_FretBars", songName)), false);
            replaceQbItems(ar, frets, false);

            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Markers", songName)), false);
            if (markers.Length != 0)
                setMarkers(frets, ar, qbText, markers);
            else
                calculateMarkers(frets, ar, qbText);
            


            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Scripts_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Anim_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Triggers_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Cameras_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Lightshow_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Crowd_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Drums_Notes", songName)), false);
            ////setLength(ar, this.Length, 3, frets, true);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Performance_Notes", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);

            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Scripts", songName)), false);
            if (this.SongQb.IsBoss) //set the death drain marker to the correct location
                setDeathDrain(ar, markers);
            else
                setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Anim", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Triggers", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Cameras", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Lightshow", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Crowd", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Drums", songName)), false);
            ////setLength(ar, this.Length, 3, frets, true);
            clearQbItems(ar);
            ar = (QbItemArray)qb.FindItem(QbKey.Create(string.Format("{0}_Performance", songName)), false);
            setLength(ar, this.Length, 3, frets, true);
            //clearQbItems(ar);

            qb.AlignPointers();
            qb.IsValid();
            qbText.AlignPointers();
            qbText.IsValid();

            long origPakLen = pak.FileLength;
            string tmpPak = string.Format("{0}_{1}", notesPak, Guid.NewGuid().ToString("N"));
            File.Copy(notesPak, tmpPak);

            if (_project.GameInfo.MidTextInQbPak)
            {
                //save markers to qbPak
                _project.FileManager.QbPakEditor.ReplaceFile(_project.GameInfo.GetNotesTextQbFilename(song), qbText);
            }
            else //save markers to mid pak
            {
                pak.ReplaceFile(_project.GameInfo.GetNotesTextQbFilename(song), qbText);
            }

            //replace notes to mid pak
            pak.ReplaceFile(_project.GameInfo.GetNotesQbFilename(song), qb);


            //pad notes pak with original file
            //add padding to ensure song works - This took over a month of testing to find!!
            //////////using (FileStream fs = new FileStream(notesPak, FileMode.Open, FileAccess.ReadWrite))
            //////////{

            //////////    using (FileStream fsI = new FileStream(tmpPak, FileMode.Open, FileAccess.Read))
            //////////    {
            //////////        fs.Seek(0, SeekOrigin.End);

            //////////        string tag = " -=> NANOOK <=-  PADDING BELOW  ";
            //////////        if (fs.Position + tag.Length < fsI.Length)
            //////////            fs.Write(Encoding.Default.GetBytes(tag), 0, tag.Length);

            //////////        if (fs.Position < fsI.Length)
            //////////        {
            //////////            fsI.Seek(fs.Position, SeekOrigin.Begin);
            //////////            copy(fsI, fs, fsI.Length - fsI.Position);
            //////////        }

            //////////    }
            //////////}

            FileHelper.Delete(tmpPak);

            return notesLength;
        }


        #region QB Editing

        private void setDeathDrain(QbItemArray ar, NotesMarker[] markers)
        {
            if (ar.Items.Count != 0 && ar.Items[0].QbItemType == QbItemType.ArrayStruct)
            {
                foreach (QbItemBase qib in ar.Items[0].Items)
                {
                    QbItemStruct hdr = qib as QbItemStruct;
                    if (qib != null && hdr.ItemCount == 2 && hdr.Items[0].QbItemType == QbItemType.StructItemInteger && hdr.Items[1].QbItemType == QbItemType.StructItemQbKey)
                    {
                        if (((QbItemQbKey)hdr.Items[1]).Values[0] == QbKey.Create("boss_battle_begin_deathlick"))
                            ((QbItemInteger)hdr.Items[0]).Values[0] = Math.Max((uint)this.Length - 11000, (uint)0); //10000 can be read from DeathLick in guitar_battle.qb
                    }
                }
            }
        }

        /// <summary>
        /// For boss battles, use the face off markers and remove notes that are not in the face off sections.
        /// </summary>
        /// <returns></returns>
        private int[] convertBossNotesToFaceOff(int[] notes, int[] faceOff)
        {
            List<int> n = new List<int>();
            int lastSec = 0;

            for (int i = 0; i < notes.Length; i += 3)
            {
                for (int j = lastSec; j < faceOff.Length; j += 2)
                {
                    //is the note within a face off section
                    if (notes[i] > faceOff[j] && notes[i] < faceOff[j] + faceOff[j + 1])
                    {
                        n.Add(notes[i]);
                        n.Add(notes[i + 1]);
                        n.Add(notes[i + 2]);
                        lastSec = j; //remember this section so we can resume from it
                        break;
                    }
                    else if (notes[i] < faceOff[j])
                        break; //we've gone too far
                }
            }

            return n.ToArray();
        }


        private int[] adjustNotes(int[] notes)
        {
            if (notes.Length == 0)
                return notes;

            List<int> n = new List<int>();


            for (int i = 0; i < notes.Length; i += 3)
            {
                if (n.Count > 3 && notes[i] - n[n.Count - 3] <= 6)
                {
                    //if the notes are very close together then it could be an editor issue, combine them
                    n[n.Count - 2] = notes[i + 1];  //the first note will have most likely been truncated if sustained
                    n[n.Count - 1] |= notes[i + 2]; //OR notes together
                }
                else if (n.Count > 3 && (notes[i] - n[n.Count - 3] < n[n.Count - 2])) 
                {
                    //if the length of the last note is longer then distance between them, then shorten the length
                    //this is only for non sustained notes (they are fixed in another area)
                    n[n.Count - 2] = (notes[i] - n[n.Count - 3]);

                    n.Add(notes[i]);
                    n.Add(notes[i + 1]);
                    n.Add(notes[i + 2]);
                }
                else
                {
                    n.Add(notes[i]);
                    n.Add(notes[i + 1]);
                    n.Add(notes[i + 2]);
                }
            }
            return n.ToArray();
        }


        private void copy(Stream from, Stream to, long length)
        {
            long l = 0;
            long size = 10000;
            int c = -1;
            byte[] b = new byte[size];
            while (c != 0 && l < length)
            {
                c = from.Read(b, 0, (int)(l + size < length ? b.Length : length - l));
                l += c;
                to.Write(b, 0, c);
            }

            //pad stream, if input stream was too short
            if (l < length)
            {
                for (int i = 0; i < size; i++)
                    b[i] = 0;

                while (l < length)
                {
                    if (length - l > size)
                        to.Write(b, 0, (int)size);
                    else
                        to.Write(b, 0, (int)(length - l));

                    l += size;
                }
            }
        }

        private int countNotes(int[] notes, int offset, int length)
        {
            int count = 0;
            for (int i = 0; i < notes.Length; i += 3)
            {
                if (notes[i] > offset + length)
                    break;
                else if (notes[i] >= offset)
                    count++;
            }

            return count;
        }

        //private void alignFaceOffToBattleStarPower(int[] faceOff, int[] battleStarPower)
        //{
        //    //remove empty sp
        //    List<int> sp = new List<int>();
        //    for (int i = 0; i < faceOff.Length; i += 2)
        //    {
        //        for (int j = 0; j < battleStarPower.Length; j += 3)
        //        {
        //            //if the section ends within a face off section but starts before it.
        //            if (faceOff[i] + faceOff[i + 1] >= battleStarPower[j] && faceOff[i] + faceOff[i + 1] < battleStarPower[j] + battleStarPower[j + 1] &&
        //                faceOff[i] < battleStarPower[j])
        //            {
        //                if (faceOff[i + 1] > battleStarPower[j + 1])
        //                    faceOff[i + 1] = battleStarPower[j + 1]; //set lengths the same

        //                faceOff[i] = battleStarPower[j]; //align the star power to the start of the face off section
        //            }

        //            //if the section starts within a face off section but ends after it.
        //            if (faceOff[i] >= battleStarPower[j] && faceOff[i] < battleStarPower[j] + battleStarPower[j + 1] &&
        //                faceOff[i] + faceOff[i + 1] > battleStarPower[j] + battleStarPower[j + 1])
        //            {
        //                if (faceOff[i + 1] > battleStarPower[j + 1])
        //                    faceOff[i + 1] = battleStarPower[j + 1]; //set lengths the same

        //                faceOff[i] = (battleStarPower[j] + battleStarPower[j + 1]) - faceOff[i + 1]; //move section to the end of the face off section
        //            }

        //        }
        //    }
        //}
        /// <summary>
        /// Move the star power to fit in with the face off.  We can't move the face off as there's only one, but each star power could be different
        /// </summary>
        /// <param name="battleStarPower"></param>
        /// <param name="faceOffP1"></param>
        /// <param name="faceOffP2"></param>
        private void alignBattleStarPowerToFaceOff(int[] battleStarPower, int[] faceOff)
        {
            //remove empty sp
            List<int> sp = new List<int>();
            for (int i = 0; i < battleStarPower.Length; i += 3)
            {
                for (int j = 0; j < faceOff.Length; j += 2)
                {
                    //if the section ends within a face off section but starts before it.
                    if (battleStarPower[i] + battleStarPower[i + 1] >= faceOff[j] && battleStarPower[i] + battleStarPower[i + 1] < faceOff[j] + faceOff[j + 1] && 
                        battleStarPower[i] < faceOff[j])
                    {
                        if (battleStarPower[i + 1] > faceOff[j + 1])
                            battleStarPower[i + 1] = faceOff[j + 1]; //set lengths the same

                        battleStarPower[i] = faceOff[j]; //align the star power to the start of the face off section
                    }

                    //if the section starts within a face off section but ends after it.
                    if (battleStarPower[i] >= faceOff[j] && battleStarPower[i] < faceOff[j] + faceOff[j + 1] &&
                        battleStarPower[i] + battleStarPower[i + 1] > faceOff[j] + faceOff[j + 1])
                    {
                        if (battleStarPower[i + 1] > faceOff[j + 1])
                            battleStarPower[i + 1] = faceOff[j + 1]; //set lengths the same

                        battleStarPower[i] = (faceOff[j] + faceOff[j + 1]) - battleStarPower[i + 1]; //move section to the end of the face off section
                    }

                }
            }
        }

        /// <summary>
        /// Remove empty star power / battle power sections. Remove overlapping sections. Ensure note count is correct
        /// </summary>
        private int[] adjustBattleStarPower(int[] battleStarPower, int[] notes)
        {
            int c;
           
            List<int> sec = new List<int>();
            for (int i = 0; i < battleStarPower.Length; i += 3)
            {
                c = countNotes(notes, battleStarPower[i], battleStarPower[i + 1]);

                if (c != 0 && battleStarPower[i] + battleStarPower[i + 1] < this.Length)
                {
                    //if this section overlaps with the last section then skip this section
                    if (i == 0 || (i >= 3 && battleStarPower[i - 3] + battleStarPower[(i - 3) + 1] < battleStarPower[i]))
                    {
                        sec.Add(battleStarPower[i]);
                        sec.Add(battleStarPower[i + 1]);
                        sec.Add(c);
                    }
                }
            }

            return sec.ToArray();
        }

        /// <summary>
        /// Combine overlapping sections
        /// </summary>
        private int[] adjustFaceOff(int[] faceOff)
        {
            List<int> fo = new List<int>();
            for (int i = 0; i < faceOff.Length; i += 2)
            {
                //if this section overlaps with the last section then combine them
                if (fo.Count >= 2 && faceOff[i - 2] + faceOff[(i - 2) + 1] >= faceOff[i])
                {
                    fo[fo.Count - 1] = (faceOff[i] + faceOff[i + 1]) - fo[fo.Count - 2]; //combine sections
                }
                else
                {
                    fo.Add(faceOff[i]);
                    fo.Add(faceOff[i + 1]);
                }
            }

            return fo.ToArray();
        }


        /// <summary>
        /// Remove frets before notes start that are close together. Create frets from start to Notes. Add / Remove frets from end
        /// </summary>
        /// <param name="frets"></param>
        /// <returns>Padding to allow a properly spaced fret to be at position 0</returns>
        private int[] adjustFrets(int[] frets)
        {
            int fretPadding = 0;
            List<int> f = new List<int>(frets);

            //remove frets < first note (sometimes there's a padding note at 0 which is not accurate)
            while (f[0] < this.Notes.MinNoteOffsetSynced + _startPaddingMs)
                f.RemoveAt(0);

            //add frets from 0 to start of frets
            int fretLen = 0; //first fret length
            if (f.Count > 1 && f[1] - f[0] < f[0])
            {
                fretLen = f[1] - f[0];
                int x = f[0] - fretLen;

                do
                {
                    f.Insert(0, x);
                    x -= fretLen;
                }
                while (x > 0);
            }

            //do we need to pad the frets to get a position of 0 (as in, move frets back so that we can have a natural fret at time 0)
            int maxNote = this.Notes.MaxNoteOffsetSynced + _startPaddingMs;

            //HACK - NOTESONLY
            //int maxNote = this.Length - 1500;

            if (f.Count > 1)
            {
                int l = f[f.Count - 1] - f[f.Count - 2];
                while (f[f.Count - 1] < maxNote)
                    f.Add(f[f.Count - 1] + l);
            }

            //remove frets that are after the end of the audio but at least 1 after the last note and length
            while (f.Count != 0 && f[f.Count - 1] > this.Length && f[f.Count - 2] > maxNote)
                f.RemoveAt(f.Count - 1);

            //do we need to pad the frets to get a position of 0
            if (f.Count > 1 && f[0] != 0)
            {
                fretPadding = fretLen - f[0];
                for (int i = 0; i < f.Count; i++)
                    f[i] += fretPadding;
                f.Insert(0, 0); //insert 0 fret
            }

            _fretPadding = fretPadding;
            
            return f.ToArray();
        }

        private int setSustain(int susNote, int susLen, int nextNote, int halfOldSustainTrigger, int halfNewSustainTrigger, bool gh3Mode)
        {
            //if ((susLen - halfSustainTrigger) > nextNote - susNote - halfSustainTrigger)
            //    return (nextNote - susNote - halfSustainTrigger) + halfSustainTrigger;

            int crop = (gh3Mode ? 100 : 0);

            susLen = (susLen - halfOldSustainTrigger) + halfNewSustainTrigger; //convert to new length

            if (nextNote != 0 && (susLen - halfNewSustainTrigger) > nextNote - susNote - crop)
                return (nextNote - susNote - crop) + halfNewSustainTrigger;
            else
                return susLen;
        }


        private void setLength(QbItemArray a, int msSongLength, int blockSize, int[] frets, bool bindToFret)
        {
            uint val;
            uint lastVal = uint.MaxValue;

            if (a.ItemCount != 0)
            {
                if (a.Items[0].QbItemType == QbItemType.Floats)
                {
                    //nothing to edit
                }
                else if (a.Items[0].QbItemType == QbItemType.ArrayArray)
                {
                    QbItemArray arr = (QbItemArray)a.Items[0];
                    int idx = arr.Items.Count - 1;
                    while (idx >= 0)
                    {
                        val = ((QbItemInteger)arr.Items[idx]).Values[0];
                        if (bindToFret)
                            val = findNearestFret(val, frets);

                        if (val == lastVal || val < 0 || val > msSongLength)
                            arr.RemoveItem(arr.Items[idx]);
                        else if (bindToFret)
                            ((QbItemInteger)arr.Items[idx]).Values[0] = val;

                        if (bindToFret)
                            lastVal = val;

                        idx--;
                    }
                }
                else if (a.Items[0].QbItemType == QbItemType.ArrayStruct)
                {
                    QbItemStructArray arr = (QbItemStructArray)a.Items[0];
                    int idx = arr.Items.Count - 1;
                    while (idx >= 0)
                    {
                        val = ((QbItemInteger)((QbItemStruct)arr.Items[idx]).Items[0]).Values[0];
                        if (bindToFret)
                            val = findNearestFret(val, frets);

                        if (val == lastVal || val < 0 || val > msSongLength)
                            arr.RemoveItem(arr.Items[idx]);
                        else if (bindToFret)
                            ((QbItemInteger)((QbItemStruct)arr.Items[idx]).Items[0]).Values[0] = val;

                        if (bindToFret)
                            lastVal = val;
                        idx--;
                    }
                }
                else
                {
                    int start = 0;
                    uint[] arr = ((QbItemInteger)a.Items[0]).Values;
                    int idx = arr.Length - blockSize;
                    while (start < arr.Length)
                    {
                        if (arr[start] >= 0)
                            break;
                        idx += blockSize;
                    }


                    while (idx >= 0)
                    {
                        if (arr[idx] < msSongLength)
                            break;
                        idx -= blockSize;
                    }
                    uint[] arr2 = new uint[(idx + blockSize) - start];
                    Array.Copy(arr, start, arr2, 0, arr2.Length - start);
                    ((QbItemInteger)a.Items[0]).Values = arr2;
                }
            }

            a.Root.AlignPointers();
        }

        private void clearQbItems(QbItemArray a)
        {
            replaceQbItems(a, new int[0], false);
        }

        private void clearAllQbItems(QbFile qbFile, List<GhNotesItem> ghItems, params QbKey[] skipItems)
        {
            bool found;
            uint crc;
            foreach (QbItemBase qib in qbFile.Items)
            {
                if (!(qib is QbItemArray))
                    continue; //skip non array items

                found = false;
                crc = qib.ItemQbKey.Crc;

                foreach (GhNotesItem gii in ghItems)
                {
                    if (gii.SongNotesQbKey.Crc == crc || gii.SongStarPowerQbKey.Crc == crc || gii.SongStarPowerBattleQbKey.Crc == crc)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    foreach (QbKey qbk in skipItems)
                    {
                        if (qbk.Crc == crc)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    if (qib.Items[0].QbItemType != QbItemType.Floats)
                        clearQbItems((QbItemArray)qib);
                }
            }
        }

        private void replaceQbItems(QbItemArray a, int[] items, bool split)
        {
            replaceQbItems(a, items, split, 3);
        }

        private void replaceQbItems(QbItemArray a, int[] items, bool split, int splitBy)
        {
            a.Items.Clear();

            //no items to add
            if (items.Length == 0)
            {
                QbItemFloats f = new QbItemFloats(a.Root);
                f.Create(QbItemType.Floats);
                a.AddItem(f);
                a.Root.AlignPointers();
            }
            else
            {
                if (!split)
                {
                    //add array to parent as one large array
                    QbItemInteger qi = new QbItemInteger(a.Root);
                    qi.Create(QbItemType.ArrayInteger);
                    qi.Values = convertIntArrayToUIntArray(items);
                    a.AddItem(qi);
                    a.Root.AlignPointers();
                }
                else
                {
                    //split array down in to blocks of <splitBy>
                    QbItemInteger qi;
                    QbItemArray aa = new QbItemArray(a.Root);
                    aa.Create(QbItemType.ArrayArray);
                    uint[] uints;
                    a.AddItem(aa);
                    a.Root.AlignPointers();

                    for (int i = 0; i < items.Length; i += splitBy)
                    {
                        qi = new QbItemInteger(a.Root);
                        qi.Create(QbItemType.ArrayInteger);
                        uints = new uint[splitBy];
                        for (int j = 0; j < splitBy; j++)
                            uints[j] = (uint)items[i + j];
                        qi.Values = uints;
                        aa.AddItem(qi);
                        a.Root.AlignPointers();
                    }

                }
            }
        }

        private uint[] convertIntArrayToUIntArray(int[] array)
        {
            uint[] outArray = new uint[array.Length];
            for (int i = 0; i < array.Length; i++)
                outArray[i] = (uint)array[i];

            return outArray;
        }

        private uint findNearestFret(uint offset, int[] frets)
        {
            uint o = 0;
            for (int i = 0; i < frets.Length; i++)
            {
                if ((uint)frets[i] <= offset)
                    o = (uint)frets[i];
                else
                    break;
            }

            return o;
        }

        private void setMarkers(int[] frets, QbItemArray arr, QbFile text, NotesMarker[] markers)
        {
            int minNote = this.Notes.MinNoteOffsetSynced + _startPaddingMs + _fretPadding;
            QbItemStruct s;
            QbItemInteger i;
            QbItemQbKey q;
            QbKey qbKey;
            QbItemString txt;

            for (int c = text.Items.Count - 1; c > 0; c--)
                text.RemoveItem(text.Items[c]);


            if (arr.Items[0] is QbItemFloats)
            {
                QbItemStructArray newArr = new QbItemStructArray(arr.Root);
                newArr.Create(QbItemType.ArrayStruct);
                arr.AddItem(newArr);
                arr.RemoveItem(arr.Items[0]);
            }

            QbItemStructArray sa = (QbItemStructArray)arr.Items[0];
            sa.Items.Clear();

            NotesMarker marker;

            List<NotesMarker> mrk = new List<NotesMarker>(markers);

            if (mrk.Count > 0 && mrk[0].Offset > minNote)  //some charts don't have sections at the start so you can't practice the start notes :-(
            {
                if (mrk[0].Offset > minNote + 5000) // if > 5secs then add new
                    mrk.Insert(0, new NotesMarker("Start", minNote));
                else //else move first marker back to start
                    mrk[0].Offset = minNote;
            }


            for (int c = 0; c < mrk.Count; c++)
            {
                marker = mrk[c];
                if (c < mrk.Count - 1 && mrk[c + 1].Offset < minNote)
                    continue; //don't add sections at the start that would have no notes (crashes song??)

                qbKey = QbKey.Create(string.Format("{0}_markers_text_{1}", this.SongQb.Id, QbKey.Create(marker.Title).Crc.ToString("x").ToLower()));

                txt = new QbItemString(text);
                txt.Create(QbItemType.SectionString);
                txt.ItemQbKey = qbKey;
                txt.Strings = new string[] { marker.Title };
                text.AddItem(txt);

                s = new QbItemStruct(arr.Root);
                s.Create(QbItemType.StructHeader);
                sa.AddItem(s);

                i = new QbItemInteger(arr.Root);
                i.Create(QbItemType.StructItemInteger);
                i.ItemQbKey = QbKey.Create("time");
                i.Values = new uint[] { findNearestFret((uint)marker.Offset, frets) };
                s.AddItem(i);

                q = new QbItemQbKey(arr.Root);
                q.Create(QbItemType.StructItemQbKeyString);
                q.ItemQbKey = QbKey.Create("marker");
                q.Values = new QbKey[] { qbKey };
                s.AddItem(q);
            }
            text.AlignPointers();
            arr.Root.AlignPointers();
        }

        private void calculateMarkers(int[] frets, QbItemArray arr, QbFile text)
        {
            int minNote = this.Notes.MinNoteOffsetSynced + _startPaddingMs + _fretPadding;
            int maxNote = this.Notes.MaxNoteOffsetSynced + _startPaddingMs + _fretPadding;

            int pos = minNote;

            int sectionSecs = 20000;
            int sections = (maxNote - minNote) / sectionSecs; //every x seconds

            for (int c = text.Items.Count - 1; c > 0; c--)
                text.RemoveItem(text.Items[c]);

            QbItemStructArray sa = new QbItemStructArray(arr.Root);
            sa.Create(QbItemType.ArrayStruct);

            arr.Items.Clear();
            arr.AddItem(sa);

            QbItemStruct s;
            QbItemInteger i;
            QbItemQbKey q;
            string sectionTitle;
            QbKey qbKey;
            QbItemString txt;

            for (int c = 0; c < sections; c++)
            {
                if (pos + 3000 > this.Length)
                    break; //don't create a section less than 3 seconds long

                sectionTitle = string.Format("Section {0}", (c + 1).ToString());
                qbKey = QbKey.Create(string.Format("{0}_markers_text_{1}", this.SongQb.Id, QbKey.Create(sectionTitle).Crc.ToString("x").ToLower()));

                txt = new QbItemString(text);
                txt.Create(QbItemType.SectionString);
                txt.ItemQbKey = qbKey;
                txt.Strings = new string[] { sectionTitle };
                text.AddItem(txt);

                s = new QbItemStruct(arr.Root);
                s.Create(QbItemType.StructHeader);
                sa.AddItem(s);

                i = new QbItemInteger(arr.Root);
                i.Create(QbItemType.StructItemInteger);
                i.ItemQbKey = QbKey.Create("time");
                i.Values = new uint[] { findNearestFret((uint)pos, frets) };
                s.AddItem(i);

                pos += sectionSecs;

                q = new QbItemQbKey(arr.Root);
                q.Create(QbItemType.StructItemQbKeyString);
                q.ItemQbKey = QbKey.Create("marker");
                q.Values = new QbKey[] { qbKey };
                s.AddItem(q);
            }

            text.AlignPointers();
            arr.Root.AlignPointers();
        }

        #endregion
        #endregion

        #region ISettingsChange Members

        /// <summary>
        /// Gets the lates LastChanged date from QbSettings, Notes and Audio, Sets LastChanged to Audio, Notes and QbSettings
        /// </summary>
        public DateTime LastChanged
        {
            get
            {
                DateTime dt = this.QbLastChanged;
                if (this.Audio.LastChanged > dt)
                    dt = this.Audio.LastChanged;
                if (this.Notes.LastChanged > dt)
                    dt = this.Notes.LastChanged;
                return dt;
            }
            set
            {
                if (_recordChange)
                    this.QbLastChanged = this.Audio.LastChanged = this.Notes.LastChanged = value;
            }
        }

        void ISettingsChange.RecordChange()
        {
            _recordChange = true;
            ((ISettingsChange)this.Audio).RecordChange();
            ((ISettingsChange)this.Notes).RecordChange();
            
        }

        #endregion

        public DateTime QbLastChanged
        {
            get
            {
                //bool b = this.qbChanged; //will set _qbLastChanged
                return _qbLastChanged;
            }
            set
            {
                if (_recordChange)
                    _qbLastChanged = value;
            }
        }

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

        private string _title;
        private string _artist;
        private string _year;
        private float _guitarVolume;
        private float _songVolume;
        private Singer _singer;
        private bool _originalArtist;
        private int _length;
        private SongQb _songQb;
        private int _minMsBeforeNotesStart;
        private int _startPaddingMs;

        private int _fretPadding;

        private bool _recordChange;
        private DateTime _lastChanged;
        private DateTime _qbLastChanged;
        private DateTime _lastApplied;

        private ProjectSongAudio _audio;
        private ProjectSongNotes _notes;
        private Project _project;


    }
}
