using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Services
{
    public class BookingService(IBookingRepository bookingRepository) : IBookingService
    {
        public async Task<IEnumerable<Booking>> GetAll()
        {
            try
            {
                var results = await bookingRepository.GetAllAsync();
                return results;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Booking?> FindById(int id)
        {
            try
            {
                Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> include =
                    q => q.Include(b => b.User)
                        .Include(b => b.BookingSeats)
                            .ThenInclude(bs => bs.Seat)
                        .Include(b => b.Show)
                            .ThenInclude(b => b.ShowTimeDetails);

                var result = await bookingRepository.GetByNewConditionAsync(b => b.Id == id, include);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Create(Booking entity)
        {
            try
            {
                await bookingRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Booking entity, int id)
        {
            try
            {
                await bookingRepository.UpdateAsync(entity,id);
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
                await bookingRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Booking> CreateBooking(Booking entity)
        {
            try
            {
                await bookingRepository.AddAsync(entity);
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingByUserId(string userId)
        {
	        try
	        {
				Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> include =
					q => q.Include(b => b.User)
                        .Include(b => b.Payment)
						.Include(b => b.BookingSeats)
                            .ThenInclude(bk => bk.Seat)
						.Include(b => b.Show)
						    .ThenInclude(b => b.ShowTimeDetails)
                        .Include(b => b.Show)
                            .ThenInclude(s => s.Movie)
                        .Include(b => b.Show)
                            .ThenInclude(b => b.Cinema)
                        .Include(b => b.Show)
                            .ThenInclude(s => s.Room);
				var results = await bookingRepository.GetListByConditionAsync(b => b.UserId == userId, include);
                return results;
            }
	        catch
	        {
		        return null;
	        }
        }
    }
}
