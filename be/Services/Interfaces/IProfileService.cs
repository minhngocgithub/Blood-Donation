using BloodDonationAPI.Models.ViewModels.Profile;

using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model);
        Task<IEnumerable<BloodType>> GetBloodTypesAsync();
    }
}
