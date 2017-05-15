using MusicPlayerCore.Player;
using System.Collections.Generic;

namespace MusicPlayerCore.Enumerator
{
    public class DefaultSongEnumerator : SongEnumerator
    {
        public DefaultSongEnumerator(List<ISong> list)
            : base(list)
        {
        }
        public override bool MoveNext()
        {
            pos++;
            pos = pos >= list.Count ? 0 : pos;
            return true;
        }

        public override bool MovePrevious()
        {
            pos--;
            pos = pos < 0 ? list.Count-1 : pos;
            return true;
        }
    }
}
