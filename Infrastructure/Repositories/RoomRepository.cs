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
    public class RoomRepository(DemoDbContext dbContext) : IRoomRepository
    {
        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            var resutls = await dbContext.Rooms
                .Include(r => r.Cinema)
                .Include(r => r.Seats)
                .Include(r => r.Shows)
                .ToListAsync();
            return resutls;
        }
        public async Task<IEnumerable<Room>> GetRoomsByCinemaIdAsync(int cinemaId)
        {
            var resutls = await dbContext.Rooms
                .Include(r => r.Cinema)
                .Include(r => r.Seats)
                .Include(r => r.Shows)
                .Where(r => r.CinemaId == cinemaId)
                .ToListAsync();
            return resutls;
        }

        public async Task<Room> GetByConditionAsync(Expression<Func<Room, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.Rooms.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dbContext.Rooms.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(Room entity)
        {
            await dbContext.Rooms.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room entity, int id)
        {
            var existingEntity = await dbContext.Rooms
                .Include(r => r.Cinema)
                .Include(r => r.Seats)
                .Include(r => r.Shows).FirstOrDefaultAsync(r => r.Id == id);
            if (existingEntity != null)
            {
                existingEntity.Name = entity.Name;
                existingEntity.SeatCapacity = entity.SeatCapacity;
                existingEntity.CinemaId = entity.CinemaId;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingEntity = await dbContext.Rooms
                .Include(r => r.Cinema)
                .Include(r => r.Seats)
                .Include(r => r.Shows).FirstOrDefaultAsync(r => r.Id == id);
            if (existingEntity != null)
            {
                dbContext.Rooms.Remove(existingEntity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
