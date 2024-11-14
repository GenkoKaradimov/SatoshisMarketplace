using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SatoshisMarketplace.Entities;
using SatoshisMarketplace.Services.Implementations;
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
                MyProducts = new List<Models.Product.ManageProductViewModel>(),
                MyFavorites = new List<Models.Product.ManageProductViewModel>(),
                MyPurchases = new List<Models.Product.ManageProductViewModel>()
            };

            try
            {
                var myProducts = await _productService.GetProductsByOwnerAsync(username);
                model.MyProducts = myProducts.Select(prod => new Models.Product.ManageProductViewModel()
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            return RedirectToAction("Products", "Product");
        }

        #region Manage Product

        [HttpGet]
        public async Task<IActionResult> ManageProduct(int id)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            Services.Models.ProductService.ProductModel product;
            Dictionary<int, string> оptionalCategories;

            try
            {
                product = await _productService.GetProductAsync(id);
                оptionalCategories = await _productService.AllCategoriesAsync();

                // Is user owner of this product
                if (product.OwnerUsername != username)
                {
                    TempData["ErrorMessage"] = "You are not the owner of this product!";
                    return RedirectToAction("Products", "Product");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Products", "Product");
            }


            return View(new Models.Product.ManageProductViewModel()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                FirstPublication = product.FirstPublication,
                LastUpdate = product.LastUpdate,
                Price = product.Price,
                IsListed = product.IsListed,
                Images = product.ProductImages,
                CategoryId = product.CategoryId,
                CategoryPath = product.CategoryPath,
                Tags = product.Tags.Select(t => new Models.Product.TagViewModel()
                {
                    Id = t.Id,
                    Name = t.DisplayName
                }).ToList(),
                Files = product.ProductFiles.Select(pf => new Models.Product.ProductFileViewModel()
                {
                    Id = pf.Id,
                    Title = pf.Title,
                    TimestampUploaded = pf.TimestampUploaded
                }).ToList(),
                OptionalCategories = оptionalCategories
            });
        }

        [HttpPut]
        public async Task<IActionResult> MakeProductPublic(int id)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            Services.Models.ProductService.ProductModel product = null;

            try
            {
                product = await _productService.GetProductAsync(id);

                if (product.OwnerUsername != username) return BadRequest("You are not owner of this product!");

                await _productService.PublishProductAsync(id, true);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Products", "Product");
            }

            TempData["ErrorMessage"] = "Product published successfully!";
            return RedirectToAction("ManageProduct", "Product");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBasicInfo([FromBody] Models.Product.UpdateProductPage model)
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
                return BadRequest(errorMessage);
            }

            try
            {
                // Is user owner of this product
                var product = await _productService.GetProductAsync(model.Id);
                if (product.OwnerUsername != username) return BadRequest("You are not the owner of this product!");

                await _productService.UpdateProductAsync(new Services.Models.ProductService.UpdateProductModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.SelectedParentCategoryId
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }

            return Ok("Basic info changed successfully!");
        }

        public async Task<IActionResult> UploadProductImage(int id, IFormFile image)
        {
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            Services.Models.ProductService.ProductModel product = null;

            try
            {
                // Convert the uploaded file to a byte array
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                // Is user owner of this product
                product = await _productService.GetProductAsync(id);
                if (product.OwnerUsername != username)
                {
                    return BadRequest("You are not owner of this product!");
                }

                // add it at database
                await _productService.AddProductFileAsync(new Services.Models.ProductService.AddProductFileModel()
                {
                    ProductId = id,
                    IsImage = true,
                    ContentType = image.ContentType,
                    ImageData = imageBytes,
                    Title = "N/A"
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return Ok("Image changed successfully");
        }

        public async Task<IActionResult> UploadProductFile([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] int productId)
        {
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Is user owner of this product
                var product = await _productService.GetProductAsync(productId);

                // Convert the uploaded file to a byte array
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                if (product.OwnerUsername != username) return BadRequest("You are not owner of this product!");

                // add it at database
                await _productService.AddProductFileAsync(new Services.Models.ProductService.AddProductFileModel()
                {
                    ProductId = productId,
                    IsImage = false,
                    ContentType = file.ContentType,
                    ImageData = fileBytes,
                    Title = fileName,
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return BadRequest("File not saved!");
            }

            return Ok("File added successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProductFile(int id)
        {
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            try
            {
                var prodFile = await _productService.GetProductFile(id);
                var product = await _productService.GetProductAsync(prodFile.ProductId);
                if (product.OwnerUsername != username) return BadRequest("You are not owner of this product!");

                await _productService.RemoveProductFileAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("ProductFile (image or file) removed successfully!");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProductTag([FromForm] int tagId, [FromForm] int productId)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            try
            {
                var product = await _productService.GetProductAsync(productId);
                if (product.OwnerUsername != username) return BadRequest("You are not owner of this product!");

                await _productService.RemoveProductTag(productId, tagId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("ProductTag removed successfully!");
        }

        [HttpGet]
        public async Task<IActionResult> AddProductTag([FromQuery] int tagId, [FromQuery] int productId)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            try
            {
                var product = await _productService.GetProductAsync(productId);
                if (product.OwnerUsername != username) return BadRequest("You are not owner of this product!");

                await _productService.AddProductTag(productId, tagId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("ProductTag add successfully!");
        }

        public async Task<IActionResult> GetProductImage(int id)
        {
            Services.Models.ProductService.ProductFIleModel image;

            try
            {
                // take image from database
                image = await _productService.GetProductFile(id);

                // prevent returning file that is not image
                if (image.ProductFileType != Services.Models.ProductService.ProductFileType.ProductImage) return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return File(image.FileData, image.ContentType);
        }

        [HttpGet]
        public async Task<IActionResult> GetTags(string val)
        {
            if(val == null) return BadRequest("Value (that is searched) is null!");
            if(val.Count() < 3) return BadRequest("Value (that is searched) is null!");

            List<Services.Models.ProductService.Tag> tags = null;

            try
            {
                tags = await _productService.TagsSearch(val);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            string json = JsonConvert.SerializeObject(tags, Formatting.Indented);

            return Ok(json);
        }

        #endregion
    }
}
