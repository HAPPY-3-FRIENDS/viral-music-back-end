using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IAuthenticationRepository
    {
        Task<string> Authentication(string username, string password);
    }
}
