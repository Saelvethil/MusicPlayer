using MusicPlayerCore.Core;
using MusicPlayerCore.Player;
using System;
using System.Collections.Generic;

namespace MusicPlayerCore.SongLibrary
{
    public class LibrarySong : ILibraryComponent
    {
        public ISong Song { get; private set; }
        public string URL { get; private set; }
        public string SongName { get; private set; }
        public double Duration { get; private set; }


        public LibrarySong(ISong song)
        {
            Song = song;
            URL = song.URL;
            SongName = song.SongName;
            Duration = song.Duration;
        }

        public void FillSongs(SongList list)
        {
            list.Add(Song);
        }

        public void AddSong(ISong song)
        {
            
        }

        public IEnumerable<ILibraryComponent> GetChildren()
        {
            throw new NotSupportedException();
        }
    }
}
