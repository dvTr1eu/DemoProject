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
    public interface IMovieRepository : IRepositoryBase<Movie, int>
    {
        Task<IEnumerable<Movie>> GetBySortCondition(string? sortOrder, Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>> include = null);
        Task<Movie> GetByNewCondition(Expression<Func<Movie, bool>> condition,
            Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>> include = null);
    }
}
