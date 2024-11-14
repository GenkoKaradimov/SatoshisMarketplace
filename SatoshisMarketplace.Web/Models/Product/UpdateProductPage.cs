using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.Product
{
    public class UpdateProductPage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 5000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public uint Price { get; set; }

        public int SelectedParentCategoryId { get; set; }
    }
}
