using FinalProjectShortenURL.Data;
using FinalProjectShortenURL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProjectShortenURL.Controllers
{
    public class AccessController : Controller
    {
        private readonly DataDbContext _dbContext;
        public AccessController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegister modelReg)
        {
            if (ModelState.IsValid)
            {
                var model = new UserModel { Email = modelReg.Email, FirstName = modelReg.FirstName, LastName = modelReg.LastName, Password = modelReg.Password };
                if (!_dbContext.Users.Any(x => x.Email.Equals(model.Email)))
                {
                    _dbContext.Users.Add(model);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Login", "Access");
                }
            ViewData["ValidateMessageRegister"] = "Email already in use !";
            }
            return View(modelReg);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserDto model)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (currentUser == null)
            {
                ViewData["ValidateMessage"] = "Invalid Email or Password !";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                     new Claim("UserId", currentUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, currentUser.Email),
                    new Claim(ClaimTypes.Name, currentUser.FirstName),
                    new Claim("Password", currentUser.Password)

                };
            ClaimsIdentity identity = new ClaimsIdentity(claims,
               "CookieAuth");

            AuthenticationProperties p = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = model.RememberMe
            };
            var cp = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(cp, p);
            return RedirectToAction("Index", "Home");

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue("UserId");
            var tempUserdb = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            var tempUser = new UserManagerModel { Id = tempUserdb.Id, Email = tempUserdb.Email, FirstName = tempUserdb.FirstName, LastName = tempUserdb.LastName, Password = tempUserdb.Password };
            return View(tempUser);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(UserManagerModel modelmanage)
        {
            if (ModelState.IsValid)
            {
                UserModel model = new UserModel {Id = modelmanage.Id , Email = modelmanage.Email , FirstName = modelmanage.FirstName , LastName = modelmanage.LastName , Password = modelmanage.Password };
                _dbContext.Users.Update(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(modelmanage);
        }
    }
}
