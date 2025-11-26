using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface ISettingService
    {
        // Basic CRUD operations
        Task<SettingDto?> GetSettingByIdAsync(int settingId);
        Task<SettingDto?> GetSettingByKeyAsync(string settingKey);
        Task<IEnumerable<SettingDto>> GetAllSettingsAsync();
        Task<SettingDto> CreateSettingAsync(SettingCreateDto createDto);
        Task<bool> UpdateSettingAsync(int settingId, SettingUpdateDto updateDto);
        Task<bool> DeleteSettingAsync(int settingId);
        
        // Setting value operations
        Task<string?> GetSettingValueAsync(string settingKey);
        Task<bool> SetSettingValueAsync(string settingKey, string settingValue);
        Task<bool> UpdateSettingValueAsync(string settingKey, string settingValue);
        Task<bool> DeleteSettingValueAsync(string settingKey);
        
        // Setting validation
        Task<bool> IsSettingExistsAsync(int settingId);
        Task<bool> IsSettingKeyExistsAsync(string settingKey);
        Task<bool> IsSettingValueValidAsync(string settingKey, string settingValue);
        
        // Setting categories
        Task<IEnumerable<SettingDto>> GetSettingsByCategoryAsync(string category);
        Task<IEnumerable<string>> GetSettingCategoriesAsync();
        Task<Dictionary<string, List<SettingDto>>> GetSettingsGroupedByCategoryAsync();
        
        // Setting search and filtering
        Task<IEnumerable<SettingDto>> SearchSettingsAsync(string searchTerm);
        Task<IEnumerable<SettingDto>> GetSettingsByDescriptionAsync(string description);
        Task<IEnumerable<SettingDto>> GetSettingsByKeyPatternAsync(string keyPattern);
        
        // Setting statistics
        Task<int> GetTotalSettingsAsync();
        Task<int> GetSettingsCountByCategoryAsync(string category);
        Task<DateTime> GetLastUpdatedAsync(string settingKey);
        
        // Application settings
        Task<string> GetApplicationNameAsync();
        Task<string> GetApplicationVersionAsync();
        Task<string> GetApplicationDescriptionAsync();
        Task<string> GetContactEmailAsync();
        Task<string> GetContactPhoneAsync();
        Task<string> GetWebsiteUrlAsync();
        
        // Email settings
        Task<string> GetSmtpServerAsync();
        Task<int> GetSmtpPortAsync();
        Task<string> GetSmtpUsernameAsync();
        Task<string> GetSmtpPasswordAsync();
        Task<bool> GetSmtpUseSslAsync();
        Task<string> GetFromEmailAsync();
        Task<string> GetFromNameAsync();
        
        // Donation settings
        Task<int> GetMinimumDonationAgeAsync();
        Task<int> GetMaximumDonationAgeAsync();
        Task<decimal> GetMinimumWeightAsync();
        Task<decimal> GetMinimumHemoglobinAsync();
        Task<int> GetDonationIntervalDaysAsync();
        Task<int> GetDefaultDonationVolumeAsync();
        
        // Event settings
        Task<int> GetDefaultEventCapacityAsync();
        Task<int> GetEventReminderDaysAsync();
        Task<int> GetEventRegistrationDeadlineHoursAsync();
        Task<bool> GetAllowEventRegistrationAsync();
        
        // Notification settings
        Task<bool> GetEmailNotificationsEnabledAsync();
        Task<bool> GetSmsNotificationsEnabledAsync();
        Task<bool> GetPushNotificationsEnabledAsync();
        Task<int> GetNotificationRetentionDaysAsync();
        
        // Security settings
        Task<int> GetPasswordMinLengthAsync();
        Task<bool> GetPasswordRequireUppercaseAsync();
        Task<bool> GetPasswordRequireLowercaseAsync();
        Task<bool> GetPasswordRequireDigitAsync();
        Task<bool> GetPasswordRequireSpecialCharAsync();
        Task<int> GetSessionTimeoutMinutesAsync();
        Task<int> GetMaxLoginAttemptsAsync();
        
        // System settings
        Task<bool> GetMaintenanceModeAsync();
        Task<string> GetMaintenanceMessageAsync();
        Task<bool> GetDebugModeAsync();
        Task<string> GetTimeZoneAsync();
        Task<string> GetDateFormatAsync();
        Task<string> GetTimeFormatAsync();
        
        // Setting initialization
        Task<bool> InitializeDefaultSettingsAsync();
        Task<bool> ResetSettingsToDefaultAsync();
        Task<bool> BackupSettingsAsync();
        Task<bool> RestoreSettingsAsync(string backupData);
        
        // Setting validation rules
        Task<bool> ValidateSettingValueAsync(string settingKey, string settingValue);
        Task<string> GetSettingValidationRuleAsync(string settingKey);
        Task<Dictionary<string, string>> GetSettingValidationRulesAsync();
        
        // Setting cache
        Task<bool> RefreshSettingsCacheAsync();
        Task<bool> ClearSettingsCacheAsync();
        Task<Dictionary<string, string>> GetCachedSettingsAsync();
    }
} 