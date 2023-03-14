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
    }
}
