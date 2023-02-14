using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class GenericDAO<T> where T : class
    {
        private static GenericDAO<T> instance = null;
        private static readonly object instanceLock = new object();

        public static GenericDAO<T> Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new GenericDAO<T>();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.Set<T>().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return t;
        }
    }
}