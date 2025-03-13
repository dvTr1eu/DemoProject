using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repositories
{
    public interface IRoomRepository : IRepositoryBase<Room, int>
    {
        Task<IEnumerable<Room>> GetRoomsByCinemaIdAsync(int cinemaId);
    }
}
