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
