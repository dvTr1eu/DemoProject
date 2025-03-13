using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminRoomController(IRoomService roomService,ICinemaService cinemaService, ISeatService seatService, IMapper mapper, INotyfService notyfService) : Controller
    {
        //public async Task<IActionResult> Index(int cinemaId)
        //{
        //    var entityRooms = await roomService.GetRoomsByCinemaIdAsync(cinemaId);
        //    return View(entityRooms);
        //}

        public async Task<IActionResult> Index()
        {
            var entityRooms = await roomService.GetAll();
            return View(entityRooms);
        }

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> CreateRoom()
        {
            var cinemaList = await cinemaService.GetAll();
            var cinemaDto = mapper.Map<IEnumerable<CinemaDto>>(cinemaList);
            ViewBag.CinemaList = cinemaDto;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomDto model)
        {
            if (ModelState.IsValid)
            {
                var roomAdd = mapper.Map<Room>(model);
                roomAdd.SeatCapacity = 96;
                bool result = await roomService.Create(roomAdd);
                if (result)
                {
                    notyfService.Success("Thêm mới thành công");
                    return RedirectToAction("Index");
                }
                notyfService.Error("Thêm mới thất bại");
            }

            return View(model);
        }

        // GET: AdminRoomController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: AdminRoomController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: AdminRoomController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await roomService.Delete(id);
                if (result)
                {
                    notyfService.Success("Xóa thành công");
                    return RedirectToAction("Index");
                }
                notyfService.Error("Xóa thất bại");
                return NotFound();
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
