using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Playlist_maker
{
    class Program
    {
        static void Main(string[] args)
        {
            String PlaylistName = "90s Rock Anthems";
            //Track[] playlist = Spotify.GetPlaylist("37i9dQZF1DX1rVvRgjX59F");
            Track[] playlist = LoadTracksFromFile("90s Rock Anthems.txt");
            String path = @"D:\Music"; 
            String playlistFile = ""; 

            foreach (Track track in playlist)
            {
                track.Title = Simplify(track.Title);
                track.Album = Simplify(track.Album);

                String entry = FindSong(path, track);
                entry = entry == "" ? $"# {track}" : entry.Replace(path, ".");
                Console.WriteLine(entry);
                playlistFile += entry + "\n";
            }

            File.WriteAllText($"{path}\\{PlaylistName}.m3u", playlistFile);
            Console.WriteLine("Done!");
            Console.Read();
        }

        private static String FindSong(String path, Track track)
        {
            String[] files = Directory.GetFiles(path);
            foreach (String file in files)
            {
                try
                {
                    TagLib.File Song = TagLib.File.Create(file);
                    bool flag1 = String.Compare(Simplify(Song.Tag.Title), track.Title, true) == 0;
                    bool flag2 = String.Compare(Simplify(Song.Tag.Album), track.Album, true) == 0;
                    // 
                    // flag2 = true;
                    //
                    if (flag1 && flag2)
                    {
                        return file;
                    }
                    Song.Dispose();
                }
                catch(Exception)
                {
                } 
            }

            String[] directories = Directory.GetDirectories(path);
            // Check if there is a folder with the artist or album name
            bool found = false;
            foreach (String directory in directories)
            {
                String[] tokens = directory.Split('\\');
                String folderName = tokens[tokens.Length - 1];
                if (Simplify(folderName) == track.Album)
                {
                    found = true;
                    String song = FindSong(directory, track);
                    if (song != "")
                        return song;
                }
                if (Simplify(folderName) == track.Artist)
                {
                    found = true;
                    String song = FindSong(directory, track);
                    if (song != "")
                        return song;
                }
            }
            if(!found)
            {
                foreach (String directory in directories)
                {
                    String song = FindSong(directory, track);
                    if (song != "")
                        return song;
                }
            }
            return "";
        }

        private static String Simplify(String str)
        {
            String newStr = "";
            foreach(char c in str)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    newStr += c;
            }
            newStr = Regex.Replace(newStr, @"\d{4}", "");
            newStr = newStr.Replace("Part", "Pt");
            newStr = newStr.Replace("Remastered", "");
            newStr = newStr.Replace("Remaster", "");
            newStr = newStr.Replace("Version", "");
            return newStr.Trim();
        }

        private static Track ParseTrack(String text)
        {
            String[] tokens = text.Split('|');
            String title = tokens[0];
            String artist = tokens[1];
            String album = tokens[2];

            return new Track(title, artist, album);
        }

        private static Track[] LoadTracksFromFile(String path)
        {
            String[] lines = File.ReadAllLines(path);
            Track[] tracks = new Track[lines.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                tracks[i] = ParseTrack(lines[i]);
            }
            return tracks;
        }
    }
}
