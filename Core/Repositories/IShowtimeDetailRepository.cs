using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repositories
{
    public interface IShowtimeDetailRepository : IRepositoryBase<ShowtimeDetail, int>
    {
        Task AddShowTime(List<TimeOnly> times, int showId);
    }
}
