using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Helpers;
using Infrastructure.Migrations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Core.DTOs.VnPayModel;

namespace MVC.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BookingController(
        IBookingService bookingService, 
        IMapper mapper,UserManager<User> userManager, 
        INotyfService notyfService, 
        IShowService showService,
        IVnPayService vnPayService, 
        IBookingSeatService bookingSeatService, 
        ISeatService seatService,
        IShowtimeDetailService showtimeDetailService,
        IPaymentService paymentService,
        SendMailHelper sendMailHelper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> CreateBook(int showId, TimeOnly showTime)
        {
            var show = await showService.GetShowByShowIdAndShowTime(showId, showTime);
            var selectedShowTime = show.ShowTimeDetails.FirstOrDefault(st => st.ShowTime == showTime);
            var seat = await seatService.GetSeatByRoomId(show.RoomId);
            var bookedSeats = await seatService.GetBookedSeatsByShowIdAndShowTime(showId, showTime);
            var model = new BookingViewDto
            {
                MovieTitle = show.Movie.Title,
                ShowDay = show.ShowDay.ToString("dd/MM/yyyy"),
                ShowTime = selectedShowTime.ShowTime,
                RoomName = show.Room.Name,
                SeatQuantity = show.Room.SeatCapacity,
                TicketPrice = show.TicketPrice,
                RoomId = show.RoomId,
                ShowId = showId,
                CinemaName = show.Cinema.Name,
                Seats = seat.ToList(),
                BookedSeats = bookedSeats
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(int roomId,int showId, TimeOnly showTime, int totalAmount, string selectedSeats)
        {
            var show = await showService.FindById(showId);
            var bookingData = new
            {
                UserId = userManager.GetUserId(User),
                ShowId = showId,
                RoomId = roomId,
                BookingTime = DateTime.Now,
                TotalAmount = totalAmount,
                SelectedSeats = selectedSeats,
                MovieTitle = show.Movie.Title,
                ShowDay = show.ShowDay.ToString("dd/MM/yyyy"),
                ShowTime = showTime.ToString("HH:mm"),
                RoomName = show.Room.Name,
                CinemaName = show.Cinema.Name
            };

            string jsonBooking = JsonConvert.SerializeObject(bookingData);
            HttpContext.Session.SetString("PendingBooking", jsonBooking);

            var vnPayModel = new VnPaymentRequestVM
                {
                    Amount = totalAmount,
                    CreatedDate = DateTime.Now,
                    Description = "Thanh toán vé xem phim",
                };

            return Redirect(vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
        }

        public async Task<IActionResult> PaymentCallBack()
        {
            var response = vnPayService.PaymentExecute(Request.Query);
            if (response != null && response.VnPayResponseCode == "00")
            {
                string jsonBooking = HttpContext.Session.GetString("PendingBooking");
                if (!string.IsNullOrEmpty(jsonBooking))
                {
                    var bookingData = JsonConvert.DeserializeObject<dynamic>(jsonBooking);

                    var booking = new Booking
                    {
                        UserId = bookingData.UserId,
                        ShowId = bookingData.ShowId,
                        BookingTime = bookingData.BookingTime,
                        TotalAmount = bookingData.TotalAmount,
                    };
                    await bookingService.CreateBooking(booking);

                    var payment = new PaymentDto
                    {
                        BookingId = booking.Id,
                        PaymentMethod = "VnPay",
                        Amount = booking.TotalAmount,
                        PaymentDate = DateTime.Now,
                        Status = true
                    };
                    var paymentAdd = mapper.Map<Payment>(payment);
                    await paymentService.CreatePayment(paymentAdd);

                    var seatList = ((string)bookingData.SelectedSeats).Split(',').Select(s => s.Trim()).ToList();

                    var showTimeDetails = await showtimeDetailService.FindByShowTime(TimeOnly.Parse(bookingData.ShowTime.ToString()));
                    foreach (var seat in seatList)
                    {
                        string seatRow = seat.Substring(0, 1);
                        int seatNumber = int.Parse(seat.Substring(1));

                        var newSeat = new Seat
                        {
                            RoomId = bookingData.RoomId,
                            SeatRow = seatRow[0],
                            SeatNumber = seatNumber,
                            Status = true,
                            SeatType = "Thường"
                        };
                        await seatService.CreateSeat(newSeat);

                        var bookingSeat = new BookingSeat
                        {
                            BookId = booking.Id,
                            SeatId = newSeat.Id,
                            ShowTimeId = showTimeDetails?.Id ?? 0
                        };

                        await bookingSeatService.Create(bookingSeat);
                    }
                }

                return RedirectToAction("Success");
            }
            TempData["Message"] = $"Lỗi thanh toán VnPay: {response.VnPayResponseCode}";
            return RedirectToAction("AccessDenied", "Account");
        }

        public IActionResult PaymentFail()
        {
            ViewBag.Message = TempData["Message"] ?? "Thanh toán thất bại. Vui lòng thử lại hoặc chọn phương thức thanh toán khác.";
            return View();
        }

        public async Task<IActionResult> Success()
        {
            try
            {
                string jsonBooking = HttpContext.Session.GetString("PendingBooking");
                var bookingModel = JsonConvert.DeserializeObject<dynamic>(jsonBooking);
                var user = await userManager.GetUserAsync(User);
                var mailContent = new SendMailHelper.MailContent();

                mailContent.ToEmail = $"{user.Email}";
                mailContent.Subject = "XÁC NHẬN ĐẶT VÉ THÀNH CÔNG";
                mailContent.Body = $"<h2>Xin chào, {user.FullName}</h2><br/>" +
                                   $"<p>Vé xem phim của bạn đã được thanh toán thành công.</p><br/>" +
                                   $"<p>THÔNG TIN VÉ</p><br/>" +
                                   $"<p>Tên phim: {bookingModel.MovieTitle}</p>" +
                                   $"<p>Rạp: {bookingModel.CinemaName}</p>" +
                                   $"<p>Phòng chiếu: {bookingModel.RoomName}</p>" +
                                   $"<p>Suất chiếu: {bookingModel.ShowDay} - {bookingModel.ShowTime}</p>" +
                                   $"<p>Ghế: {bookingModel.SelectedSeats}</p>" +
                                   $"<p>Tổng tiền: {bookingModel.TotalAmount.ToString("##,###")} VNĐ</p>";

                var sendMail = await sendMailHelper.SendMail(mailContent);
                ViewBag.SendMail = sendMail;
                HttpContext.Session.Remove("PendingBooking");
                return View(bookingModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
