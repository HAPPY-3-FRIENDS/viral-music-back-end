using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<User>> GetUsersAsync() 
            => await UserDAO.Instance.GetUsersAsync();
    }
}
