using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.User
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Password must be between 7 and 20 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Confirm Password must be between 7 and 20 characters.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
