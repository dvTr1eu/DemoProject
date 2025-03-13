using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminShowController(IShowService showService, IMapper mapper,IMovieService movieService,IShowtimeDetailService showtimeDetail, ICinemaService cinemaService,IRoomService roomService, INotyfService notyfService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var shows = await showService.GetAll();
            return View(shows);
        }

        [HttpGet]
        public async Task<IActionResult> CreateShow()
        {
            var movieList = await movieService.GetAll();
            var cinemaList = await cinemaService.GetAll();
            ViewBag.MovieList = mapper.Map<IEnumerable<MovieDto>>(movieList);
            ViewBag.CinemaList = mapper.Map<IEnumerable<CinemaDto>>(cinemaList);
            ViewBag.RoomList = new List<RoomDto>();
            return View();
        }

        public async Task<IActionResult> GetRoomsByCinema(int cinemaId)
        {
            var rooms = await roomService.GetRoomsByCinemaIdAsync(cinemaId);

            var roomList = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();

            return Json(roomList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShow(ShowDto model)
        {
            if (ModelState.IsValid)
            {
                var addShow = mapper.Map<Show>(model);
                var result = await showService.Create(addShow);
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
        public async Task<IActionResult> EditShow(int id)
        {
            var existingShow = await showService.FindById(id);
            var showDto = mapper.Map<ShowDto>(existingShow);
            var movieList = await movieService.GetAll();
            var cinemaList = await cinemaService.GetAll();
            ViewBag.MovieList = mapper.Map<IEnumerable<MovieDto>>(movieList);
            ViewBag.CinemaList = mapper.Map<IEnumerable<CinemaDto>>(cinemaList);
            ViewBag.RoomList = new List<RoomDto>();
            return View(showDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditShow(ShowDto model, int id)
        {
            if (ModelState.IsValid)
            {
                var editShow = mapper.Map<Show>(model);
                var result = await showService.Edit(editShow,id);
                if (result)
                {
                    notyfService.Success("Sửa thành công");
                    return RedirectToAction("Index");
                }
                notyfService.Error("Sửa thất bại");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteShow(int id)
        {
            if (id != null)
            {
                var result = await showService.Delete(id);
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
