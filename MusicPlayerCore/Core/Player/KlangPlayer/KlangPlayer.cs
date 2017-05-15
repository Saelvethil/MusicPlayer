using IrrKlang;
using System;
using System.Threading;

namespace MusicPlayerCore.Player.KlangPlayer
{
    class KlangPlayer : IPlayer
    {
        private ISound klangsong;
        private ISong song;
        private ISoundEngine player = new ISoundEngine();
        private static KlangPlayer klangplayer = new KlangPlayer();
        private Thread t;
        private bool paused = false;


        private KlangPlayer() { }

        public static KlangPlayer GetInstance()
        {
            return klangplayer;
        }

        public void Play(ISong song)
        {

            paused = false;
            this.song = song;
            klangsong = player.Play2D(song.URL);
            klangsong.PlayPosition = (uint)(song.Position);


            t = new Thread(() =>
            {
                while (!klangsong.Finished && !paused)
                {
                    Thread.Sleep(200);
                }

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
            player.StopAllSounds();

            if (song != null)
                song.Position = 0;

            SongFinished = delegate { };
        }

        public void Pause()
        {
            paused = true;
            song.Position = klangsong.PlayPosition;
            player.StopAllSounds();

            SongFinished = delegate { };
        }


        public bool isPlaying()
        {
            if (klangsong == null) return false;
            return player.IsCurrentlyPlaying(song.URL);
        }

        public double Position
        {
            get
            {
                if (klangsong == null) return 0;
                else if (isPlaying() == false) return song.Position / 1000d;
                else return klangsong.PlayPosition / 1000d;
            }
            set
            {
                klangsong.PlayPosition = (uint)value * 1000;
            }
        }

        public int Volume
        {
            get
            {
                Console.WriteLine("klang sound get: " + player.SoundVolume + " return: " + (int)(player.SoundVolume * 100));
                return (int)(player.SoundVolume * 100);
            }
            set
            {
                Console.WriteLine("klang sound set: " + player.SoundVolume + " return: " + (float)value / 100f);
                player.SoundVolume = (float)value / 100f;

            }
        }

        public ISoundSource GetSoundSource(string URL)
        {
            return player.GetSoundSource(URL);
        }



        public event EventHandler SongFinished = delegate { };



    }
}
