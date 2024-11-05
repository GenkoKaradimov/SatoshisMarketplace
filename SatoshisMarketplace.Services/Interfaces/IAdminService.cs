using SatoshisMarketplace.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> IsAdministratorAsync(string username);
    }
}
