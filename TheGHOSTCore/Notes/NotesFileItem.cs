using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class NotesFileItem : ISettingsChange

    {
        internal NotesFileItem(string sourceName, NotesFile baseFile, NotesFileItem sourceItem, NotesDifficulty sourceDifficulty, NotesDifficulty difficulty)
        {
            this.construct(sourceName, new int[0], new int[0], new int[0], new int[0], new int[0], sourceItem.SustainTrigger);
            this.GenerateNotes(baseFile.Frets, difficulty, sourceItem.Notes, sourceItem.StarPower, sourceItem.BattlePower, sourceDifficulty);
        }

        internal NotesFileItem(string sourceName, int[] notes, int[] starPower, int[] starBattleMode, int[] faceOffP1, int[] faceOffP2, int sustainTrigger)
        {
            construct(sourceName, notes, starPower, starBattleMode, faceOffP1, faceOffP2, sustainTrigger);
        }

        private void construct(string sourceName, int[] notes, int[] starPower, int[] starBattleMode, int[] faceOffP1, int[] faceOffP2, int sustainTrigger)
        {
            _lastChanged = DateTime.MinValue;
            _recordChange = false;

            this.Notes = notes;
            this.StarPower = starPower;
            this.BattlePower = starBattleMode;
            this.FaceOffP1 = faceOffP1;
            this.FaceOffP2 = faceOffP2;

            this.HasGeneratedNotes = false;
            this.HasGeneratedBattlePower = false;
            this.HasGeneratedStarPower = false;
            this.HasGeneratedBattlePower = false;
            this.HasGeneratedFaceOff = false;

            this.UniqueId = (uint)Guid.NewGuid().GetHashCode();

            int b = 0;
            this.ButtonsUsed = 0;

            for (int i = 0; i < this.Notes.Length; i += 3)
            {
                if ((int)(this.Notes[i + 2] & 0x1F) > b)
                    b = this.Notes[i + 2] & 0x1F;
            }

            for (int i = 4; i >= 0; i--)
            {
                if ((1 << i & b) != 0)
                {
                    this.ButtonsUsed = i + 1;
                    break;
                }
            }

            this.SourceName = sourceName;
            this.SyncOffset = 0;
            this.SustainTrigger = sustainTrigger;

        }

        internal void GenerateStarPower(int[] frets, bool force, int[] copyStarPower)
        {
            this.StarPower = NotesGenerator.GenerateStarPower(frets, this.Notes, force ? new int[0] : copyStarPower);
            this.HasGeneratedStarPower = true;
            this.LastChanged = DateTime.Now;
        }

        internal void GenerateNotes(int[] frets, NotesDifficulty difficulty, int[] sourceNotes, int[] sourceSp, int[] sourceBattle, NotesDifficulty sourceDifficulty)
        {
            List<int> sp = new List<int>(sourceSp);
            int[] n = NotesGenerator.CreateDifficulty(difficulty, sourceNotes, NotesDifficulty.Expert);

            construct(this.SourceName, n,
                NotesGenerator.GenerateStarPower(frets, n, sourceSp),
                this.BattlePower, this.FaceOffP1, this.FaceOffP2, this.SustainTrigger);

            this.HasGeneratedStarPower = true;
            this.HasGeneratedNotes = true;
            this.LastChanged = DateTime.Now;
        }

        internal void GenerateBattlePower(int[] frets, int[] copyStarPower)
        {
            this.BattlePower = NotesGenerator.GenerateBattlePower(frets, this.Notes, copyStarPower);
            this.HasGeneratedBattlePower = true;
            this.LastChanged = DateTime.Now;
        }

        internal void GenerateFaceOff(int[] frets, int[] copyFaceOffP1, int[] copyFaceOffP2, int[] copyBattlePower)
        {
            List<int> p1 = new List<int>(copyFaceOffP1 == null ? new int[0] : copyFaceOffP1);
            List<int> p2 = new List<int>(copyFaceOffP2 == null ? new int[0] : copyFaceOffP2);

            NotesGenerator.GenerateFaceOff(frets, p1, p2, copyBattlePower);

            this.FaceOffP1 = p1.ToArray();
            this.FaceOffP2 = p2.ToArray();

            this.HasGeneratedFaceOff = true;
            this.LastChanged = DateTime.Now;
        }

        public int[] Notes { get; set; }
        public int[] StarPower { get; set; }
        public int[] BattlePower { get; set; }
        public int[] FaceOffP1 { get; set; }
        public int[] FaceOffP2 { get; set; }

        public bool HasGeneratedNotes { get; set; }
        public string NotesGeneratedFrom { get; set; }
        public bool HasGeneratedStarPower { get; set; }
        public bool HasGeneratedBattlePower { get; set; }
        public bool HasGeneratedFaceOff { get; set; }

        public string SourceName { get; set; }
        public int StarPowerCount
        {
            get { return this.StarPower == null ? 0 : this.StarPower.Length / 3; }
        }
        public int NotesCount
        {
            get { return this.Notes == null ? 0 : this.Notes.Length / 3; }
        }
        public int BattlePowerCount
        {
            get { return this.BattlePower == null ? 0 : this.BattlePower.Length / 3; }
        }
        public int FaceOffP1Count
        {
            get { return this.FaceOffP1 == null ? 0 : this.FaceOffP1.Length / 2; }
        }
        public int FaceOffP2Count
        {
            get { return this.FaceOffP2 == null ? 0 : this.FaceOffP2.Length / 2; }
        }

        public uint UniqueId { get; set; }

        public int ButtonsUsed
        {
            get { return _buttonsUsed;}
            set { _buttonsUsed = value; }
        }

        public int SyncOffset
        {
            get { return _syncOffset; }
            set
            {
                if (_syncOffset == value)
                    return;
                _syncOffset = value;
                this.LastChanged = DateTime.Now;
            }
        }

        public int SustainTrigger
        {
            get { return _sustainTrigger; }
            set
            {
                if (_sustainTrigger == value)
                    return;
                _sustainTrigger = value;
                //this.LastChanged = DateTime.Now; //this item isn't saved
            }
        }

        #region ISettingsChange Members

        public DateTime LastChanged
        {
            get
            {
                return _lastChanged;
            }
            set
            {
                if (_recordChange)
                    _lastChanged = value;
            }
        }

        void ISettingsChange.RecordChange()
        {
            _recordChange = true;
        }

        #endregion

        private int _buttonsUsed;
        private int _syncOffset;
        private int _sustainTrigger;

        private bool _recordChange;
        private DateTime _lastChanged;
    }
}
