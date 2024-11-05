using Microsoft.EntityFrameworkCore;
using SatoshisMarketplace.Entities;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Services.Models.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ServerDbContext _context;

        public AdminService(ServerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAdministratorAsync(string username)
        { 
            // Fetch the user asynchronously
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

            // User not found => no admin priviliges
            if (entity == null)
            {
                return false;
            }

            return entity.IsAdministator;
        }
    }
}
