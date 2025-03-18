using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SeatRepository(DemoDbContext dbContext) : ISeatRepository
    {
        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            var results = await dbContext.Seats
                .Include(s => s.Room)
                .ToListAsync();
            return results;
        }

        public async Task<Seat> GetByConditionAsync(Expression<Func<Seat, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.Seats.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dbContext.Seats.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(Seat entity)
        {
            await dbContext.Seats.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Seat> seats)
        {
            await dbContext.Seats.AddRangeAsync(seats);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Seat>> GetListByCondition(Expression<Func<Seat, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.Seats.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dbContext.Seats.Where(condition).ToListAsync();
        }

        public async Task<List<Seat>> GetBookedSeatsByShowIdAndShowTime(int showId, TimeOnly showTime)
        {
            var results = await dbContext.BookingSeats
                .Where(bs => bs.Booking.ShowId == showId && bs.ShowtimeDetail.ShowTime == showTime)
                .Select(bs => bs.Seat)
                .ToListAsync();
            return results;
        }

        public async Task<Seat?> FindByRoomAndPosition(int roomId, char seatRow, int seatNumber)
        {
            return await dbContext.Seats
                .FirstOrDefaultAsync(s => s.RoomId == roomId && s.SeatRow == seatRow && s.SeatNumber == seatNumber);
        }

        public async Task<bool> LockSeats(int roomId, int showId, string userId, List<string> seatCodes)
        {
            foreach (var seat in seatCodes)
            {
                var existingLock = await dbContext.SeatLocks.FirstOrDefaultAsync(s =>
                    s.SeatCode == seat && s.ShowId == showId && s.IsLocked);

                if (existingLock != null)
                {
                    return false;
                }

                dbContext.SeatLocks.Add(new SeatLock
                {
                    SeatCode = seat,
                    RoomId = roomId,
                    ShowId = showId,
                    UserId = userId,
                    LockTime = DateTime.Now,
                    IsLocked = true
                });
            }

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task ReleaseLockedSeats(List<string> seatCodes, int showId)
        {
            var lockedSeats = await dbContext.SeatLocks
                .Where(s => seatCodes.Contains(s.SeatCode) && s.ShowId == showId)
                .ToListAsync();

            dbContext.SeatLocks.RemoveRange(lockedSeats);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seat entity, int id)
        {
            var existingEntity = await dbContext.Seats
                .Include(s => s.Room).FirstOrDefaultAsync(s => s.Id == id);
            if (existingEntity != null)
            {
                existingEntity.RoomId = entity.RoomId;
                existingEntity.SeatNumber = entity.SeatNumber;
                existingEntity.SeatType = entity.SeatType;
                existingEntity.Status = entity.Status;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingEntity = await dbContext.Seats
                .Include(s => s.Room).FirstOrDefaultAsync(s => s.Id == id);
            if (existingEntity != null)
            {
                dbContext.Seats.Remove(existingEntity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}