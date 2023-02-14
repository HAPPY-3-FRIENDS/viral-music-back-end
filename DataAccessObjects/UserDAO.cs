using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new();

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                }
                return instance;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<User> users = null;
            try
            {
                using (var dbContext = new ViralMusicContext())
                {
                    users = await dbContext.Users.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }
    }
}
