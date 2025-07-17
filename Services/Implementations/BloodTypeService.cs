using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class BloodTypeService : IBloodTypeService
    {
        private readonly ApplicationDbContext _context;

        public BloodTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<BloodTypeDto?> GetBloodTypeByIdAsync(int bloodTypeId)
        {
            try
            {
                var bloodType = await _context.BloodTypes.FindAsync(bloodTypeId);
                if (bloodType == null) return null;

                return new BloodTypeDto
                {
                    BloodTypeId = bloodType.BloodTypeId,
                    BloodTypeName = bloodType.BloodTypeName,
                    Description = bloodType.Description
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BloodTypeDto?> GetBloodTypeByNameAsync(string bloodTypeName)
        {
            try
            {
                var bloodType = await _context.BloodTypes
                    .FirstOrDefaultAsync(b => b.BloodTypeName == bloodTypeName);

                if (bloodType == null) return null;

                return new BloodTypeDto
                {
                    BloodTypeId = bloodType.BloodTypeId,
                    BloodTypeName = bloodType.BloodTypeName,
                    Description = bloodType.Description
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<BloodTypeDto>> GetAllBloodTypesAsync()
        {
            try
            {
                var bloodTypes = await _context.BloodTypes
                    .OrderBy(b => b.BloodTypeName)
                    .ToListAsync();

                return bloodTypes.Select(b => new BloodTypeDto
                {
                    BloodTypeId = b.BloodTypeId,
                    BloodTypeName = b.BloodTypeName,
                    Description = b.Description
                });
            }
            catch (Exception ex)
            {
                return new List<BloodTypeDto>();
            }
        }

        public async Task<BloodTypeDto> CreateBloodTypeAsync(BloodTypeCreateDto createDto)
        {
            try
            {
                if (await IsBloodTypeNameExistsAsync(createDto.BloodTypeName))
                {
                    throw new InvalidOperationException("Blood type name already exists");
                }

                var bloodType = new BloodType
                {
                    BloodTypeName = createDto.BloodTypeName,
                    Description = createDto.Description
                };

                _context.BloodTypes.Add(bloodType);
                await _context.SaveChangesAsync();

                return new BloodTypeDto
                {
                    BloodTypeId = bloodType.BloodTypeId,
                    BloodTypeName = bloodType.BloodTypeName,
                    Description = bloodType.Description
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateBloodTypeAsync(int bloodTypeId, BloodTypeUpdateDto updateDto)
        {
            try
            {
                var bloodType = await _context.BloodTypes.FindAsync(bloodTypeId);
                if (bloodType == null) return false;

                bloodType.BloodTypeName = updateDto.BloodTypeName;
                bloodType.Description = updateDto.Description;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                var bloodType = await _context.BloodTypes.FindAsync(bloodTypeId);
                if (bloodType == null) return false;

                // Check if blood type is being used by any users
                var usersWithBloodType = await _context.Users
                    .AnyAsync(u => u.BloodTypeId == bloodTypeId);

                if (usersWithBloodType)
                {
                    throw new InvalidOperationException("Cannot delete blood type that is assigned to users");
                }

                // Check if blood type is being used in donations
                var donationsWithBloodType = await _context.DonationHistories
                    .AnyAsync(d => d.BloodTypeId == bloodTypeId);

                if (donationsWithBloodType)
                {
                    throw new InvalidOperationException("Cannot delete blood type that has donation history");
                }

                _context.BloodTypes.Remove(bloodType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Blood type statistics
        public async Task<BloodTypeStatisticsDto> GetBloodTypeStatisticsAsync(int bloodTypeId)
        {
            try
            {
                var totalDonations = await _context.DonationHistories
                    .Where(d => d.BloodTypeId == bloodTypeId)
                    .CountAsync();

                var totalVolume = await _context.DonationHistories
                    .Where(d => d.BloodTypeId == bloodTypeId)
                    .SumAsync(d => d.Volume);

                var userCount = await _context.Users
                    .Where(u => u.BloodTypeId == bloodTypeId)
                    .CountAsync();

                var bloodType = await _context.BloodTypes.FindAsync(bloodTypeId);

                return new BloodTypeStatisticsDto
                {
                    BloodTypeId = bloodTypeId,
                    BloodTypeName = bloodType?.BloodTypeName ?? "Unknown",
                    TotalDonations = totalDonations,
                    TotalVolume = totalVolume,
                    UserCount = userCount
                };
            }
            catch (Exception ex)
            {
                return new BloodTypeStatisticsDto
                {
                    BloodTypeId = bloodTypeId,
                    BloodTypeName = "Unknown",
                    TotalDonations = 0,
                    TotalVolume = 0,
                    UserCount = 0
                };
            }
        }

        public async Task<IEnumerable<BloodTypeStatisticsDto>> GetAllBloodTypeStatisticsAsync()
        {
            try
            {
                var bloodTypes = await _context.BloodTypes.ToListAsync();
                var statistics = new List<BloodTypeStatisticsDto>();

                foreach (var bloodType in bloodTypes)
                {
                    var stat = await GetBloodTypeStatisticsAsync(bloodType.BloodTypeId);
                    statistics.Add(stat);
                }

                return statistics.OrderByDescending(s => s.TotalDonations);
            }
            catch (Exception ex)
            {
                return new List<BloodTypeStatisticsDto>();
            }
        }

        public async Task<int> GetTotalDonationsByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.BloodTypeId == bloodTypeId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalVolumeByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.BloodTypeId == bloodTypeId)
                    .SumAsync(d => d.Volume);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetUserCountByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.BloodTypeId == bloodTypeId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // Blood type validation
        public async Task<bool> IsBloodTypeExistsAsync(int bloodTypeId)
        {
            try
            {
                return await _context.BloodTypes.AnyAsync(b => b.BloodTypeId == bloodTypeId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsBloodTypeNameExistsAsync(string bloodTypeName)
        {
            try
            {
                return await _context.BloodTypes.AnyAsync(b => b.BloodTypeName == bloodTypeName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Blood type search
        public async Task<IEnumerable<BloodTypeDto>> SearchBloodTypesAsync(string searchTerm)
        {
            try
            {
                var bloodTypes = await _context.BloodTypes
                    .Where(b => b.BloodTypeName.Contains(searchTerm) || 
                               (b.Description != null && b.Description.Contains(searchTerm)))
                    .OrderBy(b => b.BloodTypeName)
                    .ToListAsync();

                return bloodTypes.Select(b => new BloodTypeDto
                {
                    BloodTypeId = b.BloodTypeId,
                    BloodTypeName = b.BloodTypeName,
                    Description = b.Description
                });
            }
            catch (Exception ex)
            {
                return new List<BloodTypeDto>();
            }
        }
    }
} 