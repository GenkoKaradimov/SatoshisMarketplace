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

        public async Task<List<Models.AdminService.Tag>> GetTagsAsync()
        {
            var tags = await _context.Tags.ToListAsync();

            var resp = tags.Select(tag => new Models.AdminService.Tag
            {
                Id = tag.Id,
                DisplayName = tag.DisplayName,
                Description = tag.Description
            }).ToList();

            return resp;
        }

        public async Task<Models.AdminService.Tag> GetTagByIdAsync(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(tag => tag.Id == id);

            if(tag == null)
            {
                throw new ArgumentException("Tag not found!");
            }

            var resp = new Models.AdminService.Tag()
            {
                Id = tag.Id,
                DisplayName = tag.DisplayName,
                Description = tag.Description
            };

            return resp;
        }

        public async Task<Models.AdminService.Tag> CreateTagAsync(Models.AdminService.Tag model)
        {
            var tag = new Entities.Tag()
            {
                DisplayName = model.DisplayName,
                Description = model.Description
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return new Models.AdminService.Tag()
            {
                Id = tag.Id,
                DisplayName = model.DisplayName,
                Description = model.Description
            };
        }

        public async Task<Models.AdminService.Tag> EditTagAsync(Models.AdminService.Tag model)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(tag => tag.Id == model.Id);

            if (tag == null)
            {
                throw new ArgumentException("Tag not found!");
            }

            //if(tag.DisplayName == model.DisplayName && tag.Description == model.Description)
            //{
            //    throw new ArgumentException("Nothing changed in 'tag' parameter!");
            //}

            tag.DisplayName = model.DisplayName;
            tag.Description = model.Description;

            await _context.SaveChangesAsync();

            return new Models.AdminService.Tag
            {
                Id = tag.Id,
                DisplayName = tag.DisplayName,
                Description = tag.Description
            };
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(tag => tag.Id == id);

            if (tag == null)
            {
                return false;
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
