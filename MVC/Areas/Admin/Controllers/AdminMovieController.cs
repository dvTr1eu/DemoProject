using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MVC.Helpers;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminMovieController(
        IMovieService movieService,
        IMapper mapper,
        INotyfService notyfService,
        IGenreService genreService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movieList = await movieService.GetAll();
            var movieDtos = mapper.Map<IEnumerable<MovieDto>>(movieList);
            ViewBag.GenreName = movieList.ToDictionary(
                m => m.Id,
                m => m.MovieTypes.Select(mt => mt.Genre.Name).ToList()
            );
            return View(movieDtos);
        }

        [HttpGet]
        public async Task<IActionResult> CreateMovie()
        {
            var genreList = await genreService.GetAll();
            var genreDto = mapper.Map<IEnumerable<GenreDto>>(genreList);
            ViewBag.Genres = genreDto;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieDto model, IFormFile inputFile)
        {
            if (ModelState.IsValid)
            {
                var posterUrl = await Utilities.UploadImageCloud(inputFile);
                var movieAdd = mapper.Map<Movie>(model);
                movieAdd.Poster = posterUrl;
                if (model.GenreId != null)
                {
                    foreach (var genreId in model.GenreId)
                    {
                        movieAdd.MovieTypes.Add(new MovieType { GenreId = genreId });
                    }
                }

                bool result = await movieService.Create(movieAdd);
                if (result)
                {
                    notyfService.Success("Thêm mới thành công");
                    return RedirectToAction("Index");
                }

                notyfService.Error("Thêm mới thất bại");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMovie(int id)
        {
            var model = await movieService.FindById(id);
            var modelDto = mapper.Map<MovieDto>(model);
            var genreList = await genreService.GetAll();
            var genreDto = mapper.Map<IEnumerable<GenreDto>>(genreList);
            ViewBag.Genres = genreDto;
            if (model == null)
            {
                return NotFound();
            }

            return View(modelDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovie(MovieDto model, int id, IFormFile? inputFile)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var posterUrl = inputFile == null ? model.Poster : await Utilities.UploadImageCloud(inputFile);
                var movieUpdate = mapper.Map<Movie>(model);
                movieUpdate.Poster = posterUrl;
                if (model.GenreId != null)
                {
                    foreach (var genreId in model.GenreId)
                    {
                        movieUpdate.MovieTypes.Add(new MovieType
                        {
                            MovieId = movieUpdate.Id,
                            GenreId = genreId
                        });
                    }
                }

                bool result = await movieService.Edit(movieUpdate, id);
                if (result)
                {
                    notyfService.Success("Cập nhật thành công");
                    return RedirectToAction("Index");
                }

                notyfService.Error("Cập nhật thất bại");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (id != null)
            {
                bool result = await movieService.Delete(id);
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