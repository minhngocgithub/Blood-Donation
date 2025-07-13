using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IBloodCompatibilityService
    {
        // Basic CRUD operations
        Task<BloodCompatibilityDto?> GetCompatibilityByIdAsync(int id);
        Task<IEnumerable<BloodCompatibilityDto>> GetAllCompatibilitiesAsync();
        Task<BloodCompatibilityDto> CreateCompatibilityAsync(BloodCompatibilityCreateDto createDto);
        Task<bool> UpdateCompatibilityAsync(int id, BloodCompatibilityUpdateDto updateDto);
        Task<bool> DeleteCompatibilityAsync(int id);
        
        // Compatibility queries
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibleBloodTypesAsync(int fromBloodTypeId);
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibleRecipientsAsync(int toBloodTypeId);
        Task<BloodCompatibilityDto?> GetCompatibilityAsync(int fromBloodTypeId, int toBloodTypeId);
        
        // Compatibility validation
        Task<bool> IsCompatibleAsync(int fromBloodTypeId, int toBloodTypeId);
        Task<bool> IsCompatibilityExistsAsync(int fromBloodTypeId, int toBloodTypeId);
        Task<bool> IsValidBloodTypeAsync(int bloodTypeId);
        
        // Compatibility matrix
        Task<Dictionary<string, List<string>>> GetCompatibilityMatrixAsync();
        Task<List<string>> GetCompatibleBloodTypeNamesAsync(int fromBloodTypeId);
        Task<List<string>> GetCompatibleRecipientNamesAsync(int toBloodTypeId);
        
        // Compatibility search
        Task<IEnumerable<BloodCompatibilityDto>> SearchCompatibilitiesAsync(string searchTerm);
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibilitiesByFromTypeAsync(int fromBloodTypeId);
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibilitiesByToTypeAsync(int toBloodTypeId);
        
        // Compatibility statistics
        Task<int> GetTotalCompatibilitiesAsync();
        Task<int> GetCompatibleTypesCountAsync(int fromBloodTypeId);
        Task<int> GetCompatibleRecipientsCountAsync(int toBloodTypeId);
        
        // Compatibility recommendations
        Task<IEnumerable<BloodTypeDto>> GetRecommendedDonorsAsync(int requiredBloodTypeId);
        Task<IEnumerable<BloodTypeDto>> GetRecommendedRecipientsAsync(int availableBloodTypeId);
        Task<string> GetCompatibilityExplanationAsync(int fromBloodTypeId, int toBloodTypeId);
        
        // Compatibility validation for donations
        Task<bool> IsDonationCompatibleAsync(int donorBloodTypeId, int recipientBloodTypeId);
        Task<bool> ValidateDonationCompatibilityAsync(int donationId);
        Task<IEnumerable<BloodTypeDto>> GetCompatibleDonorsForEventAsync(int eventId, int requiredBloodTypeId);
        
        // Compatibility rules management
        Task<bool> AddCompatibilityRuleAsync(int fromBloodTypeId, int toBloodTypeId);
        Task<bool> RemoveCompatibilityRuleAsync(int fromBloodTypeId, int toBloodTypeId);
        Task<bool> UpdateCompatibilityRulesAsync(List<BloodCompatibilityCreateDto> rules);
        
        // Compatibility reporting
        Task<Dictionary<string, int>> GetCompatibilityUsageAsync();
        Task<Dictionary<string, List<string>>> GetCompatibilityChartAsync();
        
        // Emergency compatibility
        Task<IEnumerable<BloodTypeDto>> GetEmergencyCompatibleTypesAsync(int requiredBloodTypeId);
        Task<bool> IsEmergencyCompatibleAsync(int fromBloodTypeId, int toBloodTypeId);
        
        // Compatibility education
        Task<string> GetCompatibilityGuidelinesAsync();
        Task<string> GetBloodTypeInformationAsync(int bloodTypeId);
        Task<Dictionary<string, string>> GetAllBloodTypeInformationAsync();
    }
} 