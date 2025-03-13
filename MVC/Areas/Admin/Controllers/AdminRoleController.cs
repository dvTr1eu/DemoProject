using Application.Services;
using Core.DTOs;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminRoleController(IRoleService roleService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await roleService.GetAllRoleAsync();
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto model)
        {
            if(ModelState.IsValid) {
                var result = await roleService.CreateRoleAsync(model.RoleName);
                if (result)
                {
                    return RedirectToAction("Index", "AdminRole");
                }
                ModelState.AddModelError(string.Empty, "Tạo thất bại");
                return View(model);
            }
            return View(model);
        }
    }
}
