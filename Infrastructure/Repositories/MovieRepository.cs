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
    public class MovieRepository(DemoDbContext dbContext) : IMovieRepository
    {
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var results = await dbContext.Movies
                .Include(mv => mv.MovieTypes)
                .ThenInclude(mt => mt.Genre)
                .Include(mv => mv.Shows)
                .ToListAsync();
            return results;
        }

        public async Task<Movie> GetByConditionAsync(Expression<Func<Movie, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.Movies.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task<Movie> GetByNewCondition(Expression<Func<Movie, bool>> condition,
            Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>> include = null)
        {
            var dataset = dbContext.Movies.AsQueryable();

            if (include != null)
            {
                dataset = include(dataset);
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<Movie>> GetBySortCondition(string? sortOrder,
            Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>> include = null)
        {
            var dataset = dbContext.Movies.AsQueryable();
            if (include != null)
            {
                dataset = include(dataset);
            }

            switch (sortOrder)
            {
                case "upcoming":
                    dataset = dataset.Where(d => d.Status == false);
                    break;
                case "default":
                    dataset = dataset.Where(d => d.Status == true);
                    break;
            }

            return await dataset.ToListAsync();
        }

        public async Task AddAsync(Movie entity)
        {
            await dbContext.Movies.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie entity, int id)
        {
            var existingModel = await dbContext.Movies.Include(m => m.MovieTypes).FirstOrDefaultAsync(m => m.Id == id);
            if (existingModel != null)
            {
                existingModel.Title = entity.Title;
                existingModel.Description = entity.Description;
                existingModel.Director = entity.Director;
                existingModel.Actors = entity.Actors;
                existingModel.TrailerUrl = entity.TrailerUrl;
                existingModel.DurationMinutes = entity.DurationMinutes;
                existingModel.Poster = entity.Poster == null ? existingModel.Poster : entity.Poster;
                existingModel.ScreeningDay = entity.ScreeningDay;
                existingModel.Status = entity.Status;
                existingModel.Language = entity.Language;
                existingModel.LimitAge = entity.LimitAge;

                dbContext.MovieTypes.RemoveRange(existingModel.MovieTypes);
                if (entity.MovieTypes != null && entity.MovieTypes.Any())
                {
                    foreach (var movieType in entity.MovieTypes)
                    {
                        dbContext.MovieTypes.Add(new MovieType
                        {
                            MovieId = id,
                            GenreId = movieType.GenreId
                        });
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingModel = await dbContext.Movies.FindAsync(id);
            if (existingModel != null)
            {
                var existingMovieTypes = await dbContext.MovieTypes.Where(s => s.MovieId == id).ToListAsync();
                dbContext.RemoveRange(existingMovieTypes);
                dbContext.Movies.Remove(existingModel);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}