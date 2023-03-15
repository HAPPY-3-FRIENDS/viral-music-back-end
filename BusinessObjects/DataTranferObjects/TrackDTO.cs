using System;

namespace BusinessObjects.DataTranferObjects
{
    public class TrackDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
