using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.UserService
{
    public class UserLog
    {
        public DateTime Timestamp { get; set; }

        public string IP { get; set; }

        public string LogType { get; set; }
    }
}
