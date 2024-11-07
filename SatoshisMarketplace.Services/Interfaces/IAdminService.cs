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

        Task<List<Models.AdminService.Category>> GetCategoryTreeAsync(int? startParentId);

        Task<Dictionary<int, string>> GetCategoryDisplayListAsync();

        Task<Models.AdminService.Category> GetCategoryByIdAsync(int id);

        Task<Models.AdminService.Category> CreateCategoryAsync(Models.AdminService.Category model);

        Task<Models.AdminService.Category> EditCategoryAsync(Models.AdminService.Category model);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
