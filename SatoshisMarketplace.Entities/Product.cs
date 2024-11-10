using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The name must be between 5 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(5000, ErrorMessage = "The description must be less than 5000 characters.")]
        public string Description { get; set; }

        [Required]
        public uint Price { get; set; }

        public DateTime FirstPublication { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsListed { get; set; }

        [Required]
        public string OwnerUsername { get; set; }

        #region Navigational Properties

        public User Owner { get; set; }

        public ICollection<ProductFile> ProductFiles { get; set; } = new List<ProductFile>();

        #endregion
    }
}
