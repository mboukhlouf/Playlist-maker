using System;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Playlist_maker
{
    class Spotify
    {
        public static Track[] GetPlaylist(String id)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"https://open.spotify.com/user/spotify/playlist/{id}");
            HtmlNodeCollection scripts = doc.DocumentNode.SelectNodes(".//script");
            foreach(HtmlNode script in scripts)
            {
                if(script.InnerText.Contains("Spotify.Entity"))
                {
                    String StrJSON = script.InnerText.Split(new String[] { "Spotify.Entity = " }, StringSplitOptions.None)[1];
                    StrJSON = StrJSON.Trim();
                    StrJSON = StrJSON.Substring(0, StrJSON.Length - 1);
                    JObject entity = JObject.Parse(StrJSON);
                    JArray tracks = (JArray)entity["tracks"]["items"];
                    Track[] playlist = new Track[tracks.Count];
                    for(int i = 0; i < tracks.Count; i++)
                    { 
                        String title, artist, album;
                        JObject song = (JObject)tracks[i]["track"];
                        title = (String)song["name"];
                        artist = (String)song["artists"][0]["name"];
                        album = (String)song["album"]["name"];

                        playlist[i] = new Track()
                        {
                            Title = title,
                            Artist = artist,
                            Album = album
                        };
                    }
                    return playlist;
                }
            }
            return null;
        }
    }
}
