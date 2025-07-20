using Blood_Donation_Website.Models.DTOs;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IRoleService
    {
        // Basic CRUD operations
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto?> GetRoleByNameAsync(RoleType roleName);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> CreateRoleAsync(RoleCreateDto createDto);
        Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto updateDto);
        Task<bool> DeleteRoleAsync(int roleId);
        
        // Role assignment operations
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<bool> IsUserInRoleAsync(int userId, int roleId);
        Task<bool> IsUserInRoleByNameAsync(int userId, RoleType roleName);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<UserDto>> GetUsersByRoleNameAsync(RoleType roleName);
        
        // Role validation
        Task<bool> IsRoleExistsAsync(int roleId);
        Task<bool> IsRoleNameExistsAsync(RoleType roleName);
        
        // Role search
        Task<IEnumerable<RoleDto>> SearchRolesAsync(string searchTerm);
        
        // Role statistics
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<int> GetUserCountByRoleNameAsync(RoleType roleName);
    }
} 