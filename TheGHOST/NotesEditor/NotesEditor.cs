using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Nanook.TheGhost
{
    internal partial class NotesEditor : UserControl
    {

        public NotesEditor() : base()
        {
            InitializeComponent();

            udHoPo.Maximum = 20;
            udHoPo.Minimum = 0;
            udHoPo.Increment = (decimal)0.1;
            udHoPo.Value = new decimal(2.95);
            udHoPo.DecimalPlaces = 2;


            _isInitialised = false;

            _timer = new Stopwatch();
            _frames = 0;

            _maxLength = 0;
            _audioPlayer = null;
            _ghNotesItems = new List<GhNotesItem>();
            _notesItems = new List<NotesFileItem>();
            _wavFilenames = new List<string>();

            _startPlayPos = 0;
            _triggerPos = 500;
            _speed = 250;
            _convMsPix = (1000 / _speed); //1000 / 250 = quarter the speed of 1 pix per sec  (1 sec = 250 pixels)
            _dotSize = 12;
            _dotSizeHalf = _dotSize / 2;
            _dotSizeQuarter = _dotSize / 4;
            _dotSizeDouble = _dotSize * 2;

            _activeItemIdx = 0;
            _isPlaying = false;
            lblFps.Text = _noFpsString;
        }

        protected virtual bool DrawFrame(Graphics g, Rectangle bounds)
        {
            if (_activeItem == null)
                return false;

            int currBackup = _currPlayPos;
            int sustainTrigger;
            int halfSustainTrigger;
            int lastN = 0;
            int lastB = 0;

            
            if (!_isInitialised)
                return false;


            if (!_isScrubbing && _isPlaying)
            {
                if (_audioPlayer != null) //get sample offset
                    _currPlayPos = (_audioPlayer.PlayedMs + _startPlayPos);
                else //use the timer
                    _currPlayPos = (int)(_timer.ElapsedMilliseconds + _startPlayPos);


                //System.Diagnostics.Debug.WriteLine(string.Format("{0} = {1}, {2}", (float)_currPlayPos / (float)_convMsPix, _currPlayPos, _convMsPix));
                
                if (_audioPlayer != null && _currPlayPos >= _maxLength)
                {
                    this.Stop();
                    return false;
                }
            }

            int playPos = _currPlayPos - _triggerPos;

            //g.FillRectangle(Brushes.Black, bounds);

            float pix;
            float pix2;

            if (_activeItem.Notes == null)
                return false;


            g.FillRectangle(Brushes.Black, bounds);

            int maxTime = (ne.ClientRectangle.Width * (int)_convMsPix) + playPos;
            int n;
            int s;
            int f;
            int b;
            int sp;

            int p1 = _dotSize;
            int p2 = p1 + _dotSize;
            int p3 = p2 + _dotSize;
            int p4 = p3 + _dotSize;
            int p5 = p4 + _dotSize;
            //draw star power
            if (_activeItem.StarPower != null && _activeItem.StarPower.Length != 0)
            {
                for (int i = 0; i < _activeItem.StarPower.Length; i += 3)
                {
                    sp = _activeItem.StarPower[i] + (int)udNotesOffset.Value;
                    s = _activeItem.StarPower[i + 1];

                    if (sp >= maxTime)
                        break;

                    if (sp + s > playPos)
                    {
                        pix = ((float)(sp - playPos) / (float)_convMsPix);
                        pix2 = ((float)s / (float)_convMsPix);

                        g.FillRectangle(Brushes.DarkCyan, pix, 0, pix2, _dotSize * 7);
                    }
                }
            }


            int startFret = 0; //get the frets either side of the visible area (used for sustain caclulations)
            int endFret = -1;

            pix = (float)_triggerPos / (float)_convMsPix;
            //draw marker
            g.FillRectangle(Brushes.White, pix - 1, 0, 3, _dotSize * 7);

            //draw frets
            for (int i = 0; i < _notesFile.Frets.Length; i++)
            {
                f = _notesFile.Frets[i] + (int)udFretsOffset.Value;
                endFret = i + 1;

                if (f >= maxTime)
                    break;

                if (f > playPos)
                {
                    pix = (float)(f - playPos) / (float)_convMsPix;
                    g.FillRectangle(Brushes.White, pix, 0, 1, _dotSize * 7);
                }
                else
                    startFret = i;

            }


            sustainTrigger = _activeItem.SustainTrigger;
            halfSustainTrigger = sustainTrigger / 2;

            bool isSustained = false; //by default notes are not sustained (in case user has scrolled frets forward in window
            for (int i = 0; i < _activeItem.Notes.Length; i += 3)
            {
                n = _activeItem.Notes[i] + (int)udNotesOffset.Value;

                if (n >= maxTime)
                    break;

                s = _activeItem.Notes[i + 1];
                b = _activeItem.Notes[i + 2];

                //calculate if this note is sustained
                int fpos;
                int fretLen = 0;
                isSustained = false;
                for (int c = startFret; c < _notesFile.Frets.Length && c < endFret;  c++)
                {
                    if ((fpos = _notesFile.Frets[c] + (int)udFretsOffset.Value) > n && c > 0) //careful fpos is assigned here
                    {
                        fretLen = fpos - (_notesFile.Frets[c - 1] + (int)udFretsOffset.Value);
                        isSustained = s > ((fretLen / 192.0) * (double)(192 >> 2));  //(fretLen / 192.0 == bpmUnit
                        break;
                    }
                }

                int tsA = 4;
                int tsB = 4;

                //find the timesig
                for (int j = 0; j < _notesFile.TimeSig.Length; j += 3)
                {
                    if (_notesFile.TimeSig[j] < n)
                    {
                        tsA = _notesFile.TimeSig[j + 1];
                        tsB = _notesFile.TimeSig[j + 2];
                    }
                }

                if (n + s > playPos)
                {
                    pix = (float)(n - playPos) / (float)_convMsPix;
                    if (isSustained && s > sustainTrigger)
                    {
                        if (chkGh3SustainClipping.Checked && i + 3 < _activeItem.Notes.Length)
                            s = clipSustain(n, s, _activeItem.Notes[i + 3] + (int)udNotesOffset.Value, halfSustainTrigger);

                        pix2 = (float)(s - (halfSustainTrigger)) / (float)_convMsPix;
                    }
                    else
                        pix2 = 0;

                    bool hopo = false;
                    int bc = 0;
                    if ((b & 1) != 0)
                        bc++;
                    if ((b & 2) != 0)
                        bc++;
                    if ((b & 4) != 0)
                        bc++;
                    if ((b & 8) != 0)
                        bc++;
                    if ((b & 16) != 0)
                        bc++;

                    if (bc == 1 && fretLen != 0 && (lastB & b) == 0)
                        hopo = ((int)(n - (fretLen * (tsB / (float)tsA) / (float)udHoPo.Value))) <= lastN;
                    if ((b & 32) != 0)
                        hopo = !hopo; //negate the hopo flag

                    if ((b & 1) != 0)
                        drawNote(g, Brushes.Green, pix, p1, pix2, hopo, n - playPos);
                    if ((b & 2) != 0)
                        drawNote(g, Brushes.Red, pix, p2, pix2, hopo, n - playPos);
                    if ((b & 4) != 0)
                        drawNote(g, Brushes.Yellow, pix, p3, pix2, hopo, n - playPos);
                    if ((b & 8) != 0)
                        drawNote(g, Brushes.Blue, pix, p4, pix2, hopo, n - playPos);
                    if ((b & 16) != 0)
                        drawNote(g, Brushes.Orange, pix, p5, pix2, hopo, n - playPos);
                    if (b == 0) //BAD BUTTON
                        drawNote(g, Brushes.Cyan, pix, p5, pix2, hopo, n - playPos);
                }

                lastN = n;
                lastB = b;
            }

            g.Dispose();

            lblTimer.Text = string.Format("{4}  /  {0}:{1}:{2}.{3}", (currBackup / 3600000).ToString(), ((currBackup % 3600000) / 60000).ToString().PadLeft(2, '0'), ((currBackup % 60000) / 1000).ToString().PadLeft(2, '0'), (currBackup % 1000).ToString().PadLeft(3, '0'), currBackup.ToString().PadLeft(8, ' '));

            if (!_isScrubbing && _isPlaying)
            {
                _frames++;
                lblFps.Text = string.Format(_fpsString, (1000 / Math.Max(1, (_timer.ElapsedMilliseconds / _frames))).ToString());

                scrl.Value = Math.Max(0, currBackup); //current pos
            }


            return true;
        }

        private int clipSustain(int susNote, int susLen, int nextNote, int halfSustainTrigger)
        {
            //if ((susLen - halfSustainTrigger) > nextNote - susNote - halfSustainTrigger)
            //    return (nextNote - susNote - halfSustainTrigger) + halfSustainTrigger;
            if ((susLen - halfSustainTrigger) > nextNote - susNote - 100)
                return (nextNote - susNote - 100) + halfSustainTrigger;
            else
                return susLen;
        }

        private void drawNote(Graphics g, Brush b, float pixX, float pixY, float pixLen, bool hopo, int noteOffset)
        {

            float h = _dotSize / 2;

            if (pixLen != 0)
                g.FillRectangle(b, pixX, pixY + (_dotSizeQuarter), pixLen, _dotSizeHalf);

            //else
            //{
            g.FillEllipse(b, pixX - _dotSizeHalf, pixY, _dotSize, _dotSize);

            if (hopo)
                g.FillEllipse(Brushes.White, pixX - _dotSizeQuarter, pixY + _dotSizeQuarter, _dotSizeHalf, _dotSizeHalf);
            //}


            //if (_isPlaying && !_isScrubbing && noteOffset - _triggerPos > (0 - (_timer * 2)) && noteOffset - _triggerPos < (_timer * 2))
            if (_isPlaying && !_isScrubbing && noteOffset - _triggerPos > -100 && noteOffset - _triggerPos < 10)
            {
                float trigPix = (float)_triggerPos / (float)_convMsPix;
                int trig = Math.Abs(noteOffset - _triggerPos);
                Color c = Color.FromArgb(255 - (int)(trig / 100F * 255F) , 255, 255, 255);

                g.FillEllipse(new SolidBrush(c), trigPix - _dotSize, pixY - _dotSizeHalf, _dotSizeDouble, _dotSizeDouble);
            }

        }

        private void setActiveItem(int index)
        {
            _activeItemIdx = index;
            _activeItem = null;
            if (_ghNotesItems.Count != 0)
            {
                if (_activeItemIdx < _ghNotesItems.Count && _ghNotesItems[_activeItemIdx].IsMapped)
                    _activeItem = _ghNotesItems[_activeItemIdx].MappedFileItem;
            }
            else
            {
                if (_activeItemIdx < _notesItems.Count)
                    _activeItem = _notesItems[_activeItemIdx];
            }
        }

        private void addListItem(string name, int imageidx, NotesFileItem item)
        {
            ListViewItem li = new ListViewItem(name);

            if (imageidx == -1)
            {
                if (item.ButtonsUsed >= 3)
                    li.ImageIndex = item.ButtonsUsed - 3;
                else
                    li.ImageIndex = 0; //easy
            }
            else
                li.ImageIndex = imageidx;

            if (item != null)
            {
                li.SubItems.Add(item.SyncOffset.ToString());
                li.SubItems.Add(item.ButtonsUsed.ToString());
                li.SubItems.Add(string.Concat(item.StarPowerCount.ToString(), item.HasGeneratedStarPower ? " *" : string.Empty));
                li.SubItems.Add(string.Concat(item.NotesCount.ToString(), item.HasGeneratedNotes ? " *" : string.Empty));
                li.Tag = item;
            }
            li.Checked = true;
            lvw.Items.Add(li);
        }

        private void updateListItem(string name, int imageidx, int idx, NotesFileItem item)
        {
            ListViewItem li = lvw.Items[idx];
            li.Text = name;

            if (imageidx == -1)
            {
                if (item.ButtonsUsed >= 3)
                    li.ImageIndex = item.ButtonsUsed - 3;
                else
                    li.ImageIndex = 0; //easy
            }
            else
                li.ImageIndex = imageidx;

            if (item != null)
            {
                while (li.SubItems.Count <= 4)
                    li.SubItems.Add("");

                li.SubItems[1].Text = item.SyncOffset.ToString();
                li.SubItems[2].Text = item.ButtonsUsed.ToString();
                li.SubItems[3].Text = string.Concat(item.StarPowerCount.ToString(), item.HasGeneratedStarPower ? " *" : string.Empty);
                li.SubItems[4].Text = string.Concat(item.NotesCount.ToString(), item.HasGeneratedNotes ? " *" : string.Empty);
                li.Tag = item;
            }
        }


        public void Initialise()
        {
            int maxNote = 0;

            if (_audioPlayer == null)
            {
                if (_wavFilenames.Count != 0)
                    _audioPlayer = new AudioPlayer(_wavFilenames.ToArray());

                NotesFileItem nii;
                int? offset = null; //test if all offsets are the same and checkbox
                bool offsetsSame = true;

                if (_ghNotesItems.Count != 0)
                {
                    foreach (GhNotesItem item in _ghNotesItems)
                    {
                        nii = item.MappedFileItem;

                        if (nii != null)
                        {
                            if (offset == null)
                                offset = nii.SyncOffset;
                            else if (offset != nii.SyncOffset)
                                offsetsSame = false;

                        }
                        addListItem(item.Name, (int)item.Difficulty, nii);

                        if (nii != null && nii.Notes.Length >= 3 && nii.Notes[nii.Notes.Length - 3] + nii.Notes[nii.Notes.Length - 2] > maxNote)
                            maxNote = nii.Notes[nii.Notes.Length - 3] + nii.Notes[nii.Notes.Length - 2];
                    }
                }
                else
                {
                    foreach (NotesFileItem item in _notesItems)
                    {
                        if (offset == null)
                            offset = item.SyncOffset;
                        else if (offset != item.SyncOffset)
                            offsetsSame = false;

                        addListItem(item.SourceName, -1, item);

                        if (item.Notes.Length >= 3 && item.Notes[item.Notes.Length - 3] + item.Notes[item.Notes.Length - 2] > maxNote)
                            maxNote = item.Notes[item.Notes.Length - 3] + item.Notes[item.Notes.Length - 2];
                    }
                }

                chkOffsetChecked.Checked = offsetsSame;

                if (_activeItemIdx < lvw.Items.Count)
                    lvw.Items[_activeItemIdx].Selected = true;
            }
            setActiveItem(_activeItemIdx);
            udFretsOffset.Value = _notesFile.NonNoteSyncOffset;

            //ne.Initialise(_timer, DrawFrame);
            ne.Initialise(0, DrawFrame);

            int audioLen = 0;
            if (_audioPlayer != null)
                audioLen = _audioPlayer.Length;

            _maxLength = (audioLen > maxNote ? audioLen : maxNote) + (ne.ClientSize.Width * (int)_convMsPix); //get the maximum fret

            scrl.Maximum = _maxLength;
            scrl.Minimum = 0;
            scrl.SmallChange = 100;
            scrl.LargeChange = (ne.ClientSize.Width * (int)_convMsPix); //pix to ms

            _startPlayPos = 0;

            _frames = 0;
            _timer.Start();
            //_startTicks = DateTime.Now.Ticks;

            _isInitialised = true;

            ne.Redraw();
        }

        public void Start()
        {
            if (!_isInitialised || (_audioPlayer != null && _currPlayPos >= _maxLength))
                return;

            if (_audioPlayer != null)
                _audioPlayer.Play(_startPlayPos, true);

            _isPlaying = true;

            _currPlayPos = _startPlayPos;

            if (_timer.IsRunning)
                _timer.Stop();
            _timer.Reset();
            _frames = 0;
            _timer.Start();
            //_startTicks = DateTime.Now.Ticks;


            ne.Start();
            btnPlay.Text = "Stop";
        }

        public void Stop()
        {
            if (!_isInitialised)
                return;

            if (_timer.IsRunning)
                _timer.Stop();

            ne.Stop();
            pause();
            //ne.Redraw();
            btnPlay.Text = "Play";
            lblFps.Text = _noFpsString;
        }

        private void pause()
        {
            if (!_isScrubbing)
                _isPlaying = false;

            _startPlayPos = _currPlayPos;
            if (_timer.IsRunning)
                _timer.Stop();
            if (_audioPlayer != null)
                _audioPlayer.Pause();
            lblFps.Text = _noFpsString;
        }

        private void resume(int msPos, bool startAudio)
        {
            _isPlaying = true;

            if (_audioPlayer != null)
                _audioPlayer.Play(_startPlayPos, true);

            _startPlayPos = msPos;
            _currPlayPos = _startPlayPos;

            if (_timer.IsRunning)
                _timer.Stop();
            _timer.Reset();
            _frames = 0;
            _timer.Start();

            //_startTicks = DateTime.Now.Ticks;


        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!_isPlaying)
                this.Start();
            else
                this.Stop();
        }

        private void udNotesOffset_ValueChanged(object sender, EventArgs e)
        {
            if (!_isInitialised)
                return;

            int offset = (int)udNotesOffset.Value;

            NotesFileItem ni;


            foreach(ListViewItem li in lvw.Items)
            {
                ni = (NotesFileItem)li.Tag;
                if (ni != null && ((chkOffsetChecked.Checked && li.Checked) || li.Selected))
                {
                    li.SubItems[1].Text = offset.ToString();

                    //if apply to all is not checked and we're editing a non generated item - adjust any
                    //other non generated items pointing to the same notesitem.
                    //supports multiple ticks in lvw, when some may not be checked
                    foreach (ListViewItem li2 in lvw.Items)
                    {
                        if (ni == (NotesFileItem)li2.Tag)
                            li2.SubItems[1].Text = offset.ToString();
                    }
                }
            }

            if (!_isPlaying)
                ne.Redraw();
        }

        private void chkGh3SustainClipping_CheckedChanged(object sender, EventArgs e)
        {
            ne.Redraw();
        }

        private void udHoPo_ValueChanged(object sender, EventArgs e)
        {
            if (!_isInitialised)
                return;

            if (!_isPlaying)
                ne.Redraw();
        }

        private void udFretsOffset_ValueChanged(object sender, EventArgs e)
        {
            if (!_isInitialised)
                return;

            if (!_isPlaying)
                ne.Redraw();
        }

        private void lvw_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvw.SelectedIndices.Count != 0)
            {
                gboNotes.Text = lvw.SelectedItems[0].Text;
                if (lvw.SelectedItems[0].SubItems.Count > 1)
                    udNotesOffset.Value = int.Parse(lvw.SelectedItems[0].SubItems[1].Text);
                setActiveItem(lvw.SelectedIndices[0]);

                if (!_isPlaying)
                    ne.Redraw();
            }
        }

        private void scrl_Scroll(object sender, ScrollEventArgs e)
        {
            if (!_isInitialised)
                return;

            int v = e.NewValue;

            if (e.Type == ScrollEventType.ThumbPosition)
            {
                _isScrubbing = false;
                if (_isPlaying)
                    resume(v, true);
            }
            else if (e.Type == ScrollEventType.ThumbTrack)
            {
                bool s = _isScrubbing;
                _isScrubbing = true;

                if (!s && _isPlaying)
                    pause();

                _startPlayPos = v;
                _currPlayPos = _startPlayPos;

                ne.Redraw();
            }
            else if (e.Type != ScrollEventType.EndScroll)
            {
                bool p = _isPlaying;

                if (_isPlaying)
                {
                    pause();
                    _startPlayPos = v;
                    _currPlayPos = _startPlayPos;
                }

                _startPlayPos = v;
                _currPlayPos = _startPlayPos;

                if (p)
                    resume(e.NewValue, true);
                else
                    ne.Redraw();
            }
        }

        //private void btnGenerate_Click(object sender, EventArgs e)
        //{
        //    GhNotesItem source = null;
        //    if (_ghNotesItems.Count != 0)
        //    {
        //        foreach (GhNotesItem ghi in _ghNotesItems)
        //        {
        //            if (ghi.IsMapped && !ghi.MappedFileItem.HasGeneratedNotes && source == null || source != null && (int)(source.Difficulty) < (int)(ghi.Difficulty))
        //                source = ghi;
        //        }

        //        if (source != null)
        //        {
        //            foreach (GhNotesItem ghi in _ghNotesItems)
        //            {
        //                if (!ghi.IsMapped)
        //                    ghi.GenerateNotes(source);
        //                else if (ghi.MappedFileItem.StarPowerCount == 0)
        //                {
        //                    ghi.MappedFileItem.GenerateStarPower(true);
        //                }
        //            }

        //            for (int it = 0; it < _ghNotesItems.Count; it++)
        //                updateListItem(_ghNotesItems[it].Name, (int)_ghNotesItems[it].Difficulty, it, _ghNotesItems[it].MappedFileItem);

        //        }
        //        else
        //            MessageBox.Show(this, "Unable to locate an item to base notes generation on, Expert Guitar is preferable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //    else
        //    {
        //        MessageBox.Show(this, "This functionality is only available when using Mapped Items.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}



        [Browsable(false)]
        [ReadOnly(true)]
        public List<NotesFileItem> NotesInfoItems
        {
            get { return _notesItems; }
        }

        [Browsable(false)]
        [ReadOnly(true)]
        public List<GhNotesItem> GhNotesInfoItems
        {
            get { return _ghNotesItems; }
        }

        [Browsable(false)]
        [ReadOnly(true)]
        public List<string> WavFilenames
        {
            get { return _wavFilenames; }
        }

        [Browsable(false)]
        [ReadOnly(true)]
        public NotesFile NotesInfo
        {
            get { return _notesFile; }
            set { _notesFile = value; }
        }

        [Browsable(false)]
        [ReadOnly(true)]
        public float HoPoMeasure
        {
            get
            {
                float f = (float)udHoPo.Value;
                if (new decimal(f) > udHoPo.Maximum)
                    return f / 100;  //test a theory that this sometimes returns the value without a deciman point on some PCs
                return f;
            }
            set
            {
                decimal d = new decimal(value);
                if (d > udHoPo.Maximum || d < udHoPo.Minimum)
                    return; //leave as default
                udHoPo.Value = d;
            }
        }

        [Browsable(false)]
        [ReadOnly(true)]
        public bool Gh3SustainClipping
        {
            get
            {
                return chkGh3SustainClipping.Checked;
            }
            set
            {
                chkGh3SustainClipping.Checked = value;
            }
        }


        public void ApplyChanges()
        {
            NotesFileItem ni;
            foreach (ListViewItem li in lvw.Items)
            {
                ni = (NotesFileItem)li.Tag;
                if (ni != null)
                    ni.SyncOffset = int.Parse(li.SubItems[1].Text);
            }
            _notesFile.NonNoteSyncOffset = (int)udFretsOffset.Value;

        }

        private int _maxLength;

        private bool _isInitialised;
        private bool _isScrubbing;
        private bool _isPlaying;

        private int _activeItemIdx;
        private NotesFileItem _activeItem;

        private List<GhNotesItem> _ghNotesItems;
        private List<NotesFileItem> _notesItems;
        private NotesFile _notesFile;
        private List<string> _wavFilenames;

        private Stopwatch _timer;
        private long _frames;

        private float _convMsPix;
        private int _triggerPos; //ms
        //private long _startTicks;
        private int _currPlayPos; //ms
        private int _startPlayPos; //ms
        private int _speed; //pix per sec
        private int _dotSize;
        private int _dotSizeHalf;
        private int _dotSizeQuarter;
        private int _dotSizeDouble;

        private AudioPlayer _audioPlayer;

        private const string _noFpsString = "FPS: 0";
        private const string _fpsString = "FPS: {0}";


    }
}
