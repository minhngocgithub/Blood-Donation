using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
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
