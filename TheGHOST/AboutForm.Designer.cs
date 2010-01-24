namespace Nanook.TheGhost
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.btnContinue = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitleTag = new System.Windows.Forms.Label();
            this.lblGreets = new System.Windows.Forms.Label();
            this.lblTeamGhost = new System.Windows.Forms.Label();
            this.lblVersionAppTitle = new System.Windows.Forms.Label();
            this.lblVersionCoreTitle = new System.Windows.Forms.Label();
            this.lblVersionApp = new System.Windows.Forms.Label();
            this.lblVersionCore = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lnkVisit = new System.Windows.Forms.LinkLabel();
            this.lblVisit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Location = new System.Drawing.Point(318, 279);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 8;
            this.btnContinue.Text = "Close";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Verdana", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(246, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(137, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TheGHOST";
            // 
            // lblTitleTag
            // 
            this.lblTitleTag.AutoSize = true;
            this.lblTitleTag.Location = new System.Drawing.Point(233, 38);
            this.lblTitleTag.Name = "lblTitleTag";
            this.lblTitleTag.Size = new System.Drawing.Size(160, 13);
            this.lblTitleTag.TabIndex = 1;
            this.lblTitleTag.Text = "Guitar Hero Customs Made Easy";
            // 
            // lblGreets
            // 
            this.lblGreets.AutoSize = true;
            this.lblGreets.Location = new System.Drawing.Point(21, 86);
            this.lblGreets.Name = "lblGreets";
            this.lblGreets.Size = new System.Drawing.Size(315, 130);
            this.lblGreets.TabIndex = 3;
            this.lblGreets.Text = resources.GetString("lblGreets.Text");
            // 
            // lblTeamGhost
            // 
            this.lblTeamGhost.AutoSize = true;
            this.lblTeamGhost.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTeamGhost.Location = new System.Drawing.Point(9, 68);
            this.lblTeamGhost.Name = "lblTeamGhost";
            this.lblTeamGhost.Size = new System.Drawing.Size(380, 18);
            this.lblTeamGhost.TabIndex = 2;
            this.lblTeamGhost.Text = "TheGHOST is brought to you by TeamGHOST:";
            // 
            // lblVersionAppTitle
            // 
            this.lblVersionAppTitle.AutoSize = true;
            this.lblVersionAppTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionAppTitle.Location = new System.Drawing.Point(21, 236);
            this.lblVersionAppTitle.Name = "lblVersionAppTitle";
            this.lblVersionAppTitle.Size = new System.Drawing.Size(120, 13);
            this.lblVersionAppTitle.TabIndex = 4;
            this.lblVersionAppTitle.Text = "Application Version:";
            // 
            // lblVersionCoreTitle
            // 
            this.lblVersionCoreTitle.AutoSize = true;
            this.lblVersionCoreTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionCoreTitle.Location = new System.Drawing.Point(21, 251);
            this.lblVersionCoreTitle.Name = "lblVersionCoreTitle";
            this.lblVersionCoreTitle.Size = new System.Drawing.Size(83, 13);
            this.lblVersionCoreTitle.TabIndex = 6;
            this.lblVersionCoreTitle.Text = "Core Version:";
            // 
            // lblVersionApp
            // 
            this.lblVersionApp.AutoSize = true;
            this.lblVersionApp.Location = new System.Drawing.Point(146, 236);
            this.lblVersionApp.Name = "lblVersionApp";
            this.lblVersionApp.Size = new System.Drawing.Size(53, 13);
            this.lblVersionApp.TabIndex = 5;
            this.lblVersionApp.Text = "<version>";
            // 
            // lblVersionCore
            // 
            this.lblVersionCore.AutoSize = true;
            this.lblVersionCore.Location = new System.Drawing.Point(146, 251);
            this.lblVersionCore.Name = "lblVersionCore";
            this.lblVersionCore.Size = new System.Drawing.Size(53, 13);
            this.lblVersionCore.TabIndex = 7;
            this.lblVersionCore.Text = "<version>";
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(7, 4);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(202, 59);
            this.picLogo.TabIndex = 4;
            this.picLogo.TabStop = false;
            // 
            // lnkVisit
            // 
            this.lnkVisit.AutoSize = true;
            this.lnkVisit.DisabledLinkColor = System.Drawing.Color.Silver;
            this.lnkVisit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkVisit.LinkColor = System.Drawing.Color.White;
            this.lnkVisit.Location = new System.Drawing.Point(62, 289);
            this.lnkVisit.Name = "lnkVisit";
            this.lnkVisit.Size = new System.Drawing.Size(220, 13);
            this.lnkVisit.TabIndex = 9;
            this.lnkVisit.TabStop = true;
            this.lnkVisit.Text = "http://the-ghost-project.blogspot.com";
            this.lnkVisit.VisitedLinkColor = System.Drawing.Color.White;
            this.lnkVisit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkVisit_LinkClicked);
            // 
            // lblVisit
            // 
            this.lblVisit.AutoSize = true;
            this.lblVisit.Location = new System.Drawing.Point(21, 289);
            this.lblVisit.Name = "lblVisit";
            this.lblVisit.Size = new System.Drawing.Size(43, 13);
            this.lblVisit.TabIndex = 10;
            this.lblVisit.Text = "Visit us:";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(405, 314);
            this.ControlBox = false;
            this.Controls.Add(this.lblVisit);
            this.Controls.Add(this.lnkVisit);
            this.Controls.Add(this.lblVersionCore);
            this.Controls.Add(this.lblVersionApp);
            this.Controls.Add(this.lblVersionCoreTitle);
            this.Controls.Add(this.lblVersionAppTitle);
            this.Controls.Add(this.lblTeamGhost);
            this.Controls.Add(this.lblGreets);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTitleTag);
            this.Controls.Add(this.picLogo);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTitleTag;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblGreets;
        private System.Windows.Forms.Label lblTeamGhost;
        private System.Windows.Forms.Label lblVersionAppTitle;
        private System.Windows.Forms.Label lblVersionCoreTitle;
        private System.Windows.Forms.Label lblVersionApp;
        private System.Windows.Forms.Label lblVersionCore;
        private System.Windows.Forms.LinkLabel lnkVisit;
        private System.Windows.Forms.Label lblVisit;

    }
}