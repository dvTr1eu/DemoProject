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
    public class ShowtimeDetailRepository(DemoDbContext dbContext) : IShowtimeDetailRepository
    {
        public async Task AddShowTime(List<TimeOnly> times, int showId)
        {
            foreach (var time in times)
            {
                var showTime = new ShowtimeDetail
                {
                    ShowId = showId,
                    ShowTime = time
                };
                await dbContext.ShowTimeDetails.AddAsync(showTime);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ShowtimeDetail>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ShowtimeDetail> GetByConditionAsync(Expression<Func<ShowtimeDetail, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.ShowTimeDetails.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(ShowtimeDetail entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ShowtimeDetail entity, int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
