using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.ViewModels.Account;
using Blood_Donation_Website.Models.DTOs;


namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task<bool> LoginAsync(LoginViewModel model);
        Task<bool> LogoutAsync();
        Task<bool> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string userId);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> VerifyEmailAsync(string userId, string token);
        Task<bool> IsUserInRoleAsync(string userId, string role);
        Task<Models.DTOs.UserDto?> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(string userId, UserDto userDto);
        Task<bool> LockUserAsync(string userId);
        Task<bool> UnlockUserAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(string userId);
    }
}
