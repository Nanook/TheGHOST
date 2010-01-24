using System;
using System.Collections.Generic;
using System.Text;
using Nanook.TheGhost;

/* TODO:
 * 
 * General:
 * Solos - detect notes not used at all and spread solo over all available
 * If notes are the same repeated, use the first one
 * 
 * Hard:
 * Don't allow 3 notes in row if is in run with 2s
 * 
 * Medium:
 * 
 * 
 * Easy:
 * 
 * */

namespace Nanook.TheGhost
{
    public static class NotesGenerator
    {

        static NotesGenerator()
        {
        }


        /// <summary>
        /// If starPower contains items the offets and lengths will be applied to the notes (so each difficulty has the same timings
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="starPower"></param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static int[] GenerateStarPower(int[] frets, int[] notes, int[] starPower)
        {
            if (frets.Length < 20)
                return new int[0];

            int sections = frets[frets.Length - 20] / 30000; //section every 40 seconds
            

            Random gen = new Random(Guid.NewGuid().GetHashCode());
            List<int> sp = new List<int>(starPower == null ? new int[0] : starPower);
            
            int sectionLen;

            if (sections == 0)
                return starPower;

            if (sp.Count != 0)
            {
                for (int i = 0; i < sp.Count; i += 3)
                    sp[i + 2] = countSectionNotes(notes, sp[i], sp[i + 1]);
                return sp.ToArray();
            }

            //has each section got at least 20 notes in it?
            if (notes.Length / sections < 20)
                sections = notes.Length / 20; //if not then split sections in to 20 note chunks

            if (sections < 2) //if we have 2 or less sections then quit
                return starPower;

            sectionLen = (notes.Length / 3) / sections;  //each note is 3 ints (offset, length, buttons)

            for (int s = 0; s < sections; s++)
            {
                int minIdx = Math.Min(
                        ((s * sectionLen) + (sectionLen / 2) + gen.Next(-10, +5)),  //start of sp : target the middle of each section (random -10 + 5 notes)
                        Math.Max(sectionLen - 30, s * sectionLen)
                    ); //try to target the middle of the section
                int maxIdx = gen.Next(minIdx, Math.Min(gen.Next(minIdx + 5, minIdx + 30), ((s + 1) * sectionLen) - 1));  //the last note in this section

                if (minIdx > maxIdx)
                    maxIdx = minIdx; //1 note sp

                if (minIdx <= 0) //(s * sectionLen))
                    continue; //this can happen if there are too many section and gen.Next(-10, +5) returns a minus 

                int spLen = (notes[maxIdx * 3] + notes[(maxIdx * 3) + 1]) - notes[minIdx * 3]; //last note offset - first note offset + last note length

                sp.Add(notes[maxIdx * 3]); //add offset
                sp.Add(spLen); //add length
                sp.Add(maxIdx - minIdx); //add note count
            }

            return starPower = sp.ToArray();

        }

        private static int countSectionNotes(int[] notes, int offset, int length)
        {
            int count = 0;
            for (int i = 0; i < notes.Length; i += 3)
            {
                if (notes[i] > offset + length)
                    break;
                else if (notes[i] > offset)
                    count++;
            }

            return count;
        }

        private static int calcLengthFromSectionNotes(int[] notes, int offset, int count)
        {
            int c = 0;
            for (int i = 0; i < notes.Length; i += 3)
            {
                if (notes[i] > offset)
                    c++;
                if (c == count)
                    return (notes[i] + notes[i + 1]) - offset; 
            }

            return (notes[notes.Length - 3] + notes[notes.Length - 2]) - offset;
        }

        public static int[] GenerateBattlePower(int[] frets, int[] notes, int[] starPower)
        {
            List<int> arr;

            if (starPower != null && starPower.Length != 0)
            {
                arr = new List<int>(starPower);
                for (int i = 0; i < arr.Count; i += 3)
                {
                    if (arr[i + 2] >= 6) //half notes if >= 6
                    {
                        arr[i + 2] = (int)Math.Round(arr[i + 2] / 2F, 0);
                        arr[i + 1] = calcLengthFromSectionNotes(notes, arr[i], arr[i + 2]);
                    }
                }
                return arr.ToArray();
            }

            arr = new List<int>();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            int p = 0;
            int l = 0;


            while (p < frets.Length - 20) //leave at least x frets before end of song
            {
                p += rnd.Next(15, 40); //leave between x and y frets between battle power
                l = rnd.Next(2, 5); //x to y frets of notes

                if (p + l < frets.Length - 20)
                {
                    arr.Add(frets[p]);
                    //create length, with random overlap
                    arr.Add((frets[p + l] - frets[p]) - 1); //-1 so it's 1ms before the next fret
                    arr.Add(countSectionNotes(notes, arr[arr.Count - 2], arr[arr.Count - 1])); //note count
                }
                p += l;
            }

            //some of these items will never be seen due to the face off sections...
            return arr.ToArray();
        }


