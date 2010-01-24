using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class ProjectBackgroundAudio : ISettingsChange
    {
        public ProjectBackgroundAudio(Project project, DatWad datWad, DatItem datItem)
        {
            _lastChanged = DateTime.MinValue;
            _lastApplied = DateTime.MinValue;
            _previewLastCreated = DateTime.MinValue;
            _recordChange = false;

            if (_names == null)
            {
                _names = new Dictionary<string, string>();
                _names.Add("menbulls.wav", "Rage Against the Machine - Bulls on Parade");
                _names.Add("menfrget.wav", "Slipknot - Before I Forget");
                _names.Add("menjungl.wav", "Guns N Roses - Welcome to the Jungle");
                _names.Add("menpaint.wav", "The Rolling Stones - Paint It Black");
                _names.Add("menevenf.wav", "Pearl Jam - Even Flow");
                _names.Add("menmonst.wav", "Matchbook Romance - Monsters");
                _names.Add("menswte.wav ", "Aerosmith - Sweet Emotion");
                _names.Add("menmama.wav ", "Aerosmith - Mama Kin");
                _names.Add("menlove.wav ", "Aerosmith - Love in an Elavator");
                _names.Add("menragd.wav ", "Aerosmith - Rag Doll");
                _names.Add("mentoys.wav ", "Aerosmith - Toys In The Atic");
                _names.Add("menwalk.wav ", "Aerosmith - Walk This Way");
            }

            _name = _names[datWad.ReadInternalFileName(datItem)];
            _project = project;
            _datWad = datWad;
            _datItem = datItem;
            _audioFiles = new AudioFileList(this.AudioFileListChanged, _project);
            _rawFiles = new Dictionary<string, string>();
            _rawLengths = new Dictionary<string, int>();

            string mask = @"{0}\{1}.wav";
            _compressedAudioFile = string.Format(mask, _project.GetWorkingPath(WorkingFileType.Compressed), this.Name);

            mask = @"{0}\{1}.raw.wav";
            _rawAudioFile = string.Format(mask, _project.GetWorkingPath(WorkingFileType.RawWav), this.Name);
        }

        public AudioFile CreateAudioFile(string filename)
        {
            return new AudioFile(filename, this);
        }

        public AudioFile CreateAudioFile(string filename, int volume)
        {
            return new AudioFile(filename, volume, this);
        }

        public AudioFileList AudioFiles
        {
            get { return _audioFiles; }
        }

        public string RawAudioFile
        {
            get { return _rawAudioFile; }
        }

        public string CompressedAudioFile
        {
            get { return _compressedAudioFile; }
        }

        public bool ReplaceEnabled
        {
            get { return _audioFiles.Count != 0; }
        }

        public string Name
        {
            get { return _name; }
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
                this.LastChanged = DateTime.Now;
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
                this.LastChanged = DateTime.Now;
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
                this.LastChanged = DateTime.Now;
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
                this.LastChanged = DateTime.Now;
            }
        }

        public int AudioLength
        {
            get { return _audioLength; }
            internal set { _audioLength = value; }
        }

        public void RemoveFiles()
        {
            foreach (string s in _rawFiles.Values)
                FileHelper.Delete(s);

            FileHelper.Delete(_compressedAudioFile);
            FileHelper.Delete(_rawAudioFile);
        }

        public void ReplaceInWad()
        {
            if (File.Exists(_compressedAudioFile))
                _datWad.ReplaceFsbFileWithWav(_datItem, _compressedAudioFile);
        }

        public void Export(bool forceMono, int forceDownSample)
        {
            _project.AudioExport.Convert(_rawAudioFile, _compressedAudioFile, forceMono, forceDownSample);
        }

        /// <summary>
        /// Call to determin if source audio is still to be decoded, calling Import() will decode it.
        /// </summary>
        public bool HasMissingRawAudio()
        {
            foreach (AudioFile af in _audioFiles)
            {
                if (!File.Exists(_rawFiles[af.Name]))
                    return true;
            }
            return false;
        }

        internal void AudioFileListChanged(AudioFileList sender, AudioFileChangeType type, AudioFile from, AudioFile to, int index)
        {
            this.LastChanged = DateTime.Now;

            if (type == AudioFileChangeType.Removed || type == AudioFileChangeType.Changed)
            {
                FileHelper.Delete(_rawFiles[from.Name]);
                _rawFiles.Remove(from.Name);
                _rawLengths.Remove(from.Name);
            }

            if (type == AudioFileChangeType.Added || type == AudioFileChangeType.Changed)
            {
                string filename = string.Format(@"{0}\{1}_{2}.wav", _project.GetWorkingPath(WorkingFileType.RawWav), _name, QbKey.Create(to.Name).Crc.ToString("X").PadLeft(8, '0'));

                //if we have no record of the file and it exists then delete it. It may not match what we need
                FileHelper.Delete(filename);

                _rawFiles.Add(to.Name, filename);
                _rawLengths.Add(to.Name, 0);
            }

        }

        /// <summary>
        /// Add files to the AudioFiles list before calling import
        /// </summary>
        public void Import()
        {

            foreach (AudioFile af in _audioFiles)
            {
                if (_rawFiles.ContainsKey(af.Name))
                {
                    if (File.Exists(af.Name) && !File.Exists(_rawFiles[af.Name]))
                    {
                        _project.AudioImport.Convert(af.Name, _rawFiles[af.Name]);

                        if (!File.Exists(_rawFiles[af.Name]))
                            throw new ApplicationException(string.Format("Audio failed to convert from '{0}' to '{1}'", af, _rawFiles[af.Name]));

                        WavSingleChunkHeader wh = null;

                        int i = 0;

                        //hack
                        while (i < 4) //try 4 times to open the file (sometimes the file is locked, but works when tried a second time)
                        {
                            try
                            {
                                using (FileStream fs = File.Open(_rawFiles[af.Name], FileMode.Open, FileAccess.ReadWrite))
                                {
                                    wh = WavProcessor.FixWavHeader(fs);
                                    _rawLengths[af.Name] = wh.AudioLength;
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
                    }
                }
            }


            int audioLength = 0;
            foreach (int x in _rawLengths.Values)
            {
                if (x > audioLength)
                    audioLength = x;
            }
            if (audioLength != 0)
                _audioLength = audioLength;

        }


        /// <summary>
        /// Sets the internal preview start, length and fadelength to safe values (start + length is not longer than audiolength etc)
        /// </summary>
        public bool SetSafePreviewSettings()
        {
            return setSafePreviewSettings(_previewStart, _previewLength, _previewFadeLength, _previewVolume);
        }

        /// <summary>
        /// Ensures that all inputs are withing safe limits
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="fadeLength"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        private bool setSafePreviewSettings(int offset, int length, int fadeLength, int volume)
        {
            bool changed = false;

            if (this.AudioFiles.Count == 0)
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

            if (_previewStart != offset || _previewLength != length || _previewFadeLength != fadeLength || _previewVolume != volume)
            {
                this.LastChanged = DateTime.Now;
                changed = true;
            }

            _previewStart = offset;
            _previewLength = length;
            _previewFadeLength = fadeLength;
            _previewVolume = volume;

            return changed;
        }

        /// <summary>
        /// Create the preview to disk
        /// </summary>
        /// <param name="finalise">Set to true if audio on disk has had the final volumes applied (therefore volumes should be 100%)</param>
        public void CreatePreview(bool finalise)
        {
            setSafePreviewSettings(_previewStart, _previewLength, _previewFadeLength, _previewVolume);

            if (_previewLastCreated == DateTime.MinValue)
                _lastChanged = DateTime.Now;

            if (_lastChanged != _previewLastCreated || _previewLastCreated == DateTime.MinValue || finalise)
            {
                List<AudioFile> f = new List<AudioFile>(_audioFiles.Count);
                for (int i = 0; i < Math.Min(_audioFiles.Count, _rawFiles.Count); i++)
                    f.Add(new AudioFile(_rawFiles[_audioFiles[i].Name], _audioFiles[i].Volume, null));

                float vol = (float)_previewVolume;
                //if (finalise)
                //    vol *= 0.5F;

                //we never ignore the audio levels (by setting finalise to true) becuae the wavs for background audio are never modified on disk.
                WavProcessor.CreatePreview(_previewStart, _previewLength, _previewFadeLength, vol / 100F, finalise, _rawAudioFile, f.ToArray());

                WavProcessor.Normalize(_rawAudioFile, false);

                if (vol != 100)
                    WavProcessor.SetLengthSilenceAndVolume(0, 0, vol / 100F, _rawAudioFile);

                _previewLastCreated = _lastChanged;
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

        private DateTime _previewLastCreated;

        private static Dictionary<string, string> _names;
        private string _name;

        private int _previewFadeLength;
        private int _previewLength;
        private int _previewVolume;
        private int _previewStart;

        private int _audioLength;

        private AudioFileList _audioFiles;
        private Dictionary<string, string> _rawFiles;
        private Dictionary<string, int> _rawLengths;

        private string _rawAudioFile;
        private string _compressedAudioFile;

        private Project _project;
        private DatWad _datWad;
        private DatItem _datItem;

    }
}
