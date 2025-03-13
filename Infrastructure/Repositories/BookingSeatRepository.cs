using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingSeatRepository(DemoDbContext dbContext) : IBookingSeatRepository
    {
        public async Task<IEnumerable<BookingSeat>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BookingSeat> GetByConditionAsync(Expression<Func<BookingSeat, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.BookingSeats.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(BookingSeat entity)
        {
           await dbContext.BookingSeats.AddAsync(entity);
           await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookingSeat entity, int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
