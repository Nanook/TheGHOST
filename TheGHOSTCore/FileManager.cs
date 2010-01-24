using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Nanook.QueenBee.Parser;
using System.Windows.Forms;
using io=System.IO;
using System.IO;

namespace Nanook.TheGhost
{
    /// <summary>
    /// Manages files being imported and eported to and from TheGHOST respectively
    /// </summary>
    public class FileManager
    {
        internal FileManager(Project project, IPluginFileCopy fileCopyPlugin, string gameLocation)
        {
            _pakFormat = null;
            _pakEditor = null;
            _dbgEditor = null;

            _songListQbFile = null;
            _guitarProgressionQbFile = null;

            _project = project;
            _files = new Dictionary<string, GameFile>();
            _fileCopy = fileCopyPlugin;
            _gameLocation = gameLocation;
            _backgroundAudioDatWad = null;

        }

        #region ProjectFiles

        internal DirectoryInfo ProjectSongSettingsFolder(ProjectTierSong pts, DirectoryInfo pDir)
        {
            string badFileChars = "[\\/:<>|?*]";
            string newPath = string.Format(@"{0}\({1} {2}) {3} - {4}", pDir.FullName, pts.Tier.Number.ToString().PadLeft(2, '0'), pts.Number.ToString().PadLeft(3, '0'), Regex.Replace(pts.Song.Artist, badFileChars, string.Empty), Regex.Replace(pts.Song.Title, badFileChars, string.Empty));
            //System.Diagnostics.Debug.WriteLine(newPath);
            return new DirectoryInfo(newPath);
        }

