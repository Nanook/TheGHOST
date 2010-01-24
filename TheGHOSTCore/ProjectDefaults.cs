using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public enum DefaultSettingMode
    {
        Game,
        Default
    }

    public enum DefaultSettingModeBlank
    {
        Game,
        Default,
        Blank
    }

    public class ProjectDefaults
    {
        internal ProjectDefaults()
        {
            _minMsBeforeNotesStart = 2000;
            _gh3SustainClipping = true;
            _forceMono = false;
            _forceDownSample = 33075;
            _previewFadeLength = 1000;
            _previewLength = 20000;
            _previewStart = 60000;
            _previewVolume = 100;
            _previewIncludeGuitar = true;
            _previewIncludeRhythm = true;
            _singer = Singer.Male;
            _hopoMeasure = 2.95F;
            _forceNoStarPower = false;
            _songVolume = 0;
            _guitarVolume = 0;
            _year = "";
            _smartModeCrowdImport = false;
            _songVolumeMode = DefaultSettingMode.Game;
            _guitarVolumeMode = DefaultSettingMode.Game;
            _yearMode = DefaultSettingModeBlank.Game;
            _singerMode = DefaultSettingMode.Game;
            _gameId = Game.GH3_Wii;
            _languageId = "";
            _audioExportPlugin = "";
            _audioImportPlugin = "";
            _fileCopyPlugin = "";
            _workingFolder = "";
            _audioGuitarVolume = 100;
            _audioRhythmVolume = 100;
            _audioSongVolume = 100;
            _reapplyAll = false;

            _projectHistory = new List<string>();
        }

        public bool Gh3SustainClipping
        {
            get { return _gh3SustainClipping; }
            set { _gh3SustainClipping = value; }
        }

        public bool SmartModeCrowdImport
        {
            get { return _smartModeCrowdImport; }
            set { _smartModeCrowdImport = value; }
        }

        public int MinMsBeforeNotesStart
        {
            get { return _minMsBeforeNotesStart; }
            set { _minMsBeforeNotesStart = value; }
        }

        public int PreviewStart
        {
            get { return _previewStart; }
            set { _previewStart = value; }
        }

        public int PreviewLength
        {
            get { return _previewLength; }
            set { _previewLength = value; }
        }

        public int PreviewFadeLength
        {
            get { return _previewFadeLength; }
            set { _previewFadeLength = value; }
        }

        public int PreviewVolume
        {
            get { return _previewVolume; }
            set { _previewVolume = value; }
        }

        public bool PreviewIncludeGuitar
        {
            get { return _previewIncludeGuitar; }
            set { _previewIncludeGuitar = value; }
        }

        public bool PreviewIncludeRhythm
        {
            get { return _previewIncludeRhythm; }
            set { _previewIncludeRhythm = value; }
        }

        public int AudioSongVolume
        {
            get { return _audioSongVolume; }
            set { _audioSongVolume = value; }
        }

        public int AudioGuitarVolume
        {
            get { return _audioGuitarVolume; }
            set { _audioGuitarVolume = value; }
        }

        public int AudioRhythmVolume
        {
            get { return _audioRhythmVolume; }
            set { _audioRhythmVolume = value; }
        }

        public bool ForceMono
        {
            get { return _forceMono; }
            set { _forceMono = value; }
        }

        public int ForceDownSample
        {
            get { return _forceDownSample; }
            set { _forceDownSample = value; }
        }

        public float HoPoMeasure
        {
            get { return _hopoMeasure; }
            set
            {
                if (value > 20 || value < 0)
                    _hopoMeasure = 2.95F;
                else
                    _hopoMeasure = value;
            }
        }

        public bool ForceNoStarPower
        {
            get { return _forceNoStarPower; }
            set { _forceNoStarPower = value; }
        }

        public float GuitarVolume
        {
            get { return _guitarVolume; }
            set { _guitarVolume = value; }
        }

        public float SongVolume
        {
            get { return _songVolume; }
            set { _songVolume = value; }
        }

        public Singer Singer
        {
            get { return _singer; }
            set { _singer = value; }
        }

        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public DefaultSettingMode GuitarVolumeMode
        {
            get { return _guitarVolumeMode; }
            set { _guitarVolumeMode = value; }
        }

        public DefaultSettingMode SongVolumeMode
        {
            get { return _songVolumeMode; }
            set { _songVolumeMode = value; }
        }

        public DefaultSettingModeBlank YearMode
        {
            get { return _yearMode; }
            set { _yearMode = value; }
        }

        public DefaultSettingMode SingerMode
        {
            get { return _singerMode; }
            set { _singerMode = value; }
        }

        public Game GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }

        public string LanguageId
        {
            get { return _languageId; }
            set { _languageId = value; }
        }

        public string AudioExportPlugin
        {
            get { return _audioExportPlugin; }
            set { _audioExportPlugin = value; }
        }

        public string AudioImportPlugin
        {
            get { return _audioImportPlugin; }
            set { _audioImportPlugin = value; }
        }

        public string FileCopyPlugin
        {
            get { return _fileCopyPlugin; }
            set { _fileCopyPlugin = value; }
        }

        public List<string> ProjectHistory
        {
            get { return _projectHistory; }
        }

        public string WorkingFolder
        {
            get { return _workingFolder; }
            set { _workingFolder = value; }
        }

        public bool ReapplyAll
        {
            get { return _reapplyAll; }
            set { _reapplyAll = value; }
        }


        private Singer _singer;

        private float _hopoMeasure;

        private bool _smartModeCrowdImport;

        private int _previewFadeLength;
        private int _previewLength;
        private int _previewVolume;
        private int _previewStart;
        private bool _previewIncludeGuitar;
        private bool _previewIncludeRhythm;
        private int _audioSongVolume;
        private int _audioGuitarVolume;
        private int _audioRhythmVolume;
        private bool _forceMono;
        private int _forceDownSample;

        private float _guitarVolume;
        private float _songVolume;

        private int _minMsBeforeNotesStart;
        private bool _gh3SustainClipping;
        private bool _forceNoStarPower;

        private string _year;

        private DefaultSettingMode _guitarVolumeMode;
        private DefaultSettingMode _songVolumeMode;
        private DefaultSettingMode _singerMode;
        private DefaultSettingModeBlank _yearMode;

        private Game _gameId;
        private string _languageId;
        private string _audioExportPlugin;
        private string _audioImportPlugin;
        private string _fileCopyPlugin;

        private string _workingFolder;

        private List<string> _projectHistory;

        private bool _reapplyAll; //don't save this
    }
}
