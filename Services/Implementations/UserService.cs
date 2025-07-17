using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Services.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null) return null;

                return new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    BloodTypeId = user.BloodTypeId,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive,
                    EmailVerified = user.EmailVerified,
                    LastDonationDate = user.LastDonationDate,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate,
                    BloodTypeName = user.BloodType?.BloodTypeName,
                    RoleName = user.Role?.RoleName,
                    RoleDescription = user.Role?.Description
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null) return null;

                return new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    BloodTypeId = user.BloodTypeId,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive,
                    EmailVerified = user.EmailVerified,
                    LastDonationDate = user.LastDonationDate,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate,
                    BloodTypeName = user.BloodType?.BloodTypeName,
                    RoleName = user.Role?.RoleName,
                    RoleDescription = user.Role?.Description
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null) return null;

                return new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    BloodTypeId = user.BloodTypeId,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive,
                    EmailVerified = user.EmailVerified,
                    LastDonationDate = user.LastDonationDate,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate,
                    BloodTypeName = user.BloodType?.BloodTypeName,
                    RoleName = user.Role?.RoleName,
                    RoleDescription = user.Role?.Description
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
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
            catch (Exception ex)
            {
                return new List<UserDto>();
            }
        }

        public async Task<PagedResponseDto<UserDto>> GetUsersPagedAsync(UserSearchDto searchDto)
        {
            try
            {
                var query = _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                {
                    query = query.Where(u => 
                        u.FullName.Contains(searchDto.SearchTerm) ||
                        u.Email.Contains(searchDto.SearchTerm) ||
                        u.Username.Contains(searchDto.SearchTerm) ||
                        (u.Phone != null && u.Phone.Contains(searchDto.SearchTerm)));
                }

                if (searchDto.BloodTypeId.HasValue)
                {
                    query = query.Where(u => u.BloodTypeId == searchDto.BloodTypeId);
                }

                if (searchDto.RoleId.HasValue)
                {
                    query = query.Where(u => u.RoleId == searchDto.RoleId);
                }

                if (searchDto.IsActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == searchDto.IsActive);
                }

                if (searchDto.EmailVerified.HasValue)
                {
                    query = query.Where(u => u.EmailVerified == searchDto.EmailVerified);
                }

                if (!string.IsNullOrEmpty(searchDto.Gender))
                {
                    query = query.Where(u => u.Gender == searchDto.Gender);
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(searchDto.SortBy))
                {
                    query = searchDto.SortBy.ToLower() switch
                    {
                        "fullname" => searchDto.SortOrder == "desc" ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName),
                        "email" => searchDto.SortOrder == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                        "createddate" => searchDto.SortOrder == "desc" ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate),
                        _ => query.OrderBy(u => u.FullName)
                    };
                }
                else
                {
                    query = query.OrderBy(u => u.FullName);
                }

                var totalCount = await query.CountAsync();
                var pageSize = searchDto.PageSize ?? 10;
                var pageNumber = searchDto.Page ?? 1;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var users = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = users.Select(u => new UserDto
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
                }).ToList();

                return new PagedResponseDto<UserDto>
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages
                };
            }
            catch (Exception ex)
            {
                return new PagedResponseDto<UserDto>
                {
                    Items = new List<UserDto>(),
                    TotalCount = 0,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };
            }
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto createDto)
        {
            try
            {
                if (await IsEmailExistsAsync(createDto.Email))
                {
                    throw new InvalidOperationException("Email already exists");
                }

                if (await IsUsernameExistsAsync(createDto.Username))
                {
                    throw new InvalidOperationException("Username already exists");
                }

                var user = new User
                {
                    Username = createDto.Username,
                    Email = createDto.Email,
                    FullName = createDto.FullName,
                    Phone = createDto.Phone,
                    Address = createDto.Address,
                    DateOfBirth = createDto.DateOfBirth,
                    Gender = createDto.Gender,
                    BloodTypeId = createDto.BloodTypeId,
                    RoleId = createDto.RoleId,
                    PasswordHash = await HashPasswordAsync(createDto.Password),
                    IsActive = true,
                    EmailVerified = false,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return await GetUserByIdAsync(user.UserId) ?? new UserDto();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.FullName = updateDto.FullName;
                user.Phone = updateDto.Phone;
                user.Address = updateDto.Address;
                user.DateOfBirth = updateDto.DateOfBirth;
                user.Gender = updateDto.Gender;
                user.BloodTypeId = updateDto.BloodTypeId;
                user.IsActive = updateDto.IsActive;
                user.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // User status operations
        public async Task<bool> ActivateUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.IsActive = true;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.IsActive = false;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.IsActive = true;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> VerifyEmailAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                user.EmailVerified = true;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // User role operations
        public async Task<bool> AssignRoleAsync(int userId, int roleId)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                return user?.Role?.RoleName == roleName;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user?.Role == null) return new List<RoleDto>();

                return new List<RoleDto>
                {
                    new RoleDto
                    {
                        RoleId = user.Role.RoleId,
                        RoleName = user.Role.RoleName,
                        Description = user.Role.Description,
                        CreatedDate = user.Role.CreatedDate
                    }
                };
            }
            catch (Exception ex)
            {
                return new List<RoleDto>();
            }
        }

        // User blood type operations
        public async Task<bool> UpdateBloodTypeAsync(int userId, int bloodTypeId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                var bloodType = await _context.BloodTypes.FindAsync(bloodTypeId);
                if (bloodType == null) return false;

                user.BloodTypeId = bloodTypeId;
                user.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<BloodTypeDto?> GetUserBloodTypeAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BloodType)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user?.BloodType == null) return null;

                return new BloodTypeDto
                {
                    BloodTypeId = user.BloodType.BloodTypeId,
                    BloodTypeName = user.BloodType.BloodTypeName,
                    Description = user.BloodType.Description
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // User statistics
        public async Task<UserDonationHistoryDto> GetUserDonationHistoryAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.DonationHistories)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null) return new UserDonationHistoryDto();

                var lastDonation = user.DonationHistories
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefault();

                var nextEligibleDate = lastDonation?.NextEligibleDate;

                return new UserDonationHistoryDto
                {
                    UserId = user.UserId,
                    UserName = user.FullName,
                    Email = user.Email,
                    TotalDonations = user.DonationHistories.Count,
                    TotalVolume = user.DonationHistories.Sum(d => d.Volume),
                    LastDonationDate = lastDonation?.DonationDate,
                    NextEligibleDate = nextEligibleDate,
                    BloodTypeName = user.BloodType?.BloodTypeName
                };
            }
            catch (Exception ex)
            {
                return new UserDonationHistoryDto();
            }
        }

        public async Task<int> GetUserTotalDonationsAsync(int userId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.UserId == userId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<DateTime?> GetUserLastDonationDateAsync(int userId)
        {
            try
            {
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                return lastDonation?.DonationDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DateTime?> GetUserNextEligibleDateAsync(int userId)
        {
            try
            {
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                return lastDonation?.NextEligibleDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // User search and filtering
        public async Task<IEnumerable<UserDto>> GetUsersByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .Where(u => u.BloodTypeId == bloodTypeId)
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
            catch (Exception ex)
            {
                return new List<UserDto>();
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
            catch (Exception ex)
            {
                return new List<UserDto>();
            }
        }

        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .Where(u => u.IsActive)
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
            catch (Exception ex)
            {
                return new List<UserDto>();
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByGenderAsync(string gender)
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.BloodType)
                    .Include(u => u.Role)
                    .Where(u => u.Gender == gender)
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
            catch (Exception ex)
            {
                return new List<UserDto>();
            }
        }

        // User validation
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsUserEligibleForDonationAsync(int userId)
        {
            try
            {
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                if (lastDonation == null) return true;

                // Check if enough time has passed since last donation (typically 90 days)
                var daysSinceLastDonation = (DateTime.Now - lastDonation.DonationDate).Days;
                return daysSinceLastDonation >= 90;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Helper methods
        private async Task<string> HashPasswordAsync(string password)
        {
            return await Task.FromResult(PasswordHelper.HashPassword(password));
        }
    }
} 