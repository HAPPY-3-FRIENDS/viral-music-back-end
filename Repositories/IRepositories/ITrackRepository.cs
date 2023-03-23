using AutoMapper;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ITrackRepository : IGenericRepository<Track>
    {
        Task<Track> GetByTrackDateTime(DateTime trackDateTime);
        Task<IEnumerable<TrackGetByGenreDTO>> GetTracksListByName(string name, IMapper mapper);
    }
}
