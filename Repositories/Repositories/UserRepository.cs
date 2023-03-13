using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public async Task<User> GetByUsername(string username)
            => await UserDAO.Instance.GetByUsername(username);

        public async Task<User> AddUserAsync(User user)
            => await UserDAO.Instance.AddUserAsync(user);

        public async Task UpdateUserAsync(User user)
            => await UserDAO.Instance.UpdateUserAsync(user);

        public async Task<int> CountUsers()
            => await UserDAO.Instance.CountAsync(x => x.RoleId == 2);
    }
}