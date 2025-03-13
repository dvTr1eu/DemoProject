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
    public class GenreRepository(DemoDbContext dbContext) : IGenreRepository
    {
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            var results = await dbContext.Genres
                .Include(g => g.MovieTypes)
                .ToListAsync();
            return results;
        }

        public async Task<Genre> GetByConditionAsync(Expression<Func<Genre, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.Genres.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(Genre entity)
        {
            await dbContext.Genres.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Genre entity, int id)
        {
            var existingModel = await dbContext.Genres.FindAsync(id);
            if (existingModel != null)
            {
                existingModel.Name = entity.Name;
                existingModel.Description = entity.Description;
                existingModel.MovieTypes = entity.MovieTypes;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var deleteModel = await dbContext.Genres.FindAsync(id);
            if (deleteModel != null)
            {
                dbContext.Genres.Remove(deleteModel);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}