using Microsoft.EntityFrameworkCore;
using SatoshisMarketplace.Entities;
using SatoshisMarketplace.Services.Interfaces;
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
            var productTask = _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            var picturesTask = _context.ProductFiles.Where(pi => pi.Type == ProductFileType.ProductImage && pi.ProductId == id).Select(pi => pi.Id).ToListAsync();

            // await getting info from database
            await Task.WhenAll(productTask, picturesTask);
            var product = productTask.Result;
            var pictures = picturesTask.Result;

            if (product == null)
            {
                throw new ArgumentException("Product not found!");
            }

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
                ProductImages = pictures
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
                    .Where(pi => pi.Type == ProductFileType.ProductImage && pi.ProductId == p.Id)
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

            await _context.SaveChangesAsync();
        }

        public async Task PublishProductAsync(int id, bool isPublished)
        {
            // get product and images from database
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new ArgumentException("Product not found!");
            }

            // if user want to publish product, product must meet the following conditions
            var countingImages = _context.ProductFiles.Where(pf => pf.Type == ProductFileType.ProductImage && pf.ProductId == id).CountAsync();
            var countingFiles = _context.ProductFiles.Where(pf => pf.Type == ProductFileType.File && pf.ProductId == id).CountAsync();
            await Task.WhenAll(countingImages, countingFiles);
            if ((await countingImages) == 0) throw new ArgumentException("Product has no images!");
            if ((await countingFiles) == 0) throw new ArgumentException("Product has no files!");

            // make it public
            product.IsListed = isPublished;
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddProductFileAsync()
        {
            return 0;
        }

        public async Task RemoveProductFileAsync()
        {

        }
    }
}
