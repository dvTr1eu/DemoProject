using System.Collections;
using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminGenreController(IGenreService genreService, IMapper mapper, INotyfService notyfService)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var genreList = await genreService.GetAll();
            var genreDto = mapper.Map<IEnumerable<GenreDto>>(genreList);
            return View(genreDto);
        }

        [HttpGet]
        public IActionResult CreateGenre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreDto genreDto)
        {
            if (ModelState.IsValid)
            {
                var genre = new Genre
                {
                    Name = genreDto.Name,
                    Description = genreDto.Description
                };

                bool result = await genreService.Create(genre);
                if (result)
                {
                    notyfService.Success("Thêm mới thành công");
                    return RedirectToAction("Index");
                }

                notyfService.Error("Thêm mới thất bại");
            }

            return View(genreDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateGenre(int id)
        {
            if (id != null)
            {
                var genre = await genreService.FindById(id);
                if (genre != null)
                {
                    var genreDto = mapper.Map<GenreDto>(genre);
                    return View(genreDto);
                }

                return NotFound();
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDto genreDto, int id)
        {
            if (ModelState.IsValid)
            {
                var genreUpdate = new Genre
                {
                    Name = genreDto.Name,
                    Description = genreDto.Description,
                };
                bool result = await genreService.Edit(genreUpdate, id);
                if (result)
                {
                    notyfService.Success("Cập nhật thành công");
                    return RedirectToAction("Index");
                }

                notyfService.Error("Cập nhật thất bại");
            }

            return View(genreDto);
        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (ModelState.IsValid)
            {
                bool result = await genreService.Delete(id);
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