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
    public class ShowtimeDetailService(IShowtimeDetailRepository showtimeDetail) : IShowtimeDetailService
    {
        public async Task AddShowTime(List<TimeOnly> times, int showId)
        {
            try
            {
                await showtimeDetail.AddShowTime(times, showId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ShowtimeDetail?> FindByShowTime(TimeOnly showTime)
        {
            try
            {
                var result =
                    await showtimeDetail.GetByConditionAsync(st => st.ShowTime == showTime,
                        new[] { "Show"});
                return result;
            }
            catch 
            {
                return null;
            }
        }

        public async Task<IEnumerable<ShowtimeDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ShowtimeDetail?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(ShowtimeDetail entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Edit(ShowtimeDetail entity, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
