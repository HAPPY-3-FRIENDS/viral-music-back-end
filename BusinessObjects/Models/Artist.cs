using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Artist
    {
        public Artist()
        {
            TrackArtists = new HashSet<TrackArtist>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<TrackArtist> TrackArtists { get; set; }
    }
}