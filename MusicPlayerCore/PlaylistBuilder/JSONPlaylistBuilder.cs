using MusicPlayerCore.Player;
using System.Text;

namespace MusicPlayerCore.Builder
{
    public class JSONPlaylistBuilder : IPlaylistBuilder
    {
        StringBuilder stringBuilder = new StringBuilder();

        public void AddHeader(string playlistName)
        {
            stringBuilder.Append("{\n \t\"name\": \""+playlistName + "\",\n \t\"song\": [ \n");
        }

        public void AddSong(ISong song)
        {
            stringBuilder.Append("\t\t{\"URL\": \""+ song.URL +"\"},\n");
        }

        public void AddFooter()
        {
            stringBuilder.Remove(stringBuilder.Length - 2, 1);
            stringBuilder.Append("\t]\n}");
            
        }

        public string GetResult()
        {
            return stringBuilder.ToString();
        }
    }
}
