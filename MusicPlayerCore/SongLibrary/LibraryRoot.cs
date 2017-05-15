using MusicPlayerCore.Core;
using MusicPlayerCore.Player;
using System;
using System.Collections.Generic;

namespace MusicPlayerCore.SongLibrary
{
    public class LibraryRoot : ILibraryComponent
    {
        private List<LibraryArtist> childs;


        public LibraryRoot()
        {
            childs = new List<LibraryArtist>();
        }

        public void FillSongs(SongList list)
        {
            Console.WriteLine("root");
            foreach (var child in childs)
            {
                Console.WriteLine("artistName "+child.ArtistName);

                child.FillSongs(list);
            }
        }

        public void AddSong(ISong song)
        {
            LibraryArtist artist = null;
            foreach (var child in childs)
            {
                if (child.ArtistName.Equals(song.ArtistName))
                {
                    artist = child;
                    break;
                }
            }

            if (artist == null)
            {
                artist = new LibraryArtist(song.ArtistName);
                childs.Add(artist);
            }
            artist.AddSong(song);

        }


        public IEnumerable<ILibraryComponent> GetChildren()
        {
            return childs;
        }
    }
}
