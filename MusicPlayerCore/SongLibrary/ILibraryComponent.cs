using MusicPlayerCore.Core;
using MusicPlayerCore.Player;
using System.Collections.Generic;

namespace MusicPlayerCore.SongLibrary
{
    public interface ILibraryComponent
    {
        void FillSongs(SongList list);
        void AddSong(ISong song);
        IEnumerable<ILibraryComponent> GetChildren();
    }
}
