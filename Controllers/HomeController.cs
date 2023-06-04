using FinalProjectShortenURL.Data;
using FinalProjectShortenURL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace FinalProjectShortenURL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataDbContext _dbcontext;

        public HomeController(ILogger<HomeController> logger,DataDbContext dbContext)
        {
            _logger = logger;
            _dbcontext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Links()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue("UserId");
            var tempUser = _dbcontext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (tempUser != null)
            {
                var data = _dbcontext.Urls.Where(u => u.UrlUserId.Equals(tempUser.Id)).ToList();
                return View(data);
            }
            else
            {
                var data2 = _dbcontext.Urls.Where(u => u.UrlUserId == null).ToList();
                return View(data2);
            }
        }

        [Authorize]
        public IActionResult IPList(string shortUrl)
        {
            ViewData["shorturl"] = shortUrl;
            var entries = _dbcontext.Urls.Include(l => l.Entries)
                 .FirstOrDefault(l => l.ShortURL == shortUrl)
                 ?.Entries;
            return View(entries!.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}