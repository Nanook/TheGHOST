using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public partial class TrackSelectScreen : ScreenBase
    {
        public TrackSelectScreen()
        {
            InitializeComponent();
            cboSelect.SelectedIndex = 4;
            _autoStartOnOpen = false;
            _backupSettings = new Dictionary<uint, ProjectSong>();
        }

        private void btnExportGameTitles_Click(object sender, EventArgs e)
        {
            export.Filter = "Text (*.txt)|*.txt|All files (*.*)|*.*";
            export.OverwritePrompt = true;

            if (export.ShowDialog(this) != DialogResult.Cancel)
                File.WriteAllText(export.FileName, _trackList.ToString());
        }

        #region ScreenBase Members

        public override void Construct(Project project)
        {
            _project = project;
        }

        public override void Open()
        {
            Dictionary<uint, ProjectTierSong> songs = _project.Songs;

            ListViewGroup lg;
            List<ProjectTier> tiers = _project.Tiers;

            _trackList = new StringBuilder();

            //back the original list up for the restore functionality
            foreach (ProjectTierSong pts in _project.Songs.Values)
            {
                if (pts.Song != null)
                    _backupSettings.Add(pts.Song.SongQb.Id.Crc, pts.Song);
            }

            string tierName;
            foreach (ProjectTier tier in tiers)
            {
                if (tier.Type == TierType.Career || tier.Type == TierType.Bonus)
                    tierName = tier.Name;
                else if (tier.Type == TierType.NonCareer)
                    tierName = _NonCareerSongs;
                else
                    tierName = string.Empty;

                cboSelect.Items.Add(tierName);
                addTrackListTierName(tierName);

                lg = lvwTracks.Groups.Add(tierName, tierName);

                foreach (ProjectTierSong song in tier.Songs)
                {
                    addSongToListView(lg, song);
                }
            }
        }

        private void addTrackListTierName(string name)
        {
            if (_trackList.Length != 0)
                _trackList.AppendLine();
            _trackList.AppendLine(name);
            _trackList.Append('-', name.Length);
            _trackList.AppendLine();
        }

        private ListViewItem addSongToListView(ListViewGroup lg, ProjectTierSong song)
        {
            //for GH3 only get songs that are s.Key.Crc == s.Checksum.Crc - this fixes an issue with cult of personality (talk talk)
            //uint id = _project.GameInfo.Game == Game.GH3_Wii ? song.Checksum.Crc : song.Key.Crc;

            TrackMap map = new TrackMap(song.SongQb, song);
            SongQb sqb = song.SongQb;

            ListViewItem li = new ListViewItem(string.Format("{0}   -   {1}{2}{3}{4}", sqb.Artist, sqb.Title, sqb.Year.Length == 0 ? string.Empty : "   (", sqb.Year, sqb.Year.Length == 0 ? string.Empty : ")"));
            _trackList.AppendLine(string.Format("{0} - {1}{2}{3}{4}", sqb.Artist, sqb.Title, sqb.Year.Length == 0 ? string.Empty : " (", sqb.Year, sqb.Year.Length == 0 ? string.Empty : ")"));
            li.SubItems.Add(string.Empty);
            li.SubItems.Add(sqb.Id.Text);
            li.Tag = map;

            setSavedSong(li, sqb, true);

            li.Group = lg;
            lvwTracks.Items.Add(li);

            if (song.IsMappingDisabled)
                disableSongItem(li);

            return li;
        }

        private void setSavedSong(ListViewItem li, SongQb song, bool blankIfNotFound)
        {
            ProjectSong ps = null;
            if (_backupSettings.ContainsKey(song.Id.Crc))
            {
                ps = _backupSettings[song.Id.Crc];
                ((TrackMap)li.Tag).TierSong.Song = ps;

                //li = new ListViewItem(string.Format("{0}   -   {1}{2}{3}{4}", ps.Artist, ps.Title, ps.Year.Length == 0 ? string.Empty : "   (", ps.Year, ps.Year.Length == 0 ? string.Empty : ")"));
                li.UseItemStyleForSubItems = false;
                li.SubItems[1].Text = string.Format("{0} - {1}{2}{3}{4}", ps.Artist, ps.Title, ps.Year.Length == 0 ? string.Empty : " (", ps.Year, ps.Year.Length == 0 ? string.Empty : ")");
                if (ps.AllFilesExist)
                    li.SubItems[1].ForeColor = Color.Black;
                else
                {
                    li.SubItems[1].ForeColor = Color.Red;
                    li.SubItems[1].Text = string.Concat(li.SubItems[1].Text, "  *");
                }
                li.SubItems[1].Font = li.Font;
                li.ImageIndex = (ps.LastChanged > ps.LastApplied || _project.Defaults.ReapplyAll) ? 2 : 1;
                li.Checked = false;
            }
            else if (blankIfNotFound)
            {
                li.UseItemStyleForSubItems = false;
                li.SubItems[1].Text = string.Empty;
                //li.SubItems[1].ForeColor = Color.Blue;
                li.SubItems[1].Font = new Font(li.Font, FontStyle.Bold);
                li.ImageIndex = 0;
                li.Checked = false;
            }
        }

        public override string TitleMessage()
        {
            return "Select the tracks to be replaced in this project.\r\nDrag and drop folders to Smart Map them.\r\nRight-Click to edit, drag to order folders.";
        }

        public bool AutoStartOnOpen
        {
            get { return _autoStartOnOpen; }
            set { _autoStartOnOpen = value; }
        }

        public override bool Close(bool appQuiting)
        {
            _trackList = null;

            if (!appQuiting)
            {
                List<TrackMap> trackMaps = new List<TrackMap>();
                List<DirectoryInfo> dirs = new List<DirectoryInfo>();

                int mappedSongs = 0;
                int changedSongs = 0;
                foreach (ListViewItem li in lvwTracks.Items)
                {
                    TrackMap map = (TrackMap)li.Tag;

                    map.TierSong.IsEditSong = li.Checked;

                    if (map.TierSong.IsEditSong || map.MappedDir != null)
                    {
                        if (map.TierSong.Song == null)
                        {
                            ProjectSong song = _project.CreateProjectSong(map.SongQb, true);

                            map.TierSong.Song = song;

                            if (map.MappedDir != null) //we have a folder mapped to the item
                            {
                                mappedSongs++;
                                trackMaps.Add(map);
                                dirs.Add(map.MappedDir);
                            }
                        }

                    }

                    if (map.TierSong.Song != null && (map.TierSong.Song.LastChanged > map.TierSong.Song.LastApplied || _project.Defaults.ReapplyAll))
                        changedSongs++;
                }

                //test for auto start
                if (_project.EditSongs.Length == 0 && (mappedSongs != 0 || changedSongs != 0))
                {
                    this.AutoStartOnOpen = (DialogResult.Yes == MessageBox.Show(this,
@"Would you like to apply the changes automatically after the audio has been imported?

No songs have been checked for editing and there are mapped folders to be imported.
Clicking Yes to this dialog will allow you to leave TheGHOST to do all the remaining
tasks without any further interaction.", "Auto Apply", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                }


                if (trackMaps.Count > 0)
                {
                    try
                    {
                        StringBuilder importError = new StringBuilder();
                        int failCount = 0;
                        for (int i = 0; i < trackMaps.Count; i++)
                        {
                            try
                            {
                                base.MessageShow(string.Format("Importing{3}{0}{3}({1} of {2})", dirs[i].Name, (i + 1).ToString(), trackMaps.Count.ToString(), Environment.NewLine));
                                trackMaps[i].TierSong.Song.AutoImportDirectory(dirs[i].FullName, _project.Defaults.SmartModeCrowdImport);
                                trackMaps[i].TierSong.Song.Notes.SmartMap(false);
                                trackMaps[i].TierSong.Song.Notes.GenerateItems(false, false, false, true);
                            }
                            catch
                            {
                                failCount++;
                                trackMaps[i].TierSong.Song = null;
                                if (failCount <= 10)
                                    importError.AppendLine(dirs[i].Name);
                                else if (failCount == 11)
                                    importError.AppendLine("...");
                            }
                        }

                        if (importError.Length != 0)
                        {
                            MessageBox.Show(this, string.Format("The following folders failed to import:{0}{0}{1}{0}Fix the errors to continue.", Environment.NewLine, importError.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    finally
                    {
                        base.MessageHide();
                    }
                }
            }

            return true;
        }

        #endregion


        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            selectItems(true);
        }


        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            selectItems(false);
        }

        private void selectItems(bool selected)
        {
            if (cboSelect.SelectedIndex == 0) //everything
            {
                foreach (ListViewItem li in lvwTracks.Items)
                    li.Checked = selected;
            }
            else if (cboSelect.SelectedIndex == 1) //Mapped To Folders
            {
                foreach (ListViewItem li in lvwTracks.Items)
                {
                    if (((TrackMap)li.Tag).MappedDir != null)
                        li.Checked = selected;
                }
            }
            else if (cboSelect.SelectedIndex == 2) //Unmapped Tracks
            {
                foreach (ListViewItem li in lvwTracks.Items)
                {
                    if (((TrackMap)li.Tag).MappedDir == null && ((TrackMap)li.Tag).TierSong.Song == null && !((TrackMap)li.Tag).TierSong.IsMappingDisabled)
                        li.Checked = selected;
                }
            }
            else if (cboSelect.SelectedIndex == 3) //Tracks with Errors
            {
                foreach (ListViewItem li in lvwTracks.Items)
                {
                    if (((TrackMap)li.Tag).TierSong.Song != null && !((TrackMap)li.Tag).TierSong.Song.AllFilesExist)
                        li.Checked = selected;
                }
            }
            else if (cboSelect.SelectedIndex == 4) //Changed Tracks
            {
                foreach (ListViewItem li in lvwTracks.Items)
                {
                    if ((((TrackMap)li.Tag).TierSong.Song != null && (((TrackMap)li.Tag).TierSong.Song.LastChanged > ((TrackMap)li.Tag).TierSong.Song.LastApplied || _project.Defaults.ReapplyAll) || (((TrackMap)li.Tag).MappedDir != null)))
                        li.Checked = selected;
                }
            }
            else if (cboSelect.SelectedIndex == 5) //Added With TheGHOST
            {
                foreach (ListViewItem li in lvwTracks.Items)
                {
                    if (((TrackMap)li.Tag).TierSong.IsAddedWithTheGhost)
                        li.Checked = selected;
                }
            }
            else if (cboSelect.SelectedIndex == 6) //all tiers
            {
                int count = 0;
                while (_project.Tiers[count++].Type == TierType.Career) ;
                if (count != 0)
                    count--;

                foreach (ListViewGroup lg in lvwTracks.Groups)
                {
                    if (count-- <= 0)
                        break;

                    foreach (ListViewItem li in lg.Items)
                        li.Checked = selected;
                }
            }
            else if (cboSelect.SelectedIndex == cboSelect.Items.Count - 2 && cboSelect.Text == _BonusTheGhost) //TheGHOST files
            {
                foreach (ListViewGroup lg in lvwTracks.Groups)
                {
                    if (lg.Name == _BonusTheGhost)
                    {
                        foreach (ListViewItem li in lg.Items)
                            li.Checked = selected;
                    }
                }
            }
            else if (cboSelect.SelectedIndex == cboSelect.Items.Count - 1 && cboSelect.Text == _NonCareerSongs) //extra files
            {
                foreach (ListViewGroup lg in lvwTracks.Groups)
                {
                    if (lg.Name == _NonCareerSongs)
                    {
                        foreach (ListViewItem li in lg.Items)
                            li.Checked = selected;
                    }
                }
            }
            else  //find listview group name and select items
            {
                foreach (ListViewGroup lg in lvwTracks.Groups)
                {
                    if (lg.Name == cboSelect.Text)
                    {
                        foreach (ListViewItem li in lg.Items)
                            li.Checked = selected;
                    }
                }
            }

        }


        private void lvwTracks_DragDrop(object sender, DragEventArgs e)
        {
            if ((string)e.Data.GetData(DataFormats.Text) == _DragText)
            {
                //item move
                //ListViewItem li = lvwTracks.HitTest(lvwTracks.PointToClient(new Point(e.X, e.Y))).Item;
                //if (_dragItem != null && li != null)
                //    moveItem(_dragItem.Index, li.Index);
            }
            else
            {
                //Folder drag and drop
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (s.Length > 0)
                {
                    List<DirectoryInfo> dirs = getFolders(s);
                    int idx = 0;
                    foreach (ListViewItem li in lvwTracks.Items)
                    {
                        if (idx >= dirs.Count)
                            break;

                        if (((TrackMap)li.Tag).TierSong.Song == null && ((TrackMap)li.Tag).MappedDir == null && !((TrackMap)li.Tag).TierSong.IsMappingDisabled) //no setting in the config file
                        {
                            li.SubItems[1].Text = dirs[idx].Name;
                            ((TrackMap)li.Tag).MappedDir = dirs[idx];
                            idx++;
                            li.ImageIndex = 2;
                        }
                    }
                }
            }
        }

        private List<DirectoryInfo> getFolders(string[] dirs)
        {
            List<DirectoryInfo> d = new List<DirectoryInfo>();
            foreach (string s in dirs)
            {
                if (Directory.Exists(s))
                    recurseFolders(new DirectoryInfo(s), d);
            }

            d.Sort(delegate(DirectoryInfo a, DirectoryInfo b)
                {
                    return string.Compare(a.FullName, b.FullName);
                });

            return d;
        }

        private void recurseFolders(DirectoryInfo d, List<DirectoryInfo> found)
        {
            foreach (DirectoryInfo di in d.GetDirectories())
                recurseFolders(di, found);

            if (d.GetFiles("*.wav").Length != 0 || d.GetFiles("*.flac").Length != 0 || d.GetFiles("*.ogg").Length != 0 || d.GetFiles("*.mp3").Length != 0)
                found.Add(d);
        }

        private static TrackMap enableSongItem(ListViewItem li)
        {
            TrackMap tm;
            tm = (TrackMap)li.Tag;
            if (tm.TierSong.IsMappingDisabled)
            {
                tm.TierSong.IsMappingDisabled = false;
                tm.MappedDir = null;
                li.SubItems[1].Text = string.Empty;
                li.SubItems[1].Font = new Font(li.SubItems[1].Font, FontStyle.Bold);
                li.SubItems[1].ForeColor = Color.Black;
                li.Checked = false;
                li.ImageIndex = 0;
            }
            return tm;
        }

        private static TrackMap disableSongItem(ListViewItem li)
        {
            TrackMap tm;
            tm = (TrackMap)li.Tag;
            if (tm.TierSong.Song == null)
            {
                tm.TierSong.IsMappingDisabled = true;
                tm.MappedDir = null;
                li.SubItems[1].Text = "--  Mapping Disabled  --";
                li.SubItems[1].Font = new Font(li.SubItems[1].Font, FontStyle.Italic);
                li.SubItems[1].ForeColor = Color.Gray;
                li.Checked = false;
                li.ImageIndex = 0;
            }
            return tm;
        }


        #region ListViewItem stuff
        private void lvwTracks_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text))
            {
                lvwTracks.Focus();
                e.Effect = DragDropEffects.All;
            }
            else
                e.Effect = DragDropEffects.None;

        }

        private void lvwTracks_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _dragItem = null;
            if (e.Item != null)
            {
                ListViewItem li = (ListViewItem)e.Item;

                if (((TrackMap)li.Tag).TierSong.Song == null && ((TrackMap)li.Tag).MappedDir != null && !((TrackMap)li.Tag).TierSong.IsMappingDisabled)
                {
                    _dragItem = (ListViewItem)e.Item;
                    lvwTracks.DoDragDrop(_DragText, DragDropEffects.Move);
                }
                else
                    lvwTracks.DoDragDrop(_DragText, DragDropEffects.None);
            }
        }

        private void lvwTracks_DragOver(object sender, DragEventArgs e)
        {
            if (_dragItem == null)
                return;

            if ((string)e.Data.GetData(DataFormats.Text) == _DragText)
            {
                ListViewItem li = lvwTracks.HitTest(lvwTracks.PointToClient(new Point(e.X, e.Y))).Item;

                if (li != null)
                {
                    if (lvwTracks.SelectedItems.Count != 0)
                    {
                        foreach (ListViewItem lvi in lvwTracks.SelectedItems)
                        {
                            if (li != lvi)
                                lvi.Selected = false;
                        }
                    }

                    li.Selected = true;

                    if (((TrackMap)li.Tag).TierSong.Song != null || ((TrackMap)li.Tag).TierSong.IsMappingDisabled)
                        e.Effect = DragDropEffects.None;
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                        if (_dragItem.Index != li.Index)
                        {
                            swapFolderItems(_dragItem.Index, li.Index);
                            _dragItem = li;
                        }

                        
                    }
                }
                else
                    e.Effect = DragDropEffects.None;

            }
        }


        private void moveItem(int fromIdx, int toIdx)
        {
            if (fromIdx == toIdx)
                return;

            int inc = fromIdx < toIdx ? 1 : -1;

            DirectoryInfo di = ((TrackMap)lvwTracks.Items[fromIdx].Tag).MappedDir;
            bool b = lvwTracks.Items[fromIdx].Checked;

            for (int i = fromIdx; i != toIdx; i += inc)
            {
                ((TrackMap)lvwTracks.Items[i].Tag).MappedDir = ((TrackMap)lvwTracks.Items[i + inc].Tag).MappedDir;
                lvwTracks.Items[i].SubItems[1].Text = lvwTracks.Items[i + inc].SubItems[1].Text;
                lvwTracks.Items[i].Checked = lvwTracks.Items[i + inc].Checked;
            }
            if (di != null)
            {
                ((TrackMap)lvwTracks.Items[toIdx].Tag).MappedDir = di;
                lvwTracks.Items[toIdx].Checked = b;
                lvwTracks.Items[toIdx].SubItems[1].Text = di.Name;
            }
        }

        private void swapFolderItems(int idxA, int idxB)
        {
            DirectoryInfo di = ((TrackMap)lvwTracks.Items[idxA].Tag).MappedDir;
            bool b = lvwTracks.Items[idxA].Checked;
            int img = lvwTracks.Items[idxA].ImageIndex;

            ((TrackMap)lvwTracks.Items[idxA].Tag).MappedDir = ((TrackMap)lvwTracks.Items[idxB].Tag).MappedDir;
            lvwTracks.Items[idxA].SubItems[1].Text = lvwTracks.Items[idxB].SubItems[1].Text;
            lvwTracks.Items[idxA].Checked = lvwTracks.Items[idxB].Checked;
            lvwTracks.Items[idxA].ImageIndex = lvwTracks.Items[idxB].ImageIndex;

            ((TrackMap)lvwTracks.Items[idxB].Tag).MappedDir = di;
            lvwTracks.Items[idxB].SubItems[1].Text = di.Name;
            lvwTracks.Items[idxB].Checked = b;
            lvwTracks.Items[idxB].ImageIndex = img;
        }

        private void lvwTracks_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwTracks.SelectedItems.Count != 0)
            {
                ListViewItem li = lvwTracks.HitTest(e.Location).Item;

                if (li != null)
                {
                    //TrackMap tm = (TrackMap)li.Tag;

                    mnu.Show(lvwTracks, e.Location);
                }
            }
        }

        private void lvwTracks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                if (((TrackMap)e.Item.Tag).TierSong.IsMappingDisabled)
                    e.Item.Checked = false;
            }
        }

        #endregion

        #region MenuCommands
        private void mnuRemoveFolder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lvwTracks.SelectedItems)
            {
                if (((TrackMap)li.Tag).TierSong.Song == null && ((TrackMap)li.Tag).MappedDir != null)
                {
                    li.Checked = false;
                    li.SubItems[1].Text = string.Empty;
                    ((TrackMap)li.Tag).MappedDir = null;
                    li.ImageIndex = 0;
                }
            }
        }

        private void mnuRemoveSavedSettings_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lvwTracks.SelectedItems)
            {
                ProjectSong ps = ((TrackMap)li.Tag).TierSong.Song;
                if (((TrackMap)li.Tag).SongQb != null && ps != null)
                {

                    li.Checked = false;
                    li.SubItems[1].Text = string.Empty;
                    li.SubItems[1].ForeColor = Color.Black;
                    ((TrackMap)li.Tag).TierSong.Song = null;
                    ((TrackMap)li.Tag).TierSong.IsEditSong = false;
                    li.SubItems[1].Font = new Font(li.SubItems[1].Font, FontStyle.Bold);
                    li.ImageIndex = 0;
                }
            }
        }

        private void mnuRestoreSavedSettings_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lvwTracks.SelectedItems)
                setSavedSong(li, ((TrackMap)li.Tag).SongQb, false);
        }

        private void mnuDisableMappings_Click(object sender, EventArgs e)
        {
            TrackMap tm;
            foreach (ListViewItem li in lvwTracks.SelectedItems)
                tm = disableSongItem(li);
        }

        private void mnuEnableMappings_Click(object sender, EventArgs e)
        {
            TrackMap tm;
            foreach (ListViewItem li in lvwTracks.SelectedItems)
                tm = enableSongItem(li);
        }

        private void mnuCheckItems_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lvwTracks.SelectedItems)
                li.Checked = true;
        }

        private void mnuUncheckItems_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lvwTracks.SelectedItems)
                li.Checked = false;
        }

        private void mnuSetToChanged_Click(object sender, EventArgs e)
        {
            TrackMap tm;
            foreach (ListViewItem li in lvwTracks.SelectedItems)
            {
                tm = (TrackMap)li.Tag;
                if (li.ImageIndex == 1 && tm.TierSong.Song != null) //green
                {
                    if (tm.TierSong.Song.LastApplied >= DateTime.Now)
                        tm.TierSong.Song.LastApplied = DateTime.Now.AddMinutes(-1);
                    tm.TierSong.Song.LastChanged = DateTime.Now;
                    li.ImageIndex = 2; //blue
                }
            }
        }

        #endregion

        private ListViewItem _dragItem;
        private const string _DragText = "TheGHOST:TrackMove";

        private Project _project;
        private const string _BonusTheGhost = "Bonus songs added with TheGHOST";
        private const string _NonCareerSongs = "Non career songs";

        private bool _autoStartOnOpen;

        private Dictionary<uint, ProjectSong> _backupSettings;

        private StringBuilder _trackList;


    }
}
