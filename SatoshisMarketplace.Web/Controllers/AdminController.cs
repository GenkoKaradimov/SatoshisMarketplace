using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Web.Models.Admin;
using SatoshisMarketplace.Web.Models.User;
using System.Diagnostics;

namespace SatoshisMarketplace.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Tags()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // get tags from database
            var tags = new List<Services.Models.AdminService.Tag>();
            try
            {
                tags = await _adminService.GetTagsAsync();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Admin");
            }

            var resp = new Models.Admin.TagsViewModel()
            {
                Tags = tags.Select(tag => new Models.Admin.TagViewModel()
                {
                    Id = tag.Id,
                    DisplayName = tag.DisplayName,
                    Description = tag.Description,

                }).ToList()
            };

            return View(resp);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTag()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(Models.Admin.CreateTagViewModel model)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // is model valid
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;

                return View();
            }

            // create new tag
            try
            {
                await _adminService.CreateTagAsync(new Services.Models.AdminService.Tag()
                {
                    DisplayName = model.DisplayName,
                    Description = model.Description
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            TempData["SuccessMessage"] = "Tag created successfully.";
            return RedirectToAction("Tags", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(int? id)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // 'id' not parsed
            if (id == null) return RedirectToAction("Tags", "Admin");

            // get tag from database
            Services.Models.AdminService.Tag tag;
            try
            {
                tag = await _adminService.GetTagByIdAsync((int)id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Tags", "Admin");
            }

            EditTagViewModel model = new EditTagViewModel()
            {
                Id = tag.Id,
                DisplayName = tag.DisplayName,
                Description = tag.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTag(Models.Admin.EditTagViewModel model)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // is model valid
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;

                return RedirectToAction("Tags", "Admin");
            }

            // edit tag
            try
            {
                await _adminService.EditTagAsync(new Services.Models.AdminService.Tag()
                {
                    Id = model.Id,
                    DisplayName = model.DisplayName,
                    Description = model.Description
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            TempData["SuccessMessage"] = "Tag edited successfully.";
            return RedirectToAction("Tags", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTag(int? id)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // 'id' not parsed
            if (id == null) return RedirectToAction("Tags", "Admin");

            // delete tag from database
            bool result;
            try
            {
                result = await _adminService.DeleteTagAsync((int)id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Tags", "Admin");
            }

            // response
            if (result)
            {
                TempData["SuccessMessage"] = "Tag deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Some error ocured";
            }

            return RedirectToAction("Tags", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // get categories from database
            List<Services.Models.AdminService.Category> categories;
            try
            {
                categories = await _adminService.GetCategoryTreeAsync(null);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Admin");
            }

            var resp = new Models.Admin.CategoriesViewModel()
            {
                Categories = categories.Select(cat => MapCategory(cat)).ToList()
            };

            return View(resp);
        }

        private Models.Admin.CategoryViewModel MapCategory(Services.Models.AdminService.Category cat)
        {
            return new Models.Admin.CategoryViewModel()
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
                ParentCategoryId = cat.ParentCategoryId,
                SubCategories = cat.SubCategories.Select(subCat => MapCategory(subCat)).ToList()
            };
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory(int? parentId)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // get categories list
            Dictionary<int, string> pairs;
            try
            {
                pairs = await _adminService.GetCategoryDisplayListAsync();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            var model = new CreateCategoryViewModel()
            {
                OptionalCategories = pairs
            };
            if (parentId != null) model.ParentCategoryId = parentId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Models.Admin.CreateCategoryViewModel model)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // is model valid
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;

                return View(model);
            }

            // create new category
            try
            {
                var data = new Services.Models.AdminService.Category()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ParentCategoryId = model.ParentCategoryId,
                };

                if (model.ParentCategoryId == 0 || model.ParentCategoryId == -1) data.ParentCategoryId = null;

                await _adminService.CreateCategoryAsync(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            TempData["SuccessMessage"] = "Category created successfully.";
            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int Id)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // get category
            Dictionary<int, string> pairs;
            Services.Models.AdminService.Category category;
            try
            {
                pairs = await _adminService.GetCategoryDisplayListAsync();
                category = await _adminService.GetCategoryByIdAsync(Id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            var model = new EditCategoryViewModel()
            {
                OptionalCategories = pairs,
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Models.Admin.EditCategoryViewModel model)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // is model valid
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;

                return View(model);
            }

            // edit new category
            try
            {
                var data = new Services.Models.AdminService.Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    ParentCategoryId = model.ParentCategoryId,
                };

                if (model.ParentCategoryId == 0 || model.ParentCategoryId == -1) data.ParentCategoryId = null;

                await _adminService.EditCategoryAsync(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = "Category edited successfully.";
            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _adminService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            // 'id' not parsed
            if (id == null) return RedirectToAction("Categories", "Admin");

            // delete category from database
            bool result;
            try
            {
                result = await _adminService.DeleteCategoryAsync((int)id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Categories", "Admin");
            }

            // response
            if (result)
            {
                TempData["SuccessMessage"] = "Category deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Some error ocured";
            }

            return RedirectToAction("Categories", "Admin");
        }
    }
}
