using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    internal delegate void AudioFileChanged(AudioFileList sender, AudioFileChangeType changeType, AudioFile from, AudioFile to, int index);
    internal enum AudioFileChangeType { Added, Removed, Changed }

    public class AudioFileList : List<AudioFile>
    {
        internal AudioFileList(AudioFileChanged callback, Project project) : base()
        {
            _callback = callback;
            _project = project;
        }

        internal AudioFileList(AudioFileChanged callback, int capacity, Project project) : base(capacity)
        {
            _callback = callback;
            _project = project;
        }

        public new void Add(AudioFile item)
        {
            base.Add(item);
            if (_callback != null)
                _callback(this, AudioFileChangeType.Added, null, item, base.Count - 1);
        }

        public new void Insert(int index, AudioFile item)
        {
            base.Insert(index, item);
            if (_callback != null)
                _callback(this, AudioFileChangeType.Added, null, item, index);
        }

        public new AudioFile this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                AudioFile v = base[index];
                base[index] = value;
                if (_callback != null)
                    _callback(this, AudioFileChangeType.Changed, v, base[index], index);
            }
        }

        public new void Remove(AudioFile item)
        {
            int index = 0;
            foreach (AudioFile s in this)
            {
                if (object.ReferenceEquals(s, item))
                    break;
                index++;
            }

            base.Remove(item);

            if (_callback != null)
                _callback(this, AudioFileChangeType.Removed, item, null, index);
        }

        public new void RemoveAt(int index)
        {
            AudioFile v = base[index];

            base.RemoveAt(index);

            if (_callback != null)
                _callback(this, AudioFileChangeType.Removed, v, null, index);
        }

        #region NotImplemented Stuff
        public AudioFileList(IEnumerable<AudioFile> collection)
            : base(collection)
        {
            throw new NotImplementedException();
        }

        public new void AddRange(IEnumerable<AudioFile> collection)
        {
            throw new NotImplementedException();
        }

        public new int RemoveAll(Predicate<AudioFile> match)
        {
            throw new NotImplementedException();
        }

        public new void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }
        #endregion

        private AudioFileChanged _callback;
        private Project _project;
    }
}
