using Blood_Donation_Website.Models.DTOs;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IContactMessageService
    {
        // Basic CRUD operations
        Task<ContactMessageDto?> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<ContactMessageDto>> GetAllMessagesAsync();
        Task<ContactMessageDto> CreateMessageAsync(ContactMessageDto messageDto);
        Task<bool> UpdateMessageAsync(int messageId, ContactMessageDto messageDto);
        Task<bool> DeleteMessageAsync(int messageId);

        // Status management
        Task<bool> UpdateMessageStatusAsync(int messageId, MessageStatus status);

        // Read/Unread management
        Task<bool> MarkAsReadAsync(int messageId);
        Task<bool> MarkAsUnreadAsync(int messageId);

        // Query operations
        Task<IEnumerable<ContactMessageDto>> GetUnreadMessagesAsync();
        Task<IEnumerable<ContactMessageDto>> GetMessagesByStatusAsync(MessageStatus status);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByCategoryAsync(string category);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByPriorityAsync(string priority);

        // Reply functionality
        Task<bool> ReplyToMessageAsync(int messageId, ContactMessageDto replyDto);

        // Statistics
        Task<ContactMessageStatisticsDto> GetMessageStatisticsAsync();

        // Search functionality
        Task<IEnumerable<ContactMessageDto>> SearchMessagesAsync(string searchTerm);
    }
} 