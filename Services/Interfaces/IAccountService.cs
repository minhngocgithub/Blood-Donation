using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.ViewModels.Account;
using Blood_Donation_Website.Models.DTOs;


namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Đăng ký tài khoản mới cho người dùng.
        /// </summary>
        Task<bool> RegisterAsync(RegisterViewModel model);
        /// <summary>
        /// Đăng nhập người dùng vào hệ thống.
        /// </summary>
        Task<bool> LoginAsync(LoginViewModel model);
        /// <summary>
        /// Đăng xuất người dùng khỏi hệ thống.
        /// </summary>
        Task<bool> LogoutAsync(string userId);
        /// <summary>
        /// Yêu cầu khôi phục mật khẩu khi người dùng quên mật khẩu.
        /// </summary>
        Task<bool> ForgotPasswordAsync(ForgotPasswordViewModel model);
        /// <summary>
        /// Đặt lại mật khẩu cho tài khoản người dùng.
        /// </summary>
        Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);
        /// <summary>
        /// Thay đổi mật khẩu hiện tại của người dùng.
        /// </summary>
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        /// <summary>
        /// Xác thực email của người dùng bằng token.
        /// </summary>
        Task<bool> VerifyEmailAsync(string userId, string token);
        /// <summary>
        /// Kiểm tra xem email đã tồn tại trong hệ thống chưa.
        /// </summary>
        Task<bool> IsEmailExistsAsync(string email);
        /// <summary>
        /// Kiểm tra xem người dùng có thuộc vai trò cụ thể hay không.
        /// </summary>
        Task<bool> IsUserInRoleAsync(string userId, string role);
        /// <summary>
        /// Khóa tài khoản người dùng.
        /// </summary>
        Task<bool> LockUserAsync(string userId);
        /// <summary>
        /// Mở khóa tài khoản người dùng.
        /// </summary>
        Task<bool> UnlockUserAsync(string userId);
        /// <summary>
        /// Lấy thông tin người dùng theo email.
        /// </summary>
        Task<UserDto?> GetUserByEmailAsync(string email);
    }
}
