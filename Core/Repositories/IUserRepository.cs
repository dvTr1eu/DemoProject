using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repositories
{
    public interface IUserRepository : IRepositoryBase<User, int>
    {
        Task<User> UpdateByStringIdAsync(User entity, string id);

    }
}