using BusinessObjects.Models;
using System;
using System.Collections.Generic;

namespace BusinessObjects.DataTranferObjects
{
    public class TrackDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<TrackArtist> TrackArtists { get; set; }
        public virtual ICollection<TrackGenre> TrackGenres { get; set; }
    }
}
