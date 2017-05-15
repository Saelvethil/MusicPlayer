using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMPLib;

namespace MusicPlayerCore.Player.WMPlayer
{

    class WMPSong : ISong
    {
        private IWMPMedia song;
        private double _position = 0;

        public WMPSong(string URL)
        {

            try
            {
                song = ((WMPlayer)Player).NewMedia(URL);
            }
            catch (System.Runtime.InteropServices.COMException COMex) { }
        }

        public string URL
        {
            get { return song.sourceURL; }
        }

        public IPlayer Player
        {
            get { return WMPlayer.GetInstance(); }
        }

        public string ArtistName
        {
            get {
                string author = song.getItemInfo("Author"); 
                return author.Length == 0 ? "unknown" : author; 
            }
        }

        public string SongName
        {
            get {
                string songname = song.getItemInfo("Title"); 
                return songname.Length == 0 ? "unknown" : songname; 
            }
        }

        public string AlbumName
        {
            get {
                string albumname = song.getItemInfo("WM/AlbumTitle"); 
                return albumname.Length == 0 ? "unknown" : albumname; 
            }
        }

        public double Duration
        {
            get { return song.duration; }//w sekundach
        }

        public string Year
        {
            get {

                string year = song.getItemInfo("WM/Year"); 
                return year.Length == 0 ? "unknown" : year; 
            }
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
