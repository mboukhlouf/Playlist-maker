using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playlist_maker
{
    class Track
    {
        public String Title { get; set; }
        public String Artist { get; set; }
        public String Album { get; set; }

        public Track()
        {

        }

        public Track(String Title, String Artist, String Album)
        {
            this.Title = Title;
            this.Artist = Artist;
            this.Album = Album;
        }

        public override String ToString()
        {
            return $"{Artist} - {Album} - {Title}";
        }
    }
}
