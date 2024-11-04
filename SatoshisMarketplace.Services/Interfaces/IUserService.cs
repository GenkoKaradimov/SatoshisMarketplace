using SatoshisMarketplace.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Interfaces
{
    public interface IUserService
    {
        Task<Models.UserService.UserModel> GetUserAsync(string username);

        Task<Models.UserService.UserModel> RegisterUserAsync(Models.UserService.UserRegistrationModel model);

        Task<Models.UserService.UserModel> LoginAsync(Models.UserService.UserLoginModel model);

        Task<bool> LogoutAsync(Models.UserService.UserLogoutModel model);

        Task<bool> ChangeUserPasswordAsync(Models.UserService.UserChangePasswordModel model);

        Task<List<Models.UserService.UserLog>> LogsByUserAsync(string username, int count);

        Task<Models.UserService.UserLogs> LogsByUserAsync(string username, int page, int logsPerPage);
    }
}
