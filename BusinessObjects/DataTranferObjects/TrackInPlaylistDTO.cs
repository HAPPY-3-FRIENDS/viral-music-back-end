using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
