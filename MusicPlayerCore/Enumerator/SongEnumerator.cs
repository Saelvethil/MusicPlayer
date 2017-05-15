using MusicPlayerCore.Player;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerCore.Enumerator
{
    public abstract class SongEnumerator : IEnumerator<ISong>
    {
        protected List<ISong> list;
        protected int pos = 0;
        public SongEnumerator(List<ISong> list)
        {
            this.list = list;
        }
        public ISong Current
        {
            get
            {
                try
                {
                    return list.ElementAt(pos);
                }
                catch
                {
                    return null;
                }

            }
        }

        public void Dispose()
        {

        }

        object System.Collections.IEnumerator.Current
        {
            get { return list.ElementAt(pos); }
        }

        abstract public bool MoveNext();

        abstract public bool MovePrevious();

        public void Reset()
        {
            pos = 0;
        }
    }
}
