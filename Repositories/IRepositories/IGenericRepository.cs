using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllIncludeAsync(List<Expression<Func<T, object>>> includes);
        Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllWithConditionIncludeAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includes);
        Task<T> GetByIdAsync(int id);
        Task<int> CountAsync();
        Task AddAsync(T t);
        Task DeleteAsync(T t);
        Task UpdateAsync(T t);
    }
}