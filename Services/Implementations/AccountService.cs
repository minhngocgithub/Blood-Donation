using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.ViewModels.Account;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Services.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<(bool Success, string Message, User? User)> LoginAsync(LoginViewModel model)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

                if (user == null)
                {
                    return (false, "Email hoặc mật khẩu không chính xác", null);
                }

                if (!await VerifyPasswordAsync(model.Password, user.PasswordHash))
                {
                    return (false, "Email hoặc mật khẩu không chính xác", null);
                }

                if (!user.EmailVerified)
                {
                    return (false, "Tài khoản chưa được xác thực. Vui lòng kiểm tra email để xác thực tài khoản", null);
                }

                await UpdateLastLoginAsync(user.Id);
                return (true, "Đăng nhập thành công", user);
            }
            catch (Exception ex)
            {
                return (false, "Có lỗi xảy ra trong quá trình đăng nhập", null);
            }
        }

        public async Task<(bool Success, string Message, User? User)> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                // Kiểm tra user đã tồn tại
                if (await UserExistsAsync(model.Email, model.Email))
                {
                    return (false, "Email đã được sử dụng", null);
                }

                // Tạo user mới
                var user = new User
                {
                    Username = model.Email, // Sử dụng email làm username
                    Email = model.Email,
                    FullName = model.FullName,
                    PasswordHash = await HashPasswordAsync(model.Password),
                    RoleId = 2, // Default role: User
                    IsActive = true,
                    EmailVerified = true, // Tạm thời set true để demo
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Gửi email chào mừng
                try
                {
                    await _emailService.SendWelcomeEmailAsync(user.Email, user.FullName);
                }
                catch (Exception ex)
                {
                    // Log error nhưng không fail registration
                    // _logger.LogError(ex, "Failed to send welcome email");
                }

                return (true, "Đăng ký thành công", user);
            }
            catch (Exception ex)
            {
                return (false, "Có lỗi xảy ra trong quá trình đăng ký", null);
            }
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email || u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            return await Task.FromResult(PasswordHelper.VerifyPassword(password, hashedPassword));
        }

        public async Task<string> HashPasswordAsync(string password)
        {
            return await Task.FromResult(PasswordHelper.HashPassword(password));
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            // Tạo token reset password
            var token = Guid.NewGuid().ToString();

            // Lưu token vào database hoặc cache (tùy implementation)
            // Ở đây tạm thời return true

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            try
            {
                var user = await GetUserByEmailAsync(model.Email);
                if (user == null) return false;

                // Verify token (tùy implementation)

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

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
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

        public async Task<bool> UpdateLastLoginAsync(int userId)
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
