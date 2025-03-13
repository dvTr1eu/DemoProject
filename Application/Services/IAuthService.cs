using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> SignUpAsync(SignUpRequest model);
        Task<bool> SignInAsync(SignInRequest model);
        Task SignOutAsync();
        Task<(bool Success, string Message, string RedirectAction, string RedirectController)> HandleGoogleResponse();

    }
}
