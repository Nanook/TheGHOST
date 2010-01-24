using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;


namespace Nanook.TheGhost
{
    internal enum YesNo
    {
        Yes, No
    }

    internal enum Presets
    {
        FullPrep,
        CompactIso,
        OnlyAddSongs,
        OnlySaveSpace
    }


    [DefaultPropertyAttribute("Title")]
    internal class PrepProperties
    {
        internal PrepProperties(PropertyGrid prpGrd)
        {
            _gh3 = true;
            _tierSongs = null;
            SetPreset(Presets.FullPrep);
            _prpGrd = prpGrd;
        }

        //Handy attributes to use on properties
        //[Browsable(false)]
        //[ReadOnly(true)]

        internal void SetPreset(Presets preset)
        {
            YesNo y = YesNo.Yes;
            YesNo n = YesNo.No;

            bool full = preset == Presets.FullPrep;
            bool song = preset == Presets.OnlyAddSongs;
            bool space = preset == Presets.OnlySaveSpace;

            this.EnableAudioBlanking = (full ? y : n);
            this.BlankTierSongs = (full ? y : n);
            this.BlankBonus = (full ? y : n);
            this.BlankAddedSongs = (full ? y : n);
            this.BlankNonCareerSongs = (full ? y : n);
            this.BlankBattles = (full ? y : n);

            this.EnableTierEditing = (full || song ? y : n);
            this.TiersCount = string.Empty;
            this.TierSongsCount = string.Empty;
            this.BonusTierSongsCount = string.Empty;
            this.RemoveBossBattles = YesNo.No;

            this.UnlockSetlistTiers = (full ? y : n);
            this.CompleteTier1Song = (full ? y : n);
            this.AddNonCareerTracksToBonus = (full || song ? y : n);
            this.SetCheats = (full ? y : n);
            this.FreeStore = (full ? y : n);
            this.DefaultBonusSongArt = (full ? y : n);
            this.DefaultBonusSongInfo = (full ? y : n);
            this.DefaultBonusSongInfoText = "Custom song replaced with TheGHOST";

            this.ReplaceVideos = (full || space ? y : n);
            this.RemoveIntroVideos = (full || space ? y : n);
            this.RemoveUnusedFiles = (full || space ? y : n);
            this.RemoveOtherLanguages = (full || space ? y : n);
            this.RemoveUpdatePartition = (full || space ? y : n);
            this.ManualEditing = n;
        }

        [CategoryAttribute("1. Audio Blanking"), DescriptionAttribute("Blanks song audio to optimise the ISO for song replacement.")]
        public YesNo EnableAudioBlanking { get; set; }

        [CategoryAttribute("1. Audio Blanking"), DescriptionAttribute("Blanks all songs in the career tiers.  Requires EnableAudioBlanking to be Yes.")]
        public YesNo BlankTierSongs { get; set; }

        [CategoryAttribute("1. Audio Blanking"), DescriptionAttribute("Blanks Bonus songs.  Requires EnableAudioBlanking to be Yes.")]
        public YesNo BlankBonus { get; set; }

        [CategoryAttribute("1. Audio Blanking"), DescriptionAttribute("Blanks song added with TheGHOST.  Newly added songs will be blank by default.  Requires EnableAudioBlanking to be Yes.")]
        public YesNo BlankAddedSongs { get; set; }

        [CategoryAttribute("1. Audio Blanking"), DescriptionAttribute("Blanks songs that are not part of the career.  Newly added songs will be blank by default.  Requires EnableAudioBlanking to be Yes.")]
        public YesNo BlankNonCareerSongs { get; set; }

        [CategoryAttribute("1. Audio Blanking"), DescriptionAttribute("Blanks Battle songs in all above items.  Requires EnableAudioBlanking to be Yes.")]
        public YesNo BlankBattles { get; set; }



        [CategoryAttribute("2. Tier Editing"), DescriptionAttribute("Allow manipulations of the tiers.  The new songs can be purchased from the store or enabled with the cheats. Read the FAQ for details.")]
        public YesNo EnableTierEditing { get; set; }

        [CategoryAttribute("2. Tier Editing"), DescriptionAttribute("Removes the boss battles, this is forced to yes if TiersCount is set.")]
        public YesNo RemoveBossBattles
        {
            get { return _removeBossBattles; }
            set
            {
                if (_removeBossBattles == value)
                    return;
                _removeBossBattles = value;
                AddNonCareerTracksToBonus = YesNo.Yes;
                refresh();
            }
        }

        [CategoryAttribute("2. Tier Editing"), DescriptionAttribute("Add the Non Career songs to the bonus tier, this is forced to yes if TiersCount is set.")]
        public YesNo AddNonCareerTracksToBonus { get; set; }

        [CategoryAttribute("2. Tier Editing"), DescriptionAttribute("Sets the amount of Tiers for all set lists.")]
        public string TiersCount
        {
            get { return _tiersCount; }
            set
            {
                if (_tiersCount == value)
                    return;
                _tiersCount = value;
                if (_tiersCount.Length != 0)
                {
                    RemoveBossBattles = YesNo.Yes;
                    AddNonCareerTracksToBonus = YesNo.Yes;
                }
                refresh();
            }
        }

        [CategoryAttribute("2. Tier Editing"), DescriptionAttribute("Sets the amount of songs in each career tier. Blank will not edit them. Set as '5' to set them all or individually with '5,5,5,5,6,6,6,6'. Setting this will force the removal of the Boss Battle Songs")]
        public string TierSongsCount
        {
            get { return _tierSongsCount; }
            set
            {
                if (_tierSongsCount == value)
                    return;
                _tierSongsCount = value;
                if (_tierSongsCount.Length != 0)
                {
                    RemoveBossBattles = YesNo.Yes;
                    AddNonCareerTracksToBonus = YesNo.Yes;
                }
                refresh();
            }
        }

