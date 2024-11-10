using SatoshisMarketplace.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.ProductService
{
    public class CreateProductModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public uint Price { get; set; }

        public string OwnerUsername { get; set; }
    }
}
