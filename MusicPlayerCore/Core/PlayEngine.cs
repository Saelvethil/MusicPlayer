using MusicPlayerCore.Enumerator;
using MusicPlayerCore.Player;
using System;

namespace MusicPlayerCore.Core
{
    public class PlayEngine
    {
        private SongList songList;
        private SongEnumerator enumerator;
        public event EventHandler SongFinished = delegate { };
        private int _volume = 20;
        private bool autoPlay = true;

        public ISong CurrentSong { get; set; }


        public PlayEngine()
        {
            songList = new SongList();
        }

        public void SetRandomEnumerator(bool value)
        {
            if (value)
            {
                enumerator = (SongEnumerator)songList.GetRandomEnumerator();
            }
            else
            {
                enumerator = (SongEnumerator)songList.GetEnumerator();
            }

        }



        public void SetPlaylist(SongList SongList)
        {
            this.songList = SongList;
            enumerator = (SongEnumerator)songList.GetEnumerator();
            if (songList.ListSize > 0)
            {
                CurrentSong = enumerator.Current;
            }
            else autoPlay = false;
        }

        public void Play()
        {

            CurrentSong = enumerator.Current;
            CurrentSong.Player.SongFinished += Player_SongFinished;
            CurrentSong.Player.Volume = _volume;
            CurrentSong.Player.Play(CurrentSong);
            autoPlay = true;


        }

        public void SetSong(ISong song)
        {
            while (true)
            {
                enumerator.MoveNext();
                if (song == enumerator.Current)
                    break;
            }
            Stop();
            CurrentSong = song;
            Play();
        }

        void Player_SongFinished(object sender, EventArgs e)
        {
            if (songList.ListSize > 0)
            {
                NextSong();
                this.SongFinished(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if(CurrentSong != null)
                 CurrentSong.Player.Stop();
            autoPlay = false;
        }

        public void Pause()
        {
            CurrentSong.Player.Pause();
            autoPlay = false;
        }



        public void NextSong()
        {
            bool tmp = autoPlay;
            CurrentSong.Player.Stop();
            enumerator.MoveNext();
            CurrentSong = enumerator.Current;
            if (tmp) Play();

        }

        public void PreviousSong()
        {
            bool tmp = autoPlay;
            CurrentSong.Player.Stop();
            enumerator.MovePrevious();
            CurrentSong = enumerator.Current;
            if (tmp) Play();
        }

        public bool IsPlaying()
        {
            if (CurrentSong == null) return false;
            return CurrentSong.Player.isPlaying();
        }

        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                CurrentSong.Player.Volume = value;
            }
        }

        
         /*       public SongList GetCurrentSongList()
        {
            return songList;
        }
        public double Position
       {
           get
           {
               return CurrentSong.Player.Position;
           }
           set
           {
               CurrentSong.Player.Position = value;
           }
       }

      public double Duration
       {
           get
           {
               return CurrentSong.Duration;
           }
       }*/
    }
}
