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
    public class TrackDAO : GenericDAO<Track>
    {
        private static TrackDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TrackDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TrackDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Track> GetByTrackDateTime(DateTime dateTime)
        {
            Track track = null;
            try
            {
                var dbContext = new ViralMusicContext();
                track = await dbContext.Tracks.OrderBy(x => x.Id).LastOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return track;
        }

        public async Task<IEnumerable<TrackGetByGenreDTO>> GetTracksListByName(string name, IMapper mapper)
        {
            List<TrackGetByGenreDTO> allTracks = new List<TrackGetByGenreDTO>();
            try
            {
                var dbContext = new ViralMusicContext();
                IEnumerable<Track> tracks = new List<Track>();
                tracks = await dbContext.Set<Track>().Where(u => u.Title.Contains(name)).ToArrayAsync();

                foreach (var track in tracks)
                {
                    TrackGetByGenreDTO trueTrack = mapper.Map<TrackGetByGenreDTO>(track);
                    trueTrack.Artists = new List<string>();
                    IEnumerable<TrackArtist> trackArtists = await TrackArtistDAO.Instance.GetAllArtistOfTrackByTrackIdAsync(track.Id);
                    foreach (var trackArtist in trackArtists)
                    {
                        Artist trueArtist = await ArtistDAO.Instance.GetByIdAsync(trackArtist.ArtistId);
                        string artist = trueArtist.Name;
                        trueTrack.Artists.Add(artist);
                    }
                    allTracks.Add(trueTrack);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return allTracks;
        }
    }
}
