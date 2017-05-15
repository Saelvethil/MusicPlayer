using MusicPlayerCore.Player;

namespace MusicPlayerCore.Builder
{
    interface IPlaylistBuilder
    {
        void AddHeader(string playlistName);
        void AddSong(ISong song);
        void AddFooter();
        string GetResult();
      
    }
}
