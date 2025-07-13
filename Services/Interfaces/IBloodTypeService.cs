using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IBloodTypeService
    {
        // Basic CRUD operations
        Task<BloodTypeDto?> GetBloodTypeByIdAsync(int bloodTypeId);
        Task<BloodTypeDto?> GetBloodTypeByNameAsync(string bloodTypeName);
        Task<IEnumerable<BloodTypeDto>> GetAllBloodTypesAsync();
        Task<BloodTypeDto> CreateBloodTypeAsync(BloodTypeCreateDto createDto);
        Task<bool> UpdateBloodTypeAsync(int bloodTypeId, BloodTypeUpdateDto updateDto);
        Task<bool> DeleteBloodTypeAsync(int bloodTypeId);
        
        // Blood type statistics
        Task<BloodTypeStatisticsDto> GetBloodTypeStatisticsAsync(int bloodTypeId);
        Task<IEnumerable<BloodTypeStatisticsDto>> GetAllBloodTypeStatisticsAsync();
        Task<int> GetTotalDonationsByBloodTypeAsync(int bloodTypeId);
        Task<int> GetTotalVolumeByBloodTypeAsync(int bloodTypeId);
        Task<int> GetUserCountByBloodTypeAsync(int bloodTypeId);
        
        // Blood type validation
        Task<bool> IsBloodTypeExistsAsync(int bloodTypeId);
        Task<bool> IsBloodTypeNameExistsAsync(string bloodTypeName);
        
        // Blood type search
        Task<IEnumerable<BloodTypeDto>> SearchBloodTypesAsync(string searchTerm);
    }
} 