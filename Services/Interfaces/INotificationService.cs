using Blood_Donation_Website.Models.DTOs;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface INotificationService
    {
        // Basic CRUD operations
        Task<NotificationDto?> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
        Task<PagedResponseDto<NotificationDto>> GetNotificationsPagedAsync(SearchParametersDto searchDto);
        Task<NotificationDto> CreateNotificationAsync(NotificationCreateDto createDto);
        Task<bool> UpdateNotificationAsync(int notificationId, NotificationUpdateDto updateDto);
        Task<bool> DeleteNotificationAsync(int notificationId);
        
        // Notification queries
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetReadNotificationsAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetNotificationsByTypeAsync(NotificationType type);
        Task<IEnumerable<NotificationDto>> GetNotificationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<NotificationDto>> GetRecentNotificationsAsync(int userId, int count = 10);
        
        // Notification status operations
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAsUnreadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<bool> IsNotificationReadAsync(int notificationId);
        
        // Notification types
        Task<IEnumerable<NotificationDto>> GetSystemNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetEventNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetDonationNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetNewsNotificationsAsync();
        
        // Notification statistics
        Task<int> GetTotalNotificationsAsync();
        Task<int> GetUnreadNotificationsCountAsync(int userId);
        Task<int> GetNotificationsCountByUserAsync(int userId);
        Task<int> GetNotificationsCountByTypeAsync(NotificationType type);
        Task<int> GetNotificationsCountByDateAsync(DateTime date);
        
        // Notification search and filtering
        Task<IEnumerable<NotificationDto>> SearchNotificationsAsync(string searchTerm);
        Task<IEnumerable<NotificationDto>> GetNotificationsByTitleAsync(string title);
        Task<IEnumerable<NotificationDto>> GetNotificationsByMessageAsync(string message);
        
        // Notification validation
        Task<bool> IsNotificationExistsAsync(int notificationId);
        Task<bool> IsNotificationBelongsToUserAsync(int notificationId, int userId);
        
        // Notification sending
        Task<bool> SendNotificationToUserAsync(int userId, string title, string message, string? type = null);
        Task<bool> SendNotificationToUsersAsync(IEnumerable<int> userIds, string title, string message, string? type = null);
        Task<bool> SendSystemNotificationAsync(string title, string message);
        Task<bool> SendEventNotificationAsync(int eventId, string title, string message);
        
        // Notification templates
        Task<bool> SendWelcomeNotificationAsync(int userId);
        Task<bool> SendEventReminderNotificationAsync(int userId, int eventId);
        Task<bool> SendDonationConfirmationNotificationAsync(int userId, int donationId);
        Task<bool> SendEligibilityNotificationAsync(int userId);
        Task<bool> SendNewsPublishedNotificationAsync(int userId, int newsId);
        
        // Notification cleanup
        Task<bool> DeleteOldNotificationsAsync(int daysOld);
        Task<bool> DeleteReadNotificationsAsync(int userId);
        Task<bool> DeleteNotificationsByTypeAsync(NotificationType type);
        
        // Notification preferences (for future use)
        Task<bool> SetNotificationPreferenceAsync(int userId, NotificationType type, bool enabled);
        Task<bool> GetNotificationPreferenceAsync(int userId, NotificationType type);
        Task<Dictionary<NotificationType, bool>> GetNotificationPreferencesAsync(int userId);
    }
} 