namespace Nanook.TheGhost
{
    partial class TrackSelectScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackSelectScreen));
            this.gboTrackSelect = new System.Windows.Forms.GroupBox();
            this.lblErrorInfo = new System.Windows.Forms.Label();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblSelect = new System.Windows.Forms.Label();
            this.cboSelect = new System.Windows.Forms.ComboBox();
            this.btnExportGameTitles = new System.Windows.Forms.Button();
            this.lvwTracks = new System.Windows.Forms.ListView();
            this.hdrGameTitles = new System.Windows.Forms.ColumnHeader();
            this.hdrMapTo = new System.Windows.Forms.ColumnHeader();
            this.hdrGhId = new System.Windows.Forms.ColumnHeader();
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.export = new System.Windows.Forms.SaveFileDialog();
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.mnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRemoveFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveSavedSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRestoreSavedSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEnableMappings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDisableMappings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCheckItems = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUncheckItems = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSetToChanged = new System.Windows.Forms.ToolStripMenuItem();
            this.gboTrackSelect.SuspendLayout();
            this.mnu.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboTrackSelect
            // 
            this.gboTrackSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboTrackSelect.Controls.Add(this.lblErrorInfo);
            this.gboTrackSelect.Controls.Add(this.btnSelectNone);
            this.gboTrackSelect.Controls.Add(this.btnSelectAll);
            this.gboTrackSelect.Controls.Add(this.lblSelect);
            this.gboTrackSelect.Controls.Add(this.cboSelect);
            this.gboTrackSelect.Controls.Add(this.btnExportGameTitles);
            this.gboTrackSelect.Controls.Add(this.lvwTracks);
            this.gboTrackSelect.Location = new System.Drawing.Point(3, 3);
            this.gboTrackSelect.Name = "gboTrackSelect";
            this.gboTrackSelect.Size = new System.Drawing.Size(504, 508);
            this.gboTrackSelect.TabIndex = 0;
            this.gboTrackSelect.TabStop = false;
            this.gboTrackSelect.Text = "Select Tracks";
            // 
            // lblErrorInfo
            // 
            this.lblErrorInfo.AutoSize = true;
            this.lblErrorInfo.Location = new System.Drawing.Point(21, 19);
            this.lblErrorInfo.Name = "lblErrorInfo";
            this.lblErrorInfo.Size = new System.Drawing.Size(408, 13);
            this.lblErrorInfo.TabIndex = 0;
            this.lblErrorInfo.Text = "* Track has missing files, items may be lost if this project is saved without cor" +
                "recting it.";
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectNone.Location = new System.Drawing.Point(290, 476);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(42, 22);
            this.btnSelectNone.TabIndex = 5;
            this.btnSelectNone.Text = "None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(244, 476);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(42, 22);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.Text = "All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lblSelect
            // 
            this.lblSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(8, 479);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(37, 13);
            this.lblSelect.TabIndex = 2;
            this.lblSelect.Text = "Select";
            // 
            // cboSelect
            // 
            this.cboSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelect.FormattingEnabled = true;
            this.cboSelect.Items.AddRange(new object[] {
            "All Tracks",
            "Mapped To Folders",
            "Unmapped Tracks",
            "Tracks with Errors",
            "Changed Tracks",
            "Added With TheGHOST",
            "All Tiers"});
            this.cboSelect.Location = new System.Drawing.Point(51, 476);
            this.cboSelect.Name = "cboSelect";
            this.cboSelect.Size = new System.Drawing.Size(187, 21);
            this.cboSelect.TabIndex = 3;
            // 
            // btnExportGameTitles
            // 
            this.btnExportGameTitles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportGameTitles.Location = new System.Drawing.Point(382, 475);
            this.btnExportGameTitles.Name = "btnExportGameTitles";
            this.btnExportGameTitles.Size = new System.Drawing.Size(116, 23);
            this.btnExportGameTitles.TabIndex = 6;
            this.btnExportGameTitles.Text = "Export Game Titles...";
            this.btnExportGameTitles.UseVisualStyleBackColor = true;
            this.btnExportGameTitles.Click += new System.EventHandler(this.btnExportGameTitles_Click);
            // 
            // lvwTracks
            // 
            this.lvwTracks.AllowDrop = true;
            this.lvwTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwTracks.CheckBoxes = true;
            this.lvwTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrGameTitles,
            this.hdrMapTo,
            this.hdrGhId});
            this.lvwTracks.FullRowSelect = true;
            this.lvwTracks.Location = new System.Drawing.Point(6, 33);
            this.lvwTracks.Name = "lvwTracks";
            this.lvwTracks.Size = new System.Drawing.Size(492, 436);
            this.lvwTracks.SmallImageList = this.img;
            this.lvwTracks.TabIndex = 1;
            this.lvwTracks.UseCompatibleStateImageBehavior = false;
            this.lvwTracks.View = System.Windows.Forms.View.Details;
            this.lvwTracks.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwTracks_ItemChecked);
            this.lvwTracks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwTracks_MouseClick);
            this.lvwTracks.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvwTracks_DragDrop);
            this.lvwTracks.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvwTracks_DragEnter);
            this.lvwTracks.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvwTracks_ItemDrag);
            this.lvwTracks.DragOver += new System.Windows.Forms.DragEventHandler(this.lvwTracks_DragOver);
            // 
            // hdrGameTitles
            // 
            this.hdrGameTitles.Text = "Game Titles";
            this.hdrGameTitles.Width = 242;
            // 
            // hdrMapTo
            // 
            this.hdrMapTo.Text = "Map to Project or Folder (bold)";
            this.hdrMapTo.Width = 220;
            // 
            // hdrGhId
            // 
            this.hdrGhId.Text = "Guitar Hero Id";
            this.hdrGhId.Width = 0;
            // 
            // img
            // 
            this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
            this.img.TransparentColor = System.Drawing.Color.Transparent;
            this.img.Images.SetKeyName(0, "Nothing.ico");
            this.img.Images.SetKeyName(1, "Green.ico");
            this.img.Images.SetKeyName(2, "Blue.ico");
            // 
            // mnu
            // 
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRemoveFolder,
            this.mnuRemoveSavedSettings,
            this.mnuRestoreSavedSettings,
            this.mnuSep,
            this.mnuEnableMappings,
            this.mnuDisableMappings,
            this.toolStripMenuItem1,
            this.mnuCheckItems,
            this.mnuUncheckItems,
            this.toolStripMenuItem2,
            this.mnuSetToChanged});
            this.mnu.Name = "contextMenuStrip1";
            this.mnu.Size = new System.Drawing.Size(200, 198);
            // 
            // mnuRemoveFolder
            // 
            this.mnuRemoveFolder.Name = "mnuRemoveFolder";
            this.mnuRemoveFolder.Size = new System.Drawing.Size(199, 22);
            this.mnuRemoveFolder.Text = "Remove Folder";
            this.mnuRemoveFolder.Click += new System.EventHandler(this.mnuRemoveFolder_Click);
            // 
            // mnuRemoveSavedSettings
            // 
            this.mnuRemoveSavedSettings.Name = "mnuRemoveSavedSettings";
            this.mnuRemoveSavedSettings.Size = new System.Drawing.Size(199, 22);
            this.mnuRemoveSavedSettings.Text = "Remove Saved Settings";
            this.mnuRemoveSavedSettings.Click += new System.EventHandler(this.mnuRemoveSavedSettings_Click);
            // 
            // mnuRestoreSavedSettings
            // 
            this.mnuRestoreSavedSettings.Name = "mnuRestoreSavedSettings";
            this.mnuRestoreSavedSettings.Size = new System.Drawing.Size(199, 22);
            this.mnuRestoreSavedSettings.Text = "Restore Saved Settings";
            this.mnuRestoreSavedSettings.Click += new System.EventHandler(this.mnuRestoreSavedSettings_Click);
            // 
            // mnuSep
            // 
            this.mnuSep.Name = "mnuSep";
            this.mnuSep.Size = new System.Drawing.Size(196, 6);
            // 
            // mnuEnableMappings
            // 
            this.mnuEnableMappings.Name = "mnuEnableMappings";
            this.mnuEnableMappings.Size = new System.Drawing.Size(199, 22);
            this.mnuEnableMappings.Text = "Enable Mappings";
            this.mnuEnableMappings.Click += new System.EventHandler(this.mnuEnableMappings_Click);
            // 
            // mnuDisableMappings
            // 
            this.mnuDisableMappings.Name = "mnuDisableMappings";
            this.mnuDisableMappings.Size = new System.Drawing.Size(199, 22);
            this.mnuDisableMappings.Text = "Disable Mappings";
            this.mnuDisableMappings.Click += new System.EventHandler(this.mnuDisableMappings_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(196, 6);
            // 
            // mnuCheckItems
            // 
            this.mnuCheckItems.Name = "mnuCheckItems";
            this.mnuCheckItems.Size = new System.Drawing.Size(199, 22);
            this.mnuCheckItems.Text = "Check Items";
            this.mnuCheckItems.Click += new System.EventHandler(this.mnuCheckItems_Click);
            // 
            // mnuUncheckItems
            // 
            this.mnuUncheckItems.Name = "mnuUncheckItems";
            this.mnuUncheckItems.Size = new System.Drawing.Size(199, 22);
            this.mnuUncheckItems.Text = "Uncheck Items";
            this.mnuUncheckItems.Click += new System.EventHandler(this.mnuUncheckItems_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 6);
            // 
            // mnuSetToChanged
            // 
            this.mnuSetToChanged.Name = "mnuSetToChanged";
            this.mnuSetToChanged.Size = new System.Drawing.Size(199, 22);
            this.mnuSetToChanged.Text = "Set to Changed";
            this.mnuSetToChanged.Click += new System.EventHandler(this.mnuSetToChanged_Click);
            // 
            // TrackSelectScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboTrackSelect);
            this.Name = "TrackSelectScreen";
            this.Size = new System.Drawing.Size(510, 514);
            this.Controls.SetChildIndex(this.gboTrackSelect, 0);
            this.gboTrackSelect.ResumeLayout(false);
            this.gboTrackSelect.PerformLayout();
            this.mnu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboTrackSelect;
        private System.Windows.Forms.ListView lvwTracks;
        private System.Windows.Forms.ColumnHeader hdrGameTitles;
        private System.Windows.Forms.ColumnHeader hdrMapTo;
        private System.Windows.Forms.Button btnExportGameTitles;
        private System.Windows.Forms.SaveFileDialog export;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.ComboBox cboSelect;
        private System.Windows.Forms.ColumnHeader hdrGhId;
        private System.Windows.Forms.FolderBrowserDialog folder;
        private System.Windows.Forms.ContextMenuStrip mnu;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveSavedSettings;
        private System.Windows.Forms.ToolStripMenuItem mnuDisableMappings;
        private System.Windows.Forms.ToolStripSeparator mnuSep;
        private System.Windows.Forms.ToolStripMenuItem mnuEnableMappings;
        private System.Windows.Forms.ImageList img;
        private System.Windows.Forms.Label lblErrorInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuCheckItems;
        private System.Windows.Forms.ToolStripMenuItem mnuUncheckItems;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuSetToChanged;
        private System.Windows.Forms.ToolStripMenuItem mnuRestoreSavedSettings;
    }
}
