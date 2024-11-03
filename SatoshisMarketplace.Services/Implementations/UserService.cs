using SatoshisMarketplace.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Implementations
{
    public class UserService : IUserService
    {
        public Task<Models.UserService.UserModel> GetUser(string username)
        {
            // check is username used

            //return result
            return null;
        }
        public Task<Models.UserService.UserModel> RegisterUserAsync(Models.UserService.UserRegistrationModel model)
        {
            // check is username used

            // register new user

            // add log that action

            //return result
            var user = new Models.UserService.UserModel
            {
                Username = model.Username
            };
            return Task.FromResult(user);
        }

        public Task<Models.UserService.UserModel> LoginAsync(Models.UserService.UserLoginModel model)
        {
            // search for account by username

            // check password is correct

            // add log that action

            //return result
            var user = new Models.UserService.UserModel
            {
                Username = model.Username
            };
            return Task.FromResult(user);
        }

        public Task<bool> LogoutAsync(Models.UserService.UserLogoutModel model)
        {
            // search for account by username

            // add log that action

            //return result
            return Task.FromResult(true);
        }

        public Task<bool> ChangeUserPasswordAsync(Models.UserService.UserChangePasswordModel model)
        {
            // check is username used

            // register new user

            // add log that action

            //return result
            return Task.FromResult(true);
        }
    }
}
