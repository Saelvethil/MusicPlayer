using MusicPlayerCore.SongLibrary;
using System;
using System.IO;

namespace MusicPlayerCore.Core
{
    public class SongLibrary
    {
        private String storagePath;
        public static LibraryRoot root;

        public static SongList DefaultPlaylist
        {
            get
            {
                SongList songList = new SongList();
                root.FillSongs(songList);
                return songList;
            }
        }



        public SongLibrary()
        {
            root = new LibraryRoot();


            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "musicplayer");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            storagePath = Path.Combine(path, "songslibrary.csv");

            if (File.Exists(storagePath))
            {
                LoadLibrary();
            }
        }


        private void LoadLibrary()
        {
            var stream = File.OpenText(storagePath);
            string line;

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                if (File.Exists(line))
                {
                    root.AddSong(SongFactory.CreateSong(line));
                }
            }
            stream.Close();
        }


        private void SaveLibrary()
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(storagePath, false))
            {

                SongList list = new SongList();
                root.FillSongs(list);

                for (int i = 0; i < list.ListSize; i++)
                {
                    file.WriteLine(list.Get(i).URL);
                }
            }
        }



        public void AddSong(String URL)
        {
            root.AddSong(SongFactory.CreateSong(URL));
            SaveLibrary();
        }


    }
}
