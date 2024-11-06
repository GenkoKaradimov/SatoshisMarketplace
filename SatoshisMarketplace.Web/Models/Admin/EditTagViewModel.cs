using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.Admin
{
    public class EditTagViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "DisplayName is required")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "DisplayName must be between 3 and 45 characters.")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters.")]
        public string Description { get; set; }
    }
}
