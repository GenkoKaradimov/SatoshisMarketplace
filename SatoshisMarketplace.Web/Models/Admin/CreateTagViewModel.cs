﻿using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.Admin
{
    public class CreateTagViewModel
    {
        [Required(ErrorMessage = "DisplayName is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "DisplayName must be between 3 and 45 characters.")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description must not be longer than 500 characters.")]
        public string Description { get; set; }
    }
}