using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Nanook.QueenBee.Parser;
using Nanook.TheGhost;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Nanook.TheGhost
{
    public partial class MainForm : Form
    {
        const int WM_USER = 0x0400;

        [DllImport("WiiScrubber.dll")]
        private static extern bool ChangePartTitle([MarshalAs(UnmanagedType.LPStr)]string isoFileName, int partNo, string title);

        // WiiScrubber.dll imports
        [DllImport("WiiScrubber.dll")]
        static extern bool ShrinkPartition([MarshalAs(UnmanagedType.LPStr)]string isoFileName, int partNo);
        [DllImport("WiiScrubber.dll")]
        static extern bool MaxPartition([MarshalAs(UnmanagedType.LPStr)]string isoFileName, int partNo);

        [DllImport("WiiScrubber.dll")]
        static extern bool ShufflePartitions([MarshalAs(UnmanagedType.LPStr)]string isoFileName);

        [DllImport("WiiScrubber.dll")]
        static extern void ReplaceFiles([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string[] files, string[] paths, int fileCount, bool caseSensitive, bool appendDiffSizeFiles, int partNo, IntPtr hwnd);

        [DllImport("WiiScrubber.dll")]
        static extern void ExtractFiles([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string[] files, string[] dests, int fileCount, bool caseSensitive, int partNo, IntPtr hwnd);

        [DllImport("WiiScrubber.dll")]
        static extern bool ExtractPartition([MarshalAs(UnmanagedType.LPStr)]string isoFileName, int partNo, string outputFolder);

        [DllImport("WiiScrubber.dll")]
        static extern bool LoadPartition([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string imageFileName, int partNo);

        [DllImport("WiiScrubber.dll")]
        static extern bool FileExists([MarshalAs(UnmanagedType.LPStr)]string isoFileName, string findFileName, int partNo, bool caseSensitive);

        [DllImport("WiiScrubber.dll")]
        static extern bool DirectDiscWrite([MarshalAs(UnmanagedType.LPStr)]string isoFileName, int offset, string data, int size);



        public MainForm()
        {
            InitializeComponent();
            chkRenameIso.Visible = false; //ISO IS LOCKED BY WIISCRUBBER

            cboLanguage.Items.Clear();
            cboLanguage.Items.AddRange(GameInfo.LanguageStrings);
            cboLanguage.SelectedIndex = 0;

            _prepProps = new PrepProperties(pgdPrep);

            cboPresets.SelectedIndex = 0;

            pgdPrep.SelectedObject = _prepProps;

            _lastIsoRead = string.Empty;

            _partitionNo = _DataPartition;
            _partitionProven = false;


#if (DEBUG)
            txtIso.Text = @"G:\TheGHOST Projects\nk-gh3stereo.iso";
            rdbGameGh3.Checked = true;

            //txtIso.Text = @"g:\GHA_PAL_ORIG.iso";
            //rdbGameGhA.Checked = true;

            txtWorkingFolder.Text = @"D:\TheGHOST_WF";
#endif
        }

        /// <summary>
        /// Call this when before the ISO is openend with WiiScrubber as it locks the DLL
        /// </summary>
        private void readDiscTitle()
        {
            if (txtIso.Text.ToLower() != _lastIsoRead.ToLower())
                _discTitle = string.Empty;
            else
                return;


            byte[] t = new byte[txtTitleDiscTitle.MaxLength];

            if (txtIso.Text.Length != 0 && File.Exists(txtIso.Text))
            {
                using (FileStream fs = File.OpenRead(txtIso.Text))
                {
                    fs.Seek(0x20, SeekOrigin.Begin);
                    fs.Read(t, 0, t.Length);
                }

                using (FileStream fs = File.OpenRead(txtIso.Text))
                {
                    fs.Seek(0x20, SeekOrigin.Begin);
                    fs.Read(t, 0, t.Length);
                }

                _discTitle = Encoding.Default.GetString(t, 0, t.Length).TrimEnd('\0');
                _lastIsoRead = txtIso.Text;
            }
        }

        private void btnPreset_Click(object sender, EventArgs e)
        {
            switch (cboPresets.SelectedIndex)
            {
                case 1:
                    _prepProps.SetPreset(Presets.CompactIso);
                    break;
                case 2:
                    _prepProps.SetPreset(Presets.OnlyAddSongs);
                    MessageBox.Show(this, "Remember to set TierCount, TierSongsCount and BonusTierSongsCount values.\r\n\r\nRead the FAQ for more information.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 3:
                    _prepProps.SetPreset(Presets.OnlySaveSpace);
                    break;
                default:
                    _prepProps.SetPreset(Presets.FullPrep);
                    break;
            }
            pgdPrep.Refresh();
        }

        private void replaceAudio()
        {
            string silentWav = string.Format(@"{0}\silent.wav", _project.WorkingPath.TrimEnd(new char[] { '\\' }));

            WavProcessor.CreateSilentWav(500, silentWav, false, 44100);

            //we can now use the edit songs collection
            foreach (ProjectSong song in _project.EditSongs)
            {
                FileHelper.Delete(song.Audio.RawPreviewFilename);

                //FileHelper.Delete(song.Audio.RawSongFilenames[0]);

                song.Audio.SongFiles.Add(song.Audio.CreateAudioFile(silentWav));
                song.Audio.Import(song.Audio.SongFiles[0].Name, song.Audio.RawSongFilenames[0]);
                File.Copy(song.Audio.RawSongFilenames[0], song.Audio.RawPreviewFilename); //copy to preview

                prgInc(1);
            }

            //we can now use the edit songs collection
            foreach (ProjectSong song in _project.EditSongs)
            {
                //Apply any padding to notes, audio, frets and generate practise markers
                song.FinaliseSettings();

                //compressed raw audio and packages it up in the format of the game.
                song.Audio.CreateGameAudio(_project.Defaults.ForceMono, _project.Defaults.ForceDownSample, true);  //don't force mono audio

                prgInc(1);
            }

            _project.FileManager.ExportAllChanged(fileCopyProgress);
            Application.DoEvents();

            FileHelper.Delete(silentWav);
            _project.FileManager.RemoveAndDeleteAllFiles();

            //prg.Value = _shrinkPrgSize * 4; //for some reason the progress event isn't firing for replacements
            //prg.Update();
        }

        private void fileCopyProgress(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
        {
            prgInc(1);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_USER + 666)
        //    {
        //        int i = m.WParam.ToInt32();
        //        prgInc(1);
        //    }

        //    base.WndProc(ref m);
        //}

        private void textbox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textbox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
                ((TextBox)sender).Text = s[0];

        }


        private void btnPrepIso_Click(object sender, EventArgs e)
        {


            if (!Directory.Exists(txtWorkingFolder.Text) || !File.Exists(txtIso.Text))
            {
                MessageBox.Show(this, "Input ISO or Working folder not found, drag and drop the ISO and Working Folder on to the text boxes above.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            readDiscTitle();
            findMainPartition();

            if (!rdbGameGh3.Checked && !rdbGameGhA.Checked)
            {
                MessageBox.Show(this, "No Game selected.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //TODO enable message somewhere
            //if (_prepProps.EnableTierEditing == YesNo.Yes && (_prepProps.RemoveBossBattles == YesNo.Yes || _prepProps.TierSongsCount.Length != 0 ||  ))
            //{
            //    if (DialogResult.No == MessageBox.Show(this, "Adding more that 48 songs can stop the save file being restored (Tested on GH3)\r\n\r\nAre you sure you want to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //        return;
            //}
        
            //shortcuts to make code cleaner
            TheGhostCore core = new TheGhostCore();
            _project = core.Project;
            _plugins = core.PluginManager;

            //set up the project settings
            _project.WorkingPath = txtWorkingFolder.Text;
            _project.SourcePath = txtIso.Text;
            _project.GameId = (rdbGameGh3.Checked ? Game.GH3_Wii : Game.GHA_Wii);   //  or  GameInfo.GameStringToEnum("GH 3 (Wii)");

            _project.SetPlugins(_plugins.AudioImportPlugins["FFMpeg Plugin"],
                                _plugins.AudioExportPlugins["IMA ADPCM Plugin"],
                                _plugins.FileCopyPlugins["Wii Scrubber"]);  //no editor plugins

            _project.LanguageId = GameInfo.LanguageStringToId(cboLanguage.Text);

            bool failed = false;
            try
            {
                _project.FileManager.ImportPaks(null);
            }
            catch
            {
                failed = true;
            }
            if (failed || !_project.FileManager.PakFormat.PakFileExists)
            {
                MessageBox.Show(this, "The selected language does not exist in the ISO or file extraction failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            int calcTierSongs = 0;
            int calcBonuSongs = 0;
            bool calcBossRemove = false;
            bool calcNonCareerMove = false;
            int calcSongsToAdd = 0;


            if (_prepProps.EnableTierEditing == YesNo.Yes)
            {

                if (_prepProps.Tiers != 0 && _prepProps.TierSongsTotalCount == 0)
                {
                    MessageBox.Show(this, "TierSongsCount must be set when setting TierSongs.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (_prepProps.TierSongsTotalCount != 0)
                {
                    foreach (int c in _prepProps.TierSongs)
                    {
                        if (c < 2)
                        {
                            MessageBox.Show(this, "Each Tier must contain at least 2 songs to function correctly with an encore.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                }


                bool calcTiersChanged;
                int bossSongCount = 0;
                int nonCareerOnlyCount = 0;
                int bonusSongCount = 0;
                int tierSongsCount = 0;
                bool ttfafExists = false; //does TTFAF exist in bonus, are we not editing the tier songs then inc tier counts, dec bonus counts
                bool kingQueenCredExists = false; //does kingsandqueencredits exist in bonus, are we not editing the tier songs then inc tier counts, dec bonus counts

                QbKey ttfafQk = QbKey.Create("thrufireandflames");
                QbKey kingQueenCred = QbKey.Create("kingsandqueenscredits");
                foreach (ProjectTierSong pts in _project.Songs.Values)
                {
                    switch (pts.Tier.Type)
                    {
                        case TierType.Career:
                            if (pts.SongQb.IsBoss)
                                bossSongCount++;
                            tierSongsCount++;
                            break;
                        case TierType.Bonus:
                            if (pts.SongQb.Id.Crc == ttfafQk.Crc)
                                ttfafExists = true;
                            if (pts.SongQb.Id.Crc == kingQueenCred.Crc)
                                kingQueenCredExists = true;
                            bonusSongCount++;
                            break;
                        case TierType.NonCareer:
                            nonCareerOnlyCount++;   //+1 for kingsandqueens in GHA +6 for non career songs
                            break;
                        case TierType.None:
                            break;
                        default:
                            break;
                    }
                }

                calcTierSongs = tierSongsCount; //will be the resulting value when editing the current tiers
                calcBonuSongs = bonusSongCount;

                calcBossRemove = (_prepProps.RemoveBossBattles == YesNo.Yes || _prepProps.TierSongsTotalCount != 0) && bossSongCount != 0;
                calcNonCareerMove = (_prepProps.AddNonCareerTracksToBonus == YesNo.Yes || _prepProps.TierSongsTotalCount != 0 || calcBossRemove) && nonCareerOnlyCount != 0;

                calcTiersChanged = (_prepProps.TierSongsTotalCount != 0 || calcBossRemove);

                if (calcTiersChanged) //if any change then force boss and non career move and copy career to other tier sets
                {
                    calcBossRemove = true;
                    calcNonCareerMove = true;
                }

                if (calcBossRemove)
                {
                    calcTierSongs -= bossSongCount;
                    if (ttfafExists)
                    {
                        calcTierSongs++;
                        calcBonuSongs--;
                    }
                    //if (!kingQueenCredExists && _project.GameInfo.Game == Game.GHA_Wii)
                    //    calcBonuSongs++;
                }

                if (calcNonCareerMove)
                {
                    calcBonuSongs += nonCareerOnlyCount; //+1 for kingsandqueens in GHA +6 for non career songs
                }

                calcSongsToAdd = 0;
                if (_prepProps.TierSongsTotalCount != 0)
                {
                    calcBonuSongs -= _prepProps.TierSongsTotalCount - calcTierSongs; //add or remove the difference from the bonus songs
                    calcTierSongs += _prepProps.TierSongsTotalCount - calcTierSongs;
                }

                if (calcBonuSongs < 1) //too many career songs. add more songs
                {
                    calcSongsToAdd += 1 - calcBonuSongs;
                    calcBonuSongs = 1;
                }

                if (_prepProps.BonusSongs != 0)
                {
                    calcSongsToAdd -= calcBonuSongs - _prepProps.BonusSongs;
                    calcBonuSongs = _prepProps.BonusSongs;
                }

                int existingCareerTiers = 0;
                foreach (ProjectTier t in _project.Tiers)
                {
                    if (t.Type == TierType.Career)
                        existingCareerTiers++;
                }


                if (DialogResult.OK != MessageBox.Show(this, string.Format(
@"The following tier changes will be made to this ISO:

{0} boss songs will be removed.{1}{2}
{3} songs will be added to the bonus songs currently being Non Career/Bonus.{4}

The career tiers will contain {5} songs{6}.
The bonus tier will contain {7} songs{8}.

The Game will contain {9} career tiers with {10} songs in total ({11} will be {12}).

Remember that setting too many songs can cause the game to crash, read the FAQ for more information.

Click OK to continue...",
                                           (calcBossRemove ? bossSongCount : 0).ToString(),
                                                   (calcTiersChanged) ? " (Tiers Changed)" : "",
                                                   (calcBossRemove && ttfafExists ? "\r\n    Through The Fire and Flames will be moved from the bonus tier to the career tiers.\r\n    This is because it can no longer be unlocked with out the Devil Battle" : ((calcNonCareerMove || calcBossRemove) && !kingQueenCredExists && _project.GameInfo.Game == Game.GHA_Wii ? "\r\n    Kings And Queen (End Credits Version) will be added as the last bonus song." : "")),
                                           (calcNonCareerMove ? nonCareerOnlyCount : 0).ToString(),
                                                   (calcTiersChanged) ? " (Tiers Changed)" : "",
                                           calcTierSongs.ToString(),
                                                   !calcTiersChanged ? " (no change)" : "",
                                           calcBonuSongs.ToString(),
                                                   (!calcTiersChanged && _prepProps.BonusSongs == 0) ? " (no change)" : "",
                                           _prepProps.Tiers != 0 ? _prepProps.Tiers.ToString() : existingCareerTiers.ToString(),
                                                   (calcTierSongs + calcBonuSongs).ToString(),
                                                   Math.Abs(calcSongsToAdd).ToString(),
                                                   (calcSongsToAdd < 0) ? "removed" : "added"
                   ), "ISO Songs", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                    return;

            }

            _shrinkPrgSize = 1;

            lblStatus.Text = "Replacing Audio";
            lblStatus.Update();

            btnPrepIso.Enabled = false;
            pgdPrep.Enabled = false;
            cboPresets.Enabled = false;
            cboLanguage.Enabled = false;
            btnPreset.Enabled = false;
            btnDiscIdSet.Enabled = false;
            btnTitleRead.Enabled = false;
            btnTitleWrite.Enabled = false;

            try
            {

                foreach (ProjectTierSong pts in _project.Songs.Values)
                {
                    pts.IsEditSong = false; //reset any edit flags
                    pts.Song = null;
                }

                if (_prepProps.EnableAudioBlanking != YesNo.Yes)
                {
                    prg.Visible = true;
                    prg.Minimum = 0;
                    _shrinkPrgSize = 1; //make the progress bar display better portions for scrubbing
                    prg.Maximum = 6;  //Extract files, build ISO, edit things, replace partition
                    prg.Value = 0;
                    Application.DoEvents();
                }
                else
                {


                    if (_prepProps.BlankTierSongs == YesNo.Yes || _prepProps.BlankBattles == YesNo.Yes)
                    {
                        foreach (ProjectTier pt in _project.Tiers)
                        {
                            if (pt.Type == TierType.Career)
                            {
                                foreach (ProjectTierSong pts in pt.Songs)
                                {
                                    if (((!pts.SongQb.IsBoss && _prepProps.BlankTierSongs == YesNo.Yes) || (pts.SongQb.IsBoss && _prepProps.BlankBattles == YesNo.Yes)) && !pts.IsAddedWithTheGhost)
                                        setEditSong(pts);
                                }
                            }
                        }
                    }

                    if (_prepProps.BlankBonus == YesNo.Yes || _prepProps.BlankBattles == YesNo.Yes)
                    {
                        foreach (ProjectTier pt in _project.Tiers)
                        {
                            if (pt.Type == TierType.Bonus)
                            {
                                foreach (ProjectTierSong pts in pt.Songs)
                                {
                                    if (((!pts.SongQb.IsBoss && _prepProps.BlankBonus == YesNo.Yes) || (pts.SongQb.IsBoss && _prepProps.BlankBattles == YesNo.Yes)) && !pts.IsAddedWithTheGhost)
                                        setEditSong(pts);
                                }
                            }
                        }
                    }

                    if (_prepProps.BlankAddedSongs == YesNo.Yes || _prepProps.BlankBattles == YesNo.Yes)
                    {
                        foreach (ProjectTier pt in _project.Tiers)
                        {
                            foreach (ProjectTierSong pts in pt.Songs)
                            {
                                if (pts.IsAddedWithTheGhost && ((!pts.SongQb.IsBoss && _prepProps.BlankAddedSongs == YesNo.Yes) || (pts.SongQb.IsBoss && _prepProps.BlankBattles == YesNo.Yes)))
                                    setEditSong(pts);
                            }
                        }
                    }

                    if (_prepProps.BlankNonCareerSongs == YesNo.Yes || _prepProps.BlankBattles == YesNo.Yes)
                    {
                        foreach (ProjectTier pt in _project.Tiers)
                        {
                            if (pt.Type == TierType.NonCareer)
                            {
                                foreach (ProjectTierSong pts in pt.Songs)
                                {
                                    if (((!pts.SongQb.IsBoss && _prepProps.BlankNonCareerSongs == YesNo.Yes) || (pts.SongQb.IsBoss && _prepProps.BlankBattles == YesNo.Yes)) && !pts.IsAddedWithTheGhost)
                                        setEditSong(pts);
                                }
                            }
                        }
                    }

                    prg.Visible = true;
                    prg.Minimum = 0;
                    _shrinkPrgSize = _project.EditSongs.Length; //make the progress bar display better portions for scrubbing
                    prg.Maximum = (_project.EditSongs.Length * 4) + (_shrinkPrgSize * 6); //*2=this loop and next, *4=File copy ImpNotesPAK, ExpNotesPAK, ExpDat, ExpWad +1 ExpQBPAK
                    prg.Value = 0;
                    Application.DoEvents();

                    replaceAudio();
                }

                replaceDataPartition(calcSongsToAdd, calcNonCareerMove, calcBossRemove, calcBonuSongs, _prepProps.TierSongs);


                lblStatus.Text = "Complete";

                MessageBox.Show(this, "Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            finally
            {
                btnPrepIso.Enabled = true;
                pgdPrep.Enabled = true;
                cboPresets.Enabled = true;
                cboLanguage.Enabled = true;
                btnPreset.Enabled = true;
                btnDiscIdSet.Enabled = true;
                btnTitleRead.Enabled = true;
                btnTitleWrite.Enabled = true;
            }


        }

        private void setEditSong(ProjectTierSong pts)
        {
            //add the song to the songs being edited.  TheGHOST does this after leaving the Track Select screen for all songs
            ProjectSong song = _project.CreateProjectSong(pts.SongQb, true);
            pts.Song = song;
            pts.IsEditSong = true;

        }

        private void prepRemoveFiles(DirectoryInfo dir)
        {
            bool remUnused = _prepProps.RemoveUnusedFiles == YesNo.Yes;
            string keepLang = null;
            if (_prepProps.RemoveOtherLanguages == YesNo.Yes)
                keepLang = _project.GameInfo.LanguageId;


            if (keepLang != null)
            {
                if (_project.GameInfo.Game == Game.GH3_Wii && dir.Name.ToLower() == "songs")  //not for GHA
                {
                    foreach (FileInfo f in dir.GetFiles())
                    {
                        //does filename end with .pak.ngc and does not match our mask
                        if (Regex.IsMatch(f.Name, @"^[a-z0-9]+(:?|_[a-z0-9]{2,})(?:|_[a-z])\.pak\.ngc$") && !Regex.IsMatch(f.Name, string.Format(@"^[a-z0-9]+?(:?|_[a-z0-9]{{2,}}){0}\.pak\.ngc$", keepLang), RegexOptions.IgnoreCase))
                            FileHelper.Delete(f.FullName);
                    }
                }
                else if (dir.Name.ToLower() == "pak")
                {
                    foreach (FileInfo f in dir.GetFiles())
                    {
                        //does filename end with .pak.ngc and does not match our mask
                        if (string.Format("pak/{0}", f.Name.ToLower()) != _project.GameInfo.QbPakFilename.ToLower() && Regex.IsMatch(f.Name, @"^qb(?:|_[a-z])\.pak\.ngc$", RegexOptions.IgnoreCase))
                            FileHelper.Delete(f.FullName);
                    }
                }
            }

            if (remUnused)
            {
                foreach (FileInfo f in dir.GetFiles())
                {

                    //if (dir.Name == "songs")
                    //{
                    if (f.Name.EndsWith(".lst.ngc") ||
                        f.Name.EndsWith(".mid.qb.ngc") ||
                        f.Name.EndsWith(".mid_text.qb.ngc") ||
                        f.Name.EndsWith(".txt.ngc") ||
                        f.Name.EndsWith("_song_scripts.qb.ngc") ||
                        Regex.IsMatch(f.Name, @"^.+_[sg]fx(?:|_[a-z])\.pak\.ngc$", RegexOptions.IgnoreCase))
                        FileHelper.Delete(f.FullName);
                    //}
                    //else if (dir.Name == "music")
                    //{
                    if (f.Name.EndsWith("_coop.dat.ngc") ||
                        f.Name.EndsWith("_coop.wad.ngc"))
                        FileHelper.Delete(f.FullName);
                    //}

                    if (f.Name.EndsWith(".zip.ngc"))
                        FileHelper.Delete(f.FullName);
                }
            }

            foreach (DirectoryInfo d in dir.GetDirectories())
                prepRemoveFiles(d);
        }

        private void prepMovies(DirectoryInfo dir)
        {
            string bik = string.Format(@"{0}\TheGHOST.bik", new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName.TrimEnd('\\'));

            foreach (FileInfo f in dir.GetFiles())
            {
                if (rdbGameGh3.Checked || rdbGameGhA.Checked)
                {
                    if ((_prepProps.ReplaceVideos == YesNo.Yes || _prepProps.RemoveIntroVideos == YesNo.Yes) &&
                        (f.Name == "atvi.bik" || f.Name == "Budcat.bik" || f.Name == "ns_logo.bik" || f.Name == "ro_logo.bik" || f.Name == "vvisions.bik"))
                        FileHelper.Delete(f.FullName);
                    else if (_prepProps.ReplaceVideos == YesNo.Yes)
                        File.Copy(bik, f.FullName, true);
                }
            }
        }

        private void replaceUpdatePartition(bool compactOnly)
        {
            if (_partitionNo != _DataPartition)
                return;

            string[] s = new string[] { @"\partition.bin", @"\main.dol", @"\apploader.img", @"\bi2.bin", @"\boot.bin" };
            string[] d = new string[s.Length];

            for (int i = 0; i < s.Length; i++)
                d[i] = string.Format(@"{0}\GameFiles{1}", _project.WorkingPath.TrimEnd('\\'), s[i]);

            ExtractFiles(txtIso.Text, s, d, s.Length, false, 1, IntPtr.Zero);

            DirectoryInfo pt = new DirectoryInfo(string.Format(@"{0}\PartitionUpdate", _project.WorkingPath.TrimEnd('\\')));

            if (pt.Exists)
                pt.Delete(true);

            pt.Create();

            if (compactOnly) //extract the partition
                ExtractPartition(_project.SourcePath, 1, pt.FullName);
            else
            {
                //create the partition

                DirectoryInfo diSys = pt.CreateSubdirectory("_sys");

                File.Create(string.Format(@"{0}\TheGHOST.dummy", diSys.FullName)).Close();

                using (FileStream fs = new FileStream(string.Format(@"{0}\__update.inf", pt.FullName), FileMode.Create))
                {
                    string dt = "2006/09/29";
                    fs.Write(Encoding.ASCII.GetBytes(dt), 0, dt.Length);
                    fs.Write(new byte[368 - dt.Length], 0, 368 - dt.Length); //write update file with no update files
                }
            }

            //REBUILD PARTITION
            FileInfo ptImg = new FileInfo(string.Format(@"{0}.img", pt.FullName));
            WiiIso.BuildPartition(d[0], d[1], d[2], d[3], d[4], pt.FullName, ptImg.FullName, true);

            //remove extracted files (partition.bin etc)
            for (int i = 0; i < s.Length; i++)
                FileHelper.Delete(d[i]);

            //REPLACE PARTITION
            LoadPartition(_project.SourcePath, ptImg.FullName, 1);
            FileHelper.Delete(ptImg.FullName);

        }

        private void replaceDataPartition(int songsToAdd, bool addNonCareerToBonus, bool removeBossBattles, int bonusSongs, int[] tierSongs)
        {
//#if (!DEBUG)
            ///////////////////////////////////////////////////////////
            //  SHRINK UPDATE PARTITION
            ///////////////////////////////////////////////////////////
                lblStatus.Text = "Shrinking Update Partition...";
            lblStatus.Update();

            if (_prepProps.RemoveUpdatePartition != YesNo.Yes)
                replaceUpdatePartition(false);

            prgInc(_shrinkPrgSize);

            ///////////////////////////////////////////////////////////
            //  EXTRACT PARTITION
            ///////////////////////////////////////////////////////////
            lblStatus.Text = "Extracting Data Partition...";
            lblStatus.Update();
//#endif
            DirectoryInfo pt = new DirectoryInfo(string.Format(@"{0}\PartitionData", _project.WorkingPath.TrimEnd('\\')));

//#if (!DEBUG)
            int i = 5; //try 5 times
            bool er = true;
            if (pt.Exists)
            {
                do
                {
                    try
                    {
                        pt.Delete(true);
                        er = false;
                    }
                    catch
                    {
                        er = true;
                        System.Threading.Thread.Sleep(100);
                        if (--i == 0)
                            throw;
                    }
                } while (er == true);

            }
            pt.Create();

            ExtractPartition(_project.SourcePath, _partitionNo, pt.FullName);
            prgInc(_shrinkPrgSize);
//#endif
            ///////////////////////////////////////////////////////////
            //  EDIT FILES
            ///////////////////////////////////////////////////////////
            lblStatus.Text = "Editing Files...";
            lblStatus.Update();

            prepMovies(new DirectoryInfo(string.Format(@"{0}\movies", pt.FullName)));

            prepRemoveFiles(pt);


            if (_prepProps.EnableTierEditing == YesNo.Yes || songsToAdd != 0 || _prepProps.AddNonCareerTracksToBonus == YesNo.Yes)
                _project.GameMods.AddSongs(pt, songsToAdd, rdbGameGh3.Checked ? "lagrange" : "dreampolice", addNonCareerToBonus, new DirectoryInfo(_project.WorkingPath));

            PakFormat pf = new PakFormat(string.Format(@"{0}\pak\{1}", pt.FullName, _project.FileManager.PakFormat.PakFilename), string.Empty, string.Empty, PakFormatType.Wii, false);
            PakEditor pe = new PakEditor(pf, false);

            if (_prepProps.EnableTierEditing == YesNo.Yes && (removeBossBattles || tierSongs != null))
                _project.GameMods.EditTiers(pe, removeBossBattles, _prepProps.Tiers, bonusSongs, tierSongs, _prepProps.UnlockSetlistTiers == YesNo.Yes);

            if (_prepProps.SetCheats == YesNo.Yes)
                _project.GameMods.SetCheats(pe);

            if (_prepProps.RemoveIntroVideos == YesNo.Yes)
                _project.GameMods.RemoveIntroVids(pe);

            if (_prepProps.FreeStore == YesNo.Yes)
                _project.GameMods.FreeStore(pe);

            if (_prepProps.DefaultBonusSongArt == YesNo.Yes)
                _project.GameMods.ResetBonusArt(pe);

            if (_prepProps.DefaultBonusSongInfo == YesNo.Yes)
                _project.GameMods.ResetBonusInfoText(pe, _prepProps.DefaultBonusSongInfoText);

            //perform this step after edit tiers as edit tiers set the unlock values
            if (_prepProps.UnlockSetlistTiers == YesNo.Yes || _prepProps.CompleteTier1Song == YesNo.Yes)
                _project.GameMods.UnlockSetlists(pe, _prepProps.UnlockSetlistTiers == YesNo.Yes, _prepProps.CompleteTier1Song == YesNo.Yes);


            prgInc(_shrinkPrgSize);

            if (_prepProps.ManualEditing == YesNo.Yes)
            {
                lblStatus.Text = "Manual Editing...";
                lblStatus.Update();

                if (MessageBox.Show(this, string.Format("Edit the partition files here: {0}{1}{1}You can use TheGHOST with the File System plugin at this point to replace songs, or make manual adjustments.{1}{1}Press OK to build the partition and insert it into the ISO.", pt.FullName, Environment.NewLine), "Continue", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                    return;
            }

            Application.DoEvents();
            int songNo = 0;

            DirectoryInfo ghostDir = new DirectoryInfo(string.Format(@"{0}\-TheGHOST-", pt.FullName));
            string fileName = string.Format(@"{0}\info.txt", ghostDir.FullName);

            if (ghostDir.Exists)
            {
                if (File.Exists(fileName))
                {
                    string[] txt = File.ReadAllLines(fileName);
                    if (txt.Length > 1)
                    {
                        if (txt[1].StartsWith("TheGHOSTSongs="))
                            songNo = int.Parse(txt[1].Split('=')[1]);
                    }
                }
            }
            else
                ghostDir.Create();

            //don't save the count of songs added with TheGHOST anymore
            //if (_prepProps.AddSongsCount > songNo)
            //    songNo = _prepProps.AddSongsCount;

            string info = string.Format("{1} - v{2} / v{3}{0}TheGHOSTSongs={4}{0}", Environment.NewLine, TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion, songNo.ToString());
            info = string.Concat(info, new string(' ', 1024 - info.Length));
            File.WriteAllText(fileName, info);

            ///////////////////////////////////////////////////////////
            //  MOVING PARTITION
            //////////////////////////////////////////////////////////
//#if (!DEBUG)
            lblStatus.Text = "Moving Partition...";
            lblStatus.Update();

            ShufflePartitions(_project.SourcePath);
            prgInc(_shrinkPrgSize);
//#endif
            ///////////////////////////////////////////////////////////
            //  BUILD PARTITION
            ///////////////////////////////////////////////////////////
            lblStatus.Text = "Building Partition...";
            lblStatus.Update();

            GameFile[] files = _project.FileManager.Import(GameFileType.Other, null,
                @"\partition.bin", @"\main.dol", @"\apploader.img", @"\bi2.bin", @"\boot.bin");
            
            FileInfo ptImg = new FileInfo(string.Format(@"{0}.img", pt.FullName));
            WiiIso.BuildPartition(files[0].LocalName, files[1].LocalName, files[2].LocalName, files[3].LocalName, files[4].LocalName,
                pt.FullName, ptImg.FullName, true);

            _project.FileManager.RemoveAndDeleteAllFiles();  //remove extracted files (partition.bin etc)

            prgInc(_shrinkPrgSize);

            ///////////////////////////////////////////////////////////
            //  REPLACE PARTITION
            //////////////////////////////////////////////////////////
            lblStatus.Text = "Replacing Partition...";
            lblStatus.Update();

            LoadPartition(_project.SourcePath, ptImg.FullName, _partitionNo);
            FileHelper.Delete(ptImg.FullName);
            prgInc(_shrinkPrgSize);

            ///////////////////////////////////////////////////////////
            //  COMPLETE
            ///////////////////////////////////////////////////////////

            MaxPartition(_project.SourcePath, _partitionNo);

            //ISO IS LOCKED BY WIISCRUBBER
            //if (chkRenameIso.Checked)
            //{
            //    FileInfo iso = new FileInfo(_project.SourcePath);
            //    File.Move(iso.FullName, string.Format("{0}_prepped{1}", iso.Name.Substring(0, iso.Name.Length - iso.Extension.Length), iso.Extension)); 
            //}

            lblStatus.Text = "Complete, Backup this ISO and use a copy of it when replacing songs.";
            prg.Value = prg.Maximum;

        }

        private void prgInc(int value)
        {
            if (prg.Value + value < prg.Maximum)
                prg.Value += value;
            else
                prg.Value = prg.Maximum;
            prg.Update();
        }


        private void btnHelp_Click(object sender, EventArgs e)
        {
            string path;
            string fileMask = @"{0}\TheGHOST-Guide.pdf";
            FileInfo f = new FileInfo(Assembly.GetEntryAssembly().Location);

            path = string.Format(fileMask, f.Directory.FullName);
            if (!File.Exists(path) && f.Directory.Parent != null)
                path = string.Format(fileMask, f.Directory.Parent.FullName);

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(path);
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No application is associated with the specified file for this operation")
                    MessageBox.Show(this, "No application is associated with PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (ex.Message == "The system cannot find the file specified")
                    MessageBox.Show(this, string.Format(@"File not found '{0}\TheGHOST-Guide.pdf'.", path), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "An error ocurred while attempting to display the PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnDiscIdGet_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtWorkingFolder.Text) || !File.Exists(txtIso.Text))
            {
                MessageBox.Show(this, "Input ISO or Working folder not found, drag and drop the ISO and Working Folder on to the text boxes above.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            readDiscTitle();
            findMainPartition();

            string bootBin = string.Format(@"{0}\boot.bin", txtWorkingFolder.Text);
            FileHelper.Delete(bootBin);


            ExtractFiles(txtIso.Text,
                new string[] { @"\boot.bin" },
                new string[] { bootBin },
                1, false, _partitionNo, IntPtr.Zero);

            if (File.Exists(bootBin))
            {
                using (FileStream fs = File.OpenRead(bootBin))
                {
                    byte[] b = new byte[4];
                    fs.Read(b, 0, 4);
                    txtDiscId.Text = Encoding.Default.GetString(b);
                }

                FileHelper.Delete(bootBin);
            }
        }

        private void btnDiscIdSet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(txtIso.Text))
                {
                    MessageBox.Show(this, "Input ISO not found, drag and drop the ISO on to the text box above.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                readDiscTitle();
                findMainPartition();

                if (txtDiscId.Text.Length == 4)
                {
                    StringBuilder sb = new StringBuilder(16);
                    sb.Append("00010000");
                    foreach (char c in txtDiscId.Text.ToCharArray())
                        sb.Append(((byte)c).ToString("X").PadLeft(2, '0'));

                    ChangePartTitle(txtIso.Text, _partitionNo, sb.ToString());

                    //stops the WiFi working...
                    if (chkIsoId.Checked)
                        DirectDiscWrite(txtIso.Text, 0, txtDiscId.Text, txtDiscId.Text.Length);

                    MessageBox.Show(this, string.Format("Disc ID set to '{0}'", txtDiscId.Text), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show(this, "The Disc ID is not 4 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch
            {
                MessageBox.Show(this, "Unable to open ISO or WiiScrubber error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - v{1} / v{2}", TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion);
            rdbGameGh3_CheckedChanged(this, e);
        }

        private void btnTitleWrite_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtWorkingFolder.Text) || !File.Exists(txtIso.Text))
            {
                MessageBox.Show(this, "Input ISO or Working folder not found, drag and drop the ISO and Working Folder on to the text boxes above.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show(this, "Are you sure the game is set correctly, if not then corruption will occur!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                if (txtTitleSaveText1.Text.Length > (_mainDolT1OffLen / 2))
                {
                    MessageBox.Show(this, string.Format("The length of Save File Text line 1 ({0}) is greater than {1} letters.", txtTitleSaveText1.Text.Length.ToString(), (_mainDolT1OffLen / 2)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (txtTitleSaveText2.Text.Length > (_mainDolT2OffLen / 2))
                {
                    MessageBox.Show(this, string.Format("The length of Save File Text line 2 ({0}) is greater than {1} letters.", txtTitleSaveText2.Text.Length.ToString(), (_mainDolT2OffLen / 2)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                findMainPartition();

                DirectDiscWrite(txtIso.Text, 0x20, txtTitleDiscTitle.Text.PadRight(txtTitleDiscTitle.MaxLength, '\0'), txtTitleDiscTitle.MaxLength);
                _discTitle = txtTitleDiscTitle.Text; //set new disc id

                string mainDol = string.Format(@"{0}\main.dol", txtWorkingFolder.Text);
                string openingBnr = string.Format(@"{0}\opening.bnr", txtWorkingFolder.Text);
                ExtractFiles(txtIso.Text,
                    new string[] { @"opening.bnr", @"\main.dol" },
                    new string[] { openingBnr, mainDol },
                    2, false, _partitionNo, IntPtr.Zero);

                int maxLen = 40;
                if (txtTitleChannelText.Text.Length > maxLen)
                {
                    MessageBox.Show(this, string.Format("The length of the Channel text ({0}) is greater than {1} letters.", txtTitleChannelText.Text.Length.ToString(), maxLen), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                byte[] b;

                using (FileStream fs = new FileStream(openingBnr, FileMode.Open, FileAccess.ReadWrite))
                {
                    writeUnicode(fs, 0x5c, txtTitleChannelText.Text, 84);
                    writeUnicode(fs, 0xb0, txtTitleChannelText.Text, 84);
                    writeUnicode(fs, 0x104, txtTitleChannelText.Text, 84);
                    writeUnicode(fs, 0x158, txtTitleChannelText.Text, 84);
                    writeUnicode(fs, 0x1ac, txtTitleChannelText.Text, 84);
                    writeUnicode(fs, 0x200, txtTitleChannelText.Text, 84);
                    writeUnicode(fs, 0x254, txtTitleChannelText.Text, 84);

                    System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    int start = 0x00;

                    fs.Seek(start, SeekOrigin.Begin);
                    b = new byte[0x600 - start];
                    fs.Read(b, 0, (0x600 - 0x10) - start);

                    byte[] hash = x.ComputeHash(b); //hash the header (including the area the hash occupies, which must be blank)

                    //write hash back
                    fs.Seek(0x5f0, SeekOrigin.Begin);
                    fs.Write(hash, 0, hash.Length);                
                }


                using (FileStream fs = File.Open(mainDol, FileMode.Open, FileAccess.ReadWrite))
                {

                    if (setSaveTitleOffsets(fs))
                    {
                        writeUnicode(fs, (long)_mainDolT1Off, txtTitleSaveText1.Text, (int)_mainDolT1OffLen);
                        b = new byte[2];
                        fs.Write(b, 0, b.Length);

                        writeUnicode(fs, (long)_mainDolT2Off, txtTitleSaveText2.Text, (int)_mainDolT2OffLen);
                        b = new byte[2];
                        fs.Write(b, 0, b.Length);
                    }
                }


                ReplaceFiles(txtIso.Text,
                    new string[] { @"opening.bnr", @"\main.dol" },
                    new string[] { openingBnr, mainDol },
                    2, false, false, _partitionNo, IntPtr.Zero);

                FileHelper.Delete(openingBnr);
                FileHelper.Delete(mainDol);

                MessageBox.Show(this, "Titles Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        private void btnTitleRead_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtWorkingFolder.Text) || !File.Exists(txtIso.Text))
            {
                MessageBox.Show(this, "Input ISO or Working folder not found, drag and drop the ISO and Working Folder on to the text boxes above.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show(this, "Are you sure the game is set correctly, if not then corruption will occur!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                readDiscTitle();
                findMainPartition();

                txtTitleDiscTitle.Text = _discTitle;

                string mainDol = string.Format(@"{0}\main.dol", txtWorkingFolder.Text);
                string openingBnr = string.Format(@"{0}\opening.bnr", txtWorkingFolder.Text);

                ExtractFiles(txtIso.Text,
                    new string[] { @"opening.bnr", @"\main.dol" },
                    new string[] { openingBnr, mainDol },
                    2, false, _partitionNo, IntPtr.Zero);

                using (FileStream fs = File.OpenRead(openingBnr))
                {
                    txtTitleChannelText.Text = readUnicode(fs, 0xb0);
                }

                using (FileStream fs = File.OpenRead(mainDol))
                {
                    if (setSaveTitleOffsets(fs))
                    {
                        txtTitleSaveText1.Text = readUnicode(fs, _mainDolT1Off);
                        fs.Seek(_mainDolT2Off, SeekOrigin.Begin);
                        txtTitleSaveText2.Text = readUnicode(fs, fs.Position);
                    }
                }

                FileHelper.Delete(openingBnr);
                FileHelper.Delete(mainDol);

            }
        }

        private bool setSaveTitleOffsets(Stream fs)
        {
            long start = 0x3a0000;
            fs.Seek(start, SeekOrigin.Begin);
            byte[] data = new byte[0x60000];
            fs.Read(data, 0, data.Length);

            byte[] f = Encoding.ASCII.GetBytes("banner.tpl\0rb\0");

            bool found = false;
            int i = 0;
            //find first string offset
            while (i < data.Length - f.Length)
            {
                found = true;
                for (int j = 0; j < f.Length; j++)
                {
                    if (data[i] != f[j])
                    {
                        found = false;
                        i -= j;
                        break;
                    }
                    i++;
                }
                if (found)
                    break;

                i++;
            }

            if (found)
            {
                _mainDolT1Off = (uint)start + (uint)i;
                _mainDolT2Off = _mainDolT1Off + _mainDolT1OffLen + 2;

                if (data[_mainDolT2Off - start - 1] == 0 && data[_mainDolT2Off - start - 2] == 0)  //check for null terminator after text 1
                {
                    start = (_mainDolT2Off - start) + _mainDolT2OffLen + 2;

                    if (data[start - 1] == 0 && data[start - 2] == 0)  //check for null terminator after text 2
                    {

                        f = Encoding.ASCII.GetBytes("%s %d\0*\0");
                        for (i = 0; i < f.Length; i++)
                        {
                            if (data[start + i] != f[i])
                            {
                                found = false;
                                break;
                            }
                        }
                    }
                    else
                        found = false;
                }
                else
                    found = false;
            }

            if (!found)
                MessageBox.Show(this, "The Save Title text could not be located, PM the author on ScoreHero to get it resolved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return found;
        }

        //private uint getMainDolIdx(uint unknownVal)
        //{
        //    uint mainDolIdx = unknownVal;
        //    if (rdbGameGh3.Checked && cboTitleRegion.SelectedIndex == 0) //GH3 PAL
        //        mainDolIdx = 0;
        //    else if (rdbGameGhA.Checked && cboTitleRegion.SelectedIndex == 0) //GHA PAL
        //        mainDolIdx = 1;
        //    else if (rdbGameGh3.Checked && cboTitleRegion.SelectedIndex == 1) //GH3 NTSC
        //        mainDolIdx = 2;
        //    else if (rdbGameGhA.Checked && cboTitleRegion.SelectedIndex == 1) //GHA NTSC
        //        mainDolIdx = 3;

        //    return mainDolIdx;
        //}

        private string readUnicode(FileStream fs, long pos)
        {
            MemoryStream ms = new MemoryStream(80);
            byte b;
            fs.Seek(pos, SeekOrigin.Begin);

            b = 255;

            while (b != 0)
            {
                b = (byte)fs.ReadByte();
                ms.WriteByte(b);
                b = (byte)fs.ReadByte();
                ms.WriteByte(b);
            }

            byte[] bs = ms.ToArray();
            flipUnicode(bs);
            return Encoding.Unicode.GetString(bs, 0, bs.Length);
        }

        private void writeUnicode(FileStream fs, long pos, string text, int pad)
        {
            MemoryStream ms = new MemoryStream(80);
            byte[] b = Encoding.Unicode.GetBytes(text);
            flipUnicode(b);
            fs.Seek(pos, SeekOrigin.Begin);

            fs.Write(b, 0, b.Length);
            if (b.Length < pad)
                fs.Write(new byte[pad - b.Length], 0, pad - b.Length);

        }

        private void flipUnicode(byte[] b)
        {
            byte x;
            for (int i = 0; i < b.Length; i += 2)
            {
                x = b[i];
                b[i] = b[i + 1];
                b[i + 1] = x;
            }
                
        }

        private void rdbGameGh3_CheckedChanged(object sender, EventArgs e)
        {
            _mainDolT1OffLen = 30;
            _mainDolT2OffLen = 30;
            setTitleInterface("GH3");
        }

        private void rdbGameGhA_CheckedChanged(object sender, EventArgs e)
        {
            _mainDolT1OffLen = 40;
            _mainDolT2OffLen = 0;
            setTitleInterface("GHA");
        }

        private void setTitleInterface(string title)
        {
            txtTitleSaveText1.MaxLength = (int)_mainDolT1OffLen / 2;
            txtTitleSaveText2.MaxLength = (int)_mainDolT2OffLen / 2;
            if (txtTitleSaveText1.Text.Length > txtTitleSaveText1.MaxLength)
                txtTitleSaveText1.Text = txtTitleSaveText1.Text.Substring(0, txtTitleSaveText1.MaxLength);
            if (txtTitleSaveText2.Text.Length > txtTitleSaveText2.MaxLength)
                txtTitleSaveText2.Text = txtTitleSaveText2.Text.Substring(0, txtTitleSaveText2.MaxLength);
            txtTitleSaveText2.Enabled = (_mainDolT2OffLen != 0);

            lblTitleSaveLengths.Text = string.Format("Line 1={0}, Line 2={1} - Max Letter Count for {2}", (_mainDolT1OffLen / 2).ToString(), (_mainDolT2OffLen / 2).ToString(), title);
        }


        private void findMainPartition()
        {
            if (!File.Exists(txtIso.Text))
                return;

            if (!_partitionProven)
            {
                int p = _partitionNo;

                while (p != 0 && !_partitionProven)
                {
                    if (FileExists(txtIso.Text, "opening.bnr", p, true))
                    {
                        _partitionProven = true;
                        _partitionNo = p;
                        break;
                    }
                    p--;
                }

            }
        }

        private const int _DataPartition = 2;
        private int _partitionNo;
        private bool _partitionProven;

        private uint _mainDolT1Off;
        private uint _mainDolT2Off;
        private uint _mainDolT1OffLen;
        private uint _mainDolT2OffLen;

        private int _shrinkPrgSize;
        private Project _project;
        private PluginManager _plugins;

        private PrepProperties _prepProps;

        private string _discTitle;
        private string _lastIsoRead;


    }
}
