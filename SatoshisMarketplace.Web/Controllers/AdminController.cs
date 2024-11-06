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

    }
}
