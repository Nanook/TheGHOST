using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class SongQb
    {
        internal SongQb(Project project, QbItemStruct song)
        {
            _song = song;
            QbItemBase temp;

            _hopoMeasure = null;


            _checksum = (QbItemQbKey)song.FindItem(QbKey.Create("checksum"), false);
            //_key = song.ItemQbKey;
            _key = QbKey.Create(((QbItemString)song.FindItem(QbKey.Create("name"), false)).Strings[0]);

            _title = (QbItemString)song.FindItem(QbKey.Create("title"), false);
            _artist = (QbItemString)song.FindItem(QbKey.Create("artist"), false);

            _leaderboard = (QbItemInteger)song.FindItem(QbKey.Create("leaderboard"), false);
            _year = (QbItemString)song.FindItem(QbKey.Create("year"), false);

            _boss = song.FindItem(QbKey.Create("boss"), false) as QbItemQbKey;  //is this a boss

            temp = song.FindItem(QbKey.Create("band_playback_volume"), false);  //sometimes integer instead of float
            if (temp == null)
            {
                _songVolume = new QbItemFloat(song.Root);
                _songVolume.Create(QbItemType.StructItemFloat);
                _songVolume.ItemQbKey = QbKey.Create("band_playback_volume");
                _songVolume.Values = new float[1];
                _songVolume.Values[0] = 0;
                song.AddItem(_songVolume);
                song.Root.AlignPointers();
            }
            else if (temp is QbItemInteger)
                _songVolume = replaceItemIntForFloat(song, (QbItemInteger)temp);
            else
                _songVolume = (QbItemFloat)temp;

            temp = song.FindItem(QbKey.Create("guitar_playback_volume"), false);  //sometimes integer instead of float
            if (temp == null)
            {
                _guitarVolume = new QbItemFloat(song.Root);
                _guitarVolume.Create(QbItemType.StructItemFloat);
                _guitarVolume.ItemQbKey = QbKey.Create("guitar_playback_volume");
                _guitarVolume.Values = new float[1];
                _guitarVolume.Values[0] = 0;
                song.AddItem(_guitarVolume);
                song.Root.AlignPointers();
            }
            else if (temp is QbItemInteger)
                _guitarVolume = replaceItemIntForFloat(song, (QbItemInteger)temp);
            else
                _guitarVolume = (QbItemFloat)temp;

            _rhythmTrack = (QbItemInteger)song.FindItem(QbKey.Create("rhythm_track"), false);

            temp = song.FindItem(QbKey.Create("artist_text"), false);
            if (temp is QbItemString)
            {
                _artistTextString = (QbItemString)temp;
                _artistText = null;
            }
            else
            {
                _artistText = song.FindItem(QbKey.Create("artist_text"), false) as QbItemQbKey;
                _artistTextString = null;
            }

            _originalArtist = (QbItemInteger)song.FindItem(QbKey.Create("original_artist"), false);

            temp = song.FindItem(QbKey.Create("singer"), false);
            if (temp == null) //GH3 wii Cream sunshine for your love has no singer item
            {
                _singer = new QbItemQbKey(song.Root);
                _singer.Create(QbItemType.StructItemQbKey);
                _singer.ItemQbKey = QbKey.Create("singer");
                _singer.Values = new QbKey[1];
                _singer.Values[0] = QbKey.Create("male");
                song.AddItem(_singer);
                song.Root.AlignPointers();
            }
            else
                _singer = (QbItemQbKey)temp;

            _fretbarOffset = song.FindItem(QbKey.Create("fretbar_offset"), false) as QbItemInteger;
            _gemOffset = song.FindItem(QbKey.Create("gem_offset"), false) as QbItemInteger;
            _inputOffset = song.FindItem(QbKey.Create("input_offset"), false) as QbItemInteger;

            //this fixes an issue with GH3(cult of personality) and GHA (talk talking)
            if (this.key.Crc != this.checksum.Crc)
                _id = (project.GameInfo.Game == Game.GH3_Wii) ? this.checksum : this.key;
            else
                _id = this.checksum.HasText ? this.checksum : this.key;
        }

        /// <summary>
        /// Some items are not the correct type, replace them.
        /// </summary>
        /// <param name="song"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private QbItemFloat replaceItemIntForFloat(QbItemStruct song, QbItemInteger item)
        {
            QbItemFloat f = new QbItemFloat(song.Root);
            f.Create(QbItemType.StructItemFloat);

            f.ItemQbKey = item.ItemQbKey;
            f.Values[0] = item.Values[0];
            song.InsertItem(f, item, true);
            song.RemoveItem(item);
            song.Root.AlignPointers();
            return f;
        }

        public void ZeroOffsets()
        {
            if (_fretbarOffset != null)
                _fretbarOffset.Values[0] = 0;
            if (_gemOffset != null)
                _gemOffset.Values[0] = 0;
            if (_inputOffset != null)
                _inputOffset.Values[0] = 0;
        }
            

        public void RemoveUseCoopNoteTracks()
        {
            //remove use_coop_notetracks if exists (if no coop tracks are present then only first 2 frets are shown)
            QbItemBase temp = _song.FindItem(false, delegate(QbItemBase item)
            {
                return (item.QbItemType == QbItemType.StructItemQbKey && ((QbItemQbKey)item).Values[0].Crc == QbKey.Create("use_coop_notetracks").Crc);
            });
            if (temp != null)
                _song.RemoveItem(temp);
        }

        private QbKey key
        {
            get { return _key; }
        }

        private QbKey checksum
        {
            get { return _checksum.Values[0]; }
        }

        public bool IsBoss
        {
            get { return (_boss != null); }
        }

        public string Title
        {
            get { return _title.Strings[0]; }
            set { _title.Strings[0] = value; }
        }

        public string Artist
        {
            get { return _artist.Strings[0]; }
            set { _artist.Strings[0] = value; }
        }

        public string Year
        {
            get { return _year.Strings[0].Length < 2 ? string.Empty : _year.Strings[0].Substring(2); }
            set
            {
                if (value.Length != 0)
                    _year.Strings[0] = string.Concat(", ", value);
                else
                    _year.Strings[0] = string.Empty;
            }
        }

        public float HopoMeasure
        {
            get
            {
                if (_hopoMeasure == null)
                    return 2.95F;
                else
                    return _hopoMeasure.Values[0];
            }
            set
            {
                QbItemBase item = _song.FindItem(QbKey.Create("hammer_on_measure_scale"), false);

                if (item != null && item.QbItemType == QbItemType.StructItemFloat)
                    _hopoMeasure = (QbItemFloat)item;
                else
                {
                    _hopoMeasure = new QbItemFloat(_song.Root);
                    _hopoMeasure.Create(QbItemType.StructItemFloat);
                    _hopoMeasure.Values = new float[1];
                    _hopoMeasure.ItemQbKey = QbKey.Create("hammer_on_measure_scale");

                    _song.AddItem(_hopoMeasure); //insert the "by" qbkey string

                    if (item != null)
                        _song.RemoveItem(item); //remove the original artist text (string version)
                    _song.Root.AlignPointers();
                }

                _hopoMeasure.Values[0] = value;
            }
        }

        public float SongVolume
        {
            get { return _songVolume.Values[0]; }
            set { _songVolume.Values[0] = value; }
        }

        public float GuitarVolume
        {
            get { return _guitarVolume.Values[0]; }
            set { _guitarVolume.Values[0] = value; }
        }

        private int Leaderboard
        {
            get { return (int)_leaderboard.Values[0]; }
            //set { _leaderboard.Values[0] = value; }
        }

        public bool RhythmTrack
        {
            get { return (int)_rhythmTrack.Values[0] == 1; }
            set { _rhythmTrack.Values[0] = (uint)(value ? 1 : 0); }
        }

        public bool OriginalArtist
        {
            get { return (int)_originalArtist.Values[0] == 1; }
            set
            {
                if (_originalArtist == null)
                    return;

                if (_artistTextString != null)
                {
                    _artistText = new QbItemQbKey(_song.Root);
                    _artistText.Create(QbItemType.StructItemQbKeyString);
                    _artistText.Values = new QbKey[1];
                    _artistText.ItemQbKey = QbKey.Create("artist_text");

                    _song.InsertItem(_artistText, _artistTextString, true); //insert the "by" qbkey string
                    _song.RemoveItem(_artistTextString); //remove the original artist text (string version)
                    _song.Root.AlignPointers();
                }

                _originalArtist.Values[0] = (uint)(value ? 1 : 0);
                if (_artistText != null)
                    _artistText.Values[0] = QbKey.Create(value ? "artist_text_by" : "artist_text_as_made_famous_by");


                QbItemBase item = _song.FindItem(QbKey.Create("covered_by"), false);
                if (item != null && value)
                {
                    QbItemString s = (QbItemString)item;
                    _song.RemoveItem(s);
                    _song.Root.AlignPointers();
                }
            }
        }

        public QbKey ArtistText
        {
            get { return _artistText.Values[0]; }
            set { _artistText.Values[0] = value; }
        }

        public Singer Singer
        {
            get { return _singer.Values[0] == QbKey.Create("female") ? Singer.Female : Singer.Male; }
            set { _singer.Values[0] = (value == Singer.Female ? QbKey.Create("female") : QbKey.Create("male")); }
        }

        public static QbItemStruct CreateSong(QbFile root, string songname)
        {
            QbItemStruct song = new QbItemStruct(root);
            song.Create(QbItemType.StructItemStruct);
            song.ItemQbKey = QbKey.Create(songname);
            
            QbItemQbKey checksum = new QbItemQbKey(root);
            checksum.Create(QbItemType.StructItemQbKey);
            checksum.ItemQbKey = QbKey.Create("checksum");
            checksum.Values = new QbKey[] { QbKey.Create(songname) };
            song.AddItem(checksum);

            QbItemString name = new QbItemString(root);
            name.Create(QbItemType.StructItemString);
            name.ItemQbKey = QbKey.Create("name");
            name.Strings = new string[] { songname };
            song.AddItem(name);

            QbItemString title = new QbItemString(root);
            title.Create(QbItemType.StructItemString);
            title.ItemQbKey = QbKey.Create("title");
            title.Strings = new string[] { songname };
            song.AddItem(title);

            QbItemString artist = new QbItemString(root);
            artist.Create(QbItemType.StructItemString);
            artist.ItemQbKey = QbKey.Create("artist");
            artist.Strings = new string[] { songname };
            song.AddItem(artist);

            QbItemString year = new QbItemString(root);
            year.Create(QbItemType.StructItemString);
            year.ItemQbKey = QbKey.Create("year");
            year.Strings = new string[] { string.Empty };
            song.AddItem(year);

            QbItemQbKey artistText = new QbItemQbKey(root);
            artistText.Create(QbItemType.StructItemQbKeyString);
            artistText.ItemQbKey = QbKey.Create("artist_text");
            artistText.Values = new QbKey[] { QbKey.Create("artist_text_by") };
            song.AddItem(artistText);

            QbItemInteger originalArtist = new QbItemInteger(root);
            originalArtist.Create(QbItemType.StructItemInteger);
            originalArtist.ItemQbKey = QbKey.Create("original_artist");
            originalArtist.Values = new uint[] { 1 };
            song.AddItem(originalArtist);

            QbItemQbKey version = new QbItemQbKey(root);
            version.Create(QbItemType.StructItemQbKey);
            version.ItemQbKey = QbKey.Create("version");
            version.Values = new QbKey[] { QbKey.Create("gh3") };
            song.AddItem(version);

            QbItemInteger leaderboard = new QbItemInteger(root);
            leaderboard.Create(QbItemType.StructItemInteger);
            leaderboard.ItemQbKey = QbKey.Create("leaderboard");
            leaderboard.Values = new uint[] { 1 };
            song.AddItem(leaderboard);

            QbItemInteger gemOffset = new QbItemInteger(root);
            gemOffset.Create(QbItemType.StructItemInteger);
            gemOffset.ItemQbKey = QbKey.Create("gem_offset");
            gemOffset.Values = new uint[] { 0 };
            song.AddItem(gemOffset);

            QbItemInteger inputOffset = new QbItemInteger(root);
            inputOffset.Create(QbItemType.StructItemInteger);
            inputOffset.ItemQbKey = QbKey.Create("input_offset");
            inputOffset.Values = new uint[] { 0 };
            song.AddItem(inputOffset);

            QbItemQbKey singer = new QbItemQbKey(root);
            singer.Create(QbItemType.StructItemQbKey);
            singer.ItemQbKey = QbKey.Create("singer");
            singer.Values = new QbKey[] { QbKey.Create("male") };
            song.AddItem(singer);

            QbItemQbKey keyboard = new QbItemQbKey(root);
            keyboard.Create(QbItemType.StructItemQbKey);
            keyboard.ItemQbKey = QbKey.Create("keyboard");
            keyboard.Values = new QbKey[] { QbKey.Create("false") };
            song.AddItem(keyboard);

            QbItemFloat bandPlaybackVolume = new QbItemFloat(root);
            bandPlaybackVolume.Create(QbItemType.StructItemFloat);
            bandPlaybackVolume.ItemQbKey = QbKey.Create("band_playback_volume");
            bandPlaybackVolume.Values = new float[] { 0F };
            song.AddItem(bandPlaybackVolume);

            QbItemFloat guitarPlaybackVolume = new QbItemFloat(root);
            guitarPlaybackVolume.Create(QbItemType.StructItemFloat);
            guitarPlaybackVolume.ItemQbKey = QbKey.Create("guitar_playback_volume");
            guitarPlaybackVolume.Values = new float[] { 0F };
            song.AddItem(guitarPlaybackVolume);

            QbItemString countOff = new QbItemString(root);
            countOff.Create(QbItemType.StructItemString);
            countOff.ItemQbKey = QbKey.Create("countoff");
            countOff.Strings = new string[] { "sticks_normal" };
            song.AddItem(countOff);

            QbItemInteger rhythmTrack = new QbItemInteger(root);
            rhythmTrack.Create(QbItemType.StructItemInteger);
            rhythmTrack.ItemQbKey = QbKey.Create("rhythm_track");
            rhythmTrack.Values = new uint[] { 0 };
            song.AddItem(rhythmTrack);

            return song;
        }


        public QbKey Id
        {
            get { return _id; }
        }

        public bool IsSetList
        {
            get { return (this.Leaderboard == 1 || this.IsBoss); }
        }

        private QbKey _id; //this fixes an issue with GH3(cult of personality) and GHA (talk talking)

        private QbItemQbKey _checksum;
        private QbKey _key;

        private QbItemQbKey _boss;
        private QbItemString _title;
        private QbItemString _artist;
        private QbItemString _year;
        private QbItemFloat _hopoMeasure;
        private QbItemFloat _songVolume;
        private QbItemFloat _guitarVolume;
        private QbItemInteger _leaderboard;
        private QbItemInteger _rhythmTrack;
        private QbItemQbKey _artistText;
        private QbItemInteger _originalArtist;
        private QbItemQbKey _singer;
        private QbItemString _artistTextString; //bossdevil = "inspired by"
        private QbItemInteger _fretbarOffset;
        private QbItemInteger _gemOffset;
        private QbItemInteger _inputOffset;

        private QbItemStruct _song;
    }

    public enum Singer
    {
        Male = 0,
        Female = 1
    }
}
