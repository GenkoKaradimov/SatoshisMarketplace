using Microsoft.EntityFrameworkCore;
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

        Task<List<Models.AdminService.Tag>> GetTagsAsync();

        Task<Models.AdminService.Tag> GetTagByIdAsync(int id);

        Task<Models.AdminService.Tag> CreateTagAsync(Models.AdminService.Tag model);

        Task<Models.AdminService.Tag> EditTagAsync(Models.AdminService.Tag model);

        Task<bool> DeleteTagAsync(int id);
    }
}
