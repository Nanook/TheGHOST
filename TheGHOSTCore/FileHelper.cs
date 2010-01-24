using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nanook.TheGhost
{
    public class FileHelper
    {
        public static void Delete(string path)
        {
            Delete(path, 500, false);
        }

        public static void Delete(string path, double lockWaitMs)
        {
            Delete(path, lockWaitMs, false);
        }

        public static void Delete(string path, double lockWaitMs, bool exceptionOnFail)
        {
            DateTime to = DateTime.Now.AddMilliseconds(lockWaitMs);

            while (File.Exists(path) && DateTime.Now < to)
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    if (DateTime.Now < to)
                    {
                        System.Threading.Thread.Sleep(100);
                        System.Threading.Thread.Sleep(0);
                    }
                }
            }

            if (File.Exists(path) && exceptionOnFail)
                throw new ApplicationException(string.Format("Failed to delete '{0}'", path));

            return;
        }


        public static void Move(string src, string dst)
        {
            Move(src, dst, 500, false);
        }

        public static void Move(string src, string dst, double lockWaitMs)
        {
            Move(src, dst, lockWaitMs, false);
        }

        public static void Move(string src, string dst, double lockWaitMs, bool exceptionOnFail)
        {
            DateTime to = DateTime.Now.AddMilliseconds(lockWaitMs);

            Delete(dst, lockWaitMs, exceptionOnFail);

            while (File.Exists(src) && DateTime.Now < to)
            {
                try
                {
                    File.Move(src, dst);
                }
                catch
                {
                    File.Copy(src, dst, true);
                    Delete(src, lockWaitMs, exceptionOnFail);
                }
            }

            return;
        }


    }
}
