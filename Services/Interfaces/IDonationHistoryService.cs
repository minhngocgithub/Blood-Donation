using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IDonationHistoryService
    {
        Task<IEnumerable<DonationHistoryDto>> GetAllDonationHistoriesAsync();
        Task<DonationHistoryDto> GetDonationHistoryByIdAsync(int id);
        Task<IEnumerable<DonationHistoryDto>> GetDonationHistoriesByUserIdAsync(int userId);
        Task<DonationHistoryDto> CreateDonationHistoryAsync(DonationHistoryDto donationHistoryDto);
        Task<DonationHistoryDto> UpdateDonationHistoryAsync(int id, DonationHistoryDto donationHistoryDto);
        Task<bool> DeleteDonationHistoryAsync(int id);
    }
}