        /// <summary>
        /// Copy files to a project folder
        /// </summary>
        public void CopyFilesToProject(ProjectSettings settings, DirectoryInfo pDir)
        {
            if (!_project.StoreProjectFiles)
                return;

            DirectoryInfo sDir;

            List<FileInfo> allDsts = new List<FileInfo>();
            List<FileInfo> allSrcs = new List<FileInfo>();

            //copy background audio
            int bai = 1;
            foreach (ProjectBackgroundAudio ba in _project.BackgroundAudio)
            {
                try
                {
                    sDir = new DirectoryInfo(string.Format(@"{0}\(00 {1}) Background Music {2}", pDir.FullName, bai.ToString().PadLeft(3, '0'), bai.ToString()));

                    //gather all the song files and reassign the paths to the new location
                    foreach (AudioFile af in ba.AudioFiles)
                        af.UpdateName(addFile(new FileInfo(af.Name), sDir, allSrcs, allDsts).FullName);
                }
                catch
                {
                }
                bai++;
            }

            //copy songs
            foreach (ProjectTierSong pts in _project.Songs.Values)
            {
                try
                {
                    if (pts.Song != null)
                    {
                        ProjectSong ps = pts.Song;
                        sDir = ProjectSongSettingsFolder(pts, pDir);

                        //gather all the song files and reassign the paths to the new location
                        foreach (AudioFile af in ps.Audio.SongFiles)
                            af.UpdateName(addFile(new FileInfo(af.Name), sDir, allSrcs, allDsts).FullName);

                        if (ps.Audio.GuitarFile != null)
                            ps.Audio.GuitarFile.UpdateName(addFile(new FileInfo(ps.Audio.GuitarFile.Name), sDir, allSrcs, allDsts).FullName);

                        if (ps.Audio.RhythmFile != null)
                            ps.Audio.RhythmFile.UpdateName(addFile(new FileInfo(ps.Audio.RhythmFile.Name), sDir, allSrcs, allDsts).FullName);

                        foreach (NotesFile nf in ps.Notes.Files)
                            nf.Filename = addFile(new FileInfo(nf.Filename), sDir, allSrcs, allDsts).FullName;
                    }
                }
                catch
                {
                }
            }

            copyMoveProjectFiles(pDir, allSrcs, allDsts);

            //remove all files that aren't required
            if (pDir.Exists)
            {
                foreach (DirectoryInfo di in pDir.GetDirectories())
                {
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (!allDsts.Exists(delegate(FileInfo fn)
                            {
                                return fn.FullName.ToLower() == fi.FullName.ToLower();
                            }))
                        {
                            FileHelper.Delete(fi.FullName);
                        }
                    }
                }


                //delete all the empty folders
                foreach (DirectoryInfo di in pDir.GetDirectories())
                {
                    if (di.GetFiles().Length == 0 || allDsts.Find(delegate(FileInfo fi)
                        {
                            return di.FullName.ToLower() == fi.DirectoryName.ToLower();
                        }) == null) //folder not in dest lists
                    {
                        try
                        {
                            di.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
            }

        }

        private static void copyMoveProjectFiles(DirectoryInfo pDir, List<FileInfo> srcFiles, List<FileInfo> dstFiles)
        {
            string pDirLower; //project directory
            DirectoryInfo sDir;
            FileInfo src;
            FileInfo dst;

            //copy / move files in to position
            for (int i = 0; i < srcFiles.Count; i++)
            {
                src = srcFiles[i];
                dst = dstFiles[i];

                pDirLower = string.Concat(pDir.FullName.ToLower(), @"\");
                sDir = dst.Directory;

                if (src.FullName.ToLower() != dst.FullName.ToLower() && src.Exists)
                {

                    //move the file to the new folder
                    if (!sDir.Exists)
                        sDir.Create();

                    //is it in the project folder (may be in a different folder)
                    if (src.FullName.ToLower().StartsWith(pDirLower))
                    {
                        //if this source location exists more than once then we need to copy as it's been added to this project
                        //by being dragged from within the project folder, moving would remove it from the other location
                        if (srcFiles.FindAll(delegate(FileInfo f)
                            {
                                return f.FullName.ToLower() == src.FullName.ToLower();
                            }).Count > 1)
                        {
                            src.CopyTo(dst.FullName, true);
                            srcFiles[i] = dstFiles[i]; //now we have a copy the last one could be moved?
                        }
                        else
                            FileHelper.Move(src.FullName, dst.FullName);
                    }
                    else
                        src.CopyTo(dst.FullName, true);
                }
            }
        }


        private FileInfo addFile(FileInfo s, DirectoryInfo dd, List<FileInfo> srcFiles, List<FileInfo> outFiles)
        {
            FileInfo dFi = new FileInfo(string.Format(@"{0}\{1}", dd.FullName, s.Name));

            if (!dd.Exists)
                dd.Create();
            srcFiles.Add(s);
            outFiles.Add(dFi);

            return dFi;
        }
        #endregion

        public void DeleteFilesInWorkingFolders()
        {
            try
            {
                string s = _project.GetWorkingPath(WorkingFileType.RawWav);
                if (Directory.Exists(s))
                {
                    foreach (FileInfo fi in (new DirectoryInfo(s)).GetFiles())
                        FileHelper.Delete(fi.FullName);
                }

                s = _project.GetWorkingPath(WorkingFileType.Compressed);
                if (Directory.Exists(s))
                {
                    foreach (FileInfo fi in (new DirectoryInfo(s)).GetFiles())
                        FileHelper.Delete(fi.FullName);
                }

                s = _project.GetWorkingPath(WorkingFileType.GhFiles);
                if (Directory.Exists(s))
                {
                    foreach (FileInfo fi in (new DirectoryInfo(s)).GetFiles())
                        FileHelper.Delete(fi.FullName);
                }
            }
            catch
            {
            }
        }

        public void RemoveFiles(GameFileType type)
        {
            string[] keys = new string[_files.Count];
            _files.Keys.CopyTo(keys, 0);
            foreach (string k in keys)
            {
                if (_files[k].Type == type)
                    _files.Remove(k);
            }
        }

        public void RemoveAndDeleteFiles(GameFileType type)
        {
            string[] keys = new string[_files.Count];
            _files.Keys.CopyTo(keys, 0);
            foreach (string k in keys)
            {
                if (_files[k].Type == type)
                {
                    //#if (!DEBUG) //don't delete the game files
                    FileHelper.Delete(_files[k].LocalName);
                    //#endif

                    _files.Remove(k);
                }
            }
        }

        public void RemoveAndDeleteAllFiles()
        {
            string[] keys = new string[_files.Count];
            _files.Keys.CopyTo(keys, 0);
            foreach (string k in keys)
            {
                try
                {
                    //#if (!DEBUG) //don't delete the game files
                    FileHelper.Delete(_files[k].LocalName);
                    //#endif
                    _files.Remove(k);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// A new file has been created, used when new files are to be written/updated in the game.
        /// </summary>
        public void AddNew(GameFile[] gameFiles)
        {
            foreach (GameFile gf in gameFiles)
                _files.Add(gf.GameName, gf);
        }

        /// <summary>
        /// A new file has been created, used when new files are to be written/updated in the game.
        /// </summary>
        public void AddNew(GameFile gameFile)
        {
            _files.Add(gameFile.GameName, gameFile);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameFiles"></param>
        /// <param name="callback"></param>
        /// <returns>Failed gamefiles.</returns>
        public GameFile[] Import(GameFile[] gameFiles, FileCopyProgress callback)
        {
            List<GameFile> failed = new List<GameFile>();

            _fileCopy.SetSourcePath(_gameLocation);
            _fileCopy.ImportFiles(GameFile.CreateGameArray(gameFiles), GameFile.CreateLocalArray(gameFiles), delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(gameFiles[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });

            foreach (GameFile gf in gameFiles)
            {
                if (!io.File.Exists(gf.LocalName))
                    throw new ApplicationException("Failed to import files from the game source.");

                if (!_files.ContainsKey(gf.GameName))
                {
                    _files.Add(gf.GameName, gf);
                    gf.UpdateLocalDateTime();
                }
            }
            return failed.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameFileType"></param>
        /// <param name="callback"></param>
        /// <param name="gameFilenames"></param>
        /// <returns>Imported gamefiles.</returns>
        public GameFile[] Import(GameFileType gameFileType, FileCopyProgress callback, params string[] gameFilenames)
        {
            List<GameFile> gfs = new List<GameFile>();
            
            foreach (string s in gameFilenames)
                gfs.Add(new GameFile(s, localGameFilename(s), gameFileType));

            GameFile[] gf = gfs.ToArray();

            Import(gf, callback);
            return gf;
        }

        /// <summary>
        /// Read a single file from the game
        /// </summary>
        /// <param name="gameFile"></param>
        /// <returns>List of failed files</returns>
        public GameFile[] Import(GameFile gameFile, FileCopyProgress callback)
        {
            List<GameFile> failed = new List<GameFile>();
            List<GameFile> gf = new List<GameFile>();
            _fileCopy.SetSourcePath(_gameLocation);
            _fileCopy.ImportFile(gameFile.GameName, gameFile.LocalName, delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(gf[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });

            if (!io.File.Exists(gameFile.LocalName))
                throw new ApplicationException("Failed to import files from the game source.");
            
            _files.Add(gameFile.GameName, gameFile);

            return failed.ToArray();
        }

        /// <summary>
        /// Write files to the game
        /// </summary>
        /// <param name="gameFiles"></param>
        /// <returns>List of failed files</returns>
        public GameFile[] Export(GameFile[] gameFiles, FileCopyProgress callback)
        {
            List<GameFile> failed = new List<GameFile>();
            _fileCopy.SetSourcePath(_gameLocation);
            _fileCopy.ExportFiles(GameFile.CreateLocalArray(gameFiles), GameFile.CreateGameArray(gameFiles), delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(gameFiles[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });
            foreach (GameFile gf in gameFiles)
                gf.UpdateLocalDateTime();

            return failed.ToArray();
        }

        /// <summary>
        /// Write a single files to the game
        /// </summary>
        /// <param name="gameFile"></param>
        /// <returns>List of failed files</returns>
        public GameFile[] Export(GameFile gameFile, FileCopyProgress callback)
        {
            List<GameFile> failed = new List<GameFile>();
            List<GameFile> gf = new List<GameFile>();
            _fileCopy.SetSourcePath(_gameLocation);
            _fileCopy.ExportFile(gameFile.LocalName, gameFile.GameName, delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(gf[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });
            gameFile.UpdateLocalDateTime();

            return failed.ToArray();

        }

        /// <summary>
        /// Imports the QB/PAB/DBG paks
        /// </summary>
        /// <returns>List of failed files</returns>
        public GameFile[] ImportPaks(FileCopyProgress callback)
        {
            List<GameFile> gf = new List<GameFile>();
            GameFile pak = new GameFile(_project.GameInfo.QbPakFilename, localGameFilename(_project.GameInfo.QbPakFilename), GameFileType.QbPak);
            GameFile pab = null;
            List<GameFile> failed = new List<GameFile>();
            if (_project.GameInfo.HasQbPab)
                pab = new GameFile(_project.GameInfo.QbPabFilename, localGameFilename(_project.GameInfo.QbPabFilename), GameFileType.QbPab);
            GameFile dbg = new GameFile(_project.GameInfo.DbgPakFilename, localGameFilename(_project.GameInfo.DbgPakFilename), GameFileType.DbgPak);

            try
            {
                GameFile[] gameSongs;
                if (_project.GameInfo.HasQbPab)
                    gameSongs = GameFile.CreateArray(pak, pab, dbg);
                else
                    gameSongs = GameFile.CreateArray(pak, dbg);

                this.Import(gameSongs, delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                    {
                        if (!success)
                            failed.Add(gameSongs[fileNo]);
                        if (callback != null)
                            callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                    });

            }
            finally
            {
            }
            return failed.ToArray();
        }

        /// <summary>
        /// Imports the notes PAK files for all changed songs
        /// </summary>
        /// <returns>List of failed files</returns>
        public GameFile[] ImportNotesPaks(FileCopyProgress callback)
        {
            List<GameFile> gameSongs = new List<GameFile>();
            List<GameFile> failed = new List<GameFile>();

            string songName;
            foreach (ProjectSong song in _project.ChangedSongs)
            {
                //get notes pak
                songName = _project.GameInfo.GetNotesFilename(song.SongQb.Id);
                if (songName != null)
                    gameSongs.Add(new GameFile(songName, localGameFilename(songName), GameFileType.NotesPak));
            }

            this.Import(gameSongs.ToArray(), delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(gameSongs[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });

            return failed.ToArray();
        }

        /// <summary>
        /// Imports the notes PAK files for all songs in the EditSongs
        /// </summary>
        /// <returns>Array of extracted GameFiles</returns>
        public GameFile[] ImportBackgroundAudio(FileCopyProgress callback)
        {
            GameFile[] gameSongs = new GameFile[2];
            List<GameFile> failed = new List<GameFile>();

            gameSongs[0] = new GameFile(_project.GameInfo.StreamDat, localGameFilename(_project.GameInfo.StreamDat), GameFileType.Dat);
            gameSongs[1] = new GameFile(_project.GameInfo.StreamWad, localGameFilename(_project.GameInfo.StreamWad), GameFileType.Wad);

            this.Import(gameSongs, delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
            {
                if (!success)
                    failed.Add(gameSongs[fileNo]);
                if (callback != null)
                    callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
            });

            return gameSongs;
        }

        private string localGameFilename(string gameFilename)
        {
            gameFilename = gameFilename.Replace('/', '\\');
            int p = gameFilename.LastIndexOf('\\');
            return string.Format(@"{0}\{1}", _project.GetWorkingPath(WorkingFileType.GhFiles), gameFilename.Substring(p + 1));
        }


        /// <summary>
        /// Writes all modified files that have been read with FileImport or added with FileAddNew, skips dats when wads failed
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>List of failed files</returns>
        public GameFile[] ExportAllChangedSmart(FileCopyProgress callback)
        {
            List<GameFile> other = new List<GameFile>();
            List<GameFile> wad = new List<GameFile>();
            List<GameFile> dat = new List<GameFile>();
            List<GameFile> failed = new List<GameFile>();
            List<GameFile> skipped = new List<GameFile>();

            //don't get TheGHOST files
            foreach (GameFile gf in _project.FileManager.ChangedFiles())
            {
                switch (gf.Type)
                {
                    case GameFileType.Wad:
                        wad.Add(gf);
                        break;
                    case GameFileType.Dat:
                        dat.Add(gf);
                        break;
                    default:
                        other.Add(gf);
                        break;
                }
            }

            //now get TheGHOST files.  This means if we run out of room, it will hopefully be these files that fail



            _project.FileManager.Export(other.ToArray(), delegate (string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(other[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });

            if (failed.Count == 0)
            {
                _project.FileManager.Export(wad.ToArray(), delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                    {
                        if (!success)
                        {
                            failed.Add(wad[fileNo]);
                            skipped.Add(dat[fileNo - skipped.Count]);
                            dat.RemoveAt(fileNo - (skipped.Count - 1));
                        }
                        if (callback != null)
                            callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                    });

                _project.FileManager.Export(dat.ToArray(), delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                {
                    if (!success)
                        failed.Add(dat[fileNo]);
                    if (callback != null)
                        callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                });

                //raise event for skipped files
                for (int i = 0; i < skipped.Count; i++)
                {
                    if (callback != null)
                        callback(skipped[i].GameName, skipped[i].LocalName, i, skipped.Count, 100, false);
                }
            }

            return failed.ToArray();
        }


        /// <summary>
        /// Writes all modified files that have been read with FileImport or added with FileAddNew
        /// </summary>
        /// <returns>List of failed files</returns>
        public GameFile[] ExportAllChanged(FileCopyProgress callback)
        {
            GameFile[] _gfs = this.ChangedFiles();
            List<GameFile> failed = new List<GameFile>();

            if (_gfs.Length != 0)
                this.Export(_gfs, delegate(string gameFilename, string diskFilename, int fileNo, int fileCount, float percentage, bool success)
                    {
                        if (!success)
                            failed.Add(_gfs[fileNo]);
                        if (callback != null)
                            callback(gameFilename, diskFilename, fileNo, fileCount, percentage, success);
                    });

            return failed.ToArray();
        }

        public GameFile[] ChangedFiles()
        {
            List<GameFile> _gfs = new List<GameFile>();
            foreach (GameFile gf in _files.Values)
            {
                if (io.File.Exists(gf.LocalName) && gf.Changed)
                    _gfs.Add(gf);
            }

            return _gfs.ToArray();
        }

        /// <summary>
        /// Returns how many items are to be updated, useful for calculating progress
        /// </summary>
        /// <returns></returns>
        public int CountAllChanged()
        {
            int changed = 0;
            List<GameFile> _gfs = new List<GameFile>();
            foreach (GameFile gf in _files.Values)
            {
                if (io.File.Exists(gf.LocalName) && gf.Changed)
                    changed++;
            }
            return changed;
        }

        public GameFile File(string gameName)
        {
            if (_files.ContainsKey(gameName))
                return _files[gameName];
            else
                return null;
        }

        public DatWad BackgroundAudioDatWad
        {
            get
            {
                if (_backgroundAudioDatWad == null)
                {
                    GameFile[] st = this.ImportBackgroundAudio(null);

                    GameFile dat;
                    GameFile wad;

                    if (st[0].Type == GameFileType.Dat)
                    {
                        dat = st[0];
                        wad = st[1];
                    }
                    else
                    {
                        dat = st[1];
                        wad = st[0];
                    }

                    _backgroundAudioDatWad = new DatWad(dat.LocalName, wad.LocalName, this.PakFormat.EndianType);
                }

                return _backgroundAudioDatWad;
            }
        }

        #region PAK and QB file routines

        public PakFormat PakFormat
        {
            get
            {
                if (_pakFormat == null)
                {
                    if (_project.GameInfo.HasQbPab)
                        _pakFormat = new PakFormat(this.File(_project.GameInfo.QbPakFilename).LocalName, this.File(_project.GameInfo.QbPabFilename).LocalName, this.File(_project.GameInfo.DbgPakFilename).LocalName, _project.GameInfo.PakFormatType);
                    else
                        _pakFormat = new PakFormat(this.File(_project.GameInfo.QbPakFilename).LocalName, string.Empty, this.File(_project.GameInfo.DbgPakFilename).LocalName, _project.GameInfo.PakFormatType);
                }

                return _pakFormat;

            }
        }

        public PakEditor QbPakEditor
        {
            get
            {
                if (_pakEditor == null)
                    _pakEditor = new PakEditor(this.PakFormat);
                return _pakEditor;
            }
        }

        public PakEditor DbgPakEditor
        {
            get
            {
                if (_dbgEditor == null)
                    _dbgEditor = new PakEditor(this.PakFormat, true);
                return _dbgEditor;
            }
        }

        public QbFile SongListQbFile
        {
            get
            {
                if (_songListQbFile == null)
                    _songListQbFile = this.LoadQbFile(_project.GameInfo.SonglistQbFilename);
                return _songListQbFile;
            }
        }

        public QbFile GuitarProgressionQbFile
        {
            get
            {
                if (_guitarProgressionQbFile == null)
                    _guitarProgressionQbFile = this.LoadQbFile(_project.GameInfo.GuitarProgressionQbFilename);
                return _guitarProgressionQbFile;
            }
        }

        public QbFile GuitarCoOpQbFile
        {
            get
            {
                if (_project.GameInfo.Game == Game.GH3_Wii)
                {

                    if (_guitarCoOpQbFile == null)
                        _guitarCoOpQbFile = this.LoadQbFile(_project.GameInfo.GuitarCoOpQbFilename);
                    return _guitarCoOpQbFile;
                }
                else
                    return null;
            }
        }

        public QbFile StoreDataQbFile
        {
            get
            {
                if (_storeDataQbFile == null)
                    _storeDataQbFile = this.LoadQbFile(_project.GameInfo.StoreDataQbFilename);
                return _storeDataQbFile;
            }
        }

        private string loadDbgQBFile(string qbFilename)
        {
            string dbgFilename = qbFilename.Replace(string.Format(".qb.{0}", this.PakFormat.FileExtension), string.Format(".{0}", this.PakFormat.QbDebugExtension));

            string key = null;

            try
            {
                if (this.DbgPakEditor != null)
                {
                    if (this.DbgPakEditor.Headers.ContainsKey(dbgFilename.ToLower()))
                        key = dbgFilename.ToLower();
                    else
                    {
                        //use the qb pak headers to lookup the debugQbKeys.
                        //the debug pak has QBKeys as the keys instead of filenames
                        foreach (PakHeaderItem phi in this.QbPakEditor.Headers.Values)
                        {
                            if (phi.Filename.ToLower() == qbFilename)
                            {
                                key = phi.DebugQbKey.ToString("X").PadLeft(8, '0').ToLower();
                                break;
                            }
                        }
                    }

                    if (key != null && this.DbgPakEditor != null && this.DbgPakEditor.Headers.ContainsKey(key))
                        return this.DbgPakEditor.ExtractFileToString(key);
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        public QbFile LoadQbFile(string qbFilename)
        {
            string dbgFileContents = loadDbgQBFile(qbFilename);

            try
            {
                return this.QbPakEditor.ReadQbFile(qbFilename, dbgFileContents);
            }
            catch (Exception ex)
            {
                //TODO: better exception / error message
                throw new ApplicationException("PAK Extract / QB Parse Error", ex);
            }
        }

        public void UpdatePaks()
        {
            //if the PAKs have been removed by the user then just get them back out, our QB is safe in memory.
            
            //IF THIS IS BROUGHT BACK ENSURE THAT FOR GHA THAT THE MARKER TEXT CAN BE WRITTEN (This method is called after the notes and markers are written)
            //if (!IO.File.Exists(localGameFilename(_project.GameInfo.QbPakFilename)))
            //    this.ImportPaks(null);

            this.StoreDataQbFile.AlignPointers();
            this.StoreDataQbFile.IsValid();
            this.QbPakEditor.ReplaceFile(_project.GameInfo.StoreDataQbFilename, this.StoreDataQbFile);

            this.SongListQbFile.AlignPointers();
            this.SongListQbFile.IsValid();
            this.QbPakEditor.ReplaceFile(_project.GameInfo.SonglistQbFilename, this.SongListQbFile);

            this.GuitarProgressionQbFile.AlignPointers();
            this.GuitarProgressionQbFile.IsValid();
            this.QbPakEditor.ReplaceFile(_project.GameInfo.GuitarProgressionQbFilename, this.GuitarProgressionQbFile);

            if (this.GuitarCoOpQbFile != null) //GH3 only
            {
                this.GuitarCoOpQbFile.AlignPointers();
                this.GuitarCoOpQbFile.IsValid();
                this.QbPakEditor.ReplaceFile(_project.GameInfo.GuitarCoOpQbFilename, this.GuitarCoOpQbFile);
            }
        }



        #endregion

        private PakEditor _pakEditor;
        private PakEditor _dbgEditor;
        private PakFormat _pakFormat;

        private QbFile _songListQbFile;
        private QbFile _guitarProgressionQbFile;
        private QbFile _guitarCoOpQbFile;
        private QbFile _storeDataQbFile;

        private DatWad _backgroundAudioDatWad;

        private Project _project;
        private IPluginFileCopy _fileCopy;
        private string _gameLocation;
        private Dictionary<string, GameFile> _files; //keyed on gamename

    }
}
