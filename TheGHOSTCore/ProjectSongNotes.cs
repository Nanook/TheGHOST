using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public delegate void NotesFileChangedEventHandler(object source, NotesFileChangedEventArgs e);
    public delegate void GhItemMapChangedEventHandler(object source, GhItemMapChangedEventArgs e);

    public class ProjectSongNotes : ISettingsChange
    {
        public event NotesFileChangedEventHandler NotesFileChanged;
        public event GhItemMapChangedEventHandler GhItemMapChanged;

        internal ProjectSongNotes(Project project, ProjectSong song)
        {
            _lastChanged = DateTime.MinValue;
            _recordChange = false;

            _project = project;
            _song = song;
            _baseFile = null;
            _updateAffectsAudio = false;

            string songName = _song.SongQb.Id.Text;

            _ghItems = new List<GhNotesItem>();
            _files = new List<NotesFile>();

            _ghItems.Add(new GhNotesItem("Easy", NotesType.Guitar, NotesDifficulty.Easy, songName));
            _ghItems.Add(new GhNotesItem("Medium", NotesType.Guitar, NotesDifficulty.Medium, songName));
            _ghItems.Add(new GhNotesItem("Hard", NotesType.Guitar, NotesDifficulty.Hard, songName));
            _ghItems.Add(new GhNotesItem("Expert", NotesType.Guitar, NotesDifficulty.Expert, songName));
            _ghItems.Add(new GhNotesItem("Easy Rhythm", NotesType.Rhythm, NotesDifficulty.Easy, songName));
            _ghItems.Add(new GhNotesItem("Medium Rhythm", NotesType.Rhythm, NotesDifficulty.Medium, songName));
            _ghItems.Add(new GhNotesItem("Hard Rhythm", NotesType.Rhythm, NotesDifficulty.Hard, songName));
            _ghItems.Add(new GhNotesItem("Expert Rhythm", NotesType.Rhythm, NotesDifficulty.Expert, songName));
            //_ghItems.Add(new GhNotesItem("Easy Co Op", NotesType.GuitarCoop, NotesDifficulty.Easy, songName));
            //_ghItems.Add(new GhNotesItem("Medium Co Op", NotesType.GuitarCoop, NotesDifficulty.Medium, songName));
            //_ghItems.Add(new GhNotesItem("Hard Co Op", NotesType.GuitarCoop, NotesDifficulty.Hard, songName));
            //_ghItems.Add(new GhNotesItem("Expert Co Op", NotesType.GuitarCoop, NotesDifficulty.Expert, songName));
            //_ghItems.Add(new GhNotesItem("Easy Rhythm Co Op", NotesType.RhythmCoop, NotesDifficulty.Easy, songName));
            //_ghItems.Add(new GhNotesItem("Medium Rhythm Co Op", NotesType.RhythmCoop, NotesDifficulty.Medium, songName));
            //_ghItems.Add(new GhNotesItem("Hard Rhythm Co Op", NotesType.RhythmCoop, NotesDifficulty.Hard, songName));
            //_ghItems.Add(new GhNotesItem("Expert Rhythm Co Op", NotesType.RhythmCoop, NotesDifficulty.Expert, songName));

        }


        public NotesFile ParseFile(string filename)
        {
            bool alreadyAdded = false;

            if (_files.Exists(delegate(NotesFile nfl)
            {
                return (nfl.Filename.ToLower() == filename.ToLower());
            }))
            {
                alreadyAdded = true;
            }

            FileInfo fi = new FileInfo(filename);

            INotesParser np = null;
            NotesFile nf = null;

            if (fi.Extension.ToLower() == ".chart")
                np = new Chart();
            else if (fi.Extension.ToLower() == ".mid")
                np = new Mid();

            if (np != null)
            {
                nf = new NotesFile(np, fi.FullName, _song.Length, _project.Defaults.Gh3SustainClipping);
                if (!alreadyAdded)
                    this.AddFile(nf);

                //set the base file
                if (_baseFile == null)
                    _baseFile = nf;

                this.LastChanged = DateTime.Now;

                OnNotesFileChanged(nf, alreadyAdded ? NotesFileChangeType.Reloaded : NotesFileChangeType.Added);

                return nf;
            }
            else
                throw new ApplicationException("Unrecognised notes format");
        }

        protected void OnNotesFileChanged(NotesFile nf, NotesFileChangeType type)
        {
            if (NotesFileChanged != null)
                NotesFileChanged(this, new NotesFileChangedEventArgs(nf, type));
        }

        public void ReloadFile(NotesFile file)
        {
            foreach (GhNotesItem gi in this.GhItems)
            {
                if (gi.IsMapped && gi.MappedFile == file)
                    this.UnMapGhItem(gi);
            }

            for (int i = 0; i < _files.Count; i++)
            {
                if (_files[i] == file)
                {
                    file = this.ParseFile(file.Filename);
                    _files[i] = file;
                    break;
                }
            }


        }

        public void SmartMap(bool exactMatch)
        {
            foreach (NotesFile nf in this.Files)
                this.SmartMap(nf, exactMatch);
        }

        public void SmartMap(NotesFile nf, bool exactMatch)
        {
            int idx = 0;

            foreach (GhNotesItem ghi in _ghItems)
            {
                if (!findMatch(nf, ghi, ghi.Type, ghi.Difficulty) && !exactMatch)
                {
                    if (ghi.Type == NotesType.RhythmCoop)
                        findMatch(nf, ghi, NotesType.Rhythm, ghi.Difficulty);

                    if (!ghi.IsMapped)
                        findMatch(nf, ghi, NotesType.Guitar, ghi.Difficulty);

                    if (!ghi.IsMapped)
                        findMatch(nf, ghi, NotesType.Rhythm, ghi.Difficulty);
                }
                idx++;
            }
        }

        public bool AllFilesExist
        {
            get
            {
                foreach (NotesFile f in _files)
                {
                    if (!File.Exists(f.Filename))
                        return false;
                }
                return true;
            }
        }

        private GhNotesItem findMappedGhItemType(NotesType type)
        {
            GhNotesItem item = null;
            foreach (GhNotesItem ghi in _ghItems)
            {
                if (type == ghi.Type && ghi.IsMapped && !ghi.MappedFileItem.HasGeneratedNotes && (item == null || item != null && (int)(item.Difficulty) < (int)(ghi.Difficulty)))
                    item = ghi;
            }
            return item;
        }

        public GhNotesItem SourceGenerationItem(NotesType type)
        {
            GhNotesItem source = null;
            source = findMappedGhItemType(type);
            if (source == null && type != NotesType.Guitar)
                source = findMappedGhItemType(NotesType.Guitar); //if not found then resort to guitar

            if (source == null)
                source = findMappedGhItemType(NotesType.Rhythm);

            if (source == null)
                source = findMappedGhItemType(NotesType.GuitarCoop);

            if (source == null)
                source = findMappedGhItemType(NotesType.RhythmCoop);

            return source;
        }

        public GhNotesItem SourceGenerationItem(NotesType type, NotesDifficulty difficulty)
        {
            foreach (GhNotesItem ghi in _ghItems)
            {
                if (type == ghi.Type && difficulty == ghi.Difficulty)
                    return ghi;
            }
            if (type != NotesType.Guitar)
                return SourceGenerationItem(NotesType.Guitar, difficulty);

            return null;
        }

        public bool GenerateNotes(GhNotesItem ghi, bool force, bool replaceExistingSp, bool replaceExistingBattle, bool replaceExistingFaceOff, bool replaceGeneratedItems)
        {
            GhNotesItem source = null;

            if (!ghi.IsMapped || force)
            {
                //attempt to find a set of notes from the same notes type (Guitar / Rhythm etc)
                source = SourceGenerationItem(ghi.Type);

                if (source != null && source != ghi)
                {
                    if (source.IsMapped && source.MappedFileItem.StarPowerCount <= 1)
                        source.MappedFileItem.GenerateStarPower(this.BaseFile.Frets, true, null);

                    ghi.GenerateNotes(this.BaseFile, source);
                    ghi.MappedFileItem.SyncOffset = source.MappedFileItem.SyncOffset;

                    this.OnGhItemMapChanged(source.MappedFile, ghi.MappedFileItem.UniqueId, ghi, true, true);
                }
                else
                    return false;
            }
            else if (ghi.MappedFileItem.StarPowerCount <= 1 || replaceExistingSp ||
                (ghi.MappedFileItem.HasGeneratedStarPower && replaceGeneratedItems))
            {
                source = SourceGenerationItem(ghi.Type, NotesDifficulty.Expert);
                if (!source.IsMapped || (ghi.MappedFileItem.StarPowerCount <= 1 && ghi.Type != NotesType.Guitar))
                    source = SourceGenerationItem(NotesType.Guitar, NotesDifficulty.Expert);

                if (!source.IsMapped)
                    this.GenerateNotes(source, true, true, true, true, true);
                if (source.MappedFileItem.StarPowerCount <= 1)
                    source.MappedFileItem.GenerateStarPower(this.BaseFile.Frets, true, null);

                ghi.MappedFileItem.GenerateStarPower(this.BaseFile.Frets, false, source.MappedFileItem.StarPower);
            }


            if (ghi.MappedFileItem.BattlePowerCount <= 1 || replaceExistingBattle ||
                (ghi.MappedFileItem.HasGeneratedBattlePower && replaceGeneratedItems))
            {
                source = SourceGenerationItem(ghi.Type, NotesDifficulty.Expert);
                if (!source.IsMapped || (ghi.MappedFileItem.BattlePowerCount <= 1 && ghi.Type != NotesType.Guitar))
                    source = SourceGenerationItem(NotesType.Guitar, NotesDifficulty.Expert);

                if (!source.IsMapped)
                    this.GenerateNotes(source, true, true, true, true, true);
                if (source.MappedFileItem.BattlePowerCount <= 1)
                    source.MappedFileItem.GenerateBattlePower(this.BaseFile.Frets, source.MappedFileItem.StarPowerCount <= 1 ? null : source.MappedFileItem.StarPower);

                ghi.MappedFileItem.GenerateBattlePower(this.BaseFile.Frets, source.MappedFileItem.StarPower);
            }


            if (ghi.MappedFileItem.FaceOffP1Count <= 1 || ghi.MappedFileItem.FaceOffP2Count <= 1 || replaceExistingFaceOff ||
                (ghi.MappedFileItem.HasGeneratedFaceOff && replaceGeneratedItems))
            {
                source = SourceGenerationItem(ghi.Type, NotesDifficulty.Expert);
                if (!source.IsMapped || ((source.MappedFileItem.FaceOffP1Count <= 1 || source.MappedFileItem.FaceOffP2Count <= 1) && ghi.Type != NotesType.Guitar))
                    source = SourceGenerationItem(NotesType.Guitar, NotesDifficulty.Expert);

                if (!source.IsMapped)
                    this.GenerateNotes(source, true, true, true, true, true);
                if (source.MappedFileItem.FaceOffP1Count <= 1 || source.MappedFileItem.FaceOffP2Count <= 1)
                    source.MappedFileItem.GenerateFaceOff(this.BaseFile.Frets, null, null, source.MappedFileItem.StarPower);

                ghi.MappedFileItem.GenerateFaceOff(this.BaseFile.Frets, source.MappedFileItem.FaceOffP1, source.MappedFileItem.FaceOffP2, source.MappedFileItem.StarPower);
            }

            return true;
        }

        public void GenerateItems(bool replaceExistingSp, bool replaceExistingBattle, bool replaceExistingFaceOff, bool replaceGeneratedItems)
        {
            foreach (GhNotesItem ghi in _ghItems)
                this.GenerateNotes(ghi, false, replaceExistingSp, replaceExistingBattle, replaceExistingFaceOff, replaceGeneratedItems);
        }

        private bool findMatch(NotesFile nf, GhNotesItem ghi, NotesType type, NotesDifficulty difficulty)
        {
            string match;
            if (nf.Parser != null)
            {
                match = nf.Parser.MatchType(type, difficulty);
                if (match.Length != 0)
                {
                    int j = 0;
                    foreach (NotesFileItem nfi in nf.Items)
                    {
                        if (!ghi.IsMapped && nfi.SourceName == match && nfi.Notes.Length > 1 * 3)
                        {
                            this.MapToGhItem(nf, nfi.UniqueId, ghi, false);
                            return true;
                        }
                        j++;
                    }
                }
            }
            return false;
        }



        /// <summary>
        /// Map the missing GH3 items to other difficulties
        /// </summary>
        //public void AutoMapMissing()
        //{
        //    foreach (GhNotesItem ghi in this.GhItems)
        //    {
        //        if (!ghi.IsMapped)
        //        {
        //            foreach (NotesFile nf in this.Files)
        //            {
        //                //loop up to expert
        //                for (int i = (int)ghi.Difficulty; i <= (int)NotesDifficulty.Expert; i++)
        //                    findMatch(nf, ghi, (NotesDifficulty)i);

        //                if (ghi.IsMapped)
        //                    break;

        //                //loop down to easy
        //                for (int i = ((int)ghi.Difficulty) - 1; i >= (int)NotesDifficulty.Easy; i--)
        //                    findMatch(nf, ghi, (NotesDifficulty)i);

        //                if (ghi.IsMapped)
        //                    break;
        //            }

        //            if (ghi.IsMapped)
        //                continue;
        //        }
        //    }
        //}

        public void MapToGhItem(NotesFile fromFile, uint uniqueId, GhNotesItem toItem, bool isGenerated)
        {
            toItem.IsMapped = true;
            toItem.MappedFile = fromFile;
            toItem.MappedFileItem = fromFile.FindItem(uniqueId);

            this.LastChanged = DateTime.Now;
            this.UpdateAffectsAudio = true;

            //Raise an event
            this.OnGhItemMapChanged(fromFile, uniqueId, toItem, isGenerated, false);
        }

        protected void OnGhItemMapChanged(NotesFile fromFile, uint uniqueId, GhNotesItem toItem, bool isGenerated, bool addedRemoved)
        {
            if (GhItemMapChanged != null)
                GhItemMapChanged(this, new GhItemMapChangedEventArgs(fromFile, uniqueId, toItem, isGenerated, addedRemoved));

            this.LastChanged = DateTime.Now;
            this.UpdateAffectsAudio = true;
        }

        public void UnMapGhItem(GhNotesItem ghItem)
        {
            if (ghItem == null)
                return;

            bool gen = ghItem.MappedFileItem.HasGeneratedNotes;

            NotesFile nf = ghItem.MappedFile;
            uint id = 0; //unmap generated
            if (!gen)
                id = ghItem.MappedFileItem.UniqueId;

            ghItem.IsMapped = false;
            ghItem.MappedFile = null;
            ghItem.MappedFileItem = null;
            this.LastChanged = DateTime.Now;

            //Raise an event
            this.OnGhItemMapChanged(nf, id, ghItem, gen, false);
        }


        /// <summary>
        /// This is the file that will be used for markers and frets etc
        /// </summary>
        public NotesFile BaseFile
        {
            get { return _baseFile; }
            set
            {
                if (_baseFile == value)
                    return;
                _baseFile = value;
                this.LastChanged = DateTime.Now;
            }
        }

        public float HoPoMeasure
        {
            get { return _hopoMeasure; }
            set
            {
                if (_hopoMeasure == value)
                    return;
                _hopoMeasure = value;
                this.LastChanged = DateTime.Now;
            }
        
        }

        public bool Gh3SustainClipping
        {
            get { return _gh3SustainClipping; }
            set
            {
                if (_gh3SustainClipping == value)
                    return;
                _gh3SustainClipping = value;
                this.LastChanged = DateTime.Now;
            }

        }

        public bool ForceNoStarPower
        {
            get { return _forceNoStarPower; }
            set
            {
                if (_forceNoStarPower == value)
                    return;
                _forceNoStarPower = value;
                this.LastChanged = DateTime.Now;
            }

        }

        public void AddFile(NotesFile file)
        {
            _files.Add(file);
        }

        public void RemoveFile(NotesFile file)
        {
            foreach(GhNotesItem gi in this.GhItems)
            {
                if (gi.IsMapped && gi.MappedFile == file)
                    this.UnMapGhItem(gi);
            }
            _files.Remove(file);

            if (file == _song.Notes.BaseFile)
            {
                if (_song.Notes.Files.Length == 0)
                    _song.Notes.BaseFile = null;
                else
                    _song.Notes.BaseFile = _song.Notes.Files[0];
            }

            OnNotesFileChanged(file, NotesFileChangeType.Removed);
        }

        public NotesFile[] Files
        {
            get { return _files.ToArray(); }
        }

        public GhNotesItem[] GhItems
        {
            get { return _ghItems.ToArray(); }
        }

        public int MinNoteOffsetSynced
        {
            get
            {
                int minNote = int.MaxValue;
                foreach (GhNotesItem gii in this.GhItems)
                {
                    if (gii.IsMapped)
                    {
                        if (minNote > gii.MappedFileItem.Notes[0] + gii.MappedFileItem.SyncOffset)
                            minNote = gii.MappedFileItem.Notes[0] + gii.MappedFileItem.SyncOffset;
                    }
                }
                if (minNote != int.MaxValue)
                    return minNote;
                else
                    return 0;
            }
        }


        public int MaxNoteOffsetSynced
        {
            get
            {
                int maxNote = int.MinValue;
                int v;
                int l;
                foreach (GhNotesItem gii in this.GhItems)
                {
                    if (gii.IsMapped && gii.MappedFileItem.Notes.Length >= 3)
                    {
                        v = gii.MappedFileItem.Notes[gii.MappedFileItem.Notes.Length - 3];
                        l = gii.MappedFileItem.Notes[gii.MappedFileItem.Notes.Length - 2];
                        if (maxNote < v + gii.MappedFileItem.SyncOffset + l)
                            maxNote = v + gii.MappedFileItem.SyncOffset + l;
                    }
                }
                if (maxNote != int.MinValue)
                    return maxNote;
                else
                    return 0;
            }
        }

        #region ISettingsChange Members

        public DateTime LastChanged
        {
            get
            {
                DateTime dt = _lastChanged;
                foreach (GhNotesItem ghi in _ghItems)
                {
                    if (ghi.IsMapped && ghi.MappedFileItem.LastChanged > dt)
                    {
                        dt = ghi.MappedFileItem.LastChanged;
                        this.UpdateAffectsAudio = true;
                    }
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
            _recordChange = true;
            foreach (NotesFile nf in _files)
                ((ISettingsChange)nf).RecordChange();

            foreach (GhNotesItem ghi in _ghItems)
            {
                if (ghi.IsMapped)
                    ((ISettingsChange)ghi.MappedFileItem).RecordChange();
            }
        }

        #endregion


        public bool UpdateAffectsAudio
        {
            get
            {
                return _updateAffectsAudio;
            }
            private set
            {
                if (_recordChange)
                    _updateAffectsAudio = value;
            }
        }

        private bool _updateAffectsAudio; //set to true when a change required the audio to be updated (to be safe)

        private float _hopoMeasure;
        private bool _gh3SustainClipping;
        private bool _forceNoStarPower;

        private NotesFile _baseFile;

        private bool _recordChange;
        private DateTime _lastChanged;

        private List<NotesFile> _files;
        private List<GhNotesItem> _ghItems;

        private Project _project;
        private ProjectSong _song;
    }
}
