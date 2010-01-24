namespace Nanook.TheGhost
{
    partial class ProjectScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gboProject = new System.Windows.Forms.GroupBox();
            this.chkStoreFiles = new System.Windows.Forms.CheckBox();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.gboDefaultSettings = new System.Windows.Forms.GroupBox();
            this.pgdDefaultSettings = new System.Windows.Forms.PropertyGrid();
            this.gboAudioExport = new System.Windows.Forms.GroupBox();
            this.lblAudioExportPluginMessage = new System.Windows.Forms.Label();
            this.cboAudioExportPlugin = new System.Windows.Forms.ComboBox();
            this.gboAudioImport = new System.Windows.Forms.GroupBox();
            this.lblAudioImportPluginMessage = new System.Windows.Forms.Label();
            this.cboAudioImportPlugin = new System.Windows.Forms.ComboBox();
            this.cboGame = new System.Windows.Forms.ComboBox();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.lblSourcePath = new System.Windows.Forms.Label();
            this.gboSourceFilePlugin = new System.Windows.Forms.GroupBox();
            this.lblSourceFilePluginMessage = new System.Windows.Forms.Label();
            this.cboSourceFilePlugin = new System.Windows.Forms.ComboBox();
            this.txtOpenProject = new System.Windows.Forms.TextBox();
            this.lblOpenProject = new System.Windows.Forms.Label();
            this.txtWorkingFolder = new System.Windows.Forms.TextBox();
            this.lblWorkingFolder = new System.Windows.Forms.Label();
            this.chkReapplyAll = new System.Windows.Forms.CheckBox();
            this.gboProject.SuspendLayout();
            this.gboDefaultSettings.SuspendLayout();
            this.gboAudioExport.SuspendLayout();
            this.gboAudioImport.SuspendLayout();
            this.gboSourceFilePlugin.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboProject
            // 
            this.gboProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboProject.Controls.Add(this.chkReapplyAll);
            this.gboProject.Controls.Add(this.chkStoreFiles);
            this.gboProject.Controls.Add(this.cboLanguage);
            this.gboProject.Controls.Add(this.lblLanguage);
            this.gboProject.Controls.Add(this.gboDefaultSettings);
            this.gboProject.Controls.Add(this.gboAudioExport);
            this.gboProject.Controls.Add(this.gboAudioImport);
            this.gboProject.Controls.Add(this.cboGame);
            this.gboProject.Controls.Add(this.txtSourcePath);
            this.gboProject.Controls.Add(this.lblFormat);
            this.gboProject.Controls.Add(this.lblSourcePath);
            this.gboProject.Controls.Add(this.gboSourceFilePlugin);
            this.gboProject.Controls.Add(this.txtOpenProject);
            this.gboProject.Controls.Add(this.lblOpenProject);
            this.gboProject.Controls.Add(this.txtWorkingFolder);
            this.gboProject.Controls.Add(this.lblWorkingFolder);
            this.gboProject.Location = new System.Drawing.Point(3, 3);
            this.gboProject.Name = "gboProject";
            this.gboProject.Size = new System.Drawing.Size(554, 536);
            this.gboProject.TabIndex = 0;
            this.gboProject.TabStop = false;
            this.gboProject.Text = "Project Settings";
            // 
            // chkStoreFiles
            // 
            this.chkStoreFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkStoreFiles.AutoSize = true;
            this.chkStoreFiles.Location = new System.Drawing.Point(464, 29);
            this.chkStoreFiles.Name = "chkStoreFiles";
            this.chkStoreFiles.Size = new System.Drawing.Size(75, 17);
            this.chkStoreFiles.TabIndex = 14;
            this.chkStoreFiles.Text = "Store Files";
            this.chkStoreFiles.UseVisualStyleBackColor = true;
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Location = new System.Drawing.Point(328, 105);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(130, 21);
            this.cboLanguage.TabIndex = 9;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(264, 108);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(58, 13);
            this.lblLanguage.TabIndex = 8;
            this.lblLanguage.Text = "Language:";
            // 
            // gboDefaultSettings
            // 
            this.gboDefaultSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboDefaultSettings.Controls.Add(this.pgdDefaultSettings);
            this.gboDefaultSettings.Location = new System.Drawing.Point(10, 330);
            this.gboDefaultSettings.Name = "gboDefaultSettings";
            this.gboDefaultSettings.Size = new System.Drawing.Size(534, 199);
            this.gboDefaultSettings.TabIndex = 13;
            this.gboDefaultSettings.TabStop = false;
            this.gboDefaultSettings.Text = "Default Settings";
            // 
            // pgdDefaultSettings
            // 
            this.pgdDefaultSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgdDefaultSettings.Location = new System.Drawing.Point(3, 15);
            this.pgdDefaultSettings.Name = "pgdDefaultSettings";
            this.pgdDefaultSettings.Size = new System.Drawing.Size(527, 180);
            this.pgdDefaultSettings.TabIndex = 0;
            this.pgdDefaultSettings.ToolbarVisible = false;
            // 
            // gboAudioExport
            // 
            this.gboAudioExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboAudioExport.Controls.Add(this.lblAudioExportPluginMessage);
            this.gboAudioExport.Controls.Add(this.cboAudioExportPlugin);
            this.gboAudioExport.Location = new System.Drawing.Point(10, 264);
            this.gboAudioExport.Name = "gboAudioExport";
            this.gboAudioExport.Size = new System.Drawing.Size(534, 60);
            this.gboAudioExport.TabIndex = 12;
            this.gboAudioExport.TabStop = false;
            this.gboAudioExport.Text = "Audio Export Plugin (Encode Wav to GH)";
            // 
            // lblAudioExportPluginMessage
            // 
            this.lblAudioExportPluginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAudioExportPluginMessage.Location = new System.Drawing.Point(208, 13);
            this.lblAudioExportPluginMessage.Name = "lblAudioExportPluginMessage";
            this.lblAudioExportPluginMessage.Size = new System.Drawing.Size(320, 39);
            this.lblAudioExportPluginMessage.TabIndex = 1;
            this.lblAudioExportPluginMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboAudioExportPlugin
            // 
            this.cboAudioExportPlugin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAudioExportPlugin.FormattingEnabled = true;
            this.cboAudioExportPlugin.Location = new System.Drawing.Point(10, 23);
            this.cboAudioExportPlugin.Name = "cboAudioExportPlugin";
            this.cboAudioExportPlugin.Size = new System.Drawing.Size(192, 21);
            this.cboAudioExportPlugin.TabIndex = 0;
            this.cboAudioExportPlugin.SelectedIndexChanged += new System.EventHandler(this.cboAudioExportPlugin_SelectedIndexChanged);
            // 
            // gboAudioImport
            // 
            this.gboAudioImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboAudioImport.Controls.Add(this.lblAudioImportPluginMessage);
            this.gboAudioImport.Controls.Add(this.cboAudioImportPlugin);
            this.gboAudioImport.Location = new System.Drawing.Point(10, 198);
            this.gboAudioImport.Name = "gboAudioImport";
            this.gboAudioImport.Size = new System.Drawing.Size(534, 60);
            this.gboAudioImport.TabIndex = 11;
            this.gboAudioImport.TabStop = false;
            this.gboAudioImport.Text = "Audio Import Plugin (Decode Ogg etc to Wav)";
            // 
            // lblAudioImportPluginMessage
            // 
            this.lblAudioImportPluginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAudioImportPluginMessage.Location = new System.Drawing.Point(208, 13);
            this.lblAudioImportPluginMessage.Name = "lblAudioImportPluginMessage";
            this.lblAudioImportPluginMessage.Size = new System.Drawing.Size(320, 39);
            this.lblAudioImportPluginMessage.TabIndex = 1;
            this.lblAudioImportPluginMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboAudioImportPlugin
            // 
            this.cboAudioImportPlugin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAudioImportPlugin.FormattingEnabled = true;
            this.cboAudioImportPlugin.Location = new System.Drawing.Point(10, 22);
            this.cboAudioImportPlugin.Name = "cboAudioImportPlugin";
            this.cboAudioImportPlugin.Size = new System.Drawing.Size(192, 21);
            this.cboAudioImportPlugin.TabIndex = 0;
            this.cboAudioImportPlugin.SelectedIndexChanged += new System.EventHandler(this.cboAudioImportPlugin_SelectedIndexChanged);
            // 
            // cboGame
            // 
            this.cboGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGame.FormattingEnabled = true;
            this.cboGame.Location = new System.Drawing.Point(94, 105);
            this.cboGame.Name = "cboGame";
            this.cboGame.Size = new System.Drawing.Size(162, 21);
            this.cboGame.TabIndex = 7;
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.AllowDrop = true;
            this.txtSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourcePath.Location = new System.Drawing.Point(94, 79);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(364, 20);
            this.txtSourcePath.TabIndex = 5;
            this.txtSourcePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.project_DragDrop);
            this.txtSourcePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.project_DragEnter);
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(10, 108);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(38, 13);
            this.lblFormat.TabIndex = 6;
            this.lblFormat.Text = "Game:";
            // 
            // lblSourcePath
            // 
            this.lblSourcePath.AutoSize = true;
            this.lblSourcePath.Location = new System.Drawing.Point(10, 82);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new System.Drawing.Size(82, 13);
            this.lblSourcePath.TabIndex = 4;
            this.lblSourcePath.Text = "Game Location:";
            // 
            // gboSourceFilePlugin
            // 
            this.gboSourceFilePlugin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboSourceFilePlugin.Controls.Add(this.lblSourceFilePluginMessage);
            this.gboSourceFilePlugin.Controls.Add(this.cboSourceFilePlugin);
            this.gboSourceFilePlugin.Location = new System.Drawing.Point(10, 132);
            this.gboSourceFilePlugin.Name = "gboSourceFilePlugin";
            this.gboSourceFilePlugin.Size = new System.Drawing.Size(534, 60);
            this.gboSourceFilePlugin.TabIndex = 10;
            this.gboSourceFilePlugin.TabStop = false;
            this.gboSourceFilePlugin.Text = "Source File Plugin";
            // 
            // lblSourceFilePluginMessage
            // 
            this.lblSourceFilePluginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSourceFilePluginMessage.Location = new System.Drawing.Point(208, 13);
            this.lblSourceFilePluginMessage.Name = "lblSourceFilePluginMessage";
            this.lblSourceFilePluginMessage.Size = new System.Drawing.Size(320, 39);
            this.lblSourceFilePluginMessage.TabIndex = 1;
            this.lblSourceFilePluginMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboSourceFilePlugin
            // 
            this.cboSourceFilePlugin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceFilePlugin.FormattingEnabled = true;
            this.cboSourceFilePlugin.Location = new System.Drawing.Point(10, 23);
            this.cboSourceFilePlugin.Name = "cboSourceFilePlugin";
            this.cboSourceFilePlugin.Size = new System.Drawing.Size(192, 21);
            this.cboSourceFilePlugin.TabIndex = 0;
            this.cboSourceFilePlugin.SelectedIndexChanged += new System.EventHandler(this.cboSourceFilePlugin_SelectedIndexChanged);
            // 
            // txtOpenProject
            // 
            this.txtOpenProject.AllowDrop = true;
            this.txtOpenProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOpenProject.Location = new System.Drawing.Point(94, 27);
            this.txtOpenProject.Name = "txtOpenProject";
            this.txtOpenProject.ReadOnly = true;
            this.txtOpenProject.Size = new System.Drawing.Size(364, 20);
            this.txtOpenProject.TabIndex = 1;
            this.txtOpenProject.DragDrop += new System.Windows.Forms.DragEventHandler(this.project_DragDrop);
            this.txtOpenProject.DragEnter += new System.Windows.Forms.DragEventHandler(this.project_DragEnter);
            // 
            // lblOpenProject
            // 
            this.lblOpenProject.AutoSize = true;
            this.lblOpenProject.Enabled = false;
            this.lblOpenProject.Location = new System.Drawing.Point(9, 30);
            this.lblOpenProject.Name = "lblOpenProject";
            this.lblOpenProject.Size = new System.Drawing.Size(43, 13);
            this.lblOpenProject.TabIndex = 0;
            this.lblOpenProject.Text = "Project:";
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.AllowDrop = true;
            this.txtWorkingFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkingFolder.Location = new System.Drawing.Point(94, 53);
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.Size = new System.Drawing.Size(364, 20);
            this.txtWorkingFolder.TabIndex = 3;
            this.txtWorkingFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.project_DragDrop);
            this.txtWorkingFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.project_DragEnter);
            // 
            // lblWorkingFolder
            // 
            this.lblWorkingFolder.AutoSize = true;
            this.lblWorkingFolder.Location = new System.Drawing.Point(10, 56);
            this.lblWorkingFolder.Name = "lblWorkingFolder";
            this.lblWorkingFolder.Size = new System.Drawing.Size(82, 13);
            this.lblWorkingFolder.TabIndex = 2;
            this.lblWorkingFolder.Text = "Working Folder:";
            // 
            // chkReapplyAll
            // 
            this.chkReapplyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkReapplyAll.AutoSize = true;
            this.chkReapplyAll.Location = new System.Drawing.Point(464, 81);
            this.chkReapplyAll.Name = "chkReapplyAll";
            this.chkReapplyAll.Size = new System.Drawing.Size(79, 17);
            this.chkReapplyAll.TabIndex = 15;
            this.chkReapplyAll.Text = "Reapply All";
            this.chkReapplyAll.UseVisualStyleBackColor = true;
            // 
            // ProjectScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboProject);
            this.Name = "ProjectScreen";
            this.Size = new System.Drawing.Size(560, 542);
            this.Load += new System.EventHandler(this.ProjectScreen_Load);
            this.Controls.SetChildIndex(this.gboProject, 0);
            this.gboProject.ResumeLayout(false);
            this.gboProject.PerformLayout();
            this.gboDefaultSettings.ResumeLayout(false);
            this.gboAudioExport.ResumeLayout(false);
            this.gboAudioImport.ResumeLayout(false);
            this.gboSourceFilePlugin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboProject;
        private System.Windows.Forms.GroupBox gboDefaultSettings;
        private System.Windows.Forms.GroupBox gboAudioExport;
        private System.Windows.Forms.Label lblAudioExportPluginMessage;
        private System.Windows.Forms.ComboBox cboAudioExportPlugin;
        private System.Windows.Forms.GroupBox gboAudioImport;
        private System.Windows.Forms.Label lblAudioImportPluginMessage;
        private System.Windows.Forms.ComboBox cboAudioImportPlugin;
        private System.Windows.Forms.ComboBox cboGame;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.Label lblSourcePath;
        private System.Windows.Forms.GroupBox gboSourceFilePlugin;
        private System.Windows.Forms.Label lblSourceFilePluginMessage;
        private System.Windows.Forms.ComboBox cboSourceFilePlugin;
        private System.Windows.Forms.TextBox txtOpenProject;
        private System.Windows.Forms.Label lblOpenProject;
        private System.Windows.Forms.TextBox txtWorkingFolder;
        private System.Windows.Forms.Label lblWorkingFolder;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.PropertyGrid pgdDefaultSettings;
        private System.Windows.Forms.CheckBox chkStoreFiles;
        private System.Windows.Forms.CheckBox chkReapplyAll;
    }
}
