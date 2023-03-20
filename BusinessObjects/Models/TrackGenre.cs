using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects.Models
{
    public partial class TrackGenre
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int TrackId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Track Track { get; set; }
    }
}
