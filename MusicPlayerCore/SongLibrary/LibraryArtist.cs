using MusicPlayerCore.Core;
using MusicPlayerCore.Player;
using System;
using System.Collections.Generic;

namespace MusicPlayerCore.SongLibrary
{
    public class LibraryArtist : ILibraryComponent
    {
        private List<LibraryAlbum> childs;
        public string ArtistName { get; private set; }

        public LibraryArtist(string artistName)
        {
            childs = new List<LibraryAlbum>();
            ArtistName = artistName;
        }

        public void FillSongs(SongList list)
        {
            foreach (var child in childs)
            {
                Console.WriteLine("\talbumName "+child.AlbumName);
                child.FillSongs(list);
            }
        }

        public void AddSong(ISong song)
        {
            LibraryAlbum album = null;
            foreach (var child in childs)
            {
                if (child.AlbumName.Equals(song.AlbumName) && child.Year.Equals(song.Year))
                {
                    album = child;
                    break;
                }
            }

            if (album == null)
            {
                album = new LibraryAlbum(song.AlbumName, song.Year);
                childs.Add(album);
            }
            album.AddSong(song);
        }


        public IEnumerable<ILibraryComponent> GetChildren()
        {
            return childs;
        }
    }
}
