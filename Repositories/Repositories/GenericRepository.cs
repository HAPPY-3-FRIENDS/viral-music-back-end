using DataAccessObjects;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public async Task AddAsync(T t)
            => await GenericDAO<T>.Instance.AddAsync(t);

        public async Task<int> CountAsync()
            => await GenericDAO<T>.Instance.CountAsync();

        public async Task DeleteAsync(T t)
            => await GenericDAO<T>.Instance.RemoveAsync(t);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await GenericDAO<T>.Instance.GetAllAsync();

        public async Task<IEnumerable<T>> GetAllIncludeAsync(List<Expression<Func<T, object>>> includes)
            => await GenericDAO<T>.Instance.GetAllIncludeAsync(includes);

        public async Task<T> GetByIdAsync(int id)
            => await GenericDAO<T>.Instance.GetByIdAsync(id);

        public async Task UpdateAsync(T t)
            => await GenericDAO<T>.Instance.UpdateAsync(t);
    }
}