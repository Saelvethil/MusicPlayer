using MusicPlayerCore.Core;
using MusicPlayerCore.Player;
using System;
using System.Collections.Generic;

namespace MusicPlayerCore.SongLibrary
{
    public class LibraryAlbum : ILibraryComponent
    {
        private List<LibrarySong> childs;
        public string AlbumName { get; private set; }
        public string Year { get; private set; }

        public LibraryAlbum(string albumName, string year)
        {
            childs = new List<LibrarySong>();
            AlbumName = albumName;
            Year = year;
        }

        public void FillSongs(SongList list)
        {
            foreach (var child in childs)
            {
                Console.WriteLine("\t\tsongName " + child.SongName);
                child.FillSongs(list);
            }
        }

        public void AddSong(ISong song)
        {
            bool contains = false;

            foreach (var child in childs)
            {
                if (child.URL.Equals(song.URL))
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                childs.Add(new LibrarySong(song));
            }
        }

        
        public IEnumerable<ILibraryComponent> GetChildren()
        {
            return childs;
        }
    }
}
