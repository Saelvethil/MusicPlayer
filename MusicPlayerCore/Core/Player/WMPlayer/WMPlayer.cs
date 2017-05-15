using System;
using System.Threading;
using WMPLib;

namespace MusicPlayerCore.Player.WMPlayer
{
    class WMPlayer : IPlayer
    {
        private WindowsMediaPlayer player = new WindowsMediaPlayer();
        private static WMPlayer wmplayer = new WMPlayer();
        private WMPSong song;
        Thread t;
        bool paused = false;

        private WMPlayer()
        {

        }

        public static WMPlayer GetInstance()
        {

            return wmplayer;
        }

        public void Play(ISong song)
        {
            paused = false;
            this.song = (WMPSong)song;
           
                player.URL = song.URL;
                player.controls.currentPosition = song.Position;
                player.controls.play();



                t = new Thread(() =>
                {
                    try
                    {
                        while (player.playState != WMPPlayState.wmppsStopped && !paused)
                        {
                            Thread.Sleep(400);
                        }
                    }
                    catch (System.Runtime.InteropServices.COMException COMex) { }


                    if (!paused)
                    {
                        SongFinished(this, EventArgs.Empty);
                    }
                });
                t.Start();
            
        }

        public void Stop()
        {
           
                paused = true;
                player.settings.volume = 100;
                player.controls.stop();
                if (song != null)
                    song.Position = 0;
                SongFinished = delegate { };
            
        }

        public void Pause()
        {
           
                paused = true;
                player.controls.pause();
                song.Position = player.controls.currentPosition;
                SongFinished = delegate { };
          
        }


        public bool isPlaying()
        {
            return player.playState == WMPPlayState.wmppsPlaying;
        }

        public double Position //w sekundach
        {
            get
            {
                return player.controls.currentPosition;
            }
            set
            {
                player.controls.currentPosition = value;
            }
        }

        public int Volume  //w skali do 100
        {
            get
            {
                return player.settings.volume;
            }
            set
            {
                player.settings.volume = value;
            }
        }

        public IWMPMedia NewMedia(string URL)
        {
            return player.newMedia(URL);
        }


        public event EventHandler SongFinished = delegate { };



    }
}
