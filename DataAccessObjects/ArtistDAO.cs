using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ArtistDAO : GenericDAO<Artist>
    {
        private static ArtistDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ArtistDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ArtistDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Artist> GetByArtistName(string artistName)
        {
            Artist artist = null;
            try
            {
                var dbContext = new ViralMusicContext();
                artist = await dbContext.Artists.FirstOrDefaultAsync(u => u.Name.Equals(artistName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return artist;
        }

        public async Task<IEnumerable<Artist>> GetListByArtistName(string artistName)
        {
            IEnumerable<Artist> artists = null;
            try
            {
                var dbContext = new ViralMusicContext();
                artists = await dbContext.Set<Artist>().Where(u => u.Name.Contains(artistName)).ToArrayAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return artists;
        }
    }
}
