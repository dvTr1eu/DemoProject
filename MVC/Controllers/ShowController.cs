using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ShowController(IShowService showService, IMapper mapper, INotyfService notyfService, ICinemaService cinemaService) : Controller
    {
        //public async Task<IActionResult> ModalBookShow()
        //{
        //    var show = await showService.GetAll();
        //    var showMapper = mapper.Map<ShowDto>(show);
        //    return PartialView("_ModalBookShowPartial",showMapper);
        //}
    }
}
