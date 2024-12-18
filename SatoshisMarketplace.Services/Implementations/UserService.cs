﻿using Microsoft.EntityFrameworkCore;
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
                PasswordHash = entity.PasswordHash,
                IsAdministrator = entity.IsAdministator
            };

            return model;
        }

        public async Task<byte[]> GetUserProfilePictureAsync(string username)
        {
            // Fetch the user asynchronously
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

            // Throw exception if user not found
            if (entity == null)
            {
                throw new ArgumentException("User not found!");
            }

            // no image availiable
            if (entity.ProfileImage == null || entity.ProfileImage.Length == 0)
            {
                throw new ArgumentException("User profile picture not found!");
            }

            return entity.ProfileImage;
        }

        public async Task UpdateUserProfilePictureAsync(UpdateUserPictureModel model)
        {
            // 'newProfilePicture' is not valid
            if (model.Image == null || model.Image.Length == 0)
            {
                throw new ArgumentException("New profile picture cannot be null or empty.");
            }

            // Fetch the user asynchronously
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Username == model.Username);

            // Throw exception if user not found
            if (entity == null)
            {
                throw new ArgumentException("User not found!");
            }

            // update profile picture
            entity.ProfileImage = model.Image;

            // add log that action
            var log = new Entities.UserLog()
            {
                Timestamp = DateTime.UtcNow,
                IP = model.IP,
                Username = model.Username,
                Type = UserLogType.ProfilePictureSet

            };
            await _context.UserLogs.AddAsync(log);

            await _context.SaveChangesAsync();
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
                PasswordHash = GetHash(model.Password),
                IsAdministator = false
                
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
                PasswordHash = newUser.PasswordHash,
                IsAdministrator = false
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

        public async Task<bool> ChangeUserPasswordAsync(Models.UserService.UserChangePasswordModel model)
        {
            // find user by username
            var entity = await _context.Users.FirstOrDefaultAsync(user => user.Username == model.Username);
            if (entity == null)
            {
                throw new ArgumentException("User is not found!");
            }

            // check password is correct
            if (!VerifyPassword(model.OldPassword, entity.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid password!");
            }

            // changing password
            entity.PasswordHash = GetHash(model.NewPassword);
            await _context.SaveChangesAsync();

            // add log that action
            var log = new Entities.UserLog()
            {
                Timestamp = DateTime.UtcNow,
                IP = model.IP,
                Username = model.Username,
                Type = UserLogType.PasswordChanged

            };
            await _context.UserLogs.AddAsync(log);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Models.UserService.UserLog>> LogsByUserAsync(string username, int count)
        {
            // load logs from database
            var logs = await _context.UserLogs
                .Where(log => log.Username == username)
                .OrderByDescending(log => log.Timestamp)
                .Take(count)
                .ToListAsync();

            // create responce model
            var model = new List<Models.UserService.UserLog>();

            // fill model
            foreach (var log in logs)
            {
                model.Add(new Models.UserService.UserLog()
                {
                    Timestamp = log.Timestamp,
                    IP = log.IP,
                    LogType = log.Type.ToString()
                });
            }

            return model;
        }

        public async Task<Models.UserService.UserLogs> LogsByUserAsync(string username, int page, int logsPerPage)
        {
            int skipCount = (page - 1) * logsPerPage;

            // load logs from database
            var logs = await _context.UserLogs
                .Where(log => log.Username == username)
                .OrderByDescending(log => log.Timestamp)
                .Skip(skipCount)
                .Take(logsPerPage)
                .ToListAsync();

            int allLogsCount = await _context.UserLogs.Where(log => log.Username == username).CountAsync();
            int pagesCount = (int)Math.Ceiling((double)allLogsCount / logsPerPage);

            // create responce model
            Models.UserService.UserLogs model = new Models.UserService.UserLogs()
            {
                Logs = new List<Models.UserService.UserLog>(),
                AllLogsCount = allLogsCount,
                PagesCount = pagesCount
            };

            // fill model
            foreach (var log in logs)
            {
                model.Logs.Add(new Models.UserService.UserLog()
                {
                    Timestamp = log.Timestamp,
                    IP = log.IP,
                    LogType = log.Type.ToString()
                });
            }

            return model;
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
