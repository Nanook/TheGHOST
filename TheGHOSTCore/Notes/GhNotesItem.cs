using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class GhNotesItem
    {
        internal GhNotesItem(string name, NotesType type, NotesDifficulty difficulty, string songName)
        {
            this.Name = name;
            this.Type = type;
            this.Difficulty = difficulty;
            this.IsMapped = false;
            this.MappedFile = null;
            this.MappedFileItem = null;

            switch (type)
            {
                case NotesType.Guitar:
                    this.SongNotesQbKey = QbKey.Create(string.Format("{0}_Song_{1}", songName, difficulty.ToString()));
                    this.SongStarPowerQbKey = QbKey.Create(string.Format("{0}_{1}_Star", songName, difficulty.ToString()));
                    this.SongStarPowerBattleQbKey = QbKey.Create(string.Format("{0}_{1}_StarBattleMode", songName, difficulty.ToString()));
                    break;
                case NotesType.Rhythm:
                    this.SongNotesQbKey = QbKey.Create(string.Format("{0}_Song_Rhythm_{1}", songName, difficulty.ToString()));
                    this.SongStarPowerQbKey = QbKey.Create(string.Format("{0}_Rhythm_{1}_Star", songName, difficulty.ToString()));
                    this.SongStarPowerBattleQbKey = QbKey.Create(string.Format("{0}_Rhythm_{1}_StarBattleMode", songName, difficulty.ToString()));
                    break;
                case NotesType.GuitarCoop:
                    this.SongNotesQbKey = QbKey.Create(string.Format("{0}_Song_GuitarCoop_{1}", songName, difficulty.ToString()));
                    this.SongStarPowerQbKey = QbKey.Create(string.Format("{0}_GuitarCoop_{1}_Star", songName, difficulty.ToString()));
                    this.SongStarPowerBattleQbKey = QbKey.Create(string.Format("{0}_GuitarCoop_{1}_StarBattleMode", songName, difficulty.ToString()));
                    break;
                case NotesType.RhythmCoop:
                    this.SongNotesQbKey = QbKey.Create(string.Format("{0}_Song_RhythmCoop_{1}", songName, difficulty.ToString()));
                    this.SongStarPowerQbKey = QbKey.Create(string.Format("{0}_RhythmCoop_{1}_Star", songName, difficulty.ToString()));
                    this.SongStarPowerBattleQbKey = QbKey.Create(string.Format("{0}_RhythmCoop_{1}_StarBattleMode", songName, difficulty.ToString()));
                    break;
            }

        }

        public void GenerateNotes(NotesFile baseFile, GhNotesItem sourceItem)
        {
            this.IsMapped = true;
            this.MappedFile = sourceItem.MappedFile;
            this.MappedFileItem = new NotesFileItem(sourceItem.MappedFileItem.SourceName, baseFile, sourceItem.MappedFileItem, sourceItem.Difficulty, this.Difficulty);
            this.MappedFileItem.NotesGeneratedFrom = string.Format("{0} : {1}", sourceItem.Type.ToString(), sourceItem.Difficulty.ToString());
            this.MappedFileItem.UniqueId = sourceItem.MappedFileItem.UniqueId;
            this.MappedFileItem.SyncOffset = sourceItem.MappedFileItem.SyncOffset;
            this.MappedFileItem.LastChanged = DateTime.Now;

        }

        public string Name { get; set; }

        public QbKey SongNotesQbKey { get; set; }
        public QbKey SongStarPowerQbKey { get; set; }
        public QbKey SongStarPowerBattleQbKey { get; set; }


        public NotesType Type { get; set; }
        public NotesDifficulty Difficulty { get; set; }

        public bool IsMapped { get; internal set; }
        public NotesFile MappedFile { get; internal set; }
        public NotesFileItem MappedFileItem { get; internal set; }

    }
}
