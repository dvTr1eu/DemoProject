using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface IShowtimeDetailService : IServiceBase<ShowtimeDetail,int>
    {
        Task AddShowTime(List<TimeOnly> times, int showId);
        Task<ShowtimeDetail?> FindByShowTime(TimeOnly showTime);
    }
}
