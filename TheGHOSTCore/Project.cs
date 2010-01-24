using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

using System.Reflection;

namespace Nanook.TheGhost
{

    public enum WorkingFileType
    {
        RawWav,
        Notes,
        GhFiles,
        Compressed,
        Root
    }

    public class Project
    {

        public Project(TheGhostCore core)
        {
            _audioExport = null;
            _audioImport = null;
            _fileCopy = null;
            _audioImportInfo = null;
            _audioExportInfo = null;
            _fileCopyInfo = null;

            _songs = null;
            _tiers = null;

            _backgroundAudio = new List<ProjectBackgroundAudio>();

            _fileManager = null;
            _core = core;

            _theGhostInfo = null;

            _defaults = new ProjectDefaults();

            _storeProjectFiles = false;

            _projectSettings = new ProjectSettings(this);
            _projectSettingsGameMods = new ProjectSettingsGameMods(this);
        }

        public void LoadProject(string projectFile)
        {
            _projectSettings = new ProjectSettings(projectFile, this);
            PluginManager pm = _core.PluginManager;

            string aiName = pm.GetPluginNameFromTypeName(pm.AudioImportPlugins, _projectSettings.AudioImportPluginName);
            PluginInfo aiP = null;
            if (pm.AudioImportPlugins.ContainsKey(aiName))
                aiP = pm.AudioImportPlugins[aiName];

            string aoName = pm.GetPluginNameFromTypeName(pm.AudioExportPlugins, _projectSettings.AudioExportPluginName);
            PluginInfo aoP = null;
            if (pm.AudioExportPlugins.ContainsKey(aoName))
                aoP = pm.AudioExportPlugins[aoName];

            string exName = pm.GetPluginNameFromTypeName(pm.FileCopyPlugins, _projectSettings.FileCopyPluginName);
            PluginInfo exP = null;
            if (pm.FileCopyPlugins.ContainsKey(exName))
                exP = pm.FileCopyPlugins[exName];

            this.SetPlugins(aiP, aoP, exP);

        }

        internal ProjectSettings Settings
        {
            get { return _projectSettings; }
        }

        public void Save()
        {
            _projectSettings.Save();
        }

        public void SaveAs(string filename)
        {
            _projectSettings.SaveAs(filename);
        }

        public void SetPlugins(PluginInfo audioImportPluginInfo, PluginInfo audioExportPluginInfo, PluginInfo fileCopyPluginInfo, params PluginInfo[] editorPluginInfos)
        {
            _fileCopyInfo = fileCopyPluginInfo;
            _audioExportInfo = audioExportPluginInfo;
            _audioImportInfo = audioImportPluginInfo;

            _projectSettings.AudioImportPluginName = _audioImportInfo == null ? string.Empty : _audioImportInfo.TypeName;
            _projectSettings.AudioExportPluginName = _audioExportInfo == null ? string.Empty : _audioExportInfo.TypeName;
            _projectSettings.FileCopyPluginName = _fileCopyInfo == null ? string.Empty : _fileCopyInfo.TypeName;
            _projectSettings.EditorPluginNames = null;

            if (_audioImportInfo != null)
                _audioImport = (IPluginAudioImport)_core.PluginManager.LoadPlugin(_audioImportInfo);
            if (_audioExportInfo != null)
                _audioExport = (IPluginAudioExport)_core.PluginManager.LoadPlugin(_audioExportInfo);
            if (_fileCopyInfo != null)
                _fileCopy = (IPluginFileCopy)_core.PluginManager.LoadPlugin(_fileCopyInfo);
            _fileManager = new FileManager(this, _fileCopy, this.SourcePath);

        }

