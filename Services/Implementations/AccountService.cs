﻿using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.ViewModels.Account;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Services.Utilities;
using Blood_Donation_Website.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public AccountService(ApplicationDbContext context, IEmailService emailService, IUserService userService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
        }

        
        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

                if (user == null)
                    return false;

                if (!await VerifyPasswordAsync(model.Password, user.PasswordHash))
                    return false;

                if (!user.EmailVerified)
                    return false;

                await UpdateLastLoginAsync(user.UserId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                if (await IsEmailExistsAsync(model.Email))
                    return false;

                var user = new User
                {
                    Username = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    Gender = null,
                    BloodTypeId = model.BloodTypeId,
                    PasswordHash = await HashPasswordAsync(model.Password),
                    RoleId = 2,
                    IsActive = true,
                    EmailVerified = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                if (!string.IsNullOrEmpty(model.Gender))
                {
                    if (Enum.TryParse<Gender>(model.Gender, out var genderValue))
                    {
                        user.Gender = genderValue;
                    }
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                try
                {
                    await _emailService.SendWelcomeEmailAsync(user.Email, user.FullName);
                }
                catch { }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return false;

                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;
                user.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null) return false;

                var token = Guid.NewGuid().ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null) return false;

                user.PasswordHash = await HashPasswordAsync(model.Password);
                user.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                if (!await VerifyPasswordAsync(currentPassword, user.PasswordHash))
                    return false;

                user.PasswordHash = await HashPasswordAsync(newPassword);
                user.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            return await _userService.GetUserByEmailAsync(email);
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            if (int.TryParse(userId, out int id))
            {
                return await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == id);
            }
            return null;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> VerifyEmailAsync(string userId, string token)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                user.EmailVerified = true;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserInRoleAsync(string userId, RoleType role)
        {
            var user = await GetUserByIdAsync(userId);
            return user?.Role?.RoleName == role;
        }

        public async Task<UserDto?> GetUserProfileAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                BloodTypeId = user.BloodTypeId,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                EmailVerified = user.EmailVerified,
                LastDonationDate = user.LastDonationDate,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate
            };
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UserDto userDto)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                user.FullName = userDto.FullName;
                user.Phone = userDto.Phone;
                user.DateOfBirth = userDto.DateOfBirth;
                user.Gender = userDto.Gender;
                user.Address = userDto.Address;
                user.BloodTypeId = userDto.BloodTypeId;
                user.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LockUserAsync(string userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnlockUserAsync(string userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                user.IsActive = true;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.BloodType)
                .ToListAsync();

            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                Address = u.Address,
                BloodTypeId = u.BloodTypeId,
                RoleId = u.RoleId,
                IsActive = u.IsActive,
                EmailVerified = u.EmailVerified,
                LastDonationDate = u.LastDonationDate,
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate,
                BloodTypeName = u.BloodType?.BloodTypeName,
                RoleName = u.Role?.RoleName
            });
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            return await Task.FromResult(PasswordHelper.VerifyPassword(password, hashedPassword));
        }

        private async Task<string> HashPasswordAsync(string password)
        {
            return await Task.FromResult(PasswordHelper.HashPassword(password));
        }

        private async Task<bool> UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
