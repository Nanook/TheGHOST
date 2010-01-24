namespace Nanook.TheGhost
{
    partial class NotesEditScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotesEditScreen));
            this.gboNotesEdit = new System.Windows.Forms.GroupBox();
            this.gboGh = new System.Windows.Forms.GroupBox();
            this.btnSmartMap = new System.Windows.Forms.Button();
            this.lblEnsureDelay = new System.Windows.Forms.Label();
            this.udDelaySeconds = new System.Windows.Forms.NumericUpDown();
            this.lblEnsure = new System.Windows.Forms.Label();
            this.btnGhView = new System.Windows.Forms.Button();
            this.lvwGh = new System.Windows.Forms.ListView();
            this.hdrGhName = new System.Windows.Forms.ColumnHeader();
            this.hdrGhButtons = new System.Windows.Forms.ColumnHeader();
            this.hdrGhNotes = new System.Windows.Forms.ColumnHeader();
            this.hdrGhHasSp = new System.Windows.Forms.ColumnHeader();
            this.hdrGhBattlePower = new System.Windows.Forms.ColumnHeader();
            this.hdrGhFaceOff = new System.Windows.Forms.ColumnHeader();
            this.hdrGhMappedTo = new System.Windows.Forms.ColumnHeader();
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.gboInput = new System.Windows.Forms.GroupBox();
            this.lvwInput = new System.Windows.Forms.ListView();
            this.hdrInputName = new System.Windows.Forms.ColumnHeader();
            this.hdrInputButtons = new System.Windows.Forms.ColumnHeader();
            this.hdrInputHasSP = new System.Windows.Forms.ColumnHeader();
            this.hdrInputNotes = new System.Windows.Forms.ColumnHeader();
            this.lvwFiles = new System.Windows.Forms.ListView();
            this.imgNotes = new System.Windows.Forms.ImageList(this.components);
            this.mnuGh = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMappedNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.generateNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateStarPowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateBattleModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateFaceOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.useForFretsEtcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceNoStarPowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gboNotesEdit.SuspendLayout();
            this.gboGh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDelaySeconds)).BeginInit();
            this.gboInput.SuspendLayout();
            this.mnuGh.SuspendLayout();
            this.mnuFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboNotesEdit
            // 
            this.gboNotesEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboNotesEdit.Controls.Add(this.gboGh);
            this.gboNotesEdit.Controls.Add(this.gboInput);
            this.gboNotesEdit.Location = new System.Drawing.Point(3, 3);
            this.gboNotesEdit.Name = "gboNotesEdit";
            this.gboNotesEdit.Size = new System.Drawing.Size(516, 518);
            this.gboNotesEdit.TabIndex = 0;
            this.gboNotesEdit.TabStop = false;
            this.gboNotesEdit.Text = "Notes Edit";
            // 
            // gboGh
            // 
            this.gboGh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboGh.Controls.Add(this.btnSmartMap);
            this.gboGh.Controls.Add(this.lblEnsureDelay);
            this.gboGh.Controls.Add(this.udDelaySeconds);
            this.gboGh.Controls.Add(this.lblEnsure);
            this.gboGh.Controls.Add(this.btnGhView);
            this.gboGh.Controls.Add(this.lvwGh);
            this.gboGh.Location = new System.Drawing.Point(6, 245);
            this.gboGh.Name = "gboGh";
            this.gboGh.Size = new System.Drawing.Size(504, 267);
            this.gboGh.TabIndex = 1;
            this.gboGh.TabStop = false;
            this.gboGh.Text = "Guitar Hero Notes (* Generated)";
            // 
            // btnSmartMap
            // 
            this.btnSmartMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSmartMap.Location = new System.Drawing.Point(342, 237);
            this.btnSmartMap.Name = "btnSmartMap";
            this.btnSmartMap.Size = new System.Drawing.Size(75, 23);
            this.btnSmartMap.TabIndex = 4;
            this.btnSmartMap.Text = "Smart Map";
            this.btnSmartMap.UseVisualStyleBackColor = true;
            this.btnSmartMap.Click += new System.EventHandler(this.btnSmartMap_Click);
            // 
            // lblEnsureDelay
            // 
            this.lblEnsureDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblEnsureDelay.AutoSize = true;
            this.lblEnsureDelay.Location = new System.Drawing.Point(86, 242);
            this.lblEnsureDelay.Name = "lblEnsureDelay";
            this.lblEnsureDelay.Size = new System.Drawing.Size(139, 13);
            this.lblEnsureDelay.TabIndex = 3;
            this.lblEnsureDelay.Text = "Seconds Before Notes Start";
            // 
            // udDelaySeconds
            // 
            this.udDelaySeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDelaySeconds.Location = new System.Drawing.Point(49, 238);
            this.udDelaySeconds.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.udDelaySeconds.Name = "udDelaySeconds";
            this.udDelaySeconds.Size = new System.Drawing.Size(35, 20);
            this.udDelaySeconds.TabIndex = 2;
            this.udDelaySeconds.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lblEnsure
            // 
            this.lblEnsure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblEnsure.AutoSize = true;
            this.lblEnsure.Location = new System.Drawing.Point(6, 242);
            this.lblEnsure.Name = "lblEnsure";
            this.lblEnsure.Size = new System.Drawing.Size(40, 13);
            this.lblEnsure.TabIndex = 1;
            this.lblEnsure.Text = "Ensure";
            // 
            // btnGhView
            // 
            this.btnGhView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGhView.Location = new System.Drawing.Point(423, 237);
            this.btnGhView.Name = "btnGhView";
            this.btnGhView.Size = new System.Drawing.Size(75, 23);
            this.btnGhView.TabIndex = 5;
            this.btnGhView.Text = "View...";
            this.btnGhView.UseVisualStyleBackColor = true;
            this.btnGhView.Click += new System.EventHandler(this.btnGhView_Click);
            // 
            // lvwGh
            // 
            this.lvwGh.AllowDrop = true;
            this.lvwGh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwGh.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrGhName,
            this.hdrGhButtons,
            this.hdrGhNotes,
            this.hdrGhHasSp,
            this.hdrGhBattlePower,
            this.hdrGhFaceOff,
            this.hdrGhMappedTo});
            this.lvwGh.FullRowSelect = true;
            this.lvwGh.Location = new System.Drawing.Point(6, 19);
            this.lvwGh.Name = "lvwGh";
            this.lvwGh.Size = new System.Drawing.Size(492, 212);
            this.lvwGh.SmallImageList = this.img;
            this.lvwGh.TabIndex = 0;
            this.lvwGh.UseCompatibleStateImageBehavior = false;
            this.lvwGh.View = System.Windows.Forms.View.Details;
            this.lvwGh.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwGh_MouseClick);
            this.lvwGh.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvwGh_DragDrop);
            this.lvwGh.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvwGh_DragEnter);
            this.lvwGh.DragOver += new System.Windows.Forms.DragEventHandler(this.lvwGh_DragOver);
            // 
            // hdrGhName
            // 
            this.hdrGhName.Text = "Name";
            this.hdrGhName.Width = 123;
            // 
            // hdrGhButtons
            // 
            this.hdrGhButtons.Text = "Buttons";
            this.hdrGhButtons.Width = 50;
            // 
            // hdrGhNotes
            // 
            this.hdrGhNotes.Text = "Notes";
            this.hdrGhNotes.Width = 51;
            // 
            // hdrGhHasSp
            // 
            this.hdrGhHasSp.Text = "Star Power";
            this.hdrGhHasSp.Width = 46;
            // 
            // hdrGhBattlePower
            // 
            this.hdrGhBattlePower.Text = "Battle Power";
            this.hdrGhBattlePower.Width = 52;
            // 
            // hdrGhFaceOff
            // 
            this.hdrGhFaceOff.Text = "Face Off Sections";
            this.hdrGhFaceOff.Width = 50;
            // 
            // hdrGhMappedTo
            // 
            this.hdrGhMappedTo.Text = "Mapped To";
            this.hdrGhMappedTo.Width = 225;
            // 
            // img
            // 
            this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
            this.img.TransparentColor = System.Drawing.Color.Transparent;
            this.img.Images.SetKeyName(0, "Easy.ico");
            this.img.Images.SetKeyName(1, "Medium.ico");
            this.img.Images.SetKeyName(2, "Hard.ico");
            this.img.Images.SetKeyName(3, "Expert.ico");
            // 
            // gboInput
            // 
            this.gboInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboInput.Controls.Add(this.lvwInput);
            this.gboInput.Controls.Add(this.lvwFiles);
            this.gboInput.Location = new System.Drawing.Point(6, 21);
            this.gboInput.Name = "gboInput";
            this.gboInput.Size = new System.Drawing.Size(504, 215);
            this.gboInput.TabIndex = 0;
            this.gboInput.TabStop = false;
            this.gboInput.Text = "Add Input Files (.chart)";
            // 
            // lvwInput
            // 
            this.lvwInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwInput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrInputName,
            this.hdrInputButtons,
            this.hdrInputHasSP,
            this.hdrInputNotes});
            this.lvwInput.FullRowSelect = true;
            this.lvwInput.HideSelection = false;
            this.lvwInput.Location = new System.Drawing.Point(6, 86);
            this.lvwInput.MultiSelect = false;
            this.lvwInput.Name = "lvwInput";
            this.lvwInput.Size = new System.Drawing.Size(492, 123);
            this.lvwInput.SmallImageList = this.img;
            this.lvwInput.TabIndex = 1;
            this.lvwInput.UseCompatibleStateImageBehavior = false;
            this.lvwInput.View = System.Windows.Forms.View.Details;
            this.lvwInput.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvwInput_ItemDrag);
            // 
            // hdrInputName
            // 
            this.hdrInputName.Text = "Name";
            this.hdrInputName.Width = 123;
            // 
            // hdrInputButtons
            // 
            this.hdrInputButtons.Text = "Buttons";
            this.hdrInputButtons.Width = 50;
            // 
            // hdrInputHasSP
            // 
            this.hdrInputHasSP.Text = "Star Power";
            this.hdrInputHasSP.Width = 65;
            // 
            // hdrInputNotes
            // 
            this.hdrInputNotes.Text = "Notes";
            this.hdrInputNotes.Width = 61;
            // 
            // lvwFiles
            // 
            this.lvwFiles.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvwFiles.AllowDrop = true;
            this.lvwFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFiles.HideSelection = false;
            this.lvwFiles.LargeImageList = this.imgNotes;
            this.lvwFiles.Location = new System.Drawing.Point(6, 19);
            this.lvwFiles.MultiSelect = false;
            this.lvwFiles.Name = "lvwFiles";
            this.lvwFiles.ShowItemToolTips = true;
            this.lvwFiles.Size = new System.Drawing.Size(492, 61);
            this.lvwFiles.TabIndex = 0;
            this.lvwFiles.UseCompatibleStateImageBehavior = false;
            this.lvwFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwFiles_MouseClick);
            this.lvwFiles.SelectedIndexChanged += new System.EventHandler(this.lvwFiles_SelectedIndexChanged);
            this.lvwFiles.DoubleClick += new System.EventHandler(this.lvwFiles_DoubleClick);
            this.lvwFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvwFiles_DragDrop);
            this.lvwFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvwFiles_DragEnter);
            // 
            // imgNotes
            // 
            this.imgNotes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgNotes.ImageStream")));
            this.imgNotes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgNotes.Images.SetKeyName(0, "Notes.ico");
            this.imgNotes.Images.SetKeyName(1, "NotesBase.ico");
            // 
            // mnuGh
            // 
            this.mnuGh.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeMappingToolStripMenuItem,
            this.removeAllMappingToolStripMenuItem,
            this.viewMappedNotesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.forceNoStarPowerToolStripMenuItem,
            this.generateNotesToolStripMenuItem,
            this.generateStarPowerToolStripMenuItem,
            this.generateBattleModeToolStripMenuItem,
            this.generateFaceOffToolStripMenuItem});
            this.mnuGh.Name = "mnuGh";
            this.mnuGh.Size = new System.Drawing.Size(195, 208);
            // 
            // removeMappingToolStripMenuItem
            // 
            this.removeMappingToolStripMenuItem.Name = "removeMappingToolStripMenuItem";
            this.removeMappingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.removeMappingToolStripMenuItem.Text = "Remove Mapping";
            this.removeMappingToolStripMenuItem.Click += new System.EventHandler(this.removeMappingToolStripMenuItem_Click);
            // 
            // removeAllMappingToolStripMenuItem
            // 
            this.removeAllMappingToolStripMenuItem.Name = "removeAllMappingToolStripMenuItem";
            this.removeAllMappingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.removeAllMappingToolStripMenuItem.Text = "Remove All Mappings";
            this.removeAllMappingToolStripMenuItem.Click += new System.EventHandler(this.removeAllMappingToolStripMenuItem_Click);
            // 
            // viewMappedNotesToolStripMenuItem
            // 
            this.viewMappedNotesToolStripMenuItem.Name = "viewMappedNotesToolStripMenuItem";
            this.viewMappedNotesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.viewMappedNotesToolStripMenuItem.Text = "View Mapped Notes...";
            this.viewMappedNotesToolStripMenuItem.Click += new System.EventHandler(this.viewMappedNotesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // generateNotesToolStripMenuItem
            // 
            this.generateNotesToolStripMenuItem.Name = "generateNotesToolStripMenuItem";
            this.generateNotesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.generateNotesToolStripMenuItem.Text = "Generate Notes";
            this.generateNotesToolStripMenuItem.Visible = false;
            this.generateNotesToolStripMenuItem.Click += new System.EventHandler(this.generateNotesToolStripMenuItem_Click);
            // 
            // generateStarPowerToolStripMenuItem
            // 
            this.generateStarPowerToolStripMenuItem.Name = "generateStarPowerToolStripMenuItem";
            this.generateStarPowerToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.generateStarPowerToolStripMenuItem.Text = "Generate Star Power";
            this.generateStarPowerToolStripMenuItem.Visible = false;
            this.generateStarPowerToolStripMenuItem.Click += new System.EventHandler(this.generateStarPowerToolStripMenuItem_Click);
            // 
            // generateBattleModeToolStripMenuItem
            // 
            this.generateBattleModeToolStripMenuItem.Name = "generateBattleModeToolStripMenuItem";
            this.generateBattleModeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.generateBattleModeToolStripMenuItem.Text = "Generate Battle Power";
            this.generateBattleModeToolStripMenuItem.Visible = false;
            this.generateBattleModeToolStripMenuItem.Click += new System.EventHandler(this.generateBattleModeToolStripMenuItem_Click);
            // 
            // generateFaceOffToolStripMenuItem
            // 
            this.generateFaceOffToolStripMenuItem.Name = "generateFaceOffToolStripMenuItem";
            this.generateFaceOffToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.generateFaceOffToolStripMenuItem.Text = "Generate Face Off";
            this.generateFaceOffToolStripMenuItem.Visible = false;
            this.generateFaceOffToolStripMenuItem.Click += new System.EventHandler(this.generateFaceOffToolStripMenuItem_Click);
            // 
            // mnuFiles
            // 
            this.mnuFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useForFretsEtcToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.removeFileToolStripMenuItem,
            this.viewNotesToolStripMenuItem});
            this.mnuFiles.Name = "mnuFiles";
            this.mnuFiles.Size = new System.Drawing.Size(167, 92);
            // 
            // useForFretsEtcToolStripMenuItem
            // 
            this.useForFretsEtcToolStripMenuItem.CheckOnClick = true;
            this.useForFretsEtcToolStripMenuItem.Name = "useForFretsEtcToolStripMenuItem";
            this.useForFretsEtcToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.useForFretsEtcToolStripMenuItem.Text = "Use for Frets etc";
            this.useForFretsEtcToolStripMenuItem.Click += new System.EventHandler(this.useForFretsEtcToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.reloadToolStripMenuItem.Text = "Reload File";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // removeFileToolStripMenuItem
            // 
            this.removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
            this.removeFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.removeFileToolStripMenuItem.Text = "Remove File";
            this.removeFileToolStripMenuItem.Click += new System.EventHandler(this.removeFileToolStripMenuItem_Click);
            // 
            // viewNotesToolStripMenuItem
            // 
            this.viewNotesToolStripMenuItem.Name = "viewNotesToolStripMenuItem";
            this.viewNotesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.viewNotesToolStripMenuItem.Text = "View Notes...";
            this.viewNotesToolStripMenuItem.Click += new System.EventHandler(this.viewNotesToolStripMenuItem_Click);
            // 
            // forceNoStarPowerToolStripMenuItem
            // 
            this.forceNoStarPowerToolStripMenuItem.Name = "forceNoStarPowerToolStripMenuItem";
            this.forceNoStarPowerToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.forceNoStarPowerToolStripMenuItem.Text = "Force No Star Power";
            this.forceNoStarPowerToolStripMenuItem.Click += new System.EventHandler(this.forceNoStarPowerToolStripMenuItem_Click);
            // 
            // NotesEditScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboNotesEdit);
            this.Name = "NotesEditScreen";
            this.Size = new System.Drawing.Size(522, 524);
            this.Controls.SetChildIndex(this.gboNotesEdit, 0);
            this.gboNotesEdit.ResumeLayout(false);
            this.gboGh.ResumeLayout(false);
            this.gboGh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udDelaySeconds)).EndInit();
            this.gboInput.ResumeLayout(false);
            this.mnuGh.ResumeLayout(false);
            this.mnuFiles.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboNotesEdit;
        private System.Windows.Forms.GroupBox gboInput;
        private System.Windows.Forms.ListView lvwFiles;
        private System.Windows.Forms.ImageList imgNotes;
        private System.Windows.Forms.GroupBox gboGh;
        private System.Windows.Forms.ListView lvwGh;
        private System.Windows.Forms.ListView lvwInput;
        private System.Windows.Forms.Button btnGhView;
        private System.Windows.Forms.ColumnHeader hdrInputName;
        private System.Windows.Forms.ColumnHeader hdrInputButtons;
        private System.Windows.Forms.ColumnHeader hdrInputHasSP;
        private System.Windows.Forms.ColumnHeader hdrGhName;
        private System.Windows.Forms.ColumnHeader hdrGhMappedTo;
        private System.Windows.Forms.ColumnHeader hdrInputNotes;
        private System.Windows.Forms.ColumnHeader hdrGhButtons;
        private System.Windows.Forms.ColumnHeader hdrGhHasSp;
        private System.Windows.Forms.ColumnHeader hdrGhNotes;
        private System.Windows.Forms.ImageList img;
        private System.Windows.Forms.NumericUpDown udDelaySeconds;
        private System.Windows.Forms.Label lblEnsure;
        private System.Windows.Forms.Label lblEnsureDelay;
        private System.Windows.Forms.ContextMenuStrip mnuGh;
        private System.Windows.Forms.ToolStripMenuItem removeMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMappedNotesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mnuFiles;
        private System.Windows.Forms.ToolStripMenuItem viewNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useForFretsEtcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAllMappingToolStripMenuItem;
        private System.Windows.Forms.Button btnSmartMap;
        private System.Windows.Forms.ToolStripMenuItem generateNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateStarPowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateFaceOffToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader hdrGhBattlePower;
        private System.Windows.Forms.ColumnHeader hdrGhFaceOff;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem generateBattleModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceNoStarPowerToolStripMenuItem;
    }
}
