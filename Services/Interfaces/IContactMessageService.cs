using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IContactMessageService
    {
        // Basic CRUD operations
        Task<ContactMessageDto?> GetMessageByIdAsync(int messageId);
        Task<IEnumerable<ContactMessageDto>> GetAllMessagesAsync();
        Task<PagedResponseDto<ContactMessageDto>> GetMessagesPagedAsync(SearchParametersDto searchDto);
        Task<ContactMessageDto> CreateMessageAsync(ContactMessageCreateDto createDto);
        Task<bool> CreateContactMessageAsync(ContactMessageDto model);
        Task<bool> UpdateMessageAsync(int messageId, ContactMessageUpdateDto updateDto);
        Task<bool> DeleteMessageAsync(int messageId);
        
        // Message status operations
        Task<bool> MarkAsNewAsync(int messageId);
        Task<bool> MarkAsInProgressAsync(int messageId);
        Task<bool> MarkAsResolvedAsync(int messageId, int resolvedBy);
        Task<bool> MarkAsClosedAsync(int messageId);
        Task<string> GetMessageStatusAsync(int messageId);
        
        // Message queries
        Task<IEnumerable<ContactMessageDto>> GetMessagesByStatusAsync(string status);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByEmailAsync(string email);
        Task<IEnumerable<ContactMessageDto>> GetMessagesBySubjectAsync(string subject);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByResolverAsync(int resolvedBy);
        Task<IEnumerable<ContactMessageDto>> GetUnresolvedMessagesAsync();
        Task<IEnumerable<ContactMessageDto>> GetResolvedMessagesAsync();
        
        // Message search and filtering
        Task<IEnumerable<ContactMessageDto>> SearchMessagesAsync(string searchTerm);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByFullNameAsync(string fullName);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByPhoneAsync(string phone);
        
        // Message statistics
        Task<int> GetTotalMessagesAsync();
        Task<int> GetNewMessagesCountAsync();
        Task<int> GetInProgressMessagesCountAsync();
        Task<int> GetResolvedMessagesCountAsync();
        Task<int> GetMessagesCountByStatusAsync(string status);
        Task<int> GetMessagesCountByDateAsync(DateTime date);
        Task<int> GetMessagesCountByResolverAsync(int resolvedBy);
        
        // Message validation
        Task<bool> IsMessageExistsAsync(int messageId);
        Task<bool> IsEmailValidAsync(string email);
        Task<bool> IsPhoneValidAsync(string phone);
        
        // Message resolution
        Task<bool> AssignResolverAsync(int messageId, int resolverId);
        Task<UserDto?> GetMessageResolverAsync(int messageId);
        Task<DateTime?> GetResolutionDateAsync(int messageId);
        Task<string?> GetResolutionNotesAsync(int messageId);
        
        // Message response
        Task<bool> SendResponseAsync(int messageId, string response, int responderId);
        Task<bool> SendAutoResponseAsync(int messageId);
        Task<bool> SendResolutionNotificationAsync(int messageId);
        
        // Message categories (for future use)
        Task<bool> CategorizeMessageAsync(int messageId, string category);
        Task<IEnumerable<ContactMessageDto>> GetMessagesByCategoryAsync(string category);
        Task<Dictionary<string, int>> GetMessageCategoriesAsync();
        
        // Message priority
        Task<bool> SetMessagePriorityAsync(int messageId, string priority);
        Task<string> GetMessagePriorityAsync(int messageId);
        Task<IEnumerable<ContactMessageDto>> GetHighPriorityMessagesAsync();
        
        // Message cleanup
        Task<bool> DeleteOldMessagesAsync(int daysOld);
        Task<bool> DeleteResolvedMessagesAsync(int daysOld);
        Task<bool> ArchiveMessagesAsync(int daysOld);
        
        // Message reporting
        Task<Dictionary<string, int>> GetMessagesByStatusChartAsync();
        Task<Dictionary<string, int>> GetMessagesByMonthChartAsync(int year);
        Task<Dictionary<string, int>> GetMessagesByResolverChartAsync();
        Task<Dictionary<string, int>> GetMessagesByCategoryChartAsync();
        
        // Message templates
        Task<bool> SendWelcomeMessageAsync(string email, string fullName);
        Task<bool> SendThankYouMessageAsync(int messageId);
        Task<bool> SendFollowUpMessageAsync(int messageId);
    }
} 