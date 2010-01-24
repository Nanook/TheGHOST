namespace Nanook.TheGhost
{
    partial class TrackEditScreen
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Song File", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Guitar File", 0);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Rhythm File", 0);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Smart Mode", "Wizard.ico");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackEditScreen));
            this.gboTrackEdit = new System.Windows.Forms.GroupBox();
            this.pgdSong = new System.Windows.Forms.PropertyGrid();
            this.gboPreview = new System.Windows.Forms.GroupBox();
            this.udPreviewVolume = new System.Windows.Forms.NumericUpDown();
            this.udPreviewFade = new System.Windows.Forms.NumericUpDown();
            this.udPreviewLength = new System.Windows.Forms.NumericUpDown();
            this.udPreviewStart = new System.Windows.Forms.NumericUpDown();
            this.lblPreviewVolume = new System.Windows.Forms.Label();
            this.chkPreviewRhythm = new System.Windows.Forms.CheckBox();
            this.chkPreviewGuitar = new System.Windows.Forms.CheckBox();
            this.lblPreviewStart = new System.Windows.Forms.Label();
            this.btnPreviewPlay = new System.Windows.Forms.Button();
            this.lblPreviewLength = new System.Windows.Forms.Label();
            this.lblPreviewFade = new System.Windows.Forms.Label();
            this.hscPreviewStart = new System.Windows.Forms.HScrollBar();
            this.lblClicking = new System.Windows.Forms.Label();
            this.tabAudio = new System.Windows.Forms.TabControl();
            this.tabImportAudio = new System.Windows.Forms.TabPage();
            this.lvwFiles = new System.Windows.Forms.ListView();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.tabAdjustVolume = new System.Windows.Forms.TabPage();
            this.lblInfo = new System.Windows.Forms.Label();
            this.chkVolumeApplyAll = new System.Windows.Forms.CheckBox();
            this.lstVolume = new System.Windows.Forms.ListView();
            this.hdrColAudio = new System.Windows.Forms.ColumnHeader();
            this.hdrColVolume = new System.Windows.Forms.ColumnHeader();
            this.imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.udVolume = new System.Windows.Forms.NumericUpDown();
            this.lblVolume = new System.Windows.Forms.Label();
            this.trk = new System.Windows.Forms.TrackBar();
            this.mnuAudio = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRemoveAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlayAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrPreview = new System.Windows.Forms.Timer(this.components);
            this.lblNormalise = new System.Windows.Forms.Label();
            this.gboTrackEdit.SuspendLayout();
            this.gboPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewFade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewStart)).BeginInit();
            this.tabAudio.SuspendLayout();
            this.tabImportAudio.SuspendLayout();
            this.tabAdjustVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trk)).BeginInit();
            this.mnuAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboTrackEdit
            // 
            this.gboTrackEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboTrackEdit.Controls.Add(this.pgdSong);
            this.gboTrackEdit.Controls.Add(this.gboPreview);
            this.gboTrackEdit.Controls.Add(this.tabAudio);
            this.gboTrackEdit.Location = new System.Drawing.Point(3, 3);
            this.gboTrackEdit.Name = "gboTrackEdit";
            this.gboTrackEdit.Size = new System.Drawing.Size(494, 491);
            this.gboTrackEdit.TabIndex = 0;
            this.gboTrackEdit.TabStop = false;
            this.gboTrackEdit.Text = "Track Edit";
            // 
            // pgdSong
            // 
            this.pgdSong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgdSong.Location = new System.Drawing.Point(6, 19);
            this.pgdSong.Name = "pgdSong";
            this.pgdSong.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgdSong.Size = new System.Drawing.Size(482, 237);
            this.pgdSong.TabIndex = 0;
            this.pgdSong.ToolbarVisible = false;
            // 
            // gboPreview
            // 
            this.gboPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboPreview.Controls.Add(this.udPreviewVolume);
            this.gboPreview.Controls.Add(this.udPreviewFade);
            this.gboPreview.Controls.Add(this.udPreviewLength);
            this.gboPreview.Controls.Add(this.udPreviewStart);
            this.gboPreview.Controls.Add(this.lblPreviewVolume);
            this.gboPreview.Controls.Add(this.chkPreviewRhythm);
            this.gboPreview.Controls.Add(this.chkPreviewGuitar);
            this.gboPreview.Controls.Add(this.lblPreviewStart);
            this.gboPreview.Controls.Add(this.btnPreviewPlay);
            this.gboPreview.Controls.Add(this.lblPreviewLength);
            this.gboPreview.Controls.Add(this.lblPreviewFade);
            this.gboPreview.Controls.Add(this.hscPreviewStart);
            this.gboPreview.Controls.Add(this.lblClicking);
            this.gboPreview.Enabled = false;
            this.gboPreview.Location = new System.Drawing.Point(6, 383);
            this.gboPreview.Name = "gboPreview";
            this.gboPreview.Size = new System.Drawing.Size(482, 101);
            this.gboPreview.TabIndex = 2;
            this.gboPreview.TabStop = false;
            this.gboPreview.Text = "Create Song Preview";
            // 
            // udPreviewVolume
            // 
            this.udPreviewVolume.Location = new System.Drawing.Point(373, 23);
            this.udPreviewVolume.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udPreviewVolume.Name = "udPreviewVolume";
            this.udPreviewVolume.Size = new System.Drawing.Size(49, 20);
            this.udPreviewVolume.TabIndex = 7;
            this.udPreviewVolume.ValueChanged += new System.EventHandler(this.udPreviewVolume_ValueChanged);
            // 
            // udPreviewFade
            // 
            this.udPreviewFade.Location = new System.Drawing.Point(266, 23);
            this.udPreviewFade.Name = "udPreviewFade";
            this.udPreviewFade.Size = new System.Drawing.Size(46, 20);
            this.udPreviewFade.TabIndex = 5;
            this.udPreviewFade.ValueChanged += new System.EventHandler(this.udPreviewFade_ValueChanged);
            // 
            // udPreviewLength
            // 
            this.udPreviewLength.Location = new System.Drawing.Point(142, 23);
            this.udPreviewLength.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.udPreviewLength.Name = "udPreviewLength";
            this.udPreviewLength.Size = new System.Drawing.Size(46, 20);
            this.udPreviewLength.TabIndex = 3;
            this.udPreviewLength.ValueChanged += new System.EventHandler(this.udPreviewLength_ValueChanged);
            // 
            // udPreviewStart
            // 
            this.udPreviewStart.Location = new System.Drawing.Point(45, 23);
            this.udPreviewStart.Name = "udPreviewStart";
            this.udPreviewStart.Size = new System.Drawing.Size(47, 20);
            this.udPreviewStart.TabIndex = 1;
            this.udPreviewStart.ValueChanged += new System.EventHandler(this.udPreviewStart_ValueChanged);
            // 
            // lblPreviewVolume
            // 
            this.lblPreviewVolume.AutoSize = true;
            this.lblPreviewVolume.Location = new System.Drawing.Point(317, 26);
            this.lblPreviewVolume.Name = "lblPreviewVolume";
            this.lblPreviewVolume.Size = new System.Drawing.Size(56, 13);
            this.lblPreviewVolume.TabIndex = 6;
            this.lblPreviewVolume.Text = "Volume %:";
            // 
            // chkPreviewRhythm
            // 
            this.chkPreviewRhythm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPreviewRhythm.AutoSize = true;
            this.chkPreviewRhythm.Checked = true;
            this.chkPreviewRhythm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreviewRhythm.Enabled = false;
            this.chkPreviewRhythm.Location = new System.Drawing.Point(328, 78);
            this.chkPreviewRhythm.Name = "chkPreviewRhythm";
            this.chkPreviewRhythm.Size = new System.Drawing.Size(84, 17);
            this.chkPreviewRhythm.TabIndex = 11;
            this.chkPreviewRhythm.Text = "Add Rhythm";
            this.chkPreviewRhythm.UseVisualStyleBackColor = true;
            this.chkPreviewRhythm.CheckedChanged += new System.EventHandler(this.chkPreviewRhythm_CheckedChanged);
            // 
            // chkPreviewGuitar
            // 
            this.chkPreviewGuitar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPreviewGuitar.AutoSize = true;
            this.chkPreviewGuitar.Checked = true;
            this.chkPreviewGuitar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreviewGuitar.Enabled = false;
            this.chkPreviewGuitar.Location = new System.Drawing.Point(247, 78);
            this.chkPreviewGuitar.Name = "chkPreviewGuitar";
            this.chkPreviewGuitar.Size = new System.Drawing.Size(76, 17);
            this.chkPreviewGuitar.TabIndex = 10;
            this.chkPreviewGuitar.Text = "Add Guitar";
            this.chkPreviewGuitar.UseVisualStyleBackColor = true;
            this.chkPreviewGuitar.CheckedChanged += new System.EventHandler(this.chkPreviewGuitar_CheckedChanged);
            // 
            // lblPreviewStart
            // 
            this.lblPreviewStart.AutoSize = true;
            this.lblPreviewStart.Location = new System.Drawing.Point(13, 26);
            this.lblPreviewStart.Name = "lblPreviewStart";
            this.lblPreviewStart.Size = new System.Drawing.Size(32, 13);
            this.lblPreviewStart.TabIndex = 0;
            this.lblPreviewStart.Text = "Start:";
            // 
            // btnPreviewPlay
            // 
            this.btnPreviewPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviewPlay.Location = new System.Drawing.Point(414, 74);
            this.btnPreviewPlay.Name = "btnPreviewPlay";
            this.btnPreviewPlay.Size = new System.Drawing.Size(52, 22);
            this.btnPreviewPlay.TabIndex = 12;
            this.btnPreviewPlay.Text = "Play";
            this.btnPreviewPlay.UseVisualStyleBackColor = true;
            this.btnPreviewPlay.Click += new System.EventHandler(this.btnPreviewPlay_Click);
            // 
            // lblPreviewLength
            // 
            this.lblPreviewLength.AutoSize = true;
            this.lblPreviewLength.Location = new System.Drawing.Point(99, 26);
            this.lblPreviewLength.Name = "lblPreviewLength";
            this.lblPreviewLength.Size = new System.Drawing.Size(43, 13);
            this.lblPreviewLength.TabIndex = 2;
            this.lblPreviewLength.Text = "Length:";
            // 
            // lblPreviewFade
            // 
            this.lblPreviewFade.AutoSize = true;
            this.lblPreviewFade.Location = new System.Drawing.Point(196, 26);
            this.lblPreviewFade.Name = "lblPreviewFade";
            this.lblPreviewFade.Size = new System.Drawing.Size(70, 13);
            this.lblPreviewFade.TabIndex = 4;
            this.lblPreviewFade.Text = "Fade Length:";
            // 
            // hscPreviewStart
            // 
            this.hscPreviewStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hscPreviewStart.LargeChange = 50;
            this.hscPreviewStart.Location = new System.Drawing.Point(16, 48);
            this.hscPreviewStart.Maximum = 200;
            this.hscPreviewStart.Name = "hscPreviewStart";
            this.hscPreviewStart.Size = new System.Drawing.Size(450, 19);
            this.hscPreviewStart.SmallChange = 50;
            this.hscPreviewStart.TabIndex = 8;
            this.hscPreviewStart.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscPreviewStart_Scroll);
            // 
            // lblClicking
            // 
            this.lblClicking.AutoSize = true;
            this.lblClicking.Location = new System.Drawing.Point(13, 79);
            this.lblClicking.Name = "lblClicking";
            this.lblClicking.Size = new System.Drawing.Size(187, 13);
            this.lblClicking.TabIndex = 9;
            this.lblClicking.Text = "If the audio clicks, reduce the volume.";
            // 
            // tabAudio
            // 
            this.tabAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabAudio.Controls.Add(this.tabImportAudio);
            this.tabAudio.Controls.Add(this.tabAdjustVolume);
            this.tabAudio.Location = new System.Drawing.Point(6, 258);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.SelectedIndex = 0;
            this.tabAudio.Size = new System.Drawing.Size(482, 123);
            this.tabAudio.TabIndex = 3;
            this.tabAudio.SelectedIndexChanged += new System.EventHandler(this.tabAudio_SelectedIndexChanged);
            // 
            // tabImportAudio
            // 
            this.tabImportAudio.Controls.Add(this.lblNormalise);
            this.tabImportAudio.Controls.Add(this.lvwFiles);
            this.tabImportAudio.Location = new System.Drawing.Point(4, 22);
            this.tabImportAudio.Name = "tabImportAudio";
            this.tabImportAudio.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportAudio.Size = new System.Drawing.Size(474, 97);
            this.tabImportAudio.TabIndex = 0;
            this.tabImportAudio.Text = "Import Audio";
            this.tabImportAudio.UseVisualStyleBackColor = true;
            // 
            // lvwFiles
            // 
            this.lvwFiles.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvwFiles.AllowDrop = true;
            this.lvwFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFiles.HideSelection = false;
            listViewItem1.Tag = "song";
            listViewItem2.Tag = "guitar";
            listViewItem3.Tag = "rhythm";
            listViewItem4.Tag = "SmartMode";
            listViewItem4.ToolTipText = "Supports FoF folders, Song.ini *.mid and *.chart";
            this.lvwFiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.lvwFiles.LargeImageList = this.imgs;
            this.lvwFiles.Location = new System.Drawing.Point(0, 17);
            this.lvwFiles.MultiSelect = false;
            this.lvwFiles.Name = "lvwFiles";
            this.lvwFiles.ShowItemToolTips = true;
            this.lvwFiles.Size = new System.Drawing.Size(474, 80);
            this.lvwFiles.TabIndex = 1;
            this.lvwFiles.UseCompatibleStateImageBehavior = false;
            this.lvwFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwFiles_MouseClick);
            this.lvwFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvwFiles_DragDrop);
            this.lvwFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvwFiles_DragEnter);
            this.lvwFiles.DragOver += new System.Windows.Forms.DragEventHandler(this.lvwFiles_DragOver);
            // 
            // imgs
            // 
            this.imgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgs.ImageStream")));
            this.imgs.TransparentColor = System.Drawing.Color.Transparent;
            this.imgs.Images.SetKeyName(0, "AudioMissing.ico");
            this.imgs.Images.SetKeyName(1, "Audio.ico");
            this.imgs.Images.SetKeyName(2, "Wizard.ico");
            // 
            // tabAdjustVolume
            // 
            this.tabAdjustVolume.Controls.Add(this.lblInfo);
            this.tabAdjustVolume.Controls.Add(this.chkVolumeApplyAll);
            this.tabAdjustVolume.Controls.Add(this.lstVolume);
            this.tabAdjustVolume.Controls.Add(this.udVolume);
            this.tabAdjustVolume.Controls.Add(this.lblVolume);
            this.tabAdjustVolume.Controls.Add(this.trk);
            this.tabAdjustVolume.Location = new System.Drawing.Point(4, 22);
            this.tabAdjustVolume.Name = "tabAdjustVolume";
            this.tabAdjustVolume.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdjustVolume.Size = new System.Drawing.Size(474, 97);
            this.tabAdjustVolume.TabIndex = 1;
            this.tabAdjustVolume.Text = "Adjust Volume";
            this.tabAdjustVolume.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(216, 57);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(258, 39);
            this.lblInfo.TabIndex = 13;
            this.lblInfo.Text = "Set the volume levels to get the correct balance for the in game audio. Slider ad" +
                "justs all by %";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkVolumeApplyAll
            // 
            this.chkVolumeApplyAll.AutoSize = true;
            this.chkVolumeApplyAll.Location = new System.Drawing.Point(326, 13);
            this.chkVolumeApplyAll.Name = "chkVolumeApplyAll";
            this.chkVolumeApplyAll.Size = new System.Drawing.Size(82, 17);
            this.chkVolumeApplyAll.TabIndex = 12;
            this.chkVolumeApplyAll.Text = "Apply To All";
            this.chkVolumeApplyAll.UseVisualStyleBackColor = true;
            // 
            // lstVolume
            // 
            this.lstVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstVolume.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrColAudio,
            this.hdrColVolume});
            this.lstVolume.FullRowSelect = true;
            this.lstVolume.HideSelection = false;
            this.lstVolume.Location = new System.Drawing.Point(0, 0);
            this.lstVolume.MultiSelect = false;
            this.lstVolume.Name = "lstVolume";
            this.lstVolume.ShowItemToolTips = true;
            this.lstVolume.Size = new System.Drawing.Size(210, 97);
            this.lstVolume.SmallImageList = this.imgSmall;
            this.lstVolume.TabIndex = 11;
            this.lstVolume.UseCompatibleStateImageBehavior = false;
            this.lstVolume.View = System.Windows.Forms.View.Details;
            this.lstVolume.SelectedIndexChanged += new System.EventHandler(this.lstVolume_SelectedIndexChanged);
            // 
            // hdrColAudio
            // 
            this.hdrColAudio.Text = "Audio";
            this.hdrColAudio.Width = 124;
            // 
            // hdrColVolume
            // 
            this.hdrColVolume.Text = "Volume";
            // 
            // imgSmall
            // 
            this.imgSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSmall.ImageStream")));
            this.imgSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSmall.Images.SetKeyName(0, "AudioSmall.ico");
            // 
            // udVolume
            // 
            this.udVolume.Location = new System.Drawing.Point(271, 11);
            this.udVolume.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.udVolume.Name = "udVolume";
            this.udVolume.Size = new System.Drawing.Size(49, 20);
            this.udVolume.TabIndex = 9;
            this.udVolume.ValueChanged += new System.EventHandler(this.udVolume_ValueChanged);
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(215, 14);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(56, 13);
            this.lblVolume.TabIndex = 8;
            this.lblVolume.Text = "Volume %:";
            // 
            // trk
            // 
            this.trk.LargeChange = 2;
            this.trk.Location = new System.Drawing.Point(209, 33);
            this.trk.Maximum = 100;
            this.trk.Minimum = -100;
            this.trk.Name = "trk";
            this.trk.Size = new System.Drawing.Size(205, 42);
            this.trk.TabIndex = 14;
            this.trk.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trk.ValueChanged += new System.EventHandler(this.trk_ValueChanged);
            this.trk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trk_MouseDown);
            this.trk.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trk_MouseUp);
            // 
            // mnuAudio
            // 
            this.mnuAudio.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRemoveAudio,
            this.mnuPlayAudio});
            this.mnuAudio.Name = "mnuAudio";
            this.mnuAudio.Size = new System.Drawing.Size(155, 48);
            // 
            // mnuRemoveAudio
            // 
            this.mnuRemoveAudio.Name = "mnuRemoveAudio";
            this.mnuRemoveAudio.Size = new System.Drawing.Size(154, 22);
            this.mnuRemoveAudio.Text = "Remove Audio";
            this.mnuRemoveAudio.Click += new System.EventHandler(this.mnuRemoveAudio_Click);
            // 
            // mnuPlayAudio
            // 
            this.mnuPlayAudio.Name = "mnuPlayAudio";
            this.mnuPlayAudio.Size = new System.Drawing.Size(154, 22);
            this.mnuPlayAudio.Text = "Play Audio...";
            this.mnuPlayAudio.Visible = false;
            // 
            // tmrPreview
            // 
            this.tmrPreview.Tick += new System.EventHandler(this.tmrPreview_Tick);
            // 
            // lblNormalise
            // 
            this.lblNormalise.AutoSize = true;
            this.lblNormalise.Location = new System.Drawing.Point(3, 3);
            this.lblNormalise.Name = "lblNormalise";
            this.lblNormalise.Size = new System.Drawing.Size(375, 13);
            this.lblNormalise.TabIndex = 2;
            this.lblNormalise.Text = "Single song audio tracks will be normalised making all song volumes the same.";
            // 
            // TrackEditScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboTrackEdit);
            this.Name = "TrackEditScreen";
            this.Size = new System.Drawing.Size(500, 494);
            this.Controls.SetChildIndex(this.gboTrackEdit, 0);
            this.gboTrackEdit.ResumeLayout(false);
            this.gboPreview.ResumeLayout(false);
            this.gboPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewFade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewStart)).EndInit();
            this.tabAudio.ResumeLayout(false);
            this.tabImportAudio.ResumeLayout(false);
            this.tabImportAudio.PerformLayout();
            this.tabAdjustVolume.ResumeLayout(false);
            this.tabAdjustVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trk)).EndInit();
            this.mnuAudio.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboTrackEdit;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.ContextMenuStrip mnuAudio;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveAudio;
        private System.Windows.Forms.ToolStripMenuItem mnuPlayAudio;
        private System.Windows.Forms.HScrollBar hscPreviewStart;
        private System.Windows.Forms.GroupBox gboPreview;
        private System.Windows.Forms.Label lblPreviewFade;
        private System.Windows.Forms.Label lblPreviewLength;
        private System.Windows.Forms.Button btnPreviewPlay;
        private System.Windows.Forms.PropertyGrid pgdSong;
        private System.Windows.Forms.Label lblPreviewStart;
        private System.Windows.Forms.CheckBox chkPreviewGuitar;
        private System.Windows.Forms.CheckBox chkPreviewRhythm;
        private System.Windows.Forms.Label lblPreviewVolume;
        private System.Windows.Forms.NumericUpDown udPreviewStart;
        private System.Windows.Forms.NumericUpDown udPreviewLength;
        private System.Windows.Forms.NumericUpDown udPreviewVolume;
        private System.Windows.Forms.NumericUpDown udPreviewFade;
        private System.Windows.Forms.Timer tmrPreview;
        private System.Windows.Forms.Label lblClicking;
        private System.Windows.Forms.TabControl tabAudio;
        private System.Windows.Forms.TabPage tabImportAudio;
        private System.Windows.Forms.ListView lvwFiles;
        private System.Windows.Forms.TabPage tabAdjustVolume;
        private System.Windows.Forms.NumericUpDown udVolume;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.ListView lstVolume;
        private System.Windows.Forms.ColumnHeader hdrColAudio;
        private System.Windows.Forms.ColumnHeader hdrColVolume;
        private System.Windows.Forms.ImageList imgSmall;
        private System.Windows.Forms.CheckBox chkVolumeApplyAll;
        private System.Windows.Forms.TrackBar trk;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblNormalise;
    }
}
