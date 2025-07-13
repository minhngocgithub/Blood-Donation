using BloodDonationAPI.Models.DTOs;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactMessageDto>> GetAllMessagesAsync();
        Task<ContactMessageDto?> GetMessageByIdAsync(int id);
        Task<(bool succeeded, string message)> CreateMessageAsync(ContactMessageDto messageDto);
        Task<(bool succeeded, string message)> RespondToMessageAsync(int messageId, string response, int respondedByUserId);
        Task<List<ContactMessageDto>> GetPendingMessagesAsync();
        Task<List<ContactMessageDto>> GetMessagesByStatusAsync(string status);
        Task<(bool succeeded, string message)> UpdateMessageStatusAsync(int messageId, string status);
    }
}
