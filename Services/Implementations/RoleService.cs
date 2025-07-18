using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null) return null;

                return new RoleDto
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description,
                    CreatedDate = role.CreatedDate
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName == roleName);

                if (role == null) return null;

                return new RoleDto
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description,
                    CreatedDate = role.CreatedDate
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _context.Roles
                    .OrderBy(r => r.RoleName)
                    .ToListAsync();

                return roles.Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    CreatedDate = r.CreatedDate
                });
            }
            catch
            {
                return new List<RoleDto>();
            }
        }

        public async Task<RoleDto> CreateRoleAsync(RoleCreateDto createDto)
        {
            try
            {
                if (await IsRoleNameExistsAsync(createDto.RoleName))
                {
                    throw new InvalidOperationException("Role name already exists");
                }

                var role = new Role
                {
                    RoleName = createDto.RoleName,
                    Description = createDto.Description,
                    CreatedDate = DateTime.Now
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return new RoleDto
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description,
                    CreatedDate = role.CreatedDate
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto updateDto)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null) return false;

                role.RoleName = updateDto.RoleName;
                role.Description = updateDto.Description;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null) return false;

                // Check if role is being used by any users
                var usersWithRole = await _context.Users
                    .AnyAsync(u => u.RoleId == roleId);

                if (usersWithRole)
                {
                    throw new InvalidOperationException("Cannot delete role that is assigned to users");
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Role assignment operations
        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                var role = await _context.Roles.FindAsync(roleId);
                if (role == null) return false;

                user.RoleId = roleId;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                // Set to default role (assuming role ID 2 is default user role)
                user.RoleId = 2;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserInRoleAsync(int userId, int roleId)
        {
            try
            {
                return await _context.Users
                    .AnyAsync(u => u.UserId == userId && u.RoleId == roleId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserInRoleByNameAsync(int userId, string roleName)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                return user?.Role?.RoleName == roleName;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .Where(u => u.RoleId == roleId)
                    .OrderBy(u => u.FullName)
                    .ToListAsync();

                return users.Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    Gender = u.Gender,
                    BloodTypeId = u.BloodTypeId,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive,
                    EmailVerified = u.EmailVerified,
                    LastDonationDate = u.LastDonationDate,
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate,
                    BloodTypeName = u.BloodType?.BloodTypeName,
                    RoleName = u.Role?.RoleName,
                    RoleDescription = u.Role?.Description
                });
            }
            catch
            {
                return new List<UserDto>();
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleNameAsync(string roleName)
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .Where(u => u.Role.RoleName == roleName)
                    .OrderBy(u => u.FullName)
                    .ToListAsync();

                return users.Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Address = u.Address,
                    DateOfBirth = u.DateOfBirth,
                    Gender = u.Gender,
                    BloodTypeId = u.BloodTypeId,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive,
                    EmailVerified = u.EmailVerified,
                    LastDonationDate = u.LastDonationDate,
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate,
                    BloodTypeName = u.BloodType?.BloodTypeName,
                    RoleName = u.Role?.RoleName,
                    RoleDescription = u.Role?.Description
                });
            }
            catch
            {
                return new List<UserDto>();
            }
        }

        // Role validation
        public async Task<bool> IsRoleExistsAsync(int roleId)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.RoleId == roleId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsRoleNameExistsAsync(string roleName)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.RoleName == roleName);
            }
            catch
            {
                return false;
            }
        }

        // Role search
        public async Task<IEnumerable<RoleDto>> SearchRolesAsync(string searchTerm)
        {
            try
            {
                var roles = await _context.Roles
                    .Where(r => r.RoleName.Contains(searchTerm) || 
                               (r.Description != null && r.Description.Contains(searchTerm)))
                    .OrderBy(r => r.RoleName)
                    .ToListAsync();

                return roles.Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    CreatedDate = r.CreatedDate
                });
            }
            catch
            {
                return new List<RoleDto>();
            }
        }

        // Role statistics
        public async Task<int> GetUserCountByRoleAsync(int roleId)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.RoleId == roleId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetUserCountByRoleNameAsync(string roleName)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Role)
                    .Where(u => u.Role.RoleName == roleName)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }
    }
} 