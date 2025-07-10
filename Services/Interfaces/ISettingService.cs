using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingDto>> GetAllSettingsAsync();
        Task<SettingDto> GetSettingByKeyAsync(string key);
        Task<SettingDto> CreateSettingAsync(SettingDto settingDto);
        Task<SettingDto> UpdateSettingAsync(string key, SettingDto settingDto);
        Task<bool> DeleteSettingAsync(string key);
        Task<string> GetSettingValueAsync(string key);
        Task<bool> UpdateSettingValueAsync(string key, string value);
    }
}
