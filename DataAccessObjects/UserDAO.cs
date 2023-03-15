using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public async Task<User> Authentication(string username, string password)
        {
            User user = null;
            try
            {
                var dbContext = new ViralMusicContext();
                user = await dbContext.Users.Include(u => u.Role).Where(u => u.Username.Equals(username) && u.Password.Equals(password)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
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

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                SetUserRole(user);
                if (user.Avatar == null)
                {
                    // Default avatar
                    user.Avatar = "https://www.pumpkin.care/wp-content/uploads/2020/08/Cat-Memes-2020.jpg";
                }

                await AddAsync(user);
                user.Role = GetRole((int)user.RoleId);
                return user;
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

                await UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetUserRole(User user)
        {
            switch (user.Role.RoleName)
            {
                case "Admin":
                    user.RoleId = 1;
                    break;
                default:
                    user.RoleId = 2;
                    break;
            }
            user.Role = null;
        }

        private Role GetRole(int roleId)
        {
            Role role = null;   
            switch (roleId)
            {
                case 1:
                    role = new Role
                    {
                        Id = 1,
                        RoleName = "Admin"
                    };
                    break;
                default:
                    role = new Role
                    {
                        Id = 2,
                        RoleName = "User"
                    };
                    break;
            }
            return role;
        }
    }
}