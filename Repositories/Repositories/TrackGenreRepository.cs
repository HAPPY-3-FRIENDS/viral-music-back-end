using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class TrackGenreRepository : GenericRepository<TrackGenre>, ITrackGenreRepository
    {
        public async Task AddListGenreToATrack(int trackId, List<int> listGenreIds)
            => await TrackGenreDAO.Instance.AddListGenreToATrack(trackId, listGenreIds);

        public async Task<IEnumerable<TrackGenre>> GetAllGenresOfTrackByTrackIdAsync(int trackId)
            => await TrackGenreDAO.Instance.GetAllGenresOfTrackByTrackIdAsync(trackId);

        public async Task<IEnumerable<TrackGenre>> GetAllTracksOfGenreByGenreIdAsync(int genreId)
            => await TrackGenreDAO.Instance.GetAllTracksOfGenreByGenreIdAsync(genreId);
    }
}
