using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Web.Models.User;
using System.Diagnostics;

namespace SatoshisMarketplace.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _userService;

        public AdminController(IAdminService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            // is user logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // is this user administrator
            var isAdmin = await _userService.IsAdministratorAsync(username);
            if (!isAdmin) return RedirectToAction("Index", "Home");

            return View();
        }

    }
}
