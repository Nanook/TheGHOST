using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Nanook.TheGhost;

namespace Nanook.TheGhost
{
    public class ProjectDefaultsProperties
    {
        public enum DownSampleRate
        {
            UseInput = 0,
            LimitTo8000Hz = 8000,
            LimitTo11025Hz = 11025,
            LimitTo22050Hz = 22050,
            LimitTo32000Hz = 32000,
            LimitTo33075Hz = 33075,
            LimitTo44100Hz = 44100,
            LimitTo48000Hz = 48000
        }

        internal ProjectDefaultsProperties(ProjectDefaults defaults)
        {
            _defaults = defaults;
        }

        [Browsable(false)]
        public ProjectDefaults Base
        {
            get { return _defaults; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Guitar Hero guitar volume setting.")]
        public float GuitarVolume
        {
            get { return _defaults.GuitarVolume; }
            set { _defaults.GuitarVolume = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Guitar Hero song volume setting.")]
        public float SongVolume
        {
            get { return _defaults.SongVolume; }
            set { _defaults.SongVolume = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Singer, Male or Female")]
        public Singer Singer
        {
            get { return _defaults.Singer; }
            set { _defaults.Singer = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("The year to default all songs to.")]
        public string Year
        {
            get { return _defaults.Year; }
            set { _defaults.Year = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Default the guitar volume to the GuitarVolume specified here (Default) or the one in the game (Game).")]
        public DefaultSettingMode GuitarVolumeMode
        {
            get { return _defaults.GuitarVolumeMode; }
            set { _defaults.GuitarVolumeMode = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Default the song volume to the SongVolume specified here (Default) or the one in the game (Game).")]
        public DefaultSettingMode SongVolumeMode
        {
            get { return _defaults.SongVolumeMode; }
            set { _defaults.SongVolumeMode = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Default the year to the Year specified here (Default), the one in the game (Game) or always use no year (Blank).")]
        public DefaultSettingModeBlank YearMode
        {
            get { return _defaults.YearMode; }
            set { _defaults.YearMode = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("Default the singer to the Singer specified here (Default) or the one in the game (Game).")]
        public DefaultSettingMode SingerMode
        {
            get { return _defaults.SingerMode; }
            set { _defaults.SingerMode = value; }
        }

        [CategoryAttribute("1. Song"), DescriptionAttribute("When using Smart Mode import the crowd audio if present.")]
        public bool SmartModeCrowdImport
        {
            get { return _defaults.SmartModeCrowdImport; }
            set { _defaults.SmartModeCrowdImport = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Volume of the Song 0% = silent, 100% = Full volume.")]
        public int AudioSongVolume
        {
            get { return _defaults.AudioSongVolume; }
            set { _defaults.AudioSongVolume = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Volume of the Guitar 0% = silent, 100% = Full volume.")]
        public int AudioGuitarVolume
        {
            get { return _defaults.AudioGuitarVolume; }
            set { _defaults.AudioGuitarVolume = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Volume of the Song 0% = silent, 100% = Full volume.")]
        public int AudioRhythmVolume
        {
            get { return _defaults.AudioRhythmVolume; }
            set { _defaults.AudioRhythmVolume = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Offset in seconds where preview is to start.")]
        public float PreviewStart
        {
            get { return (float)_defaults.PreviewStart / 1000F; }
            set { _defaults.PreviewStart = (int)(value * 1000F); }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Length in seconds of the preview.")]
        public float PreviewLength
        {
            get { return _defaults.PreviewLength / 1000F; }
            set { _defaults.PreviewLength = (int)(value * 1000F); }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Length in seconds of the fade in / out.")]
        public float PreviewFadeLength
        {
            get { return _defaults.PreviewFadeLength / 1000F; }
            set { _defaults.PreviewFadeLength = (int)(value * 1000F); }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Volume of the preview 0% = silent, 100% = Full volume.")]
        public int PreviewVolume
        {
            get {  return _defaults.PreviewVolume; }
            set {  _defaults.PreviewVolume = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("If the guitar audio is specified then include it in the preview.")]
        public bool PreviewIncludeGuitar
        {
            get { return _defaults.PreviewIncludeGuitar; }
            set { _defaults.PreviewIncludeGuitar = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("If the rhythm audio is specified then include it in the preview.")]
        public bool PreviewIncludeRhythm
        {
            get { return _defaults.PreviewIncludeRhythm; }
            set { _defaults.PreviewIncludeRhythm = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Force the output audio to mono for all replacements.")]
        public bool ForceMono
        {
            get { return _defaults.ForceMono; }
            set { _defaults.ForceMono = value; }
        }

        [CategoryAttribute("2. Audio"), DescriptionAttribute("Force the output audio to be no higher than this sample rate. GH uses 33075. WARNING Audio set at 44100 or higher causes problems when song, guitar and rhythm are set. See the FAQ")]
        public DownSampleRate ForceDownsample
        {
            get { return (DownSampleRate)_defaults.ForceDownSample; }
            set { _defaults.ForceDownSample = (int)value; }
        }

        [CategoryAttribute("3. Notes"), DescriptionAttribute("Hammer On/Pull Off Measure.  This adjusts which notes are auto HOPO")]
        public float HOPOMeasure
        {
            get { return _defaults.HoPoMeasure; }
            set { _defaults.HoPoMeasure = value; }
        }

        [CategoryAttribute("3. Notes"), DescriptionAttribute("Ensure x seconds elapses before the first note is to be triggered.")]
        public int SecsBeforeNotesStart
        {
            get { return _defaults.MinMsBeforeNotesStart / 1000; }
            set { _defaults.MinMsBeforeNotesStart = value * 1000; }
        }

        [CategoryAttribute("3. Notes"), DescriptionAttribute("Ensure sustained notes have a small gap and don't continue until the next note.")]
        public bool Gh3SustainClipping
        {
            get { return _defaults.Gh3SustainClipping; }
            set { _defaults.Gh3SustainClipping = value; }
        }

        [CategoryAttribute("3. Notes"), DescriptionAttribute("Force replaced songs to default to having no Star Power.")]
        public bool ForceNoStarPower
        {
            get { return _defaults.ForceNoStarPower; }
            set { _defaults.ForceNoStarPower = value; }
        }

        private ProjectDefaults _defaults;

    }
}
