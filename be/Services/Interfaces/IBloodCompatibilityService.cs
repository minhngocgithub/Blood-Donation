using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IBloodCompatibilityService
    {
        Task<IEnumerable<BloodCompatibility>> GetAllBloodCompatibilitiesAsync();
        Task<BloodCompatibility> GetBloodCompatibilityByIdAsync(int id);
        Task<IEnumerable<BloodCompatibility>> GetCompatibleTypesForBloodTypeAsync(int bloodTypeId);
        Task<BloodCompatibility> CreateBloodCompatibilityAsync(BloodCompatibility bloodCompatibility);
        Task<BloodCompatibility> UpdateBloodCompatibilityAsync(int id, BloodCompatibility bloodCompatibility);
        Task<bool> DeleteBloodCompatibilityAsync(int id);
    }
}
