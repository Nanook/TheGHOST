namespace Nanook.TheGhost
{
    partial class NotesEditor
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
            if (disposing && (_audioPlayer != null))
            {
                _audioPlayer.Dispose();
                _audioPlayer = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotesEditor));
            this.scrl = new System.Windows.Forms.HScrollBar();
            this.btnPlay = new System.Windows.Forms.Button();
            this.udNotesOffset = new System.Windows.Forms.NumericUpDown();
            this.udFretsOffset = new System.Windows.Forms.NumericUpDown();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lvw = new System.Windows.Forms.ListView();
            this.hdrGhName = new System.Windows.Forms.ColumnHeader();
            this.hdrGhOffset = new System.Windows.Forms.ColumnHeader();
            this.hdrGhButtons = new System.Windows.Forms.ColumnHeader();
            this.hdrGhStarPower = new System.Windows.Forms.ColumnHeader();
            this.hdrGhNotes = new System.Windows.Forms.ColumnHeader();
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.gboNotes = new System.Windows.Forms.GroupBox();
            this.lblFps = new System.Windows.Forms.Label();
            this.chkGh3SustainClipping = new System.Windows.Forms.CheckBox();
            this.chkOffsetChecked = new System.Windows.Forms.CheckBox();
            this.lblNotesOffset = new System.Windows.Forms.Label();
            this.gboOptions = new System.Windows.Forms.GroupBox();
            this.lblOptHoPoInfo = new System.Windows.Forms.Label();
            this.lblOptHoPo = new System.Windows.Forms.Label();
            this.udHoPo = new System.Windows.Forms.NumericUpDown();
            this.lblOptOffsetInfo = new System.Windows.Forms.Label();
            this.lblOptOffset = new System.Windows.Forms.Label();
            this.ne = new Nanook.TheGhost.NotesEditorDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.udNotesOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFretsOffset)).BeginInit();
            this.gboNotes.SuspendLayout();
            this.gboOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udHoPo)).BeginInit();
            this.SuspendLayout();
            // 
            // scrl
            // 
            this.scrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scrl.Location = new System.Drawing.Point(0, 255);
            this.scrl.Name = "scrl";
            this.scrl.Size = new System.Drawing.Size(710, 19);
            this.scrl.TabIndex = 4;
            this.scrl.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrl_Scroll);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(242, 43);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(48, 23);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // udNotesOffset
            // 
            this.udNotesOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udNotesOffset.Location = new System.Drawing.Point(72, 18);
            this.udNotesOffset.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udNotesOffset.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.udNotesOffset.Name = "udNotesOffset";
            this.udNotesOffset.Size = new System.Drawing.Size(60, 20);
            this.udNotesOffset.TabIndex = 1;
            this.udNotesOffset.ValueChanged += new System.EventHandler(this.udNotesOffset_ValueChanged);
            // 
            // udFretsOffset
            // 
            this.udFretsOffset.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udFretsOffset.Location = new System.Drawing.Point(72, 18);
            this.udFretsOffset.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udFretsOffset.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.udFretsOffset.Name = "udFretsOffset";
            this.udFretsOffset.Size = new System.Drawing.Size(60, 20);
            this.udFretsOffset.TabIndex = 1;
            this.udFretsOffset.ValueChanged += new System.EventHandler(this.udFretsOffset_ValueChanged);
            // 
            // lblTimer
            // 
            this.lblTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimer.AutoEllipsis = true;
            this.lblTimer.Location = new System.Drawing.Point(66, 46);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(147, 18);
            this.lblTimer.TabIndex = 4;
            this.lblTimer.Text = "00000 / 00:00:00.000";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvw
            // 
            this.lvw.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvw.CheckBoxes = true;
            this.lvw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrGhName,
            this.hdrGhOffset,
            this.hdrGhButtons,
            this.hdrGhStarPower,
            this.hdrGhNotes});
            this.lvw.FullRowSelect = true;
            this.lvw.HideSelection = false;
            this.lvw.Location = new System.Drawing.Point(0, 0);
            this.lvw.MultiSelect = false;
            this.lvw.Name = "lvw";
            this.lvw.Size = new System.Drawing.Size(412, 170);
            this.lvw.SmallImageList = this.img;
            this.lvw.TabIndex = 0;
            this.lvw.UseCompatibleStateImageBehavior = false;
            this.lvw.View = System.Windows.Forms.View.Details;
            this.lvw.SelectedIndexChanged += new System.EventHandler(this.lvw_SelectedIndexChanged);
            // 
            // hdrGhName
            // 
            this.hdrGhName.Text = "Name";
            this.hdrGhName.Width = 172;
            // 
            // hdrGhOffset
            // 
            this.hdrGhOffset.Text = "Offset";
            this.hdrGhOffset.Width = 49;
            // 
            // hdrGhButtons
            // 
            this.hdrGhButtons.Text = "Buttons";
            this.hdrGhButtons.Width = 48;
            // 
            // hdrGhStarPower
            // 
            this.hdrGhStarPower.Text = "Star Power";
            this.hdrGhStarPower.Width = 48;
            // 
            // hdrGhNotes
            // 
            this.hdrGhNotes.Text = "Notes";
            this.hdrGhNotes.Width = 45;
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
            // gboNotes
            // 
            this.gboNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gboNotes.Controls.Add(this.lblFps);
            this.gboNotes.Controls.Add(this.chkOffsetChecked);
            this.gboNotes.Controls.Add(this.lblNotesOffset);
            this.gboNotes.Controls.Add(this.udNotesOffset);
            this.gboNotes.Controls.Add(this.lblTimer);
            this.gboNotes.Controls.Add(this.btnPlay);
            this.gboNotes.Location = new System.Drawing.Point(413, 98);
            this.gboNotes.Name = "gboNotes";
            this.gboNotes.Size = new System.Drawing.Size(296, 72);
            this.gboNotes.TabIndex = 2;
            this.gboNotes.TabStop = false;
            this.gboNotes.Text = "Notes Name";
            // 
            // lblFps
            // 
            this.lblFps.AutoSize = true;
            this.lblFps.Location = new System.Drawing.Point(5, 49);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(54, 13);
            this.lblFps.TabIndex = 6;
            this.lblFps.Text = "FPS:  000";
            // 
            // chkGh3SustainClipping
            // 
            this.chkGh3SustainClipping.AutoSize = true;
            this.chkGh3SustainClipping.Checked = true;
            this.chkGh3SustainClipping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGh3SustainClipping.Location = new System.Drawing.Point(9, 68);
            this.chkGh3SustainClipping.Name = "chkGh3SustainClipping";
            this.chkGh3SustainClipping.Size = new System.Drawing.Size(129, 17);
            this.chkGh3SustainClipping.TabIndex = 5;
            this.chkGh3SustainClipping.Text = "GH 3 Sustain Clipping";
            this.chkGh3SustainClipping.UseVisualStyleBackColor = true;
            this.chkGh3SustainClipping.CheckedChanged += new System.EventHandler(this.chkGh3SustainClipping_CheckedChanged);
            // 
            // chkOffsetChecked
            // 
            this.chkOffsetChecked.AutoSize = true;
            this.chkOffsetChecked.Location = new System.Drawing.Point(152, 19);
            this.chkOffsetChecked.Name = "chkOffsetChecked";
            this.chkOffsetChecked.Size = new System.Drawing.Size(138, 17);
            this.chkOffsetChecked.TabIndex = 3;
            this.chkOffsetChecked.Text = "Apply to Checked Items";
            this.chkOffsetChecked.UseVisualStyleBackColor = true;
            // 
            // lblNotesOffset
            // 
            this.lblNotesOffset.AutoSize = true;
            this.lblNotesOffset.Location = new System.Drawing.Point(6, 22);
            this.lblNotesOffset.Name = "lblNotesOffset";
            this.lblNotesOffset.Size = new System.Drawing.Size(38, 13);
            this.lblNotesOffset.TabIndex = 0;
            this.lblNotesOffset.Text = "Offset:";
            // 
            // gboOptions
            // 
            this.gboOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboOptions.Controls.Add(this.lblOptHoPoInfo);
            this.gboOptions.Controls.Add(this.chkGh3SustainClipping);
            this.gboOptions.Controls.Add(this.lblOptHoPo);
            this.gboOptions.Controls.Add(this.udHoPo);
            this.gboOptions.Controls.Add(this.lblOptOffsetInfo);
            this.gboOptions.Controls.Add(this.lblOptOffset);
            this.gboOptions.Controls.Add(this.udFretsOffset);
            this.gboOptions.Location = new System.Drawing.Point(413, 0);
            this.gboOptions.Name = "gboOptions";
            this.gboOptions.Size = new System.Drawing.Size(296, 92);
            this.gboOptions.TabIndex = 1;
            this.gboOptions.TabStop = false;
            this.gboOptions.Text = "Options";
            // 
            // lblOptHoPoInfo
            // 
            this.lblOptHoPoInfo.AutoSize = true;
            this.lblOptHoPoInfo.Location = new System.Drawing.Point(141, 47);
            this.lblOptHoPoInfo.Name = "lblOptHoPoInfo";
            this.lblOptHoPoInfo.Size = new System.Drawing.Size(146, 13);
            this.lblOptHoPoInfo.TabIndex = 9;
            this.lblOptHoPoInfo.Text = "2.95 is just under 1/3 of a fret";
            // 
            // lblOptHoPo
            // 
            this.lblOptHoPo.AutoSize = true;
            this.lblOptHoPo.Location = new System.Drawing.Point(6, 47);
            this.lblOptHoPo.Name = "lblOptHoPo";
            this.lblOptHoPo.Size = new System.Drawing.Size(55, 13);
            this.lblOptHoPo.TabIndex = 8;
            this.lblOptHoPo.Text = "H.O./P.O:";
            // 
            // udHoPo
            // 
            this.udHoPo.Location = new System.Drawing.Point(72, 43);
            this.udHoPo.Name = "udHoPo";
            this.udHoPo.Size = new System.Drawing.Size(60, 20);
            this.udHoPo.TabIndex = 7;
            this.udHoPo.ValueChanged += new System.EventHandler(this.udHoPo_ValueChanged);
            // 
            // lblOptOffsetInfo
            // 
            this.lblOptOffsetInfo.AutoSize = true;
            this.lblOptOffsetInfo.Location = new System.Drawing.Point(141, 21);
            this.lblOptOffsetInfo.Name = "lblOptOffsetInfo";
            this.lblOptOffsetInfo.Size = new System.Drawing.Size(132, 13);
            this.lblOptOffsetInfo.TabIndex = 2;
            this.lblOptOffsetInfo.Text = "Applies to Frets && Sections";
            // 
            // lblOptOffset
            // 
            this.lblOptOffset.AutoSize = true;
            this.lblOptOffset.Location = new System.Drawing.Point(3, 22);
            this.lblOptOffset.Name = "lblOptOffset";
            this.lblOptOffset.Size = new System.Drawing.Size(41, 13);
            this.lblOptOffset.TabIndex = 0;
            this.lblOptOffset.Text = " Offset:";
            // 
            // ne
            // 
            this.ne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ne.BackColor = System.Drawing.Color.Black;
            this.ne.Location = new System.Drawing.Point(0, 171);
            this.ne.Name = "ne";
            this.ne.Size = new System.Drawing.Size(710, 84);
            this.ne.TabIndex = 3;
            // 
            // NotesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboOptions);
            this.Controls.Add(this.gboNotes);
            this.Controls.Add(this.lvw);
            this.Controls.Add(this.scrl);
            this.Controls.Add(this.ne);
            this.MinimumSize = new System.Drawing.Size(400, 274);
            this.Name = "NotesEditor";
            this.Size = new System.Drawing.Size(710, 274);
            ((System.ComponentModel.ISupportInitialize)(this.udNotesOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udFretsOffset)).EndInit();
            this.gboNotes.ResumeLayout(false);
            this.gboNotes.PerformLayout();
            this.gboOptions.ResumeLayout(false);
            this.gboOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udHoPo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NotesEditorDisplay ne;
        private System.Windows.Forms.HScrollBar scrl;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.NumericUpDown udNotesOffset;
        private System.Windows.Forms.NumericUpDown udFretsOffset;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.ListView lvw;
        private System.Windows.Forms.GroupBox gboNotes;
        private System.Windows.Forms.GroupBox gboOptions;
        private System.Windows.Forms.CheckBox chkOffsetChecked;
        private System.Windows.Forms.Label lblNotesOffset;
        private System.Windows.Forms.Label lblOptOffset;
        private System.Windows.Forms.ImageList img;
        private System.Windows.Forms.ColumnHeader hdrGhName;
        private System.Windows.Forms.ColumnHeader hdrGhOffset;
        private System.Windows.Forms.ColumnHeader hdrGhButtons;
        private System.Windows.Forms.ColumnHeader hdrGhStarPower;
        private System.Windows.Forms.ColumnHeader hdrGhNotes;
        private System.Windows.Forms.Label lblOptOffsetInfo;
        private System.Windows.Forms.CheckBox chkGh3SustainClipping;
        private System.Windows.Forms.Label lblOptHoPo;
        private System.Windows.Forms.NumericUpDown udHoPo;
        private System.Windows.Forms.Label lblOptHoPoInfo;
        private System.Windows.Forms.Label lblFps;
    }
}
