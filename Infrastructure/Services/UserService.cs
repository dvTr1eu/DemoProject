using Application.Services;
using AutoMapper;
using Core.Entities;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class UserService(UserManager<User> userManager, IMapper mapper, IUserRepository userRepository) : IUserService
    {
        public async Task<User> FindByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            var users = await userRepository.GetAllAsync();
            return users;
        }
    }
}
