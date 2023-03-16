using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ITrackArtistRepository : IGenericRepository<TrackArtist>
    {
        Task<IEnumerable<TrackArtist>> GetAllTracksOfArtistByArtistIdAsync(int artistId);
        Task<IEnumerable<TrackArtist>> GetAllArtistOfTrackByTrackIdAsync(int trackId);
        Task AddListArtistToATrack(int trackId, List<int> listArtistIds);
    }
}