        public void ReadCoreFiles()
        {

            _songs = null; //force tierlist to be rebuilt
            _tiers = null;
            _backgroundAudio.Clear();
            _gameInfo = null;

            this.FileManager.ImportPaks(null);

//#if (!DEBUG)
            DatWad dw = this.FileManager.BackgroundAudioDatWad;
            foreach (DatItem di in dw.GetBackgroundAudioItems())
                this.BackgroundAudio.Add(new ProjectBackgroundAudio(this, dw, di));

            ProjectBackgroundAudio a;
            ProjectBackgroundAudio b;
            for (int i = 0; i < this.BackgroundAudio.Count - 1; i++)
            {
                for (int j = 0; j < this.BackgroundAudio.Count - 1 - i; j++)
                {
                    a = this.BackgroundAudio[j];
                    b = this.BackgroundAudio[j + 1];
                    if (string.Compare(a.Name.ToLower(), b.Name.ToLower()) > 0)
                    {
                        this.BackgroundAudio[j] = b;
                        this.BackgroundAudio[j + 1] = a;
                    }
                }
            }

            _projectSettings.LoadSettings();

//#endif

        }

        public void SetDefaults(ProjectDefaults defaults)
        {
            _defaults = defaults;
        }

        public FileManager FileManager
        {
            get { return _fileManager; }
        }

        /// <summary>
        /// Used by the ProjectSonAudio class
        /// </summary>
        internal IPluginAudioExport AudioExport
        {
            get { return _audioExport; }
        }

        /// <summary>
        /// Used by the ProjectSonAudio class
        /// </summary>
        internal IPluginAudioImport AudioImport
        {
            get { return _audioImport; }
        }

        public PluginInfo AudioExportInfo
        {
            get { return _audioExportInfo; }
        }

        public PluginInfo AudioImportInfo
        {
            get { return _audioImportInfo; }
        }

        public PluginInfo FileCopyPluginInfo
        {
            get { return _fileCopyInfo; }
        }

        public string WorkingPath
        {
            get { return _defaults.WorkingFolder; }
            set { _defaults.WorkingFolder = value; }
        }

        public string GetWorkingPath(WorkingFileType fileType)
        {
            string f;

            switch (fileType)
            {
                case WorkingFileType.RawWav:
                    f = "AudioRaw";
                    break;
                case WorkingFileType.Notes:
                    f = "SourceFiles";
                    break;
                case WorkingFileType.GhFiles:
                    f = "GameFiles";
                    break;
                case WorkingFileType.Compressed:
                    f = "AudioCompressed";
                    break;
                default:
                    f = "";
                    break;
            }

            string pth = string.Format(@"{0}\{1}", this.WorkingPath.TrimEnd('\\'), f).TrimEnd('\\');

            if (pth != null && pth.Length != 0 && !Directory.Exists(pth))
                Directory.CreateDirectory(pth);

            return pth;
        }

        public string SourcePath
        {
            get { return _projectSettings.SourcePath; }
            set { _projectSettings.SourcePath = value; }
        }

        public Game GameId
        {
            get { return (Game)Enum.Parse(typeof(Game), _projectSettings.GameId); }
            set
            {
                _gameInfo = null;
                _projectSettings.GameId = value.ToString();
            }
        }

        public string Filename
        {
            get { return _projectSettings.Filename; }
            set { _projectSettings.Filename = value; }
        }

        public bool StoreProjectFiles
        {
            get { return _storeProjectFiles; }
            set { _storeProjectFiles = value; }
        }

        public GameInfo GameInfo
        {
            get
            {
                if (_gameInfo == null)
                    _gameInfo = new GameInfo(this.GameId, this.LanguageId);

                return _gameInfo;
            }
            set { _gameInfo = value; }
        }

        public ProjectSettingsGameMods GameModsSettings
        {
            get { return _projectSettingsGameMods; }
        }

        public GameMods GameMods
        {
            get
            {
                if (_gameMods == null)
                    _gameMods = new GameMods(this);

                return _gameMods;
            }
            set { _gameMods = value; }
        }

        public string LanguageId
        {
            get { return _projectSettings.LanguageId; }
            set { _projectSettings.LanguageId = value; }
        }

        #region Game Tier and Song List

