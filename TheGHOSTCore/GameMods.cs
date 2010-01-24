using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;
using System.Text.RegularExpressions;

namespace Nanook.TheGhost
{
    public class GameMods
    {
        internal GameMods(Project project)
        {
            _project = project;
            _restoreSongs = new List<QbKey>();
            _nonCareerSongs = new List<QbKey>();
            _specialSongs = new List<QbKey>();

            if (project.GameInfo.Game == Game.GH3_Wii)
            {
                foreach (string s in "slowride,TalkDirtyToMe,hitmewithyourbestshot,storyofmylife,rocknrollallnite,mississippiqueen,schoolsout,sunshineofyourlove,barracuda,bullsonparade,whenyouwereyoung,missmurder,theseeker,laydown,paintitblack,paranoid,anarchyintheuk,koolthing,mynameisjonas,evenflow,holidayincambodia,rockulikeahurricane,sameoldsonganddance,lagrange,welcometothejungle,blackmagicwoman,cherubrock,blacksunshine,themetal,pridenjoy,beforeiforget,stricken,threesandsevens,knightsofcydonia,cultofpersonality,rainingblood,cliffsofdover,numberofthebeast,one,avalancha,bellyofashark,cantbesaved,closer,dontholdback,downndirty,fcpremix,generationrock,gothatfar,hierkommtalex,imintheband,impulse,inlove,mauvaisgarcon,metalheavylady,minuscelsius,mycurse,nothingformehere,prayeroftherefugee,radiosong,ruby,shebangsadrum,takethislife,thewayitends".Split(','))
                    _restoreSongs.Add(QbKey.Create(s));
                foreach (string s in "sabotage,reptilia,suckmykiss,citiesonflame,helicopter,monsters".Split(','))
                    _nonCareerSongs.Add(QbKey.Create(s));
                foreach (string s in "thrufireandflames".Split(','))
                    _specialSongs.Add(QbKey.Create(s));

            }
            else
            {
                foreach (string s in "dreampolice,alltheyoungdudes,makeit,unclesalty,drawtheline,ihatemyselfforlovingyou,alldayandallofthenight,nosurprize,movinout,sweetemotion,completecontrol,personalitycrisis,livinontheedge,ragdoll,loveinanelevator,shesellssanctuary,kingofrock,nobodysfault,brightlightfright,walkthiswayrundmc,hardtohandle,alwaysontherun,backinthesaddle,beyondbeautiful,dreamon,catscratchfever,sextypething,mamakin,toysintheattic,trainkeptarollin,walkthisway,ratsinthecellar,combination,letthemusicdothetalking,shakinmycage,pink,talktalking,mercy,pandorasbox,joeperryguitarbattle".Split(','))
                    _restoreSongs.Add(QbKey.Create(s));
                //foreach (string s in "kingsandqueens,kingsandqueenscredits".Split(','))
                //    _nonCareerSongs.Add(QbKey.Create(s));
                foreach (string s in "kingsandqueens,kingsandqueenscredits".Split(','))
                    _specialSongs.Add(QbKey.Create(s));

            }
        }

        /// <summary>
        /// Add song to bonus list and shop
        /// </summary>
        /// <param name="songQk"></param>
        public void BonusSongAddToGame(QbFile qbStore, QbFile qbSongList, QbKey qkSong, bool insertAdd)
        {
            //is this song (this) in the bonus tiers already?
            bool found = getAllBonusSongs(qbStore).Contains(qkSong);
            addSongToSonglist(qbSongList, qkSong);
            this.AddBonusSongQbItems(qkSong, qbStore, insertAdd);
        }

        private static void addSongToSonglist(QbFile qbSongList, QbKey qkSong)
        {
            QbItemStruct songs = qbSongList.FindItem(QbKey.Create("permanent_songlist_props"), false) as QbItemStruct;
            QbItemArray songsList = qbSongList.FindItem(QbKey.Create("gh3_songlist"), false) as QbItemArray;
            if (songsList != null)
            {
                List<QbKey> songlistQk = new List<QbKey>(((QbItemQbKey)songsList.Items[0]).Values);

                if (!songlistQk.Contains(qkSong))
                {
                    songs.AddItem((QbItemBase)SongQb.CreateSong(songs.Root, qkSong.Text));
                    songlistQk.Add(qkSong);
                }


                //update gh3_songlist
                ((QbItemQbKey)songsList.Items[0]).Values = songlistQk.ToArray();

                qbSongList.AlignPointers();
                qbSongList.IsValid();
            }
        }

        /// <summary>
        /// Remove song from bonus list and shop
        /// </summary>
        public void BonusSongRemoveFromGame(QbFile qbStore, QbFile qbSongList, QbKey qkSong)
        {
            //is this song (this) in the bonus tiers?
            bool found = getAllBonusSongs(qbStore).Contains(qkSong);

            removeSongFromSonglist(qbSongList, qkSong);

            this.RemoveBonusSongQbItems(qkSong, qbStore);
        }

        private void removeSongFromSonglist(QbFile qbSongList, QbKey qkSong)
        {
            QbItemArray songsList = qbSongList.FindItem(QbKey.Create("gh3_songlist"), false) as QbItemArray;
            if (songsList != null)
            {
                List<QbKey> songlistQk = new List<QbKey>(((QbItemQbKey)songsList.Items[0]).Values);

                if (songlistQk.Contains(qkSong))
                    songlistQk.Remove(qkSong);

                //update gh3_songlist
                ((QbItemQbKey)songsList.Items[0]).Values = songlistQk.ToArray();

                qbSongList.AlignPointers();
                qbSongList.IsValid();
            }
        }

        public void CopyProjectNamesToOtherTiers()
        {
            List<TierQb> gen = _project.BuildTierList(_project.FileManager.GuitarProgressionQbFile, QbKey.Create("GH3_General_Songs"));
            List<TierQb> genP2 = _project.BuildTierList(_project.FileManager.GuitarProgressionQbFile, QbKey.Create("GH3_GeneralP2_Songs"));
            List<TierQb> genP2CoOp = _project.BuildTierList(_project.FileManager.GuitarProgressionQbFile, QbKey.Create("GH3_GeneralP2_Songs_Coop"));
            List<TierQb> genCoOp = null;

            QbFile coop = _project.FileManager.GuitarCoOpQbFile;
            if (coop != null) //gh3 only
                genCoOp = _project.BuildTierList(coop, QbKey.Create("GH3_CoopCareer_Songs"));

            ProjectTier pt;
            for (int i = 0; i < _project.Tiers.Count; i++)
            {
                pt = _project.Tiers[i];

                if (pt.Type == TierType.Career)
                {
                    if (i < gen.Count)
                        gen[i].Name = pt.Name;
                    if (genP2 != null && i < genP2.Count)
                        genP2[i].Name = pt.Name;
                    if (genP2CoOp != null && i < genP2CoOp.Count)
                        genP2CoOp[i].Name = pt.Name;
                    if (genCoOp != null && gen.Count == genCoOp.Count) //only if gh3 and has the same ter count
                        genCoOp[i].Name = pt.Name;
                }
            }



        }

        public void SetCheats(PakEditor qbPak)
        {
            string cheatQb = @"scripts\guitar\menu\menu_cheats.qb.ngc";
            if (_project.GameInfo.Game == Game.GH3_Wii)
                cheatQb = cheatQb.Substring(1);  //remove first char

            QbFile qb = qbPak.ReadQbFile(cheatQb);

            this.SetCheats(qb);

            qbPak.ReplaceFile(cheatQb, qb);
        }

        public void SetCheats(QbFile menuCheatsQb)
        {
            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                //edit the cheats
                uint t = 0x00010000;
                uint f = 0x00011000;
                bool b = true;
                menuCheatsQb.FindItem(true, delegate(QbItemBase qib)
                {
                    if (qib.QbItemType == QbItemType.StructItemArray && qib.ItemQbKey == QbKey.Create("unlock_pattern"))
                    {
                        uint[] ui = new uint[1];
                        if (b)
                        {
                            ui[0] = t;
                            t >>= 4;
                        }
                        else
                        {
                            ui[0] = f;
                            f >>= 4;
                        }
                        b = !b;
                        ((QbItemInteger)qib.Items[0]).Values = ui;
                    }
                    return false;
                });
                menuCheatsQb.AlignPointers();
                menuCheatsQb.IsValid();
            }
        }


        public void RemoveIntroVids(PakEditor qbPak)
        {
            string bootupQb = @"scripts\guitar\menu\bootup_menu_flow.qb.ngc";
            if (_project.GameInfo.Game == Game.GH3_Wii)
                bootupQb = bootupQb.Substring(1);  //remove first char

            QbFile qb = qbPak.ReadQbFile(bootupQb);

            this.RemoveIntroVids(qb);

            qbPak.ReplaceFile(bootupQb, qb);
        }

