using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.User
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old password is required")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Old password must be between 7 and 20 characters.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "New password must be between 7 and 20 characters.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Confirm new password must be between 7 and 20 characters.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation new password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
