using MusicPlayerCore.Player;
using System;
using System.Collections.Generic;

namespace MusicPlayerCore.Enumerator
{
    public class RandomSongEnumerator : SongEnumerator
    {
        Random rnd = new Random();
        public RandomSongEnumerator(List<ISong> list)
            : base(list)
        {
        }

        public override bool MoveNext()
        {
            pos = rnd.Next(list.Count);
            return true;
        }

        public override bool MovePrevious()
        {
            pos = rnd.Next(list.Count);
            return true;
        }
    }
}
