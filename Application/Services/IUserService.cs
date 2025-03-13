using Core.Entities;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUserAsync();
    }
}
