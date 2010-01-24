using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class GameInfo
    {
        static GameInfo()
        {
            _languages = new Dictionary<string, string>();

            _languages.Add("", "English (Default)");
            _languages.Add("_f", "French");
            _languages.Add("_g", "German");
            _languages.Add("_i", "Italian");
            _languages.Add("_s", "Spanish");

            _games = new Dictionary<Game, string>();
            _games.Add(Game.GH3_Wii, "GH 3 (Wii)");
            _games.Add(Game.GHA_Wii, "GH Aerosmith (Wii)");
            //_games.Add(Game.GH3_PS2, "GH 3 (PS2)");
            //_games.Add(Game.GH3_1_3_PC, "GH 3 v1.3 (PC)");
        }

        internal GameInfo(Game game, string languageId)
        {
            this.Game = game;

            this.LanguageId = languageId;

            switch (game)
            {
                case Game.GH3_Wii:
                    FileExtension = "ngc";
                    HasQbPab = false;
                    MidTextInQbPak = false;
                    GuitarProgressionQbFilename = string.Format(@"cripts\guitar\guitar_progression.qb.{0}", FileExtension);
                    GuitarCoOpQbFilename = string.Format(@"cripts\guitar\guitar_coop.qb.{0}", FileExtension);
                    SonglistQbFilename = string.Format(@"cripts\guitar\songlist.qb.{0}", FileExtension);
                    StoreDataQbFilename = string.Format(@"cripts\guitar\store_data.qb.{0}", FileExtension);
                    this.PakFormatType = PakFormatType.Wii;
                    StreamDat = string.Format(@"streams/streamall.dat.{0}", FileExtension);
                    StreamWad = string.Format(@"streams/streamall.wad.{0}", FileExtension);
                    QbPakFilename = string.Format(@"pak/qb{0}.pak.{1}", languageId, FileExtension);
                    QbPabFilename = string.Empty;
                    DbgPakFilename = string.Format(@"pak/dbg.pak.{0}", FileExtension);
                    break;
                case Game.GHA_Wii:
                    FileExtension = "ngc";
                    HasQbPab = false;
                    MidTextInQbPak = true;
                    GuitarProgressionQbFilename = string.Format(@"scripts\guitar\guitar_progression.qb.{0}", FileExtension);
                    GuitarCoOpQbFilename = string.Format(@"scripts\guitar\guitar_coop.qb.{0}", FileExtension);
                    SonglistQbFilename = string.Format(@"scripts\guitar\songlist.qb.{0}", FileExtension);
                    StoreDataQbFilename = string.Format(@"scripts\guitar\store_data.qb.{0}", FileExtension);
                    this.PakFormatType = PakFormatType.Wii;
                    StreamDat = string.Format(@"streams/streamall.dat.{0}", FileExtension);
                    StreamWad = string.Format(@"streams/streamall.wad.{0}", FileExtension);
                    QbPakFilename = string.Format(@"pak/qb{0}.pak.{1}", languageId, FileExtension);
                    QbPabFilename = string.Empty;
                    DbgPakFilename = string.Format(@"pak/dbg.pak.{0}", FileExtension);
                    break;
                case Game.GH3_PS2: //FILLER VALUES
                    FileExtension = "ps2";
                    HasQbPab = true;
                    MidTextInQbPak = true;
                    GuitarProgressionQbFilename = string.Format(@"scripts\guitar\guitar_progression.qb.{0}", FileExtension);
                    GuitarCoOpQbFilename = string.Format(@"scripts\guitar\guitar_coop.qb.{0}", FileExtension);
                    SonglistQbFilename = string.Format(@"scripts\guitar\songlist.qb.{0}", FileExtension);
                    StoreDataQbFilename = string.Format(@"scripts\guitar\store_data.qb.{0}", FileExtension);
                    this.PakFormatType = PakFormatType.PS2;
                    StreamDat = string.Format(@"streams/streamall.dat.{0}", FileExtension);
                    StreamWad = string.Format(@"streams/streamall.wad.qb.{0}", FileExtension);
                    QbPakFilename = string.Format(@"pak\qb{0}.pak.{1}", languageId, FileExtension);
                    QbPabFilename = string.Empty;
                    DbgPakFilename = string.Format(@"pak\dbg.pak.{0}", FileExtension);
                    break;
                case Game.GH3_1_3_PC: //FILLER VALUES
                    FileExtension = "xen";
                    HasQbPab = true;
                    MidTextInQbPak = true;
                    GuitarProgressionQbFilename = string.Format(@"scripts\guitar\guitar_progression.qb.{0}", FileExtension);
                    GuitarCoOpQbFilename = string.Format(@"scripts\guitar\guitar_coop.qb.{0}", FileExtension);
                    SonglistQbFilename = string.Format(@"scripts\guitar\songlist.qb.{0}", FileExtension);
                    StoreDataQbFilename = string.Format(@"scripts\guitar\store_data.qb.{0}", FileExtension);
                    this.PakFormatType = PakFormatType.PC;
                    StreamDat = string.Format(@"streams/streamall.dat.{0}", FileExtension);
                    StreamWad = string.Format(@"streams/streamall.wad.qb.{0}", FileExtension);
                    QbPakFilename = string.Format(@"pak\qb{0}.pak.{1}", languageId, FileExtension);
                    QbPabFilename = string.Empty;
                    DbgPakFilename = string.Format(@"pak\dbg.pak.{0}", FileExtension);
                    break;
                default:
                    throw new ApplicationException(string.Format("'{0}' is not a known Game type", game.ToString()));
            }




        }

        /// <summary>
        /// DAT file for wii, ISF file for PS2
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public string GetMusicHeaderFilename(QbKey song)
        {
            if (this.Game == Game.GH3_Wii || this.Game == Game.GHA_Wii)
            {
                if (!song.HasText)
                {
                    song = findMissingText(song);
                    if (!song.HasText)
                        throw new ApplicationException(string.Format("Missing text in Song QbKey '{0}'", song.Crc.ToString("X")));
                }
                return string.Format(@"music/{0}.dat.{1}", song.Text, FileExtension);
            }
            else if (this.Game == Game.GH3_PS2)
            {
                string crc = song.Crc.ToString("X").PadLeft(8, '0');
                return string.Format(@"music/{0}/{1}.ISF", crc[0], crc);
            }
            else
                throw new ApplicationException(string.Format("'{0}' is not a supported Game type", this.Game.ToString()));

        }

        /// <summary>
        /// WAD file for wii, IMF file for PS2
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public string GetMusicFilename(QbKey song)
        {
            if (this.Game == Game.GH3_Wii || this.Game == Game.GHA_Wii)
            {
                if (!song.HasText)
                {
                    song = findMissingText(song);
                    if (!song.HasText)
                        throw new ApplicationException(string.Format("Missing text in Song QbKey '{0}'", song.Crc.ToString("X")));
                }
                return string.Format(@"music/{0}.wad.{1}", song.Text, FileExtension);
            }
            else if (this.Game == Game.GH3_PS2)
            {
                string crc = song.Crc.ToString("X").PadLeft(8, '0');
                return string.Format(@"music/{0}/{1}.IMF", crc[0], crc);
            }
            else
                throw new ApplicationException(string.Format("'{0}' is not a supported Game type", this.Game.ToString()));
        }

        public string GetNotesFilename(QbKey song)
        {
            if (this.Game == Game.GH3_Wii)
            {
                if (!song.HasText)
                {
                    song = findMissingText(song);
                    if (!song.HasText)
                        throw new ApplicationException(string.Format("Missing text in Song QbKey '{0}'", song.Crc.ToString("X")));
                }
                return string.Format(@"songs/{0}{1}.pak.{2}", song.Text, this.LanguageId, FileExtension);
            }
            else if (this.Game == Game.GHA_Wii)
            {
                if (!song.HasText)
                    throw new ApplicationException(string.Format("Missing text in Song QbKey '{0}'", song.Crc));
                return string.Format(@"songs/{0}.pak.{1}", song.Text, FileExtension);
            }
            else if (this.Game == Game.GH3_PS2)
            {
                return null;
            }
            else
                throw new ApplicationException(string.Format("'{0}' is not a supported Game type", this.Game.ToString()));
        }

        public string GetNotesQbFilename(QbKey song)
        {
            if (this.Game == Game.GH3_Wii || this.Game == Game.GHA_Wii)
            {
                if (!song.HasText)
                {
                    song = findMissingText(song);
                    if (!song.HasText)
                        throw new ApplicationException(string.Format("Missing text in Song QbKey '{0}'", song.Crc.ToString("X")));
                }
                return string.Format(@"data/songs/{0}.mid.qb.{1}", song.Text, FileExtension);
            }
            else if (this.Game == Game.GH3_PS2)
            {
                return null;
            }
            else
                throw new ApplicationException(string.Format("'{0}' is not a supported Game type", this.Game.ToString()));
        }

        public string GetNotesTextQbFilename(QbKey song)
        {
            if (this.Game == Game.GH3_Wii || this.Game == Game.GHA_Wii)
            {
                if (!song.HasText)
                {
                    song = findMissingText(song);
                    if (!song.HasText)
                        throw new ApplicationException(string.Format("Missing text in Song QbKey '{0}'", song.Crc));
                }
                if (this.Game == Game.GHA_Wii)
                    return string.Format(@"songs\{0}.mid_text.qb.{1}", song.Text, FileExtension);
                else //GH3
                    return string.Format(@"data/songs/{0}.mid_text.qb.{1}", song.Text, FileExtension);
            }
            else if (this.Game == Game.GH3_PS2)
            {
                return null;
            }
            else
                throw new ApplicationException(string.Format("'{0}' is not a supported Game type", this.Game.ToString()));
        }

        private QbKey findMissingText(QbKey song)
        {
            for (int i = 1; i < 100; i++)
            {
                if (song.Crc == QbKey.Create(string.Format("theghost{0}", i.ToString().PadLeft(3, '0'))).Crc)
                    song = QbKey.Create(string.Format("theghost{0}", i.ToString().PadLeft(3, '0')));
            }
            return song;
        }

        public static string[] GameStrings
        {
            get
            {
                string[] v = new string[_games.Count];
                if (v.Length != 0)
                    _games.Values.CopyTo(v, 0);
                return v;
            }
        }

        public static string GameEnumToString(Game game)
        {
            if (_games.ContainsKey(game))
                return _games[game];
            else
                throw new ApplicationException(string.Format("'{0}' is not a known Game type", game.ToString()));
        }

        public static Game GameStringToEnum(string game)
        {
            foreach (Game key in _games.Keys)
            {
                if (_games[key].ToLower() == game.ToLower())
                    return key;
            }

            throw new ApplicationException(string.Format("'{0}' is not a known Game string", game));
        }

        public static string[] LanguageStrings
        {
            get
            {
                string[] v = new string[_languages.Count]; 
                if (v.Length != 0)
                    _languages.Values.CopyTo(v, 0);
                return v;
            }
        }

        public static string LanguageIdToString(string languageId)
        {
            if (_languages.ContainsKey(languageId))
                return _languages[languageId];
            else
                throw new ApplicationException(string.Format("'{0}' is not a known Language ID type", languageId));
        }

        public static string LanguageStringToId(string languageString)
        {
            foreach (string key in _languages.Keys)
            {
                if (_languages[key].ToLower() == languageString.ToLower())
                    return key;
            }

            throw new ApplicationException(string.Format("'{0}' is not a known Language string", languageString));
        }

        public readonly string LanguageId;

        public readonly bool HasQbPab;
        public readonly string QbPakFilename;
        public readonly string QbPabFilename;
        public readonly string DbgPakFilename;
        public readonly string FileExtension;

        public readonly string GuitarProgressionQbFilename;
        public readonly string GuitarCoOpQbFilename;
        public readonly string SonglistQbFilename;
        public readonly string StoreDataQbFilename;

        public readonly string StreamDat;
        public readonly string StreamWad;

        public readonly bool MidTextInQbPak;

        public Game Game { get; set; }
        public PakFormatType PakFormatType { get; set; }

        private static Dictionary<string, string> _languages;
        private static Dictionary<Game, string> _games;


    }
}
