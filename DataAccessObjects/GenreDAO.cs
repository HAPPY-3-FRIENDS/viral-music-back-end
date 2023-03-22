using AutoMapper;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class GenreDAO : GenericDAO<Genre>
    {
        private static GenreDAO instance = null;
        private static readonly object instanceLock = new object();

        public static GenreDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new GenreDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Genre> GetByGenreName(string genreName)
        {
            Genre genre = null;
            try
            {
                var dbContext = new ViralMusicContext();
                genre = await dbContext.Genres.FirstOrDefaultAsync(u => u.Name.Equals(genreName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return genre;
        }

        public async Task<IEnumerable<GenreGetAllTrackDTO>> getAllTracksFilterByGenre(IMapper mapper)
        {
            List<GenreGetAllTrackDTO> allGenreGetAllTrack = new List<GenreGetAllTrackDTO>();
            try
            {
                var dbContext = new ViralMusicContext();
                List<Genre> allGenres = await dbContext.Genres.ToListAsync();
                foreach (var genre in allGenres)
                {
                    var genreGetAllTrack = mapper.Map<GenreGetAllTrackDTO>(genre);
                    allGenreGetAllTrack.Add(genreGetAllTrack);
                }


                List<TrackGenre> trackgenres = await dbContext.TrackGenres.ToListAsync();
                foreach (var trackgenre in trackgenres)
                {
                    Track trueTrack = await dbContext.Tracks.FirstOrDefaultAsync(u => u.Id.Equals(trackgenre.TrackId));
                    TrackGetByGenreDTO track = mapper.Map<TrackGetByGenreDTO>(trueTrack);
                    IEnumerable<TrackArtist> trackArtists = await TrackArtistDAO.Instance.GetAllArtistOfTrackByTrackIdAsync(trackgenre.TrackId);
                    foreach (var trackArtist in trackArtists)
                    {
                        Artist trueArtist = await ArtistDAO.Instance.GetByIdAsync(trackArtist.ArtistId);
                        string artist = trueArtist.Name;
                        track.Artists.Add(artist);
                    }

                    foreach (var genre in allGenreGetAllTrack)
                    {
                        if (genre.Id == trackgenre.GenreId)
                        {
                            genre.Tracks.Add(track);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return allGenreGetAllTrack;
        }
    }
}
