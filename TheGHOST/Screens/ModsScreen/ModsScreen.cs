using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public partial class ModsScreen : ScreenBase
    {
        public ModsScreen()
        {
            InitializeComponent();
            _project = null;
            _lastAudioLen = 0;
            _settingVolume = false;
            tmrPreview.Enabled = false;
            tmrPreview.Interval = 500;
            _startVolumes = null;
        }

        #region ScreenBase Members

        public override string TitleMessage()
        {
            return "Modify other areas of the game";
        }

        public override void Construct(Project project)
        {
            _project = project;
        }

        public override void Open()
        {
            //foreach (ProjectTier tier in _project.Tiers)
            //{
            //    if (tier.Type != TierType.Career)
            //        break;
            //    lstTierNames.Items.Add(tier.Name);
            //}

            string[] tn = _project.GameModsSettings.TierNames;
            for (int i = 0; i < tn.Length; i++)
            {
                lstTierNames.Items.Add(tn[i]);
            }

            chkAddNonCareerTracksToBonus.Checked = _project.GameModsSettings.AddNonCareerTracksToBonus;
            chkCompleteTier1Song.Checked = _project.GameModsSettings.CompleteTier1Song;
            chkDefaultBonusSongArt.Checked = _project.GameModsSettings.DefaultBonusSongArt;
            chkFreeStore.Checked = _project.GameModsSettings.FreeStore;
            chkSetCheats.Checked = _project.GameModsSettings.SetCheats;
            chkUnlockSetlists.Checked = _project.GameModsSettings.UnlockSetlists;
            chkDefaultBonusSongInfoText.Checked = _project.GameModsSettings.DefaultBonusSongInfo;
            txtDefaultBonusSongInfoText.Text = _project.GameModsSettings.DefaultBonusSongInfoText;

            foreach (ProjectBackgroundAudio ba in _project.BackgroundAudio)
            {
                ListViewItem li = new ListViewItem(ba.Name);
                li.Tag = ba;
                lvwAudio.Items.Add(li);
            }

            if (lvwAudio.Items.Count != 0)
                lvwAudio.Items[0].Selected = true;
        }

        public override bool Close(bool appQuiting)
        {
            stopPreview();

            //test that the background audio exists
            StringBuilder sb = new StringBuilder();
            foreach (ProjectBackgroundAudio ba in _project.BackgroundAudio)
            {
                foreach (AudioFile af in ba.AudioFiles)
                {
                    if (!File.Exists(af.Name))
                    {
                        if (sb.Length != 0)
                            sb.AppendLine();
                        sb.Append(af.Name);
                    }
                }
            }

            if (sb.Length != 0)
            {
                MessageBox.Show(this, string.Format("The following background audio files are missing:{0}{0}{1}{0}{0}Please correct them to continue.", Environment.NewLine, sb.ToString()), "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            for (int i = 0; i < _project.Tiers.Count; i++)
            {
                if (_project.Tiers[i].Type != TierType.Career)
                    break;
                if (i >= lstTierNames.Items.Count)
                    break;
                _project.GameModsSettings.SetTierName(i, (string)lstTierNames.Items[i]);
                _project.Tiers[i].Name = (string)lstTierNames.Items[i];
            }

            _project.GameModsSettings.AddNonCareerTracksToBonus = chkAddNonCareerTracksToBonus.Checked;
            _project.GameModsSettings.CompleteTier1Song = chkCompleteTier1Song.Checked;
            _project.GameModsSettings.DefaultBonusSongArt = chkDefaultBonusSongArt.Checked;
            _project.GameModsSettings.FreeStore = chkFreeStore.Checked;
            _project.GameModsSettings.SetCheats = chkSetCheats.Checked;
            _project.GameModsSettings.UnlockSetlists = chkUnlockSetlists.Checked;
            _project.GameModsSettings.DefaultBonusSongInfo = chkDefaultBonusSongInfoText.Checked;
            _project.GameModsSettings.DefaultBonusSongInfoText = txtDefaultBonusSongInfoText.Text;

            return true;
        }

        #endregion

        private void lstTierNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTierName.Text = lstTierNames.Text;
        }

        private void btnTierName_Click(object sender, EventArgs e)
        {
            if (lstTierNames.SelectedIndex >= 0)
                lstTierNames.Items[lstTierNames.SelectedIndex] = txtTierName.Text;
        }

        private void lvwAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvwFiles.Items.Clear();

            try
            {
                if (lvwAudio.SelectedItems.Count == 0)
                {
                    tmrGreyHack.Enabled = true;
                    return;
                }

                ListViewItem li = new ListViewItem("Add Audio");
                li.ImageIndex = 0;
                lvwFiles.Items.Add(li);

                int i = 1;
                foreach (AudioFile af in ((ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag).AudioFiles)
                {
                    li = lvwFiles.Items.Add(new ListViewItem(string.Format("Audio {0}", (i++).ToString())));
                    li.Tag = af;
                    li.ImageIndex = 1;
                    li.ToolTipText = af.Name;
                }

                setPreviewSettings();
            }
            finally
            {
                tabAudio_SelectedIndexChanged(this, e);
            }
        }

        private void hscPreviewStart_Scroll(object sender, ScrollEventArgs e)
        {
            if (lvwAudio.SelectedItems.Count > 0)
                ((ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag).PreviewStart = hscPreviewStart.Value * 1000;
            setPreviewSettings();
        }

        private void udPreviewStart_ValueChanged(object sender, EventArgs e)
        {
            if (lvwAudio.SelectedItems.Count > 0)
                ((ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag).PreviewStart = (int)udPreviewStart.Value * 1000;
            setPreviewSettings();
        }

        private void udPreviewLength_ValueChanged(object sender, EventArgs e)
        {
            if (lvwAudio.SelectedItems.Count > 0)
                ((ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag).PreviewLength = (int)udPreviewLength.Value * 1000;
            setPreviewSettings();
        }

        private void udPreviewFade_ValueChanged(object sender, EventArgs e)
        {
            if (lvwAudio.SelectedItems.Count > 0)
                ((ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag).PreviewFadeLength = (int)udPreviewFade.Value * 1000;
            setPreviewSettings();
        }

        private void udPreviewVolume_ValueChanged(object sender, EventArgs e)
        {
            if (lvwAudio.SelectedItems.Count > 0)
                ((ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag).PreviewVolume = (int)udPreviewVolume.Value;
            setPreviewSettings();
        }

        /// <summary>
        /// Enables the preview window
        /// </summary>
        /// <returns></returns>
        private void setPreviewSettings()
        {
            gboPreview.Enabled = lvwFiles.Items.Count > 1;

            if (_applyingSettings)
                return;

            try
            {
                _applyingSettings = true;

                if (gboPreview.Enabled)
                {
                    ProjectBackgroundAudio b = (ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag;

                    b.SetSafePreviewSettings();

                    if (_lastAudioLen != Math.Max(0, (b.AudioLength / 1000) - (b.PreviewLength / 1000)))
                    {
                        _lastAudioLen = Math.Max(0, (b.AudioLength / 1000) - (b.PreviewLength / 1000));

                        udPreviewStart.Maximum = _lastAudioLen;
                        hscPreviewStart.Maximum = (b.AudioLength / 1000); //is seconds
                        udPreviewLength.Maximum = hscPreviewStart.Maximum;

                        hscPreviewStart.SmallChange = 1;
                        hscPreviewStart.LargeChange = (b.PreviewLength / 1000);
                    }

                    udPreviewStart.Value = Math.Min(udPreviewStart.Maximum, b.PreviewStart / 1000);
                    udPreviewFade.Value = Math.Min(udPreviewFade.Maximum, b.PreviewFadeLength / 1000);
                    udPreviewLength.Value = Math.Min(udPreviewLength.Maximum, b.PreviewLength / 1000);
                    udPreviewVolume.Value = Math.Min(udPreviewVolume.Maximum, b.PreviewVolume);
                    hscPreviewStart.Value = (int)udPreviewStart.Value;

                    if (udPreviewLength.Value != 0)
                        udPreviewFade.Maximum = (int)((float)udPreviewLength.Value / 2F);
                    else
                    {
                        udPreviewLength.Value = 0;
                        udPreviewFade.Maximum = 0;
                    }

                    if (_player != null) //playing 
                    {
                        tmrPreview.Enabled = false;
                        tmrPreview.Enabled = true; //reset timer
                    }
                }
                else
                {
                    stopPreview();
                    setDefaults();
                }
            }
            finally
            {
                _applyingSettings = false;
            }

        }


        private void lvwFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                lvwFiles.Focus();
                e.Effect = DragDropEffects.All;
            }
            else
                e.Effect = DragDropEffects.None;

        }

        private void lvwFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (lvwAudio.SelectedItems.Count == 0)
                return;

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                stopPreview();

                foreach (string x in s)
                {
                    if (File.Exists(x))
                    {
                        ProjectBackgroundAudio ba = (ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag;
                        ListViewItem li = lvwFiles.HitTest(lvwFiles.PointToClient(new Point(e.X, e.Y))).Item;
                        if (li != null)
                        {
                            ListViewItem add = lvwFiles.Items[0];
                            AudioFile af = ba.CreateAudioFile(x);
                            if (add == li)
                            {
                                li = new ListViewItem(string.Format("Audio {0}", lvwFiles.Items.Count.ToString()));
                                lvwFiles.Items.Add(li);
                                ba.AudioFiles.Add(af);
                            }
                            else
                                ba.AudioFiles[li.Index - 1] = ba.CreateAudioFile(x);

                            try
                            {
                                base.MessageShow("Decoding Audio...");
                                ba.Import();
                            }
                            finally
                            {
                                base.MessageHide();
                            }

                            li.Tag = af;
                            li.ImageIndex = 1;
                            li.ToolTipText = af.Name;

                            if (ba.AudioFiles.Count == 1)
                                setDefaults();

                            setPreviewSettings();
                        }
                    }
                }
            }
        }

        private void setDefaults()
        {
            if (lvwAudio.SelectedItems.Count == 0)
                return;

            ProjectBackgroundAudio ba = (ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag;
            ba.PreviewStart = 0;
            ba.PreviewVolume = _project.Defaults.PreviewVolume;
            ba.PreviewLength = 60000;
            ba.PreviewFadeLength = 1000;
        }

        private void lvwFiles_DragOver(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                ListViewItem li = lvwFiles.HitTest(lvwFiles.PointToClient(new Point(e.X, e.Y))).Item;
                if (li != null)
                    li.Selected = true;
            }
        }

        private void btnPreviewPlay_Click(object sender, EventArgs e)
        {
            if (_player != null)
                stopPreview();
            else
                playPreview();
        }

        private void playPreview()
        {
            if (_player != null)
                stopPreview();

            if (lvwAudio.SelectedItems.Count == 0)
                return;

            ProjectBackgroundAudio ba = (ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag;

            foreach (AudioFile af in ba.AudioFiles)
            {
                if (!File.Exists(af.Name))
                {
                    MessageBox.Show(this, string.Format("'{0}' does not exist.", af.Name), "Missing Audio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            //if no raw files then decode them now.
            bool prevEnabled = gboPreview.Enabled;
            try
            {
                gboPreview.Enabled = false;
                if (ba.HasMissingRawAudio())
                {
                    base.MessageShow("Decoding Audio...");
                    ba.Import();
                }
                base.MessageShow("Creating Preview...");
                ba.CreatePreview(false);
            }
            finally
            {
                gboPreview.Enabled = prevEnabled;
                base.MessageHide();
            }

            if (File.Exists(ba.RawAudioFile))
            {
                _player = new AudioPlayer(ba.RawAudioFile);
                _player.WavStatusChanged += new WavStatusChangedEventHandler(_player_WavStatusChanged);
                _player.Play(0, false);
            }
        }

        private void stopPreview()
        {
            if (_player != null)
            {
                _player.Stop();
                _player.WavStatusChanged -= new WavStatusChangedEventHandler(_player_WavStatusChanged);
                _player = null;
            }
        }

        private void _player_WavStatusChanged(object source, WavStatusChangedEventArgs e)
        {
            if (btnPreviewPlay.InvokeRequired)
            {
                btnPreviewPlay.Invoke((MethodInvoker)delegate { _player_WavStatusChanged(source, e); });
                return;
            }

            if (e.Status == WavStatus.Playing)
                btnPreviewPlay.Text = "Stop";
            else if (e.Status == WavStatus.Stopped)
                btnPreviewPlay.Text = "Play";
            else if (e.Status == WavStatus.End) //end reached. Must be stopped from the main thread.
                stopPreview();
        }

        private void tmrPreview_Tick(object sender, EventArgs e)
        {
            tmrPreview.Enabled = false; //must do this first or 2 events will be fired.
            playPreview();
        }

        private void lvwFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwFiles.SelectedItems.Count != 0)
            {
                mnuRemoveAudio.Enabled = lvwFiles.SelectedItems[0].ImageIndex == 1;
                mnuPlayAudio.Enabled = lvwFiles.SelectedItems[0].ImageIndex == 1;

                mnuAudio.Show(lvwFiles, e.Location);
            }
        }

        private void mnuRemoveAudio_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count != 0)
            {
                ProjectBackgroundAudio b = (ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag;
                ListViewItem li = lvwFiles.SelectedItems[0];

                b.AudioFiles.Remove((AudioFile)li.Tag);
                lvwFiles.Items.Remove(li);

                int i = 1;
                foreach (ListViewItem l in lvwFiles.Items)
                {
                    if (l.Text.StartsWith("Audio "))
                        l.Text = string.Format("Audio {0}", (i++).ToString());
                }

            }
        }

        private void tabAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabAudio.SelectedIndex != 1)
                return;

            lstVolume.Items.Clear();

            _settingVolume = true; //supress event
            udVolume.Value = 0;
            _settingVolume = false;

            udVolume.Enabled = lvwAudio.SelectedItems.Count != 0;
            chkVolumeApplyAll.Enabled = udVolume.Enabled;

            if (lvwAudio.SelectedItems.Count == 0)
                return;

            ProjectBackgroundAudio b = (ProjectBackgroundAudio)lvwAudio.SelectedItems[0].Tag;

            for (int i = 0; i < b.AudioFiles.Count; i++)
                addVolumeItem(string.Format("Audio {0}", (i + 1).ToString()), b.AudioFiles[i]);

            if (lstVolume.Items.Count != 0)
                lstVolume.Items[0].Selected = true;
        }

        private void addVolumeItem(string title, AudioFile audio)
        {
            ListViewItem l;

            l = new ListViewItem();
            l.Text = title;
            l.SubItems.Add(audio.Volume.ToString());
            l.ToolTipText = audio.Name;
            l.ImageIndex = 0;
            l.Tag = audio;
            lstVolume.Items.Add(l);
        }

        private void lstVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _settingVolume = true;
                if (lstVolume.SelectedItems.Count != 0)
                {
                    udVolume.Value = ((AudioFile)lstVolume.SelectedItems[0].Tag).Volume;
                    udVolume.Tag = lstVolume.SelectedItems[0];
                    udVolume.Enabled = true;
                }
                else
                {
                    udVolume.Tag = null;
                    udVolume.Enabled = false;
                }
            }
            finally
            {
                _settingVolume = false;
            }
        }

        private void udVolume_ValueChanged(object sender, EventArgs e)
        {
            if (_settingVolume)
                return;

            ListViewItem li = udVolume.Tag as ListViewItem;
            if (li != null)
            {
                if (!chkVolumeApplyAll.Checked)
                {
                    li.SubItems[1].Text = udVolume.Value.ToString();
                    ((AudioFile)li.Tag).Volume = (int)udVolume.Value;
                }
                else
                {
                    foreach (ListViewItem l in lstVolume.Items)
                    {
                        l.SubItems[1].Text = udVolume.Value.ToString();
                        ((AudioFile)l.Tag).Volume = (int)udVolume.Value;
                    }
                }

                if (_player != null) //playing 
                {
                    tmrPreview.Enabled = false;
                    tmrPreview.Enabled = true; //reset timer
                }

            }
        }

        private void tmrGreyHack_Tick(object sender, EventArgs e)
        {
            //this hack is in place because the listview box for the background audio items fires twice, the first time with 0 items selected
            //this stops the preview playback when switching items.

            tmrGreyHack.Enabled = false;
            if (lvwAudio.SelectedItems.Count == 0)
                setPreviewSettings(); //disable the preview window
        }

        private void trk_MouseUp(object sender, MouseEventArgs e)
        {
            _startVolumes = null;
            trk.Value = 0;
        }

        private void trk_MouseDown(object sender, MouseEventArgs e)
        {
            _startVolumes = new int[lstVolume.Items.Count];
            for (int i = 0; i < _startVolumes.Length; i++)
                _startVolumes[i] = ((AudioFile)lstVolume.Items[i].Tag).Volume;
        }

        private void trk_ValueChanged(object sender, EventArgs e)
        {
            if (_startVolumes == null)
                return;

            float value = trk.Value / 100F;

            int v = 0;
            for (int i = 0; i < _startVolumes.Length; i++)
            {
                float x = (float)_startVolumes[i];
                v = (int)Math.Round((decimal)(x + (x * value)), 0);
                ((AudioFile)lstVolume.Items[i].Tag).Volume = v;
                lstVolume.Items[i].SubItems[1].Text = ((AudioFile)lstVolume.Items[i].Tag).Volume.ToString();
            }

            try
            {
                _settingVolume = true;
                udVolume.Value = v;
            }
            catch
            {
                _settingVolume = false;
            }

            if (_player != null) //playing 
            {
                tmrPreview.Enabled = false;
                tmrPreview.Enabled = true; //reset timer
            }
        }

        private int[] _startVolumes;

        private bool _settingVolume;
        private bool _applyingSettings;
        private int _lastAudioLen;
        private Project _project;
        private AudioPlayer _player;


    }
}
