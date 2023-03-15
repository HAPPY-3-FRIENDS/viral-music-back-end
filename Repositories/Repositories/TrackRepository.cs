using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository
    {
        public async Task<Track> GetByTrackDateTime(DateTime trackDateTime)
            => await TrackDAO.Instance.GetByTrackDateTime(trackDateTime);
    }
}
