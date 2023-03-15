using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