        public void RemoveIntroVids(QbFile bootupMenuFlowQb)
        {
            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                //remove the intro videos
                QbItemScript qbs = bootupMenuFlowQb.FindItem(QbKey.Create("bootup_sequence"), false) as QbItemScript;
                if (qbs.ScriptData.Length == 474) //make sure it hasn't already been edited
                {
                    byte[] scr = new byte[68 + 130];
                    Array.Copy(qbs.ScriptData, scr, 68);
                    Array.Copy(qbs.ScriptData, qbs.ScriptData.Length - 130, scr, 68, 130);
                    qbs.ScriptData = scr;
                    bootupMenuFlowQb.AlignPointers();
                    bootupMenuFlowQb.IsValid();
                }
            }
        }

        public void ResetBonusArt(PakEditor qbPak)
        {
            string storeQb = @"scripts\guitar\store_data.qb.ngc";
            if (_project.GameInfo.Game == Game.GH3_Wii)
                storeQb = storeQb.Substring(1);  //remove first char

            QbFile qbStore = qbPak.ReadQbFile(storeQb);

            this.ResetBonusArt(qbStore);

            qbPak.ReplaceFile(storeQb, qbStore);
        }

        public void ResetBonusArt(QbFile storeDataQb)
        {

            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                QbItemArray qa = storeDataQb.FindItem(QbKey.Create("Bonus_Songs_Info"), false) as QbItemArray;

                if (qa != null)
                {
                    //set all prices to 0
                    QbKey cover = QbKey.Create("album_cover");
                    qa.FindItem(true, delegate(QbItemBase qib)
                    {
                        if (qib.ItemQbKey != null && qib.ItemQbKey.Crc == cover.Crc)
                            ((QbItemQbKey)qib).Values[0] = QbKey.Create("store_song_default");
                        return false; //loop until end of qb
                    });

                    storeDataQb.AlignPointers();
                    storeDataQb.IsValid();
                }
            }
        }

        public void ResetBonusInfoText(PakEditor qbPak, string infoText)
        {
            string storeQb = @"scripts\guitar\store_data.qb.ngc";
            if (_project.GameInfo.Game == Game.GH3_Wii)
                storeQb = storeQb.Substring(1);  //remove first char

            QbFile qbStore = qbPak.ReadQbFile(storeQb);

            this.ResetBonusInfoText(qbStore, infoText);

            qbPak.ReplaceFile(storeQb, qbStore);
        }

        public void ResetBonusInfoText(QbFile storeDataQb, string infoText)
        {

            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                QbItemArray qa = storeDataQb.FindItem(QbKey.Create("Bonus_Songs_Info"), false) as QbItemArray;

                if (qa != null)
                {
                    //set all prices to 0
                    QbKey info = QbKey.Create("text");
                    qa.FindItem(true, delegate(QbItemBase qib)
                    {
                        if (qib.ItemQbKey != null && qib.ItemQbKey.Crc == info.Crc)
                            ((QbItemString)qib).Strings[0] = infoText;
                        return false; //loop until end of qb
                    });

                    storeDataQb.AlignPointers();
                    storeDataQb.IsValid();
                }
            }
        }

        public void FreeStore(PakEditor qbPak)
        {
            string storeQb = @"scripts\guitar\store_data.qb.ngc";
            if (_project.GameInfo.Game == Game.GH3_Wii)
                storeQb = storeQb.Substring(1);  //remove first char

            QbFile qbStore = qbPak.ReadQbFile(storeQb);
            this.FreeStore(qbStore);
            qbPak.ReplaceFile(storeQb, qbStore);
        }

        public void FreeStore(QbFile storeDataQb)
        {
            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                //set all prices to 0
                QbKey price = QbKey.Create("price");
                storeDataQb.FindItem(true, delegate(QbItemBase qib)
                    {
                        if (qib.ItemQbKey != null && qib.ItemQbKey.Crc == price.Crc)
                            ((QbItemInteger)qib).Values[0] = 0;
                        return false; //loop until end of qb
                    });

                storeDataQb.AlignPointers();
                storeDataQb.IsValid();
            }
        }

        public void UnlockSetlists(PakEditor qbPak, bool unlockAllTiers, bool completeTier1Song)
        {
            string progQb = @"scripts\guitar\guitar_progression.qb.ngc";
            string progCoopQb = @"scripts\guitar\guitar_coop.qb.ngc";

            if (_project.GameInfo.Game == Game.GH3_Wii)
                progQb = progQb.Substring(1);  //remove first char

            if (_project.GameInfo.Game == Game.GH3_Wii)
                progCoopQb = progCoopQb.Substring(1);  //remove first char


            QbFile qb = qbPak.ReadQbFile(progQb);
            QbFile qbCoop = _project.GameInfo.Game == Game.GH3_Wii ? qbPak.ReadQbFile(progCoopQb) : null;
            this.UnlockSetlists(qb, qbCoop, unlockAllTiers, completeTier1Song);

            qbPak.ReplaceFile(progQb, qb);
            if (qbCoop != null)
                qbPak.ReplaceFile(progCoopQb, qbCoop);
        }

        #region Tier Editing

        private QbItemStruct findTierProgStruct(QbKey firstChild, QbItemArray careerProgressionStruct)
        {
            QbKey tname = QbKey.Create("name");

            return careerProgressionStruct.FindItem(true, delegate(QbItemBase qib)
            {
                QbItemStruct qs = (qib as QbItemStruct);
                if (qs != null && qs.Items.Count != 0 && qs.Items[0] is QbItemQbKey)
                {
                    QbItemQbKey qk = (QbItemQbKey)qs.Items[0];
                    if (qk.ItemQbKey.Crc == tname.Crc && qk.Values[0].Crc == firstChild.Crc)
                        return true;
                }
                return false;
            }) as QbItemStruct;
        }

        private QbItemStruct copyTierProg(QbItemStruct prog, int toTierNo, params string[] qbKeyMask)
        {
            QbItemStruct copy = (QbItemStruct)prog.Clone();

            //replace all 1s for our new number
            copy.FindItem(true, delegate(QbItemBase qib)
                {
                    if (qib.ItemQbKey != null)
                    {
                        if (qib is QbItemInteger && qib.ItemQbKey.Crc == QbKey.Create("tier").Crc)
                            ((QbItemInteger)qib).Values[0] = (uint)toTierNo;
                        else if (qib is QbItemQbKey)
                        {
                            foreach (string s in qbKeyMask)
                            {
                                QbKey k = QbKey.Create(string.Format(s, "1"));
                                QbKey k2 = QbKey.Create(string.Format(s, "0"));

                                if (((QbItemQbKey)qib).Values[0].Crc == k.Crc)
                                    ((QbItemQbKey)qib).Values[0] = QbKey.Create(string.Format(s, toTierNo.ToString()));
                                else if (((QbItemQbKey)qib).Values[0].Crc == k2.Crc)
                                    ((QbItemQbKey)qib).Values[0] = QbKey.Create(string.Format(s, (toTierNo - 1).ToString()));
                            }
                        }
                    }
                    return false; //return false to continue search
                });
            return copy;
        }

        /// <summary>
        /// All but this tier 1 have a dependency on the previous tier being completed
        /// </summary>
        /// <param name="tier"></param>
        private void addDependencyToTier1SongsComplete(QbItemStruct prog)
        {
            //insert dependency on previous tier.  Obviously tier 0 does exist, it's just for replacement purposes
            QbItemBase qbs = prog.Items[prog.Items.Count - 1];
            if (qbs is QbItemArray)
            {
                QbItemStructArray qsa = (QbItemStructArray)qbs.Items[0];
                QbItemStruct qis = new QbItemStruct(prog.Root);
                qis.Create(QbItemType.StructHeader);

                QbItemQbKey qbQk = new QbItemQbKey(prog.Root);
                qbQk.Create(QbItemType.StructItemQbKey);
                qbQk.ItemQbKey = QbKey.Create("type");
                qbQk.Values = new QbKey[] { QbKey.Create("atom") };
                qis.AddItem(qbQk);

                qbQk = new QbItemQbKey(prog.Root);
                qbQk.Create(QbItemType.StructItemQbKey);
                qbQk.ItemQbKey = QbKey.Create("atom");
                qbQk.Values = new QbKey[] { QbKey.Create("career_tier0_complete") };
                qis.AddItem(qbQk);

                qsa.InsertItem(qis, qsa.Items[0], true);
            }
        }


        private void insertFinishGame(QbItemStruct prog)
        {
            prog.FindItem(true, delegate(QbItemBase qib)
            {
                if (qib is QbItemStruct && qib.ItemQbKey.Crc == QbKey.Create("atom_params").Crc)
                {
                    QbItemQbKey qk = new QbItemQbKey(prog.Root);
                    qk.Create(QbItemType.StructItemQbKey);
                    qk.Values = new QbKey[] { QbKey.Create("finished_game") };
                    ((QbItemStruct)qib).AddItem(qk);
                    return true; //stop after the first find
                }

                return false; //return false to continue search
            });
        }


        private bool removeBonusSong(QbFile storeQb, QbKey songId)
        {
            //find bonus song list
            QbItemArray bonusSongs = storeQb.FindItem(true, delegate(QbItemBase qib)
            {
                return (qib.QbItemType == QbItemType.StructItemArray && qib.ItemQbKey == QbKey.Create("songs"));
            }) as QbItemArray;


            //remove bonus song from list
            List<QbKey> songs = new List<QbKey>((bonusSongs.Items[0] as QbItemQbKey).Values);

            if (songs.Exists(delegate(QbKey qk)
                {
                    return qk.Crc == songId.Crc;
                }))
            {
                this.RemoveBonusSongQbItems(songId, storeQb);
                return true;
            }
            else
                return false;
        }


        private void setCareerTiers(QbItemStruct careerSongsSec, int existingTierCount, int newTierCount, bool setUnlockIfOneTier)
        {
            //if setUnlockIfOneTier is false and we only have only 1 tier then don't unlock the new tiers, if tier 2 exists the lock is clones from that

            int add = newTierCount - existingTierCount;

            int clone;
            QbKey qkTier;
            QbKey qkNewTier;
            QbKey qkLastTier = QbKey.Create(string.Format("tier{0}", newTierCount.ToString()));

            for (int t = existingTierCount + 1; t <= newTierCount; t++)
            {
                int origTiersCount = Math.Min(existingTierCount, (_project.GameInfo.Game == Game.GH3_Wii) ? 8 : 6); //6 for aerosmith
                clone = t % origTiersCount;
                if (clone == 0)
                    clone = origTiersCount;

                qkTier = QbKey.Create(string.Format("tier{0}", clone.ToString()));
                qkNewTier = QbKey.Create(string.Format("tier{0}", t.ToString()));

                QbItemStruct clonedTier = careerSongsSec.FindItem(qkTier, false).Clone() as QbItemStruct;
                clonedTier.ItemQbKey = qkNewTier.Clone();

                if (clone == 1) //if tier 1 is being cloned then check is tier 1 has the unlocked value and make this the same
                {
                    QbItemStruct tier2 = careerSongsSec.FindItem(QbKey.Create("tier2"), false) as QbItemStruct;
                    if (tier2 != null)
                    {
                        tier2 = tier2.Clone() as QbItemStruct;
                        QbItemInteger ul = tier2.FindItem(QbKey.Create("defaultunlocked"), false) as QbItemInteger;
                        if (ul == null) //remove the cloned item
                        {
                            if ((ul = clonedTier.FindItem(QbKey.Create("defaultunlocked"), false) as QbItemInteger) != null)
                                clonedTier.RemoveItem(ul);
                        }
                    }
                    else if (!setUnlockIfOneTier)
                    {
                        //clear lock
                        QbItemInteger ul = clonedTier.FindItem(QbKey.Create("defaultunlocked"), false) as QbItemInteger;
                        if (ul != null)
                            clonedTier.RemoveItem(ul);
                    }

                }

                careerSongsSec.AddItem(clonedTier);
            }

            while (careerSongsSec.Items.Count > 3 && careerSongsSec.Items[careerSongsSec.Items.Count - 1].ItemQbKey.Crc != qkLastTier.Crc)
                careerSongsSec.RemoveItem(careerSongsSec.Items[careerSongsSec.Items.Count - 1]); //remove last item

            careerSongsSec.Root.AlignPointers();
            careerSongsSec.Root.IsValid();

        }

        private QbItemStruct copyCareerTiers(QbItemStruct careerSongsSec, QbItemStruct toTier)
        {
            //all other tier structs have encore2 in them so add it. and no completion movie item
            QbItemBase tmp;

            QbKey qkInitialMovie = QbKey.Create("initial_movie");
            QbKey qkCompletionMovie = QbKey.Create("completion_movie");
            QbKey qkEncoreP1 = QbKey.Create("encorep1");

            QbKey qkPrefix = QbKey.Create("prefix");
            string prefix = ((QbItemString)toTier.FindItem(qkPrefix, false)).Strings[0];

            QbItemStruct to = (QbItemStruct)careerSongsSec.Clone();

            if ((tmp = to.FindItem(qkInitialMovie, false)) != null)
                to.RemoveItem(tmp); //remove initial move if found

            to.ItemQbKey = toTier.ItemQbKey.Clone();
            ((QbItemString)to.FindItem(qkPrefix, false)).Strings[0] = prefix;

            QbItemQbKey encorep2 = new QbItemQbKey(to.Root);
            encorep2.Create(QbItemType.StructItemQbKey);
            encorep2.Values = new QbKey[] { QbKey.Create("encorep2") };


            foreach (QbItemBase qb in to.Items)
            {
                if (qb.QbItemType == QbItemType.StructItemStruct)
                {
                    if ((tmp = qb.FindItem(qkCompletionMovie, false)) != null)
                        qb.RemoveItem(tmp); //remove initial move if found

                    if ((tmp = qb.FindItem(false, delegate (QbItemBase q)
                        {
                            return (qb.QbItemType == QbItemType.StructItemQbKey && qb.ItemQbKey != null && qb.ItemQbKey.Crc == 0 && ((QbItemQbKey)qb).Values[0].Crc == qkEncoreP1.Crc);
                        })) != null)
                    {
                        qb.InsertItem(encorep2.Clone(), tmp, false);
                    }
                }
            }

            return to;
        }

