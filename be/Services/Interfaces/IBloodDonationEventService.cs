using BloodDonationAPI.Models.DTOs;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IBloodDonationEventService
    {
        Task<List<BloodDonationEventDto>> GetAllEventsAsync();
        Task<BloodDonationEventDto?> GetEventByIdAsync(int id);
        Task<List<BloodDonationEventDto>> GetUpcomingEventsAsync();
        Task<List<BloodDonationEventDto>> GetEventsByLocationAsync(int locationId);
        Task<(bool succeeded, string message)> CreateEventAsync(BloodDonationEventDto eventDto);
        Task<(bool succeeded, string message)> UpdateEventAsync(BloodDonationEventDto eventDto);
        Task<(bool succeeded, string message)> DeleteEventAsync(int id);
        Task<(bool succeeded, string message)> RegisterForEventAsync(int userId, int eventId, DateTime preferredDate);
        Task<List<DonationRegistrationDto>> GetUserRegistrationsAsync(int userId);
        Task<bool> HasUserRegisteredForEventAsync(int userId, int eventId);
        Task<int> GetEventRegistrationCountAsync(int eventId);
    }
}
