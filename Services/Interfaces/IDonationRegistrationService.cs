using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IDonationRegistrationService
    {
        Task<IEnumerable<DonationRegistrationDto>> GetAllRegistrationsAsync();
        Task<DonationRegistrationDto> GetRegistrationByIdAsync(int id);
        Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByUserIdAsync(int userId);
        Task<DonationRegistrationDto> CreateRegistrationAsync(DonationRegistrationDto registrationDto);
        Task<DonationRegistrationDto> UpdateRegistrationAsync(int id, DonationRegistrationDto registrationDto);
        Task<bool> DeleteRegistrationAsync(int id);
        Task<bool> ApproveRegistrationAsync(int id);
        Task<bool> RejectRegistrationAsync(int id, string reason);
    }
}
