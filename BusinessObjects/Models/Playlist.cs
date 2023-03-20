using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            TrackInPlaylists = new HashSet<TrackInPlaylist>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Owner { get; set; }

        public virtual User OwnerNavigation { get; set; }
        public virtual ICollection<TrackInPlaylist> TrackInPlaylists { get; set; }
    }
}
