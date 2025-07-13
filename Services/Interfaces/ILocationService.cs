using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface ILocationService
    {
        // Basic CRUD operations
        Task<LocationDto?> GetLocationByIdAsync(int locationId);
        Task<LocationDto?> GetLocationByNameAsync(string locationName);
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync();
        Task<IEnumerable<LocationDto>> GetActiveLocationsAsync();
        Task<LocationDto> CreateLocationAsync(LocationCreateDto createDto);
        Task<bool> UpdateLocationAsync(int locationId, LocationUpdateDto updateDto);
        Task<bool> DeleteLocationAsync(int locationId);
        
        // Location status operations
        Task<bool> ActivateLocationAsync(int locationId);
        Task<bool> DeactivateLocationAsync(int locationId);
        Task<bool> IsLocationActiveAsync(int locationId);
        
        // Location capacity operations
        Task<bool> UpdateLocationCapacityAsync(int locationId, int capacity);
        Task<int> GetLocationCapacityAsync(int locationId);
        Task<int> GetAvailableCapacityAsync(int locationId);
        
        // Location events
        Task<IEnumerable<BloodDonationEventDto>> GetLocationEventsAsync(int locationId);
        Task<IEnumerable<BloodDonationEventDto>> GetLocationUpcomingEventsAsync(int locationId);
        Task<int> GetLocationEventCountAsync(int locationId);
        
        // Location search and filtering
        Task<IEnumerable<LocationDto>> SearchLocationsAsync(string searchTerm);
        Task<IEnumerable<LocationDto>> GetLocationsByCapacityAsync(int minCapacity);
        Task<IEnumerable<LocationDto>> GetLocationsByAddressAsync(string address);
        
        // Location validation
        Task<bool> IsLocationExistsAsync(int locationId);
        Task<bool> IsLocationNameExistsAsync(string locationName);
        
        // Location statistics
        Task<int> GetTotalEventsAtLocationAsync(int locationId);
        Task<int> GetTotalDonationsAtLocationAsync(int locationId);
    }
} 