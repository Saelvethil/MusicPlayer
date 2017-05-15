using MusicPlayerCore.Player;
using MusicPlayerCore.Player.KlangPlayer;
using MusicPlayerCore.Player.WMPlayer;
using System;

namespace MusicPlayerCore.Core
{
    public class SongFactory
    {
        public static ISong CreateSong(String URL)
        {
            string extension = System.IO.Path.GetExtension(URL);
            if (extension.Equals(".ogg")) return new KlangSong(URL);
            else if (extension.Equals(".mp3") || extension.Equals(".wav")) return new WMPSong(URL);
            else throw new FormatException("Extension of file isnt supported: "+URL);
        }
    }
}
