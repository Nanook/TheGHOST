using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    internal enum NoteType
    {
        NoteOn,
        NoteOff,
        NoteAftertouch,
        Parameter,
        Program,
        ChannelAftertouch,
        Pitch,
        Repeat //??
    }

    internal class MidNote
    {
        internal MidNote(NoteType type, string section, int offset, int note)
        {
            Type = type;
            Section = section;
            Offset = offset;
            Note = note;
        }

        public NotesDifficulty Difficulty
        {
            get
            {
                if (Note >= 96 && Note <= 96 + 10)
                    return NotesDifficulty.Expert;
                else if (Note >= 84 && Note <= 84 + 10)
                    return NotesDifficulty.Hard;
                else if (Note >= 72 && Note <= 72 + 10)
                    return NotesDifficulty.Medium;
                else if (Note >= 60 && Note <= 60 + 10)
                    return NotesDifficulty.Easy;

                return (NotesDifficulty)(-1);
            }
        }

        public int GhNote
        {
            get
            {
                if (Note >= 96 && Note < 96 + 5)
                    return 1 << (Note - 96);
                else if (Note >= 84 && Note < 84 + 5)
                    return 1 << (Note - 84);
                else if (Note >= 72 && Note < 72 + 5)
                    return 1 << (Note - 72);
                else if (Note >= 60 && Note < 60 + 5)
                    return 1 << (Note - 60);

                return 0;  //no note
            }
        }

        public bool IsStarPower
        {
            get
            {
                switch (Difficulty)
                {
                    case NotesDifficulty.Easy:
                        return (Note == 60 + 7);
                    case NotesDifficulty.Medium:
                        return (Note == 72 + 7);
                    case NotesDifficulty.Hard:
                        return (Note == 84 + 7);
                    case NotesDifficulty.Expert:
                        return (Note == 96 + 7);
                    default:
                        return false;
                }
            }
        }

        public bool IsFaceOffP1
        {
            get
            {
                switch (Difficulty)
                {
                    case NotesDifficulty.Easy:
                        return (Note == 60 + 9);
                    case NotesDifficulty.Medium:
                        return (Note == 72 + 9);
                    case NotesDifficulty.Hard:
                        return (Note == 84 + 9);
                    case NotesDifficulty.Expert:
                        return (Note == 96 + 9);
                    default:
                        return false;
                }
            }
        }

        public bool IsFaceOffP2
        {
            get
            {
                switch (Difficulty)
                {
                    case NotesDifficulty.Easy:
                        return (Note == 60 + 10);
                    case NotesDifficulty.Medium:
                        return (Note == 72 + 10);
                    case NotesDifficulty.Hard:
                        return (Note == 84 + 10);
                    case NotesDifficulty.Expert:
                        return (Note == 96 + 10);
                    default:
                        return false;
                }
            }
        }

        public NoteType Type;
        public readonly string Section;
        public readonly int Offset;
        public readonly int Note;
    }
}
