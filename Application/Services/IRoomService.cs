using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface IRoomService : IServiceBase<Room,int>
    {
        Task<IEnumerable<Room>> GetRoomsByCinemaIdAsync(int cinemaId);
    }
}
