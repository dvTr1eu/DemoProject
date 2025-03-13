using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Helpers
{
    public static class IdentityResultExtensions
    {
        public static AuthResponse ToAuthResult(this IdentityResult result)
        {
            return new AuthResponse()
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.ToDictionary(e => e.Code, e => e.Description)
            };
        }
    }
}
