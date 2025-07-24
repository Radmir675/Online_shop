using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Very short password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
