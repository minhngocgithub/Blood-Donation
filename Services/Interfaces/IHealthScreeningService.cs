using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IHealthScreeningService
    {
        // Basic CRUD operations
        Task<HealthScreeningDto?> GetScreeningByIdAsync(int screeningId);
        Task<IEnumerable<HealthScreeningDto>> GetAllScreeningsAsync();
        Task<HealthScreeningDto> CreateScreeningAsync(HealthScreeningDto screeningDto);
        Task<bool> UpdateScreeningAsync(int screeningId, HealthScreeningDto screeningDto);
        Task<bool> DeleteScreeningAsync(int screeningId);

        // Status management
        Task<bool> UpdateScreeningStatusAsync(int screeningId, string status);

        // Check-in functionality
        Task<bool> CheckInScreeningAsync(int screeningId);

        // Query operations
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByStatusAsync(string status);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByUserAsync(int userId);
        Task<IEnumerable<HealthScreeningDto>> GetScreeningsByEventAsync(int eventId);

        // Statistics
        Task<object> GetScreeningStatisticsAsync();

        // Validation
        Task<bool> IsEligibleForDonationAsync(int screeningId);
    }
} 