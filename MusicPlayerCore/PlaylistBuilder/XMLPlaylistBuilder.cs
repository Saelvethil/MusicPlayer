
using MusicPlayerCore.Player;
using System.Text;

namespace MusicPlayerCore.Builder
{
    class XMLPlaylistBuilder : IPlaylistBuilder
    {
        StringBuilder stringBuilder = new StringBuilder();

        public void AddHeader(string playlistName)
        {

            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
            stringBuilder.Append("<PlayList name=\"" + playlistName + "\">\n");
        }

        public void AddSong(ISong song)
        {
            stringBuilder.Append("\t<Song>\n");
            stringBuilder.Append("\t\t<URL>");
            stringBuilder.Append(song.URL);
            stringBuilder.Append("</URL>\n"); 
            stringBuilder.Append("\t</Song>\n"); 
        }

        public void AddFooter()
        {
           stringBuilder.Append("</PlayList>");
        }

        public string GetResult()
        {
            return stringBuilder.ToString();
        }
    }
}
