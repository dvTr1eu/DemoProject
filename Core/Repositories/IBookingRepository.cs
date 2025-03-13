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
    public interface IBookingRepository : IRepositoryBase<Booking, int>
    {
        Task<IEnumerable<Booking>> GetListByConditionAsync(Expression<Func<Booking, bool>> condition,
            Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> include = null);
        Task<Booking> GetByNewConditionAsync(Expression<Func<Booking, bool>> condition,
            Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> include = null);
    }
}
