using Microsoft.EntityFrameworkCore;
using SatoshisMarketplace.Entities;
using SatoshisMarketplace.Services.Interfaces;
using SatoshisMarketplace.Services.Models.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ServerDbContext _context;
        public UserService(ServerDbContext context)
        {
            _context = context;
        }
        public async Task<Models.UserService.UserModel> GetUserAsync(string username)
        {
            // Fetch the user asynchronously
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

            // Throw exception if user not found
            if (entity == null)
            {
                throw new ArgumentException("User not found!");
            }

            // Map entity to UserModel
            var model = new Models.UserService.UserModel
            {
                Username = entity.Username,
                PasswordHash = entity.PasswordHash
            };

            return model;
        }
        public async Task<Models.UserService.UserModel> RegisterUserAsync(Models.UserService.UserRegistrationModel model)
        {
            // check is username free
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Username == model.Username);
            if (entity != null)
            {
                throw new ArgumentException("Username is taken!");
            }

            // register new user
            var newUser = new Entities.User()
            {
                Username = model.Username,
                PasswordHash = GetHash(model.Password)
            };
            await _context.Users.AddAsync(newUser);

            // add log that action
            var log = new Entities.UserLog()
            {
                Timestamp = DateTime.UtcNow,
                IP = model.IP,
                Username = model.Username,
                Type = UserLogType.AccountCreated
            };
            await _context.UserLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            //return result
            var user = new UserModel()
            {
                Username = newUser.Username,
                PasswordHash = newUser.PasswordHash
            };
            return user;
        }

        public async Task<Models.UserService.UserModel> LoginAsync(Models.UserService.UserLoginModel model)
        {
            // search for account by username
            var user = await GetUserAsync(model.Username);

            // check password is correct
            if (!VerifyPassword(model.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid password!");
            }

            // add log that action
            var log = new Entities.UserLog()
            {
                Timestamp = DateTime.UtcNow,
                IP = model.IP,
                Username = model.Username,
                Type = UserLogType.Login

            };
            await _context.UserLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            //return result
            return user;
        }

        public async Task<bool> LogoutAsync(Models.UserService.UserLogoutModel model)
        {
            // search for account by username
            var user = await GetUserAsync(model.Username);

            // add log that action
            var log = new Entities.UserLog()
            {
                Timestamp = DateTime.UtcNow,
                IP = model.IP,
                Username = model.Username,
                Type = UserLogType.Logout

            };
            await _context.UserLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            //return result
            return true;
        }

        public Task<bool> ChangeUserPasswordAsync(Models.UserService.UserChangePasswordModel model)
        {
            // check is username used

            // register new user

            // add log that action

            //return result
            return Task.FromResult(true);
        }

        private bool VerifyPassword(string password, byte[] passwordHash)
        {
            byte[] hash = GetHash(password);

            return hash.SequenceEqual(passwordHash);
        }
        private byte[] GetHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

                // Generating Hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                return hashBytes;
            }
        }
    }
}
