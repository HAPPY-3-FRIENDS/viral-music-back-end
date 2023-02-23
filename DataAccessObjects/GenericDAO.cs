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

        public async Task<T> GetByIdAsync(int id)
        {
            T t = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.Set<T>().FindAsync(id);
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return t;
        }

        public async Task<int> CountAsync()
        {
            int t = 0;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    t = await dbContext.Set<T>().CountAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return t;
        }

        public async Task AddAsync(T t)
        {
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    dbContext.Set<T>().Add(t);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(T t)
        {
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    dbContext.Set<T>().Update(t);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveAsync(T t)
        {
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    dbContext.Set<T>().Remove(t);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}