using SatoshisMarketplace.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.ProductService
{
    public class ProductFIleModel
    {
        public int Id { get; set; }

        public byte[] FileData { get; set; }

        public string ContentType { get; set; }

        public DateTime TimestampUploaded { get; set; }

        public string Title { get; set; }

        public int ProductId { get; set; }
    }
}
