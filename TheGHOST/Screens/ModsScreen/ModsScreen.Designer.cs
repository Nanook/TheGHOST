namespace Nanook.TheGhost
{
    partial class ModsScreen
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Add Audio", 0);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModsScreen));
            this.gboMods = new System.Windows.Forms.GroupBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabModsQb = new System.Windows.Forms.TabPage();
            this.gboTierNames = new System.Windows.Forms.GroupBox();
            this.btnTierName = new System.Windows.Forms.Button();
            this.txtTierName = new System.Windows.Forms.TextBox();
            this.lblTierName = new System.Windows.Forms.Label();
            this.lstTierNames = new System.Windows.Forms.ListBox();
            this.gboModsQb = new System.Windows.Forms.GroupBox();
            this.txtDefaultBonusSongInfoText = new System.Windows.Forms.TextBox();
            this.chkDefaultBonusSongInfoText = new System.Windows.Forms.CheckBox();
            this.lblModsQb = new System.Windows.Forms.Label();
            this.chkUnlockSetlists = new System.Windows.Forms.CheckBox();
            this.chkDefaultBonusSongArt = new System.Windows.Forms.CheckBox();
            this.chkCompleteTier1Song = new System.Windows.Forms.CheckBox();
            this.chkFreeStore = new System.Windows.Forms.CheckBox();
            this.chkAddNonCareerTracksToBonus = new System.Windows.Forms.CheckBox();
            this.chkSetCheats = new System.Windows.Forms.CheckBox();
            this.tabModsAudio = new System.Windows.Forms.TabPage();
            this.tabAudio = new System.Windows.Forms.TabControl();
            this.tabImportAudio = new System.Windows.Forms.TabPage();
            this.lblNormalise = new System.Windows.Forms.Label();
            this.lvwFiles = new System.Windows.Forms.ListView();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.tabAdjustVolume = new System.Windows.Forms.TabPage();
            this.chkVolumeApplyAll = new System.Windows.Forms.CheckBox();
            this.lstVolume = new System.Windows.Forms.ListView();
            this.hdrColAudio = new System.Windows.Forms.ColumnHeader();
            this.hdrColVolume = new System.Windows.Forms.ColumnHeader();
            this.imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.udVolume = new System.Windows.Forms.NumericUpDown();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.trk = new System.Windows.Forms.TrackBar();
            this.gboPreview = new System.Windows.Forms.GroupBox();
            this.lblClicking = new System.Windows.Forms.Label();
            this.lblPreviewLength = new System.Windows.Forms.Label();
            this.udPreviewVolume = new System.Windows.Forms.NumericUpDown();
            this.udPreviewFade = new System.Windows.Forms.NumericUpDown();
            this.udPreviewLength = new System.Windows.Forms.NumericUpDown();
            this.udPreviewStart = new System.Windows.Forms.NumericUpDown();
            this.lblPreviewVolume = new System.Windows.Forms.Label();
            this.lblPreviewStart = new System.Windows.Forms.Label();
            this.btnPreviewPlay = new System.Windows.Forms.Button();
            this.lblPreviewFade = new System.Windows.Forms.Label();
            this.hscPreviewStart = new System.Windows.Forms.HScrollBar();
            this.lvwAudio = new System.Windows.Forms.ListView();
            this.hdrName = new System.Windows.Forms.ColumnHeader();
            this.tmrPreview = new System.Windows.Forms.Timer(this.components);
            this.mnuAudio = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRemoveAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlayAudio = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrGreyHack = new System.Windows.Forms.Timer(this.components);
            this.gboMods.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabModsQb.SuspendLayout();
            this.gboTierNames.SuspendLayout();
            this.gboModsQb.SuspendLayout();
            this.tabModsAudio.SuspendLayout();
            this.tabAudio.SuspendLayout();
            this.tabImportAudio.SuspendLayout();
            this.tabAdjustVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trk)).BeginInit();
            this.gboPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewFade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewStart)).BeginInit();
            this.mnuAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboMods
            // 
            this.gboMods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboMods.Controls.Add(this.tabs);
            this.gboMods.Location = new System.Drawing.Point(3, 3);
            this.gboMods.Name = "gboMods";
            this.gboMods.Size = new System.Drawing.Size(491, 468);
            this.gboMods.TabIndex = 0;
            this.gboMods.TabStop = false;
            this.gboMods.Text = "Game Modifications";
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabModsQb);
            this.tabs.Controls.Add(this.tabModsAudio);
            this.tabs.Location = new System.Drawing.Point(6, 19);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(479, 443);
            this.tabs.TabIndex = 0;
            // 
            // tabModsQb
            // 
            this.tabModsQb.Controls.Add(this.gboTierNames);
            this.tabModsQb.Controls.Add(this.gboModsQb);
            this.tabModsQb.Location = new System.Drawing.Point(4, 22);
            this.tabModsQb.Name = "tabModsQb";
            this.tabModsQb.Padding = new System.Windows.Forms.Padding(3);
            this.tabModsQb.Size = new System.Drawing.Size(471, 417);
            this.tabModsQb.TabIndex = 0;
            this.tabModsQb.Text = "QB Mods";
            this.tabModsQb.UseVisualStyleBackColor = true;
            // 
            // gboTierNames
            // 
            this.gboTierNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboTierNames.Controls.Add(this.btnTierName);
            this.gboTierNames.Controls.Add(this.txtTierName);
            this.gboTierNames.Controls.Add(this.lblTierName);
            this.gboTierNames.Controls.Add(this.lstTierNames);
            this.gboTierNames.Location = new System.Drawing.Point(6, 185);
            this.gboTierNames.Name = "gboTierNames";
            this.gboTierNames.Size = new System.Drawing.Size(459, 226);
            this.gboTierNames.TabIndex = 1;
            this.gboTierNames.TabStop = false;
            this.gboTierNames.Text = "Tier Names";
            // 
            // btnTierName
            // 
            this.btnTierName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTierName.Location = new System.Drawing.Point(378, 197);
            this.btnTierName.Name = "btnTierName";
            this.btnTierName.Size = new System.Drawing.Size(75, 23);
            this.btnTierName.TabIndex = 3;
            this.btnTierName.Text = "Update";
            this.btnTierName.UseVisualStyleBackColor = true;
            this.btnTierName.Click += new System.EventHandler(this.btnTierName_Click);
            // 
            // txtTierName
            // 
            this.txtTierName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTierName.Location = new System.Drawing.Point(50, 199);
            this.txtTierName.Name = "txtTierName";
            this.txtTierName.Size = new System.Drawing.Size(322, 20);
            this.txtTierName.TabIndex = 2;
            // 
            // lblTierName
            // 
            this.lblTierName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTierName.AutoSize = true;
            this.lblTierName.Location = new System.Drawing.Point(6, 202);
            this.lblTierName.Name = "lblTierName";
            this.lblTierName.Size = new System.Drawing.Size(38, 13);
            this.lblTierName.TabIndex = 1;
            this.lblTierName.Text = "Name:";
            // 
            // lstTierNames
            // 
            this.lstTierNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTierNames.FormattingEnabled = true;
            this.lstTierNames.IntegralHeight = false;
            this.lstTierNames.Location = new System.Drawing.Point(6, 19);
            this.lstTierNames.Name = "lstTierNames";
            this.lstTierNames.Size = new System.Drawing.Size(447, 176);
            this.lstTierNames.TabIndex = 0;
            this.lstTierNames.SelectedIndexChanged += new System.EventHandler(this.lstTierNames_SelectedIndexChanged);
            // 
            // gboModsQb
            // 
            this.gboModsQb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboModsQb.Controls.Add(this.txtDefaultBonusSongInfoText);
            this.gboModsQb.Controls.Add(this.chkDefaultBonusSongInfoText);
            this.gboModsQb.Controls.Add(this.lblModsQb);
            this.gboModsQb.Controls.Add(this.chkUnlockSetlists);
            this.gboModsQb.Controls.Add(this.chkDefaultBonusSongArt);
            this.gboModsQb.Controls.Add(this.chkCompleteTier1Song);
            this.gboModsQb.Controls.Add(this.chkFreeStore);
            this.gboModsQb.Controls.Add(this.chkAddNonCareerTracksToBonus);
            this.gboModsQb.Controls.Add(this.chkSetCheats);
            this.gboModsQb.Location = new System.Drawing.Point(6, 6);
            this.gboModsQb.Name = "gboModsQb";
            this.gboModsQb.Size = new System.Drawing.Size(459, 173);
            this.gboModsQb.TabIndex = 0;
            this.gboModsQb.TabStop = false;
            this.gboModsQb.Text = "QB Mods";
            // 
            // txtDefaultBonusSongInfoText
            // 
            this.txtDefaultBonusSongInfoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultBonusSongInfoText.Location = new System.Drawing.Point(37, 137);
            this.txtDefaultBonusSongInfoText.Name = "txtDefaultBonusSongInfoText";
            this.txtDefaultBonusSongInfoText.Size = new System.Drawing.Size(416, 20);
            this.txtDefaultBonusSongInfoText.TabIndex = 7;
            // 
            // chkDefaultBonusSongInfoText
            // 
            this.chkDefaultBonusSongInfoText.AutoSize = true;
            this.chkDefaultBonusSongInfoText.Location = new System.Drawing.Point(17, 120);
            this.chkDefaultBonusSongInfoText.Name = "chkDefaultBonusSongInfoText";
            this.chkDefaultBonusSongInfoText.Size = new System.Drawing.Size(142, 17);
            this.chkDefaultBonusSongInfoText.TabIndex = 6;
            this.chkDefaultBonusSongInfoText.Text = "Default Bonus Song Info";
            this.chkDefaultBonusSongInfoText.UseVisualStyleBackColor = true;
            // 
            // lblModsQb
            // 
            this.lblModsQb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModsQb.Location = new System.Drawing.Point(210, 19);
            this.lblModsQb.Name = "lblModsQb";
            this.lblModsQb.Size = new System.Drawing.Size(243, 115);
            this.lblModsQb.TabIndex = 8;
            this.lblModsQb.Text = "Modifications to various QB items within the game.  These items can only set they" +
                " cannot be undone if already applied to the ISO.";
            // 
            // chkUnlockSetlists
            // 
            this.chkUnlockSetlists.AutoSize = true;
            this.chkUnlockSetlists.Location = new System.Drawing.Point(17, 18);
            this.chkUnlockSetlists.Name = "chkUnlockSetlists";
            this.chkUnlockSetlists.Size = new System.Drawing.Size(96, 17);
            this.chkUnlockSetlists.TabIndex = 0;
            this.chkUnlockSetlists.Text = "Unlock Setlists";
            this.chkUnlockSetlists.UseVisualStyleBackColor = true;
            // 
            // chkDefaultBonusSongArt
            // 
            this.chkDefaultBonusSongArt.AutoSize = true;
            this.chkDefaultBonusSongArt.Location = new System.Drawing.Point(17, 103);
            this.chkDefaultBonusSongArt.Name = "chkDefaultBonusSongArt";
            this.chkDefaultBonusSongArt.Size = new System.Drawing.Size(137, 17);
            this.chkDefaultBonusSongArt.TabIndex = 5;
            this.chkDefaultBonusSongArt.Text = "Default Bonus Song Art";
            this.chkDefaultBonusSongArt.UseVisualStyleBackColor = true;
            // 
            // chkCompleteTier1Song
            // 
            this.chkCompleteTier1Song.AutoSize = true;
            this.chkCompleteTier1Song.Location = new System.Drawing.Point(17, 35);
            this.chkCompleteTier1Song.Name = "chkCompleteTier1Song";
            this.chkCompleteTier1Song.Size = new System.Drawing.Size(128, 17);
            this.chkCompleteTier1Song.TabIndex = 1;
            this.chkCompleteTier1Song.Text = "Complete Tier 1 Song";
            this.chkCompleteTier1Song.UseVisualStyleBackColor = true;
            // 
            // chkFreeStore
            // 
            this.chkFreeStore.AutoSize = true;
            this.chkFreeStore.Location = new System.Drawing.Point(17, 86);
            this.chkFreeStore.Name = "chkFreeStore";
            this.chkFreeStore.Size = new System.Drawing.Size(75, 17);
            this.chkFreeStore.TabIndex = 4;
            this.chkFreeStore.Text = "Free Store";
            this.chkFreeStore.UseVisualStyleBackColor = true;
            // 
            // chkAddNonCareerTracksToBonus
            // 
            this.chkAddNonCareerTracksToBonus.AutoSize = true;
            this.chkAddNonCareerTracksToBonus.Location = new System.Drawing.Point(17, 52);
            this.chkAddNonCareerTracksToBonus.Name = "chkAddNonCareerTracksToBonus";
            this.chkAddNonCareerTracksToBonus.Size = new System.Drawing.Size(187, 17);
            this.chkAddNonCareerTracksToBonus.TabIndex = 2;
            this.chkAddNonCareerTracksToBonus.Text = "Add Non Career Tracks To Bonus";
            this.chkAddNonCareerTracksToBonus.UseVisualStyleBackColor = true;
            // 
            // chkSetCheats
            // 
            this.chkSetCheats.AutoSize = true;
            this.chkSetCheats.Location = new System.Drawing.Point(17, 69);
            this.chkSetCheats.Name = "chkSetCheats";
            this.chkSetCheats.Size = new System.Drawing.Size(78, 17);
            this.chkSetCheats.TabIndex = 3;
            this.chkSetCheats.Text = "Set Cheats";
            this.chkSetCheats.UseVisualStyleBackColor = true;
            // 
            // tabModsAudio
            // 
            this.tabModsAudio.Controls.Add(this.tabAudio);
            this.tabModsAudio.Controls.Add(this.gboPreview);
            this.tabModsAudio.Controls.Add(this.lvwAudio);
            this.tabModsAudio.Location = new System.Drawing.Point(4, 22);
            this.tabModsAudio.Name = "tabModsAudio";
            this.tabModsAudio.Padding = new System.Windows.Forms.Padding(3);
            this.tabModsAudio.Size = new System.Drawing.Size(471, 417);
            this.tabModsAudio.TabIndex = 1;
            this.tabModsAudio.Text = "Background Music";
            this.tabModsAudio.UseVisualStyleBackColor = true;
            // 
            // tabAudio
            // 
            this.tabAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabAudio.Controls.Add(this.tabImportAudio);
            this.tabAudio.Controls.Add(this.tabAdjustVolume);
            this.tabAudio.Location = new System.Drawing.Point(0, 182);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.SelectedIndex = 0;
            this.tabAudio.Size = new System.Drawing.Size(470, 123);
            this.tabAudio.TabIndex = 5;
            this.tabAudio.SelectedIndexChanged += new System.EventHandler(this.tabAudio_SelectedIndexChanged);
            // 
            // tabImportAudio
            // 
            this.tabImportAudio.Controls.Add(this.lblNormalise);
            this.tabImportAudio.Controls.Add(this.lvwFiles);
            this.tabImportAudio.Location = new System.Drawing.Point(4, 22);
            this.tabImportAudio.Name = "tabImportAudio";
            this.tabImportAudio.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportAudio.Size = new System.Drawing.Size(462, 97);
            this.tabImportAudio.TabIndex = 0;
            this.tabImportAudio.Text = "Import Audio";
            this.tabImportAudio.UseVisualStyleBackColor = true;
            // 
            // lblNormalise
            // 
            this.lblNormalise.AutoSize = true;
            this.lblNormalise.Location = new System.Drawing.Point(3, 3);
            this.lblNormalise.Name = "lblNormalise";
            this.lblNormalise.Size = new System.Drawing.Size(389, 13);
            this.lblNormalise.TabIndex = 1;
            this.lblNormalise.Text = "Audio will be normalised making all volumes the same (before Volume% is applied)";
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
            this.lvwFiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvwFiles.LargeImageList = this.imgs;
            this.lvwFiles.Location = new System.Drawing.Point(0, 17);
            this.lvwFiles.MultiSelect = false;
            this.lvwFiles.Name = "lvwFiles";
            this.lvwFiles.ShowItemToolTips = true;
            this.lvwFiles.Size = new System.Drawing.Size(462, 81);
            this.lvwFiles.TabIndex = 0;
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
            // 
            // tabAdjustVolume
            // 
            this.tabAdjustVolume.Controls.Add(this.chkVolumeApplyAll);
            this.tabAdjustVolume.Controls.Add(this.lstVolume);
            this.tabAdjustVolume.Controls.Add(this.udVolume);
            this.tabAdjustVolume.Controls.Add(this.lblVolume);
            this.tabAdjustVolume.Controls.Add(this.lblInfo);
            this.tabAdjustVolume.Controls.Add(this.trk);
            this.tabAdjustVolume.Location = new System.Drawing.Point(4, 22);
            this.tabAdjustVolume.Name = "tabAdjustVolume";
            this.tabAdjustVolume.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdjustVolume.Size = new System.Drawing.Size(462, 97);
            this.tabAdjustVolume.TabIndex = 1;
            this.tabAdjustVolume.Text = "Adjust Volume";
            this.tabAdjustVolume.UseVisualStyleBackColor = true;
            // 
            // chkVolumeApplyAll
            // 
            this.chkVolumeApplyAll.AutoSize = true;
            this.chkVolumeApplyAll.Location = new System.Drawing.Point(326, 11);
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
            this.udVolume.Location = new System.Drawing.Point(271, 9);
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
            this.lblVolume.Location = new System.Drawing.Point(215, 12);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(56, 13);
            this.lblVolume.TabIndex = 8;
            this.lblVolume.Text = "Volume %:";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(211, 53);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(250, 41);
            this.lblInfo.TabIndex = 16;
            this.lblInfo.Text = "Set the volume levels to get the correct balance for the in game audio. Slider ad" +
                "justs all by %";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trk
            // 
            this.trk.LargeChange = 2;
            this.trk.Location = new System.Drawing.Point(209, 31);
            this.trk.Maximum = 100;
            this.trk.Minimum = -100;
            this.trk.Name = "trk";
            this.trk.Size = new System.Drawing.Size(205, 42);
            this.trk.TabIndex = 15;
            this.trk.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trk.ValueChanged += new System.EventHandler(this.trk_ValueChanged);
            this.trk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trk_MouseDown);
            this.trk.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trk_MouseUp);
            // 
            // gboPreview
            // 
            this.gboPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboPreview.Controls.Add(this.lblClicking);
            this.gboPreview.Controls.Add(this.lblPreviewLength);
            this.gboPreview.Controls.Add(this.udPreviewVolume);
            this.gboPreview.Controls.Add(this.udPreviewFade);
            this.gboPreview.Controls.Add(this.udPreviewLength);
            this.gboPreview.Controls.Add(this.udPreviewStart);
            this.gboPreview.Controls.Add(this.lblPreviewVolume);
            this.gboPreview.Controls.Add(this.lblPreviewStart);
            this.gboPreview.Controls.Add(this.btnPreviewPlay);
            this.gboPreview.Controls.Add(this.lblPreviewFade);
            this.gboPreview.Controls.Add(this.hscPreviewStart);
            this.gboPreview.Enabled = false;
            this.gboPreview.Location = new System.Drawing.Point(0, 308);
            this.gboPreview.Name = "gboPreview";
            this.gboPreview.Size = new System.Drawing.Size(471, 109);
            this.gboPreview.TabIndex = 3;
            this.gboPreview.TabStop = false;
            this.gboPreview.Text = "Create Background Music";
            // 
            // lblClicking
            // 
            this.lblClicking.AutoSize = true;
            this.lblClicking.Location = new System.Drawing.Point(13, 79);
            this.lblClicking.Name = "lblClicking";
            this.lblClicking.Size = new System.Drawing.Size(187, 13);
            this.lblClicking.TabIndex = 12;
            this.lblClicking.Text = "If the audio clicks, reduce the volume.";
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
            this.btnPreviewPlay.Location = new System.Drawing.Point(403, 74);
            this.btnPreviewPlay.Name = "btnPreviewPlay";
            this.btnPreviewPlay.Size = new System.Drawing.Size(52, 22);
            this.btnPreviewPlay.TabIndex = 11;
            this.btnPreviewPlay.Text = "Play";
            this.btnPreviewPlay.UseVisualStyleBackColor = true;
            this.btnPreviewPlay.Click += new System.EventHandler(this.btnPreviewPlay_Click);
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
            this.hscPreviewStart.Size = new System.Drawing.Size(439, 19);
            this.hscPreviewStart.SmallChange = 50;
            this.hscPreviewStart.TabIndex = 8;
            this.hscPreviewStart.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscPreviewStart_Scroll);
            // 
            // lvwAudio
            // 
            this.lvwAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwAudio.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrName});
            this.lvwAudio.FullRowSelect = true;
            this.lvwAudio.HideSelection = false;
            this.lvwAudio.Location = new System.Drawing.Point(0, 3);
            this.lvwAudio.MultiSelect = false;
            this.lvwAudio.Name = "lvwAudio";
            this.lvwAudio.Size = new System.Drawing.Size(471, 177);
            this.lvwAudio.TabIndex = 0;
            this.lvwAudio.UseCompatibleStateImageBehavior = false;
            this.lvwAudio.View = System.Windows.Forms.View.Details;
            this.lvwAudio.SelectedIndexChanged += new System.EventHandler(this.lvwAudio_SelectedIndexChanged);
            // 
            // hdrName
            // 
            this.hdrName.Text = "Title ID";
            this.hdrName.Width = 244;
            // 
            // tmrPreview
            // 
            this.tmrPreview.Tick += new System.EventHandler(this.tmrPreview_Tick);
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
            // tmrGreyHack
            // 
            this.tmrGreyHack.Interval = 10;
            this.tmrGreyHack.Tick += new System.EventHandler(this.tmrGreyHack_Tick);
            // 
            // ModsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboMods);
            this.Name = "ModsScreen";
            this.Size = new System.Drawing.Size(497, 474);
            this.Controls.SetChildIndex(this.gboMods, 0);
            this.gboMods.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabModsQb.ResumeLayout(false);
            this.gboTierNames.ResumeLayout(false);
            this.gboTierNames.PerformLayout();
            this.gboModsQb.ResumeLayout(false);
            this.gboModsQb.PerformLayout();
            this.tabModsAudio.ResumeLayout(false);
            this.tabAudio.ResumeLayout(false);
            this.tabImportAudio.ResumeLayout(false);
            this.tabImportAudio.PerformLayout();
            this.tabAdjustVolume.ResumeLayout(false);
            this.tabAdjustVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trk)).EndInit();
            this.gboPreview.ResumeLayout(false);
            this.gboPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewFade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPreviewStart)).EndInit();
            this.mnuAudio.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboMods;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabModsQb;
        private System.Windows.Forms.TabPage tabModsAudio;
        private System.Windows.Forms.CheckBox chkUnlockSetlists;
        private System.Windows.Forms.CheckBox chkCompleteTier1Song;
        private System.Windows.Forms.CheckBox chkAddNonCareerTracksToBonus;
        private System.Windows.Forms.CheckBox chkSetCheats;
        private System.Windows.Forms.CheckBox chkFreeStore;
        private System.Windows.Forms.GroupBox gboModsQb;
        private System.Windows.Forms.CheckBox chkDefaultBonusSongArt;
        private System.Windows.Forms.GroupBox gboTierNames;
        private System.Windows.Forms.Label lblModsQb;
        private System.Windows.Forms.ListBox lstTierNames;
        private System.Windows.Forms.Label lblTierName;
        private System.Windows.Forms.Button btnTierName;
        private System.Windows.Forms.TextBox txtTierName;
        private System.Windows.Forms.ListView lvwAudio;
        private System.Windows.Forms.GroupBox gboPreview;
        private System.Windows.Forms.NumericUpDown udPreviewVolume;
        private System.Windows.Forms.NumericUpDown udPreviewFade;
        private System.Windows.Forms.NumericUpDown udPreviewLength;
        private System.Windows.Forms.NumericUpDown udPreviewStart;
        private System.Windows.Forms.Label lblPreviewVolume;
        private System.Windows.Forms.Label lblPreviewStart;
        private System.Windows.Forms.Button btnPreviewPlay;
        private System.Windows.Forms.Label lblPreviewLength;
        private System.Windows.Forms.Label lblPreviewFade;
        private System.Windows.Forms.HScrollBar hscPreviewStart;
        private System.Windows.Forms.ListView lvwFiles;
        private System.Windows.Forms.ColumnHeader hdrName;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.Timer tmrPreview;
        private System.Windows.Forms.Label lblClicking;
        private System.Windows.Forms.ContextMenuStrip mnuAudio;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveAudio;
        private System.Windows.Forms.ToolStripMenuItem mnuPlayAudio;
        private System.Windows.Forms.TextBox txtDefaultBonusSongInfoText;
        private System.Windows.Forms.CheckBox chkDefaultBonusSongInfoText;
        private System.Windows.Forms.TabControl tabAudio;
        private System.Windows.Forms.TabPage tabImportAudio;
        private System.Windows.Forms.TabPage tabAdjustVolume;
        private System.Windows.Forms.CheckBox chkVolumeApplyAll;
        private System.Windows.Forms.ListView lstVolume;
        private System.Windows.Forms.ColumnHeader hdrColAudio;
        private System.Windows.Forms.ColumnHeader hdrColVolume;
        private System.Windows.Forms.NumericUpDown udVolume;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.ImageList imgSmall;
        private System.Windows.Forms.Timer tmrGreyHack;
        private System.Windows.Forms.TrackBar trk;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblNormalise;
    }
}
