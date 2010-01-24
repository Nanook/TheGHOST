using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Nanook.TheGhost
{

    public delegate bool DrawGraphics(Graphics g, Rectangle bounds);


    internal partial class NotesEditorDisplay : UserControl
    {
        public NotesEditorDisplay() : base()
        {
            InitializeComponent();

            _callback = null;

            //hack to make vista update the screen
            _isVista = OsInfo.OSMajorVersion == 6; //"Windows Longhorn";

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            //_timer = new PerfTimer();
            _timer = new Stopwatch();
            OnSizeChanged(new EventArgs());

            if (this.DesignMode)
                return;
        }

        public void Redraw()
        {
            if (_callback == null || _buffer == null || _screenRect == null)
                return;

            //not playing, draw the frame and render
            using (Graphics g = Graphics.FromImage(_buffer))
            {
                _callback(g, _screenRect);

                //draw the buffer to the window

                if (_isVista) //BAD HACK I don't know why this makes vista update the window more, but it does.  May be something to do with the listview
                    this.Parent.Invalidate(new Rectangle(this.Parent.Controls[4].Left, this.Parent.Controls[4].Top - 60, _screenRect.Width, _screenRect.Height + 60), true);
                else
                    this.Invalidate();
            }

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!this.DesignMode)
            {

                if (_buffer != null)
                {
                    _buffer.Dispose();
                    _buffer = null;
                }

                _buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                _screenRect = this.ClientRectangle;

                using (Graphics g = Graphics.FromImage(_buffer))
                {
                    g.FillRectangle(Brushes.Black, _screenRect);
                }
            }
            base.OnSizeChanged(e);

            this.Redraw();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode || _buffer == null)
                base.OnPaint(e);
            else
                e.Graphics.DrawImageUnscaled(_buffer, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.DesignMode)
                base.OnPaintBackground(e);
            //Don't allow the background to paint
        }

        public void Initialise(int interval, DrawGraphics callback)
        {
            _callback = callback;
        }

        public void Start()
        {
            //_timer.Start(PerfTimer.Counter);
            _timer.Start();
            Application.Idle += new EventHandler(this.OnApplicationIdle);
            if (_callback == null)
                return;

        }

        public void Stop()
        {
            Application.Idle -= new EventHandler(this.OnApplicationIdle);
            if (_callback == null)
                return;

        }

        private bool AppStillIdle
        {
            get
            {
                NativeMethods.Message msg;
                return !NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        //public void OnApplicationIdle(object sender, EventArgs e)
        //{
        //    int tc;
        //    while (AppStillIdle)
        //    {
        //        int i = 0;
        //        _timer.Record();

        //        Redraw();

        //        tc = Environment.TickCount;
        //        while (_timer.TicksPerSecond <= 0.01)  //100 fps
        //            _timer.Record();

        //        _timer.Start(_timer.RecordTime);

        //        // ... update game objects with object.Update(timer.TicksPerSecond).
        //        // ... display fps with 1.0/timer.TicksPerSecond.
        //        //System.Diagnostics.Debug.WriteLine(1.0 / _timer.TicksPerSecond);
        //    }
        //}

        public void OnApplicationIdle(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(0); //allow audio thread to process on a single CPU machine

            while (AppStillIdle)
            {
                while (_timer.ElapsedMilliseconds < 10);  //100 fps

                Redraw();

                _timer.Reset();
                _timer.Start();
            }
        }

        private Bitmap _buffer;
        private Rectangle _screenRect;
        private DrawGraphics _callback;
        //private PerfTimer _timer;
        private Stopwatch _timer;

        private bool _isVista;

    }

    internal class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public IntPtr msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd,
                                              uint messageFilterMin,
                                              uint messageFilterMax, uint flags);
        public NativeMethods() { }
    }

}
