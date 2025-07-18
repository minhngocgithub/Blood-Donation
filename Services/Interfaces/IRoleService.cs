using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IRoleService
    {
        // Basic CRUD operations
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto?> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> CreateRoleAsync(RoleCreateDto createDto);
        Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto updateDto);
        Task<bool> DeleteRoleAsync(int roleId);
        
        // Role assignment operations
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<bool> IsUserInRoleAsync(int userId, int roleId);
        Task<bool> IsUserInRoleByNameAsync(int userId, string roleName);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<UserDto>> GetUsersByRoleNameAsync(string roleName);
        
        // Role validation
        Task<bool> IsRoleExistsAsync(int roleId);
        Task<bool> IsRoleNameExistsAsync(string roleName);
        
        // Role search
        Task<IEnumerable<RoleDto>> SearchRolesAsync(string searchTerm);
        
        // Role statistics
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<int> GetUserCountByRoleNameAsync(string roleName);
    }
} 