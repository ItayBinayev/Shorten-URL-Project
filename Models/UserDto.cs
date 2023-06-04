using System.ComponentModel.DataAnnotations;

namespace FinalProjectShortenURL.Models
{
    public class UserDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
