using Application.Services;
using AutoMapper;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminBookingController(IBookingService bookingService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var booking = await bookingService.GetAll();
            var bookingList = mapper.Map<IEnumerable<BookingDto>>(booking);
            return View(bookingList);
        }
    }
}
