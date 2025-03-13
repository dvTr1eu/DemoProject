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
    public class CinemaRepository(DemoDbContext dbContext) : ICinemaRepository
    {
        public async Task<IEnumerable<Cinema>> GetAllAsync()
        {
            var results = await dbContext.Cinemas
                .Include(c => c.Rooms)
                .ToListAsync();
            return results;
        }

        public async Task<Cinema> GetByConditionAsync(Expression<Func<Cinema, bool>> condition,
            string[]? includes = null)
        {
            var dataset = dbContext.Cinemas.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(Cinema entity)
        {
            await dbContext.Cinemas.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cinema entity, int id)
        {
            var existingEntity = await dbContext.Cinemas.Include(c => c.Rooms).FirstOrDefaultAsync(c => c.Id == id);
            if (existingEntity != null)
            {
                existingEntity.Address = entity.Address;
                existingEntity.Name = entity.Name;
                existingEntity.ImageMap = entity.ImageMap;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingEntity = await dbContext.Cinemas.Include(c => c.Rooms).FirstOrDefaultAsync(c => c.Id == id);
            if (existingEntity != null)
            {
                dbContext.Cinemas.Remove(existingEntity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}