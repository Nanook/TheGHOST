using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class AudioFile
    {
        internal AudioFile(string filename, ISettingsChange change) : this(filename, 100, change)
        {
        }

        internal AudioFile(string filename, int volume, ISettingsChange change)
        {
            this.Name = filename;
            this.Volume = volume;
            _change = change;
        }

        public string Name
        {
            get { return _name; }
            internal set //only allow interl set as we don't capture the change externally
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ApplicationException("AudioFile.Name cannot be null or empty string");

                if (_name == value)
                    return;
                _name = value;
                if (_change != null)
                    _change.LastChanged = DateTime.Now;
            }
        }

        public int Volume
        {
            get { return _volume; }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;
                if (_change != null)
                    _change.LastChanged = DateTime.Now;
            }
        }

        /// <summary>
        /// Updates the name without setting the LastChange value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal void UpdateName(string name)
        {
            _name = name;
        }

        private ISettingsChange _change;
        private string _name;
        private int _volume;
    }
}
