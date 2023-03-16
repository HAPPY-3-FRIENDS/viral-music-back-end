using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViralMusicAPI.Exceptions;

namespace DataAccessObjects
{
    public class TrackArtistDAO : GenericDAO<TrackArtist>
    {
        private static TrackArtistDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TrackArtistDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TrackArtistDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<TrackArtist>> GetAllArtistOfTrackByTrackIdAsync(int trackId)
        {
            IEnumerable<TrackArtist> t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.TrackArtists
                        .Include(x => x.Artist)
                        .Where(x => x.TrackId == trackId)
                        .ToListAsync();
                }

                if (t == null) throw new NotFoundException("Can not find any artist in this track!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return t;
        }

        public async Task<IEnumerable<TrackArtist>> GetAllTracksOfArtistByArtistIdAsync(int artistId)
        {
            IEnumerable<TrackArtist> t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.TrackArtists
                        .Include(x => x.Track)
                        .Where(x => x.ArtistId == artistId)
                        .ToListAsync();
                }

                if (t == null) throw new NotFoundException("Can not find any track of this artist!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return t;
        }

        public async Task AddListArtistToATrack(int trackId, List<int> listArtistIds)
        {
            List<TrackArtist> t = new List<TrackArtist>();
            try
            {
                foreach (var artistId in listArtistIds)
                {
                    TrackArtist trackArtist = new TrackArtist
                    {
                        TrackId = trackId,
                        ArtistId = artistId
                    };
                    t.Add(trackArtist);
                }
                using (var dbContext = new ViralMusicContext())
                {
                    foreach (var trackArtist in t)
                    {
                        await dbContext.TrackArtists.AddRangeAsync(trackArtist);
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
