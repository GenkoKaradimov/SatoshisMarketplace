using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The name must be less than 50 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The description must be less than 500 characters.")]
        public string Description { get; set; }

        #region Navigational Properties
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public int? ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        #endregion
    }
}
