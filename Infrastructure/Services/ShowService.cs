using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Services
{
    public class ShowService(IShowRepository showRepository) : IShowService
    {
        public async Task<IEnumerable<Show>> GetAll()
        {
            var results = await showRepository.GetAllAsync();
            return results;
        }

        public async Task<Show?> FindById(int id)
        {
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include =
                q => q.Include(q => q.Movie).Include(q => q.ShowTimeDetails).Include(s => s.Room).ThenInclude(r => r.Cinema);
            var result = await showRepository.GetByNewCondition(m => m.Id == id, include);
            return result;
        }

        public async Task<bool> Create(Show entity)
        {
            try
            {
                await showRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Show entity, int id)
        {
            try
            {
                await showRepository.UpdateAsync(entity,id);
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
                await showRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(IEnumerable<Show> Shows, int TotalCount)> GetListPagination(int page, int pageSize)
        {
            var (shows, totalCount) = await showRepository.GetAllAsync(page, pageSize);
            return (shows, totalCount);
        }

        public async Task<IEnumerable<Show?>> GetShowByShowDayAndMovieId(string day, int movieId)
        {
            var convertDay = DateTime.Parse(day);
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include =
                q => q.Include(q => q.Movie).Include(q => q.ShowTimeDetails.OrderBy(d => d.ShowTime)).Include(s => s.Room).ThenInclude(r => r.Cinema);
            var result = await showRepository.GetListByCondition(m => m.ShowDay == convertDay && m.MovieId == movieId, include);
            return result;
        }

        public async Task<IEnumerable<Show>> GetShowByDay(string day)
        {
            var convertDay = DateTime.Parse(day);
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include =
                q => q.Include(q => q.Movie).Include(q => q.ShowTimeDetails.OrderBy(d => d.ShowTime)).Include(s => s.Room).ThenInclude(r => r.Cinema);
            var result = await showRepository.GetListByCondition(m => m.ShowDay == convertDay, include);
            return result;
        }

        public async Task<Show> GetShowByShowIdAndShowTime(int showId, TimeOnly showTime)
        {
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include =
                q => q.Include(q => q.Movie).Include(q => q.ShowTimeDetails.OrderBy(d => d.ShowTime)).Include(s => s.Room).ThenInclude(r => r.Cinema);
            var result = await showRepository.GetByNewCondition(m => m.Id == showId && m.ShowTimeDetails.Any(st => st.ShowTime == showTime), include);
            return result;
        }

        public async Task<IEnumerable<Show?>> GetShowByMovieId(int movieId)
        {
            Func<IQueryable<Show>, IIncludableQueryable<Show, object>> include =
                q => q.Include(q => q.Movie).Include(q => q.ShowTimeDetails.OrderBy(d => d.ShowTime)).Include(s => s.Room).ThenInclude(r => r.Cinema);
            var result = await showRepository.GetListByCondition(m => m.MovieId == movieId, include);
            return result;
        }
    }
}
