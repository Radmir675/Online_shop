using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Please enter role name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Very short role")]
        public string Name { get; set; }
    }
}
