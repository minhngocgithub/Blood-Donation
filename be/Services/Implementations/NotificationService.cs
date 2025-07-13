using BloodDonationAPI.Data;
using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public NotificationService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            var notifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
            return notifications.Select(MapToDto);
        }

        public async Task<NotificationDto?> GetNotificationByIdAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return notification != null ? MapToDto(notification) : null;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
            return notifications.Select(MapToDto);
        }

        public async Task<NotificationDto> CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Type = notificationDto.Type,
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return MapToDto(notification);
        }

        public async Task<NotificationDto?> UpdateNotificationAsync(int id, NotificationDto notificationDto)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return null;
            notification.Title = notificationDto.Title;
            notification.Message = notificationDto.Message;
            notification.Type = notificationDto.Type;
            await _context.SaveChangesAsync();
            return MapToDto(notification);
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;
            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool succeeded, string message)> CreateBulkNotificationsAsync(string title, string message, string type, string? link = null)
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.IsActive)
                    .ToListAsync();

                var notifications = users.Select(user => new Notification
                {
                    UserId = user.UserId,
                    Title = title,
                    Message = message,
                    Type = type,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

                _context.Notifications.AddRange(notifications);
                await _context.SaveChangesAsync();

                // Gửi email hàng loạt (có thể thực hiện bất đồng bộ)
                foreach (var user in users)
                {
                    await SendNotificationEmailAsync(notifications.First(n => n.UserId == user.UserId));
                }

                return (true, $"Đã tạo {notifications.Count} thông báo");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi tạo thông báo hàng loạt: " + ex.Message);
            }
        }

        private NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.NotificationId,
                UserId = notification.UserId ?? 0,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type ?? string.Empty,
                IsRead = notification.IsRead,
                CreatedDate = notification.CreatedDate
            };
        }

        private async Task SendNotificationEmailAsync(Notification notification)
        {
            var user = await _context.Users.FindAsync(notification.UserId);
            if (user != null && ShouldSendEmail(notification.Type))
            {
                // await _emailService.SendNotificationEmailAsync(
                //     user.Email,
                //     notification.Title,
                //     notification.Message,
                //     null // No Link property in entity
                // );
            }
        }

        private bool ShouldSendEmail(string? notificationType)
        {
            // Định nghĩa các loại thông báo cần gửi email
            return notificationType switch
            {
                "DonationReminder" => true,
                "EmergencyDonation" => true,
                "EventCancellation" => true,
                "HealthScreeningResult" => true,
                _ => false
            };
        }
    }
}
