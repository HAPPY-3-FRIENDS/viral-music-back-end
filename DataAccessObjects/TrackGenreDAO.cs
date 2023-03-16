using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViralMusicAPI.Exceptions;

namespace DataAccessObjects
{
    public class TrackGenreDAO : GenericDAO<TrackGenre>
    {
        private static TrackGenreDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TrackGenreDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TrackGenreDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<TrackGenre>> GetAllGenresOfTrackByTrackIdAsync(int trackId)
        {
            IEnumerable<TrackGenre> t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.TrackGenres
                        .Include(x => x.Genre)
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

        public async Task<IEnumerable<TrackGenre>> GetAllTracksOfGenreByGenreIdAsync(int genreId) 
        { 
            IEnumerable<TrackGenre> t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.TrackGenres
                        .Include(x => x.Track)
                        .Where(x => x.GenreId == genreId)
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

        public async Task AddListGenreToATrack(int trackId, List<int> listGenreIds)
        {
            List<TrackGenre> t = new List<TrackGenre>();
            try
            {
                foreach (var genreId in listGenreIds)
                {
                    TrackGenre trackGenre = new TrackGenre
                    {
                        TrackId = trackId,
                        GenreId = genreId
                    };
                    t.Add(trackGenre);
                }
                using (var dbContext = new ViralMusicContext())
                {
                    foreach (var trackGenre in t)
                    {
                        await dbContext.TrackGenres.AddRangeAsync(trackGenre);
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
