using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Nanook.QueenBee.Parser;
using System.Diagnostics;

namespace Nanook.TheGhost
{
    public partial class ProgressScreen : ScreenBase
    {
        private delegate void newThread();

        public ProgressScreen()
        {
            _plugins = null;
            InitializeComponent();
            _autoStartOnOpen = false;
        }

        #region ScreenBase Members

        public override void Construct(Project project)
        {
            _project = project;
        }

        public override void Open()
        {
            txtTempPath.Text = _project.GetWorkingPath(WorkingFileType.Root);
            prg.Minimum = 0;
            prg.Maximum = (_project.ChangedSongs.Length * 6) + 1; //*5=1(extracting song paks), 3=(replacing song pak, dat, wad), 1=(Compress Audio), 1=Finalising settings + 1 for replace qbPAK
            prg.Value = 0;

            if (_autoStartOnOpen)
                btnStart_Click(this, new EventArgs());
        }

        public override bool Close(bool appQuiting)
        {
            if (!appQuiting && !btnStart.Enabled)
            {
                MessageBox.Show(this, "Once 'Start' has been pressed you can not go back.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
                return true;

        }

        public override string TitleMessage()
        {
            return "Your chance to edit the audio before it gets compressed.\r\nShows the progress of the compression and update.";
        }

        #endregion

        public PluginManager PluginManager
        {
            get { return _plugins; }
            set { _plugins = value; }
        }

        public bool AutoStartOnOpen
        {
            get { return _autoStartOnOpen; }
            set { _autoStartOnOpen = value; }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            newThread nt = new newThread(finishAll);
            nt.BeginInvoke(null, null);
        }

        private void finishAll()
        {
            Application.DoEvents();

            logProgress("Applying Game Modifications...");
            Application.DoEvents();

            if (_project.GameModsSettings.LastChanged > _project.GameModsSettings.LastApplied || _project.Defaults.ReapplyAll)
            {
                if (_project.GameModsSettings.AddNonCareerTracksToBonus)
                    _project.GameMods.AddBonusSongsFromNonCareer(_project.FileManager.StoreDataQbFile, _project.FileManager.SongListQbFile, _project.FileManager.GuitarProgressionQbFile);

                if (_project.GameModsSettings.UnlockSetlists || _project.GameModsSettings.CompleteTier1Song)
                    _project.GameMods.UnlockSetlists(_project.FileManager.GuitarProgressionQbFile, _project.FileManager.GuitarCoOpQbFile, _project.GameModsSettings.UnlockSetlists, _project.GameModsSettings.CompleteTier1Song);

                if (_project.GameModsSettings.DefaultBonusSongArt)
                    _project.GameMods.ResetBonusArt(_project.FileManager.StoreDataQbFile);

                if (_project.GameModsSettings.FreeStore)
                    _project.GameMods.FreeStore(_project.FileManager.StoreDataQbFile);

                if (_project.GameModsSettings.SetCheats)
                    _project.GameMods.SetCheats(_project.FileManager.QbPakEditor);

                if (_project.GameModsSettings.DefaultBonusSongInfo)
                    _project.GameMods.ResetBonusInfoText(_project.FileManager.QbPakEditor, _project.GameModsSettings.DefaultBonusSongInfoText);


                _project.GameMods.CopyProjectNamesToOtherTiers();

                _project.GameModsSettings.LastApplied = DateTime.Now;
            }

            //replace the background audio and tidy up
            DatWad dw = _project.FileManager.BackgroundAudioDatWad;
            foreach (ProjectBackgroundAudio b in _project.BackgroundAudio)
            {
                if ((b.LastChanged > b.LastApplied || _project.Defaults.ReapplyAll) && b.ReplaceEnabled)
                {
                    if (b.HasMissingRawAudio())
                        b.Import();

                    b.CreatePreview(true);
                    b.Export(_project.Defaults.ForceMono, _project.Defaults.ForceDownSample);
                    b.ReplaceInWad();
                    b.LastApplied = DateTime.Now;
                }
                b.RemoveFiles();
            }


            logProgress("Importing Notes PAKs...");
            Application.DoEvents();


            _project.FileManager.ImportNotesPaks(delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    logProgress(string.Format("    Extracting {0} ({1} of {2}){3}", gameFilename, (fileNo + 1).ToString(), fileCount.ToString(), success ? string.Empty : " FAILED"));
                    incProgress();
                });
            Application.DoEvents();

            logProgress("Finalising Settings and Creating Audio...");
            Application.DoEvents();

            //some items do not get updated, bump progress along to correct position
            setProgress(_project.ChangedSongs.Length, true);

            int idx = 1;
            ProjectSong[] changedSongs = (ProjectSong[])_project.ChangedSongs.Clone(); //changed items get removed when applied
            foreach (ProjectSong ps in changedSongs)
            {
                logProgress(string.Format("    Finalising Settings and Adjusting Audio for {0} - {1} [{2}] ({3} of {4})", ps.Artist, ps.Title, ps.SongQb.Id.Text, idx.ToString(), changedSongs.Length.ToString()));

                if (ps.Audio.LastChanged > ps.LastApplied || _project.Defaults.ReapplyAll)
                    ps.Audio.ImportMissingAudioFiles();

                ps.FinaliseSettings(); //handles the last changed/applied stuff
                incProgress();

                logProgress(string.Format("    Creating Guitar Hero Audio for {0} - {1} [{2}] ({3} of {4})", ps.Artist, ps.Title, ps.SongQb.Id.Text, idx.ToString(), changedSongs.Length.ToString()));
                
                if (ps.Notes.UpdateAffectsAudio || ps.Audio.LastChanged > ps.LastApplied || _project.Defaults.ReapplyAll)
                    ps.Audio.CreateGameAudio(_project.Defaults.ForceMono, _project.Defaults.ForceDownSample, true);

                incProgress();
                Application.DoEvents();

                idx++;
            }

            //some items do not get updated, bump progress along to correct position
            setProgress(_project.ChangedSongs.Length * 3, true);

            _project.FileManager.UpdatePaks();

            int lastStage = 0;
            string[] stages = new string[] { "Replacing Guitar Hero Game Files...", "Replacing Song Audio...", "Replacing Song Headers...", "Skipped Headers..." };

            GameFile[] failed = _project.FileManager.ExportAllChangedSmart(delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (fileNo == 0) //first file
                        logProgress(stages[lastStage++]);

                    logProgress(string.Format("    Replacing {0} ({1} of {2}){3}", gameFilename, (fileNo + 1).ToString(), fileCount.ToString(), success ? string.Empty : " FAILED"));

                    incProgress();
                });

            bool critical = false;
            List<QbKey> failedQk = new List<QbKey>();

            string wadExt = string.Format(".wad.{0}", _project.GameInfo.FileExtension);
            foreach (GameFile gf in failed)
            {
                switch (gf.Type)
	            {
		            case GameFileType.QbPak:
                    case GameFileType.QbPab:
                    case GameFileType.NotesPak:
                        critical = true;
                        break;

                    case GameFileType.Wad:
                        string tmp = gf.LocalName.Substring(0, gf.LocalName.Length - wadExt.Length);
                        tmp = tmp.Substring(_project.GetWorkingPath(WorkingFileType.GhFiles).TrimEnd('\\', '/').Length + 1);
                        failedQk.Add(QbKey.Create(tmp));  //insert in correct order
                    break;
                }
            }

            //update the changed songs
            foreach (ProjectSong ps in changedSongs)
            {
                ps.LastApplied = DateTime.Now; //this will prevent the track appearing in the changed songs list
            }

            if (critical)
            {
                Application.DoEvents();
                logProgress("Failed Replacing Critical Files.");
                MessageBox.Show(this, "Failed replacing critical files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (failedQk.Count != 0)
            {
                List<ProjectTierSong> failedSongs = new List<ProjectTierSong>(failedQk.Count);
                foreach (QbKey qk in failedQk)
                    failedSongs.Add(_project.Songs[qk.Crc]);

                Application.DoEvents();
                logProgress("Failed Songs...");
                foreach (ProjectTierSong fs in failedSongs)
                {
                    Application.DoEvents();
                    logProgress(string.Format("    {0} - {1} [{2}]{3}", fs.Song.Artist, fs.Song.Title, fs.SongQb.Id.Text, fs.IsBonusSong ? " (Bonus)" : ""));
                }

                if (MessageBox.Show(
@"Would you like to remove the failed songs from the Guitar Hero menu system (complete the ISO)?

WARNING: Only Bonus songs can be removed at this stage. Career songs will remain corrupt until replaced.

Yes, any failed songs that were added with TheGHOST can be readded with TheGHOSTWiiIsoTool.
       any failed songs not added with TheGHOST can not be brought back.
No, you can try to create more space with the TheGHOSTWiiIsoTool and try to replace the tracks again.", "Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (ProjectTierSong fs in failedSongs)
                    {
                        if (fs.IsBonusSong)
                            _project.GameMods.BonusSongRemoveFromGame(_project.FileManager.StoreDataQbFile, _project.FileManager.SongListQbFile, fs.SongQb.Id);
                    }
                    _project.FileManager.UpdatePaks();
                    _project.FileManager.RemoveAndDeleteFiles(GameFileType.Dat);
                    _project.FileManager.RemoveAndDeleteFiles(GameFileType.Wad);
                    _project.FileManager.ExportAllChanged(null);
                }

            }

            //remove the background audio raw files
            foreach (ProjectBackgroundAudio b in _project.BackgroundAudio)
            {
                if (b.ReplaceEnabled)
                    b.RemoveFiles();
            }

            //show 100%
            setProgress(int.MaxValue, false); //100%
            Application.DoEvents();

            logProgress("Complete");
            MessageBox.Show("Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.DoEvents();

            _project.FileManager.RemoveAndDeleteAllFiles();
        }

        private void incProgress()
        {
            if (prg.InvokeRequired)
            {
                prg.Invoke(new MethodInvoker(delegate()
                {
                    incProgress();
                }));
                return;
            }

            if (prg.Value < prg.Maximum)
                prg.Value++;
        }

        private void setProgress(int value, bool onlyIfLessThan)
        {
            if (prg.InvokeRequired)
            {
                prg.Invoke(new MethodInvoker(delegate()
                {
                    setProgress(value, onlyIfLessThan);
                }));
                return;
            }

            if (!onlyIfLessThan || prg.Value < value)
            {
                if (value < prg.Maximum)
                    prg.Value = value;
                else
                    prg.Value = prg.Maximum;
            }
        }

        private void logProgress(string message)
        {
            if (txtProgress.InvokeRequired)
            {
                txtProgress.Invoke(new MethodInvoker(delegate()
                {
                    logProgress(message);
                }));
                return;
            }

            int l = txtProgress.Text.Length;
            txtProgress.Text = string.Format("{3}{0}:  {1}{2}", DateTime.Now, message, Environment.NewLine, txtProgress.Text);
            txtProgress.Select(l, 0);
            txtProgress.ScrollToCaret();

            //Application.DoEvents();
        }

        private PluginManager _plugins;
        private Project _project;
        private bool _autoStartOnOpen;
    }
}
