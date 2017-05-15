namespace MusicPlayerCore.Player
{
    public interface ISong
    {
        string URL { get; }

        IPlayer Player { get; }

        string ArtistName { get; }

        string SongName { get; }

        string AlbumName { get; }

        double Duration { get; }

        double Position { get; set;  }

        string Year { get; }


    }
}
