using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nanook.TheGhost
{
    public partial class ExceptionMessageForm : Form
    {
        public ExceptionMessageForm()
        {
            InitializeComponent();
        }

        public ExceptionMessageForm(Exception ex)
            : this()
        {
            lblMessage.Text = ex.Message.Replace("\r\n", ", ").Replace("\n", ", ");

            txtException.Text = String.Format("{0} - v{1} / v{2}\r\n    OS: {3} {4} ({5}) {6} - {7}\r\n{8}",
                TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion,
                OsInfo.GetOSName().Trim(), OsInfo.GetOSProductType().Trim(), OsInfo.GetPlatform().ToString(), OsInfo.OSVersion.Trim(), OsInfo.GetOSServicePack().Trim(),
                ex.ToString());
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmExceptionMessage_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - v{1} / v{2} - Exception", TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion);
        }

        private void btnTerminate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtException.Text);
        }


    }
}