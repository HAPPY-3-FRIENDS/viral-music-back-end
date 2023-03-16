using BusinessObjects.Models;

namespace BusinessObjects.DataTranferObjects
{
    public class TrackGenreDTO
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int TrackId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Track Track { get; set; }
    }
}
