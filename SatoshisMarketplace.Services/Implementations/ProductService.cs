using Microsoft.EntityFrameworkCore;
using SatoshisMarketplace.Entities;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Services.Models.AdminService;
using SatoshisMarketplace.Services.Models.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ServerDbContext _context;

        public ProductService(ServerDbContext context)
        {
            _context = context;
        }

        public async Task<Models.ProductService.ProductModel> GetProductAsync(int id)
        {
            // get product and images from database
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new ArgumentException("Product not found!");
            }

            var pictures = await _context.ProductFiles.Where(pi => pi.Type == Entities.ProductFileType.ProductImage && pi.ProductId == id).Select(pi => pi.Id).ToListAsync();
            var files = await _context.ProductFiles.Where(pf => pf.Type == Entities.ProductFileType.File && pf.ProductId == id).Select(pf => new ProductFIleModel() { Id = pf.Id, Title = pf.Title, TimestampUploaded = pf.TimestampUploaded }).ToListAsync();
            var tags = await _context.ProductTag.Where(pt => pt.ProductId == id).Select(pt => new Models.ProductService.Tag { Id = pt.Tag.Id, DisplayName = pt.Tag.DisplayName }).ToListAsync();
            var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductId == product.Id); // at this moment one product have only one category

            // return model
            return new Models.ProductService.ProductModel()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                IsListed = product.IsListed,
                FirstPublication = product.FirstPublication,
                LastUpdate = product.LastUpdate,
                OwnerUsername = product.OwnerUsername,
                Price = product.Price,
                ProductImages = pictures,
                ProductFiles = files,
                Tags = tags,
                CategoryId = productCategory?.CategoryId ?? null,
                CategoryPath = await GetCategoryPath(productCategory?.CategoryId ?? null)
            };
        }

        public async Task<List<Models.ProductService.ProductModel>> GetProductsByOwnerAsync(string ownerUsername)
        {
            var products = await _context.Products
                .Where(p => p.OwnerUsername == ownerUsername)
                .Select(p => new Models.ProductService.ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    IsListed = p.IsListed,
                    FirstPublication = p.FirstPublication,
                    LastUpdate = p.LastUpdate,
                    OwnerUsername = p.OwnerUsername,
                    Price = p.Price,
                    ProductImages = _context.ProductFiles
                    .Where(pi => pi.Type == Entities.ProductFileType.ProductImage && pi.ProductId == p.Id)
                    .Select(pi => pi.Id)
                    .ToList()
                }).ToListAsync();

            return products;
        }

        public async Task<Models.ProductService.ProductFIleModel> GetProductFileAsync(int id)
        {
            var file = await _context.ProductFiles.FirstOrDefaultAsync(pf => pf.Id == id);

            if (file == null)
            {
                throw new ArgumentException("File not found!");
            }

            return new Models.ProductService.ProductFIleModel()
            {
                Id = file.Id,
                FileData = file.ImageData,
                ContentType = file.ContentType,
                Title = file.Title,
                TimestampUploaded = file.TimestampUploaded,
                ProductId = file.ProductId
            };
        }

        public async Task<int> CreateProductAsync(Models.ProductService.CreateProductModel model)
        {
            // create database object
            var product = new Entities.Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                FirstPublication = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                IsListed = false,
                OwnerUsername = model.OwnerUsername
            };

            // add to database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateProductAsync(Models.ProductService.UpdateProductModel model)
        {
            // get product and images from database
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.Id);

            if (product == null)
            {
                throw new ArgumentException("Product not found!");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;

            product.LastUpdate = DateTime.UtcNow;

            if (model.CategoryId != 0 && model.CategoryId != -1)
            {
                var cuurentCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductId == product.Id);
                if(cuurentCategory != null) _context.ProductCategories.Remove(cuurentCategory);

                var newCategory = new Entities.ProductCategory()
                {
                    CategoryId = model.CategoryId,
                    ProductId = product.Id
                };

                await _context.ProductCategories.AddAsync(newCategory);
            }

            await _context.SaveChangesAsync();
        }

        public async Task PublishProductAsync(int id, bool bePublished)
        {
            // get product and images from database
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new ArgumentException("Product not found!");
            }

            // if user want to publish product, product must meet the following conditions
            var countingImages = await _context.ProductFiles.Where(pf => pf.Type == Entities.ProductFileType.ProductImage && pf.ProductId == id).CountAsync();
            var countingFiles = await _context.ProductFiles.Where(pf => pf.Type == Entities.ProductFileType.File && pf.ProductId == id).CountAsync();

            if ((countingImages) == 0) throw new ArgumentException("Product has no images!");
            if ((countingFiles) == 0) throw new ArgumentException("Product has no files!");

            // make it public
            product.IsListed = bePublished;
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddProductFileAsync(Models.ProductService.AddProductFileModel model)
        {
            // get product and images from database
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.ProductId);

            if (product == null) throw new ArgumentException("Product not found!");

            var file = new Entities.ProductFile()
            {
                ProductId = model.ProductId,
                TimestampUploaded = DateTime.UtcNow,
                Title = model.Title,
                Type = Entities.ProductFileType.ProductImage,
                ContentType = model.ContentType,
                ImageData = model.ImageData
            };

            if (!model.IsImage) file.Type = Entities.ProductFileType.File;

            await _context.ProductFiles.AddAsync(file);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task RemoveProductFileAsync(int productFileId)
        {
            var prod = await _context.ProductFiles.FirstOrDefaultAsync(pf => pf.Id == productFileId);

            if (prod == null)
            {
                throw new ArgumentException("Product File not found!");
            }

            _context.ProductFiles.Remove(prod);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.ProductService.ProductFIleModel> GetProductFile(int id)
        {
            var file = await _context.ProductFiles.FirstOrDefaultAsync(pf => pf.Id == id);
            if (file == null) throw new ArgumentException("File not found!");

            return new Models.ProductService.ProductFIleModel()
            {
                Id = id,
                Title = file.Title,
                ProductId = file.ProductId,
                TimestampUploaded = file.TimestampUploaded,
                FileData = file.ImageData,
                ContentType = file.ContentType,
                ProductFileType = (Models.ProductService.ProductFileType)(int)file.Type
            };
        }

        public async Task RemoveProductTag(int productId, int tagId)
        {
            var productTag = await _context.ProductTag.FirstOrDefaultAsync(x => x.TagId == tagId && x.ProductId == productId);

            if (productTag == null) throw new ArgumentException("Product Tag not found!");

            _context.ProductTag.Remove(productTag);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductTag(int productId, int tagId)
        {
            var productTag = new Entities.ProductTag() { ProductId = productId, TagId = tagId };

            await _context.ProductTag.AddAsync(productTag);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Models.ProductService.Tag>> TagsSearch(string val)
        {
            var tags = await _context.Tags.Where(t => t.DisplayName.ToLower().Contains(val.ToLower())).ToListAsync();

            return tags.Select(t => new Models.ProductService.Tag()
            {
                Id = t.Id,
                DisplayName = t.DisplayName,
                Description = t.Description
            }).ToList();
        }

        public async Task<Dictionary<int, string>> AllCategoriesAsync()
        {
            var categories = await _context.Categories.Select(c => new { c.Id, c.Name }).ToListAsync();

            return categories.ToDictionary(c => c.Id, c => c.Name);
        }

        public async Task<Dictionary<int, string>> AllCategoriesAsync(string val)
        {
            var categories = await _context.Categories.Select(c => new { c.Id, c.Name }).Where(c => c.Name.ToLower().Contains(val.ToLower())).ToListAsync();

            return categories.ToDictionary(c => c.Id, c => c.Name);
        }

        private async Task<string> GetCategoryPath(int? categoryId)
        {
            if (categoryId == null) return string.Empty;

            var pathSegments = new List<string>();

            while (categoryId != null)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (category == null) break;

                pathSegments.Insert(0, category.Name);

                categoryId = category.ParentCategoryId;
            }

            pathSegments.Insert(0, "root");

            return string.Join("/", pathSegments);
        }

    }
}
