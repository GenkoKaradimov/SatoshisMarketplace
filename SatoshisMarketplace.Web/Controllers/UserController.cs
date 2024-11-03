using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Web.Models.User;

namespace SatoshisMarketplace.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            string? username = HttpContext.Session.GetString("Username");
            if (username != null) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate input model
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;

                return View();
            }

            // search user
            Services.Models.UserService.UserModel user = null;

            try
            {
                string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "noIP";

                var data = new Services.Models.UserService.UserLoginModel()
                {
                    Username = model.Username,
                    Password = model.Password,
                    IP = ipAddress
                };

                user = await _userService.LoginAsync(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            // save username to session
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            string? username = HttpContext.Session.GetString("Username");
            if(username == null) return RedirectToAction("Login", "User");

            // register logout
            try
            {
                string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "noIP";

                var data = new Services.Models.UserService.UserLogoutModel()
                {
                    Username = username,
                    IP = ipAddress
                };

                var user = await _userService.LogoutAsync(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // remove username from session - real logout
            HttpContext.Session.Remove("Username");
            
            return RedirectToAction("Login", "User"); ;
        }

        public ActionResult Registration()
        {
            string? username = HttpContext.Session.GetString("Username");
            if (username != null) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            // Validate input model
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                string errorMessage = String.Join(" ", errors);

                TempData["ErrorMessage"] = errorMessage;

                return View();
            }

            // make registration
            Services.Models.UserService.UserModel user = null;

            try
            {
                string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "noIP";

                var data = new Services.Models.UserService.UserRegistrationModel()
                {
                    Username = model.Username,
                    Password = model.Password,
                    IP = ipAddress
                };

                user = await _userService.RegisterUserAsync(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            
            // save username to session
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserSetting()
        {
            return View();
        }
    }
}
