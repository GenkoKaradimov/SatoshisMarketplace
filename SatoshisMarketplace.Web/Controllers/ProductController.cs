using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Web.Models.User;
using System.Diagnostics;

namespace SatoshisMarketplace.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            var model = new Models.Product.ProductsViewModel()
            {
                MyProducts = new List<Models.Product.ProductViewModel>(),
                MyFavorites = new List<Models.Product.ProductViewModel>(),
                MyPurchases = new List<Models.Product.ProductViewModel>()
            };

            try
            {
                var myProducts = await _productService.GetProductsByOwnerAsync(username);
                model.MyProducts = myProducts.Select(prod => new Models.Product.ProductViewModel()
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    FirstPublication = prod.FirstPublication,
                    IsListed = prod.IsListed,
                    Images = prod.ProductImages
                }).ToList();
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Models.Product.CreateProductPage model)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // Validate input model
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;
                return View();
            }

            try
            {
                await _productService.CreateProductAsync(new Services.Models.ProductService.CreateProductModel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    OwnerUsername = username
                });
            }
            catch (Exception ex) {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            return RedirectToAction("Products", "Product");
        }
    }
}
