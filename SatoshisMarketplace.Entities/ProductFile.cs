using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public class ProductFile
    {
        public int Id { get; set; }

        [MaxLength(1073741824)] // 1 GB in bytes
        public byte[] ImageData { get; set; }

        [MaxLength(50)]
        public string ContentType { get; set; }

        public ProductFileType Type { get; set; }

        public DateTime TimestampUploaded { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        #region Navigational Properties

        public int ProductId { get; set; }

        public Product Product { get; set; }

        #endregion
    }
}
