using BusinessObjects.Exceptions;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViralMusicAPI.Exceptions;

namespace DataAccessObjects
{
    public class TrackInPlaylistDAO : GenericDAO<TrackInPlaylist>
    {
        private static TrackInPlaylistDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TrackInPlaylistDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TrackInPlaylistDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<TrackInPlaylist>> GetAllTracksInPlaylistByPlaylistIdAsync(int playlistId)
        {
            IEnumerable<TrackInPlaylist> t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.TrackInPlaylists
                        .Include(x => x.Track)
                        .Include(x => x.Track.TrackArtists).ThenInclude(x => x.Artist).AsNoTracking()
                        .Where(x => x.PlaylistId == playlistId)
                        .ToListAsync();
                }

                if (t == null) throw new NotFoundException("Can not find any track in this playlist!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return t;
        }

        public async Task AddATrackToAPlaylist(int playlistId, int trackId)
        {
            Playlist playlist = await GenericDAO<Playlist>.Instance.GetByIdAsync(playlistId);
            if (playlist == null) throw new BadRequestException("Playlist with playlistId '" + playlistId + "' is not existed!");

            Track track = await GenericDAO<Track>.Instance.GetByIdAsync(trackId);
            if (track == null) throw new BadRequestException("Track with trackId '" + trackId + "' is not existed!");

            TrackInPlaylist existedTrackInPlaylist = await GenericDAO<TrackInPlaylist>.Instance.GetAWithConditionAsync(x => x.PlaylistId == playlistId && x.TrackId == trackId);
            if (existedTrackInPlaylist != null) throw new BadRequestException("Track with trackId '" + trackId + "' is existed in Playlist with playlistId '" + playlistId + "'!");

            TrackInPlaylist trackInPlaylist = new TrackInPlaylist
            {
                PlaylistId = playlistId,
                TrackId = trackId,
            };
            await GenericDAO<TrackInPlaylist>.Instance.AddAsync(trackInPlaylist);
        }

        public async Task DeleteATrackInAPlaylist(int playlistId, int trackId)
        {
            Playlist playlist = await GenericDAO<Playlist>.Instance.GetByIdAsync(playlistId);
            if (playlist == null) throw new BadRequestException("Playlist with playlistId '" + playlistId + "' is not existed!");

            Track track = await GenericDAO<Track>.Instance.GetByIdAsync(trackId);
            if (track == null) throw new BadRequestException("Track with trackId '" + trackId + "' is not existed!");

            TrackInPlaylist existedTrackInPlaylist = await GenericDAO<TrackInPlaylist>.Instance.GetAWithConditionAsync(x => x.PlaylistId == playlistId && x.TrackId == trackId);

            await GenericDAO<TrackInPlaylist>.Instance.RemoveAsync(existedTrackInPlaylist);
        }
    }
}
