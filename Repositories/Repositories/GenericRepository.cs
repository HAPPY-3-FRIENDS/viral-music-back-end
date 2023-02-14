using DataAccessObjects;
using Repositories.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public async Task<IEnumerable<T>> GetAllAsync()
            => await GenericDAO<T>.Instance.GetAllAsync();
    }
}