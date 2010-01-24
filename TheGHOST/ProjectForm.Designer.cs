namespace Nanook.TheGhost
{
    partial class ProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.gboProject = new System.Windows.Forms.GroupBox();
            this.rdoBrowse = new System.Windows.Forms.RadioButton();
            this.cboRecent = new System.Windows.Forms.ComboBox();
            this.rdoRecent = new System.Windows.Forms.RadioButton();
            this.rdoCreate = new System.Windows.Forms.RadioButton();
            this.lblTag = new System.Windows.Forms.Label();
            this.lblGhost = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.open = new System.Windows.Forms.OpenFileDialog();
            this.create = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.gboProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(6, 1);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(209, 59);
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // gboProject
            // 
            this.gboProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboProject.Controls.Add(this.rdoBrowse);
            this.gboProject.Controls.Add(this.cboRecent);
            this.gboProject.Controls.Add(this.rdoRecent);
            this.gboProject.Controls.Add(this.rdoCreate);
            this.gboProject.Location = new System.Drawing.Point(10, 65);
            this.gboProject.Name = "gboProject";
            this.gboProject.Size = new System.Drawing.Size(389, 143);
            this.gboProject.TabIndex = 2;
            this.gboProject.TabStop = false;
            this.gboProject.Text = "Project";
            // 
            // rdoBrowse
            // 
            this.rdoBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoBrowse.AutoSize = true;
            this.rdoBrowse.Location = new System.Drawing.Point(20, 59);
            this.rdoBrowse.Name = "rdoBrowse";
            this.rdoBrowse.Size = new System.Drawing.Size(120, 17);
            this.rdoBrowse.TabIndex = 1;
            this.rdoBrowse.TabStop = true;
            this.rdoBrowse.Text = "Browse for a Project";
            this.rdoBrowse.UseVisualStyleBackColor = true;
            // 
            // cboRecent
            // 
            this.cboRecent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRecent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecent.FormattingEnabled = true;
            this.cboRecent.Location = new System.Drawing.Point(39, 109);
            this.cboRecent.Name = "cboRecent";
            this.cboRecent.Size = new System.Drawing.Size(344, 21);
            this.cboRecent.TabIndex = 3;
            // 
            // rdoRecent
            // 
            this.rdoRecent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoRecent.AutoSize = true;
            this.rdoRecent.ForeColor = System.Drawing.Color.Transparent;
            this.rdoRecent.Location = new System.Drawing.Point(20, 87);
            this.rdoRecent.Name = "rdoRecent";
            this.rdoRecent.Size = new System.Drawing.Size(134, 17);
            this.rdoRecent.TabIndex = 2;
            this.rdoRecent.TabStop = true;
            this.rdoRecent.Text = "Open a Recent Project";
            this.rdoRecent.UseVisualStyleBackColor = true;
            this.rdoRecent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdoRecent_MouseDown);
            this.rdoRecent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rdoRecent_MouseUp);
            // 
            // rdoCreate
            // 
            this.rdoCreate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoCreate.AutoSize = true;
            this.rdoCreate.Location = new System.Drawing.Point(20, 31);
            this.rdoCreate.Name = "rdoCreate";
            this.rdoCreate.Size = new System.Drawing.Size(126, 17);
            this.rdoCreate.TabIndex = 0;
            this.rdoCreate.TabStop = true;
            this.rdoCreate.Text = "Create a New Project";
            this.rdoCreate.UseVisualStyleBackColor = true;
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Location = new System.Drawing.Point(238, 35);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(160, 13);
            this.lblTag.TabIndex = 1;
            this.lblTag.Text = "Guitar Hero Customs Made Easy";
            // 
            // lblGhost
            // 
            this.lblGhost.AutoSize = true;
            this.lblGhost.Font = new System.Drawing.Font("Verdana", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGhost.Location = new System.Drawing.Point(251, 9);
            this.lblGhost.Name = "lblGhost";
            this.lblGhost.Size = new System.Drawing.Size(137, 25);
            this.lblGhost.TabIndex = 0;
            this.lblGhost.Text = "TheGHOST";
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Location = new System.Drawing.Point(323, 214);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Continue...";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // ProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(409, 246);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblGhost);
            this.Controls.Add(this.lblTag);
            this.Controls.Add(this.gboProject);
            this.Controls.Add(this.picLogo);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TheGHOST";
            this.Load += new System.EventHandler(this.ProjectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.gboProject.ResumeLayout(false);
            this.gboProject.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.GroupBox gboProject;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.Label lblGhost;
        private System.Windows.Forms.ComboBox cboRecent;
        private System.Windows.Forms.RadioButton rdoRecent;
        private System.Windows.Forms.RadioButton rdoCreate;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.RadioButton rdoBrowse;
        private System.Windows.Forms.OpenFileDialog open;
        private System.Windows.Forms.SaveFileDialog create;
    }
}