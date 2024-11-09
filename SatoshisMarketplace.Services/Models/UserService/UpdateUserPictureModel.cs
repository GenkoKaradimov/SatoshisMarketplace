using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.UserService
{
    public class UpdateUserPictureModel
    {
        public string Username { get; set; }

        public string IP { get; set; }

        public byte[] Image { get; set; }
    }
}
