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
    public partial class NotesEditScreen : ScreenBase
    {
        public NotesEditScreen()
        {
            InitializeComponent();
            _song = null;
        }


        public void SetSong(ProjectSong song)
        {
            _song = song;

            if (_song != null)
            {
                _song.Notes.GhItemMapChanged += new GhItemMapChangedEventHandler(Notes_GhItemMapChanged);
                _song.Notes.NotesFileChanged += new NotesFileChangedEventHandler(Notes_NotesFileChanged);

                setTitle();

                _notesDragUniqueId = 0;

                foreach (GhNotesItem gii in _song.Notes.GhItems)
                    addGhItem(gii);

                _ghItems = _song.Notes.GhItems; //hold a local copy

                //populate any already configured notes files
                foreach (NotesFile nf in _song.Notes.Files)
                    notesFileChanged(NotesFileChangeType.Added, nf);

                refreshMappedItemInformation();

                udDelaySeconds.Value = (decimal)(_song.MinMsBeforeNotesStart / 1000F);
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

            gboNotesEdit.Text = string.Format("Replace Notes  '{2} - {3}'  [Tier {0} / Song {1}] ({4} of {5})", pts.Tier.Number.ToString(), pts.Number.ToString(), _song.Artist, _song.Title, songIdx.ToString(), _project.EditSongs.Length.ToString());
        }

        #region ScreenBase Members

        public override string TitleMessage()
        {
            return "Drag notes files into this window and match up the difficulties etc.\r\nUse the 'Smart Map' button will create missing items.\r\nUse Right-Click to for additional functionality. Use 'View...' to sync the notes etc.";
        }

        public override void Construct(Project project)
        {
            _project = project;
        }

        public override void Open()
        {
            setTitle();
            refreshMappedItemInformation();
        }

        public override bool Close(bool appQuiting)
        {
            if (!appQuiting)
            {
                _song.MinMsBeforeNotesStart = (int)udDelaySeconds.Value * 1000;
                smartMap(false);
            }
            return true;
        }

        #endregion

        private List<string> getWavs()
        {
            List<string> wavs = new List<string>();
            if (_song.Audio.SongFiles.Count != 0)
                wavs.AddRange(_song.Audio.RawSongFilenames);  //get all wavs
            if (_song.Audio.GuitarFile != null && _song.Audio.GuitarFile.Name.Length != 0)
                wavs.Add(_song.Audio.RawGuitarFilename);
            if (_song.Audio.RhythmFile != null && _song.Audio.RhythmFile.Name.Length != 0)
                wavs.Add(_song.Audio.RawRhythmFilename);

            return wavs;
        }

        private void ensureAudio()
        {
            //if no raw files then decode them now.
            if (_song.Audio.HasMissingRawAudio())
            {
                try
                {
                    base.MessageShow("Decoding Audio...");
                    _song.Audio.Import();
                }
                finally
                {
                    base.MessageHide();
                }
            }        
        }

        private void btnGhView_Click(object sender, EventArgs e)
        {
            //bool hasMapped = false;

            if (lvwFiles.Items.Count != 0)
            {
                ensureAudio();

                List<string> wavs = getWavs();

                //hasMapped = true;
                NotesEditorForm frm = new NotesEditorForm();
                frm.Initialise(_song.Notes.BaseFile, _ghItems, _song.Notes.HoPoMeasure, _song.Notes.Gh3SustainClipping, wavs.ToArray());
                frm.ShowDialog();
                _song.Notes.HoPoMeasure = frm.HoPoMeasure;
                _song.Notes.Gh3SustainClipping = frm.Gh3SustainClipping;
                frm.Dispose();

                //refresh the list in case mappings have been added
                refreshMappedItemInformation();

            }


            //if (!hasMapped)
            //    MessageBox.Show(string.Format("There are no mapped note items."), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void viewMappedNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnGhView_Click(sender, e);
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
                ensureAudio();

                FileInfo fi = new FileInfo(s[0]);
                _song.Notes.ParseFile(fi.FullName);

                //the notes view is displayed as model, if we show it on this thread, the app passing us the dragged file is locked, show on our thread using a timer
                Timer t = new Timer();
                t.Interval = 1;
                t.Tick += new EventHandler(t_Tick);
                t.Enabled = true;
            }

        }

        void t_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Enabled = false;
            viewNotesToolStripMenuItem_Click(this, new EventArgs());
        }

        private void Notes_NotesFileChanged(object source, NotesFileChangedEventArgs e)
        {
            notesFileChanged(e.ChangeType, e.NotesFile);
        }

        private void notesFileChanged(NotesFileChangeType changeType, NotesFile notesFile)
        {
            if (changeType != NotesFileChangeType.Removed)
            {
                FileInfo fi = new FileInfo(notesFile.Filename);

                ListViewItem li = null;
                if (changeType == NotesFileChangeType.Added)
                    li = lvwFiles.Items.Add(string.Format("{0} ({1})", (lvwFiles.Items.Count + 1).ToString(), fi.Extension.Substring(1)));
                else //reloaded
                {
                    foreach (ListViewItem lvi in lvwFiles.Items)
                    {
                        if (((NotesFile)lvi.Tag).Filename.ToLower() == notesFile.Filename.ToLower())
                        {
                            li = lvi;
                            break;
                        }
                    }
                }

                if (li != null)
                {
                    li.ImageIndex = notesFile == _song.Notes.BaseFile ? 1 : 0;
                    li.ToolTipText = fi.FullName;
                    li.Tag = notesFile;  //must be set before selecting.
                    li.Selected = true;
                }
                Application.DoEvents();

                _song.Notes.SmartMap(notesFile, true);
            }
            else
            {
                int idx = -1;
                foreach (ListViewItem li in lvwFiles.Items)
                {
                    if (li.Tag == notesFile)
                    {
                        idx = li.Index;
                        break;
                    }
                }

                if (idx != -1)
                    lvwFiles.Items.RemoveAt(idx);
            }
        }

        private void lvwFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwFiles.SelectedItems.Count != 0)
            {
                useForFretsEtcToolStripMenuItem.Checked = _song.Notes.BaseFile == lvwFiles.SelectedItems[0].Tag;
                mnuFiles.Show(lvwFiles, e.Location);
            }
        }

        private void generateFaceOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool skipped = false;
            foreach (ListViewItem li in lvwGh.SelectedItems)
            {
                if (li != null)
                {
                    if (li.Tag != null)
                        _song.Notes.GenerateNotes((GhNotesItem)li.Tag, false, false, false, true, false);
                    else
                        skipped = true;
                }
            }
            refreshMappedItemInformation();

            if (skipped)
                MessageBox.Show(this, "1 or more items could not be generated because they have no notes.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void generateBattleModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool skipped = false;
            foreach (ListViewItem li in lvwGh.SelectedItems)
            {
                if (li != null)
                {
                    if (li.Tag != null)
                        _song.Notes.GenerateNotes((GhNotesItem)li.Tag, false, false, true, false, false);
                    else
                        skipped = true;
                }
            }
            refreshMappedItemInformation();

            if (skipped)
                MessageBox.Show(this, "1 or more items could not be generated because they have no notes.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void generateStarPowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool skipped = false;
            foreach (ListViewItem li in lvwGh.SelectedItems)
            {
                if (li != null)
                {
                    if (li.Tag != null)
                        _song.Notes.GenerateNotes((GhNotesItem)li.Tag, false, true, false, false, false);
                    else
                        skipped = true;
                }
            }
            refreshMappedItemInformation();

            if (skipped)
                MessageBox.Show(this, "1 or more items could not be generated because they have no notes.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void forceNoStarPowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _song.Notes.ForceNoStarPower = !_song.Notes.ForceNoStarPower;
            forceNoStarPowerToolStripMenuItem.Checked = _song.Notes.ForceNoStarPower;

            refreshMappedItemInformation();
        }

        private void generateNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GhNotesItem source = null;
            GhNotesItem ghi = null;
            bool skipped = false;
            bool skipped2 = false;

            foreach (ListViewItem li in lvwGh.SelectedItems)
            {
                if (li != null)
                {
                    ghi = _ghItems[li.Index];
                    source = _song.Notes.SourceGenerationItem(ghi.Type);
                    if (source != ghi)
                    {
                        if (!_song.Notes.GenerateNotes(ghi, true, false, false, false, false))
                            skipped2 = true;
                    }
                    else
                        skipped = true;
                }
            }
            refreshMappedItemInformation();

            if (skipped)
                MessageBox.Show(this, "1 or more items could not be generated because they are the generation source.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (skipped2)
                MessageBox.Show(this, "1 or more items could not be generated because there are no non-generated items to use as a source.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count != 0)
                _song.Notes.ReloadFile((NotesFile)lvwFiles.SelectedItems[0].Tag);
        }

        private void removeMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lvwGh.SelectedItems)
            {
                if (li != null)
                    _song.Notes.UnMapGhItem((GhNotesItem)li.Tag);
            }
        }

        private void removeAllMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GhNotesItem gi in _song.Notes.GhItems)
            {
                if (gi.IsMapped)
                    _song.Notes.UnMapGhItem(gi);
            }
        }

        private void useForFretsEtcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count != 0)
                _song.Notes.BaseFile = (NotesFile)lvwFiles.SelectedItems[0].Tag;
            setFileTickIcon();
        }

        private void setFileTickIcon()
        {
            foreach (ListViewItem li in lvwFiles.Items)
            {
                li.ImageIndex = li.Tag == _song.Notes.BaseFile ? 1 : 0;
            }
        }

        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count != 0)
            {
                NotesFile nf = (NotesFile)lvwFiles.SelectedItems[0].Tag;
                _song.Notes.RemoveFile(nf);

                foreach (ListViewItem li in lvwFiles.Items)
                {
                    FileInfo fi = new FileInfo(((NotesFile)li.Tag).Filename);
                    li.Text = string.Format("{0} ({1})", (li.Index + 1).ToString(), fi.Extension.Substring(1));
                }

                refreshMappedItemInformation();
            }
            setFileTickIcon();
        }

        private void lvwFiles_DoubleClick(object sender, EventArgs e)
        {
            viewNotesToolStripMenuItem_Click(sender, e);
        }

        private void viewNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedItems.Count != 0)
            {
                ensureAudio();

                NotesFile nf = (NotesFile)lvwFiles.SelectedItems[0].Tag;
                List<string> wavs = getWavs();

                NotesEditorForm frm = new NotesEditorForm();
                frm.Initialise(nf, nf, _song.Notes.HoPoMeasure, _song.Notes.Gh3SustainClipping, wavs.ToArray());
                frm.ShowDialog();
                _song.Notes.HoPoMeasure = frm.HoPoMeasure;
                frm.Dispose();
            }
        }


        private void lvwFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvwInput.Items.Clear();

            if (lvwFiles.SelectedIndices.Count != 0)
            {

                ListViewItem li;
                foreach (NotesFileItem nii in ((NotesFile)lvwFiles.SelectedItems[0].Tag).Items)
                {
                    li = new ListViewItem(nii.SourceName);
                    li.SubItems.Add(nii.ButtonsUsed.ToString());
                    li.SubItems.Add(nii.StarPowerCount.ToString());
                    li.SubItems.Add(nii.NotesCount.ToString());
                    li.SubItems.Add(nii.BattlePowerCount.ToString());
                    li.SubItems.Add((nii.FaceOffP1Count + nii.FaceOffP2Count).ToString());
                    if (nii.ButtonsUsed >= 3)
                        li.ImageIndex = nii.ButtonsUsed - 3;
                    else
                        li.ImageIndex = 0; //easy
                    li.Tag = nii;
                    lvwInput.Items.Add(li);
                }
            }
        }

        private void lvwInput_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item != null)
            {
                _notesDragUniqueId = ((NotesFileItem)((ListViewItem)e.Item).Tag).UniqueId;
                lvwInput.DoDragDrop(_DragText, DragDropEffects.Copy);
            }
        }

        private void lvwGh_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) && (string)e.Data.GetData(DataFormats.Text) == _DragText)
            {
                lvwGh.Focus();
                e.Effect = DragDropEffects.All;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void lvwGh_DragDrop(object sender, DragEventArgs e)
        {
            if ((string)e.Data.GetData(DataFormats.Text) == _DragText)
            {
                ListViewItem li = lvwGh.HitTest(lvwGh.PointToClient(new Point(e.X, e.Y))).Item;
                if (li != null)
                {
                    GhNotesItem ghi = _ghItems[li.Index];
                    _song.Notes.MapToGhItem(((NotesFile)lvwFiles.SelectedItems[0].Tag), _notesDragUniqueId, ghi, false);
                }
            }
        }

        private void lvwGh_DragOver(object sender, DragEventArgs e)
        {
            if ((string)e.Data.GetData(DataFormats.Text) == _DragText)
            {
                ListViewItem li = lvwGh.HitTest(lvwGh.PointToClient(new Point(e.X, e.Y))).Item;
                if (li != null)
                {
                    if (lvwGh.SelectedItems.Count != 0)
                    {
                        foreach (ListViewItem lvi in lvwGh.SelectedItems)
                            lvi.Selected = false;
                    }
                    li.Selected = true;
                }
            }
        }

        private void lvwGh_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwGh.SelectedItems.Count != 0)
            {
                GhNotesItem gi = (GhNotesItem)lvwGh.SelectedItems[0].Tag;

                bool mapped = false;
                foreach (GhNotesItem g in _ghItems)
                {
                    if (g.IsMapped)
                    {
                        mapped = true;
                        break;
                    }
                }

                removeMappingToolStripMenuItem.Enabled = mapped && gi != null && gi.IsMapped;
                viewMappedNotesToolStripMenuItem.Enabled = mapped && gi != null && gi.IsMapped;
                removeAllMappingToolStripMenuItem.Enabled = mapped;
                forceNoStarPowerToolStripMenuItem.Enabled = mapped;
                forceNoStarPowerToolStripMenuItem.Checked = _song.Notes.ForceNoStarPower;

                mnuGh.Show(lvwGh, e.Location);
            }
        }

        private void Notes_GhItemMapChanged(object source, GhItemMapChangedEventArgs e)
        {
            setGhItemInformation(e.FromFile, e.UniqueId, e.ToGhNotesItem);
        }

        private void btnSmartMap_Click(object sender, EventArgs e)
        {
            smartMap(false);
        }

        private void smartMap(bool replaceGeneratedItems)
        {
            _song.Notes.SmartMap(false);
            _song.Notes.GenerateItems(false, false, false, replaceGeneratedItems);

            refreshMappedItemInformation();
        }

        private void addGhItem(GhNotesItem item)
        {
            ListViewItem li = new ListViewItem(item.Name);
            li.ImageIndex = (int)(item.Difficulty);
            li.SubItems.Add("");
            li.SubItems.Add("");
            li.SubItems.Add("");
            li.SubItems.Add("");
            li.SubItems.Add("");
            li.SubItems.Add("");
            lvwGh.Items.Add(li);
        }

        private void refreshMappedItemInformation()
        {
            foreach (GhNotesItem ghi in _ghItems)
            {
                uint id = 0;
                if (ghi.IsMapped)
                {
                    bool gen = ghi.MappedFileItem.HasGeneratedNotes;
                    NotesFile nf = ghi.MappedFile;
                    id = ghi.MappedFileItem.UniqueId;
                }
                setGhItemInformation(ghi.MappedFile, id, ghi);
            }
        }

        private void setGhItemInformation(NotesFile file, uint uniqueId, GhNotesItem ghItem)
        {
            ListViewItem li = null;

            foreach (ListViewItem l in lvwGh.Items)
            {
                if (l.Text == ghItem.Name)
                {
                    li = l;
                    break;
                }
            }

            int fileNo = 0;
            for (int i = 0; i < lvwFiles.Items.Count; i++)
            {
                if (lvwFiles.Items[i].Tag == file)
                {
                    fileNo = i + 1;
                    break;
                }
            }

            if (li != null && ghItem != null)
            {
                if (ghItem.IsMapped)
                {
                    li.SubItems[1].Text = ghItem.MappedFileItem.ButtonsUsed.ToString();
                    li.SubItems[2].Text = string.Concat(ghItem.MappedFileItem.NotesCount.ToString(), ghItem.MappedFileItem.HasGeneratedNotes ? " *" : string.Empty);
                    if (_song.Notes.ForceNoStarPower)
                        li.SubItems[3].Text = "0 (Forced)";
                    else
                        li.SubItems[3].Text = string.Concat(ghItem.MappedFileItem.StarPowerCount.ToString(), ghItem.MappedFileItem.HasGeneratedStarPower ? " *" : string.Empty);
                    li.SubItems[4].Text = string.Concat(ghItem.MappedFileItem.BattlePowerCount.ToString(), ghItem.MappedFileItem.HasGeneratedBattlePower ? " *" : string.Empty);
                    li.SubItems[5].Text = string.Concat((ghItem.MappedFileItem.FaceOffP1Count + ghItem.MappedFileItem.FaceOffP2Count).ToString(), ghItem.MappedFileItem.HasGeneratedFaceOff ? " *" : string.Empty);

                    string fileName = string.Format("{0} ({1})", fileNo.ToString(), (new FileInfo(file.Filename)).Extension.Substring(1));

                    if (ghItem.MappedFileItem.HasGeneratedNotes)
                        li.SubItems[6].Text = string.Format("Generated from {0} : {1}", fileName, ghItem.MappedFileItem.SourceName);
                    else
                        li.SubItems[6].Text = string.Format("{0} : {1}", fileName, ghItem.MappedFileItem.SourceName);
                    li.Tag = ghItem;
                }
                else
                {
                    li.SubItems[1].Text = string.Empty;
                    li.SubItems[2].Text = string.Empty;
                    li.SubItems[3].Text = string.Empty;
                    li.SubItems[4].Text = string.Empty;
                    li.SubItems[5].Text = string.Empty;
                    li.SubItems[6].Text = string.Empty;
                    li.Tag = null;
                }
            }
        }

        private Project _project;
        private ProjectSong _song;
        private uint _notesDragUniqueId;
        //private List<NotesFile> _srcFiles;  //notes from input files (charts etc)
        private GhNotesItem[] _ghItems;  //guitar hero notes
        private const string _DragText = "TheGHOST:NotesCopy";


    }
}
