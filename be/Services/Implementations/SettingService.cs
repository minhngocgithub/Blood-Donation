using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodDonationAPI.Services.Implementations
{
    public class SettingService : ISettingService
    {
        public Task<IEnumerable<SettingDto>> GetAllSettingsAsync()
            => throw new NotImplementedException();
        public Task<SettingDto> GetSettingByKeyAsync(string key)
            => throw new NotImplementedException();
        public Task<SettingDto> CreateSettingAsync(SettingDto settingDto)
            => throw new NotImplementedException();
        public Task<SettingDto> UpdateSettingAsync(string key, SettingDto settingDto)
            => throw new NotImplementedException();
        public Task<bool> DeleteSettingAsync(string key)
            => throw new NotImplementedException();
        public Task<string> GetSettingValueAsync(string key)
            => throw new NotImplementedException();
        public Task<bool> UpdateSettingValueAsync(string key, string value)
            => throw new NotImplementedException();
    }
} 