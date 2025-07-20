using Blood_Donation_Website.Models.DTOs;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IUserService
    {
        // Basic CRUD operations
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<PagedResponseDto<UserDto>> GetUsersPagedAsync(UserSearchDto searchDto);
        Task<UserDto> CreateUserAsync(UserCreateDto createDto);
        Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto);
        Task<bool> DeleteUserAsync(int userId);
        
        // User status operations
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> LockUserAsync(int userId);
        Task<bool> UnlockUserAsync(int userId);
        Task<bool> VerifyEmailAsync(int userId);
        
        // User role operations
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);
        Task<bool> IsUserInRoleAsync(int userId, RoleType roleName);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId);
        
        // User blood type operations
        Task<bool> UpdateBloodTypeAsync(int userId, int bloodTypeId);
        Task<BloodTypeDto?> GetUserBloodTypeAsync(int userId);
        
        // User statistics
        Task<UserDonationHistoryDto> GetUserDonationHistoryAsync(int userId);
        Task<int> GetUserTotalDonationsAsync(int userId);
        Task<DateTime?> GetUserLastDonationDateAsync(int userId);
        Task<DateTime?> GetUserNextEligibleDateAsync(int userId);
        
        // User search and filtering
        Task<IEnumerable<UserDto>> GetUsersByBloodTypeAsync(int bloodTypeId);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<UserDto>> GetActiveUsersAsync();
        Task<IEnumerable<UserDto>> GetUsersByGenderAsync(Gender gender);
        
        // User validation
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsUsernameExistsAsync(string username);
        Task<bool> IsUserEligibleForDonationAsync(int userId);
    }
} 