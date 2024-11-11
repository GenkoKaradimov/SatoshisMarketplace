using SatoshisMarketplace.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.ProductService
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] File { get; set; }

        public uint Price { get; set; }

        public DateTime FirstPublication { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsListed { get; set; }

        public string OwnerUsername { get; set; }

        public List<int> ProductImages { get; set; }

        public List<ProductFIleModel> ProductFiles { get; set; }

    }
}
