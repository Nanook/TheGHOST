using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class TierQb
    {
        internal TierQb(QbItemStruct tierQi)
        {
            _title = (QbItemString)tierQi.FindItem(QbKey.Create("title"), false);
            _songs = (QbItemArray)tierQi.FindItem(QbKey.Create("songs"), false);
        }

        public string Name
        {
            get { return _title.Strings[0]; }
            internal set { _title.Strings[0] = value; }
        }

        public QbKey[] Songs
        {
            get { return ((QbItemQbKey)_songs.Items[0]).Values; }
            set { ((QbItemQbKey)_songs.Items[0]).Values = value; }
        }

        private QbItemString _title;
        private QbItemArray _songs;
    }


}
