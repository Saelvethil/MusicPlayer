namespace MusicPlayerCore.Core
{
    public class ApplicationController
    {
        public PlayEngine PlayEngine;
        public SongLibrary SongLibrary;
        public PlaylistManager PlaylistManager;

        public ApplicationController()
        {
            SongLibrary = new SongLibrary();
            //SongLibrary.AddSong(Path.GetFullPath("../../../../../song.mp3"));
            //SongLibrary.AddSong(Path.GetFullPath("../../../../../song9.mp3"));
            //SongLibrary.AddSong(Path.GetFullPath("../../../../../song10.mp3"));
            PlayEngine = new PlayEngine();            
            PlaylistManager = new PlaylistManager();
            PlayEngine.SetPlaylist(PlaylistManager.Playlist);
        }


        
        
        




    }
}
