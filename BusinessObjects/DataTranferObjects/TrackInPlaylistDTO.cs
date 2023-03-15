using BusinessObjects.Models;

namespace BusinessObjects.DataTranferObjects
{
    public class TrackInPlaylistDTO
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
    }
}
