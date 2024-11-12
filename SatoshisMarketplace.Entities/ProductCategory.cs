using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class ProductCategory
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        #region Navigational Properties

        public Product Product { get; set; }

        public Category Category { get; set; }

        #endregion
    }
}
