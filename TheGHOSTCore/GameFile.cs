using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nanook.TheGhost
{
    public enum GameFileType
    {
        QbPak,
        QbPab,
        DbgPak,
        Wad,
        Dat,
        NotesPak,
        Other
    }

    public class GameFile
    {
        /// <summary>
        /// Create an instance that maps a game file to a local path
        /// </summary>
        /// <param name="gameName">Game path and filename</param>
        /// <param name="localName">Local disk path and filename</param>
        /// <param name="type">Type of file</param>
        internal GameFile(string gameName, string localName, GameFileType type) : this(gameName, localName, type, false)
        {
        }

        /// <summary>
        /// Create an instance that maps a game file to a local path
        /// </summary>
        /// <param name="gameName">Game path and filename</param>
        /// <param name="localName">Local disk path and filename</param>
        /// <param name="type">Type of file</param>
        /// <param name="isNew">true=import this file, false do not import this file, allow export</param>
        internal GameFile(string gameName, string localName, GameFileType type, bool isNew)
        {
            this.Type = type;
            this.GameName = gameName;
            this.LocalName = localName;
            if (isNew)
                this.LocalDateTime = DateTime.MinValue; // this will cause Changed to return true
            else
                this.UpdateLocalDateTime();
        }

        public static string[] CreateGameArray(params GameFile[] files)
        {
            string[] s = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
                s[i] = files[i].GameName;

            return s;
        }

        public static string[] CreateLocalArray(params GameFile[] files)
        {
            string[] s = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
                s[i] = files[i].LocalName;

            return s;
        }

        public static GameFile[] CreateArray(params GameFile[] gameFiles)
        {
            return gameFiles;
        }

        /// <summary>
        /// Called to update the LocalDateTime to indicate there are no changes
        /// </summary>
        internal void UpdateLocalDateTime()
        {
            this.LocalDateTime = (new FileInfo(LocalName)).LastWriteTime;
        }

        public bool Changed
        {
            get { return (new FileInfo(LocalName)).LastWriteTime != LocalDateTime; }
        }

        /// <summary>
        /// Holds the modified date of the local file when it was added to this class, it is used to
        /// test against the modified date of the local file to detect if it has changed
        /// </summary>
        internal DateTime LocalDateTime { get; set; }

        public GameFileType Type { get; set; }
        public string GameName { get; set; }
        public string LocalName { get; set; }

    }

}