        [CategoryAttribute("2. Tier Editing"), DescriptionAttribute("Sets the amount of songs in the bonus tier. This value does not include Non Career songs to be added if AddNonCareerTracksToBonus is set to Yes and they have not already been applied.")]
        public string BonusTierSongsCount
        {
            get { return _bonusTierSongsCount; }
            set
            {
                if (_bonusTierSongsCount == value)
                    return;

                int x;

                if (value.Length == 0 || int.TryParse(value, out x))
                    _bonusTierSongsCount = value;
                else
                    _bonusTierSongsCount = string.Empty;
                refresh();
            }
        }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Open all the career, quickplay and coop tiers etc ready for play without unlocking.  GH does not see these tiers as completed.")]
        public YesNo UnlockSetlistTiers { get; set; }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Complete a tier by completing 1 song.")]
        public YesNo CompleteTier1Song { get; set; }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Sets the cheats to 1 note each (G, GR, R, RY, Y, YB, B, BO, O).  Use this to unlock all the characters, costumes, instruments and songs without purchasing them.")]
        public YesNo SetCheats { get; set; }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Set all the prices in the store to $0.")]
        public YesNo FreeStore { get; set; }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Defaults all the bonus song art to default picture.")]
        public YesNo DefaultBonusSongArt { get; set; }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Defaults all the bonus song store info to the text below.")]
        public YesNo DefaultBonusSongInfo { get; set; }

        [CategoryAttribute("3. Hacks"), DescriptionAttribute("Defaults all the bonus song store info to this value.")]
        public string DefaultBonusSongInfoText { get; set; }


        [CategoryAttribute("4. File Editing"), DescriptionAttribute("Replace all the videos for a small filler video to free up lots of space in the ISO.")]
        public YesNo ReplaceVideos { get; set; }

        [CategoryAttribute("4. File Editing"), DescriptionAttribute("Remove the many intro videos and only show the last one.  This is done automatically if ReplaceVideos is Yes")]
        public YesNo RemoveIntroVideos { get; set; }

        [CategoryAttribute("4. File Editing"), DescriptionAttribute("Remove files from the ISO that are not used by the game to free up more space in the ISO.")]
        public YesNo RemoveUnusedFiles { get; set; }

        [CategoryAttribute("4. File Editing"), DescriptionAttribute("Remove all languages other than the one specified above.  TheGHOST only replaces notes for one language anyway.")]
        public YesNo RemoveOtherLanguages { get; set; }

        [CategoryAttribute("4. File Editing"), DescriptionAttribute("Removes all files in the update partition.  Some mod chips have issues with this and won't boot the disc.  The alternative is to BrickBlock the ISO then set this to No. ")]
        public YesNo RemoveUpdatePartition { get; set; }

        [CategoryAttribute("4. File Editing"), DescriptionAttribute("Allows you to edit the ISO file system before it get inserted back in to the ISO.  You can also run TheGHOST with the 'File System' plugin. Do what you like, but be careful.")]
        public YesNo ManualEditing { get; set; }


        [Browsable(false)]
        public bool IsGh3
        {
            get { return _gh3; }
            set
            {
                if (value == _gh3)
                    return;
                _gh3 = value;
                refresh();
            }
        }

        [Browsable(false)]
        public int[] TierSongs
        {
            get
            {
                if (_tierSongs == null)
                {
                    int tiers = (this.Tiers != 0 ? this.Tiers : (_gh3 ? 8 : 6));
                    int def = 5; //default tier song count

                    if (this.TierSongsCount == null || this.TierSongsCount.Length == 0)
                        return null;

                    string[] s = this.TierSongsCount.Replace(" ", "").Split(',');
                    _tierSongs = new int[tiers];

                    int val;
                         
                    for (int i = 0; i < tiers; i++)
                    {
                        try
                        {
                            if (int.TryParse(s[Math.Min(s.Length - 1, i)], out val))
                                _tierSongs[i] = val;
                            else
                                _tierSongs[i] = def;
                        }
                        catch
                        {
                            _tierSongs[i] = def;
                        }
                    }

                    _totalSongs = this.BonusSongs;
                    foreach (int x in _tierSongs)
                        _totalSongs += x;

                    if (_prpGrd != null)
                        _prpGrd.Refresh();
                }

                return _tierSongs;
            }
        }

        [Browsable(false)]
        public int BonusSongs
        {
            get
            {
                int x;
                if (_bonusTierSongsCount.Length == 0)
                    return 0;
                else if (int.TryParse(_bonusTierSongsCount, out x))
                    return x;
                else
                    return 0;
            }
        }

        [Browsable(false)]
        public int Tiers
        {
            get
            {
                int x;
                if (_tiersCount.Length == 0)
                    return 0;
                else if (int.TryParse(_tiersCount, out x))
                    return x;
                else
                    return 0;
            }
        }



        [Browsable(false)]
        public int TierSongsTotalCount
        {
            get
            {
                int[] c = this.TierSongs;

                if (c != null)
                {
                    int t = 0;
                    foreach (int i in c)
                        t += i;
                    return t;
                }
                else
                    return 0;
            }
        }


        private void refresh()
        {
            _tierSongs = null;
            int[] x = this.TierSongs;
            if (_prpGrd != null)
                _prpGrd.Refresh();
        }


        private string _tiersCount;
        private string _tierSongsCount;
        private string _bonusTierSongsCount;
        private YesNo _removeBossBattles;

        private int[] _tierSongs;
        private int _totalSongs;

        private bool _gh3;

        private PropertyGrid _prpGrd;
    }
}
