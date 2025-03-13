using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Services
{
    public class BookingSeatService(IBookingSeatRepository bookingSeatRepository) : IBookingSeatService
    {
        public async Task<IEnumerable<BookingSeat>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<BookingSeat?> FindById(int id)
        {
            var result = await bookingSeatRepository.GetByConditionAsync(bs => bs.BookId == id, new []{"Seats","Bookings"});
            return result;
        }

        public async Task<bool> Create(BookingSeat entity)
        {
            try
            {
                await bookingSeatRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(BookingSeat entity, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