        public static void GenerateFaceOff(int[] frets, List<int> p1, List<int> p2, int[] starPower)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            int p = 0;
            int l = 0;
            bool t = true;
            List<int> arr;

            if (p1.Count != 0 && p2.Count != 0)
                return;

            p1.Clear();
            p2.Clear();

            while (p < frets.Length - 1)
            {
                l = rnd.Next(15, 30);
                if (t)
                    arr = p1;
                else
                    arr = p2;
                t = !t;

                arr.Add(frets[p]);
                //create length, with random overlap
                arr.Add((frets[Math.Min(p + l + rnd.Next(5, 10), frets.Length - 1)] - frets[p]) - 1); //random overlap 5 to 10 frets

                p += l;
            }

            insertStarPowerIntoFaceOff(p1, starPower, frets[frets.Length - 1]);
            insertStarPowerIntoFaceOff(p2, starPower, frets[frets.Length - 1]);

        }

        private static void insertStarPowerIntoFaceOff(List<int> faceOff, int[] starPower, int maxLen)
        {
            int lastJ = 0;
            //add the star power sections to the face off sections
            for (int i = 0; i < starPower.Length; i += 3)
            {

                for (int j = lastJ; j < faceOff.Count; j += 2)
                {
                    lastJ = j;
                    //does star power start in this section AND star power goes beyond this section
                    if (faceOff[j] <= starPower[i] && faceOff[j] + faceOff[j + 1] > starPower[i] &&
                        starPower[i] + starPower[i + 1] > faceOff[j] + faceOff[j + 1])
                    {
                        faceOff[j + 1] = (starPower[i] + starPower[i + 1]) - faceOff[j]; //extend this section
                        break;
                    }

                    //does star power start and end in this section
                    else if (faceOff[j] <= starPower[i] && faceOff[j] + faceOff[j + 1] > starPower[i] + starPower[i + 1])
                    {
                        break; //do nothing
                    }


                    //star power starts and ends before this section
                    else if (faceOff[j] > starPower[i] && faceOff[j] > starPower[i] + starPower[i + 1])
                    {
                        faceOff.Insert(j, starPower[i + 1]); //insert length
                        faceOff.Insert(j, starPower[i]);  //insert offset (before the above length)
                        break; //insert a new section
                    }

                    //are we after the star power started  AND star power stops in this face off section
                    else if (faceOff[j] > starPower[i] &&
                        starPower[i] + starPower[i + 1] > faceOff[j])
                    {
                        faceOff[j + 1] += faceOff[j] - starPower[i]; //add the difference
                        faceOff[j] = starPower[i]; //start of the star power
                        break;
                    }
                }
            }

            //extend the last section to the end of the song
            if (faceOff.Count > 2)
               faceOff[faceOff.Count - 1] = maxLen - faceOff[faceOff.Count - 2];

            int c = 0;
            while (c < faceOff.Count)
            {
                //if this section overlaps with the last section then combine them
                if (c >= 2 && faceOff[c - 2] + faceOff[(c - 2) + 1] >= faceOff[c])
                {
                    faceOff[c - 1] = (faceOff[c] + faceOff[c + 1]) - faceOff[c - 2]; //combine sections
                    faceOff.RemoveAt(c);
                    faceOff.RemoveAt(c);
                }
                else
                    c += 2;
            }

        }