        private void buildCompleteList()
        {
            try
            {
                QbKey qkKqCredits = QbKey.Create("kingsandqueenscredits");

                Dictionary<uint, SongQb> songListComplete = new Dictionary<uint, SongQb>();

                _songs = new Dictionary<uint, ProjectTierSong>();
                _tiers = new List<ProjectTier>();


                QbFile slQb = this.FileManager.SongListQbFile;
                QbItemBase permSongsQi = slQb.FindItem(QbKey.Create("permanent_songlist_props"), false);

                if (permSongsQi != null)
                {
                    permSongsQi.FindItem(false, delegate(QbItemBase item)
                    {
                        if (item.QbItemType == QbItemType.StructItemStruct)
                        {
                            SongQb s = new SongQb(this, (QbItemStruct)item);
                            if ((s.IsSetList || s.Id.Crc == qkKqCredits.Crc) && !songListComplete.ContainsKey(s.Id.Crc))
                                songListComplete.Add(s.Id.Crc, s);
                        }
                        return false; //if we return false then the search continues
                    });
                }

                int tierNo = 1;
                //career
                foreach (TierQb t in BuildTierList(this.FileManager.GuitarProgressionQbFile, QbKey.Create("GH3_Career_Songs")))
                    _tiers.Add(new ProjectTier(t, TierType.Career, tierNo++));
                //bonus
                foreach (TierQb t in BuildTierList(this.FileManager.StoreDataQbFile, QbKey.Create("GH3_Bonus_Songs")))
                    _tiers.Add(new ProjectTier(t, TierType.Bonus, tierNo++));

                int si;
                foreach (ProjectTier pt in _tiers)
                {
                    si = 1;
                    pt.Save(songListComplete);
                    foreach (ProjectTierSong pts in pt.Songs)
                    {
                        _songs.Add(pts.SongQb.Id.Crc, pts);
                        if (pt.Type == TierType.Bonus)
                            pts.IsBonusSong = true;
                        pts.Number = si++;
                    }
                }

                int gs = this.AddedGhostSongCount;
                //build a list of keys
                List<QbKey> theGhostSongs = new List<QbKey>(gs);
                for (int i = 1; i <= gs; i++)
                {
                    QbKey qk = QbKey.Create(string.Format("theghost{0}", i.ToString().PadLeft(3, '0')));

                    if (_songs.ContainsKey(qk.Crc))
                        _songs[qk.Crc].IsAddedWithTheGhost = true;
                }

                List<TierQb> quickList = BuildTierList(this.FileManager.GuitarProgressionQbFile, QbKey.Create("GH3_General_Songs")); //quickplay

                //find the non career songs or bonus
                ProjectTier ncTier = new ProjectTier(null);
                foreach (TierQb tier in quickList)
                {
                    foreach (QbKey song in tier.Songs)
                    {
                        if (!_songs.ContainsKey(song.Crc))
                        {
                            if (songListComplete.ContainsKey(song.Crc))
                            {
                                ProjectTierSong pts = new ProjectTierSong(songListComplete[song.Crc]);
                                _songs.Add(song.Crc, pts);
                                pts.Tier = ncTier;
                                ncTier.InsertSong(pts, ncTier.Songs.Count);
                            }
                            else
                            {
                            }
                        }
                    }
                }

                if (this.GameInfo.Game == Game.GHA_Wii)
                {
                    QbKey song = qkKqCredits;
                    if (!_songs.ContainsKey(song.Crc) && songListComplete.ContainsKey(song.Crc))
                    {
                        ProjectTierSong pts = new ProjectTierSong(songListComplete[song.Crc]);
                        _songs.Add(song.Crc, pts);
                        pts.Tier = ncTier;
                        ncTier.InsertSong(pts, ncTier.Songs.Count);
                    }
                }

                if (ncTier.Songs.Count != 0)
                {
                    ncTier.Type = TierType.NonCareer;
                    _tiers.Add(ncTier);
                }

                //foreach (ProjectTierSong pts in _songs.Values)
                //    System.Diagnostics.Debug.WriteLine(pts.SongQb.Id.Text);

            }
            catch
            {
                throw;
            }
        }

