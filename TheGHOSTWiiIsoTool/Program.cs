using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Nanook.TheGhost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainExHandler);
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExHandler);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void AppDomainExHandler(object sender, UnhandledExceptionEventArgs e)
        {
            showException((Exception)e.ExceptionObject);
        }

        static void ThreadExHandler(object sender, ThreadExceptionEventArgs e)
        {
            showException((Exception)e.Exception);
        }

        static void showException(Exception ex)
        {
            try
            {
                ExceptionMessageForm f = new ExceptionMessageForm(ex);
                if (f.ShowDialog() == DialogResult.Abort) //Abort is exit app
                {
                    f.Close();
                    ForceClose = true;
                    foreach (Form frm in Application.OpenForms)
                        frm.Close();

                    Application.Exit();
                }
                else
                    f.Close();
            }
            catch
            {
            }
        }

        public static bool ForceClose { get; set; }
    }
}
