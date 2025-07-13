using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IHealthScreeningService
    {
        // Basic CRUD operations
        Task<HealthScreeningDto?> GetScreeningByIdAsync(int screeningId);
        Task<IEnumerable<HealthScreeningDto>> GetAllScreeningsAsync();
        Task<PagedResponseDto<HealthScreeningDto>> GetScreeningsPagedAsync(SearchParametersDto searchDto);
        Task<HealthScreeningDto> CreateScreeningAsync(HealthScreeningCreateDto createDto);
        Task<bool> UpdateScreeningAsync(int screeningId, HealthScreeningUpdateDto updateDto);
        Task<bool> DeleteScreeningAsync(int screeningId);
        
        // Screening queries
        Task<HealthScreeningDto?> GetScreeningByRegistrationAsync(int registrationId);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByUserAsync(int userId);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByEventAsync(int eventId);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByScreenerAsync(int screenerId);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByEligibilityAsync(bool isEligible);
        
        // Screening status operations
        Task<bool> ApproveScreeningAsync(int screeningId);
        Task<bool> DisqualifyScreeningAsync(int screeningId, string reason);
        Task<bool> IsScreeningEligibleAsync(int screeningId);
        Task<string?> GetDisqualifyReasonAsync(int screeningId);
        
        // Screening validation
        Task<bool> IsScreeningExistsAsync(int screeningId);
        Task<bool> IsScreeningExistsForRegistrationAsync(int registrationId);
        Task<bool> IsWeightValidAsync(decimal weight);
        Task<bool> IsHeightValidAsync(decimal height);
        Task<bool> IsBloodPressureValidAsync(string bloodPressure);
        Task<bool> IsHeartRateValidAsync(int heartRate);
        Task<bool> IsTemperatureValidAsync(decimal temperature);
        Task<bool> IsHemoglobinValidAsync(decimal hemoglobin);
        
        // Screening statistics
        Task<int> GetTotalScreeningsAsync();
        Task<int> GetEligibleScreeningsCountAsync();
        Task<int> GetDisqualifiedScreeningsCountAsync();
        Task<int> GetScreeningsByDateAsync(DateTime date);
        Task<int> GetScreeningsByScreenerAsync(int screenerId);
        
        // Screening search and filtering
        Task<IEnumerable<HealthScreeningDto>> SearchScreeningsAsync(string searchTerm);
        Task<IEnumerable<HealthScreeningDto>> GetEligibleScreeningsAsync();
        Task<IEnumerable<HealthScreeningDto>> GetDisqualifiedScreeningsAsync();
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByBloodPressureRangeAsync(string minBP, string maxBP);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByHemoglobinRangeAsync(decimal minHb, decimal maxHb);
        
        // Screening reporting
        Task<Dictionary<string, int>> GetScreeningResultsByMonthAsync(int year, int month);
        Task<Dictionary<string, int>> GetDisqualificationReasonsAsync();
        Task<Dictionary<string, decimal>> GetAverageVitalsAsync();
        
        // Screening notifications
        Task<bool> SendScreeningResultsAsync(int screeningId);
        Task<bool> SendDisqualificationNotificationAsync(int screeningId);
        Task<bool> SendEligibilityNotificationAsync(int screeningId);
        
        // Screening health checks
        Task<bool> IsWeightInRangeAsync(decimal weight);
        Task<bool> IsHeightInRangeAsync(decimal height);
        Task<bool> IsBloodPressureNormalAsync(string bloodPressure);
        Task<bool> IsHeartRateNormalAsync(int heartRate);
        Task<bool> IsTemperatureNormalAsync(decimal temperature);
        Task<bool> IsHemoglobinSufficientAsync(decimal hemoglobin);
        
        // Screening workflow
        Task<bool> StartScreeningAsync(int registrationId, int screenerId);
        Task<bool> CompleteScreeningAsync(int screeningId);
        Task<bool> ReviewScreeningAsync(int screeningId, int reviewerId);
    }
} 