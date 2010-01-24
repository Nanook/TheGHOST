using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Nanook.TheGhost
{
    public partial class ProjectForm : Form
    {
        internal ProjectForm()
        {
            InitializeComponent();
            _project = string.Empty;
            _storeProjectFiles = false;
            this.DialogResult = DialogResult.Cancel;
        }

        internal void Construct(TheGhostCore core)
        {
            _core = core;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {

            if (rdoBrowse.Checked)
            {
                open.Title = "Open Project";
                open.Filter = "TheGHOST Project (*.tgp)|*.tgp|All files (*.*)|*.*";
                open.Multiselect = false;
                open.ValidateNames = true;

                if (open.ShowDialog(this) != DialogResult.Cancel)
                {
                    _project = open.FileName;
                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
            }
            else if (rdoCreate.Checked)
            {
                create.Title = "Create Project";
                create.Filter = "TheGHOST Project (*.tgp)|*.tgp|All files (*.*)|*.*";
                create.ValidateNames = true;
                create.OverwritePrompt = true;

                if (create.ShowDialog(this) != DialogResult.Cancel)
                {
                    _project = create.FileName;
                    _storeProjectFiles = DialogResult.Yes == MessageBox.Show(this,
@"Would you like to keep all added files with the project?

This will create a folder named the same as the project file that
together contain eveything required for a full project backup.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    this.DialogResult = DialogResult.OK;
                    this.Hide();

                    //remove the existing folder
                    try
                    {
                        if (File.Exists(create.FileName))
                        {
                            FileHelper.Delete(create.FileName);
                            string d = Path.ChangeExtension(create.FileName, string.Empty);
                            Directory.Delete(d, true);
                        }
                    }
                    catch
                    {
                    }

                }
            }
            else
            {
                if (File.Exists(cboRecent.Text))
                {
                    _project = cboRecent.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else
                    MessageBox.Show(this, string.Format("'{0}' does not exist.", cboRecent.Text), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - v{1} / v{2}", TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion);

            foreach (string s in _core.Project.Defaults.ProjectHistory)
            {
                cboRecent.Items.Add(s);
            }
            if (cboRecent.Items.Count == 0)
            {
                //rdoRecent.Enabled = false;
                cboRecent.Enabled = false;
            }
            else
            {
                cboRecent.SelectedIndex = 0;
                rdoRecent.Checked = true;
            }
        }

        private void rdoRecent_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void rdoRecent_MouseUp(object sender, MouseEventArgs e)
        {
            if (!cboRecent.Enabled)
                rdoCreate.Checked = true;
        }

        public string Project
        {
            get { return _project; }
        }

        public bool StoreProjectFiles
        {
            get { return _storeProjectFiles; }
        }


        private TheGhostCore _core;
        private string _project;
        private bool _storeProjectFiles;

    }
}
