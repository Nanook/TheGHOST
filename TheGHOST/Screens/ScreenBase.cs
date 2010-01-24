using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Nanook.TheGhost
{
    internal delegate void ScreenBusyEventHandler(object source, ScreenBusyEventArgs e);


    public partial class ScreenBase : UserControl
    {
        internal event ScreenBusyEventHandler ScreenBusy;

        public ScreenBase() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Return true=has changed, false=has not changed
        /// </summary>
        //public virtual bool HasChanged()
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// The screen is being created
        /// </summary>
        public virtual void Construct(Project project)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Screen is being displayed
        /// </summary>
        public virtual void Open()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The screen is being hidden
        /// </summary>
        /// <returns>false if screen cannot be closed.</returns>
        public virtual bool Close(bool appQuiting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the title to be displayed in the main window
        /// </summary>
        /// <returns></returns>
        public virtual string TitleMessage()
        {
            throw new NotImplementedException();
        }


        public void MessageShow()
        {
            this.MessageShow("Please Wait...");
        }

        public void MessageShow(string message)
        {
            if (!pnlMessage.Visible)
            {
                pnlMessage.Location = new Point((this.Width - pnlMessage.Width) / 2, (this.Height - pnlMessage.Height) / 2);

                pnlMessage.Visible = true;

            }
            lblMessage.Text = message;
            pnlMessage.BringToFront();

            if (ScreenBusy != null)
                this.ScreenBusy(this, new ScreenBusyEventArgs(true));

            Application.DoEvents();
        }

        public void MessageHide()
        {
            pnlMessage.Visible = false;
            Application.DoEvents();

            if (ScreenBusy != null)
                this.ScreenBusy(this, new ScreenBusyEventArgs(false));
        }

        
    }
}
