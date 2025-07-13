using BloodDonationAPI.Models.DTOs;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IHealthScreeningService
    {
        Task<List<HealthScreeningDto>> GetAllScreeningsAsync();
        Task<HealthScreeningDto?> GetScreeningByIdAsync(int id);
        Task<List<HealthScreeningDto>> GetScreeningsByUserAsync(int userId);
        Task<(bool succeeded, string message)> CreateScreeningAsync(HealthScreeningDto screeningDto);
        Task<(bool succeeded, string message)> UpdateScreeningAsync(HealthScreeningDto screeningDto);
        Task<bool> IsUserEligibleToDonateAsync(int userId);
        Task<(bool isEligible, string reason)> CheckDonationEligibilityAsync(HealthScreeningDto screening);
        Task<List<HealthScreeningDto>> GetPendingScreeningsAsync();
        Task<List<HealthScreeningDto>> GetScreeningsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