        private static int convertSolo(List<int> notes, NotesDifficulty difficulty, int[] sourceNotes, int index, NotesDifficulty sourceDifficulty)
        {

            int idx = index;

            int[] sn = sourceNotes;
            List<int> n = notes;

            int b = sn[idx + 2];
            //if not single button then stop
            if ((b & 0x1f) != 0x1 && (b & 0x1f) != 0x2 && (b & 0x1f) != 0x4 && (b & 0x1f) != 0x8 && (b & 0x1f) != 0x10)
                return idx;
            
            //find string of single buton notes that break the threshold
            while (idx + 3 < sn.Length && sn[idx + 3] - sn[idx] < THRESHOLD[(int)difficulty])
            {
                b = sn[(idx + 3) + 2];
                //if not single button then stop
                if ((b & 0x1f) != 0x1 && (b & 0x1f) != 0x2 && (b & 0x1f) != 0x4 && (b & 0x1f) != 0x8 && (b & 0x1f) != 0x10)
                    break;

                idx += 3;
            }

            //move the solo notes to the new list
            if ((idx - index) / 3 > 5)
            {
                int lastB = 0;     //lastB modified
                int lastBOrig = 0; //lastB untouched

                for (int i = index; i <= idx; i += 3)
                {
                    b = sn[i + 2]; //button mask
                    int hopo = (b & 0x20);
                    b &= 0x1f; //note buttons only

                    bool brokeThreshold = (n.Count >= 3 && sn[i] - n[n.Count - 3] < SOLO_THRESHOLD[(int)difficulty]);

                    if (!brokeThreshold)
                    {
                        switch (difficulty)
                        {
                            case NotesDifficulty.Easy:
                                int x = b & 0x1; //green to green;
                                if (lastB == 0x2) //if last button was red then use green
                                    x |= (b & 0x2) >> 1; //red to green
                                else
                                    x |= (b & 0x2); //red to red
                                x |= (b & 0x4) >> 1; //yellow to red
                                if (lastB == 0x4) //if last button was yellow then use red
                                    x |= (b & 0x8) >> 2; //blue to yellow
                                else
                                    x |= (b & 0x8) >> 1; //blue to yellow
                                x |= (b & 0x10) >> 2; //orange to yellow
                                b = x; 
                                break;
                            case NotesDifficulty.Medium:
                                //if we have orange or the last note had orange or blue
                                if ((b & 0x10) == 0x10 || ((lastBOrig & 0x10) == 0x10 || (lastBOrig & 0x10) == 0x8)) //orange
                                   b = ((b & 0x18) >> 1) | (b & 0x7);
                                break;
                            default:
                                break;
                        }
                        n.Add(sn[i]);
                        n.Add(sn[i + 1]);
                        n.Add(b | hopo);

                        lastB = b;  //copy last note without hopo
                        lastBOrig = sn[i + 2] & 0x1f; //note buttons only;

                    }
                }
                return idx; //last button in solo
            }
            else
                return index;

        }


