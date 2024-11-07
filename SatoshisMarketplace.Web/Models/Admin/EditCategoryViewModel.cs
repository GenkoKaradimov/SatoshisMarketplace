using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.Admin
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "DisplayName must be between 3 and 45 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The description must be less than 500 characters.")]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public Dictionary<int, string>? OptionalCategories { get; set; }
    }
}
