using System.Configuration;
using System.Text;
using Application.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using CloudinaryDotNet;
using Core.Entities;
using Core.Repositories;
using Infrastructure.Helpers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVC.Helpers;

namespace MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var services = builder.Services;


            // Add services to the container.
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc();


            // Configure Identity
            var connectionString = builder.Configuration.GetConnectionString("DemoProject") ?? throw new
                InvalidOperationException("Connection string 'DemoProject' not found.");
            services.AddDbContext<DemoDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 1;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<DemoDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //service
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ICinemaService, CinemaService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IShowService, ShowService>();
            services.AddScoped<IShowtimeDetailService, ShowtimeDetailService>();
            services.AddScoped<IBookingSeatService, BookingSeatService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();

            //repositories
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IShowRepository, ShowRepository>();
            services.AddScoped<IShowtimeDetailRepository, ShowtimeDetailRepository>();
            services.AddScoped<IBookingSeatRepository, BookingSeatRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<SendMailHelper>();

            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

            services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySettings"));

            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IOptions<CloudinarySetting>>().Value;
                return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, ops =>
                {
                    ops.LoginPath = "/Account/Login";
                    ops.AccessDeniedPath = "/Account/AccessDenied";
                    ops.LogoutPath = "/Account/Logout";
                })
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    var googleConfig = builder.Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleConfig["ClientId"];
                    options.ClientSecret = googleConfig["ClientSecret"];
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNotyf();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    var roles = new[] { "Admin", "Customer" };
            //    foreach (var role in roles)
            //    {
            //        if (!await roleManager.RoleExistsAsync(role))
            //        {
            //            await roleManager.CreateAsync(new IdentityRole(role));
            //        }
            //    }
            //}

            //using (var scope = app.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            //    string email = "admin@gmail.com";
            //    string password = "Admin@123";

            //    if (await userManager.FindByEmailAsync(email) == null)
            //    {
            //        var user = new User
            //        {
            //            UserName = email,
            //            Email = email,
            //            FullName = "Admin",
            //            CreateDate = DateTime.Now
            //        };
            //        await userManager.CreateAsync(user, password);

            //        await userManager.AddToRoleAsync(user, "Admin");
            //    }
            //}

            app.Run();
        }
    }
}
