using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public interface INotesParser
    {
        void Parse(string filename);
        string[] HeaderNames();
        int[] GetNotes(string headerName);
        int[] GetStarPower(string headerName);
        int[] GetFaceOffP1(string headerName);
        int[] GetFaceOffP2(string headerName);
        int[] GetFretBars(int msSongLength);
        int[] GetTimeSig();
        int GetSustainTrigger();
        NotesMarker[] GetNotesMarkers();
        string MatchType(NotesType type, NotesDifficulty difficulty);
    }
}
