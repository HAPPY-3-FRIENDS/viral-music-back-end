using BusinessObjects.Models;
using Repositories.IRepositories;

namespace Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

    }
}