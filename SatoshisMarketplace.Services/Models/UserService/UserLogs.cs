using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.UserService
{
    public class UserLogs
    {
        public int AllLogsCount { get; set; }

        public int PagesCount { get; set; }

        public List<UserLog> Logs { get; set; }
    }
}
