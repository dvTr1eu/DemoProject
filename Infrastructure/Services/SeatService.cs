using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class SeatService(ISeatRepository seatRepository) : ISeatService
    {
        public async Task<IEnumerable<Seat>> GetAll()
        {
            var results = await seatRepository.GetAllAsync();
            return results;
        }

        public async Task<Seat?> FindById(int id)
        {
            var roomItem = await seatRepository.GetByConditionAsync(g => g.Id == id, new[] { "BookingSeats", "Rooms" });
            return roomItem;
        }

        public async Task<bool> Create(Seat entity)
        {
            try
            {
                await seatRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateRange(List<Seat> seats)
        {
            try
            {
                await seatRepository.AddRangeAsync(seats);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Seat> CreateSeat(Seat entity)
        {
            try
            {
                await seatRepository.AddAsync(entity);
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Seat>> GetSeatByRoomId(int roomId)
        {
            var results = await seatRepository.GetListByCondition(s => s.RoomId == roomId, new []{"Rooms", "BookingSeats"});
            return results;
        }

        public async Task<List<Seat>> GetBookedSeatsByShowIdAndShowTime(int showId, TimeOnly showTime)
        {
            try
            {
                var result = await seatRepository.GetBookedSeatsByShowIdAndShowTime(showId, showTime);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Seat?> FindByRoomAndPosition(int roomId, char seatRow, int seatNumber)
        {
            try
            {
                var result = await seatRepository.FindByRoomAndPosition(roomId, seatRow, seatNumber);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Edit(Seat entity, int id)
        {
            try
            {
                await seatRepository.UpdateAsync(entity, id);
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
                await seatRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}