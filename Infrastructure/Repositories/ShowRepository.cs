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
    public class ShowRepository(DemoDbContext dbContext) : IShowRepository
    {
        public async Task<IEnumerable<Show>> GetAllAsync()
        {
            var results = await dbContext.Shows
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .ThenInclude(s => s.Cinema)
                .Include(s => s.ShowTimeDetails)
                .ToListAsync();
            return results;
        }

        public async Task<Show> GetByConditionAsync(Expression<Func<Show, bool>> condition, string[]? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(Show entity)
        {
            await dbContext.Shows.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Show entity, int id)
        {
            var existingEntity = await dbContext.Shows
                .Include(s => s.Movie)
                .Include(s => s.ShowTimeDetails)
                .Include(s => s.Room)
                .ThenInclude(s => s.Cinema).FirstOrDefaultAsync(r => r.Id == id);
            if (existingEntity != null)
            {
                existingEntity.TicketPrice = entity.TicketPrice;
                existingEntity.ShowDay = entity.ShowDay;
                existingEntity.ShowTimeDetails = entity.ShowTimeDetails;
                existingEntity.RoomId = entity.RoomId;
                existingEntity.MovieId = entity.MovieId;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingModel = await dbContext.Movies.FindAsync(id);
            if (existingModel != null)
            {
                dbContext.Movies.RemoveRange(existingModel);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<Show> Shows, int TotalCount)> GetAllAsync(int page, int pageSize)
        {
            var query = dbContext.Shows
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .ThenInclude(s => s.Cinema)
                .Include(s => s.ShowTimeDetails);

            int totalCount = await query.CountAsync();

            var results = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize) 
                .ToListAsync();

            return (results, totalCount);
        }

        public async Task<Show> GetByNewCondition(Expression<Func<Show, bool>> condition, Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include = null)
        {
            var dataset = dbContext.Shows.AsQueryable();

            if (include != null)
            {
                dataset = include(dataset);
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<Show>> GetListByCondition(Expression<Func<Show, bool>> condition, Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include = null)
        {
            var dataset = dbContext.Shows.AsQueryable();

            if (include != null)
            {
                dataset = include(dataset);
            }

            return await dataset.Where(condition).ToListAsync();
        }
    }
}
