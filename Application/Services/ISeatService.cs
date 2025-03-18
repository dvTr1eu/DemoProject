using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface ISeatService : IServiceBase<Seat,int>
    {
        Task<bool> CreateRange(List<Seat> seats);
        Task<Seat> CreateSeat(Seat entity);
        Task<IEnumerable<Seat>> GetSeatByRoomId(int roomId);
        Task<List<Seat>> GetBookedSeatsByShowIdAndShowTime(int showId, TimeOnly showTime);
        Task<Seat?> FindByRoomAndPosition(int roomId, char seatRow, int seatNumber);
        Task<bool> LockSeats(int roomId, int showId, string userId, List<string> seatCodes);
        Task ReleaseLockedSeats(List<string> seatCodes, int showId);
    }
}
