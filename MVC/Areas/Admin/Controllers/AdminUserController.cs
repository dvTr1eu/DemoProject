using Application.Services;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController(IUserService userService, IMapper mapper, UserManager<User> userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllUserAsync();
            var userList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userList.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToList()
                });
            }
            var userDtos = mapper.Map<IEnumerable<UserDto>>(userList);
            return View(userDtos);
        }
    }
}