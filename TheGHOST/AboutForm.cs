using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Nanook.TheGhost
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void SetVersions(string appVersion, string coreVersion)
        {
            lblVersionApp.Text = appVersion;
            lblVersionCore.Text = coreVersion;
        }

        private void lnkVisit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(lnkVisit.Text);
            Process.Start(sInfo);
        }

    }
}
