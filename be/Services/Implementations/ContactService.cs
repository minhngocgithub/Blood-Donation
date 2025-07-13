using BloodDonationAPI.Data;
using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ContactService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<ContactMessageDto>> GetAllMessagesAsync()
        {
            var messages = await _context.ContactMessages
                .Include(m => m.ResolvedByUser)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();

            return messages.Select(MapToDto).ToList();
        }

        public async Task<ContactMessageDto?> GetMessageByIdAsync(int id)
        {
            var message = await _context.ContactMessages
                .Include(m => m.ResolvedByUser)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            return message == null ? null : MapToDto(message);
        }

        public async Task<(bool succeeded, string message)> CreateMessageAsync(ContactMessageDto messageDto)
        {
            try
            {
                var contactMessage = new ContactMessage
                {
                    FullName = messageDto.Name,
                    Email = messageDto.Email,
                    Phone = messageDto.Phone,
                    Subject = messageDto.Subject,
                    Message = messageDto.Message,
                    Status = "New",
                    CreatedDate = DateTime.UtcNow
                };

                _context.ContactMessages.Add(contactMessage);
                await _context.SaveChangesAsync();

                // Send confirmation email (generic)
                await _emailService.SendEmailAsync(messageDto.Email, "Xác nhận liên hệ", $"Cảm ơn {messageDto.Name} đã liên hệ với chúng tôi. Chúng tôi sẽ phản hồi sớm nhất có thể.");

                // Notify admins
                await NotifyAdminsAboutNewMessage(contactMessage.MessageId);

                return (true, "Gửi tin nhắn thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi gửi tin nhắn: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> RespondToMessageAsync(int messageId, string response, int respondedByUserId)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null)
                {
                    return (false, "Không tìm thấy tin nhắn");
                }

                // No Response/RespondedByUserId/ResponseDate in entity, so use ResolvedBy/ResolvedByUser/ResolvedDate
                message.Status = "Responded";
                message.ResolvedBy = respondedByUserId;
                message.ResolvedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Send response email (generic)
                await _emailService.SendEmailAsync(message.Email, "Phản hồi liên hệ", response);

                return (true, "Phản hồi tin nhắn thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi phản hồi tin nhắn: " + ex.Message);
            }
        }

        public async Task<List<ContactMessageDto>> GetPendingMessagesAsync()
        {
            var messages = await _context.ContactMessages
                .Include(m => m.ResolvedByUser)
                .Where(m => m.Status == "New")
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();

            return messages.Select(MapToDto).ToList();
        }

        public async Task<List<ContactMessageDto>> GetMessagesByStatusAsync(string status)
        {
            var messages = await _context.ContactMessages
                .Include(m => m.ResolvedByUser)
                .Where(m => m.Status == status)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();

            return messages.Select(MapToDto).ToList();
        }

        public async Task<(bool succeeded, string message)> UpdateMessageStatusAsync(int messageId, string status)
        {
            try
            {
                var message = await _context.ContactMessages.FindAsync(messageId);
                if (message == null)
                {
                    return (false, "Không tìm thấy tin nhắn");
                }

                message.Status = status;
                await _context.SaveChangesAsync();

                return (true, "Cập nhật trạng thái tin nhắn thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi cập nhật trạng thái tin nhắn: " + ex.Message);
            }
        }

        private ContactMessageDto MapToDto(ContactMessage message)
        {
            return new ContactMessageDto
            {
                Id = message.MessageId,
                Name = message.FullName,
                Email = message.Email,
                Phone = message.Phone ?? string.Empty,
                Subject = message.Subject,
                Message = message.Message,
                Status = message.Status,
                CreatedDate = message.CreatedDate,
                ResponseDate = message.ResolvedDate,
                Response = null, // No response text in entity
                RespondedByUserId = message.ResolvedBy,
                RespondedByUserName = message.ResolvedByUser?.FullName ?? string.Empty
            };
        }

        private async Task NotifyAdminsAboutNewMessage(int messageId)
        {
            var admins = await _context.Users
                .Where(u => u.Role.RoleName == "Admin" || u.Role.RoleName == "SuperAdmin")
                .ToListAsync();

            foreach (var admin in admins)
            {
                var notification = new Notification
                {
                    UserId = admin.UserId,
                    Title = "Tin nhắn liên hệ mới",
                    Message = "Có tin nhắn liên hệ mới cần được xử lý",
                    Type = "Contact",
                    // No Link property in Notification entity, so skip or add if needed
                    CreatedDate = DateTime.UtcNow
                };

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync();
        }
    }
}
