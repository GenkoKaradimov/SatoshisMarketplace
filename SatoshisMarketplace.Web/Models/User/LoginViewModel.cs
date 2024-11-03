using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Password must be between 7 and 20 characters.")]
        public string Password { get; set; }
    }
}
