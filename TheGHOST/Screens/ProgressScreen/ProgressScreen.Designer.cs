namespace Nanook.TheGhost
{
    partial class ProgressScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressScreen));
            this.gboProgress = new System.Windows.Forms.GroupBox();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.prg = new System.Windows.Forms.ProgressBar();
            this.txtTempPath = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStartMessage = new System.Windows.Forms.Label();
            this.gboProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // gboProgress
            // 
            this.gboProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gboProgress.Controls.Add(this.txtProgress);
            this.gboProgress.Controls.Add(this.prg);
            this.gboProgress.Controls.Add(this.txtTempPath);
            this.gboProgress.Controls.Add(this.btnStart);
            this.gboProgress.Controls.Add(this.lblStartMessage);
            this.gboProgress.Location = new System.Drawing.Point(3, 3);
            this.gboProgress.Name = "gboProgress";
            this.gboProgress.Size = new System.Drawing.Size(436, 449);
            this.gboProgress.TabIndex = 0;
            this.gboProgress.TabStop = false;
            this.gboProgress.Text = "Update Progress";
            // 
            // txtProgress
            // 
            this.txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgress.BackColor = System.Drawing.SystemColors.Window;
            this.txtProgress.Location = new System.Drawing.Point(9, 152);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ReadOnly = true;
            this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgress.Size = new System.Drawing.Size(419, 287);
            this.txtProgress.TabIndex = 4;
            this.txtProgress.WordWrap = false;
            // 
            // prg
            // 
            this.prg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prg.Location = new System.Drawing.Point(9, 123);
            this.prg.Name = "prg";
            this.prg.Size = new System.Drawing.Size(419, 23);
            this.prg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prg.TabIndex = 3;
            // 
            // txtTempPath
            // 
            this.txtTempPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTempPath.Location = new System.Drawing.Point(9, 72);
            this.txtTempPath.Name = "txtTempPath";
            this.txtTempPath.ReadOnly = true;
            this.txtTempPath.Size = new System.Drawing.Size(340, 20);
            this.txtTempPath.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(353, 71);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStartMessage
            // 
            this.lblStartMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStartMessage.Location = new System.Drawing.Point(6, 16);
            this.lblStartMessage.Name = "lblStartMessage";
            this.lblStartMessage.Size = new System.Drawing.Size(424, 52);
            this.lblStartMessage.TabIndex = 0;
            this.lblStartMessage.Text = resources.GetString("lblStartMessage.Text");
            this.lblStartMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProgressScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboProgress);
            this.Name = "ProgressScreen";
            this.Size = new System.Drawing.Size(442, 452);
            this.Controls.SetChildIndex(this.gboProgress, 0);
            this.gboProgress.ResumeLayout(false);
            this.gboProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboProgress;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblStartMessage;
        private System.Windows.Forms.TextBox txtTempPath;
        private System.Windows.Forms.ProgressBar prg;
        private System.Windows.Forms.TextBox txtProgress;
    }
}
