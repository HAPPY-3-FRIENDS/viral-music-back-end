using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects.Models
{
    public partial class Genre
    {
        public Genre()
        {
            TrackGenres = new HashSet<TrackGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TrackGenre> TrackGenres { get; set; }
    }
}
