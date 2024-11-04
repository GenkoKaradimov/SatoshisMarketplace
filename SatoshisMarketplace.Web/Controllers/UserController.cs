using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Web.Models.User;
using System.Diagnostics;

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

        public async Task<IActionResult> UserSettings()
        {
            // is logged in
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            // get user data
            Services.Models.UserService.UserModel user = null;
            List<Services.Models.UserService.UserLog> logs = null;
            try
            {
                user = await _userService.GetUserAsync(username);
                logs = await _userService.LogsByUserAsync(username, 5);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Login", "User");
            }

            // prepare model
            var model = new Models.User.UserSettingsViewModel()
            {
                Username = user.Username,
                Logs = new List<UserLog>()
            };

            // fill logs
            foreach (var log in logs)
            {
                model.Logs.Add(new UserLog()
                {
                    Timestamp = log.Timestamp,
                    IP = log.IP,
                    LogType = log.LogType
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Logs(int? page)
        {
            // get current page
            int currentPage = page ?? 1;
            int logsPerPage = 6;

            // Check user
            string? username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }

            // get logs
            var logs = await _userService.LogsByUserAsync(username, currentPage, logsPerPage);

            // create return model
            var model = new Models.User.UserLogs()
            {
                Username = username,
                AllLogsCount = logs.AllLogsCount,
                PagesCount = logs.PagesCount,
                CurrentPage = currentPage,
                Logs = new List<UserLog>()
            };

            // fill logs
            foreach(var log in logs.Logs)
            {
                model.Logs.Add(new UserLog()
                {
                    Timestamp = log.Timestamp,
                    IP= log.IP,
                    LogType= log.LogType
                });
            }

            return View(model);
        }

        public ActionResult ChangePassword()
        {
            string? username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "User");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            // is logged in
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

            // change password
            try
            {
                string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "noIP";

                var data = new Services.Models.UserService.UserChangePasswordModel()
                {
                    Username = username,
                    OldPassword = model.OldPassword,
                    NewPassword = model.NewPassword,
                    IP = ipAddress
                };

                await _userService.ChangeUserPasswordAsync(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

            return RedirectToAction("UserSettings", "User");
        }
    }
}
