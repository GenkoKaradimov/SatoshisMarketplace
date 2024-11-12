using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class ProductTag
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int TagId { get; set; }

        #region Navigational Properties

        public Product Product { get; set; }

        public Tag Tag { get; set; }

        #endregion
    }
}
