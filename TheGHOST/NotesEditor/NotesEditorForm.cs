using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nanook.TheGhost
{
    internal partial class NotesEditorForm : Form
    {
        public NotesEditorForm()
        {
            InitializeComponent();
        }

        public void Initialise(NotesFile baseNotesInfo, NotesFile itemsNotesInfo, float hopoMeasure, bool gh3SustainClipping, string[] wavPaths)
        {
            ne.NotesInfo = baseNotesInfo;
            ne.NotesInfoItems.AddRange(itemsNotesInfo.Items);
            ne.WavFilenames.AddRange(wavPaths);
            ne.HoPoMeasure = hopoMeasure;
            ne.Gh3SustainClipping = gh3SustainClipping;
            ne.Initialise();

            loadWindowInfo(this, _windowInfo);
        }

        public void Initialise(NotesFile baseNotesInfo, GhNotesItem[] items, float hopoMeasure, bool gh3SustainClipping, string[] wavPaths)
        {
            ne.NotesInfo = baseNotesInfo;
            ne.GhNotesInfoItems.AddRange(items);
            ne.WavFilenames.AddRange(wavPaths);
            ne.HoPoMeasure = hopoMeasure;
            ne.Gh3SustainClipping = gh3SustainClipping;
            ne.Initialise();

            loadWindowInfo(this, _windowInfo);
        }

        private void NotesEditorForm_Load(object sender, EventArgs e)
        {
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ne.ApplyChanges();
            Application.DoEvents();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Close();
        }

        private void NotesEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                saveWindowInfo(this);
                ne.Stop();
                //ne.Dispose();
                Application.DoEvents();
            }
            catch { }
        }

        private void saveWindowInfo(Form win)
        {
            if (_windowInfo == null)
                return;

            if (win.WindowState == FormWindowState.Normal)
                _windowInfo = string.Format("{0},{1},{2},{3},{4}", win.Location.X, win.Location.Y, win.Size.Width, win.Size.Height, (int)(win.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : win.WindowState));
            else
            {
                string[] s = _windowInfo.Split(',');
                s[4] = ((int)(win.WindowState == FormWindowState.Minimized ? FormWindowState.Normal : win.WindowState)).ToString();
                _windowInfo = string.Join(",", s);
            }
        }

        private void loadWindowInfo(Form win, string settings)
        {
            if (_windowInfo == null)
            {
                win.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
                win.Refresh();
                return;
            }

            if (settings.Length != 0)
            {
                _windowInfo = settings;
                string[] wi = settings.Split(',');
                win.Location = new Point(int.Parse(wi[0]), int.Parse(wi[1]));
                win.Size = new Size(int.Parse(wi[2]), int.Parse(wi[3]));
                win.WindowState = (FormWindowState)int.Parse(wi[4]);
                win.Refresh();
            }
            else
            {
                win.WindowState = FormWindowState.Normal;
                win.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - win.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - win.Height) / 2);
            }
        }

        private void NotesEditorForm_Resize_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                saveWindowInfo(this);
        }

        public float HoPoMeasure
        {
            get { return ne.HoPoMeasure; }
            set { ne.HoPoMeasure = value; }
        }

        public bool Gh3SustainClipping
        {
            get { return ne.Gh3SustainClipping; }
            set { ne.Gh3SustainClipping = value; }
        }

        public static string WindowInfo
        {
            get { return _windowInfo; }
            set { _windowInfo = value; }
        }

        private static string _windowInfo;

    }
}
