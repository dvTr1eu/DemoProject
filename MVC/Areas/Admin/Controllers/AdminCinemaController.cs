using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCinemaController(ICinemaService cinemaService, IMapper mapper, INotyfService notyfService)
        : Controller
    {
        public async Task<IActionResult> Index()
        {
            var cinemas = await cinemaService.GetAll();
            var cinemaList = mapper.Map<IEnumerable<CinemaDto>>(cinemas);
            return View(cinemaList);
        }

        [HttpGet]
        public IActionResult CreateCinema()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCinema(CinemaDto model)
        {
            if (ModelState.IsValid)
            {
                var addCinema = mapper.Map<Cinema>(model);
                bool result = await cinemaService.Create(addCinema);
                if (result)
                {
                    notyfService.Success("Thêm mới thành công");
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Tạo mới thất bại.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCinema(int id)
        {
            var existingCinema = await cinemaService.FindById(id);
            if (existingCinema == null)
            {
                return NotFound();
            }

            var cinemaDto = mapper.Map<CinemaDto>(existingCinema);
            return View(cinemaDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCinema(CinemaDto model, int id)
        {
            if (ModelState.IsValid)
            {
                var cinemaUpdate = mapper.Map<Cinema>(model);
                bool result = await cinemaService.Edit(cinemaUpdate, id);
                if (result)
                {
                    notyfService.Success("Cập nhật thành công");
                    return RedirectToAction("Index");
                }
                notyfService.Error("Cập nhật thông tin thất bại");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteCinema(int id)
        {
            if (id != null)
            {
                bool result = await cinemaService.Delete(id);
                if (result)
                {
                    notyfService.Success("Xóa thành công");
                    return RedirectToAction("Index");
                }
                notyfService.Error("Xóa thất bại");
            }
            return NotFound();
        }
    }
}