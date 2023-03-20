using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects.Models
{
    public partial class TrackInPlaylist
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public int PlaylistId { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual Track Track { get; set; }
    }
}
