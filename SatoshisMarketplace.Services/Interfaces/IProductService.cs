using Microsoft.EntityFrameworkCore;
using SatoshisMarketplace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Interfaces
{
    public interface IProductService
    {
        Task<Models.ProductService.ProductModel> GetProductAsync(int id);

        Task<List<Models.ProductService.ProductModel>> GetProductsByOwnerAsync(string ownerUsername);

        Task<Models.ProductService.ProductFIleModel> GetProductFileAsync(int id);

        Task<int> CreateProductAsync(Models.ProductService.CreateProductModel model);

        Task UpdateProductAsync(Models.ProductService.UpdateProductModel model);

        Task PublishProductAsync(int id, bool isPublished);

        Task<int> AddProductFileAsync();

        Task RemoveProductFileAsync();
    }
}
