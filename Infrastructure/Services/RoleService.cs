using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager, IRoleRepository roleRepository, IMapper mapper)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<bool> CheckRoleExistsAsync(string roleName)
        {
            var result = await _roleManager.RoleExistsAsync(roleName);
            return result;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleName);
            if (existingRole == null)
            {

                var identityRole = new IdentityRole
                {
                    Name = roleName,
                };
                var result = await _roleManager.CreateAsync(identityRole);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRoleAsync()
        {
            var results = await _roleRepository.GetAllRoleAsync();
            return results;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRoleAsync2()
        {
            var results = await _roleRepository.GetAllRoleAsync();

            return _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleDto>>(results);
        }
    }
}
