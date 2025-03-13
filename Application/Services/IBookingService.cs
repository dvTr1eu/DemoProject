using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface IBookingService : IServiceBase<Booking, int>
    { 
        Task<Booking> CreateBooking(Booking entity);
        Task<IEnumerable<Booking>> GetBookingByUserId(string userId);
    }
}
