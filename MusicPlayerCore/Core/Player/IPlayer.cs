using System;

namespace MusicPlayerCore.Player
{
    public interface IPlayer
    {
        void Play(ISong song);

        void Stop();

        void Pause();

        bool isPlaying();

        double Position { get; set; }

        int Volume { get; set; }

        event EventHandler SongFinished;

    }
}
