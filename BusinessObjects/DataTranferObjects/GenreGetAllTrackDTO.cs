using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DataTranferObjects
{
    public class GenreGetAllTrackDTO
    {
        public GenreGetAllTrackDTO()
        {
            Tracks = new List<TrackGetByGenreDTO>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TrackGetByGenreDTO> Tracks { get; set; }
    }
}
