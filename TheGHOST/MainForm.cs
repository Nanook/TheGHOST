using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using Nanook.QueenBee.Parser;


namespace Nanook.TheGhost
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _screens = new List<ScreenBase>();
            _currScreenIdx = -1;
            _core = new TheGhostCore();

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            loadConfiguration(_core.Project.Defaults);
            this.Visible = false;

            ProjectForm frm = new ProjectForm();
            frm.Construct(_core);
            frm.ShowDialog();
            _projectFilename = frm.Project;

            if (frm.DialogResult == DialogResult.Cancel)
            {
                frm.Close();
                _core.Project.Filename = string.Empty;
                _exitApp = true;
            }
            else
            {
                frm.Close();
                _core.Project.Filename = _projectFilename;
                _core.Project.StoreProjectFiles = frm.StoreProjectFiles;
                this.Visible = true;
                _exitApp = false;
            }

            projectScreen.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
        }

        void screen_ScreenBusy(object source, ScreenBusyEventArgs e)
        {
            if (e.Busy)
            {
                if (btnMove.Tag == null)
                {
                    btnMoveNext.Tag = btnMoveNext.Enabled;
                    btnMoveBack.Tag = btnMoveBack.Enabled;
                    btnMove.Tag = btnMove.Enabled;

                    btnMoveNext.Enabled = false;
                    btnMoveBack.Enabled = false;
                    btnMove.Enabled = false;
                }
            }
            else
            {
                btnMoveNext.Enabled = (bool)btnMoveNext.Tag;
                btnMoveBack.Enabled = (bool)btnMoveBack.Tag;
                btnMove.Enabled = (bool)btnMove.Tag;

                btnMoveNext.Tag = null;
                btnMoveBack.Tag = null;
                btnMove.Tag = null;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (_currScreen != null) //null when quitting from the project screen
            {
                if (!Program.ForceClose)
                {
                    //code to test and prompt to user
                    switch (saveProjectPrompt())
                    {
                        case DialogResult.Yes:
                            ((ScreenBase)_currScreen).Close(true);
                            saveProject();
                            break;
                        case DialogResult.No:
                            ((ScreenBase)_currScreen).Close(true);
                            break;
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;
                    }
                }

                try
                {
                    saveConfiguration(_core.Project.Defaults);
                }
                catch
                {
                }

                try
                {
                    _core.Project.FileManager.RemoveAndDeleteAllFiles();
                    _core.Project.FileManager.DeleteFilesInWorkingFolders();
                }
                catch
                {
                }

            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_exitApp)
            {
                this.Close();
            }
            else
            {
                this.Text = string.Format("{0} - v{1} / v{2}", TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion);

                btnMoveBack.Enabled = false;
                btnMove.Enabled = false;
                _currScreen = (ScreenBase)projectScreen;
                projectScreen.PluginManager = _core.PluginManager;
                lblScreenMessage.Text = _currScreen.TitleMessage();
                ((UserControl)_currScreen).Visible = true;
                _currScreen.Construct(_core.Project);
                _currScreen.Open();
            }
        }

        private void showScreen(ScreenBase screen)
        {
            //set the active usercontrol
            UserControl src = (UserControl)_currScreen;

            UserControl uc = screen as UserControl;
            if (uc != null)
            {
                //copy the properties
                uc.Location = src.Location;
                uc.Size = src.Size;
                uc.Anchor = src.Anchor;

                this.Controls.Add(uc);
                lblScreenMessage.Text = screen.TitleMessage();

                uc.Visible = true;
                uc.BringToFront();
                _currScreen = screen;
            }

            //remove any other screens
            UserControl c;
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                c = this.Controls[i] as UserControl;
                if ((uc == null || c != uc) && c is ScreenBase)
                    this.Controls.Remove(c);
            }

            btnMoveNext.Enabled = (_currScreen is ModsScreen || _currScreen is TrackSelectScreen || _currScreenIdx < _screens.Count - 1);
            btnMoveBack.Enabled = (_currScreen is TrackSelectScreen || _currScreenIdx > 0);
            btnMove.Enabled = (_currScreen is TrackEditScreen || _currScreen is NotesEditScreen || _currScreen is ProgressScreen);

            if (_currScreen != null)
            {
                _currScreen.Construct(_core.Project);
                _currScreen.Open();
            }

            Application.DoEvents();
        }

        private void removeEventHandlers(ToolStripItem item)
        {
            if (item is ToolStripMenuItem)
            {
                ToolStripMenuItem x = (ToolStripMenuItem)item;
                x.Click -= new EventHandler(mnuMoveItemSong_Click);

                foreach (ToolStripMenuItem mi in x.DropDown.Items)
                    removeEventHandlers(mi);
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            #region create menu
            ToolStripMenuItem miP = null;
            string tierName;

            foreach (ToolStripItem mi in mnuMove.Items)
                removeEventHandlers(mi);
            mnuMove.Items.Clear();

            foreach (ProjectTier tier in _core.Project.Tiers)
            {
                foreach (ProjectTierSong pts in tier.Songs)
                {
                    if (pts.IsEditSong)
                    {
                        if (miP == null)
                        {
                            if (tier.Type == TierType.Career)
                                tierName = tier.Name;
                            else if (tier.Type == TierType.Bonus)
                                tierName = _BonusTheGhost;
                            else if (tier.Type == TierType.NonCareer)
                                tierName = _NonCareerSongs;
                            else
                                tierName = string.Empty;

                            miP = new ToolStripMenuItem(tierName);
                            mnuMove.Items.Add(miP);
                        }
                        addSongToMenu(miP, pts);
                    }
                }
                miP = null; //new tier
            }

            mnuMove.Items.Add("-");
            addScreenToMenu(mnuMove, _progressScreen, "End Track Editing");

            #endregion

            mnuMove.Show(btnMove, new Point(btnMove.Width / 2, btnMove.Height / 2));
        }

        private void addSongToMenu(ToolStripMenuItem mi, ProjectTierSong pts)
        {
            ToolStripMenuItem m = new ToolStripMenuItem(string.Format("{2}. {0} - {1}", pts.Song.Artist, pts.Song.Title, pts.Number.ToString()));
            m.Tag = pts.Song;
            m.Click += new EventHandler(mnuMoveItemSong_Click);
            mi.DropDown.Items.Add(m);
        }

        private void addScreenToMenu(ContextMenuStrip mnuStrip, ScreenBase screen, string title)
        {
            ToolStripMenuItem m = new ToolStripMenuItem(title);
            m.Tag = screen;
            m.Click += new EventHandler(mnuMoveItemScreen_Click);
            mnuStrip.Items.Add(m);
        }

        private void mnuMoveItemScreen_Click(object sender, EventArgs e)
        {
            ScreenBase scrn = (ScreenBase)((ToolStripMenuItem)sender).Tag;
            int idx = 0;
            foreach (ScreenBase sb in _screens)
            {
                if (sb == scrn)
                {
                    if (_currScreen.Close(false))
                    {
                        _currScreenIdx = idx;
                        showScreen(sb);
                    }
                    return;
                }
                idx++;
            }
        }

        private void mnuMoveItemSong_Click(object sender, EventArgs e)
        {
            ProjectSong ps = (ProjectSong)((ToolStripMenuItem)sender).Tag;

            if (ps == null)
                return;

            int idx = 0;
            foreach (ScreenBase sb in _screens)
            {
                //if (((ProjectSong)sb.Tag).SongQb.Id.Crc == ps.SongQb.Id.Crc)
                if (sb.Tag == ps)
                {
                    if (_currScreen.Close(false))
                    {
                        _currScreenIdx = idx;
                        showScreen(sb);
                    }
                    return;
                }
                idx++;
            }
        }

        private ProjectSong songExistsInEditSongs(uint crc)
        {
            foreach (ProjectSong ps in _core.Project.EditSongs)
            {
                if (ps.SongQb.Id.Crc == crc)
                    return ps;
            }
            return null;
        }

        private void btnMoveBack_Click(object sender, EventArgs e)
        {
            if (_currScreen == null)
                return;

            //prevent back bein pressed while already processing
            if (_processing)
                return;
            try
            {
                _processing = true;

                if (_currScreen is TrackSelectScreen)
                {
                    //create and init plugin screens
                    _modsScreen = (ScreenBase)(new ModsScreen());
                    _modsScreen.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
                    _modsScreen.Construct(_core.Project);

                    showScreen(_modsScreen);
                }
                else if (_currScreenIdx > 0)
                {
                    if (_currScreen.Close(false))
                    {
                        _currScreenIdx--;

                        if (_currScreenIdx < _screens.Count)
                            showScreen(_screens[_currScreenIdx]);
                        else
                        {
                            _currScreen = null;
                            showScreen(null);
                        }
                    }
                }
            }
            finally
            {
                _processing = false;
            }

        }

        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            if (_currScreen == null)
                return;

            //prevent next bein pressed while already processing
            if (_processing)
                return;
            try
            {
                _processing = true;

                if (_currScreen.Close(false))
                {

                    if (_currScreen is ProjectScreen)
                    {
                        //create and init plugin screens
                        _modsScreen = (ScreenBase)(new ModsScreen());
                        _modsScreen.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
                        _modsScreen.Construct(_core.Project);

                        showScreen(_modsScreen);
                    }
                    else if (_currScreen is ModsScreen)
                    {
                        //create and init plugin screens
                        _trackSelect = (ScreenBase)(new TrackSelectScreen());
                        _trackSelect.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
                        _trackSelect.Construct(_core.Project);

                        showScreen(_trackSelect);
                    }
                    else if (_currScreen is TrackSelectScreen)
                    {
                        ScreenBase sb;
                        foreach (ProjectSong song in _core.Project.EditSongs)
                        {
                            sb = new TrackEditScreen();
                            sb.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
                            sb.Construct(_core.Project);
                            ((TrackEditScreen)sb).SetSong(song);
                            sb.Tag = song;
                            _screens.Add(sb);

                            sb = new NotesEditScreen();
                            sb.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
                            sb.Construct(_core.Project);
                            ((NotesEditScreen)sb).SetSong(song);
                            sb.Tag = song;
                            _screens.Add(sb);
                        }

                        _progressScreen = new ProgressScreen();
                        _progressScreen.ScreenBusy += new ScreenBusyEventHandler(screen_ScreenBusy);
                        //not happy about this...
                        ((ProgressScreen)_progressScreen).AutoStartOnOpen = ((TrackSelectScreen)_trackSelect).AutoStartOnOpen;
                        _screens.Add(_progressScreen);

                        if (_screens.Count != 0)
                        {
                            //extractSongFiles();

                            _currScreenIdx = 0; //active...
                            showScreen(_screens[_currScreenIdx]);
                            btnMoveBack.Visible = true;
                        }
                    }
                    else
                    {
                        _currScreenIdx++;

                        if (_currScreenIdx < _screens.Count)
                            showScreen(_screens[_currScreenIdx]);
                        else
                        {
                            _currScreen = null;
                            showScreen(null);
                        }
                    }
                }
            }
            finally
            {
                _processing = false;
            }
        }





        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Yes=Save, No=Don't Save, Cancel=Don't quit</returns>
        private DialogResult saveProjectPrompt()
        {
            if (!(_currScreen is ProjectScreen))
                return MessageBox.Show(this, "Do you want to save this project?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            else
            {

                if (MessageBox.Show(this, "Do you want to exit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    return DialogResult.No;
                else
                    return DialogResult.Cancel;
            }
        }

        private void saveProject()
        {
            try
            {
                if (_core.Project.Filename == null || _core.Project.Filename.Length == 0)
                {
                    save.Filter = "The GHOST Project (*.tgp)|*.tgp|All files (*.*)|*.*";
                    save.InitialDirectory = _core.Project.GetWorkingPath(WorkingFileType.Root);
                    save.FileName = "project.tgp";
                    save.CheckPathExists = true;
                    save.OverwritePrompt = true;

                    if (save.ShowDialog(this) != DialogResult.Cancel)
                    {
                        if (_currScreen != null)
                            ((ScreenBase)_currScreen).MessageShow("Saving Project...");
                        _core.Project.SaveAs(save.FileName);
                    }
                }
                else
                {
                    if (_currScreen != null)
                        ((ScreenBase)_currScreen).MessageShow("Saving Project...");
                    _core.Project.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Failed to save project file:{0}{0}{1}", Environment.NewLine, ex.Message), "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (_currScreen != null)
                    ((ScreenBase)_currScreen).MessageHide();
            }
        }

        #region Configuration
        private void loadConfiguration(ProjectDefaults defaults)
        {
            try
            {
                Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                int v = int.Parse(loadSetting(c, "ConfigVersion", "1"));
                loadWindowInfo(this, loadSetting(c, "WindowInfo"));
                NotesEditorForm.WindowInfo = loadSetting(c, "NotesEditorWindowInfo");
                defaults.WorkingFolder = loadSetting(c, "WorkingFolder");
                defaults.HoPoMeasure = float.Parse(loadSetting(c, "HammerOnMeasure"));
                defaults.GuitarVolume = float.Parse(loadSetting(c, "GuitarVolume"));
                defaults.GuitarVolumeMode = (DefaultSettingMode)Enum.Parse(typeof(DefaultSettingMode), loadSetting(c, "GuitarVolumeMode"));
                defaults.MinMsBeforeNotesStart = int.Parse(loadSetting(c, "MinMsBeforeNotesStart"));
                defaults.SmartModeCrowdImport = (loadSetting(c, "SmartModeCrowdImport") == "True");
                defaults.ForceMono = (loadSetting(c, "ForceMono", "False") == "True");
                defaults.ForceDownSample = int.Parse(loadSetting(c, "ForceDownSample", "0"));
                defaults.PreviewFadeLength = int.Parse(loadSetting(c, "PreviewFadeLength"));
                defaults.PreviewIncludeGuitar = (loadSetting(c, "PreviewIncludeGuitar") == "True");
                defaults.PreviewIncludeRhythm = (loadSetting(c, "PreviewIncludeRhythm") == "True");
                defaults.PreviewLength = int.Parse(loadSetting(c, "PreviewLength"));
                defaults.PreviewStart = int.Parse(loadSetting(c, "PreviewStart"));
                defaults.PreviewVolume = int.Parse(loadSetting(c, "PreviewVolume"));
                defaults.AudioSongVolume = int.Parse(loadSetting(c, "AudioSongVolume", defaults.AudioSongVolume.ToString()));
                defaults.AudioGuitarVolume = int.Parse(loadSetting(c, "AudioGuitarVolume", defaults.AudioGuitarVolume.ToString()));
                defaults.AudioRhythmVolume = int.Parse(loadSetting(c, "AudioRhythmVolume", defaults.AudioRhythmVolume.ToString()));
                defaults.Singer = (Singer)Enum.Parse(typeof(Singer), loadSetting(c, "Singer"));
                defaults.SingerMode = (DefaultSettingMode)Enum.Parse(typeof(DefaultSettingMode), loadSetting(c, "SingerMode"));
                defaults.SongVolume = float.Parse(loadSetting(c, "SongVolume"));
                defaults.SongVolumeMode = (DefaultSettingMode)Enum.Parse(typeof(DefaultSettingMode), loadSetting(c, "SongVolumeMode"));
                defaults.Year = loadSetting(c, "Year");
                defaults.YearMode = (DefaultSettingModeBlank)Enum.Parse(typeof(DefaultSettingModeBlank), loadSetting(c, "YearMode"));
                defaults.Gh3SustainClipping = (loadSetting(c, "Gh3SustainClipping") == defaults.Gh3SustainClipping.ToString());
                defaults.ForceNoStarPower = (loadSetting(c, "ForceNoStarPower") == "False");

                defaults.GameId = (Game)Enum.Parse(typeof(Game), loadSetting(c, "GameId"));
                defaults.LanguageId = loadSetting(c, "LanguageId");

                try
                {
                    string aiName = loadSetting(c, "AudioExportPlugin", string.Empty);
                    if (_core.PluginManager.AudioExportPlugins.ContainsKey(aiName))
                        defaults.AudioExportPlugin = _core.PluginManager.AudioExportPlugins[aiName].Title;

                }
                catch
                {
                }
                try
                {
                    string aoName = loadSetting(c, "AudioImportPlugin", string.Empty);
                    if (_core.PluginManager.AudioImportPlugins.ContainsKey(aoName))
                        defaults.AudioImportPlugin = _core.PluginManager.AudioImportPlugins[aoName].Title;
                }
                catch
                {
                }
                try
                {
                    string exName = loadSetting(c, "FileCopyPlugin", string.Empty);
                    if (_core.PluginManager.FileCopyPlugins.ContainsKey(exName))
                        defaults.FileCopyPlugin = _core.PluginManager.FileCopyPlugins[exName].Title;

                }
                catch
                {
                }

                string prj;
                for (int i = 0; i < 10; i++)
                {
                    prj = loadSetting(c, string.Format("ProjectHistory{0}", i.ToString().PadLeft(2, '0')));
                    if (prj.Length != 0 && File.Exists(prj))
                        defaults.ProjectHistory.Add(prj);
                }


            }
            catch
            {
                //showException("Load Configuration Error", ex);
            }
        }

        private void saveConfiguration(ProjectDefaults defaults)
        {
            try
            {
                Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //always back this up. File paths only backed up when load is used
                //AppState.Backup = chkBackup.Checked;

                saveSetting(c, "ConfigVersion", "1");

                //saveSetting(c, "InputFormat", AppState.InputFormat);
                saveWindowInfo(this);
                saveSetting(c, "WindowInfo", _windowInfo);
                saveSetting(c, "NotesEditorWindowInfo", NotesEditorForm.WindowInfo);
                saveSetting(c, "WorkingFolder", defaults.WorkingFolder);
                saveSetting(c, "HammerOnMeasure", defaults.HoPoMeasure.ToString());
                saveSetting(c, "GuitarVolume", defaults.GuitarVolume.ToString());
                saveSetting(c, "GuitarVolumeMode", defaults.GuitarVolumeMode.ToString());
                saveSetting(c, "MinMsBeforeNotesStart", defaults.MinMsBeforeNotesStart.ToString());
                saveSetting(c, "SmartModeCrowdImport", defaults.SmartModeCrowdImport.ToString());
                saveSetting(c, "ForceMono", defaults.ForceMono.ToString());
                saveSetting(c, "ForceDownSample", defaults.ForceDownSample.ToString());
                saveSetting(c, "PreviewFadeLength", defaults.PreviewFadeLength.ToString());
                saveSetting(c, "PreviewIncludeGuitar", defaults.PreviewIncludeGuitar.ToString());
                saveSetting(c, "PreviewIncludeRhythm", defaults.PreviewIncludeRhythm.ToString());
                saveSetting(c, "PreviewLength", defaults.PreviewLength.ToString());
                saveSetting(c, "PreviewStart", defaults.PreviewStart.ToString());
                saveSetting(c, "PreviewVolume", defaults.PreviewVolume.ToString());
                saveSetting(c, "AudioSongVolume", defaults.AudioSongVolume.ToString());
                saveSetting(c, "AudioGuitarVolume", defaults.AudioGuitarVolume.ToString());
                saveSetting(c, "AudioRhythmVolume", defaults.AudioRhythmVolume.ToString());
                saveSetting(c, "Singer", defaults.Singer.ToString());
                saveSetting(c, "SingerMode", defaults.SingerMode.ToString());
                saveSetting(c, "SongVolume", defaults.SongVolume.ToString());
                saveSetting(c, "SongVolumeMode", defaults.SongVolumeMode.ToString());
                saveSetting(c, "Year", defaults.Year.ToString());
                saveSetting(c, "YearMode", defaults.YearMode.ToString());
                saveSetting(c, "Gh3SustainClipping", defaults.Gh3SustainClipping.ToString());
                saveSetting(c, "ForceNoStarPower", defaults.ForceNoStarPower.ToString());

                saveSetting(c, "GameId", _core.Project.GameId.ToString());
                saveSetting(c, "LanguageId", _core.Project.LanguageId);

                saveSetting(c, "AudioExportPlugin", _core.Project.AudioExportInfo.Title);
                saveSetting(c, "AudioImportPlugin", _core.Project.AudioImportInfo.Title);
                saveSetting(c, "FileCopyPlugin", _core.Project.FileCopyPluginInfo.Title);

                string currPrj = _core.Project.Filename;
                string key;
                int j = 0;
                for (int i = 0; i < 10; i++)
                {
                    key = string.Format("ProjectHistory{0}", i.ToString().PadLeft(2, '0'));
                    if (i == 0 && File.Exists(currPrj))
                        saveSetting(c, key, currPrj); //always save the current project as the top item
                    else
                    {
                        if (defaults.ProjectHistory.Count > j &&  defaults.ProjectHistory[j].ToLower() != currPrj.ToLower() && File.Exists(defaults.ProjectHistory[j]))
                            saveSetting(c, key, defaults.ProjectHistory[j]); //always save the current project as the top item
                        else
                            saveSetting(c, key, string.Empty); //always save the current project as the top item
                        j++;
                    }
                }

                c.Save();
            }
            catch
            {
                //showException("Save Configuration Error", ex);
            }
        }

        private void saveWindowInfo(Form win)
        {
            if (win.WindowState == FormWindowState.Normal)
                _windowInfo = string.Format("{0},{1},{2},{3},{4}", win.Location.X, win.Location.Y, win.Size.Width, win.Size.Height, (int)(win.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : win.WindowState));
            else
            {
                string[] s = _windowInfo.Split(',');
                s[4] = ((int)(win.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : win.WindowState)).ToString();
                _windowInfo = string.Join(",", s);
            }
        }

        private  void loadWindowInfo(Form win, string settings)
        {
            if (settings.Length != 0)
            {
                _windowInfo = settings;
                string[] wi = settings.Split(',');
                win.Location = new Point(int.Parse(wi[0]), int.Parse(wi[1]));
                win.Size = new Size(int.Parse(wi[2]), int.Parse(wi[3]));
                win.WindowState = (FormWindowState)int.Parse(wi[4]);
                win.Refresh();
            }
            else
            {
                win.WindowState = FormWindowState.Normal;
                win.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - win.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - win.Height) / 2);
            }
        }


        private string loadSetting(Configuration c, string item)
        {
            return loadSetting(c, item, string.Empty);
        }

        private string loadSetting(Configuration c, string item, string defaultItem)
        {
            KeyValueConfigurationElement kvce;
            kvce = c.AppSettings.Settings[item];
            if (kvce != null)
                return kvce.Value;
            else
                return defaultItem;

        }

        private void saveSetting(Configuration c, string name, string value)
        {
            if (c.AppSettings.Settings[name] == null)
                c.AppSettings.Settings.Add(name, value);
            else
                c.AppSettings.Settings[name].Value = value;
        }

        private void EditorForm_Resize_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                saveWindowInfo(this);
        }

        #endregion

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm frm = new AboutForm();
            frm.SetVersions(TheGhostCore.AppVersion, TheGhostCore.CoreVersion);

            frm.ShowDialog(this);
            frm.Close();
            frm = null;
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
            catch(Exception ex)
            {
                if (ex.Message == "No application is associated with the specified file for this operation")
                    MessageBox.Show(this, "No application is associated with PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (ex.Message == "The system cannot find the file specified")
                    MessageBox.Show(this, string.Format(@"File not found '{0}\TheGHOST-Guide.pdf'.", path), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show(this, "An error ocurred while attempting to display the PDF.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool _processing;

        private TheGhostCore _core;
        private ScreenBase _currScreen;
        private ScreenBase _trackSelect;
        private ScreenBase _modsScreen;
        private ScreenBase _progressScreen;

        private List<ScreenBase> _screens;
        private int _currScreenIdx;  //index within _screens (-minus 1 means project or track is active)

        private string _windowInfo;

        private bool _exitApp;
        private string _projectFilename;

        private const string _BonusTheGhost = "Bonus songs added with TheGHOST";
        private const string _NonCareerSongs = "Non career songs";

    }
}
