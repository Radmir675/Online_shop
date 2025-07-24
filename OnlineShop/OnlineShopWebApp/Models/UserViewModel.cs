using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        [EmailAddress(ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Very short password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your password confirm")]
        [Compare("Password", ErrorMessage = "Passwords are not same")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "No permission level specified")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Your phone number is very short")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Format wrong")]
        public string Number { get; set; }



    }
}
