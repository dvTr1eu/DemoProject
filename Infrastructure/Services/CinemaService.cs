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
    public class CinemaService(ICinemaRepository cinemaRepository) : ICinemaService
    {
        public async Task<IEnumerable<Cinema>> GetAll()
        {
            var results = await cinemaRepository.GetAllAsync();
            return results;
        }

        public async Task<Cinema?> FindById(int id)
        {
            var result = await cinemaRepository.GetByConditionAsync(c => c.Id == id, new[] { "Rooms" });
            return result;
        }

        public async Task<bool> Create(Cinema entity)
        {
            try
            {
                await cinemaRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Cinema entity, int id)
        {
            try
            {
                await cinemaRepository.UpdateAsync(entity, id);
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
                await cinemaRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}