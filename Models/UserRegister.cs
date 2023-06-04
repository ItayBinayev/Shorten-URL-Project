using System.ComponentModel.DataAnnotations;

namespace FinalProjectShortenURL.Models
{
    public class UserRegister
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "{0} must be atleat {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "{0} must be atleat {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$",
                   ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one number.")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordValid { get; set; }
    }
}