        internal List<TierQb> BuildTierList(QbFile fileQb, QbKey tiersQk)
        {
            List<TierQb> tierList = new List<TierQb>();

            QbItemBase tiersQi = fileQb.FindItem(tiersQk, false);

            if (tiersQi != null)
            {
                tiersQi.FindItem(false, delegate(QbItemBase item)
                {
                    if (item.QbItemType == QbItemType.StructItemStruct)
                        tierList.Add(new TierQb((QbItemStruct)item));
                    return false; //if we return false then the search continues
                });
            }

            foreach (TierQb t in tierList)
            {
                for (int i = 0; i < t.Songs.Length; i++)
                {
                    if (this.Songs.ContainsKey(t.Songs[i].Crc))
                        t.Songs[i] = this.Songs[t.Songs[i].Crc].SongQb.Id; //get the key from the songlist (it has the text)
                }
            }


            return tierList;

        }
        #endregion

        public ProjectSong CreateProjectSong(SongQb songQb, bool recordChange)
        {
            ProjectSong ps = new ProjectSong(this, songQb);
            if (recordChange)
                ((ISettingsChange)ps).RecordChange();
            return ps;
        }

        public List<ProjectBackgroundAudio> BackgroundAudio
        {
            get { return _backgroundAudio; }
        }

        public ProjectSong[] ChangedSongs
        {
            get
            {
                List<ProjectSong> songs = new List<ProjectSong>();
                foreach (ProjectTierSong pts in _songs.Values)
                {
                    if (pts.Song != null && (pts.Song.LastChanged > pts.Song.LastApplied || this.Defaults.ReapplyAll))
                        songs.Add(pts.Song);
                }
                return songs.ToArray();
            }
        }

        public ProjectSong[] EditSongs
        {
            get
            {
                List<ProjectSong> es = new List<ProjectSong>();
                foreach (ProjectTierSong pts in _songs.Values)
                {
                    if (pts.IsEditSong && pts.Song != null)
                        es.Add(pts.Song);
                }
                return es.ToArray();
            }
        }

        public ProjectDefaults Defaults
        {
            get { return _defaults; }
        }

        /// <summary>
        /// Get the amount of songs added to the ISO by TheGHOST
        /// </summary>
        public int AddedGhostSongCount
        {
            get
            {
                if (_theGhostInfo == null)
                {
                    _theGhostInfo = string.Empty;
                    _theGhostSongsNo = 0; //no added songs
                    try
                    {
                        GameFile[] files = this.FileManager.Import(GameFileType.Other, null, @"-TheGHOST-/info.txt");

                        if (files.Length == 1 && File.Exists(files[0].LocalName))
                        {
                            string[] txt = File.ReadAllLines(files[0].LocalName);
                            if (txt.Length > 1)
                            {
                                if (txt[1].StartsWith("TheGHOSTSongs="))
                                    _theGhostSongsNo = int.Parse(txt[1].Split('=')[1]);
                            }
                            FileHelper.Delete(files[0].LocalName);
                        }
                    }
                    catch
                    {
                    }
                }
                return _theGhostSongsNo;
            }
        }

        /// <summary>
        /// loop through the tiers and bonus songs to get all the songs in a consistent order
        /// </summary>
        /// <returns></returns>
        public Dictionary<uint, ProjectTierSong> Songs
        {
            get
            {
                if (_songs == null)
                    buildCompleteList();
                return _songs;
            }
        }

        public List<ProjectTier> Tiers
        {
            get
            {
                if (_songs == null)
                    buildCompleteList();
                return _tiers;
            }
        }

        public string WorkingFolder
        {
            get { return _workingFolder; }
            set { _workingFolder = value; }
        }

        private string _theGhostInfo;
        private int _theGhostSongsNo;

        private ProjectDefaults _defaults;

        private Dictionary<uint, ProjectTierSong> _songs;
        private List<ProjectTier> _tiers;

        private List<ProjectBackgroundAudio> _backgroundAudio;

        private IPluginAudioExport _audioExport;
        private IPluginAudioImport _audioImport;
        private IPluginFileCopy _fileCopy;
        private PluginInfo _fileCopyInfo;
        private PluginInfo _audioExportInfo;
        private PluginInfo _audioImportInfo;

        private GameInfo _gameInfo;
        private FileManager _fileManager;
        private GameMods _gameMods;

        private TheGhostCore _core;
        private ProjectSettingsGameMods _projectSettingsGameMods;
        private ProjectSettings _projectSettings;

        private string _workingFolder;

        private bool _storeProjectFiles;
    }
}
