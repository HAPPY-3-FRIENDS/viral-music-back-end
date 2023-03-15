using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class TrackInPlaylistRepository : GenericRepository<TrackInPlaylist>, ITrackInPlaylistRepository
    {
        public async Task<IEnumerable<TrackInPlaylist>> GetAllTracksInPlaylistByPlaylistIdAsync(int playlistId)
            => await TrackInPlaylistDAO.Instance.GetAllTracksInPlaylistByPlaylistIdAsync(playlistId);

        public async Task AddATrackToAPlaylist(int playlistId, int trackId)
            => await TrackInPlaylistDAO.Instance.AddATrackToAPlaylist(playlistId, trackId);

        public async Task DeleteATrackInAPlaylist(int playlistId, int trackId)
            => await TrackInPlaylistDAO.Instance.DeleteATrackInAPlaylist(playlistId, trackId);
    }
}
