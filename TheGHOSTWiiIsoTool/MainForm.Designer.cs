namespace Nanook.TheGhost
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtWorkingFolder = new System.Windows.Forms.TextBox();
            this.lblWorkingFolder = new System.Windows.Forms.Label();
            this.lblIso = new System.Windows.Forms.Label();
            this.txtIso = new System.Windows.Forms.TextBox();
            this.prg = new System.Windows.Forms.ProgressBar();
            this.btnPrepIso = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkRenameIso = new System.Windows.Forms.CheckBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPrep = new System.Windows.Forms.TabPage();
            this.btnPreset = new System.Windows.Forms.Button();
            this.cboPresets = new System.Windows.Forms.ComboBox();
            this.lblPresets = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.pgdPrep = new System.Windows.Forms.PropertyGrid();
            this.tabDiscId = new System.Windows.Forms.TabPage();
            this.lblIsoId = new System.Windows.Forms.Label();
            this.chkIsoId = new System.Windows.Forms.CheckBox();
            this.btnDiscIdGet = new System.Windows.Forms.Button();
            this.lblDiscIdTitle = new System.Windows.Forms.Label();
            this.btnDiscIdSet = new System.Windows.Forms.Button();
            this.lblDiscIdInfo = new System.Windows.Forms.Label();
            this.lblDiscId = new System.Windows.Forms.Label();
            this.txtDiscId = new System.Windows.Forms.TextBox();
            this.tabGameTitle = new System.Windows.Forms.TabPage();
            this.txtTitleDiscTitle = new System.Windows.Forms.TextBox();
            this.lblTitleDiscTitle = new System.Windows.Forms.Label();
            this.lblTitleSaveLengths = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTitleSaveText2 = new System.Windows.Forms.TextBox();
            this.txtTitleSaveText1 = new System.Windows.Forms.TextBox();
            this.lblTitleSaveText = new System.Windows.Forms.Label();
            this.txtTitleChannelText = new System.Windows.Forms.TextBox();
            this.lblTitleChannelText = new System.Windows.Forms.Label();
            this.btnTitleRead = new System.Windows.Forms.Button();
            this.btnTitleWrite = new System.Windows.Forms.Button();
            this.rdbGameGh3 = new System.Windows.Forms.RadioButton();
            this.rdbGameGhA = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGame = new System.Windows.Forms.Label();
            this.tabs.SuspendLayout();
            this.tabPrep.SuspendLayout();
            this.tabDiscId.SuspendLayout();
            this.tabGameTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWorkingFolder
            // 
            this.txtWorkingFolder.AllowDrop = true;
            this.txtWorkingFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkingFolder.Location = new System.Drawing.Point(96, 31);
            this.txtWorkingFolder.Name = "txtWorkingFolder";
            this.txtWorkingFolder.Size = new System.Drawing.Size(368, 20);
            this.txtWorkingFolder.TabIndex = 3;
            this.txtWorkingFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.textbox_DragDrop);
            this.txtWorkingFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.textbox_DragEnter);
            // 
            // lblWorkingFolder
            // 
            this.lblWorkingFolder.AutoSize = true;
            this.lblWorkingFolder.Location = new System.Drawing.Point(8, 34);
            this.lblWorkingFolder.Name = "lblWorkingFolder";
            this.lblWorkingFolder.Size = new System.Drawing.Size(82, 13);
            this.lblWorkingFolder.TabIndex = 2;
            this.lblWorkingFolder.Text = "Working Folder:";
            // 
            // lblIso
            // 
            this.lblIso.AutoSize = true;
            this.lblIso.Location = new System.Drawing.Point(8, 11);
            this.lblIso.Name = "lblIso";
            this.lblIso.Size = new System.Drawing.Size(28, 13);
            this.lblIso.TabIndex = 0;
            this.lblIso.Text = "ISO:";
            // 
            // txtIso
            // 
            this.txtIso.AllowDrop = true;
            this.txtIso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIso.Location = new System.Drawing.Point(96, 8);
            this.txtIso.Name = "txtIso";
            this.txtIso.Size = new System.Drawing.Size(368, 20);
            this.txtIso.TabIndex = 1;
            this.txtIso.DragDrop += new System.Windows.Forms.DragEventHandler(this.textbox_DragDrop);
            this.txtIso.DragEnter += new System.Windows.Forms.DragEventHandler(this.textbox_DragEnter);
            // 
            // prg
            // 
            this.prg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prg.Location = new System.Drawing.Point(3, 343);
            this.prg.Name = "prg";
            this.prg.Size = new System.Drawing.Size(371, 23);
            this.prg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prg.TabIndex = 10;
            // 
            // btnPrepIso
            // 
            this.btnPrepIso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrepIso.Location = new System.Drawing.Point(380, 343);
            this.btnPrepIso.Name = "btnPrepIso";
            this.btnPrepIso.Size = new System.Drawing.Size(75, 23);
            this.btnPrepIso.TabIndex = 12;
            this.btnPrepIso.Text = "Prep ISO";
            this.btnPrepIso.UseVisualStyleBackColor = true;
            this.btnPrepIso.Click += new System.EventHandler(this.btnPrepIso_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(0, 327);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(316, 13);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Select the prep options and click the Prep ISO button.";
            // 
            // chkRenameIso
            // 
            this.chkRenameIso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRenameIso.AutoSize = true;
            this.chkRenameIso.Checked = true;
            this.chkRenameIso.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRenameIso.Location = new System.Drawing.Point(373, 326);
            this.chkRenameIso.Name = "chkRenameIso";
            this.chkRenameIso.Size = new System.Drawing.Size(87, 17);
            this.chkRenameIso.TabIndex = 11;
            this.chkRenameIso.Text = "Rename ISO";
            this.chkRenameIso.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(413, 55);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(51, 21);
            this.btnHelp.TabIndex = 7;
            this.btnHelp.Text = "Help...";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabPrep);
            this.tabs.Controls.Add(this.tabDiscId);
            this.tabs.Controls.Add(this.tabGameTitle);
            this.tabs.Location = new System.Drawing.Point(3, 75);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(471, 394);
            this.tabs.TabIndex = 8;
            // 
            // tabPrep
            // 
            this.tabPrep.Controls.Add(this.btnPreset);
            this.tabPrep.Controls.Add(this.cboPresets);
            this.tabPrep.Controls.Add(this.lblPresets);
            this.tabPrep.Controls.Add(this.cboLanguage);
            this.tabPrep.Controls.Add(this.lblLanguage);
            this.tabPrep.Controls.Add(this.pgdPrep);
            this.tabPrep.Controls.Add(this.lblStatus);
            this.tabPrep.Controls.Add(this.prg);
            this.tabPrep.Controls.Add(this.chkRenameIso);
            this.tabPrep.Controls.Add(this.btnPrepIso);
            this.tabPrep.Location = new System.Drawing.Point(4, 22);
            this.tabPrep.Name = "tabPrep";
            this.tabPrep.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrep.Size = new System.Drawing.Size(463, 368);
            this.tabPrep.TabIndex = 0;
            this.tabPrep.Text = "Prep";
            this.tabPrep.UseVisualStyleBackColor = true;
            // 
            // btnPreset
            // 
            this.btnPreset.Location = new System.Drawing.Point(222, 2);
            this.btnPreset.Name = "btnPreset";
            this.btnPreset.Size = new System.Drawing.Size(31, 21);
            this.btnPreset.TabIndex = 25;
            this.btnPreset.Text = "Set";
            this.btnPreset.UseVisualStyleBackColor = true;
            this.btnPreset.Click += new System.EventHandler(this.btnPreset_Click);
            // 
            // cboPresets
            // 
            this.cboPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPresets.FormattingEnabled = true;
            this.cboPresets.Items.AddRange(new object[] {
            "Full Prep",
            "Compact ISO",
            "Only Add Songs",
            "Only Save Space"});
            this.cboPresets.Location = new System.Drawing.Point(54, 2);
            this.cboPresets.Name = "cboPresets";
            this.cboPresets.Size = new System.Drawing.Size(162, 21);
            this.cboPresets.TabIndex = 24;
            // 
            // lblPresets
            // 
            this.lblPresets.AutoSize = true;
            this.lblPresets.Location = new System.Drawing.Point(3, 7);
            this.lblPresets.Name = "lblPresets";
            this.lblPresets.Size = new System.Drawing.Size(45, 13);
            this.lblPresets.TabIndex = 23;
            this.lblPresets.Text = "Presets:";
            // 
            // cboLanguage
            // 
            this.cboLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Items.AddRange(new object[] {
            "English (Default)",
            "French",
            "German",
            "Italian",
            "Spanish"});
            this.cboLanguage.Location = new System.Drawing.Point(333, 2);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(130, 21);
            this.cboLanguage.TabIndex = 22;
            // 
            // lblLanguage
            // 
            this.lblLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(269, 5);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(58, 13);
            this.lblLanguage.TabIndex = 21;
            this.lblLanguage.Text = "Language:";
            // 
            // pgdPrep
            // 
            this.pgdPrep.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgdPrep.Location = new System.Drawing.Point(0, 26);
            this.pgdPrep.Name = "pgdPrep";
            this.pgdPrep.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pgdPrep.Size = new System.Drawing.Size(463, 298);
            this.pgdPrep.TabIndex = 20;
            this.pgdPrep.ToolbarVisible = false;
            // 
            // tabDiscId
            // 
            this.tabDiscId.Controls.Add(this.lblIsoId);
            this.tabDiscId.Controls.Add(this.chkIsoId);
            this.tabDiscId.Controls.Add(this.btnDiscIdGet);
            this.tabDiscId.Controls.Add(this.lblDiscIdTitle);
            this.tabDiscId.Controls.Add(this.btnDiscIdSet);
            this.tabDiscId.Controls.Add(this.lblDiscIdInfo);
            this.tabDiscId.Controls.Add(this.lblDiscId);
            this.tabDiscId.Controls.Add(this.txtDiscId);
            this.tabDiscId.Location = new System.Drawing.Point(4, 22);
            this.tabDiscId.Name = "tabDiscId";
            this.tabDiscId.Padding = new System.Windows.Forms.Padding(3);
            this.tabDiscId.Size = new System.Drawing.Size(463, 368);
            this.tabDiscId.TabIndex = 1;
            this.tabDiscId.Text = "Disc ID";
            this.tabDiscId.UseVisualStyleBackColor = true;
            // 
            // lblIsoId
            // 
            this.lblIsoId.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIsoId.Location = new System.Drawing.Point(6, 219);
            this.lblIsoId.Name = "lblIsoId";
            this.lblIsoId.Size = new System.Drawing.Size(451, 146);
            this.lblIsoId.TabIndex = 7;
            this.lblIsoId.Text = resources.GetString("lblIsoId.Text");
            // 
            // chkIsoId
            // 
            this.chkIsoId.AutoSize = true;
            this.chkIsoId.Location = new System.Drawing.Point(9, 197);
            this.chkIsoId.Name = "chkIsoId";
            this.chkIsoId.Size = new System.Drawing.Size(182, 17);
            this.chkIsoId.TabIndex = 6;
            this.chkIsoId.Text = "Modify ISO Disk ID (First 4 Bytes)";
            this.chkIsoId.UseVisualStyleBackColor = true;
            // 
            // btnDiscIdGet
            // 
            this.btnDiscIdGet.Location = new System.Drawing.Point(162, 28);
            this.btnDiscIdGet.Name = "btnDiscIdGet";
            this.btnDiscIdGet.Size = new System.Drawing.Size(48, 22);
            this.btnDiscIdGet.TabIndex = 3;
            this.btnDiscIdGet.Text = "Read";
            this.btnDiscIdGet.UseVisualStyleBackColor = true;
            this.btnDiscIdGet.Click += new System.EventHandler(this.btnDiscIdGet_Click);
            // 
            // lblDiscIdTitle
            // 
            this.lblDiscIdTitle.AutoSize = true;
            this.lblDiscIdTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscIdTitle.Location = new System.Drawing.Point(6, 5);
            this.lblDiscIdTitle.Name = "lblDiscIdTitle";
            this.lblDiscIdTitle.Size = new System.Drawing.Size(313, 13);
            this.lblDiscIdTitle.TabIndex = 0;
            this.lblDiscIdTitle.Text = "The Disc ID should be set after completing each disc.";
            // 
            // btnDiscIdSet
            // 
            this.btnDiscIdSet.Location = new System.Drawing.Point(214, 28);
            this.btnDiscIdSet.Name = "btnDiscIdSet";
            this.btnDiscIdSet.Size = new System.Drawing.Size(48, 22);
            this.btnDiscIdSet.TabIndex = 4;
            this.btnDiscIdSet.Text = "Write";
            this.btnDiscIdSet.UseVisualStyleBackColor = true;
            this.btnDiscIdSet.Click += new System.EventHandler(this.btnDiscIdSet_Click);
            // 
            // lblDiscIdInfo
            // 
            this.lblDiscIdInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDiscIdInfo.Location = new System.Drawing.Point(3, 64);
            this.lblDiscIdInfo.Name = "lblDiscIdInfo";
            this.lblDiscIdInfo.Size = new System.Drawing.Size(392, 123);
            this.lblDiscIdInfo.TabIndex = 5;
            this.lblDiscIdInfo.Text = resources.GetString("lblDiscIdInfo.Text");
            // 
            // lblDiscId
            // 
            this.lblDiscId.AutoSize = true;
            this.lblDiscId.Location = new System.Drawing.Point(6, 32);
            this.lblDiscId.Name = "lblDiscId";
            this.lblDiscId.Size = new System.Drawing.Size(45, 13);
            this.lblDiscId.TabIndex = 1;
            this.lblDiscId.Text = "Disc ID:";
            // 
            // txtDiscId
            // 
            this.txtDiscId.Location = new System.Drawing.Point(57, 29);
            this.txtDiscId.MaxLength = 4;
            this.txtDiscId.Name = "txtDiscId";
            this.txtDiscId.Size = new System.Drawing.Size(100, 20);
            this.txtDiscId.TabIndex = 2;
            // 
            // tabGameTitle
            // 
            this.tabGameTitle.Controls.Add(this.txtTitleDiscTitle);
            this.tabGameTitle.Controls.Add(this.lblTitleDiscTitle);
            this.tabGameTitle.Controls.Add(this.lblTitleSaveLengths);
            this.tabGameTitle.Controls.Add(this.label2);
            this.tabGameTitle.Controls.Add(this.txtTitleSaveText2);
            this.tabGameTitle.Controls.Add(this.txtTitleSaveText1);
            this.tabGameTitle.Controls.Add(this.lblTitleSaveText);
            this.tabGameTitle.Controls.Add(this.txtTitleChannelText);
            this.tabGameTitle.Controls.Add(this.lblTitleChannelText);
            this.tabGameTitle.Controls.Add(this.btnTitleRead);
            this.tabGameTitle.Controls.Add(this.btnTitleWrite);
            this.tabGameTitle.Location = new System.Drawing.Point(4, 22);
            this.tabGameTitle.Name = "tabGameTitle";
            this.tabGameTitle.Size = new System.Drawing.Size(463, 368);
            this.tabGameTitle.TabIndex = 2;
            this.tabGameTitle.Text = "Game Title";
            this.tabGameTitle.UseVisualStyleBackColor = true;
            // 
            // txtTitleDiscTitle
            // 
            this.txtTitleDiscTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleDiscTitle.Location = new System.Drawing.Point(6, 175);
            this.txtTitleDiscTitle.MaxLength = 68;
            this.txtTitleDiscTitle.Name = "txtTitleDiscTitle";
            this.txtTitleDiscTitle.Size = new System.Drawing.Size(451, 20);
            this.txtTitleDiscTitle.TabIndex = 7;
            // 
            // lblTitleDiscTitle
            // 
            this.lblTitleDiscTitle.AutoSize = true;
            this.lblTitleDiscTitle.Location = new System.Drawing.Point(3, 159);
            this.lblTitleDiscTitle.Name = "lblTitleDiscTitle";
            this.lblTitleDiscTitle.Size = new System.Drawing.Size(54, 13);
            this.lblTitleDiscTitle.TabIndex = 6;
            this.lblTitleDiscTitle.Text = "Disc Title:";
            // 
            // lblTitleSaveLengths
            // 
            this.lblTitleSaveLengths.AutoSize = true;
            this.lblTitleSaveLengths.Location = new System.Drawing.Point(87, 74);
            this.lblTitleSaveLengths.Name = "lblTitleSaveLengths";
            this.lblTitleSaveLengths.Size = new System.Drawing.Size(0, 13);
            this.lblTitleSaveLengths.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(1, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(294, 169);
            this.label2.TabIndex = 8;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // txtTitleSaveText2
            // 
            this.txtTitleSaveText2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleSaveText2.Location = new System.Drawing.Point(6, 112);
            this.txtTitleSaveText2.Name = "txtTitleSaveText2";
            this.txtTitleSaveText2.Size = new System.Drawing.Size(451, 20);
            this.txtTitleSaveText2.TabIndex = 5;
            // 
            // txtTitleSaveText1
            // 
            this.txtTitleSaveText1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleSaveText1.Location = new System.Drawing.Point(6, 90);
            this.txtTitleSaveText1.Name = "txtTitleSaveText1";
            this.txtTitleSaveText1.Size = new System.Drawing.Size(451, 20);
            this.txtTitleSaveText1.TabIndex = 4;
            // 
            // lblTitleSaveText
            // 
            this.lblTitleSaveText.AutoSize = true;
            this.lblTitleSaveText.Location = new System.Drawing.Point(3, 74);
            this.lblTitleSaveText.Name = "lblTitleSaveText";
            this.lblTitleSaveText.Size = new System.Drawing.Size(78, 13);
            this.lblTitleSaveText.TabIndex = 2;
            this.lblTitleSaveText.Text = "Save File Text:";
            // 
            // txtTitleChannelText
            // 
            this.txtTitleChannelText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitleChannelText.Location = new System.Drawing.Point(6, 25);
            this.txtTitleChannelText.MaxLength = 40;
            this.txtTitleChannelText.Name = "txtTitleChannelText";
            this.txtTitleChannelText.Size = new System.Drawing.Size(451, 20);
            this.txtTitleChannelText.TabIndex = 1;
            // 
            // lblTitleChannelText
            // 
            this.lblTitleChannelText.AutoSize = true;
            this.lblTitleChannelText.Location = new System.Drawing.Point(3, 9);
            this.lblTitleChannelText.Name = "lblTitleChannelText";
            this.lblTitleChannelText.Size = new System.Drawing.Size(97, 13);
            this.lblTitleChannelText.TabIndex = 0;
            this.lblTitleChannelText.Text = "Disc Channel Text:";
            // 
            // btnTitleRead
            // 
            this.btnTitleRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTitleRead.Location = new System.Drawing.Point(301, 202);
            this.btnTitleRead.Name = "btnTitleRead";
            this.btnTitleRead.Size = new System.Drawing.Size(75, 23);
            this.btnTitleRead.TabIndex = 9;
            this.btnTitleRead.Text = "Read Titles";
            this.btnTitleRead.UseVisualStyleBackColor = true;
            this.btnTitleRead.Click += new System.EventHandler(this.btnTitleRead_Click);
            // 
            // btnTitleWrite
            // 
            this.btnTitleWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTitleWrite.Location = new System.Drawing.Point(382, 202);
            this.btnTitleWrite.Name = "btnTitleWrite";
            this.btnTitleWrite.Size = new System.Drawing.Size(75, 23);
            this.btnTitleWrite.TabIndex = 10;
            this.btnTitleWrite.Text = "Write Titles";
            this.btnTitleWrite.UseVisualStyleBackColor = true;
            this.btnTitleWrite.Click += new System.EventHandler(this.btnTitleWrite_Click);
            // 
            // rdbGameGh3
            // 
            this.rdbGameGh3.AutoSize = true;
            this.rdbGameGh3.Location = new System.Drawing.Point(95, 55);
            this.rdbGameGh3.Name = "rdbGameGh3";
            this.rdbGameGh3.Size = new System.Drawing.Size(65, 17);
            this.rdbGameGh3.TabIndex = 5;
            this.rdbGameGh3.Text = "Wii GH3";
            this.rdbGameGh3.UseVisualStyleBackColor = true;
            this.rdbGameGh3.CheckedChanged += new System.EventHandler(this.rdbGameGh3_CheckedChanged);
            // 
            // rdbGameGhA
            // 
            this.rdbGameGhA.AutoSize = true;
            this.rdbGameGhA.Location = new System.Drawing.Point(171, 55);
            this.rdbGameGhA.Name = "rdbGameGhA";
            this.rdbGameGhA.Size = new System.Drawing.Size(108, 17);
            this.rdbGameGhA.TabIndex = 6;
            this.rdbGameGhA.Text = "Wii GH Aerosmith";
            this.rdbGameGhA.UseVisualStyleBackColor = true;
            this.rdbGameGhA.CheckedChanged += new System.EventHandler(this.rdbGameGhA_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 472);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(314, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Thanks to Dack for WiiScrubber and the Disc ID changing code.";
            // 
            // lblGame
            // 
            this.lblGame.AutoSize = true;
            this.lblGame.Location = new System.Drawing.Point(8, 57);
            this.lblGame.Name = "lblGame";
            this.lblGame.Size = new System.Drawing.Size(38, 13);
            this.lblGame.TabIndex = 4;
            this.lblGame.Text = "Game:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 489);
            this.Controls.Add(this.rdbGameGhA);
            this.Controls.Add(this.rdbGameGh3);
            this.Controls.Add(this.lblGame);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.txtWorkingFolder);
            this.Controls.Add(this.lblWorkingFolder);
            this.Controls.Add(this.lblIso);
            this.Controls.Add(this.txtIso);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TheGHOST Wii ISO Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabs.ResumeLayout(false);
            this.tabPrep.ResumeLayout(false);
            this.tabPrep.PerformLayout();
            this.tabDiscId.ResumeLayout(false);
            this.tabDiscId.PerformLayout();
            this.tabGameTitle.ResumeLayout(false);
            this.tabGameTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWorkingFolder;
        private System.Windows.Forms.Label lblWorkingFolder;
        private System.Windows.Forms.Label lblIso;
        private System.Windows.Forms.TextBox txtIso;
        private System.Windows.Forms.ProgressBar prg;
        private System.Windows.Forms.Button btnPrepIso;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkRenameIso;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabPrep;
        private System.Windows.Forms.TabPage tabDiscId;
        private System.Windows.Forms.RadioButton rdbGameGhA;
        private System.Windows.Forms.RadioButton rdbGameGh3;
        private System.Windows.Forms.Label lblDiscIdInfo;
        private System.Windows.Forms.Label lblDiscId;
        private System.Windows.Forms.TextBox txtDiscId;
        private System.Windows.Forms.Label lblDiscIdTitle;
        private System.Windows.Forms.Button btnDiscIdSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGame;
        private System.Windows.Forms.TabPage tabGameTitle;
        private System.Windows.Forms.Button btnTitleWrite;
        private System.Windows.Forms.Button btnTitleRead;
        private System.Windows.Forms.TextBox txtTitleSaveText1;
        private System.Windows.Forms.Label lblTitleSaveText;
        private System.Windows.Forms.TextBox txtTitleChannelText;
        private System.Windows.Forms.Label lblTitleChannelText;
        private System.Windows.Forms.TextBox txtTitleSaveText2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTitleSaveLengths;
        private System.Windows.Forms.PropertyGrid pgdPrep;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cboPresets;
        private System.Windows.Forms.Label lblPresets;
        private System.Windows.Forms.Button btnPreset;
        private System.Windows.Forms.Button btnDiscIdGet;
        private System.Windows.Forms.CheckBox chkIsoId;
        private System.Windows.Forms.Label lblIsoId;
        private System.Windows.Forms.TextBox txtTitleDiscTitle;
        private System.Windows.Forms.Label lblTitleDiscTitle;
    }
}

