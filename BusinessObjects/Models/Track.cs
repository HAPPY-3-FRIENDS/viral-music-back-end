using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Track
    {
        public Track()
        {
            TrackArtists = new HashSet<TrackArtist>();
            TrackGenres = new HashSet<TrackGenre>();
            TrackInPlaylists = new HashSet<TrackInPlaylist>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }

        public virtual ICollection<TrackArtist> TrackArtists { get; set; }
        public virtual ICollection<TrackGenre> TrackGenres { get; set; }
        public virtual ICollection<TrackInPlaylist> TrackInPlaylists { get; set; }
    }
}