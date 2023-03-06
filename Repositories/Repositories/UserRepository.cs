using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public async Task<int> CountUsers()
            => await UserDAO.Instance.CountAsync(x => x.RoleId == 2);

        public async Task<User> GetByUsername(string username)
            => await UserDAO.Instance.getByUsername(username);

        public async Task<User> InitUser(User user)
            => await UserDAO.Instance.InitUser(user);
    }
}