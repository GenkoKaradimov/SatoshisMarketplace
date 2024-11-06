using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class Tag
    {
        [Key]
        [Required]
        public int Id { get; set; }


        [Required]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "DisplayName must be between 3 and 45 characters.")]
        public string DisplayName { get; set; }


        [Required]
        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters.")]
        public string Description { get; set; }
    }
}
