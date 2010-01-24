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
    public partial class ProjectScreen : ScreenBase
    {
        public ProjectScreen()
        {
            InitializeComponent();

            cboGame.Items.Clear();
            cboGame.Items.AddRange(GameInfo.GameStrings);

            cboLanguage.Items.Clear();
            cboLanguage.Items.AddRange(GameInfo.LanguageStrings);

        }

        private void ProjectScreen_Load(object sender, EventArgs e)
        {
        }

        private void project_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void project_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
                ((TextBox)sender).Text = s[0];

            if (sender == txtOpenProject)
                loadProject();
        }

        private void cboSourceFilePlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSourceFilePluginMessage.Text = _plugins.FileCopyPlugins[cboSourceFilePlugin.Text].Description;
        }

        private void cboAudioExportPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAudioExportPluginMessage.Text = _plugins.AudioExportPlugins[cboAudioExportPlugin.Text].Description;
        }

        private void cboAudioImportPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAudioImportPluginMessage.Text = _plugins.AudioImportPlugins[cboAudioImportPlugin.Text].Description;
        }

        private void loadProject()
        {
            try
            {
                _project.LoadProject(txtOpenProject.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Failed to load project file and apply settings:{0}{0}{1}", Environment.NewLine, ex.Message), "Project Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            txtWorkingFolder.Text = _project.GetWorkingPath(WorkingFileType.Root);
            txtSourcePath.Text = _project.SourcePath;
            cboGame.Text = GameInfo.GameEnumToString(_project.GameId);
            chkStoreFiles.Checked = _project.StoreProjectFiles;

            cboSourceFilePlugin.Text = _project.FileCopyPluginInfo == null ? string.Empty : _project.FileCopyPluginInfo.Title;
            cboAudioExportPlugin.Text = _project.AudioExportInfo == null ? string.Empty : _project.AudioExportInfo.Title;
            cboAudioImportPlugin.Text = _project.AudioImportInfo == null ? string.Empty : _project.AudioImportInfo.Title;
            cboLanguage.Text = GameInfo.LanguageIdToString(_project.LanguageId);

            //lstEditorPlugins.Text = _project.EditorPluginNames;
        }

        private PluginInfo[] valuesToArray(Dictionary<string, PluginInfo> values)
        {
            PluginInfo[] ppi = new PluginInfo[values.Count];
            int i = 0;
            foreach (PluginInfo p in values.Values)
                ppi[i++] = p;
            return ppi;
        }

        #region ScreenBase Members

        public override string TitleMessage()
        {
            return "The Game Location may be an ISO file or an installation folder.\r\nThe working folder my have a enough space to hold uncompressed audio etc.";
        }

        public override void Construct(Project project)
        {
            _project = project;
            _defaults = new ProjectDefaultsProperties(_project.Defaults);
            pgdDefaultSettings.SelectedObject = _defaults;
        }

        public override void Open()
        {

            cboGame.SelectedIndex = 0;

            foreach (PluginInfo p in _plugins.AudioExportPlugins.Values)
            {
                cboAudioExportPlugin.Items.Add(p.Title);
            }

            foreach (PluginInfo p in _plugins.AudioImportPlugins.Values)
            {
                cboAudioImportPlugin.Items.Add(p.Title);
            }

            foreach (PluginInfo p in _plugins.FileCopyPlugins.Values)
            {
                cboSourceFilePlugin.Items.Add(p.Title);
            }

            txtWorkingFolder.Text = _project.GetWorkingPath(WorkingFileType.Root);
            cboGame.Text = GameInfo.GameEnumToString(_project.Defaults.GameId);
            cboLanguage.Text = GameInfo.LanguageIdToString(_project.Defaults.LanguageId);
            chkStoreFiles.Checked = _project.StoreProjectFiles;

            if (_project.Defaults.FileCopyPlugin.Length != 0 && cboSourceFilePlugin.Items.Contains(_project.Defaults.FileCopyPlugin))
                cboSourceFilePlugin.Text = _project.Defaults.FileCopyPlugin;
            if (_project.Defaults.AudioExportPlugin.Length != 0 && cboAudioExportPlugin.Items.Contains(_project.Defaults.AudioExportPlugin))
                cboAudioExportPlugin.Text = _project.Defaults.AudioExportPlugin;
            if (_project.Defaults.AudioImportPlugin.Length != 0 && cboAudioImportPlugin.Items.Contains(_project.Defaults.AudioImportPlugin))
                cboAudioImportPlugin.Text = _project.Defaults.AudioImportPlugin;

            if (_project.Filename.Length != 0)
            {
                txtOpenProject.Text = _project.Filename;
                if (File.Exists(_project.Filename)) //if exists then load, else then will be created
                    loadProject();
            }

        }

        public PluginManager PluginManager
        {
            get { return _plugins; }
            set { _plugins = value; }
        }

        public override bool Close(bool appQuiting)
        {
            if (!appQuiting)
            {

                if (txtOpenProject.Text.Length == 0)
                {
                    MessageBox.Show(this, "The Project can not blank", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else if (txtWorkingFolder.Text.Length == 0 || !Directory.Exists(txtWorkingFolder.Text))
                {
                    MessageBox.Show(this, "The Working Folder does not exist", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else if (txtSourcePath.Text.Length == 0 || !(Directory.Exists(txtSourcePath.Text) || File.Exists(txtSourcePath.Text)))
                {
                    MessageBox.Show(this, "The Game Location does not exist", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                _project.WorkingPath = txtWorkingFolder.Text;
                _project.SourcePath = txtSourcePath.Text;
                _project.GameId = GameInfo.GameStringToEnum(cboGame.Text);
                _project.StoreProjectFiles = chkStoreFiles.Checked;
                _project.SetDefaults(_defaults.Base);

                if (!_plugins.AudioImportPlugins.ContainsKey(cboAudioImportPlugin.Text)
                 || !_plugins.AudioExportPlugins.ContainsKey(cboAudioExportPlugin.Text)
                 || !_plugins.FileCopyPlugins.ContainsKey(cboSourceFilePlugin.Text))
                {
                    MessageBox.Show(this, "One or more plugins are missing or not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }


                _project.SetPlugins(_plugins.AudioImportPlugins[cboAudioImportPlugin.Text],
                                    _plugins.AudioExportPlugins[cboAudioExportPlugin.Text],
                                    _plugins.FileCopyPlugins[cboSourceFilePlugin.Text]);  //no editor plugins

                _project.LanguageId = GameInfo.LanguageStringToId(cboLanguage.Text);

                _project.Defaults.ReapplyAll = chkReapplyAll.Checked;

                try
                {
                    base.MessageShow("Extracting Core Files...");
                    _project.ReadCoreFiles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("An error occured reading the core files.{0}Ensure you have the correct game source and language selected{0}{0}{1}", Environment.NewLine, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                finally
                {
                    base.MessageHide();
                }


            }

            return true;
        }

        #endregion

        private Project _project;
        private PluginManager _plugins;
        private ProjectDefaultsProperties _defaults;

    }
}
