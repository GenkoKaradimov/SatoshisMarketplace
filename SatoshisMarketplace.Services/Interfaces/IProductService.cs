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

        Task<int> AddProductFileAsync(Models.ProductService.AddProductFileModel model);

        Task RemoveProductFileAsync(int productFileId);

        Task<Models.ProductService.ProductFIleModel> GetProductFile(int id);

        Task RemoveProductTag(int productId, int tagId);

        Task AddProductTag(int productId, int tagId);

        Task<List<Models.ProductService.Tag>> TagsSearch(string val);

        Task<Dictionary<int, string>> AllCategoriesAsync();

        Task<Dictionary<int, string>> AllCategoriesAsync(string val);

        Task<Models.ProductService.ProductFIleModel> DownloadFile(int fileId, string username);

        #region Favourite Products

        Task AddFavoriteProduct(int productId, string username);

        Task RemoveFavoriteProduct(int productId, string username);

        Task<List<Models.ProductService.ProductModel>> GetFavoriteProductsByUserAsync(string username);

        Task<bool> IsFavoriteProduct(int productId, string? username);

        #endregion

    }
}
