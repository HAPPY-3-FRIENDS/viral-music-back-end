using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class TrackArtist
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public int TrackId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Track Track { get; set; }
    }
}