using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class UserRepository(
        DemoDbContext dbContext,
        ILogger<User> logger,
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager,
        IMapper mapper) : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                var users = await dbContext.Users.ToListAsync();
                foreach (var user in users)
                {
                    var roles = await roleManager.Roles.ToListAsync();
                    user.Roles = roles.ToList();
                }

                return users;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error get all users.");
                throw;
            }
        }

        public async Task<User> GetByConditionAsync(Expression<Func<User, bool>> condition, string[]? includes = null)
        {
            var dataset = dbContext.Users.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dataset = dataset.Include(include);
                }
            }

            return await dataset.FirstOrDefaultAsync(condition);
        }

        public async Task AddAsync(User entity)
        {
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateByStringIdAsync(User entity, string id)
        {
            throw new NotImplementedException();
        }
    }
}