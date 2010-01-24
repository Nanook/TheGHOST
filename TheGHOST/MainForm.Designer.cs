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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnMoveNext = new System.Windows.Forms.Button();
            this.btnMoveBack = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblScreenMessage = new System.Windows.Forms.Label();
            this.save = new System.Windows.Forms.SaveFileDialog();
            this.btnGreetz = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.mnuMove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.projectScreen = new Nanook.TheGhost.ProjectScreen();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(506, 544);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnMoveNext
            // 
            this.btnMoveNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveNext.Location = new System.Drawing.Point(412, 544);
            this.btnMoveNext.Name = "btnMoveNext";
            this.btnMoveNext.Size = new System.Drawing.Size(75, 23);
            this.btnMoveNext.TabIndex = 5;
            this.btnMoveNext.Text = "Next >>";
            this.btnMoveNext.UseVisualStyleBackColor = true;
            this.btnMoveNext.Click += new System.EventHandler(this.btnMoveNext_Click);
            // 
            // btnMoveBack
            // 
            this.btnMoveBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveBack.Location = new System.Drawing.Point(311, 544);
            this.btnMoveBack.Name = "btnMoveBack";
            this.btnMoveBack.Size = new System.Drawing.Size(75, 23);
            this.btnMoveBack.TabIndex = 3;
            this.btnMoveBack.Text = "<< Back";
            this.btnMoveBack.UseVisualStyleBackColor = true;
            this.btnMoveBack.Click += new System.EventHandler(this.btnMoveBack_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.BackColor = System.Drawing.Color.Black;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.picLogo);
            this.pnlHeader.Controls.Add(this.lblScreenMessage);
            this.pnlHeader.Location = new System.Drawing.Point(1, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(589, 58);
            this.pnlHeader.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picLogo.BackgroundImage")));
            this.picLogo.Location = new System.Drawing.Point(2, -1);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(196, 57);
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            // 
            // lblScreenMessage
            // 
            this.lblScreenMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScreenMessage.ForeColor = System.Drawing.Color.White;
            this.lblScreenMessage.Location = new System.Drawing.Point(203, 4);
            this.lblScreenMessage.Name = "lblScreenMessage";
            this.lblScreenMessage.Size = new System.Drawing.Size(380, 48);
            this.lblScreenMessage.TabIndex = 0;
            this.lblScreenMessage.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btnGreetz
            // 
            this.btnGreetz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGreetz.Location = new System.Drawing.Point(12, 544);
            this.btnGreetz.Name = "btnGreetz";
            this.btnGreetz.Size = new System.Drawing.Size(75, 23);
            this.btnGreetz.TabIndex = 1;
            this.btnGreetz.Text = "About...";
            this.btnGreetz.UseVisualStyleBackColor = true;
            this.btnGreetz.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(93, 544);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "Help...";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnMove
            // 
            this.btnMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMove.Location = new System.Drawing.Point(387, 544);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(25, 23);
            this.btnMove.TabIndex = 4;
            this.btnMove.Text = "...";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // mnuMove
            // 
            this.mnuMove.Name = "mnuMove";
            this.mnuMove.Size = new System.Drawing.Size(61, 4);
            // 
            // projectScreen
            // 
            this.projectScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectScreen.Location = new System.Drawing.Point(1, 63);
            this.projectScreen.Name = "projectScreen";
            this.projectScreen.PluginManager = null;
            this.projectScreen.Size = new System.Drawing.Size(590, 469);
            this.projectScreen.TabIndex = 0;
            this.projectScreen.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 573);
            this.Controls.Add(this.projectScreen);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnGreetz);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.btnMoveBack);
            this.Controls.Add(this.btnMoveNext);
            this.Controls.Add(this.btnExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TheGHOST";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.EditorForm_Resize_Move);
            this.Move += new System.EventHandler(this.EditorForm_Resize_Move);
            this.ResizeEnd += new System.EventHandler(this.EditorForm_Resize_Move);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMoveNext;
        private System.Windows.Forms.Button btnMoveBack;
        private System.Windows.Forms.Panel pnlHeader;
        private ProjectScreen projectScreen;
        private System.Windows.Forms.SaveFileDialog save;
        private System.Windows.Forms.Label lblScreenMessage;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnGreetz;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.ContextMenuStrip mnuMove;
    }
}

