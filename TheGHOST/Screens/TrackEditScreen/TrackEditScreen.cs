using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Nanook.TheGhost
{
    public partial class TrackEditScreen : ScreenBase
    {
        public TrackEditScreen()
        {
            InitializeComponent();
            _song = null;
            _lastAudioLen = 0;
            tmrPreview.Enabled = false;
            tmrPreview.Interval = 500;
            _importingAudio = false;
            _settingVolume = false;
            _startVolumes = null;
        }

        public void SetSong(ProjectSong song)
        {
            _song = song;
            
            if (_song != null)
            {
                setTitle();

                _song.Title = _song.Title;
                _song.Artist = _song.Artist;
                _song.Year = _song.Year;

                //proxy class for the GUI, the core doesn't know about the interface objects
                _trackProperties = new TrackProperties(_song);

                pgdSong.SelectedObject = _trackProperties;

                _song.Audio.AudioFileChanged += new AudioFileChangedEventHandler(Audio_AudioFileChanged);
                _song.Audio.PreviewSettingsChanged += new PreviewSettingsChangedEventHandler(Audio_PreviewSettingsChanged);

                //Change track details and preview
                foreach (AudioFile af in _song.Audio.SongFiles)
                    audioFileChanged("song", af.Name, true);
                if (_song.Audio.GuitarFile != null && _song.Audio.GuitarFile.Name.Length != 0)
                    audioFileChanged("guitar", _song.Audio.GuitarFile.Name, true);
                if (_song.Audio.RhythmFile != null && _song.Audio.RhythmFile.Name.Length != 0)
                    audioFileChanged("rhythm", _song.Audio.RhythmFile.Name, true);

                setPreviewSettings();

            }
        }

        private void setTitle()
        {
            int songIdx = 0;
            //find song index
            for (int i = 0; i < _project.EditSongs.Length; i++)
            {
                if (_project.EditSongs[i] == _song)
                {
                    songIdx = i + 1;
                    break;
                }
            }

            ProjectTierSong pts = null;
            if (_project.Songs.ContainsKey(_song.SongQb.Id.Crc))
                pts = _project.Songs[_song.SongQb.Id.Crc];

            gboTrackEdit.Text = string.Format("Replace Audio  '{2} - {3}'  [Tier {0} / Song {1}] ({4} of {5})", pts.Tier.Number.ToString(), pts.Number.ToString(), _song.Artist, _song.Title, songIdx.ToString(), _project.EditSongs.Length.ToString());
        }

        private void Audio_PreviewSettingsChanged(object source, PreviewSettingsChangedEventArgs e)
        {
            setPreviewSettings();
        }

        #region ScreenBase Members

        public override string TitleMessage()
        {
            return "Drag your audio files over the tracks to replace them.\r\nSong audio must be specified.\r\nSmart Mode accepts FOF folders, Notes files and Song.ini files.";
        }

        public override void Construct(Project project)
        {
            _project = project;
        }

        public override void Open()
        {
            setTitle();
        }

        public override bool Close(bool appQuiting)
        {
            stopPreview();

            if (!appQuiting)
            {
                if (_importingAudio)
                {
                    MessageBox.Show(this, "Audio is still being decoded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.DoEvents();
                    return false;
                }

                _song.Audio.RemoveAndDeleteRedundantFiles();

                if ((_song.Audio.GuitarFile != null || _song.Audio.RhythmFile != null) && _song.Audio.SongFiles.Count == 0)
                {
                    MessageBox.Show(this, "Song audio file must be specified.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                pgdSong.Refresh();

                createPreviewWav();
            }
            return true;
        }

        #endregion

        private void tabAudio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabAudio.SelectedIndex != 1)
                return;

            lstVolume.Items.Clear();

            for (int i = 0; i < _song.Audio.SongFiles.Count; i++)
                addVolumeItem("Song File", _song.Audio.SongFiles[i]);

            if (_song.Audio.GuitarFile != null)
                addVolumeItem("Guitar File", _song.Audio.GuitarFile);

            if (_song.Audio.RhythmFile != null)
                addVolumeItem("Rhythm File", _song.Audio.RhythmFile);

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
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                ListViewItem li = lvwFiles.HitTest(lvwFiles.PointToClient(new Point(e.X, e.Y))).Item;
                if (li != null)
                {
                    try
                    {
                        if (_importingAudio)
                            return;

                        _importingAudio = true;
                        int songCount = _song.Audio.SongFiles.Count;

                        if (li.Tag is string && (string)li.Tag == "SmartMode")
                        {
                            if (Directory.Exists(s[0]))
                            {
                                try
                                {
                                    base.MessageShow("Importing Folder...");
                                    _song.AutoImportDirectory(s[0], _project.Defaults.SmartModeCrowdImport);
                                    base.MessageHide();
                                }
                                catch (Exception ex)
                                {
                                    base.MessageHide();
                                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                            else if (s[0].ToLower().EndsWith(@"\song.ini"))
                                _song.AutoImportSongIni(s[0]);
                            else if (s[0].ToLower().EndsWith(@".chart"))
                                _song.Notes.ParseFile(s[0]);

                        }
                        else
                        {

                            string rawFilename = null;
                            string srcFilename = s[0];

                            bool added = false;

                            if ((_song.Audio.GuitarFile != null && srcFilename.ToLower() == _song.Audio.GuitarFile.Name.ToLower()) || (_song.Audio.RhythmFile != null && srcFilename.ToLower() == _song.Audio.RhythmFile.Name.ToLower()))
                                added = true;

                            foreach (AudioFile af in _song.Audio.SongFiles)
                            {
                                if (s[0].ToLower() == af.Name.ToLower())
                                {
                                    added = true;
                                    break;
                                }
                            }
                            if (added)
                            {
                                MessageBox.Show(this, string.Format("'{0}' has already been added.", srcFilename), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }


                            if (li.Tag is string && (string)li.Tag == "AddSong")
                            {
                                //add new song
                                _song.Audio.SongFiles.Add(_song.Audio.CreateAudioFile(srcFilename, _project.Defaults.AudioSongVolume));

                                rawFilename = _song.Audio.RawSongFilenames[_song.Audio.SongFiles.Count - 1];
                                _lastAudioLen = int.MinValue;  //force preview interface to be updated
                            }
                            else if (li.Index == 0 || li.Index >= 0 && li.Index < songCount)
                            {
                                if (li.Index >= _song.Audio.SongFiles.Count)
                                    _song.Audio.SongFiles.Add(_song.Audio.CreateAudioFile(srcFilename, _project.Defaults.AudioSongVolume));
                                else
                                    _song.Audio.SongFiles[li.Index] = _song.Audio.CreateAudioFile(srcFilename, _project.Defaults.AudioSongVolume);
                                rawFilename = _song.Audio.RawSongFilenames[li.Index];
                                _lastAudioLen = int.MinValue;  //force preview interface to be updated
                            }
                            else if (li.Index == songCount + 1)
                            {
                                _song.Audio.GuitarFile = _song.Audio.CreateAudioFile(srcFilename, _project.Defaults.AudioGuitarVolume);
                                rawFilename = _song.Audio.RawGuitarFilename;
                            }
                            else if (li.Index == songCount + 2)
                            {
                                _song.Audio.RhythmFile = _song.Audio.CreateAudioFile(srcFilename, _project.Defaults.AudioRhythmVolume);
                                rawFilename = _song.Audio.RawRhythmFilename;
                            }

                            try
                            {
                                if (rawFilename != null)
                                {
                                    try
                                    {
                                        base.MessageShow("Decoding Audio...");
                                        _song.Audio.Import(srcFilename, rawFilename);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            //remove failed audio
                                            if (li.Tag is string && (string)li.Tag == "AddSong")
                                                _song.Audio.SongFiles.RemoveAt(_song.Audio.SongFiles.Count - 1);
                                            else if (li.Index == 0 || li.Index >= 0 && li.Index < songCount)
                                            {
                                                for (int i = 0; i < _song.Audio.SongFiles.Count; i++)
                                                {
                                                    if (_song.Audio.SongFiles[i].Name.ToLower() == srcFilename.ToLower())
                                                    {
                                                        _song.Audio.SongFiles.RemoveAt(i);
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (li.Index == songCount + 1)
                                                _song.Audio.GuitarFile = null;
                                            else if (li.Index == songCount + 2)
                                                _song.Audio.RhythmFile = null;
                                        }
                                        catch
                                        {
                                        }
                                        throw;
                                    }
                                    finally
                                    {
                                        base.MessageHide();
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show(this, string.Format("Failed to import audio '{0}'.", srcFilename), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }

                        }

                        pgdSong.Refresh();

                        //enable preview
                        setPreviewSettings();

                        Application.DoEvents();

                        createPreviewWav();
                    }
                    catch
                    {
                        MessageBox.Show(this, string.Format("Failed to import {0}.", s[0]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        _importingAudio = false;
                    }
                }
            }
        }

        private void Audio_AudioFileChanged(object source, AudioFileChangedEventArgs e)
        {
            audioFileChanged(e.Type, e.Filename, e.AddedRemoved);
        }

        private void audioFileChanged(string type, string filename, bool addedRemoved)
        {
            if (lvwFiles.InvokeRequired)
                return; // on last screen don't update

            ListViewItem li = null;

            if (type == "song")
            {
                //hack to get the inserted item to go in to the corect place, it's a known bug
                lvwFiles.BeginUpdate();
                lvwFiles.View = View.List;

                //go back to 1 song
                int c = lvwFiles.Items.Count - 1;
                while (c > 0)
                {
                    if (lvwFiles.Items[c].Tag is string && (((string)lvwFiles.Items[c].Tag) == "song" || ((string)lvwFiles.Items[c].Tag) == "AddSong"))
                        lvwFiles.Items.RemoveAt(c);
                    c--;
                }

                int songCount = _song.Audio.SongFiles.Count;

                //reset the first item
                lvwFiles.Items[0].ImageIndex = 0;
                lvwFiles.Items[0].ToolTipText = string.Empty;

                for (int i = 0; i < _song.Audio.SongFiles.Count; i++)
                    li = lvwFiles.Items.Insert(1, (ListViewItem)lvwFiles.Items[0].Clone());

                //create the add song icon
                if (songCount != 0)
                {
                    //li = lvwFiles.Items.Insert(songCount, (ListViewItem)lvwFiles.Items[0].Clone());
                    lvwFiles.Items[songCount].Text = "Add Song";
                    lvwFiles.Items[songCount].Tag = "AddSong";
                    lvwFiles.Items[songCount].ToolTipText = "All added songs are merged together";
                }

                for (int i = 0; i < _song.Audio.SongFiles.Count; i++)
                {
                    li = lvwFiles.Items[i];
                    li.ToolTipText = _song.Audio.SongFiles[i].Name;
                    li.Selected = true;
                    li.ImageIndex = 1;
                }

                lvwFiles.View = View.LargeIcon;
                lvwFiles.EndUpdate();

                li = null; //this stops any remaining songs being greyed out when removing items.
            }
            else if (type == "guitar")
            {
                foreach (ListViewItem lv in lvwFiles.Items)
                {
                    if (lv.Tag is string && ((string)lv.Tag) == "guitar")
                        li = lv;
                }
                if (li != null && _song.Audio.GuitarFile != null)
                    li.ToolTipText = _song.Audio.GuitarFile.Name;
            }
            else if (type == "rhythm")
            {
                foreach (ListViewItem lv in lvwFiles.Items)
                {
                    if (lv.Tag is string && ((string)lv.Tag) == "rhythm")
                        li = lv;
                }
                if (li != null && _song.Audio.RhythmFile != null)
                    li.ToolTipText = _song.Audio.RhythmFile.Name;
            }

            if (li != null)
            {
                li.Selected = true;
                li.ImageIndex = addedRemoved ? 1 : 0;
            }
            pgdSong.Refresh();
            setPreviewSettings();
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
                string fn = null;
                ListViewItem li = lvwFiles.SelectedItems[0];
                li.ImageIndex = 0;

                int songCount = _song.Audio.SongFiles.Count;

                if (li.Tag is string && (string)li.Tag == "AddSong") //add new song audio
                {
                    //nothing
                }
                else if (li.Index == 0 || li.Index >= 0 && li.Index < songCount)
                {
                    fn = _song.Audio.RawSongFilenames[li.Index];
                    _song.Audio.SongFiles.RemoveAt(li.Index);
                }
                else if (li.Index == songCount + 1)
                {
                    fn = _song.Audio.RawGuitarFilename;
                    _song.Audio.GuitarFile = null;
                }
                else if (li.Index == songCount + 2)
                {
                    fn = _song.Audio.RawRhythmFilename;
                    _song.Audio.RhythmFile = null;
                }

                if (fn != null && File.Exists(fn))
                    FileHelper.Delete(fn);

                pgdSong.Refresh();
                setPreviewSettings();
            }
        }

        /// <summary>
        /// Enables the preview window
        /// </summary>
        /// <returns></returns>
        private void setPreviewSettings()
        {
            gboPreview.Enabled = _song.Audio.SongFiles.Count != 0;

            if (_applyingSettings)
                return;

            try
            {
                _applyingSettings = true;

                if (gboPreview.Enabled)
                {
                    _song.Audio.SetSafePreviewSettings();

                    if (_lastAudioLen != Math.Max(0, (_song.Audio.AudioLength / 1000) - (_song.Audio.PreviewLength / 1000)))
                    {
                        _lastAudioLen = Math.Max(0, (_song.Audio.AudioLength / 1000) - (_song.Audio.PreviewLength / 1000));

                        udPreviewStart.Maximum = _lastAudioLen;
                        hscPreviewStart.Maximum = (_song.Audio.AudioLength / 1000); //is seconds
                        udPreviewLength.Maximum = hscPreviewStart.Maximum;

                        hscPreviewStart.SmallChange = 1;
                        hscPreviewStart.LargeChange = (_song.Audio.PreviewLength / 1000);
                    }

                    udPreviewStart.Value = Math.Min(udPreviewStart.Maximum, _song.Audio.PreviewStart / 1000);
                    udPreviewFade.Value = Math.Min(udPreviewFade.Maximum, _song.Audio.PreviewFadeLength / 1000);
                    udPreviewLength.Value = Math.Min(udPreviewLength.Maximum, _song.Audio.PreviewLength / 1000);
                    udPreviewVolume.Value = Math.Min(udPreviewVolume.Maximum, _song.Audio.PreviewVolume);


                    hscPreviewStart.Value = (int)udPreviewStart.Value;
                    if (udPreviewLength.Value != 0)
                        udPreviewFade.Maximum = (int)((float)udPreviewLength.Value / 2F);
                    else
                    {
                        udPreviewLength.Value = 0;
                        udPreviewFade.Maximum = 0;
                    }

                    chkPreviewGuitar.Enabled = _song.Audio.GuitarFile != null;
                    chkPreviewRhythm.Enabled = _song.Audio.RhythmFile != null;

                    chkPreviewGuitar.Checked = _song.Audio.PreviewIncludeGuitar;
                    chkPreviewRhythm.Checked = _song.Audio.PreviewIncludeRhythm;

                    if (_player != null) //playing 
                    {
                        tmrPreview.Enabled = false;
                        tmrPreview.Enabled = true; //reset timer
                    }
                }
            }
            finally
            {
                _applyingSettings = false;
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

            string missingFileName = null;
            foreach (AudioFile af in _song.Audio.SongFiles)
            {
                if (!File.Exists(af.Name))
                {
                    missingFileName = af.Name;
                    break;
                }
            }
            if (_song.Audio.GuitarFile != null && !File.Exists(_song.Audio.GuitarFile.Name))
                missingFileName = _song.Audio.GuitarFile.Name;
            if (_song.Audio.RhythmFile != null && !File.Exists(_song.Audio.RhythmFile.Name))
                missingFileName = _song.Audio.RhythmFile.Name;

            if (missingFileName != null)
            {
                MessageBox.Show(this, string.Format("'{0}' does not exist.", missingFileName), "Missing Audio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            //if no raw files then decode them now.
            bool prevEnabled = gboPreview.Enabled;
            try
            {
                gboPreview.Enabled = false;
                if (_song.Audio.HasMissingRawAudio())
                {
                    base.MessageShow("Decoding Audio...");
                    _song.Audio.Import();
                }
                base.MessageShow("Creating Preview...");
                createPreviewWav();
            }
            finally
            {
                gboPreview.Enabled = prevEnabled;
                base.MessageHide();
            }
            _player = new AudioPlayer(_song.Audio.RawPreviewFilename);
            _player.WavStatusChanged += new WavStatusChangedEventHandler(_player_WavStatusChanged);
            _player.Play(0, false);
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

        private void createPreviewWav()
        {
            stopPreview();
            _song.Audio.CreateRawPreview((int)udPreviewStart.Value * 1000, (int)udPreviewLength.Value * 1000, (int)udPreviewFade.Value * 1000, (int)udPreviewVolume.Value, chkPreviewGuitar.Checked, chkPreviewRhythm.Checked, false);
        }

        private void hscPreviewStart_Scroll(object sender, ScrollEventArgs e)
        {
            _song.Audio.PreviewStart = hscPreviewStart.Value * 1000;
        }

        private void udPreviewStart_ValueChanged(object sender, EventArgs e)
        {
            _song.Audio.PreviewStart = (int)udPreviewStart.Value * 1000;
        }

        private void udPreviewLength_ValueChanged(object sender, EventArgs e)
        {
            _song.Audio.PreviewLength = (int)udPreviewLength.Value * 1000;
        }

        private void udPreviewFade_ValueChanged(object sender, EventArgs e)
        {
            _song.Audio.PreviewFadeLength = (int)udPreviewFade.Value * 1000;
        }

        private void udPreviewVolume_ValueChanged(object sender, EventArgs e)
        {
            _song.Audio.PreviewVolume = (int)udPreviewVolume.Value;
        }

        private void chkPreviewGuitar_CheckedChanged(object sender, EventArgs e)
        {
            _song.Audio.PreviewIncludeGuitar = chkPreviewGuitar.Checked;
        }

        private void chkPreviewRhythm_CheckedChanged(object sender, EventArgs e)
        {
            _song.Audio.PreviewIncludeRhythm = chkPreviewRhythm.Checked;
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

        private void tmrPreview_Tick(object sender, EventArgs e)
        {
            tmrPreview.Enabled = false; //must do this first or 2 events will be fired.
            playPreview();
        }

        private int[] _startVolumes;

        private bool _settingVolume;

        private bool _applyingSettings;
        private bool _importingAudio;

        private ProjectSong _song;
        private TrackProperties _trackProperties;
        private int _lastAudioLen;

        private AudioPlayer _player;

        private Project _project;




    }
}
