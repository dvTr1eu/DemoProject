using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface IShowService : IServiceBase<Show, int>
    {
        Task<(IEnumerable<Show> Shows, int TotalCount)> GetListPagination(int page, int pageSize);
        Task<IEnumerable<Show>> GetShowByShowDayAndMovieId(string day, int movieId);
        Task<IEnumerable<Show>> GetShowByDay(string day);
        Task<Show> GetShowByShowIdAndShowTime(int showId, TimeOnly showTime);
        Task<IEnumerable<Show>> GetShowByMovieId(int movieId);
    }
}
