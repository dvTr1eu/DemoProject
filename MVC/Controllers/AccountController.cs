using System.Security.Claims;
using System.Text.Json;
using Application.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Infrastructure.Services;
using Core.DTOs;
using Infrastructure.Helpers;
using UAParser;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MVC.Controllers
{
    public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IBookingService bookingService,IMapper mapper, SendMailHelper sendMailHelper,IAuthService authService, IUserService userService, INotyfService notyfService) : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
	        var user = await userManager.GetUserAsync(User);
	        var userDto = mapper.Map<UserDto>(user);
            var booking = await bookingService.GetBookingByUserId(user.Id);
            var bookingList = mapper.Map<IEnumerable<BookingDto>>(booking);
            ViewBag.BookingList = bookingList;
            return View(userDto);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.SignUpAsync(request);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("error", result.Errors.ToString());
                    return View(request);
                }
                notyfService.Success("Đăng nhập thành công");
                return RedirectToAction("Login", "Account");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(SignInRequest request, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool succeeded = await authService.SignInAsync(request);
                if (succeeded)
                {
                    bool isAdmin = User.IsInRole("Admin");
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        var urlCheck = returnUrl.Split('/')[0];
                        if (isAdmin && urlCheck == "Admin")
                        {
                            return Redirect(returnUrl);
                        }

                        return Redirect(returnUrl);
                    }
                    return isAdmin ? Redirect("/Admin/AdminHome/Index") : RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "SignIn Failed");
                return View(request);
            }

            return View(request);
        }

        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var (Success, Message, RedirectAction, RedirectController) = await authService.HandleGoogleResponse();
            TempData[Success ? "success" : "error"] = Message;
            return RedirectToAction(RedirectAction, RedirectController);
        }

        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            var user = await userService.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        private async Task<string> GetLocationAsync(HttpContext httpContext)
        {
            try
            {
                // Get the client IP address
                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "127.0.0.1")
                {
                    // Optionally use a service to get the external IP if local:
                    // This can be useful for testing from development environments.
                    using var client = new HttpClient();
                    ipAddress = await client.GetStringAsync("https://api.ipify.org");
                }

                // Call the IP geolocation API
                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync($"http://ip-api.com/json/{ipAddress}");

                // Parse the response
                var locationData = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (locationData != null && locationData.TryGetValue("city", out var city) &&
                    locationData.TryGetValue("regionName", out var region) &&
                    locationData.TryGetValue("country", out var country))
                {
                    return $"{city}, {region}, {country}";
                }

                return "Unknown Location";
            }
            catch
            {
                return "Unknown Location";
            }
        }


        private string GetDeviceInfo(HttpContext httpContext)
        {
            try
            {
                // Retrieve the 'User-Agent' header from the incoming HTTP request.
                // The User-Agent string contains details about the client device's operating system,
                // browser, and other attributes.
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                // Check if the 'User-Agent' string is empty or null. If it is, return "Unknown Device"
                // because without a user agent, we cannot determine the device details.
                if (string.IsNullOrEmpty(userAgent))
                {
                    return "Unknown Device";
                }
                // Use the UAParser library, which provides methods to parse the user agent string.
                // This library identifies the type of device, operating system, and browser from the string.
                var parser = Parser.GetDefault(); // Obtain a parser instance with default settings.
                // Parse the user agent string to extract detailed information about the client's device.
                var clientInfo = parser.Parse(userAgent);
                // Convert the operating system information extracted from the user agent to a string.
                // This typically includes the OS name and version.
                var os = clientInfo.OS.ToString(); // Operating System
                // Convert the browser information extracted from the user agent to a string.
                // This typically includes the browser name and version.
                var browser = clientInfo.UA.ToString(); // Browser
                // Concatenate and return the browser and operating system information in a readable format.
                return $"{browser} on {os}";
            }
            catch
            {
                // If any error occurs during the processing of the user agent string,
                // return "Unknown Device". This catch block ensures that the method returns
                // a valid string even in case of an error.
                return "Unknown Device";
            }
        }

        private async Task SendPasswordChangedNotificationEmail(string email, User user, string location, string device)
        {
            // Create the email subject
            var mailContent = new SendMailHelper.MailContent();
            mailContent.Subject = "Mật khẩu của bạn đã được cập nhật";
            mailContent.ToEmail = email;
            // Craft HTML message body
            mailContent.Body = $@"
<div style=""font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #333; line-height: 1.5; padding: 20px;"">
    <h2 style=""color: #007bff; text-align: center;"">Thông báo thay đổi mật khẩu</h2>
    <p style=""margin-bottom: 20px;"">Hi {user.FullName},</p>
    
    <p>Tôi muốn bạn biết là mật khẩu của tài khoản <strong>MILLION CINEMA</strong> đã được thay đổi .</p>
    
    <div style=""margin: 20px 0; padding: 10px; background-color: #f8f9fa; border: 1px solid #ddd; border-radius: 5px;"">
        <p><strong>Ngày và giờ:</strong> {DateTime.UtcNow:dddd, MMMM dd, yyyy HH:mm} UTC</p>
        <p><strong>Địa điểm:</strong> {location}</p>
        <p><strong>Thiết bị:</strong> {device}</p>
    </div>
    
    <p>Nếu bạn không phải là người thay đổi hãy lập tức liên hệ hỗ trợ để được giải quyết</p>
    
    <p style=""margin-top: 30px;"">Cảm ơn,<br />MILLION CINEMA</p>
</div>";
            // Send the email
            await sendMailHelper.SendMail(mailContent);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var location = await GetLocationAsync(HttpContext);
                    var device = GetDeviceInfo(HttpContext);

                    await SendPasswordChangedNotificationEmail(user.Email, user, location, device);
                    await signInManager.RefreshSignInAsync(user);
                    notyfService.Success("Thay đổi mật khẩu thành công");
                    return RedirectToAction("Index");   
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await authService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
