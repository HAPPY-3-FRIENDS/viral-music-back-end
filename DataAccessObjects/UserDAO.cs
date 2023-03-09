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

        public async Task<User> getByUsername(string username)
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

        /*public async Task<User> mapUserDTOtoUser(UserDTO userDTO)
        {
            User user = null;
            try
            {
                var dbContext = new ViralMusicContext();
                Role role = await dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == userDTO.Role);
                if (role != null)
                {
                    user = await dbContext.Users.FindAsync(username);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }*/

        public async Task<User> InitUser(User user)
        {
            var dbContext = new ViralMusicContext();
            Role role = await dbContext.Roles.FindAsync(2);
            user.RoleId = 2;
            /*user.Role = role;*/
            return user;
        }
    }
}