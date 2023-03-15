using BusinessObjects.Models;

namespace BusinessObjects.DataTranferObjects
{
    public class TrackArtistDTO
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public int TrackId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Track Track { get; set; }
    }
}