//#if DEBUG
//        /// <summary>
//        /// Edits tiers using the files in Partition folder
//        /// </summary>
//        /// <param name="removeBossBattles"></param>
//        /// <param name="bonusSongs"></param>
//        /// <param name="songCounts"></param>
//        public void CoopCopyTest(PakEditor pak)
//        {
//            QbKey[] nonCareerSongs = new QbKey[0];

//            string progQb = @"scripts\guitar\guitar_progression.qb.ngc";
//            string coopQb = @"scripts\guitar\guitar_coop.qb.ngc";
//            string storeDataQb = @"scripts\guitar\store_data.qb.ngc";
//            string songlistQb = @"scripts\guitar\songlist.qb.ngc";
//            if (_project.GameInfo.Game == Game.GH3_Wii)
//            {
//                progQb = progQb.Substring(1);  //remove first char
//                coopQb = coopQb.Substring(1);  //remove first char
//                storeDataQb = storeDataQb.Substring(1);  //remove first char
//                songlistQb = songlistQb.Substring(1);  //remove first char
//            }

//            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
//            {
//                //add song to song list
//                QbFile qbProg = pak.ReadQbFile(progQb);
//                QbFile qbCoop = _project.GameInfo.Game == Game.GH3_Wii ? pak.ReadQbFile(coopQb) : null;
//                QbFile qbStore = pak.ReadQbFile(storeDataQb);
//                QbFile qbSonglist = pak.ReadQbFile(songlistQb);

//                copyCareerTiersToCoop(qbCoop, qbProg);

