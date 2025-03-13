using Application.Services;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Services
{
    public class GenreService(IGenreRepository genreRepository) : IGenreService
    {
        public async Task<IEnumerable<Genre>> GetAll()
        {
            var results = await genreRepository.GetAllAsync();
            return results;
        }

        public async Task<Genre?> FindById(int id)
        {
            var genreItem = await genreRepository.GetByConditionAsync(g => g.Id == id, new[] { "MovieTypes" });
            return genreItem;
        }

        public async Task<bool> Create(Genre entity)
        {
            try
            {
                await genreRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Genre entity, int id)
        {
            try
            {
                await genreRepository.UpdateAsync(entity, id);
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
                await genreRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}