        public static int[] CreateDifficulty(NotesDifficulty difficulty, int[] sourceNotes, NotesDifficulty sourceDifficulty)
        {
            int[] sn = sourceNotes;

            List<int> n = new List<int>();
            int lastB = 0;     //lastB modified
            int lastBOrig = 0; //lastB untouched
            int lastBC = 0;    //last buttons count

            if ((int)difficulty >= (int)sourceDifficulty)
            {
                n = new List<int>(sn);
            }
            else
            {
                for (int i = 0; i < sn.Length; i += 3)
                {
                    int b = sn[i + 2]; //button mask
                    int hopo = (b & 0x20);
                    int bc = 0; //count buttons used
                    bool brokeThreshold = (n.Count >= 3 && sn[i] - n[n.Count - 3] < THRESHOLD[(int)difficulty]);


                    //count buttons used
                    for (int j = 4; j >= 0; j--)
                    {
                        if ((1 << j & b) != 0)
                        {
                            bc = j + 1;
                            break;
                        }
                    }

                    if (n.Count == 0 || !brokeThreshold || brokeThreshold && bc > lastBC)
                    {
                        //if the threshold was broken and the note was more prominent then remove the previous note
                        if (n.Count != 0 && (brokeThreshold && bc > lastBC))
                        {
                            n.RemoveAt(n.Count - 1);
                            n.RemoveAt(n.Count - 1);
                            n.RemoveAt(n.Count - 1);
                            //n[n.Count - 1] |= 0x20; //turn on hopo instead of deleting for debugging
                        }
                        else
                        {
                            int endIdx = convertSolo(n, difficulty, sourceNotes, i, sourceDifficulty);
                            //end index is the last note of the solo, add 3 as it will not incremented when the for loop continues.
                            if (endIdx != i)
                            {
                                lastB = 0;
                                lastBOrig = 0;
                                lastBC = 0;
                                i = endIdx;
                                continue;
                            }

                        }

                        n.Add(sn[i]);
                        n.Add(sn[i + 1]);
                        b &= 0x1f; //note buttons only

                        if (b == lastBOrig)  //repeat what ever change we made last time
                            b = lastB;
                        else
                        {

                            switch (difficulty)
                            {
                                case NotesDifficulty.Easy:
                                    switch (b)
                                    {
                                        case 0xb:     //01011
                                            b = 0x2;  //00010
                                            break;
                                        case 0x16:    //10110
                                            b = 0x4;  //00100
                                            break;
                                        case 0xd:     //01101
                                            b = 0x4;  //00100
                                            break;
                                        case 0x1a:    //11010
                                            b = 0x8;  //01000
                                            break;
                                        case 0x1c:    //11100
                                            b = 0x8;  //01000
                                            break;
                                        case 0xe:     //01110
                                            b = 0x4;  //00100
                                            break;
                                        case 0x7:     //00111
                                            b = 0x2;  //00010
                                            break;
                                        case 0x1e:    //11110
                                            b = 0x18; //11000
                                            break;
                                        case 0xf:     //01111
                                            b = 0x3;  //00011
                                            break;
                                        case 0x1f:    //11111
                                            b = 0xe;  //01110
                                            break;
                                    }
                                    int x = b & 0x1; //green to green;
                                    x |= (b & 0x2); //red to red
                                    x |= (b & 0x4) >> 1; //yellow to red
                                    x |= (b & 0x8) >> 1; //blue to yellow
                                    x |= (b & 0x10) >> 2; //orange to yellow
                                    b = x; 
                                    break;
                                case NotesDifficulty.Medium:
                                    switch (b)
                                    {
                                        case 0xb:     //01011
                                            b = 0x3;  //00011
                                            break;
                                        case 0x16:    //10110
                                            b = 0xc;  //01100
                                            break;
                                        case 0xd:     //01101
                                            b = 0x6;  //00110
                                            break;
                                        case 0x1a:    //11010
                                            b = 0x18; //11000
                                            break;
                                        case 0x1c:    //11100
                                            b = 0x14; //10100
                                            break;
                                        case 0xe:     //01110
                                            b = 0xa;  //01010
                                            break;
                                        case 0x7:     //00111
                                            b = 0x5;  //00101
                                            break;
                                        case 0x1e:    //11110
                                            b = 0x12; //10010
                                            break;
                                        case 0xf:     //01111
                                            b = 0x9;  //01001
                                            break;
                                        case 0x1f:    //11111
                                            b = 0xa;  //01010
                                            break;
                                    }
                                    //if we have orange or the last note had orange or blue
                                    if ((b & 0x10) == 0x10 || ((lastBOrig & 0x10) == 0x10 || (lastBOrig & 0x10) == 0x8)) //orange
                                        b = ((b & 0x18) >> 1) | (b & 0x7);

                                    break;
                                case NotesDifficulty.Hard:

                                    switch(b)
                                    {
                                        case 0xb:     //01011
                                            b = 0xa;  //01010
                                            break;
                                        case 0x16:    //10110
                                            b = 0x14; //10100
                                            break;
                                        case 0xd:     //01101
                                            b = 0x5;  //00101
                                            break;
                                        case 0x1a:    //11010
                                            b = 0xa;  //01010
                                            break;
                                        case 0x1e:    //11110
                                            b = 0x1a; //11010
                                            break;
                                        case 0xf:     //01111
                                            b = 0xb;  //01011
                                            break;
                                        case 0x1f:    //11111
                                            b = 0x1a; //11010
                                            break;
                                    }
                                    break;
                            }
                        }
                        n.Add(b | hopo);
                        lastB = b;  //copy last note without hopo
                        lastBC = bc;
                        lastBOrig = sn[i + 2] & 0x1f; //note buttons only;
                    }
                }
            }

            return n.ToArray();
        }

        public static int DefaultStarPowerSections
        {
            get { return 10; }
        }

        public static int DefaultBattlePowerSections
        {
            get { return 20; }
        }

        
        private static int[] THRESHOLD = {400,200,125,0};
        private static int[] SOLO_THRESHOLD = { 300, 150, 100, 0 }; //multi button threshold



    }
}
