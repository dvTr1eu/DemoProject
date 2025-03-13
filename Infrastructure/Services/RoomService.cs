using Application.Services;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Services
{
    public class RoomService(IRoomRepository roomRepository) : IRoomService
    {
        public async Task<IEnumerable<Room>> GetAll()
        {
            var results = await roomRepository.GetAllAsync();
            return results;
        }

        public async Task<IEnumerable<Room>> GetRoomsByCinemaIdAsync(int cinemaId)
        {
            var results = await roomRepository.GetRoomsByCinemaIdAsync(cinemaId);
            return results;
        }

        public async Task<Room?> FindById(int id)
        {
            var roomItem = await roomRepository.GetByConditionAsync(g => g.Id == id, new[] { "Cinemas","Seats","Showtimes" });
            return roomItem;
        }

        public async Task<bool> Create(Room entity)
        {
            try
            { 
                await roomRepository.AddAsync(entity);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> Edit(Room entity, int id)
        {
            try
            {
                await roomRepository.UpdateAsync(entity, id);
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
                await roomRepository.DeleteAsync(id);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}