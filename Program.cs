using FinalProjectShortenURL.Data;
using FinalProjectShortenURL.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectShortenURL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication("CookieAuth")
      .AddCookie("CookieAuth", options =>
      {
          options.LoginPath = "/Access/Login";
          options.Cookie.Name = "CookieAuth";
          options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
      });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DataDbContext>(x =>
          x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
          );

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

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}