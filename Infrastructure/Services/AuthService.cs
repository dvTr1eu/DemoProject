using System.Security.Claims;
using Application.Services;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class AuthService(SignInManager<User> signInManager, UserManager<User> userManager,RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        public async Task<IdentityResult> SignUpAsync(SignUpRequest model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,  
                PhoneNumber = model.PhoneNumber,
                DateBirth = model.DateBirth,
                CreateDate = DateTime.Now
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(AppRole.Customer))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }

                await userManager.AddToRoleAsync(user, AppRole.Customer);
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }

        public async Task<bool> SignInAsync(SignInRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (user == null && !passwordValid)
            {
                return false;
            }

            var result = await signInManager.PasswordSignInAsync(model.Email,
                model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName)
                };
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                await signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                return true;
            }

            return false;
        }


        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<(bool Success, string Message, string RedirectAction, string RedirectController)> HandleGoogleResponse()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var result = await httpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return (false, "Đăng nhập thất bại", "SignIn", "Account");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return (false, "Không thể lấy thông tin email từ Google", "SignIn", "Account");
            }

            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(null, "123456789");


                var newUser = new User { UserName = email, Email = email, FullName = name, CreateDate = DateTime.Now};
                newUser.PasswordHash = hashedPassword;

                var createUserResult = await userManager.CreateAsync(newUser);
                await userManager.AddToRoleAsync(newUser, AppRole.Customer);

                if (!createUserResult.Succeeded)
                {
                    return (false, "Đăng ký tài khoản thất bại. Vui lòng thử lại", "SignUp", "Account");
                }

                await signInManager.SignInAsync(newUser, isPersistent: false);
                return (true, "Đăng ký tài khoản thành công", "Index", "Home");
            }
            else
            {
                await signInManager.SignInAsync(existingUser, isPersistent: false);
                return (true, "Đăng nhập thành công", "Index", "Home");
            }
        }
    }
}
