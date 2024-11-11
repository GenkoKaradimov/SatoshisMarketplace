using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.ProductService
{
    public class AddProductFileModel
    {
        public int ProductId { get; set; }

        public string Title { get; set; }

        public bool IsImage { get; set; }

        public byte[] ImageData { get; set; }

        public string ContentType { get; set; }
    }
}
