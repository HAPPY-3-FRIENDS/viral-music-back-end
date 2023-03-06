using BusinessObjects.Models;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> InitUser(User user);
        Task<int> CountUsers();
    }
}