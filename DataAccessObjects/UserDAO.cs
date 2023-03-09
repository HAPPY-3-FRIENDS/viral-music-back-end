using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO : GenericDAO<User>
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();

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
                    return instance;
                }
            }
        }

        public async Task<User> GetByUsername(string username)
        {
            User user = null;
            try
            {
                var dbContext = new ViralMusicContext();
                user = await dbContext.Users.Include(u => u.Role).Where(u => u.Username.Equals(username)).FirstOrDefaultAsync();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                SetUserRole(user);

                using (var dbContext = new ViralMusicContext())
                {
                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                SetUserRole(user);

                using (var dbContext = new ViralMusicContext())
                {
                    dbContext.ChangeTracker.Clear();
                    dbContext.Users.Update(user);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void SetUserRole(User user)
        {
            switch (user.Role.RoleName)
            {
                case "Admin":
                    user.RoleId = 1;
                    break;
                case "User":
                    user.RoleId = 2;
                    break;
            }
            user.Role = null;
        }
    }
}