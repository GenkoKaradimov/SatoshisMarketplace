using Microsoft.EntityFrameworkCore;
using SatoshisMarketplace.Entities;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Services.Models.AdminService;
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

            if (tag == null)
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

        public async Task<List<Models.AdminService.Category>> GetCategoryTreeAsync(int? startParentId)
        {
            // get all categories from database
            var list = await _context.Categories
                .Select(cat => new Models.AdminService.Category
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description,
                    ParentCategoryId = cat.ParentCategoryId
                }).ToListAsync();

            // order them
            var resp = SubCategories(startParentId, list);

            return resp;
        }

        public async Task<Dictionary<int, string>> GetCategoryDisplayListAsync()
        {
            // get all categories from database
            var list = await _context.Categories
                .Select(cat => new Models.AdminService.Category
                {
                    Id = cat.Id,
                    Name = cat.Name
                }).ToListAsync();

            // create dictionary with Id as key and Name as value
            var resp = list.ToDictionary(cat => cat.Id, cat => cat.Name);

            return resp;
        }

        private List<Models.AdminService.Category> SubCategories(int? parentId, List<Models.AdminService.Category> allCategories)
        {
            var subCategories = allCategories
                .Where(cat => cat.ParentCategoryId == parentId)
                .Select(cat => new Models.AdminService.Category
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description,
                    SubCategories = SubCategories(cat.Id, allCategories)
                }).ToList();

            return subCategories;
        }

        public async Task<Models.AdminService.Category> GetCategoryByIdAsync(int id)
        {
            var cat = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (cat == null)
            {
                throw new ArgumentException("Category not found!");
            }

            return new Models.AdminService.Category()
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
                ParentCategoryId = cat.ParentCategoryId
            };
        }

        public async Task<Models.AdminService.Category> CreateCategoryAsync(Models.AdminService.Category model)
        {
            var category = new Entities.Category()
            {
                Name = model.Name,
                Description = model.Description,
                ParentCategoryId = model.ParentCategoryId
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new Models.AdminService.Category()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId
            };
        }

        public async Task<Models.AdminService.Category> EditCategoryAsync(Models.AdminService.Category model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Id == model.Id);

            if (category == null)
            {
                throw new ArgumentException("Category not found!");
            }

            if (model.ParentCategoryId == model.Id)
            {
                throw new ArgumentException("A category cannot be a parent of itself.");
            }

            category.Name = model.Name;
            category.Description = model.Description;
            category.ParentCategoryId = model.ParentCategoryId;

            await _context.SaveChangesAsync();

            return new Models.AdminService.Category()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.Include(cat => cat.SubCategories).FirstOrDefaultAsync(cat => cat.Id == id);

            if (category == null)
            {
                return false;
            }

            if(category.SubCategories.Count() != 0)
            {
                throw new ArgumentException("Category has " + category.SubCategories.Count() + " sub categories, you have to remove them first!");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
