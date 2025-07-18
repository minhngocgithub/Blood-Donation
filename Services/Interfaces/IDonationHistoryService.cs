using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IDonationHistoryService
    {
        // Basic CRUD operations
        Task<DonationHistoryDto?> GetDonationByIdAsync(int donationId);
        Task<IEnumerable<DonationHistoryDto>> GetAllDonationsAsync();
        Task<PagedResponseDto<DonationHistoryDto>> GetDonationsPagedAsync(DonationSearchDto searchDto);
        Task<DonationHistoryDto> CreateDonationAsync(DonationHistoryCreateDto createDto);
        Task<bool> UpdateDonationAsync(int donationId, DonationHistoryUpdateDto updateDto);
        Task<bool> DeleteDonationAsync(int donationId);
        
        // Donation status operations
        Task<bool> CompleteDonationAsync(int donationId);
        Task<bool> CancelDonationAsync(int donationId, string reason);
        Task<bool> IssueCertificateAsync(int donationId);
        Task<string> GetDonationStatusAsync(int donationId);
        
        // Donation queries
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByUserAsync(int userId);
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByEventAsync(int eventId);
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByBloodTypeAsync(int bloodTypeId);
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByStatusAsync(string status);
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByRegistrationAsync(int registrationId);
        
        // Donation statistics
        Task<int> GetTotalDonationsAsync();
        Task<int> GetTotalDonationsByUserAsync(int userId);
        Task<int> GetTotalDonationsByEventAsync(int eventId);
        Task<int> GetTotalDonationsByBloodTypeAsync(int bloodTypeId);
        Task<int> GetTotalVolumeAsync();
        Task<int> GetTotalVolumeByUserAsync(int userId);
        Task<int> GetTotalVolumeByEventAsync(int eventId);
        Task<int> GetTotalVolumeByBloodTypeAsync(int bloodTypeId);
        
        // Donation eligibility
        Task<DateTime?> GetUserNextEligibleDateAsync(int userId);
        Task<bool> IsUserEligibleForDonationAsync(int userId);
        Task<bool> CanUserDonateAsync(int userId, DateTime donationDate);
        
        // Donation search and filtering
        Task<IEnumerable<DonationHistoryDto>> SearchDonationsAsync(string searchTerm);
        Task<IEnumerable<DonationHistoryDto>> GetCompletedDonationsAsync();
        Task<IEnumerable<DonationHistoryDto>> GetCancelledDonationsAsync();
        Task<IEnumerable<DonationHistoryDto>> GetDonationsWithCertificatesAsync();
        
        // Donation validation
        Task<bool> IsDonationExistsAsync(int donationId);
        Task<bool> IsDonationDateValidAsync(DateTime donationDate);
        Task<bool> IsDonationVolumeValidAsync(int volume);
        
        // Donation reporting
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByMonthAsync(int year, int month);
        Task<IEnumerable<DonationHistoryDto>> GetDonationsByYearAsync(int year);
        Task<Dictionary<string, int>> GetDonationsByBloodTypeChartAsync();
        Task<Dictionary<string, int>> GetDonationsByMonthChartAsync(int year);
        
        // Donation certificates
        Task<bool> GenerateDonationCertificateAsync(int donationId);
        Task<bool> SendDonationCertificateAsync(int donationId);
        Task<int> GetCertificateCountAsync();
        
        // Donation notifications
        Task<bool> SendDonationConfirmationAsync(int donationId);
        Task<bool> SendDonationReminderAsync(int userId);
        Task<bool> SendEligibilityNotificationAsync(int userId);
    }
} 