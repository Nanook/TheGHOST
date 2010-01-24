namespace Nanook.TheGhost.AudioTool
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblMp3 = new System.Windows.Forms.Label();
            this.lblOgg = new System.Windows.Forms.Label();
            this.lblWavDest = new System.Windows.Forms.Label();
            this.lblXBADPCM = new System.Windows.Forms.Label();
            this.lblComplete = new System.Windows.Forms.Label();
            this.imgXbadPcm = new System.Windows.Forms.PictureBox();
            this.imgWavDest = new System.Windows.Forms.PictureBox();
            this.imgOgg = new System.Windows.Forms.PictureBox();
            this.imgMP3 = new System.Windows.Forms.PictureBox();
            this.btnInstallWavDest = new System.Windows.Forms.Button();
            this.btnInstallXbadpcm = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgXbadPcm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgWavDest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgOgg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMP3)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Test the system configuration is correct for running TheGHOST. Click Begin Tests " +
                "to start";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(14, 138);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(287, 25);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Begin Tests!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblMp3
            // 
            this.lblMp3.Enabled = false;
            this.lblMp3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMp3.Location = new System.Drawing.Point(87, 181);
            this.lblMp3.Name = "lblMp3";
            this.lblMp3.Size = new System.Drawing.Size(214, 45);
            this.lblMp3.TabIndex = 3;
            this.lblMp3.Text = "MP3 Playback";
            this.lblMp3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOgg
            // 
            this.lblOgg.Enabled = false;
            this.lblOgg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOgg.Location = new System.Drawing.Point(87, 232);
            this.lblOgg.Name = "lblOgg";
            this.lblOgg.Size = new System.Drawing.Size(214, 45);
            this.lblOgg.TabIndex = 5;
            this.lblOgg.Text = "OGG Playback";
            this.lblOgg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWavDest
            // 
            this.lblWavDest.Enabled = false;
            this.lblWavDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWavDest.Location = new System.Drawing.Point(87, 283);
            this.lblWavDest.Name = "lblWavDest";
            this.lblWavDest.Size = new System.Drawing.Size(214, 45);
            this.lblWavDest.TabIndex = 7;
            this.lblWavDest.Text = "Wav Dest Filter";
            this.lblWavDest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblXBADPCM
            // 
            this.lblXBADPCM.Enabled = false;
            this.lblXBADPCM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXBADPCM.Location = new System.Drawing.Point(87, 334);
            this.lblXBADPCM.Name = "lblXBADPCM";
            this.lblXBADPCM.Size = new System.Drawing.Size(214, 45);
            this.lblXBADPCM.TabIndex = 9;
            this.lblXBADPCM.Text = "XBADPCM Codec";
            this.lblXBADPCM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblComplete
            // 
            this.lblComplete.Location = new System.Drawing.Point(1, 386);
            this.lblComplete.Name = "lblComplete";
            this.lblComplete.Size = new System.Drawing.Size(311, 102);
            this.lblComplete.TabIndex = 10;
            this.lblComplete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblComplete.Visible = false;
            // 
            // imgXbadPcm
            // 
            this.imgXbadPcm.Enabled = false;
            this.imgXbadPcm.Image = global::Nanook.TheGhost.AudioTool.Properties.Resources.GreenTick;
            this.imgXbadPcm.Location = new System.Drawing.Point(17, 334);
            this.imgXbadPcm.Name = "imgXbadPcm";
            this.imgXbadPcm.Size = new System.Drawing.Size(54, 45);
            this.imgXbadPcm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgXbadPcm.TabIndex = 8;
            this.imgXbadPcm.TabStop = false;
            this.imgXbadPcm.Visible = false;
            // 
            // imgWavDest
            // 
            this.imgWavDest.Enabled = false;
            this.imgWavDest.Image = global::Nanook.TheGhost.AudioTool.Properties.Resources.GreenTick;
            this.imgWavDest.Location = new System.Drawing.Point(17, 283);
            this.imgWavDest.Name = "imgWavDest";
            this.imgWavDest.Size = new System.Drawing.Size(54, 45);
            this.imgWavDest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgWavDest.TabIndex = 6;
            this.imgWavDest.TabStop = false;
            this.imgWavDest.Visible = false;
            // 
            // imgOgg
            // 
            this.imgOgg.Enabled = false;
            this.imgOgg.Image = global::Nanook.TheGhost.AudioTool.Properties.Resources.GreenTick;
            this.imgOgg.Location = new System.Drawing.Point(17, 232);
            this.imgOgg.Name = "imgOgg";
            this.imgOgg.Size = new System.Drawing.Size(54, 45);
            this.imgOgg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgOgg.TabIndex = 4;
            this.imgOgg.TabStop = false;
            this.imgOgg.Visible = false;
            // 
            // imgMP3
            // 
            this.imgMP3.Enabled = false;
            this.imgMP3.Image = global::Nanook.TheGhost.AudioTool.Properties.Resources.GreenTick;
            this.imgMP3.Location = new System.Drawing.Point(17, 181);
            this.imgMP3.Name = "imgMP3";
            this.imgMP3.Size = new System.Drawing.Size(54, 45);
            this.imgMP3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgMP3.TabIndex = 2;
            this.imgMP3.TabStop = false;
            this.imgMP3.Visible = false;
            // 
            // btnInstallWavDest
            // 
            this.btnInstallWavDest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstallWavDest.Location = new System.Drawing.Point(17, 45);
            this.btnInstallWavDest.Name = "btnInstallWavDest";
            this.btnInstallWavDest.Size = new System.Drawing.Size(284, 23);
            this.btnInstallWavDest.TabIndex = 11;
            this.btnInstallWavDest.Text = "Install Wav Dest Filter";
            this.btnInstallWavDest.UseVisualStyleBackColor = true;
            this.btnInstallWavDest.Click += new System.EventHandler(this.btnInstallWavDest_Click);
            // 
            // btnInstallXbadpcm
            // 
            this.btnInstallXbadpcm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstallXbadpcm.Location = new System.Drawing.Point(17, 74);
            this.btnInstallXbadpcm.Name = "btnInstallXbadpcm";
            this.btnInstallXbadpcm.Size = new System.Drawing.Size(284, 23);
            this.btnInstallXbadpcm.TabIndex = 12;
            this.btnInstallXbadpcm.Text = "Install XBADPCM Codec";
            this.btnInstallXbadpcm.UseVisualStyleBackColor = true;
            this.btnInstallXbadpcm.Click += new System.EventHandler(this.btnInstallXbadpcm_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 42);
            this.label2.TabIndex = 13;
            this.label2.Text = "This application is only required when using the \'Windows Audio\' or \'XBADPCM\' Plu" +
                "gins as they use Direct Show.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 487);
            this.Controls.Add(this.btnInstallXbadpcm);
            this.Controls.Add(this.btnInstallWavDest);
            this.Controls.Add(this.lblComplete);
            this.Controls.Add(this.lblXBADPCM);
            this.Controls.Add(this.imgXbadPcm);
            this.Controls.Add(this.lblWavDest);
            this.Controls.Add(this.imgWavDest);
            this.Controls.Add(this.lblOgg);
            this.Controls.Add(this.imgOgg);
            this.Controls.Add(this.lblMp3);
            this.Controls.Add(this.imgMP3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TheGHOST Audio Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.imgXbadPcm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgWavDest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgOgg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMP3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox imgMP3;
        private System.Windows.Forms.Label lblMp3;
        private System.Windows.Forms.PictureBox imgOgg;
        private System.Windows.Forms.Label lblOgg;
        private System.Windows.Forms.PictureBox imgWavDest;
        private System.Windows.Forms.Label lblWavDest;
        private System.Windows.Forms.PictureBox imgXbadPcm;
        private System.Windows.Forms.Label lblXBADPCM;
        private System.Windows.Forms.Label lblComplete;
        private System.Windows.Forms.Button btnInstallWavDest;
        private System.Windows.Forms.Button btnInstallXbadpcm;
        private System.Windows.Forms.Label label2;
    }
}