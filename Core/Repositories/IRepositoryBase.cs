using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRepositoryBase<T, TId> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByConditionAsync(Expression<Func<T, bool>> condition, string[]? includes = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, TId id);
        Task DeleteAsync(TId id);
    }
}