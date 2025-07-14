using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IBloodDonationEventService
    {
        // Basic CRUD operations
        Task<BloodDonationEventDto?> GetEventByIdAsync(int eventId);
        Task<BloodDonationEventDto?> GetEventByNameAsync(string eventName);
        Task<IEnumerable<BloodDonationEventDto>> GetAllEventsAsync();
        Task<PagedResponseDto<BloodDonationEventDto>> GetEventsPagedAsync(EventSearchDto searchDto);
        Task<BloodDonationEventDto> CreateEventAsync(BloodDonationEventCreateDto createDto);
        Task<bool> UpdateEventAsync(int eventId, BloodDonationEventUpdateDto updateDto);
        Task<bool> DeleteEventAsync(int eventId);
        
        // Event status operations
        Task<bool> ActivateEventAsync(int eventId);
        Task<bool> DeactivateEventAsync(int eventId);
        Task<bool> CancelEventAsync(int eventId);
        Task<bool> CompleteEventAsync(int eventId);
        Task<string> GetEventStatusAsync(int eventId);
        
        // Event capacity management
        Task<bool> UpdateEventCapacityAsync(int eventId, int maxDonors);
        Task<int> GetEventAvailableSlotsAsync(int eventId);
        Task<bool> IsEventFullAsync(int eventId);
        Task<bool> IncrementCurrentDonorsAsync(int eventId);
        Task<bool> DecrementCurrentDonorsAsync(int eventId);
        
        // Event scheduling
        Task<IEnumerable<BloodDonationEventDto>> GetUpcomingEventsAsync();
        Task<IEnumerable<BloodDonationEventDto>> GetPastEventsAsync();
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByLocationAsync(int locationId);
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByCreatorAsync(int creatorId);
        
        // Event search and filtering
        Task<IEnumerable<BloodDonationEventDto>> SearchEventsAsync(string searchTerm);
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByStatusAsync(string status);
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByBloodTypeAsync(string requiredBloodTypes);
        
        // Event statistics
        Task<EventStatisticsDto> GetEventStatisticsAsync(int eventId);
        Task<IEnumerable<EventStatisticsDto>> GetAllEventStatisticsAsync();
        Task<int> GetEventRegistrationCountAsync(int eventId);
        Task<int> GetEventDonationCountAsync(int eventId);
        
        // Event validation
        Task<bool> IsEventExistsAsync(int eventId);
        Task<bool> IsEventNameExistsAsync(string eventName);
        Task<bool> IsEventDateValidAsync(DateTime eventDate);
        Task<bool> IsEventTimeValidAsync(TimeSpan startTime, TimeSpan endTime);
        
        // Event notifications
        Task<bool> SendEventRemindersAsync(int eventId);
        Task<bool> SendEventUpdatesAsync(int eventId, string updateMessage);
    }
} 