using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
        Task<NotificationDto?> GetNotificationByIdAsync(int id);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        Task<NotificationDto> CreateNotificationAsync(NotificationDto notificationDto);
        Task<NotificationDto?> UpdateNotificationAsync(int id, NotificationDto notificationDto);
        Task<bool> DeleteNotificationAsync(int id);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAllAsReadAsync(int userId);
    }
}
