using MusicPlayerCore.Enumerator;
using MusicPlayerCore.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerCore.Core
{
    public class SongList : IEnumerable<ISong>
    {
        private string _name = "playlist";
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int ListSize
        {
            get
            {
                return list.Count();
            }
        }

        private List<ISong> list = new List<ISong>();

        public IEnumerator<ISong> GetEnumerator()
        {
            return new DefaultSongEnumerator(list);
        }
        public IEnumerator<ISong> GetRandomEnumerator()
        {
            return new RandomSongEnumerator(list);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Add(ISong song)
        {
            list.Add(song);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public ISong Get(int index)
        {
            return list.ElementAt(index);
        }

        public bool Contains(ISong song)
        {
            return list.Contains(song);
        }
        public void Remove(ISong song)
        {
            list.Remove(song);
        }
    }
}
