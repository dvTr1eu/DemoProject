using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Services
{
    public class MovieService(IMovieRepository movieRepository) : IMovieService
    {
        public async Task<bool> Create(Movie entity)
        {
            try
            {
                await movieRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await movieRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Movie entity, int id)
        {
            try
            {
                await movieRepository.UpdateAsync(entity, id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Movie?> FindById(int id)
        {
            Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>> include =
                q => q.Include(m => m.MovieTypes).ThenInclude(mt => mt.Genre);
            var result = await movieRepository.GetByNewCondition(m => m.Id == id, include);
            return result;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            var results = await movieRepository.GetAllAsync();
            return results;
        }

        public async Task<IEnumerable<Movie>> ListSort(string? sortOrder)
        {
            Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>> include =
                q => q.Include(m => m.MovieTypes).ThenInclude(mt => mt.Genre);
            var results = await movieRepository.GetBySortCondition(sortOrder, include);
            return results;
        }
    }
}