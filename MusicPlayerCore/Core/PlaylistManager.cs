using MusicPlayerCore.Builder;
using MusicPlayerCore.Player;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace MusicPlayerCore.Core
{
    public class PlaylistManager
    {
        private SongList _songList;
        public SongList Playlist
        {
            get
            {
                
                return _songList;
            }
            set
            {
                _songList = value;
            }

        }

        public void AddSongToPlaylist(ISong song)
        {
            Playlist.Add(song);
        }
        public void RemoveSongFromPlaylist(int index)
        {
            Playlist.RemoveAt(index);
        }
        public void RemoveSongFromPlaylist(ISong song)
        {
            Playlist.Remove(song);
        }
        public PlaylistManager()
        {
            if (!IsPlaylistAvailable())
            {
                _songList = SongLibrary.DefaultPlaylist;                
            }
        }

        private Boolean IsPlaylistAvailable()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "musicplayer");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }
            if (File.Exists(Path.Combine(path, "lastPlaylist.txt")))
            {
                string line = "";
                using (var stream = File.OpenText(Path.Combine(path, "lastPlaylist.txt")))                
                {
                   line = stream.ReadLine();
                }
                    if (File.Exists(line))
                    {
                        LoadPlaylistFromFile(line);
                        return true;
                    }               

            }
            return false;

        }
        private void SetLastPlaylist(string PlaylistPath)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "musicplayer");
            path = Path.Combine(path, "lastPlaylist.txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, false))///Ewentualnie @
            {
                file.WriteLine(PlaylistPath);
            }
        }

        public void SavePlaylist(string fullPath)
        {
            string extension = System.IO.Path.GetExtension(fullPath);
            IPlaylistBuilder builder;
            if (extension.Equals(".json")) builder = new JSONPlaylistBuilder();
            else builder = new XMLPlaylistBuilder();

            SongList songList = Playlist;
            builder.AddHeader(songList.Name);
            for (int i = 0; i < songList.ListSize; i++)
            {
                builder.AddSong(songList.ElementAt(i));
            }

            builder.AddFooter();

            string resource = builder.GetResult();
            SaveFile(resource, fullPath);


        }
        private void SaveFile(string playlist, string fullPath)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@fullPath, false))
            {
                file.WriteLine(playlist);
            }
            SetLastPlaylist(fullPath);


        }
        public void LoadPlaylistFromFile(string fullPath)
        {

            string extension = System.IO.Path.GetExtension(fullPath);
            if (extension.Equals(".xml")) LoadXMLFile(fullPath);
            else if (extension.Equals(".json")) LoadJSONFile(fullPath);
            else throw new FormatException("This file format is not supported");
            SetLastPlaylist(fullPath);

        }

        private void LoadXMLFile(string fullPath)
        {
            Playlist = new SongList();
            using (XmlTextReader reader = new XmlTextReader(@fullPath))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("Playlist"))
                            {
                                Playlist.Name = reader.GetAttribute("name");
                                Console.WriteLine(Playlist.Name);
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            Playlist.Add(SongFactory.CreateSong(reader.Value));
                            Console.WriteLine(reader.Value);
                            break;

                    }
                }
            }

        }

        private void LoadJSONFile(string fullPath)
        {
            Playlist = new SongList();
            string st = "";
            using (var stream = File.OpenText(fullPath))
            {
                 st = stream.ReadToEnd();
            }
            string name = st.Substring(st.IndexOf(':'), st.IndexOf(',') - st.IndexOf(':'));
            name = name.Substring(name.IndexOf("\"") + 1, name.LastIndexOf("\"") - 1 - name.IndexOf("\""));
      
            Playlist.Name = name;

            string songs = st.Substring(st.IndexOf('[') + 1, st.IndexOf(']') - st.IndexOf('[') - 1);
            string[] songArray = songs.Split(new Char[] { ',' });

            for (int i = 0; i < songArray.Length; i++)
            {
                int ind = songArray[i].IndexOf(':');
                if (ind == -1)
                {
                    return;
                }
                songArray[i] = songArray[i].Substring(ind);

                string URL = songArray[i].Substring(songArray[i].IndexOf("\"") + 1, songArray[i].LastIndexOf("\"") - songArray[i].IndexOf("\"") - 1);
                Playlist.Add(SongFactory.CreateSong(URL));
  
            }

        }
    }
}