using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public delegate void AudioFileChangedEventHandler(object source, AudioFileChangedEventArgs e);
    public delegate void PreviewSettingsChangedEventHandler(object source, PreviewSettingsChangedEventArgs e);

    public class ProjectSongAudio : ISettingsChange
    {
        public event AudioFileChangedEventHandler AudioFileChanged;
        public event PreviewSettingsChangedEventHandler PreviewSettingsChanged;

        internal ProjectSongAudio(Project project, ProjectSong song)
        {
            _lastChanged = DateTime.MinValue;
            _recordChange = false;

            _song = song;
            _project = project;

            _songAudioQbKey = QbKey.Create(string.Format("{0}_song", _song.SongQb.Id.Text));
            _guitarAudioQbKey = QbKey.Create(string.Format("{0}_guitar", _song.SongQb.Id.Text));
            _rhythmAudioQbKey = QbKey.Create(string.Format("{0}_rhythm", _song.SongQb.Id.Text));
            _previewAudioQbKey = QbKey.Create(string.Format("{0}_preview", _song.SongQb.Id.Text));

            _rawSongFilenames = new List<string>();
            _songFiles = new AudioFileList(this.AudioFileListChanged, _project);
            _guitarFile = null;
            _rhythmFile = null;

            setAudioFilenames();

        }

        public AudioFile CreateAudioFile(string filename)
        {
            return new AudioFile(filename, this);
        }

        public AudioFile CreateAudioFile(string filename, int volume)
        {
            return new AudioFile(filename, volume, this);
        }

        private void setAudioFilenames()
        {
            string mask = @"{0}\{1}.wav";
            _compressedSongFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.Compressed), _songAudioQbKey.Text);
            _compressedGuitarFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.Compressed), _guitarAudioQbKey.Text);
            _compressedRhythmFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.Compressed), _rhythmAudioQbKey.Text);
            _compressedPreviewFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.Compressed), _previewAudioQbKey.Text);

            mask = @"{0}\{1}.raw.wav";
            _rawSongFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.RawWav), _songAudioQbKey.Text);
            _rawSongFilenames = new List<string>(this.SongFiles.Count);
            foreach (AudioFile af in _songFiles)
                _rawSongFilenames.Add(string.Format(mask, _project.GetWorkingPath(WorkingFileType.RawWav), string.Format("{0}_{1}", _songAudioQbKey.Text, QbKey.Create(af.Name).Crc.ToString("X").PadLeft(8, '0'))));
            _rawGuitarFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.RawWav), _guitarAudioQbKey.Text);
            _rawRhythmFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.RawWav), _rhythmAudioQbKey.Text);
            _rawPreviewFilename = string.Format(mask, _project.GetWorkingPath(WorkingFileType.RawWav), _previewAudioQbKey.Text);
        }

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int PreviewStart
        {
            get { return _previewStart; }
            set
            {
                if (_previewStart == value || value < 0)
                    return;
                _previewStart = value;
                this.OnPreviewSettingsChanged();
            }
        }

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int PreviewLength
        {
            get { return _previewLength; }
            set
            {
                if (_previewLength == value || value < 0)
                    return;
                _previewLength = value;
                this.OnPreviewSettingsChanged();
            }
        }

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int PreviewFadeLength
        {
            get { return _previewFadeLength; }
            set
            {
                if (_previewFadeLength == value || value < 0)
                    return;
                _previewFadeLength = value;
                this.OnPreviewSettingsChanged();
            }
        }

        public int PreviewVolume
        {
            get { return _previewVolume; }
            set
            {
                if (_previewVolume == value || value < 0)
                    return;
                _previewVolume = value;
                this.OnPreviewSettingsChanged();
            }
        }

        public bool PreviewIncludeGuitar
        {
            get { return _previewIncludeGuitar; }
            set
            {
                if (_previewIncludeGuitar == value)
                    return;
                _previewIncludeGuitar = value;

                this.OnPreviewSettingsChanged();
            }
        }

        public bool PreviewIncludeRhythm
        {
            get { return _previewIncludeRhythm; }
            set
            {
                if (_previewIncludeRhythm == value)
                    return;
                _previewIncludeRhythm = value;

                this.OnPreviewSettingsChanged();
            }
        }

        protected void OnPreviewSettingsChanged()
        {
            this.LastChanged = DateTime.Now;
            if (this.PreviewSettingsChanged != null)
                this.PreviewSettingsChanged(this, new PreviewSettingsChangedEventArgs(_previewStart, _previewLength, _previewFadeLength, _previewVolume, _previewIncludeGuitar, _previewIncludeRhythm));
        }

        public AudioFileList SongFiles
        {
            get { return _songFiles; }
        }

        /// <summary>
        /// called when a song file has been added or removed
        /// </summary>
        /// <param name="sender"></param>
        internal void AudioFileListChanged(AudioFileList sender, AudioFileChangeType type, AudioFile from, AudioFile to, int index)
        {
            this.LastChanged = DateTime.Now;

            if (type == AudioFileChangeType.Added)
            {
                setAudioFilenames();

                deletePreview();

                if (File.Exists(this.RawSongFilenames[index]))
                {
                    FileHelper.Delete(this.RawSongFilenames[index]);
                    OnAudioFileChanged(this.RawSongFilenames[index], "song", false);
                }
            }
            else if (type == AudioFileChangeType.Removed)
            {
                string name = _rawSongFilenames[index];
                setAudioFilenames();

                deletePreview();

                FileHelper.Delete(name);
                OnAudioFileChanged(name, "song", false);
            }
            else
            {
                setAudioFilenames();

                deletePreview();

                FileHelper.Delete(_rawSongFilenames[index]);
                OnAudioFileChanged(_rawSongFilenames[index], "song", false);
            }
        }



        public AudioFile GuitarFile
        {
            get { return _guitarFile; }
            set
            {
                string o = _guitarFile == null ? string.Empty : _guitarFile.Name;
                string n = value == null ? string.Empty : value.Name;

                if (o.ToLower() == n.ToLower())
                    return;

                this.LastChanged = DateTime.Now;

                if (_guitarFile != null && _guitarFile.Name.Length != 0)
                    OnAudioFileChanged(this.RawGuitarFilename, "guitar", false); //removed

                //delete if changed
                FileHelper.Delete(this.RawGuitarFilename);

                _guitarFile = value;
                setAudioFilenames();
                deletePreview();

                if (_guitarFile != null && _guitarFile.Name.Length != 0)
                    OnAudioFileChanged(this.RawGuitarFilename, "guitar", true); //added
            }
        }

        public AudioFile RhythmFile
        {
            get { return _rhythmFile; }
            set
            {
                string o = _rhythmFile == null ? string.Empty : _rhythmFile.Name;
                string n = value == null ? string.Empty : value.Name;

                if (o.ToLower() == n.ToLower())
                    return;

                this.LastChanged = DateTime.Now;

                if (_rhythmFile != null && _rhythmFile.Name.Length != 0)
                    OnAudioFileChanged(this.RawRhythmFilename, "rhythm", false); //removed

                //delete if changed
                FileHelper.Delete(this.RawRhythmFilename);

                _rhythmFile = value;
                setAudioFilenames();
                deletePreview();

                if (_rhythmFile != null && _rhythmFile.Name.Length != 0)
                    OnAudioFileChanged(this.RawRhythmFilename, "rhythm", true); //added
            }
        }

        public string[] RawSongFilenames
        {
            get { return _rawSongFilenames.ToArray(); }
        }

        /// <summary>
        /// Merged RawSongFilenames name
        /// </summary>
        public string RawSongFilename
        {
            get { return _rawSongFilename; }
        }

        public string RawGuitarFilename
        {
            get { return _rawGuitarFilename; }
        }

        public string RawRhythmFilename
        {
            get { return _rawRhythmFilename; }
        }

        public string RawPreviewFilename
        {
            get { return _rawPreviewFilename; }
        }



        public string CompressedSongFilename
        {
            get { return _compressedSongFilename; }
        }

        public string CompressedGuitarFilename
        {
            get { return _compressedGuitarFilename; }
        }

        public string CompressedRhythmFilename
        {
            get { return _compressedRhythmFilename; }
        }

        public string CompressedPreviewFilename
        {
            get { return _compressedPreviewFilename; }
        }


        public QbKey SongAudioQbKey
        {
            get { return _songAudioQbKey; }
        }

        public QbKey GuitarAudioQbKey
        {
            get { return _guitarAudioQbKey; }
        }

        public QbKey RhythmAudioQbKey
        {
            get { return _rhythmAudioQbKey; }
        }

        public QbKey PreviewAudioQbKey
        {
            get { return _previewAudioQbKey; }
        }

        private void deletePreview()
        {
            FileHelper.Delete(this.RawPreviewFilename);
        }

        /// <summary>
        /// Create raw preview using the already specified settings in this class
        /// </summary>
        public void CreateRawPreview(bool finalise)
        {
            this.CreateRawPreview(_previewStart, _previewLength, _previewFadeLength, _previewVolume, finalise, _previewIncludeGuitar, _previewIncludeRhythm);
        }

        /// <summary>
        /// Sets the internal preview start, length and fadelength to safe values (start + length is not longer than audiolength etc)
        /// </summary>
        public bool SetSafePreviewSettings()
        {
            if (setSafePreviewSettings(_previewStart, _previewLength, _previewFadeLength, _previewVolume, _previewIncludeGuitar, _previewIncludeRhythm))
            {
                if (this.PreviewSettingsChanged != null)
                    this.PreviewSettingsChanged(this, new PreviewSettingsChangedEventArgs(_previewStart, _previewLength, _previewFadeLength, _previewVolume, _previewIncludeGuitar, _previewIncludeRhythm));
                return true;
            }
            return false;
        }

        private bool setSafePreviewSettings(int offset, int length, int fadeLength, int volume, bool includeGuitar, bool includeRhythm)
        {
            bool changed = false;

            if (this.SongFiles.Count == 0)
                return false;

            //not enough room for the audio, select the same length but in the middle
            if (offset + length > this.AudioLength)
            {
                if (length > this.AudioLength)
                {
                    offset = 0;
                    length = this.AudioLength;
                }
                else
                    offset = (this.AudioLength - length) / 2;
            }

            if (fadeLength * 2 > length)
                fadeLength = length / 2;

            if (_previewStart != offset || _previewLength != length || _previewFadeLength != fadeLength || _previewVolume != volume ||
                       _previewIncludeGuitar != includeGuitar || _previewIncludeRhythm != includeRhythm)
            {
                this.LastChanged = DateTime.Now;
                changed = true;
            }

            _previewStart = offset;
            _previewLength = length;
            _previewFadeLength = fadeLength;
            _previewVolume = volume;
            _previewIncludeGuitar = includeGuitar;
            _previewIncludeRhythm = includeRhythm;

            return changed;
        }

        /// <summary>
        /// Create the raw preview to disk, if the offset + length is greater then the song length, then select the same length from the middle of the audio. If this is still too long then use the full track.
        /// </summary>
        public void CreateRawPreview(int offset, int length, int fadeLength, int volume, bool includeGuitar, bool includeRhythm, bool finalise)
        {
            bool changed = setSafePreviewSettings(offset, length, fadeLength, volume, includeGuitar, includeRhythm);

            List<AudioFile> l = new List<AudioFile>();

            for (int i = 0; i < _rawSongFilenames.Count; i++)
            {
                if (File.Exists(_rawSongFilenames[i]))
                    l.Add(new AudioFile(_rawSongFilenames[i], _songFiles[i].Volume, null));
            }
            if (_previewIncludeGuitar && this.GuitarFile != null && File.Exists(this.RawGuitarFilename))
                l.Add(new AudioFile(this.RawGuitarFilename, this.GuitarFile.Volume, null));
            if (_previewIncludeRhythm && this.RhythmFile != null && File.Exists(this.RawRhythmFilename))
                l.Add(new AudioFile(this.RawRhythmFilename, this.RhythmFile.Volume, null));

            if (l.Count != 0)
                WavProcessor.CreatePreview(_previewStart, _previewLength, _previewFadeLength, (float)_previewVolume / 100F, finalise, this.RawPreviewFilename, l.ToArray());

            if (changed)
            {
                if (this.PreviewSettingsChanged != null)
                    this.PreviewSettingsChanged(this, new PreviewSettingsChangedEventArgs(_previewStart, _previewLength, _previewFadeLength, _previewVolume, _previewIncludeGuitar, _previewIncludeRhythm));
            }

        }

        public int AudioLength
        {
            get { return _audioLength; }
            internal set { _audioLength = value; }
        }

        /// <summary>
        /// Creates games files and adds any new files to the FileManager
        /// </summary>
        public void CreateGameAudio(bool forceMono, int forceDownSample, bool finalise)
        {
            List<string> wavs = new List<string>();

            //always crete the preview (the song audio may have been reduced)
            this.CreateRawPreview(finalise);

            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                if (this.SongFiles.Count != 0)
                {
                    if (this.RawSongFilenames.Length == 1)
                    {
                        FileHelper.Delete(this.RawSongFilename);
                        FileHelper.Move(this.RawSongFilenames[0], _rawSongFilename);
                    }
                    else
                    {
                        List<AudioFile> l = new List<AudioFile>();
                        for (int i = 0; i < _rawSongFilenames.Count; i++)
                        {
                            if (File.Exists(_rawSongFilenames[i]))
                                l.Add(new AudioFile(_rawSongFilenames[i], _songFiles[i].Volume, null));
                        }
                        WavProcessor.CombineAudio(1F, this.RawSongFilename, l.ToArray());
                    }
                    this.Export(this.RawSongFilename, this.CompressedSongFilename, forceMono, forceDownSample);
                    wavs.Add(this.RawSongFilename);
                }
                if (this.GuitarFile != null && File.Exists(this.GuitarFile.Name))
                {
                    this.Export(this.RawGuitarFilename, this.CompressedGuitarFilename, forceMono, forceDownSample);
                    wavs.Add(this.RawGuitarFilename);
                }

                if ((this.RhythmFile == null || !File.Exists(this.RhythmFile.Name)) && wavs.Count != 0) //create blank rhythm if not present
                {
                    WavProcessor.CreateSilentWav(500, this.RawRhythmFilename, wavs.ToArray());
                    wavs.Add(this.RawRhythmFilename);
                }

                if (wavs.Count == 0)
                    return;


                if (File.Exists(this.RawPreviewFilename))
                {
                    this.Export(this.RawPreviewFilename, this.CompressedPreviewFilename, forceMono, forceDownSample);
                    wavs.Add(this.RawPreviewFilename);
                }

                if (File.Exists(this.RawRhythmFilename))
                    this.Export(this.RawRhythmFilename, this.CompressedRhythmFilename, forceMono, forceDownSample);

                this.DeleteRawFiles();

                string hdrFilename = _project.GameInfo.GetMusicHeaderFilename(_song.SongQb.Id);
                string musFilename = _project.GameInfo.GetMusicFilename(_song.SongQb.Id);

                GameFile gfh = new GameFile(hdrFilename, localGameFilename(hdrFilename), GameFileType.Dat, true);
                GameFile gf = new GameFile(musFilename, localGameFilename(musFilename), GameFileType.Wad, true);


                string gn = this.CompressedGuitarFilename;
                if (this.GuitarFile == null)
                    gn = this.CompressedSongFilename;


                DatWad.CreateDatWad(_song.SongQb.Id, _project.FileManager.PakFormat.EndianType, gfh.LocalName, gf.LocalName,
                    this.CompressedSongFilename, gn, this.CompressedRhythmFilename, this.CompressedPreviewFilename);

                this.DeleteCompressedFiles();

                _project.FileManager.AddNew(gfh);
                _project.FileManager.AddNew(gf);


            }
            else
                throw new ApplicationException(string.Format("'{0}' is not a supported Game type", _project.GameInfo.Game.ToString()));
        }

        private string localGameFilename(string gameFilename)
        {
            gameFilename = gameFilename.Replace('/', '\\');
            int p = gameFilename.LastIndexOf('\\');
            return string.Format(@"{0}\{1}", _project.GetWorkingPath(WorkingFileType.GhFiles), gameFilename.Substring(p + 1));
        }

        public void Export(string sourceName, string destinationName, bool forceMono, int forceDownSample)
        {
            _project.AudioExport.Convert(sourceName, destinationName, forceMono, forceDownSample);

            if (!File.Exists(destinationName))
                throw new ApplicationException(string.Format("Audio failed to convert from '{0}' to '{1}'", sourceName, destinationName));

        }

        public void DeleteRawFiles()
        {
            //sometimes cannot remove audio as the Audio Export plugin has not released it.  It get removed at the end of the overall processing
            FileHelper.Delete(this.RawGuitarFilename);
            FileHelper.Delete(this.RawRhythmFilename);

            foreach (string f in _rawSongFilenames)
                FileHelper.Delete(f);

            FileHelper.Delete(this.RawSongFilename);
            FileHelper.Delete(this.RawPreviewFilename);

        }

        public void DeleteCompressedFiles()
        {
            FileHelper.Delete(this.CompressedGuitarFilename);
            FileHelper.Delete(this.CompressedRhythmFilename);
            FileHelper.Delete(this.CompressedSongFilename);
            FileHelper.Delete(this.CompressedPreviewFilename);
    
        }

        /// <summary>
        /// Call to determin if source audio is still to be decoded, calling Import() will decode it.
        /// </summary>
        public bool HasMissingRawAudio()
        {
            foreach (string fn in _rawSongFilenames)
            {
                if (!File.Exists(fn))
                    return true;
            }

            if (this.GuitarFile != null && GuitarFile != null && this.RawGuitarFilename.Length != 0 && !File.Exists(this.RawGuitarFilename))
                return true;

            if (this.RhythmFile != null && RhythmFile != null && this.RawRhythmFilename.Length != 0 && !File.Exists(this.RawRhythmFilename))
                return true;

            return false;

        }

        public bool ImportMissingAudioFiles()
        {
            bool b = false;

            for (int i = 0; i < _rawSongFilenames.Count; i++)
            {
                if (File.Exists(_songFiles[i].Name) && !File.Exists(_rawSongFilenames[i]))
                {
                    this.Import(_songFiles[i].Name, _rawSongFilenames[i]);
                    b = true;
                }
            }

            if (this.GuitarFile != null && GuitarFile != null && this.RawGuitarFilename.Length != 0 && !File.Exists(this.RawGuitarFilename))
            {
                this.Import(this.GuitarFile.Name, this.RawGuitarFilename);
                b = true;
            }

            if (this.RhythmFile != null && RhythmFile != null && this.RawRhythmFilename.Length != 0 && !File.Exists(this.RawRhythmFilename))
            {
                this.Import(this.RhythmFile.Name, this.RawRhythmFilename);
                b = true;
            }

            return b;
        }


        /// <summary>
        /// Imports all audio that has been added but not decoded to wav
        /// </summary>
        public void Import()
        {
            for (int i = 0; i < this.SongFiles.Count; i++)
            {
                if (!File.Exists(this.RawSongFilenames[i]))
                    this.Import(this.SongFiles[i].Name, this.RawSongFilenames[i]);
            }

            if (this.GuitarFile != null && GuitarFile.Name.Length != 0 && this.RawGuitarFilename.Length != 0 && !File.Exists(this.RawGuitarFilename))
                this.Import(this.GuitarFile.Name, this.RawGuitarFilename);

            if (this.RhythmFile != null && RhythmFile.Name.Length != 0 && this.RawRhythmFilename.Length != 0 && !File.Exists(this.RawRhythmFilename))
                this.Import(this.RhythmFile.Name, this.RawRhythmFilename);

        }

        public void Import(string sourceName, string destinationName)
        {
            bool error = false;

            try
            {
                DateTime dt = DateTime.Now;
                if (Path.HasExtension(sourceName) && Path.GetExtension(sourceName).ToLower() == ".wav")
                    File.Copy(sourceName, destinationName, true);
                else
                    _project.AudioImport.Convert(sourceName, destinationName);
                //System.Diagnostics.Debug.WriteLine((DateTime.Now - dt).TotalMilliseconds.ToString());
            }
            catch
            {
                error = true;
            }

            if (error || !File.Exists(destinationName))
                throw new ApplicationException(string.Format("Audio failed to convert from '{0}' to '{1}'", sourceName, destinationName));

            WavSingleChunkHeader wh = null;

            int i = 0;

            //hack
            while (i < 4) //try 4 times to open the file (sometimes the file is locked, but works when tried a second time)
            {
                try
                {
                    using (FileStream fs = File.Open(destinationName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        wh = WavProcessor.FixWavHeader(fs);
                    }
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(250);
                    System.Threading.Thread.Sleep(0);
                    i++;
                }
            }

            if (i >= 4)
                throw new ApplicationException(string.Format("Failed to open '{0}' to edit the header.", destinationName));

            bool f = false;

            foreach (string fn in _rawSongFilenames)
            {
                if (destinationName.ToLowerInvariant() == fn.ToLowerInvariant())
                {
                    f = true;
                    break;
                }
            }

            if (f)
            {
                _audioLength = wh.AudioLength;
                _song.Length = this.AudioLength;

                OnAudioFileChanged(destinationName, "song", true);
            }
            else if (destinationName.ToLowerInvariant() == this.RawGuitarFilename.ToLowerInvariant())
            {
                OnAudioFileChanged(destinationName, "guitar", true);
            }
            else if (destinationName.ToLowerInvariant() == this.RawRhythmFilename.ToLowerInvariant())
            {
                OnAudioFileChanged(destinationName, "rhythm", true);
            }
        }

        protected void OnAudioFileChanged(string filename, string audioId, bool addedRemoved)
        {
            if (AudioFileChanged != null)
                AudioFileChanged(this, new AudioFileChangedEventArgs(filename, audioId, addedRemoved));
        }


        /// <summary>
        /// Remove silent audio, remove guitar if it's the same as the song.
        /// </summary>
        /// <returns>True if anything was removed (the preview might need recreating</returns>
        public bool RemoveAndDeleteRedundantFiles()
        {
            bool changed = false;

            //delete any silent songs - allow 1 (for prepping etc)
            for (int i = this.SongFiles.Count - 1; i >= 0; i--)
            {
                if (this.SongFiles.Count > 1 && File.Exists(this.RawSongFilenames[i]) && WavProcessor.IsWavSilent(this.RawSongFilenames[i], 0.95F))
                {
                    //FileHelper.Delete(this.RawSongFilenames[i]);
                    this.SongFiles.RemoveAt(i); //raw filesnames are kept in sync from an even on the collection
                    //OnAudioFileChanged(this.RawSongFilenames[i], "song", false);
                    changed = true;
                }
            }

            if (this.GuitarFile != null && File.Exists(this.RawGuitarFilename) && WavProcessor.IsWavSilent(this.RawGuitarFilename, 0.95F))
                this.GuitarFile = null;

            if (this.RhythmFile != null && File.Exists(this.RawRhythmFilename) && WavProcessor.IsWavSilent(this.RawRhythmFilename, 0.95F))
                this.RhythmFile = null;

            if (this.SongFiles.Count == 1 && this.GuitarFile != null && File.Exists(this.RawGuitarFilename) && File.Exists(this.SongFiles[0].Name) && File.Exists(this.GuitarFile.Name))
            {
                if (WavProcessor.FilesEqual(this.SongFiles[0].Name, this.GuitarFile.Name))
                {
                    FileHelper.Delete(this.RawGuitarFilename);
                    this.GuitarFile = null;
                    changed = true;
                    //OnAudioFileChanged(this.RawGuitarFilename, "guitar", false);
                }
            }

            if (changed)
                deletePreview(); //this should happen by the events raise setting guitar file to null or removing a song etc.

            return changed;
        }

        public bool AllFilesExist
        {
            get
            {

                foreach (AudioFile af in _songFiles)
                {
                    if (af != null && af.Name.Length != 0 && !File.Exists(af.Name))
                        return false;
                }

                if (_guitarFile != null && _guitarFile.Name.Length != 0 && !File.Exists(_guitarFile.Name))
                    return false;

                if (_rhythmFile != null && _rhythmFile.Name.Length != 0 && !File.Exists(_rhythmFile.Name))
                    return false;

                return true;
            }
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

        private int _audioLength;
        private DateTime _lastChanged;
        private bool _recordChange;
        
        private QbKey _songAudioQbKey;
        private QbKey _guitarAudioQbKey;
        private QbKey _rhythmAudioQbKey;
        private QbKey _previewAudioQbKey;

        private AudioFile _guitarFile;
        private AudioFile _rhythmFile;

        private AudioFileList _songFiles;

        private List<string> _rawSongFilenames;
        private string _rawSongFilename; //merged audio
        private string _rawGuitarFilename;
        private string _rawRhythmFilename;
        private string _rawPreviewFilename;

        private string _compressedSongFilename;
        private string _compressedGuitarFilename;
        private string _compressedRhythmFilename;
        private string _compressedPreviewFilename;

        private int _previewFadeLength;
        private int _previewLength;
        private int _previewVolume;
        private int _previewStart;
        private bool _previewIncludeGuitar;
        private bool _previewIncludeRhythm;

        private Project _project;
        private ProjectSong _song;
    }
}
