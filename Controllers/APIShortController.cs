using FinalProjectShortenURL.Data;
using FinalProjectShortenURL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProjectShortenURL.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIShortController : ControllerBase
    {
        private readonly DataDbContext _context;

        public APIShortController(DataDbContext context)
        {
            _context = context;
        }
        [HttpPost("shorten")]
        public ActionResult<string> Shorten([FromBody] string fullurl)
        {
            string shorturl = @"https://localhost:7026/w/";
            string tempforcheck = "";
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue("UserId");
            var tempUser = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (!string.IsNullOrEmpty(fullurl))
            {
                Uri tmp;
                if (!Uri.IsWellFormedUriString(fullurl, UriKind.Absolute))
                    return BadRequest("Error, Invalid URL");
                if (!Uri.TryCreate(fullurl, UriKind.Absolute, out tmp))
                    return BadRequest("Error, Invalid URL");
                if (!(tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps))
                    return BadRequest("Error, Invalid URL");
                if (tempUser == null && _context.Urls.Any(u => u.FullURL == fullurl && u.UrlUserId == null))
                {
                    shorturl += _context.Urls.First(u => u.FullURL == fullurl && u.UrlUserId == null).ShortURL;
                    return Ok(shorturl);
                }
                else if (tempUser != null && _context.Urls.Any(u => u.FullURL == fullurl && u.UrlUserId.Equals(tempUser.Id)))
                {
                    shorturl += _context.Urls.First(u => u.FullURL == fullurl && u.UrlUserId.Equals(tempUser.Id)).ShortURL;
                    return Ok(shorturl);
                }
                else
                {
                    URLModel newURL = new URLModel();
                    Random rnd = new Random();
                    do
                    {
                        tempforcheck = "";
                        const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
                        var chars = Enumerable.Range(0, rnd.Next(6, 10))
                            .Select(x => pool[rnd.Next(0, pool.Length)]);
                        tempforcheck += new string(chars.ToArray());

                    } while (_context.Urls.Any(u => u.ShortURL == tempforcheck));
                    newURL.ShortURL = tempforcheck;
                    newURL.FullURL = fullurl;
                    
                        if (tempUser != null)
                            newURL.UrlUserId = tempUser.Id;
                    
                    _context.Urls.Add(newURL);
                    _context.SaveChanges();
                    return Ok(shorturl + tempforcheck);
                }
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpGet("/w/{shortenurl}")]
        public async Task<ActionResult> RedirectShort(string shortenurl)
        {

            if (!string.IsNullOrEmpty(shortenurl))
            {
                var webObj = await _context.Urls.FirstOrDefaultAsync(u => u.ShortURL == shortenurl);
                if (webObj != null) //_context.URLs.FirstOrDefaultAsync(u => u.ShortURL == shortenurl) != null
                {
                    string website = webObj.FullURL;
                    webObj.CounterOfRequests++;
                    webObj.Entries.Add(new Entry { PublicIP = await EntryExtension.GetPublicIpAddress() });
                    await _context.SaveChangesAsync();
                    return Redirect(website);
                }
            }
            return BadRequest();

        }
    
}
}
