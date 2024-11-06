using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.AdminService
{
    public class Tag
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
