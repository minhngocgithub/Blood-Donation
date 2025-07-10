using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blood_Donation_Website.Services.Implementations
{
    public class DonationHistoryService : IDonationHistoryService
    {
        public Task<IEnumerable<DonationHistoryDto>> GetAllDonationHistoriesAsync()
            => throw new NotImplementedException();
        public Task<DonationHistoryDto> GetDonationHistoryByIdAsync(int id)
            => throw new NotImplementedException();
        public Task<IEnumerable<DonationHistoryDto>> GetDonationHistoriesByUserIdAsync(int userId)
            => throw new NotImplementedException();
        public Task<DonationHistoryDto> CreateDonationHistoryAsync(DonationHistoryDto donationHistoryDto)
            => throw new NotImplementedException();
        public Task<DonationHistoryDto> UpdateDonationHistoryAsync(int id, DonationHistoryDto donationHistoryDto)
            => throw new NotImplementedException();
        public Task<bool> DeleteDonationHistoryAsync(int id)
            => throw new NotImplementedException();
    }
} 