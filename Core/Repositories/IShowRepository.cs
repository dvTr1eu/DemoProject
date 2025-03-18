using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Repositories
{
    public interface IShowRepository : IRepositoryBase<Show, int>
    {
        Task<(IEnumerable<Show> Shows, int TotalCount)> GetAllAsync(int page, int pageSize);
        Task<Show> GetByNewCondition(Expression<Func<Show, bool>> condition,
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include = null);

        Task<IEnumerable<Show>> GetListByCondition(Expression<Func<Show, bool>> condition,
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include = null);
    }
}
