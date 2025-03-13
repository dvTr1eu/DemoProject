using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleRepository(DemoDbContext dbContext) : IRoleRepository
    {
        async Task<IEnumerable<IdentityRole>> IRoleRepository.GetAllRoleAsync()
        {
            var results = await dbContext.Roles.ToListAsync();
            return results;
        }

        public async Task<IdentityRole> EditRole(string roleId)
        {
            throw new NotImplementedException();
        }
    }
}
