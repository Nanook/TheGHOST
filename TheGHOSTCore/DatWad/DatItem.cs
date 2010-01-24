using System;
using System.Collections.Generic;
using System.Text;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public class DatItem
    {
        internal DatItem(QbKey itemQbKey, uint fileOffset, uint fileSize, byte[] reserved)
        {
            _itemQkey = itemQbKey;
            _fileOffset = fileOffset;
            _fileSize = fileSize;
            _reserved = reserved;
        }

        public QbKey ItemQbKey
        {
            get { return _itemQkey; }
            set { _itemQkey = value; }
        }

        public uint FileOffset
        {
            get { return _fileOffset; }
            set { _fileOffset = value; }
        }

        public uint FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        public byte[] Reserved
        {
            get { return _reserved; }
            set { _reserved = value; }
        }

        private QbKey _itemQkey;  //songname_guitar etc
        private uint _fileOffset;
        private uint _fileSize;
        private byte[] _reserved;
    }
}
