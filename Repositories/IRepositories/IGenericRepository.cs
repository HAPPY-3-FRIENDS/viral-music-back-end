using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> CountAsync();
        Task AddAsync(T t);
        Task DeleteAsync(T t);
        Task UpdateAsync(T t);
    }
}