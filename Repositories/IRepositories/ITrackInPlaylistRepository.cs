using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ITrackInPlaylistRepository : IGenericRepository<TrackInPlaylist>
    {
        Task<IEnumerable<TrackInPlaylist>> GetAllTracksInPlaylistByPlaylistIdAsync(int playlistId);
        Task AddATrackToAPlaylist(int playlistId, int trackId);
        Task DeleteATrackInAPlaylist(int playlistId, int trackId);
    }
}
