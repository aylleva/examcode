
using ExamMVC.Areas.Admin.ViewModels;
using ExamMVC.DAL;
using ExamMVC.Models;
using ExamMVC.Services.Implementations;
using ExamMVC.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExamMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDBContext>(opt=>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );

            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDBContext>();


            builder.Services.AddScoped<ILayoutService,LayoutService>(); 

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                "admin",
                "{area:exists}/{controller=home}/{action=index}/{Id?}"
                );
            app.MapControllerRoute(
                  "default",
                  "{controller=home}/{action=index}/{Id?}"
                  );

            app.Run();
        }
    }
}
