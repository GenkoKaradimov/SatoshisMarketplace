using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class FavoriteProduct
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        // ID of User
        public string Username { get; set; }

        [Required]
        public DateTime TimestampCreated { get; set; }

        #region Navigational Properties

        public Product Product { get; set; }

        public User User { get; set; }

        #endregion
    }
}
