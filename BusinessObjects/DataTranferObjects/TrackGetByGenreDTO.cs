using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DataTranferObjects
{
    public class TrackGetByGenreDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<String> Artists { get; set; }
    }
}
