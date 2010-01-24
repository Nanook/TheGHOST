using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace Nanook.TheGhost
{
    [DefaultPropertyAttribute("Title")]
    public class TrackProperties
    {
        internal TrackProperties(ProjectSong song)
        {
            _song = song;
        }

        //Handy attributes to use on properties
        //[Browsable(false)]
        //[ReadOnly(true)]


        [CategoryAttribute("1. Song Edit"), DescriptionAttribute("Title of the song.")]
        public string Title
        {
            get { return _song.Title; }
            set { _song.Title = value; }
        }

        [CategoryAttribute("1. Song Edit"), DescriptionAttribute("Artist of the song.")]
        public string Artist
        {
            get { return _song.Artist; }
            set { _song.Artist = value; }
        }

        [CategoryAttribute("1. Song Edit"), DescriptionAttribute("Year of the song.")]
        public string Year
        {
            get { return _song.Year; }
            set { _song.Year = value; }
        }



        [CategoryAttribute("2. Misc"), DescriptionAttribute("Volume of Guitar audio.")]
        public float GuitarVolume
        {
            get { return _song.GuitarVolume; }
            set { _song.GuitarVolume = value; }
        }

        [CategoryAttribute("2. Misc"), DescriptionAttribute("Volume of Song audio.")]
        public float SongVolume
        {
            get { return _song.SongVolume; }
            set { _song.SongVolume = value; }
        }


        [CategoryAttribute("2. Misc"), DescriptionAttribute("Male or Female.")]
        public Singer Singer
        {
            get { return _song.Singer; }
            set { _song.Singer = value; }
        }

        [CategoryAttribute("3. Audio"), ReadOnly(true), DescriptionAttribute("Guitar audio file. If not set the song audio file will be used.")]
        public string GuitarFilename
        {
            get { return _song.Audio.GuitarFile == null ? string.Empty : _song.Audio.GuitarFile.Name; }
            //set { _song.Audio.GuitarFilename = value; }
        }


        [CategoryAttribute("3. Audio"), ReadOnly(true), DescriptionAttribute("Song audio file. This item must be set.")]
        public string SongFilename
        {
            get
            {
                StringBuilder files = new StringBuilder();
                foreach (AudioFile af in _song.Audio.SongFiles)
                {
                    if (files.Length != 0)
                        files.Append("|");
                    files.Append(af.Name);
                }
                return files.ToString();
                //return _song.Audio.SongFilenames;
            }
            //set { _song.Audio.SongFilenames = value; }
        }


        [CategoryAttribute("3. Audio"), ReadOnly(true), DescriptionAttribute("Rhythm audio file. If not set this item will be silent.")]
        public string RhythmFilename
        {
            get { return _song.Audio.RhythmFile == null ? string.Empty : _song.Audio.RhythmFile.Name; }
            //set { _song.Audio.RhythmFilename = value; }
        }


        private ProjectSong _song;
    }
}
