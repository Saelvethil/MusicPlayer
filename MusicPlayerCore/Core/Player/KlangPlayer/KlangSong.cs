using IrrKlang;
using System;
using System.IO;

namespace MusicPlayerCore.Player.KlangPlayer
{
    class KlangSong : ISong
    {
        private ISoundSource song;
        private double _position;

        public KlangSong(String URL)
        {
            song = ((KlangPlayer)Player).GetSoundSource(URL);
        }

        public string URL
        {
            get { return song.Name; }
        }

        public IPlayer Player
        {
            get { return KlangPlayer.GetInstance(); }
        }

        public string ArtistName
        {
            get { return "unknown"; }
        }

        public string SongName  //koncowka sciezki tzn nazwa pliku bez rozszerzenia
        {
            get {

                return Path.GetFileNameWithoutExtension(song.Name); 
            }
        }

        public string AlbumName
        {
            get { return "unknown"; }
        }

        public double Duration
        {
            get { return song.PlayLength / 1000; }
        }

        public string Year
        {
            get { return "unknown"; }
        }


        public double Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
    }
}
