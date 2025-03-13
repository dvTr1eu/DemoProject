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
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories
{
    public class BookingRepository(DemoDbContext dbContext) : IBookingRepository
    {
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            var result = await dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Payment)
                .Include(b => b.BookingSeats)
                .Include(b => b.Show)
                .ThenInclude(s => s.ShowTimeDetails)
                .ToListAsync();
            return result;
        }

        public async Task<Booking> GetByConditionAsync(Expression<Func<Booking, bool>> condition, string[]? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(Booking entity)
        {
            await dbContext.Bookings.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking entity, int id)
        {
            var existingEntity = await dbContext.Bookings
                .Include(b => b.Show)
                .ThenInclude(s => s.ShowTimeDetails)
                .Include(b => b.BookingSeats)
                .Include(b => b.User).FirstOrDefaultAsync(r => r.Id == id);
            if (existingEntity != null)
            {
                existingEntity.ShowId = entity.ShowId;
                existingEntity.BookingTime = entity.BookingTime;
                existingEntity.TotalAmount = entity.TotalAmount;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingModel = await dbContext.Bookings.FindAsync(id);
            if (existingModel != null)
            {
                dbContext.Bookings.RemoveRange(existingModel);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Booking>> GetListByConditionAsync(Expression<Func<Booking, bool>> condition, Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> include = null)
        {
            var dataset = dbContext.Bookings.AsQueryable();

            if (include != null)
            {
                dataset = include(dataset);
            }

            return await dataset.Where(condition).ToListAsync();
        }

        public async Task<Booking> GetByNewConditionAsync(Expression<Func<Booking, bool>> condition, Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> include = null)
        {
            var dataset = dbContext.Bookings.AsQueryable();

            if (include != null)
            {
                dataset = include(dataset);
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }
    }
}
