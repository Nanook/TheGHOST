using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Configuration;

using Nanook.QueenBee.Parser;

using System.Xml;

namespace Nanook.TheGhost
{
    public class ProjectSettings
    {

        internal ProjectSettings(Project project)
        {
            _project = project;
            _tierNames = null;
        }

        internal ProjectSettings(string projectFile, Project project)
        {
            _project = project;
            _projectFile = projectFile;

            _tierNames = new List<string>();

            if (File.Exists(_projectFile))
            {
                XmlReaderSettings xmlS = new XmlReaderSettings();
                xmlS.IgnoreComments = true;
                xmlS.IgnoreProcessingInstructions = true;
                xmlS.IgnoreWhitespace = true;

                using (XmlReader xml = XmlReader.Create(_projectFile, xmlS))
                {

                    while (!xml.EOF && xml.Read())
                    {
                        if (xml.Name == "xml")
                            break;
                    }

                    while (!xml.EOF && xml.Read())
                    {
                        if (xml.Name == "TheGHOST_ProjectSettings")
                        {
                            try
                            {
                                #region TheGHOST_ProjectSettings
                                string version = xml.GetAttribute("version");
                                if (version == "1")
                                {
                                    _project.StoreProjectFiles = bool.Parse(xml.GetAttribute("storeFiles"));

                                    while (!xml.EOF && xml.Read())
                                    {
                                        switch (xml.Name)
                                        {
                                            case "Plugins":
                                                #region Plugins
                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.Element)
                                                    {
                                                        switch (xml.Name)
                                                        {
                                                            case "AudioExportPlugin":
                                                                this.AudioExportPluginName = xml.ReadString();
                                                                break;

                                                            case "AudioImportPlugin":
                                                                this.AudioImportPluginName = xml.ReadString();
                                                                break;

                                                            case "FileCopyPlugin":
                                                                this.FileCopyPluginName = xml.ReadString();
                                                                break;

                                                            case "EditorPlugins":
                                                                break;
                                                        }
                                                    }
                                                    else if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Plugins")
                                                        break;
                                                }
                                                #endregion
                                                break;

                                            case "Settings":
                                                #region Settings
                                                this.GameId = xml.GetAttribute("game");
                                                this.LanguageId = xml.GetAttribute("language");

                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.Element)
                                                    {
                                                        switch (xml.Name)
                                                        {
                                                            case "SourcePath":
                                                                this.SourcePath = xml.ReadString();
                                                                break;
                                                        }
                                                    }
                                                    else if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Settings")
                                                        break;
                                                }
                                                #endregion
                                                break;

                                            case "Defaults":
                                                #region Defaults
                                                _project.Defaults.HoPoMeasure = float.Parse(xml.GetAttribute("hammerOnMeasure"));
                                                _project.Defaults.GuitarVolume = float.Parse(xml.GetAttribute("guitarVolume"));
                                                _project.Defaults.GuitarVolumeMode = (DefaultSettingMode)Enum.Parse(typeof(DefaultSettingMode), xml.GetAttribute("guitarVolumeMode"));
                                                _project.Defaults.MinMsBeforeNotesStart = int.Parse(xml.GetAttribute("minMsBeforeNotesStart"));
                                                _project.Defaults.SmartModeCrowdImport = bool.Parse(xml.GetAttribute("smartModeCrowdImport"));
                                                _project.Defaults.ForceMono = (xml.GetAttribute("forceMono") == null) ? _project.Defaults.ForceMono : bool.Parse(xml.GetAttribute("forceMono"));
                                                _project.Defaults.ForceDownSample = (xml.GetAttribute("forceDownSample") == null) ? _project.Defaults.ForceDownSample : int.Parse(xml.GetAttribute("forceDownSample"));
                                                _project.Defaults.PreviewFadeLength = int.Parse(xml.GetAttribute("previewFadeLength"));
                                                _project.Defaults.PreviewIncludeGuitar = bool.Parse(xml.GetAttribute("previewIncludeGuitar"));
                                                _project.Defaults.PreviewIncludeRhythm = bool.Parse(xml.GetAttribute("previewIncludeRhythm"));
                                                _project.Defaults.PreviewLength = int.Parse(xml.GetAttribute("previewLength"));
                                                _project.Defaults.PreviewStart = int.Parse(xml.GetAttribute("previewStart"));
                                                _project.Defaults.PreviewVolume = int.Parse(xml.GetAttribute("previewVolume"));
                                                _project.Defaults.AudioSongVolume = (xml.GetAttribute("audioSongVolume") == null) ? _project.Defaults.AudioSongVolume : int.Parse(xml.GetAttribute("audioSongVolume"));
                                                _project.Defaults.AudioGuitarVolume = (xml.GetAttribute("audioGuitarVolume") == null) ? _project.Defaults.AudioGuitarVolume : int.Parse(xml.GetAttribute("audioGuitarVolume"));
                                                _project.Defaults.AudioRhythmVolume = (xml.GetAttribute("audioRhythmVolume") == null) ? _project.Defaults.AudioRhythmVolume : int.Parse(xml.GetAttribute("audioRhythmVolume"));
                                                _project.Defaults.Singer = (Singer)Enum.Parse(typeof(Singer), xml.GetAttribute("singer"));
                                                _project.Defaults.SingerMode = (DefaultSettingMode)Enum.Parse(typeof(DefaultSettingMode), xml.GetAttribute("singerMode"));
                                                _project.Defaults.SongVolume = float.Parse(xml.GetAttribute("songVolume"));
                                                _project.Defaults.SongVolumeMode = (DefaultSettingMode)Enum.Parse(typeof(DefaultSettingMode), xml.GetAttribute("songVolumeMode"));
                                                _project.Defaults.Year = xml.GetAttribute("year");
                                                _project.Defaults.YearMode = (DefaultSettingModeBlank)Enum.Parse(typeof(DefaultSettingModeBlank), xml.GetAttribute("yearMode"));
                                                _project.Defaults.Gh3SustainClipping = bool.Parse(xml.GetAttribute("gh3SustainClipping"));
                                                _project.Defaults.ForceNoStarPower = (xml.GetAttribute("forceNoStarPower") == null) ? _project.Defaults.ForceNoStarPower : bool.Parse(xml.GetAttribute("forceNoStarPower"));
                                                #endregion
                                                break;
                                            #region Loaded later
                                            case "QbMods":
                                                #region QbMods
                                                if (xml.GetAttribute("addNonCareerTracksToBonus") != null) //version change renamed this value
                                                    _project.GameModsSettings.AddNonCareerTracksToBonus = bool.Parse(xml.GetAttribute("addNonCareerTracksToBonus"));
                                                else
                                                    _project.GameModsSettings.AddNonCareerTracksToBonus = bool.Parse(xml.GetAttribute("addCoOpTracksToBonus"));
                                                if (xml.GetAttribute("unlockSetlists") != null) //version change renamed this value
                                                    _project.GameModsSettings.UnlockSetlists = bool.Parse(xml.GetAttribute("unlockSetlists"));
                                                else
                                                    _project.GameModsSettings.UnlockSetlists = bool.Parse(xml.GetAttribute("unlockCareerTiers"));
                                                _project.GameModsSettings.CompleteTier1Song = bool.Parse(xml.GetAttribute("completeTier1Song"));
                                                _project.GameModsSettings.DefaultBonusSongArt = bool.Parse(xml.GetAttribute("defaultBonusSongArt"));
                                                _project.GameModsSettings.FreeStore = bool.Parse(xml.GetAttribute("freeStore"));
                                                _project.GameModsSettings.SetCheats = bool.Parse(xml.GetAttribute("setCheats"));
                                                if (xml.GetAttribute("defaultBonusSongInfo") != null)
                                                {
                                                    _project.GameModsSettings.DefaultBonusSongInfo = bool.Parse(xml.GetAttribute("defaultBonusSongInfo"));
                                                    _project.GameModsSettings.DefaultBonusSongInfoText = xml.GetAttribute("defaultBonusSongInfoText");
                                                }

                                                _project.GameModsSettings.LastApplied = DateTime.Parse(xml.GetAttribute("lastApplied"), null, System.Globalization.DateTimeStyles.AssumeLocal);
                                                DateTime qbModsLastChanged = DateTime.Parse(xml.GetAttribute("lastChanged"), null, System.Globalization.DateTimeStyles.AssumeLocal);

                                                int ti = 0;
                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.Element && xml.Name == "TierName" /*&& ti < tc*/) //cater for all tiers doesn't really matter on load
                                                        _project.GameModsSettings.SetTierName(ti++, xml.ReadString());
                                                    else if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "QbMods")
                                                        break;
                                                }
                                                ((ISettingsChange)_project.GameModsSettings).RecordChange();
                                                _project.GameModsSettings.LastChanged = qbModsLastChanged;
                                                #endregion
                                                break;

                                            case "BackgroundMusic":
                                                #region BackgroundMusic
                                                #endregion
                                                break;

                                            case "Songs":
                                                #region Songs
                                                #endregion
                                                break;
                                            #endregion
                                        }
                                    }
                                }
                                else
                                    throw new ApplicationException(string.Format("Version '{0}' project files are not supported", version));

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(string.Format("Exception loading project: {0}", ex.Message));
                            }
                        }
                        else if (xml.Name == "configuration")
                            throw new ApplicationException("This project format is from an old version and no longer supported");
                        else
                            throw new ApplicationException("This project format is not recognised");
                    }
                }
            }

        }

        private string pathAbsToRel(string filename, DirectoryInfo pDir, bool noPath)
        {
            if (noPath)
                return string.Concat(@"~\", (new FileInfo(filename)).Name);

            if (_project.StoreProjectFiles && filename.ToLower().StartsWith(string.Concat(pDir.FullName.ToLower(), @"\")))
                return string.Concat(@"~\", filename.Substring(pDir.FullName.Length + 1));
            return filename;
        }

        private string pathRelToAbs(string path, DirectoryInfo pDir)
        {
            if (path.StartsWith(@"~\"))
                return string.Concat(pDir.FullName, path.Substring(1));
            return path;
        }

        internal void Save()
        {
            XmlWriterSettings xmlS = new XmlWriterSettings();
            xmlS.CheckCharacters = true;
            xmlS.CloseOutput = true;
            xmlS.Indent = true;
            xmlS.IndentChars = "  ";

            string bak = string.Concat(_projectFile, ".bak");
            string tmp = string.Concat(_projectFile, DateTime.Now.ToString("yyyyMMddHHmmss"));

            FileHelper.Delete(bak);
            if (File.Exists(_projectFile))
                File.Copy(_projectFile, bak, true);
            FileHelper.Delete(tmp);

            FileInfo pFile = new FileInfo(this.Filename);
            DirectoryInfo pDir = new DirectoryInfo(pFile.FullName.Substring(0, pFile.FullName.Length - pFile.Extension.Length));

            _project.FileManager.CopyFilesToProject(this, pDir);
            string currVersion = "1";

            try
            {
                using (XmlWriter xml = XmlWriter.Create(tmp, xmlS))
                {
                    xml.WriteStartDocument();
                    #region TheGHOST_ProjectSettings
                    xml.WriteStartElement("TheGHOST_ProjectSettings");
                    xml.WriteAttributeString("version", currVersion);
                    xml.WriteAttributeString("storeFiles", _project.StoreProjectFiles.ToString());

                    xml.WriteStartElement("Plugins");
                    xml.WriteElementString("AudioExportPlugin", this.AudioExportPluginName);
                    xml.WriteElementString("AudioImportPlugin", this.AudioImportPluginName);
                    xml.WriteElementString("FileCopyPlugin", this.FileCopyPluginName);
                    xml.WriteElementString("EditorPlugins", this.EditorPluginNames != null ? this.FileCopyPluginName : string.Empty);
                    xml.WriteEndElement(); //Plugins


                    xml.WriteStartElement("Settings");
                    xml.WriteAttributeString("game", this.GameId);
                    xml.WriteAttributeString("language", this.LanguageId);
                    xml.WriteElementString("SourcePath", this.SourcePath);
                    xml.WriteEndElement(); //Settings

                    xml.WriteStartElement("Defaults");
                    xml.WriteAttributeString("hammerOnMeasure", _project.Defaults.HoPoMeasure.ToString());
                    xml.WriteAttributeString("guitarVolume", _project.Defaults.GuitarVolume.ToString());
                    xml.WriteAttributeString("guitarVolumeMode", _project.Defaults.GuitarVolumeMode.ToString());
                    xml.WriteAttributeString("minMsBeforeNotesStart", _project.Defaults.MinMsBeforeNotesStart.ToString());
                    xml.WriteAttributeString("smartModeCrowdImport", _project.Defaults.SmartModeCrowdImport.ToString());
                    xml.WriteAttributeString("forceMono", _project.Defaults.ForceMono.ToString());
                    xml.WriteAttributeString("forceDownSample", _project.Defaults.ForceDownSample.ToString());
                    xml.WriteAttributeString("previewFadeLength", _project.Defaults.PreviewFadeLength.ToString());
                    xml.WriteAttributeString("previewIncludeGuitar", _project.Defaults.PreviewIncludeGuitar.ToString());
                    xml.WriteAttributeString("previewIncludeRhythm", _project.Defaults.PreviewIncludeRhythm.ToString());
                    xml.WriteAttributeString("previewLength", _project.Defaults.PreviewLength.ToString());
                    xml.WriteAttributeString("previewStart", _project.Defaults.PreviewStart.ToString());
                    xml.WriteAttributeString("previewVolume", _project.Defaults.PreviewVolume.ToString());
                    xml.WriteAttributeString("audioSongVolume", _project.Defaults.AudioSongVolume.ToString());
                    xml.WriteAttributeString("audioGuitarVolume", _project.Defaults.AudioGuitarVolume.ToString());
                    xml.WriteAttributeString("audioRhythmVolume", _project.Defaults.AudioRhythmVolume.ToString());
                    xml.WriteAttributeString("singer", _project.Defaults.Singer.ToString());
                    xml.WriteAttributeString("singerMode", _project.Defaults.SingerMode.ToString());
                    xml.WriteAttributeString("songVolume", _project.Defaults.SongVolume.ToString());
                    xml.WriteAttributeString("songVolumeMode", _project.Defaults.SongVolumeMode.ToString());
                    xml.WriteAttributeString("year", _project.Defaults.Year.ToString());
                    xml.WriteAttributeString("yearMode", _project.Defaults.YearMode.ToString());
                    xml.WriteAttributeString("gh3SustainClipping", _project.Defaults.Gh3SustainClipping.ToString());
                    xml.WriteAttributeString("forceNoStarPower", _project.Defaults.ForceNoStarPower.ToString());
                    xml.WriteEndElement(); //Defaults

                    xml.WriteStartElement("QbMods");
                    xml.WriteAttributeString("addNonCareerTracksToBonus", _project.GameModsSettings.AddNonCareerTracksToBonus.ToString());
                    xml.WriteAttributeString("completeTier1Song", _project.GameModsSettings.CompleteTier1Song.ToString());
                    xml.WriteAttributeString("defaultBonusSongArt", _project.GameModsSettings.DefaultBonusSongArt.ToString());
                    xml.WriteAttributeString("freeStore", _project.GameModsSettings.FreeStore.ToString());
                    xml.WriteAttributeString("setCheats", _project.GameModsSettings.SetCheats.ToString());
                    xml.WriteAttributeString("unlockSetlists", _project.GameModsSettings.UnlockSetlists.ToString());
                    xml.WriteAttributeString("defaultBonusSongInfo", _project.GameModsSettings.DefaultBonusSongInfo.ToString());
                    xml.WriteAttributeString("defaultBonusSongInfoText", _project.GameModsSettings.DefaultBonusSongInfoText);
                    xml.WriteAttributeString("lastChanged", _project.GameModsSettings.LastChanged.ToString("s"));
                    xml.WriteAttributeString("lastApplied", _project.GameModsSettings.LastApplied.ToString("s"));
                    xml.WriteStartElement("TierNames");
                    foreach (ProjectTier tier in _project.Tiers)
                    {
                        if (tier.Type == TierType.Career)
                            xml.WriteElementString("TierName", tier.Name);
                    }
                    xml.WriteEndElement(); //TierNames
                    xml.WriteEndElement(); //QbMods

                    #region BackgroundMusic
                    xml.WriteStartElement("BackgroundMusic");
                    foreach (ProjectBackgroundAudio ba in _project.BackgroundAudio)
                    {
                        xml.WriteStartElement("Background");
                        if (ba.AudioFiles.Count > 0)
                        {
                            xml.WriteAttributeString("start", ba.PreviewStart.ToString());
                            xml.WriteAttributeString("length", ba.PreviewLength.ToString());
                            xml.WriteAttributeString("fadeLength", ba.PreviewFadeLength.ToString());
                            xml.WriteAttributeString("volume", ba.PreviewVolume.ToString());
                            xml.WriteAttributeString("lastChanged", ba.LastChanged.ToString("s"));
                            xml.WriteAttributeString("lastApplied", ba.LastApplied.ToString("s"));

                            xml.WriteStartElement("AudioFiles");
                            xml.WriteAttributeString("length", ba.AudioLength.ToString());
                            foreach (AudioFile af in ba.AudioFiles)
                            {
                                xml.WriteStartElement("AudioFile");
                                xml.WriteAttributeString("volume", af.Volume.ToString());
                                xml.WriteString(pathAbsToRel(af.Name, pDir, false));
                                xml.WriteEndElement(); //AudioFile
                            }
                            xml.WriteEndElement(); //AudioFiles
                        }
                        xml.WriteEndElement(); //Background
                    }
                    xml.WriteEndElement(); //BackgroundAudio
                    #endregion

                    #region Songs
                    xml.WriteStartElement("Songs");

                    foreach (ProjectTierSong pts in _project.Songs.Values)
                    {
                        if (pts.IsMappingDisabled)
                        {
                            xml.WriteStartElement("Song");
                            //xml.WriteAttributeString("id", pts.SongQb.Id.Text);
                            xml.WriteAttributeString("mappingDisabled", "True");
                            xml.WriteEndElement(); //Song
                        }
                        else if (pts.Song != null) //do we have project settings
                        {
                            saveSongFragment(pDir, xml, pts.Song, currVersion, null);
                            if (_project.StoreProjectFiles)
                                saveSongFragment(pDir, xml, pts.Song, currVersion, string.Format(@"{0}\TheGHOST_Settings.tgs", _project.FileManager.ProjectSongSettingsFolder(pts, pDir).FullName));
                        }
                        else //blank stub
                        {
                            xml.WriteStartElement("Song");
                            xml.WriteEndElement(); //Song
                        }
                    }
                    xml.WriteEndElement(); //Songs
                    #endregion

                    xml.WriteEndElement(); //TheGHOST_ProjectSettings
                    #endregion
                    xml.WriteEndDocument();
                }

                FileHelper.Delete(_projectFile);
                FileHelper.Move(tmp, _projectFile);
            }
            catch (Exception ex)
            {
                try
                {
                    FileHelper.Delete(tmp);
                }
                finally
                {
                }
                throw new ApplicationException(string.Format("Save failed: {0}", ex.Message), ex);
            }

        }

        internal void LoadSettings()
        {
            if (_projectFile.Length == 0 || !File.Exists(_projectFile))
                return;

            if (File.Exists(_projectFile))
            {
                try
                {
                    FileInfo pFile = new FileInfo(this.Filename);
                    DirectoryInfo pDir = new DirectoryInfo(pFile.FullName.Substring(0, pFile.FullName.Length - pFile.Extension.Length));

                    XmlReaderSettings xmlS = new XmlReaderSettings();
                    xmlS.IgnoreComments = true;
                    xmlS.IgnoreProcessingInstructions = true;
                    xmlS.IgnoreWhitespace = true;

                    using (XmlReader xml = XmlReader.Create(_projectFile, xmlS))
                    {

                        while (!xml.EOF && xml.Read())
                        {
                            if (xml.Name == "xml")
                                break;
                        }

                        while (!xml.EOF && xml.Read())
                        {
                            if (xml.Name == "TheGHOST_ProjectSettings")
                            {
                                #region TheGHOST_ProjectSettings
                                if (xml.GetAttribute("version") == "1")
                                {
                                    while (!xml.EOF && xml.Read())
                                    {
                                        switch (xml.Name)
                                        {
                                            case "Plugins":
                                                #region Plugins
                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Plugins")
                                                        break;
                                                }
                                                #endregion
                                                break;

                                            case "Settings":
                                                #region Settings
                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Settings")
                                                        break;
                                                }
                                                #endregion
                                                break;

                                            case "QbMods":
                                                #region QbMods
                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "QbMods")
                                                        break;
                                                }
                                                #endregion
                                                break;

                                            case "BackgroundMusic":
                                                #region BackgroundMusic

                                                int bi = 0;
                                                ProjectBackgroundAudio ba = null;
                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (xml.NodeType == XmlNodeType.Element && xml.Name == "Background")
                                                    {
                                                        DateTime baLastChanged = DateTime.MinValue;
                                                        if (bi < _project.BackgroundAudio.Count)
                                                        {
                                                            ba = _project.BackgroundAudio[bi++];
                                                            if (xml.GetAttribute("start") != null)
                                                                ba.PreviewStart = int.Parse(xml.GetAttribute("start"));
                                                            if (xml.GetAttribute("length") != null)
                                                                ba.PreviewLength = int.Parse(xml.GetAttribute("length"));
                                                            if (xml.GetAttribute("fadeLength") != null)
                                                                ba.PreviewFadeLength = int.Parse(xml.GetAttribute("fadeLength"));
                                                            if (xml.GetAttribute("volume") != null)
                                                                ba.PreviewVolume = int.Parse(xml.GetAttribute("volume"));
                                                            if (xml.GetAttribute("lastChanged") != null)
                                                                baLastChanged = DateTime.Parse(xml.GetAttribute("lastChanged"), null, System.Globalization.DateTimeStyles.AssumeLocal);
                                                            if (xml.GetAttribute("lastApplied") != null)
                                                                ba.LastApplied = DateTime.Parse(xml.GetAttribute("lastApplied"), null, System.Globalization.DateTimeStyles.AssumeLocal);
                                                        }

                                                        while (!xml.IsEmptyElement && !xml.EOF && xml.Read())
                                                        {
                                                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "AudioFiles")
                                                                ba.AudioLength = int.Parse(xml.GetAttribute("length"));
                                                            else if (xml.NodeType == XmlNodeType.Element && xml.Name == "AudioFile")
                                                            {
                                                                int vol = int.Parse(xml.GetAttribute("volume"));
                                                                ba.AudioFiles.Add(ba.CreateAudioFile(pathRelToAbs(xml.ReadString(), pDir), vol));
                                                            }
                                                            else // if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Background")
                                                                break;
                                                        }

                                                        ((ISettingsChange)ba).RecordChange();
                                                        ba.LastChanged = baLastChanged;
                                                    }
                                                    else if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "BackgroundMusic")
                                                        break;
                                                }
                                                #endregion
                                                break;

                                            case "Songs":
                                                #region Songs

                                                //update to load songs in order rather than by ID.
                                                ProjectTierSong[] allSongs;

                                                try
                                                {
                                                    allSongs = new ProjectTierSong[_project.Songs.Count];
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new ApplicationException(string.Format("{0}{1}{1}Check that the correct Game is selected!", ex.Message, Environment.NewLine), ex);
                                                }


                                                _project.Songs.Values.CopyTo(allSongs, 0);
                                                int songIdx = 0;

                                                bool useIds = true; //UPGRADE COMPATABILITY: if IDs exist and the first one exists, then load with IDs (will be saved without)

                                                while (!xml.EOF && xml.Read())
                                                {
                                                    if (songIdx < allSongs.Length && xml.NodeType == XmlNodeType.Element && xml.Name == "Song")
                                                    {
                                                        ProjectSong song = null;
                                                        //get song from position

                                                        if (useIds)
                                                        {
                                                            if (xml.GetAttribute("id") != null) //load by ID
                                                            {
                                                                QbKey songQk = QbKey.Create(xml.GetAttribute("id"));
                                                                if (_project.Songs.ContainsKey(songQk.Crc))
                                                                    song = _project.CreateProjectSong(_project.Songs[songQk.Crc].SongQb, false);
                                                            }
                                                            if (song == null && songIdx == 0)
                                                                useIds = false;
                                                        }

                                                        if (!useIds)
                                                            song = _project.CreateProjectSong(allSongs[songIdx].SongQb, false);

                                                        songIdx++;


                                                        if (song != null)
                                                        {
                                                            if (xml.GetAttribute("lastApplied") != null)
                                                                song.LastApplied = DateTime.Parse(xml.GetAttribute("lastApplied"), null, System.Globalization.DateTimeStyles.AssumeLocal);

                                                            if (xml.GetAttribute("mappingDisabled") != null && bool.Parse(xml.GetAttribute("mappingDisabled")))
                                                                _project.Songs[song.SongQb.Id.Crc].IsMappingDisabled = true;
                                                            else
                                                            {
                                                                if (!xml.IsEmptyElement)
                                                                    loadSongFragment(pDir, xml, song);
                                                                else
                                                                    song = null;

                                                                //add the song settings
                                                                if (song != null)
                                                                    _project.Songs[song.SongQb.Id.Crc].Song = song;
                                                            }
                                                        }

                                                    }
                                                    else if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Songs")
                                                        break;
                                                }
                                                #endregion
                                                break;
                                        }
                                        if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "TheGHOST_ProjectSettings")
                                            break;

                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Exception loading project: {0}", ex.Message));
                }
            }
        }

        internal void LoadSongXmlSettings(string filename, ProjectSong song)
        {
            DirectoryInfo sDir = (new FileInfo(filename)).Directory;

            XmlReaderSettings xmlS = new XmlReaderSettings();
            xmlS.IgnoreComments = true;
            xmlS.IgnoreProcessingInstructions = true;
            xmlS.IgnoreWhitespace = true;

            using (XmlReader xml = XmlReader.Create(filename, xmlS))
            {

                while (!xml.EOF && xml.Read())
                {
                    if (xml.Name == "xml")
                        break;
                }

                while (!xml.EOF && xml.Read())
                {
                    if (xml.Name == "Song")
                        loadSongFragment(sDir, xml, song);
                }
            }
        }

        private void loadSongFragment(DirectoryInfo pDir, XmlReader xml, ProjectSong song)
        {
            DateTime qbLastChanged = DateTime.MinValue;
            while (!xml.EOF && xml.Read())
            {
                switch (xml.Name)
                {
                    case "QbSettings":
                        #region QbSettings
                        song.Artist = xml.GetAttribute("artist");
                        song.Title = xml.GetAttribute("title");
                        song.Year = xml.GetAttribute("year");
                        song.Singer = (Singer)Enum.Parse(typeof(Singer), xml.GetAttribute("singer"));
                        song.GuitarVolume = float.Parse(xml.GetAttribute("guitarVolume"));
                        song.SongVolume = float.Parse(xml.GetAttribute("songVolume"));
                        qbLastChanged = DateTime.Parse(xml.GetAttribute("lastChanged"), null, System.Globalization.DateTimeStyles.AssumeLocal);
                        #endregion
                        break;
                    case "Notes":
                        #region Notes
                        List<MapSetting> genItems = new List<MapSetting>();

                        song.MinMsBeforeNotesStart = int.Parse(xml.GetAttribute("minMsBeforeStart"));
                        song.Notes.HoPoMeasure = float.Parse(xml.GetAttribute("hoPoMeasure"));
                        song.Notes.Gh3SustainClipping = bool.Parse(xml.GetAttribute("gh3SustainClipping"));
                        song.Notes.ForceNoStarPower = xml.GetAttribute("forceNoStarPower") == null ? false : bool.Parse(xml.GetAttribute("forceNoStarPower"));
                        DateTime notesLastChanged = DateTime.Parse(xml.GetAttribute("lastChanged"), null, System.Globalization.DateTimeStyles.AssumeLocal);

                        while (!xml.EOF && xml.Read())
                        {
                            //skip NotesFiles and MappedItems elements and process the children
                            switch (xml.Name)
                            {
                                case "NotesFile":
                                    #region NotesFile
                                    bool baseFile = xml.GetAttribute("baseFile") == null ? false : bool.Parse(xml.GetAttribute("baseFile"));
                                    int nonNoteSyncOffset = int.Parse(xml.GetAttribute("nonNoteSyncOffset"));
                                    NotesFile nf = null;
                                    while (!xml.EOF && xml.Read())
                                    {
                                        switch (xml.Name)
                                        {
                                            case "Filename":
                                                nf = song.Notes.ParseFile(pathRelToAbs(xml.ReadString(), pDir));
                                                if (baseFile)
                                                    song.Notes.BaseFile = nf;
                                                nf.NonNoteSyncOffset = nonNoteSyncOffset;
                                                break;
                                            case "Item": //ignore the Items tag

                                                string sourceName = xml.GetAttribute("name");
                                                foreach (NotesFileItem nfi in nf.Items)
                                                {
                                                    if (nfi.SourceName == sourceName)
                                                    {
                                                        nfi.UniqueId = uint.Parse(xml.GetAttribute("id"));
                                                        nfi.SyncOffset = int.Parse(xml.GetAttribute("syncOffset"));
                                                        break;
                                                    }
                                                }
                                                break;
                                        }
                                        if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "NotesFile")
                                            break;

                                    }
                                    #endregion
                                    break;
                                case "MappedItem": //ignore mapped items
                                    #region MappedItem
                                    string mapName = xml.GetAttribute("name");

                                    foreach (GhNotesItem ghi in song.Notes.GhItems)
                                    {
                                        if (ghi.Name == mapName)
                                        {
                                            uint mapId;
                                            //item is mapped
                                            if (xml.GetAttribute("mapToItemId") != null)
                                            {
                                                mapId = uint.Parse(xml.GetAttribute("mapToItemId"));

                                                int syncOffset = xml.GetAttribute("syncOffset") == null ? 0 : int.Parse(xml.GetAttribute("syncOffset"));
                                                bool genNotes = bool.Parse(xml.GetAttribute("hasGeneratedNotes"));
                                                bool genSP = bool.Parse(xml.GetAttribute("hasGeneratedStarPower"));
                                                bool genBP = bool.Parse(xml.GetAttribute("hasGeneratedBattlePower"));
                                                bool genFO = bool.Parse(xml.GetAttribute("hasGeneratedFaceOff"));
                                                if (!genNotes)
                                                {
                                                    foreach (NotesFile nfl in song.Notes.Files)
                                                    {
                                                        if (nfl.FindItem(mapId) != null)
                                                        {
                                                            song.Notes.MapToGhItem(nfl, mapId, ghi, false);

                                                            if (genSP || genBP || genFO)
                                                                song.Notes.GenerateNotes(ghi, false, genSP, genBP, genFO, true);
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    genItems.Add(new MapSetting(syncOffset, genSP, genFO, genBP, genNotes, mapId, ghi));
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    #endregion
                                    break;
                            }
                            if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Notes")
                                break;
                        }
                        //must map the generated notes after all the direct mapping have been made
                        foreach (MapSetting mi in genItems)
                        {
                            foreach (GhNotesItem ghi in song.Notes.GhItems)
                            {
                                if (ghi.IsMapped && !ghi.MappedFileItem.HasGeneratedNotes && ghi.MappedFileItem.UniqueId == mi.UniqueId)
                                {
                                    mi.GhItem.GenerateNotes(song.Notes.BaseFile, ghi);
                                    mi.GhItem.MappedFileItem.SyncOffset = mi.SyncOffset;
                                    if (mi.GenSP || mi.GenBP || mi.GenFO)
                                        song.Notes.GenerateNotes(mi.GhItem, false, mi.GenSP, mi.GenBP, mi.GenFO, true);
                                    break;
                                }
                            }
                        }
                        ((ISettingsChange)song.Notes).RecordChange();
                        song.Notes.LastChanged = notesLastChanged;
                        #endregion
                        break;
                    case "Audio":
                        #region Audio
                        DateTime audioLastChanged = DateTime.Parse(xml.GetAttribute("lastChanged"), null, System.Globalization.DateTimeStyles.AssumeLocal);

                        while (!xml.EOF && xml.Read())
                        {
                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Preview")
                            {
                                song.Audio.PreviewStart = int.Parse(xml.GetAttribute("start"));
                                song.Audio.PreviewLength = int.Parse(xml.GetAttribute("length"));
                                song.Audio.PreviewFadeLength = int.Parse(xml.GetAttribute("fadeLength"));
                                song.Audio.PreviewVolume = int.Parse(xml.GetAttribute("volume"));
                                song.Audio.PreviewIncludeGuitar = bool.Parse(xml.GetAttribute("includeGuitar"));
                                song.Audio.PreviewIncludeRhythm = bool.Parse(xml.GetAttribute("includeRhythm"));
                            }
                            else if (xml.NodeType == XmlNodeType.Element && xml.Name == "AudioFiles")
                            {
                                if (xml.GetAttribute("length") != null)
                                    song.Audio.AudioLength = int.Parse(xml.GetAttribute("length"));
                            }
                            else if (xml.NodeType == XmlNodeType.Element && xml.Name == "AudioFile")
                            {
                                string t = xml.GetAttribute("type");
                                int vol = 100;
                                if (xml.GetAttribute("volume") != null)
                                    vol = int.Parse(xml.GetAttribute("volume"));
                                string fn = pathRelToAbs(xml.ReadString(), pDir);
                                switch (t)
                                {
                                    case "song":
                                        if (fn.Trim().Length != 0)
                                            song.Audio.SongFiles.Add(song.Audio.CreateAudioFile(fn, vol));
                                        break;
                                    case "guitar":
                                        song.Audio.GuitarFile = fn == null || fn.Trim().Length == 0 ? null : song.Audio.CreateAudioFile(fn, vol);
                                        break;
                                    case "rhythm":
                                        song.Audio.RhythmFile = fn == null || fn.Trim().Length == 0 ? null : song.Audio.CreateAudioFile(fn, vol);
                                        break;
                                }
                            }
                            else if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Audio")
                                break;
                        }

                        ((ISettingsChange)song.Audio).RecordChange();
                        song.Audio.LastChanged = audioLastChanged;
                        #endregion
                        break;
                }
                if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "Song")
                {
                    ((ISettingsChange)song).RecordChange();
                    song.QbLastChanged = qbLastChanged;
                    break;
                }
            }
        }

        private void saveSongFragment(DirectoryInfo pDir, XmlWriter xml, ProjectSong ps, string version, string filename)
        {
            bool b = (filename != null && filename.Length != 0);
            if (b)
            {
                try
                {
                    FileHelper.Delete(filename);
                    xml = XmlWriter.Create(filename, xml.Settings);
                    xml.WriteComment("This is a copy of the settings from the project file.  This file can be imported with TheGHOST using Smart Mode");
                }
                catch
                {
                    return; //skip this
                }
            }

            xml.WriteStartElement("Song");
            if (!b)
            {
                //xml.WriteAttributeString("id", ps.SongQb.Id.Text);
                xml.WriteAttributeString("lastApplied", ps.LastApplied.ToString("s"));
            }
            else
                xml.WriteAttributeString("version", version);

            #region QbSettings
            xml.WriteStartElement("QbSettings");
            xml.WriteAttributeString("artist", ps.Artist);
            xml.WriteAttributeString("title", ps.Title);
            xml.WriteAttributeString("year", ps.Year);
            xml.WriteAttributeString("singer", ps.Singer.ToString());
            xml.WriteAttributeString("guitarVolume", ps.GuitarVolume.ToString());
            xml.WriteAttributeString("songVolume", ps.SongVolume.ToString());
            xml.WriteAttributeString("lastChanged", ps.QbLastChanged.ToString("s"));
            xml.WriteEndElement(); //QbSettings
            #endregion

            #region Notes
            xml.WriteStartElement("Notes");
            xml.WriteAttributeString("minMsBeforeStart", ps.MinMsBeforeNotesStart.ToString());
            xml.WriteAttributeString("hoPoMeasure", ps.Notes.HoPoMeasure.ToString());
            xml.WriteAttributeString("gh3SustainClipping", ps.Notes.Gh3SustainClipping.ToString());
            xml.WriteAttributeString("forceNoStarPower", ps.Notes.ForceNoStarPower.ToString());
            xml.WriteAttributeString("lastChanged", ps.Notes.LastChanged.ToString("s"));

            xml.WriteStartElement("NotesFiles");
            foreach (NotesFile nf in ps.Notes.Files)
            {
                xml.WriteStartElement("NotesFile");
                if (ps.Notes.BaseFile == nf)
                    xml.WriteAttributeString("baseFile", true.ToString());
                xml.WriteAttributeString("nonNoteSyncOffset", nf.NonNoteSyncOffset.ToString());
                xml.WriteStartElement("Filename");
                xml.WriteString(pathAbsToRel(nf.Filename, pDir, b));
                xml.WriteEndElement(); //Filename
                xml.WriteStartElement("Items");
                foreach (NotesFileItem nfi in nf.Items)
                {
                    xml.WriteStartElement("Item");
                    xml.WriteAttributeString("id", nfi.UniqueId.ToString());
                    xml.WriteAttributeString("name", nfi.SourceName);
                    xml.WriteAttributeString("syncOffset", nfi.SyncOffset.ToString());
                    xml.WriteEndElement(); //Item
                }
                xml.WriteEndElement(); //Items
                xml.WriteEndElement(); //NotesFile
            }
            xml.WriteEndElement(); //NotesFiles

            xml.WriteStartElement("MappedItems");
            foreach (GhNotesItem ghi in ps.Notes.GhItems)
            {
                xml.WriteStartElement("MappedItem");
                xml.WriteAttributeString("name", ghi.Name);
                if (ghi.IsMapped)
                {
                    if (ghi.MappedFileItem != null)
                        xml.WriteAttributeString("mapToItemId", ghi.MappedFileItem.UniqueId.ToString());
                    xml.WriteAttributeString("syncOffset", ghi.MappedFileItem.SyncOffset.ToString());
                    xml.WriteAttributeString("hasGeneratedBattlePower", ghi.MappedFileItem.HasGeneratedBattlePower.ToString());
                    xml.WriteAttributeString("hasGeneratedFaceOff", ghi.MappedFileItem.HasGeneratedFaceOff.ToString());
                    xml.WriteAttributeString("hasGeneratedStarPower", ghi.MappedFileItem.HasGeneratedStarPower.ToString());
                    xml.WriteAttributeString("hasGeneratedNotes", ghi.MappedFileItem.HasGeneratedNotes.ToString());
                }
                xml.WriteEndElement(); //MappedItem
            }
            xml.WriteEndElement(); //MappedItems

            xml.WriteEndElement(); //Notes
            #endregion

            #region Audio
            xml.WriteStartElement("Audio");
            xml.WriteAttributeString("lastChanged", ps.Audio.LastChanged.ToString("s"));

            xml.WriteStartElement("Preview");
            xml.WriteAttributeString("start", ps.Audio.PreviewStart.ToString());
            xml.WriteAttributeString("length", ps.Audio.PreviewLength.ToString());
            xml.WriteAttributeString("fadeLength", ps.Audio.PreviewFadeLength.ToString());
            xml.WriteAttributeString("volume", ps.Audio.PreviewVolume.ToString());
            xml.WriteAttributeString("includeGuitar", ps.Audio.PreviewIncludeGuitar.ToString());
            xml.WriteAttributeString("includeRhythm", ps.Audio.PreviewIncludeRhythm.ToString());
            xml.WriteEndElement(); //Preview

            xml.WriteStartElement("AudioFiles");
            xml.WriteAttributeString("length", ps.Audio.AudioLength.ToString());

            if (ps.Audio.SongFiles.Count != 0)
            {
                foreach (AudioFile af in ps.Audio.SongFiles)
                {
                    xml.WriteStartElement("AudioFile");
                    xml.WriteAttributeString("type", "song");
                    xml.WriteAttributeString("volume", af.Volume.ToString());
                    xml.WriteString(pathAbsToRel(af.Name, pDir, b));
                    xml.WriteEndElement(); //AudioFile
                }
            }
            else
            {
                xml.WriteStartElement("AudioFile");
                xml.WriteAttributeString("type", "song");
                xml.WriteAttributeString("volume", "100");
                xml.WriteString(string.Empty);
                xml.WriteEndElement(); //AudioFile
            }

            xml.WriteStartElement("AudioFile");
            xml.WriteAttributeString("type", "guitar");
            xml.WriteAttributeString("volume", (ps.Audio.GuitarFile == null || ps.Audio.GuitarFile.Name.Length == 0) ? "100" : ps.Audio.GuitarFile.Volume.ToString());
            xml.WriteString(ps.Audio.GuitarFile == null ? string.Empty : pathAbsToRel(ps.Audio.GuitarFile.Name, pDir, b));
            xml.WriteEndElement(); //AudioFile

            xml.WriteStartElement("AudioFile");
            xml.WriteAttributeString("type", "rhythm");
            xml.WriteAttributeString("volume", (ps.Audio.RhythmFile == null || ps.Audio.RhythmFile.Name.Length == 0) ? "100" : ps.Audio.RhythmFile.Volume.ToString());
            xml.WriteString(ps.Audio.RhythmFile == null ? string.Empty : pathAbsToRel(ps.Audio.RhythmFile.Name, pDir, b));
            xml.WriteEndElement(); //AudioFile
            xml.WriteEndElement(); //AudioFiles

            xml.WriteEndElement(); //Audio
            #endregion

            xml.WriteEndElement(); //Song

            if (b)
                xml.Close();

        }

        internal void SaveAs(string filename)
        {
            _projectFile = filename;
            this.Save();
        }


        private string loadSetting(AppSettingsSection app, string item)
        {
            return loadSetting(app, item, string.Empty);
        }

        private string loadSetting(AppSettingsSection app, string item, string defaultItem)
        {
            KeyValueConfigurationElement kvce;
            kvce = app.Settings[item];
            if (kvce != null)
                return kvce.Value;
            else
                return defaultItem;

        }

        private void saveSetting(AppSettingsSection app, string name, string value)
        {
            if (app.Settings[name] == null)
                app.Settings.Add(name, value);
            else
                app.Settings[name].Value = value;
        }

        private void saveXmlSetting(AppSettingsSection app, string name, string value)
        {
            if (app.Settings[name] == null)
                app.Settings.Add(name, value);
            else
                app.Settings[name].Value = value;
        }

        public string Filename
        {
            get { return _projectFile; }
            set { _projectFile = value; }
        }

        public string SourcePath { get; set; }
        public string GameId { get; set; }
        public string LanguageId { get; set; }

        public string AudioExportPluginName { get; set; }
        public string AudioImportPluginName { get; set; }
        public string FileCopyPluginName { get; set; }
        public string[] EditorPluginNames { get; set; }

        private Project _project;

        private string _projectFile;

        private List<string> _tierNames;
      
        private class MapSetting
        {
            public MapSetting(int syncOffset, bool genSP, bool genFO, bool genBP, bool genNotes, uint uniqueId, GhNotesItem ghItem)
            {
                this.SyncOffset = syncOffset;
                this.GenSP = genSP;
                this.GenFO = genFO;
                this.GenBP = genBP;
                this.GenNotes = genNotes;
                this.UniqueId = uniqueId;
                this.GhItem = ghItem;
            }

            public int SyncOffset { get; set; }
            public bool GenSP { get; set; }
            public bool GenFO { get; set; }
            public bool GenBP { get; set; }
            public bool GenNotes { get; set; }
            public uint UniqueId { get; set; }
            public GhNotesItem GhItem { get; set; }
        }
    }


}
