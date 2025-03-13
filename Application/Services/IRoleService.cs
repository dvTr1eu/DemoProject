using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IRoleService
    {
        Task<bool> CheckRoleExistsAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
        Task<IEnumerable<IdentityRole>> GetAllRoleAsync();
        Task<IEnumerable<RoleDto>> GetAllRoleAsync2();
    }
}
