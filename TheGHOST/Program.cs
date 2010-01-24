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
            //for (int i = 1; i <= 100; i++)
            //{
            //    Nanook.QueenBee.Parser.QbKey q = Nanook.QueenBee.Parser.QbKey.Create(string.Format("theghost{0}", i.ToString().PadLeft(3, '0')));
            //    System.Diagnostics.Debug.WriteLine(string.Format(@"0x{0}|{1}|cripts\guitar\store_data.qb.ngc", q.Crc.ToString("X"), q.Text));
            //    System.Diagnostics.Debug.WriteLine(string.Format(@"0x{0}|{1}|cripts\guitar\songlist.qb.ngc", q.Crc.ToString("X"), q.Text));
            //}
                //System.IO.Directory.CreateDirectory("G:\\TheGHOST\\gh3dlc\\__" + i.ToString().PadLeft(2, '0'));

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
