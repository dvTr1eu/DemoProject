using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repositories
{
    public interface ISeatRepository : IRepositoryBase<Seat, int>
    {
        Task AddRangeAsync(List<Seat> seats);
        Task<IEnumerable<Seat>> GetListByCondition(Expression<Func<Seat, bool>> condition, string[]? includes = null);
        Task<List<Seat>> GetBookedSeatsByShowIdAndShowTime(int showId, TimeOnly showTime);
        Task<Seat?> FindByRoomAndPosition(int roomId, char seatRow, int seatNumber);
        Task<bool> LockSeats(int roomId, int showId, string userId, List<string> seatCodes);
        Task ReleaseLockedSeats(List<string> seatCodes, int showId);
    }
}
