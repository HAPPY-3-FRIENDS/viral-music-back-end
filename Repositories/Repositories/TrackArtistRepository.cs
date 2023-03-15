using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class TrackArtistRepository : GenericRepository<TrackArtist>, ITrackArtistRepository
    {
        public async Task<IEnumerable<TrackArtist>> GetAllArtistOfTrackByTrackIdAsync(int trackId)
            => await TrackArtistDAO.Instance.GetAllArtistOfTrackByTrackIdAsync(trackId);

        public async Task<IEnumerable<TrackArtist>> GetAllTracksOfArtistByArtistIdAsync(int artistId)
            => await TrackArtistDAO.Instance.GetAllTracksOfArtistByArtistIdAsync(artistId);
    }
}
