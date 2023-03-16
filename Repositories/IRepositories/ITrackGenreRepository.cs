using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ITrackGenreRepository : IGenericRepository<TrackGenre>
    {
        Task<IEnumerable<TrackGenre>> GetAllTracksOfGenreByGenreIdAsync(int genreId);
        Task<IEnumerable<TrackGenre>> GetAllGenresOfTrackByTrackIdAsync(int trackId);
        Task AddListGenreToATrack(int trackId, List<int> listGenreIds);
    }
}