//                pak.ReplaceFile(coopQb, qbCoop);
//            }
//        }
//#endif

        private void copyCareerTiersToCoop(QbFile coopQb, QbFile guitarProgressionQb)
        {
            int coopMoviesCount = 6;

            QbKey qkEncore = QbKey.Create("encorep1");
            QbKey qkEncore2 = QbKey.Create("encorep2");
            QbKey qkLevel = QbKey.Create("level");
            QbKey qkLevel2 = QbKey.Create("level2");
            QbKey qkNumTiers = QbKey.Create("num_tiers");
            QbKey qkCompMovie = QbKey.Create("completion_movie");
            QbKey qkSetList = QbKey.Create("setlist_icon");
            QbKey qkNumSongs = QbKey.Create("numsongstoprogress");
            QbKey qkCoopNumSongs = QbKey.Create("GH3_COOPCareer_NumSongToProgress");
            QbKey qkAtom = QbKey.Create("atom");
            QbKey qkTier = QbKey.Create("tier");

            QbItemStruct careerSongsSec = guitarProgressionQb.FindItem(QbKey.Create("GH3_Career_Songs"), false) as QbItemStruct;
            QbItemInteger qbNumTiers = careerSongsSec.FindItem(qkNumTiers, false) as QbItemInteger;
            int currTierCount = (int)qbNumTiers.Values[0];

            QbItemStruct coopSongsSec = coopQb.FindItem(QbKey.Create("GH3_CoopCareer_Songs"), false) as QbItemStruct;
            QbItemInteger qbCoopNumTiers = coopSongsSec.FindItem(qkNumTiers, false) as QbItemInteger;
            int currCoopTierCount = (int)qbCoopNumTiers.Values[0];
            qbCoopNumTiers.Values[0] = (uint)currTierCount; //set new value

            //copy file ids

            #region copy setlist

            for (int i = 1; i <= currCoopTierCount; i++)
            {
                QbItemStruct srcTier = coopSongsSec.FindItem(QbKey.Create(string.Format("tier{0}", i.ToString())), false) as QbItemStruct;
                if (srcTier == null)
                    continue;

                coopSongsSec.RemoveItem(srcTier);
            }
            


            for (int i = 1; i <= currTierCount; i++)
            {
                QbItemStruct srcTier = careerSongsSec.FindItem(QbKey.Create(string.Format("tier{0}", i.ToString())), false) as QbItemStruct;
                if (srcTier == null)
                    continue;


                srcTier = (QbItemStruct)srcTier.Clone();


                foreach (QbItemBase qib in srcTier.Items)
                {
                    if (qib.QbItemType == QbItemType.StructItemQbKey)
                    {
                        if (((QbItemQbKey)qib).Values[0].Crc == qkEncore.Crc)
                            ((QbItemQbKey)qib).Values[0] = qkEncore2.Clone();
                    }
                    else if (qib.QbItemType == QbItemType.StructItemString && qib.ItemQbKey != null && qib.ItemQbKey.Crc == qkCompMovie.Crc)
                    {
                        int m = i + 1;
                        if (m < currTierCount)
                            ((QbItemString)qib).Strings[0] = string.Format("coop_{0}", (m < coopMoviesCount ? m : (m % coopMoviesCount == 0 ? coopMoviesCount : m % coopMoviesCount)).ToString().PadLeft(2,'0'));
                        else
                            ((QbItemString)qib).Strings[0] = "singleplayer_end"; //on 2nd last tier

                    }
                }

                if (i == currTierCount) //last tier
                {
                    QbItemBase tmp = srcTier.FindItem(qkCompMovie, false);
                    if (tmp != null)
                        srcTier.RemoveItem(tmp); //no movie in last coop tier

                    tmp = srcTier.FindItem(qkSetList, false);
                    if (tmp != null)
                        srcTier.RemoveItem(tmp);

                    tmp = new QbItemQbKey(guitarProgressionQb);
                    tmp.Create(QbItemType.StructItemQbKey);
                    tmp.ItemQbKey = qkLevel2.Clone();
                    ((QbItemQbKey)tmp).Values = new QbKey[] { QbKey.Create("load_z_hell") };
                    srcTier.AddItem(tmp);

                    tmp = new QbItemQbKey(guitarProgressionQb);
                    tmp.Create(QbItemType.StructItemQbKey);
                    //tmp.ItemQbKey = null
                    ((QbItemQbKey)tmp).Values = new QbKey[] { QbKey.Create("nocash") };
                    srcTier.AddItem(tmp);

                }

                coopSongsSec.AddItem(srcTier);

                coopQb.AlignPointers();
                coopQb.IsValid();
            }

            #endregion


            #region copy songs to progress values

            QbItemStruct careerSongs = guitarProgressionQb.FindItem(QbKey.Create("GH3_Career_NumSongToProgress"), false) as QbItemStruct;
            QbItemStruct coopSongs = coopQb.FindItem(QbKey.Create("GH3_COOPCareer_NumSongToProgress"), false) as QbItemStruct;

            foreach (QbItemBase qib in careerSongs.Items)
            {
                QbItemInteger qbVal = coopSongs.FindItem(qib.ItemQbKey, false) as QbItemInteger;
                if (qbVal != null)
                    qbVal.Values[0] = ((QbItemInteger)qib).Values[0];
            }
            


            #endregion

            #region copy progression

            QbItemArray careerSongsProg = guitarProgressionQb.FindItem(QbKey.Create("GH3_Career_Progression"), false) as QbItemArray;
            QbItemArray coopSongsProg = coopQb.FindItem(QbKey.Create("GH3_CoopCareer_Progression"), false) as QbItemArray;

            string[] masks = new string[4];

            masks[0] = "career_tier{0}_songscomplete";
            masks[1] = "career_tier{0}_encoreunlock";
            masks[2] = "career_tier{0}_encorecomplete";
            masks[3] = "career_tier{0}_complete";


            //remove existing progression entries
            for (int i = 1; i <= currCoopTierCount; i++)
            {
                for (int c = 0; c < masks.Length; c++)
                {
                    QbItemBase qib = findTierProgStruct(QbKey.Create(string.Format(masks[c], i.ToString())), coopSongsProg);
                    if (qib != null)
                        coopSongsProg.Items[0].RemoveItem(qib);
                }
            }

            //point all checks for the setlist being complete to the last tier, do while less items in list
            QbKey qkEndOrig = QbKey.Create(string.Format(masks[3], currCoopTierCount - 1)); //-1 if original coop tier list
            QbKey qkEndTG = QbKey.Create(string.Format(masks[3], currCoopTierCount)); //TheGHOST has alreay edited this ISO
            QbKey qkEndNew = QbKey.Create(string.Format(masks[3], currTierCount)); //New value
            coopSongsProg.Items[0].FindItem(true, delegate(QbItemBase qb)
                {
                    if (qb.ItemQbKey != null && qb.ItemQbKey.Crc == qkAtom.Crc)
                    {
                        QbItemQbKey qbk = (QbItemQbKey)qb;
                        if (qbk.Values[0].Crc == qkEndOrig.Crc || qbk.Values[0].Crc == qkEndTG.Crc)
                            qbk.Values[0] = qkEndNew;
                    }
                    return false;
                });


            QbItemBase qibUnlock = findTierProgStruct(QbKey.Create("career_unlock_unlockedsongs"), coopSongsProg);

            //edit to set end unlock info.  We hack this to just point to the last tier (so we can allow GH3 to have 1 toer if required)
            qibUnlock.FindItem(true, delegate(QbItemBase qb)
                {
                    if (qb.ItemQbKey != null && qb.ItemQbKey.Crc == qkTier.Crc)
                        ((QbItemInteger)qb).Values[0] = (uint)currTierCount; //point to the last tier
                    return false;
                });


            //insert new entries
            for (int i = 1; i <= currTierCount; i++)
            {
                for (int c = 0; c < masks.Length; c++)
                {
                    QbItemBase qib = findTierProgStruct(QbKey.Create(string.Format(masks[c], i.ToString())), careerSongsProg);
                    if (qib == null)
                        continue;

                    qib = qib.Clone();

                    qib.FindItem(true, delegate(QbItemBase qb)
                        {
                            if (qb.ItemQbKey != null && qb.ItemQbKey.Crc == qkNumSongs.Crc)
                                ((QbItemQbKey)qb).Values[0] = qkCoopNumSongs.Clone();
                            return false;
                        });

                    coopSongsProg.Items[0].InsertItem(qib, qibUnlock, true);

                }
            }





            #endregion



            coopQb.AlignPointers();
            coopQb.IsValid();
            
        }

        /// <summary>
        /// Edits tiers using the files in Partition folder
        /// </summary>
        /// <param name="removeBossBattles"></param>
        /// <param name="bonusSongs"></param>
        /// <param name="songCounts"></param>
        public void EditTiers(PakEditor pak, bool removeBossBattles, int setTierCount, int bonusSongs, int[] songCounts, bool unlockTiers)
        {
            QbKey[] nonCareerSongs = new QbKey[0];

            string progQb = @"scripts\guitar\guitar_progression.qb.ngc";
            string coopQb = @"scripts\guitar\guitar_coop.qb.ngc";
            string storeDataQb = @"scripts\guitar\store_data.qb.ngc";
            string songlistQb = @"scripts\guitar\songlist.qb.ngc";
            if (_project.GameInfo.Game == Game.GH3_Wii)
            {
                progQb = progQb.Substring(1);  //remove first char
                coopQb = coopQb.Substring(1);  //remove first char
                storeDataQb = storeDataQb.Substring(1);  //remove first char
                songlistQb = songlistQb.Substring(1);  //remove first char
            }

            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                //add song to song list
                QbFile qbProg = pak.ReadQbFile(progQb);
                QbFile qbCoop = _project.GameInfo.Game == Game.GH3_Wii ? pak.ReadQbFile(coopQb) : null;
                QbFile qbStore = pak.ReadQbFile(storeDataQb);
                QbFile qbSonglist = pak.ReadQbFile(songlistQb);

                this.EditTiers(qbProg, qbStore, qbSonglist, qbCoop, removeBossBattles, setTierCount, bonusSongs, songCounts, unlockTiers);

                pak.ReplaceFile(progQb, qbProg);
                pak.ReplaceFile(storeDataQb, qbStore);
                if (qbCoop != null)
                    pak.ReplaceFile(coopQb, qbCoop);
                pak.ReplaceFile(songlistQb, qbSonglist);
            }
        }


        /// <summary>
        /// Clones Tier1 to the battle tiers.
        /// </summary>
        /// <param name="guitarProgressionQb"></param>
        public void EditTiers(QbFile guitarProgressionQb, QbFile storeQb, QbFile songlistQb, QbFile coopQb, bool removeBossBattles, int setTierCount, int bonusSongCount, int[] songCounts, bool unlockTiers)
        {
            bool isGh3 = _project.GameInfo.Game == Game.GH3_Wii;
            int minTierSongCount = int.MaxValue; //so we can work out the encore unlock values

            QbKey qkUnlockSong = _project.GameInfo.Game == Game.GH3_Wii ? QbKey.Create("thrufireandflames") : QbKey.Create("kingsandqueens");
            QbKey qkCreditsSong = QbKey.Create("kingsandqueenscredits");
            bool unlockSongExists = false;
            bool creditsSongExists = false;

            QbKey qkDevil = _project.GameInfo.Game == Game.GH3_Wii ? QbKey.Create("bossdevil") : QbKey.Create(""); //don't do it for Aerosmith
            QbKey[] bossSongs = new QbKey[] { QbKey.Create("bosstom"), QbKey.Create("bossslash"), QbKey.Create("bossdevil"), QbKey.Create("bossjoe") };

            string tierSongsComplete = "career_tier{0}_songscomplete";
            QbFile qb = guitarProgressionQb; //short ref

            QbKey tname = QbKey.Create("name");
            QbKey tcomp = QbKey.Create(string.Format(tierSongsComplete, "1"));


            QbItemStruct[] progs = new QbItemStruct[4];
            string[][] masks = new string[4][];

            List<QbKey> careerSongs = getAllCareerSongs(guitarProgressionQb);
            List<QbKey> bonusSongs = getAllBonusSongs(storeQb);
            int usedSong = 0;

            if (removeBossBattles)
            {
                foreach (QbKey qk in bossSongs)
                    careerSongs.Remove(qk);
            }

            if (songCounts != null)
            {
                //find TTFAF or KQ and remove, it will be added as the last song of the career
                for (int c = careerSongs.Count - 1; c >= 0; c--)
                {
                    QbKey qk = careerSongs[c];
                    if (qk.Crc == qkUnlockSong.Crc || qk.Crc == qkCreditsSong.Crc) //TTFAF or KQ (no credits)
                    {
                        careerSongs.Remove(qk);
                        if (qk.Crc == qkUnlockSong.Crc)
                            unlockSongExists = true;
                        else
                            creditsSongExists = true;
                    }
                }

                for (int c = bonusSongs.Count - 1; c >= 0; c--)
                {
                    QbKey qk = bonusSongs[c];
                    if (qk.Crc == qkUnlockSong.Crc || qk.Crc == qkCreditsSong.Crc) //TTFAF or KQ (no credits)
                    {
                        removeBonusSong(storeQb, qk);
                        bonusSongs.Remove(qk);
                        if (qk.Crc == qkUnlockSong.Crc)
                            unlockSongExists = true;
                        else
                            creditsSongExists = true;
                    }
                }
            }
            else if (removeBossBattles && isGh3) //add TTFAF to the end of the career tiers
            {
                for (int c = bonusSongs.Count - 1; c >= 0; c--)
                {
                    QbKey qk = bonusSongs[c];
                    if (qk.Crc == qkUnlockSong.Crc) //TTFAF
                    {
                        removeBonusSong(storeQb, qk);
                        bonusSongs.Remove(qk);
                        unlockSongExists = true;
                        break;
                    }
                }
            }

            //remove the boss items and set to encore
            QbItemStruct careerSongsSec = qb.FindItem(QbKey.Create("GH3_Career_Songs"), false) as QbItemStruct;
            QbItemInteger qbNumTiers = careerSongsSec.FindItem(QbKey.Create("num_tiers"), false) as QbItemInteger;
            int currTierCount = (int)qbNumTiers.Values[0];

            if (setTierCount != 0 && currTierCount != setTierCount)
            {
                setCareerTiers(careerSongsSec, currTierCount, setTierCount, unlockTiers);
                qbNumTiers.Values[0] = (uint)setTierCount;
            }

            if (setTierCount == 0)
                setTierCount = currTierCount; //no change

            #region career editing

            if (careerSongsSec != null)
            {
                QbKey qkEncore = QbKey.Create("encorep1");
                QbKey qkAeroEncore = QbKey.Create("aerosmith_encore_p1");
                QbKey qkBoss = QbKey.Create("boss");
                QbKey qkLevel = QbKey.Create("level");
                QbKey qkSongs = QbKey.Create("songs");
                QbKey qkUnlocked = QbKey.Create("defaultunlocked");
                QbKey qkNumTiers = QbKey.Create("num_tiers");

                int tierCount = 0;
                int tierNo = 0;

                //remove Through the fire and flames. It can only be unlocked with the cheat or by beating the devil
                //this.RemoveBonusSongQbItems(QbKey.Create("thrufireandflames"), storeQb);

                foreach (QbItemBase qib in careerSongsSec.Items)
                {
                    bool hasEncore = false;
                    bool hasBoss = false;
                    List<QbKey> sngs = null;

                    if (qib is QbItemInteger && qib.ItemQbKey != null && qib.ItemQbKey.Crc == qkNumTiers.Crc)
                        tierCount = (int)((QbItemInteger)qib).Values[0];

                    if (qib is QbItemStruct)
                    {
                        tierNo++; //assume they QB hold them in order

                        foreach (QbItemBase qib2 in qib.Items)
                        {
                            bool devilRemoved = false;
                            QbKey qiQk = null;
                            if (qib2.QbItemType == QbItemType.StructItemArray) //find songs to be removed
                            {
                                qiQk = qib2.ItemQbKey;
                                if (qiQk.Crc == qkSongs.Crc)
                                {
                                    if (removeBossBattles) //remove the battles if required
                                    {
                                        sngs = new List<QbKey>(((QbItemQbKey)qib2.Items[0]).Values);
                                        for (int si = sngs.Count - 1; si >= 0; si--)
                                        {
                                            foreach (QbKey k in bossSongs)
                                            {
                                                if (sngs[si].Crc == k.Crc)
                                                {
                                                    devilRemoved = k == qkDevil; //is it the devil?
                                                    removeSongFromSonglist(songlistQb, k);
                                                    sngs.RemoveAt(si--); //remove the boss song from the list
                                                }
                                            }
                                        }
                                    }

                                    //are we to modify the career tier song counts
                                    if (songCounts != null)
                                    {
                                        //set the career tier songs
                                        int tierSongs = songCounts[Math.Min(songCounts.Length - 1, tierNo - 1)];
                                        sngs.Clear();

                                        for (int i = 0; i < tierSongs; i++)
                                        {
                                            //last song of the career, use TTFAF or KQ (no credits)
                                            if (tierNo == tierCount && i == tierSongs - 1 && unlockSongExists)
                                            {
                                                careerSongs.Insert(usedSong, qkUnlockSong);
                                            }
                                            else if (usedSong >= careerSongs.Count)
                                            {
                                                //move song from bonus to career tier
                                                removeBonusSong(storeQb, bonusSongs[0]);
                                                careerSongs.Add(bonusSongs[0]);
                                                bonusSongs.RemoveAt(0);
                                            }

                                            sngs.Add(careerSongs[usedSong++]);
                                        }


                                    }
                                    else if (isGh3 && removeBossBattles && devilRemoved) //if tier songs are not to be edited then add ttfaf to the last tier (replace devil battle)
                                    {
                                        sngs.Add(qkUnlockSong);
                                    }

                                    if (sngs.Count < minTierSongCount)
                                        minTierSongCount = sngs.Count;

                                    ((QbItemQbKey)qib2.Items[0]).Values = sngs.ToArray();
                                }
                            }
                            else if (qib2.QbItemType == QbItemType.StructItemQbKey) //set the encore flag on all tiers, remove the boss
                            {
                                qiQk = ((QbItemQbKey)qib2).Values[0];
                                if (qiQk.Crc == qkEncore.Crc)
                                    hasEncore = true;
                                else if (qiQk.Crc == qkBoss.Crc || qiQk.Crc == qkAeroEncore.Crc)
                                {
                                    if (hasEncore) //just remove the boss or Aerosmith item
                                        qib.RemoveItem(qib2);
                                    else
                                        ((QbItemQbKey)qib2).Values[0] = QbKey.Create("encorep1");
                                    hasBoss = true;
                                    break;
                                }
                                else if (qiQk.Crc == qkLevel.Crc && !hasBoss && !hasEncore)
                                {
                                    QbItemQbKey enc = new QbItemQbKey(qib2.Root);
                                    enc.Create(QbItemType.StructItemQbKey);
                                    enc.Values = new QbKey[] { QbKey.Create("encorep1") };
                                    //insert new item
                                    qib.InsertItem(enc, qib2, true);
                                    break;
                                }
                            }
                            else if (qib2.QbItemType == QbItemType.StructItemInteger)
                            {
                                if (qib2.ItemQbKey != null && qib2.ItemQbKey.Crc == qkUnlocked.Crc && sngs != null && ((QbItemInteger)qib2).Values[0] < (uint)sngs.Count - 1)
                                    ((QbItemInteger)qib2).Values[0] = (uint)sngs.Count - 1;
                            }

                        }
                    }
                }

                if (songCounts == null)
                    usedSong = careerSongs.Count; //songs not touched

                //move the remaining Career Songs to the bonus section - ttfaf will not be the last song
                while (usedSong < careerSongs.Count)
                {
                    bonusSongs.Insert(0, careerSongs[careerSongs.Count - 1]);
                    this.AddBonusSongQbItems(careerSongs[careerSongs.Count - 1], storeQb, true);
                    careerSongs.RemoveAt(careerSongs.Count - 1);
                }

                while (bonusSongs.Count != 0 && bonusSongs.Count > bonusSongCount)
                {
                    this.BonusSongRemoveFromGame(storeQb, songlistQb, bonusSongs[bonusSongs.Count - 1]); //remove from set list also
                    //removeBonusSong(storeQb, bonusSongs[bonusSongs.Count - 1]);
                    bonusSongs.RemoveAt(bonusSongs.Count - 1);
                }

                //set the last bonus song to the creditsSongExists song
                if (!isGh3 && creditsSongExists)
                {
                    if (bonusSongs.Count != 0 && bonusSongs.Count == bonusSongCount) //remove last song
                    {
                        this.BonusSongRemoveFromGame(storeQb, songlistQb, bonusSongs[bonusSongs.Count - 1]); //remove from set list also
                        //removeBonusSong(storeQb, bonusSongs[bonusSongs.Count - 1]);
                        bonusSongs.RemoveAt(bonusSongs.Count - 1);
                    }
                    bonusSongs.Add(qkCreditsSong);
                    this.AddBonusSongQbItems(qkCreditsSong, storeQb, false);
                }

                //System.Diagnostics.Debug.WriteLine("\r\nCAREER\r\n");
                //foreach (QbKey q1 in careerSongs)
                //    System.Diagnostics.Debug.WriteLine(q1.Text);
                //System.Diagnostics.Debug.WriteLine("\r\nBONUS\r\n");
                //foreach (QbKey q1 in bonusSongs)
                //    System.Diagnostics.Debug.WriteLine(q1.Text);


                //set the songs to unlock section
                unlockXSongsToProgress(guitarProgressionQb,
                    Math.Max(1, (int)(minTierSongCount * 0.65)),
                    Math.Max(1, (int)(minTierSongCount * 0.75)),
                    Math.Max(1, (int)(minTierSongCount * 0.85)),
                    Math.Max(1, (int)(minTierSongCount * 0.95)),
                    Math.Max(1, bonusSongs.Count));
            }

            #endregion


            //modify the progression rules.
            QbItemArray careerProgSec = qb.FindItem(QbKey.Create("GH3_Career_Progression"), false) as QbItemArray;
            if (careerProgSec != null)
            {
                //use this temp
                progs[0] = findTierProgStruct(QbKey.Create("career_tier1_intro_songscomplete"), careerProgSec);
                if (progs[0] != null)
                    careerProgSec.Items[0].RemoveItem(progs[0]);

                progs[0] = findTierProgStruct(QbKey.Create("career_tier1_songscomplete"), careerProgSec);
                masks[0] = new string[] { "career_tier{0}_songscomplete", "career_tier{0}_complete" };
                progs[1] = findTierProgStruct(QbKey.Create("career_tier1_encoreunlock"), careerProgSec);
                masks[1] = new string[] { "career_tier{0}_encoreunlock", "career_tier{0}_songscomplete" };
                progs[2] = findTierProgStruct(QbKey.Create("career_tier1_encorecomplete"), careerProgSec);
                masks[2] = new string[] { "career_tier{0}_encorecomplete", "career_tier{0}_encoreunlock" };
                progs[3] = findTierProgStruct(QbKey.Create("career_tier1_complete"), careerProgSec);
                masks[3] = new string[] { "career_tier{0}_complete", "career_tier{0}_songscomplete", "career_tier{0}_encorecomplete" };
                progs[0] = (QbItemStruct)progs[0].Clone(); //we need to clone this before modifying it or the modified version will be saved.
                addDependencyToTier1SongsComplete(progs[0]);


                QbItemStruct copy = null;
                QbItemStruct last = progs[3]; //insert new items after this one
                QbItemBase rem = null;

                if (setTierCount == 1)
                    insertFinishGame(progs[3]);

                //set all prog items that trigger when completing the last tier
                QbKey qkCurr = QbKey.Create(string.Format("career_tier{0}_complete", currTierCount.ToString()));
                QbKey qkLast = QbKey.Create(string.Format("career_tier{0}_complete", setTierCount.ToString()));
                QbKey qkAtom = QbKey.Create("atom");
                careerProgSec.FindItem(true, delegate(QbItemBase qib)
                    {
                        if (qib.ItemQbKey != null && qib.ItemQbKey.Crc == qkAtom.Crc && ((QbItemQbKey)qib).Values[0].Crc == qkCurr.Crc)
                            ((QbItemQbKey)qib).Values[0] = qkLast.Clone(); //set to new last value
                        return false;
                    });

                //copy tier 1 to all tiers
                for (int t = 2; t <= Math.Max(setTierCount, currTierCount); t++)
                {
                    //remove the boss items
                    rem = findTierProgStruct(QbKey.Create(string.Format("career_tier{0}_bosscomplete", t.ToString())), careerProgSec);
                    if (rem != null)
                        careerProgSec.Items[0].RemoveItem(rem);
                    rem = findTierProgStruct(QbKey.Create(string.Format("career_tier{0}_bossunlock", t.ToString())), careerProgSec);
                    if (rem != null)
                        careerProgSec.Items[0].RemoveItem(rem);

                    rem = findTierProgStruct(QbKey.Create(string.Format("career_tier{0}_intro_songscomplete", t.ToString())), careerProgSec);
                    if (rem != null)
                        careerProgSec.Items[0].RemoveItem(rem);

                    for (int p = 0; p < progs.Length; p++)
                    {
                        //replace all 1s for our new number
                        copy = copyTierProg(progs[p], t, masks[p]);

                        //remove the existing item if present
                        rem = findTierProgStruct(((QbItemQbKey)copy.Items[0]).Values[0], careerProgSec);
                        careerProgSec.Items[0].RemoveItem(rem);

                        if (t <= setTierCount)
                        {
                            if (t == setTierCount && p == progs.Length - 1)
                                insertFinishGame(copy);
                            careerProgSec.Items[0].InsertItem(copy, last, false);
                        }
                        qb.AlignPointers();
                        qb.IsValid();

                        last = copy;
                    }
                }
            }
        
            QbItemStruct[] src = new QbItemStruct[] { (QbItemStruct)qb.FindItem(QbKey.Create("GH3_General_Songs"), false), 
                                                      (QbItemStruct)qb.FindItem(QbKey.Create("GH3_GeneralP2_Songs"), false),
                                                      (QbItemStruct)qb.FindItem(QbKey.Create("GH3_GeneralP2_Songs_Coop"), false) };
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] != null)
                {
                    QbItemStruct dst = copyCareerTiers(careerSongsSec, src[i]);
                    qb.InsertItem(dst, src[i], true);
                    qb.RemoveItem(src[i]);
                    qb.AlignPointers();
                    qb.IsValid();
                }
            }

            if (coopQb != null)
            {
                copyCareerTiersToCoop(coopQb, guitarProgressionQb);
            }
        }

        #endregion

        private List<QbKey> getAllCareerSongs(QbFile guitarProgression)
        {
            QbItemStruct careerSongsSec = guitarProgression.FindItem(QbKey.Create("GH3_Career_Songs"), false) as QbItemStruct;

            List<QbKey> songs = new List<QbKey>();

            if (careerSongsSec != null)
            {
                QbKey qkSongs = QbKey.Create("songs");
                careerSongsSec.FindItem(true, delegate(QbItemBase qib)
                    {
                        if (qib.ItemQbKey != null && qib.ItemQbKey.Crc == qkSongs.Crc)
                            songs.AddRange(((QbItemQbKey)qib.Items[0]).Values);
                        return false;
                    });
            }
            return songs;
        }

        private List<QbKey> getAllBonusSongs(QbFile storeData)
        {
            QbItemStruct bonusSongsSec = storeData.FindItem(QbKey.Create("GH3_Bonus_Songs"), false) as QbItemStruct;

            List<QbKey> songs = new List<QbKey>();

            if (bonusSongsSec != null)
            {
                QbKey qkSongs = QbKey.Create("songs");
                bonusSongsSec.FindItem(true, delegate(QbItemBase qib)
                    {
                        if (qib.ItemQbKey != null && qib.ItemQbKey.Crc == qkSongs.Crc)
                            songs.AddRange(((QbItemQbKey)qib.Items[0]).Values);
                        return false;
                    });
            }
            return songs;        
        }

        private void unlockXSongsToProgress(QbFile guitarProgressionQb, int easy, int medium, int hard, int expert, int bonus)
        {
            Dictionary<uint, int> vals = new Dictionary<uint, int>(4);
            vals.Add(QbKey.Create("easy").Crc, easy);
            vals.Add(QbKey.Create("medium").Crc, medium);
            vals.Add(QbKey.Create("hard").Crc, hard);
            vals.Add(QbKey.Create("expert").Crc, expert);

            if (bonus != 0)
                vals.Add(QbKey.Create("bonus").Crc, bonus);

            //set the tiers to be complete after 1 song complete
            QbKey qk = QbKey.Create("GH3_Career_NumSongToProgress");
            QbItemStruct nsp = guitarProgressionQb.FindItem(false, delegate(QbItemBase qib)
            {
                return (qib.QbItemType == QbItemType.SectionStruct && qib.ItemQbKey.Crc == qk.Crc);
            }) as QbItemStruct;
            if (nsp != null)
            {
                foreach (QbItemBase qib in nsp.Items)
                {
                    if (qib.QbItemType == QbItemType.StructItemInteger && qib.ItemQbKey != null && vals.ContainsKey(qib.ItemQbKey.Crc))
                        ((QbItemInteger)qib).Values[0] = (uint)vals[qib.ItemQbKey.Crc]; //set to 1 item
                }
            }
        }

        public void UnlockSetlists(QbFile guitarProgressionQb, QbFile guitarCoOpQb, bool unlockAllTiers, bool completeTier1Song)
        {
            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                List<QbFile> setlists = new List<QbFile>();
                if (guitarProgressionQb != null)
                    setlists.Add(guitarProgressionQb);
                if (guitarCoOpQb != null)
                    setlists.Add(guitarCoOpQb);

                foreach (QbFile qbf in setlists)
                {

                    QbKey qk;
                    if (completeTier1Song)
                        unlockXSongsToProgress(qbf, 1, 1, 1, 1, 0);

                    if (unlockAllTiers)
                    {
                        //display the songs in the tiers
                        List<QbItemStruct> tiers = new List<QbItemStruct>();
                        int t;
                        int i = 1;
                        do
                        {
                            t = tiers.Count;
                            qk = QbKey.Create(string.Format("tier{0}", (i++).ToString()));
                            qbf.FindItem(true, delegate(QbItemBase qib)
                                {
                                    if (qib.QbItemType == QbItemType.StructItemStruct && qk.Crc == qib.ItemQbKey.Crc)
                                        tiers.Add((QbItemStruct)qib);
                                    return false; //find for all setlists
                                });
                        } while (t != tiers.Count);


                        //we do not have ALL tiers for all careers
                        QbItemInteger defaultUnlocked;
                        QbItemArray songs;
                        QbKey duQk = QbKey.Create("defaultunlocked");
                        QbKey sQk = QbKey.Create("songs");
                        foreach (QbItemStruct tier in tiers)
                        {
                            //if no defaultUnlocked item then create one, set it to 20 (won't have mote than 100 songs in a tier
                            defaultUnlocked = tier.FindItem(duQk, false) as QbItemInteger;
                            songs = tier.FindItem(sQk, false) as QbItemArray;

                            if (defaultUnlocked == null)
                            {
                                defaultUnlocked = new QbItemInteger(tier.Root);
                                defaultUnlocked.Create(QbItemType.StructItemInteger);
                                defaultUnlocked.ItemQbKey = duQk.Clone();
                                tier.AddItem(defaultUnlocked);
                            }
                            defaultUnlocked.Values = new uint[] { (uint)((QbItemQbKey)songs.Items[0]).Values.Length };
                        }
                    }
                    qbf.AlignPointers();
                    qbf.IsValid();
                }
            }

        }

        public void AddBonusSongsFromNonCareer(QbFile storeDataQb, QbFile songListQb, QbFile guitarProgressionQb)
        {
            List<QbKey> nc = _nonCareerSongs;

            if (nc.Count == 0)
                nc = _specialSongs;

            List<QbKey> c = getAllCareerSongs(guitarProgressionQb);

            foreach (QbKey qk in nc)
            {
                if (c.Contains(qk))
                    continue;

                //add the existing song to the shop
                this.BonusSongAddToGame(storeDataQb, songListQb, qk.Clone(), false);

                //force item to leaderboard
                QbItemStruct qis = songListQb.FindItem(qk, true) as QbItemStruct;
                if (qis != null)
                {
                    QbItemInteger qii = qis.FindItem(QbKey.Create("leaderboard"), false) as QbItemInteger;
                    if (qii != null)
                        qii.Values[0] = 1; //mark as leaderboard
                }

            }
        }


        /// <summary>
        /// Add new songs to GH3, this method requires that the partition 2 files are on the local disc
        /// </summary>
        public void AddSongs(DirectoryInfo rootPartitionPath, int adjustBy, string sourceSong, bool addNonCareerSongs, DirectoryInfo workingPath)
        {
            PakFormat pf = new PakFormat(string.Format(@"{0}\pak\{1}", rootPartitionPath.FullName, _project.FileManager.PakFormat.PakFilename), string.Empty, string.Empty, PakFormatType.Wii, false);
            PakEditor pe = new PakEditor(pf, false);

            string songlistQb = @"scripts\guitar\songlist.qb.ngc";
            string storeDataQb = @"scripts\guitar\store_data.qb.ngc";
            string guitProgQb = @"scripts\guitar\guitar_progression.qb.ngc";

            if (_project.GameInfo.Game == Game.GH3_Wii)
            {
                songlistQb = songlistQb.Substring(1);  //remove first char
                storeDataQb = storeDataQb.Substring(1);  //remove first char
                guitProgQb = guitProgQb.Substring(1);  //remove first char
            }


            if (_project.GameInfo.Game == Game.GH3_Wii || _project.GameInfo.Game == Game.GHA_Wii)
            {
                //add song to song list
                QbFile qb = pe.ReadQbFile(songlistQb);
                QbFile qbStore = pe.ReadQbFile(storeDataQb);
                QbFile qbGuitProg = pe.ReadQbFile(guitProgQb);

                string silentXbox = createBlankSourceAudio(workingPath);

                QbItemArray songsList = qb.FindItem(QbKey.Create("gh3_songlist"), false) as QbItemArray;

                List<QbKey> careerSongs = getAllCareerSongs(qbGuitProg);
                List<QbKey> bonusSongs = getAllBonusSongs(qbStore);
                List<QbKey> allSongs = new List<QbKey>(careerSongs);
                allSongs.AddRange(bonusSongs);

                //remove the boss battles. If we pick new files from the career then they will be removed anyway
                //QbKey[] bossSongs = new QbKey[] { QbKey.Create("bosstom"), QbKey.Create("bossslash"), QbKey.Create("bossdevil"), QbKey.Create("bossjoe") };
                //foreach (QbKey qk in bossSongs)
                //    careerSongs.Remove(qk);


                //insert the special songs on the end (that aren't in the career)
                List<QbKey> specialSongs = new List<QbKey>();
                foreach (QbKey k in _specialSongs)
                {
                    if (!careerSongs.Contains(k))
                        specialSongs.Add(k);
                }


                if (adjustBy > 0)
                {
                    //Calculate extras
                    int add = adjustBy + (addNonCareerSongs ? _nonCareerSongs.Count : 0); 

                    foreach (QbKey k in specialSongs)
                    {
                        if (!bonusSongs.Contains(k))
                        {
                            if (_project.GameInfo.Game == Game.GH3_Wii)
                                add--; //if bonus songs currently doesn't contian this special track then add one less song
                            else
                                add++; //GHA k&qcred will be added from not existing
                        }
                    }
                    add = Math.Max(0, add);

                    List<QbKey> newBonusList = new List<QbKey>();

                    //add the special songs on the end - DO THIS FIRST
                    foreach (QbKey k in specialSongs)
                    {
                        if (!careerSongs.Contains(k))
                            newBonusList.Add(k);
                    }

                    int specialAdded = newBonusList.Count;

                    //find the last _restoresong (a regular-song list in the correct order) in the career
                    int start = -1;
                    for (int i = careerSongs.Count - 1; i >= 0; i--)
                    {
                        start = _restoreSongs.IndexOf(careerSongs[i]);
                        if (start >= 0)
                            break;
                    }

                    if (start != -1)
                    {
                        start++; //move to the next item

                        for (int i = start; i < _restoreSongs.Count; i++)
                        {
                            if (newBonusList.Count >= add + bonusSongs.Count) //add + existing bonussongs = correct count
                                break;

                            if (!careerSongs.Contains(_restoreSongs[i]))
                                newBonusList.Insert(newBonusList.Count - specialAdded, _restoreSongs[i]);
                        }
                    }

                    //nonCareer songs (only on GH3)
                    if (addNonCareerSongs)
                    {
                        for (int i = 0; i < _nonCareerSongs.Count; i++)
                        {
                            if (newBonusList.Count >= add + bonusSongs.Count)
                                break;

                            if (!careerSongs.Contains(_nonCareerSongs[i]))
                                newBonusList.Insert(newBonusList.Count - specialAdded, _nonCareerSongs[i]);
                        }
                    }

                    //add ghost songs
                    int num = 1;
                    while (newBonusList.Count < add + bonusSongs.Count)
                    {
                        QbKey qk = QbKey.Create(string.Format("theghost{0}", (num++).ToString().PadLeft(3, '0')));

                        if (!careerSongs.Contains(qk))
                            newBonusList.Insert(newBonusList.Count - specialAdded, qk);
                    }



                    //remove any bonus songs not in our list
                    foreach (QbKey k in bonusSongs)
                    {
                        if (!newBonusList.Contains(k))
                        {
                            if (!careerSongs.Contains(k))
                                this.BonusSongRemoveFromGame(qbStore, qb, k); //remove from set list also
                            else
                                removeBonusSong(qbStore, k);
                        }
                    }

                    //add all the missing items to the bonus lists
                    foreach (QbKey k in newBonusList)
                    {
                        if (!bonusSongs.Contains(k))
                        {
                            this.BonusSongAddToGame(qbStore, qb, k.Clone(), false);
                            addBonusSongNotes(rootPartitionPath, QbKey.Create(sourceSong), k.Clone(), pe);
                            createBonusSongAudio(rootPartitionPath, silentXbox, k);
                        }
                    }

                    //set the correct order in the store list
                    //find bonus song list
                    QbItemArray qbBonusSongs = qbStore.FindItem(QbKey.Create("songs"), true) as QbItemArray;

                    //add bonus song to list
                    if (qbBonusSongs != null)
                        (qbBonusSongs.Items[0] as QbItemQbKey).Values = newBonusList.ToArray();


                    qbStore.AlignPointers();
                    qbStore.IsValid();
                    qb.AlignPointers();
                    qb.IsValid();




                }
                else if (adjustBy < 0)
                {
                    if (addNonCareerSongs) //adds kings and queens credits if required
                        this.AddBonusSongsFromNonCareer(qbStore, qb, qbGuitProg);

                    bonusSongs = getAllBonusSongs(qbStore);

                    //remove bonus songs that aren't required. keep any required
                    int offset = 0;
                    while (adjustBy != 0 && bonusSongs.Count > 0 && bonusSongs.Count - offset > 0)
                    {
                        int idx = (bonusSongs.Count - offset) - 1;

                        if (bonusSongs[idx].Crc == QbKey.Create("kingsandqueenscredits").Crc
                         || bonusSongs[idx].Crc == QbKey.Create("kingsandqueens").Crc
                         || bonusSongs[idx].Crc == QbKey.Create("thrufireandflames").Crc)
                        {
                            offset++;
                            continue;
                        }

                        if (!careerSongs.Contains(bonusSongs[idx]))
                            this.BonusSongRemoveFromGame(qbStore, qb, bonusSongs[idx]); //remove from set list also
                        else
                            removeBonusSong(qbStore, bonusSongs[idx]);
                        bonusSongs.RemoveAt(idx);
                        adjustBy++;
                    }
                }
                else if (adjustBy == 0 && addNonCareerSongs)
                {
                    this.AddBonusSongsFromNonCareer(qbStore, qb, qbGuitProg);
                }

                qb.AlignPointers();
                qb.IsValid();
                qbStore.AlignPointers();
                qbStore.IsValid();

                FileHelper.Delete(silentXbox);
                pe.ReplaceFile(songlistQb, qb);
                pe.ReplaceFile(storeDataQb, qbStore);
            }

        }

        /// <summary>
        /// Remove song to bonus list and shop
        /// </summary>
        /// <param name="songQk"></param>
        public void RemoveBonusSongQbItems(QbKey songQk, QbFile qbStore)
        {
            QbItemBase item;

            //find bonus song list
            QbItemArray bonusSongs = qbStore.FindItem(QbKey.Create("songs"), true) as QbItemArray;

            //find bonus song price list
            QbItemStruct songData = qbStore.FindItem(QbKey.Create("store_song_data"), true) as QbItemStruct;

            //find bonus song info
            QbItemArray bonusInfo = qbStore.FindItem(QbKey.Create("Bonus_Songs_Info"), true) as QbItemArray;



            //remove bonus song from list
            if (bonusSongs != null)
            {
                List<QbKey> songs = new List<QbKey>((bonusSongs.Items[0] as QbItemQbKey).Values);
                songs.Remove(songQk);
                (bonusSongs.Items[0] as QbItemQbKey).Values = songs.ToArray();
            }


            //remove bonus song price
            if (songData != null)
            {
                item = songData.FindItem(false, delegate(QbItemBase qib)
                    {
                        return (qib.ItemQbKey.Crc == songQk.Crc);
                    });
                if (item != null)
                    songData.RemoveItem(item);
            }

            //remove bonus info
            if (bonusInfo != null && bonusInfo.Items.Count > 0)
            {
                QbKey itemQk = QbKey.Create("item");
                QbItemStructArray infoArray = (bonusInfo.Items[0] as QbItemStructArray);
                item = infoArray.FindItem(false, delegate(QbItemBase qib)
                    {
                        QbItemQbKey iqk = (QbItemQbKey)(qib.Items[0]);
                        return iqk.ItemQbKey.Crc == itemQk.Crc && iqk.Values[0] == songQk.Crc;
                    });
                if (item != null)
                    infoArray.RemoveItem(item);
            }

            qbStore.AlignPointers();
            qbStore.IsValid();
        }

        /// <summary>
        /// Add song to bonus list and shop
        /// </summary>
        /// <param name="songQk"></param>
        public void AddBonusSongQbItems(QbKey songQk, QbFile qbStore, bool insertAdd)
        {
            QbKey qkCreditsSong = _project.GameInfo.Game == Game.GH3_Wii ? QbKey.Create("thrufireandflames") : QbKey.Create("kingsandqueenscredits");

            //find bonus song list
            QbItemArray bonusSongs = qbStore.FindItem(QbKey.Create("songs"), true) as QbItemArray;

            //find bonus song price list
            QbItemStruct songData = qbStore.FindItem(QbKey.Create("store_song_data"), true) as QbItemStruct;

            //find bonus song info
            QbItemArray bonusInfo = qbStore.FindItem(QbKey.Create("Bonus_Songs_Info"), true) as QbItemArray;


            //add bonus song to list
            if (bonusSongs != null)
            {
                List<QbKey> songs = new List<QbKey>((bonusSongs.Items[0] as QbItemQbKey).Values);
                if (!songs.Contains(songQk))
                {
                    if (insertAdd)
                        songs.Insert(0, songQk.Clone());
                    else
                    {
                        //insert second from the end (before TTFAF / kingandqueenscredits)
                        if (songs.Count == 0 || songs[songs.Count - 1].Crc != qkCreditsSong.Crc)
                            songs.Add(songQk.Clone()); //clone qbkey
                        else
                            songs.Insert(songs.Count - 1, songQk.Clone()); //clone qbkey
                    }
                    (bonusSongs.Items[0] as QbItemQbKey).Values = songs.ToArray();
                }
            }


            if (songData != null)
            {
                //add bonus song price
                if (null == songData.FindItem(false, delegate(QbItemBase qib)
                    {
                        return (qib.ItemQbKey.Crc == songQk.Crc);
                    }))
                {
                    QbItemStruct songDataItem = new QbItemStruct(qbStore);
                    songDataItem.Create(QbItemType.StructItemStruct);
                    songDataItem.ItemQbKey = songQk.Clone();
                    QbItemInteger songDataItemPrice = new QbItemInteger(qbStore);
                    songDataItemPrice.Create(QbItemType.StructItemInteger);
                    songDataItemPrice.ItemQbKey = QbKey.Create("price");
                    songDataItemPrice.Values = new uint[] { 0 };
                    songDataItem.AddItem(songDataItemPrice);
                    if (!insertAdd || songData.Items.Count == 0)
                        songData.AddItem(songDataItem);
                    else
                        songData.InsertItem(songDataItem, songData.Items[0], true);
                }
            }


            if (bonusInfo != null)
            {
                //add bonus info
                QbKey itemQk = QbKey.Create("item");
                QbItemStructArray infoArray = (bonusInfo.Items[0] as QbItemStructArray);
                if (null == infoArray.FindItem(false, delegate(QbItemBase qib)
                    {
                        QbItemQbKey iqk = (QbItemQbKey)(qib.Items[0]);
                        return iqk.ItemQbKey.Crc == itemQk.Crc && iqk.Values[0] == songQk.Crc;
                    }))
                {
                    QbItemStruct infoStruct = new QbItemStruct(qbStore);
                    infoStruct.Create(QbItemType.StructHeader);
                    if (!insertAdd || infoArray.Items.Count == 0)
                        infoArray.AddItem(infoStruct);
                    else
                        infoArray.InsertItem(infoStruct, infoArray.Items[0], true);
                    qbStore.AlignPointers();
                    qbStore.IsValid();
                    QbItemQbKey infoItem = new QbItemQbKey(qbStore);
                    infoItem.Create(QbItemType.StructItemQbKey);
                    infoItem.ItemQbKey = itemQk; //"item"
                    infoItem.Values = new QbKey[] { songQk.Clone() };
                    infoStruct.AddItem(infoItem);
                    qbStore.AlignPointers();
                    qbStore.IsValid();
                    QbItemString infoText = new QbItemString(qbStore);
                    infoText.Create(QbItemType.StructItemString);
                    infoText.ItemQbKey = QbKey.Create("text");
                    infoText.Strings = new string[] { "Bonus song added with TheGHOST" };
                    infoStruct.AddItem(infoText);
                    qbStore.AlignPointers();
                    qbStore.IsValid();
                    QbItemQbKey infoCover = new QbItemQbKey(qbStore);
                    infoCover.Create(QbItemType.StructItemQbKey);
                    infoCover.ItemQbKey = QbKey.Create("album_cover");
                    infoCover.Values = new QbKey[] { QbKey.Create("store_song_default") };
                    infoStruct.AddItem(infoCover);
                    qbStore.AlignPointers();
                    qbStore.IsValid();
                }
            }
        }

        private string createBlankSourceAudio(DirectoryInfo rootPath)
        {
            string silentWav = string.Format(@"{0}\silent.wav", rootPath.FullName.TrimEnd('\\'));
            string silentEnc = string.Format(@"{0}\silent.xbox", rootPath.FullName.TrimEnd('\\'));

            if (File.Exists(silentWav))
            {
                try
                {
                    FileHelper.Delete(silentWav);
                }
                catch { }
                if (File.Exists(silentWav))
                {
                    try
                    {
                        //HACK!!  Appears to force the file lock to be released and file gets auto deleted!!
                        _project.AudioExport.Convert(silentWav, silentEnc, true, 32000);
                    }
                    catch
                    {
                        FileHelper.Delete(silentWav);
                    }
                }
            }

            WavProcessor.CreateSilentWav(500, silentWav, false, 44100);
            _project.AudioExport.Convert(silentWav, silentEnc, true, 32000);

            FileHelper.Delete(silentWav);

            if (File.Exists(silentWav))
                System.Diagnostics.Debug.WriteLine("Delete Failure: " + DateTime.Now.ToString("s"));


            return silentEnc;
        }

        private void createBonusSongAudio(DirectoryInfo rootPath, string xboxWav, QbKey dest)
        {
            string dat = string.Format(@"{0}\music\{1}.dat.ngc", rootPath.FullName.TrimEnd('\\'), dest.Text);
            string wad = string.Format(@"{0}\music\{1}.wad.ngc", rootPath.FullName.TrimEnd('\\'), dest.Text);

            if (!File.Exists(dat) || !File.Exists(wad))
                DatWad.CreateDatWad(dest, _project.FileManager.PakFormat.EndianType, dat, wad, xboxWav, xboxWav, xboxWav, xboxWav);
        }

        private void addBonusSongNotes(DirectoryInfo rootPath, QbKey source, QbKey dest, PakEditor qbPak)
        {

            string srcFolder = string.Format(@"{0}\songs", rootPath.FullName.TrimEnd('\\'));
            string srcMask = string.Format(@"{0}*.pak.ngc", source.Text);

            string[] srcFi = Directory.GetFiles(srcFolder, srcMask, SearchOption.TopDirectoryOnly);

            if (_project.GameInfo.Game == Game.GHA_Wii)
            {
                //GHA holds the section text in the main qb.pak file
                string destNotes = string.Format(@"songs\{0}.mid_text.qb.ngc", dest.Text);
                if (!qbPak.Headers.ContainsKey(destNotes.ToLower()))
                {
                    QbFile qbNotes = qbPak.ReadQbFile(string.Format(@"songs\{0}.mid_text.qb.ngc", source.Text));
                    //copy notes section qb file to new file in qb.pak.ngc
                    qbPak.AddFile(qbNotes, destNotes, QbKey.Create(".qb"), true);
                }
            }

            foreach (string src in srcFi)
            {
                FileInfo srcF = new FileInfo(src);
                string dst = string.Format(@"{0}\{1}{2}", srcF.Directory.FullName.TrimEnd('\\'), dest.Text, srcF.Name.Substring(source.Text.Length));

                //skip if exists
                if (File.Exists(dst) || !Regex.IsMatch(src, string.Format(@"{0}(|_.)\.pak\.ngc$", source.Text))) //only allow "" or _<fgis>
                    continue;

                File.Copy(src, dst, true);

                PakFormat pf = new PakFormat(dst, string.Empty, string.Empty, PakFormatType.Wii, false);
                PakEditor pe = new PakEditor(pf, false);
                string qbDst;
                QbFile qb;
                Dictionary<uint, string> srcQk;
                string dstS;
                foreach (string qbSrc in pe.QbFilenames)
                {
                    qbDst = qbSrc.ToLower().Replace(source.Text.ToLower(), dest.Text.ToLower());
                    pe.RenameFile(qbSrc, qbDst, QbKey.Create(".mqb"));


                    if (!qbSrc.Contains(".mid_text."))
                    {
                        //map the section QbKeys
                        qb = pe.ReadQbFile(qbDst);

                        srcQk = getMidItems(source.Text);
                        foreach (QbItemBase qib in qb.Items)
                        {
                            if (qib.ItemQbKey != null)
                            {
                                if (srcQk.ContainsKey(qib.ItemQbKey.Crc))
                                {
                                    dstS = string.Format("{0}{1}", dest.Text, srcQk[qib.ItemQbKey.Crc].Substring(source.Text.Length));
                                    qib.ItemQbKey = QbKey.Create(dstS);
                                }
                                //else
                                //    throw new ApplicationException("Item QBKey not recognised");
                            }
                        }
                        pe.ReplaceFile(qbDst, qb);
                    }

                }
            }
        }

        private Dictionary<uint, string> getMidItems(string songName)
        {
            Dictionary<uint, string> m = new Dictionary<uint, string>();
            List<string> s = new List<string>(new string[] {
                "{0}_FaceOffP1", 
                "{0}_FaceOffP2", 
                "{0}_BossBattleP1", 
                "{0}_BossBattleP2", 
                "{0}_TimeSig", 
                "{0}_FretBars", 
                "{0}_Markers", 
                "{0}_Scripts_Notes", 
                "{0}_Anim_Notes", 
                "{0}_Triggers_Notes", 
                "{0}_Cameras_Notes", 
                "{0}_Lightshow_Notes", 
                "{0}_Crowd_Notes", 
                "{0}_Drums_Notes", 
                "{0}_Performance_Notes", 
                "{0}_Scripts", 
                "{0}_Anim", 
                "{0}_Triggers", 
                "{0}_Cameras", 
                "{0}_Lightshow", 
                "{0}_Crowd", 
                "{0}_Drums", 
                "{0}_Performance",
                "{0}_Song_Startup" });

            List<string> d = new List<string>(new string[] {
                "{{0}}_Song_{0}", 
                "{{0}}_{0}_Star", 
                "{{0}}_{0}_StarBattleMode", 
                "{{0}}_Song_Rhythm_{0}", 
                "{{0}}_Rhythm_{0}_Star", 
                "{{0}}_Rhythm_{0}_StarBattleMode", 
                "{{0}}_Song_GuitarCoop_{0}", 
                "{{0}}_GuitarCoop_{0}_Star", 
                "{{0}}_GuitarCoop_{0}_StarBattleMode", 
                "{{0}}_Song_RhythmCoop_{0}", 
                "{{0}}_RhythmCoop_{0}_Star", 
                "{{0}}_RhythmCoop_{0}_StarBattleMode" });

            foreach (string a in d)
            {
                foreach (string b in new string[] { "Easy", "Medium", "Hard", "Expert" })
                    s.Insert(0, string.Format(a, b));
            }

            string x;
            foreach (string a in s)
            {
                x = string.Format(a, songName);
                m.Add(QbKey.Create(x).Crc, x);
            }

            return m;
        }

        private Project _project;
        private List<QbKey> _restoreSongs;
        private List<QbKey> _nonCareerSongs;
        private List<QbKey> _specialSongs;
    }
}
