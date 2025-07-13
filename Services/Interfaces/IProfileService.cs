using Blood_Donation_Website.Models.ViewModels.Profile;

using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model);
        Task<IEnumerable<BloodType>> GetBloodTypesAsync();
    }
}
