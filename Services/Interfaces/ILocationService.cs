using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync();
        Task<LocationDto> GetLocationByIdAsync(int id);
        Task<LocationDto> CreateLocationAsync(LocationDto locationDto);
        Task<LocationDto> UpdateLocationAsync(int id, LocationDto locationDto);
        Task<bool> DeleteLocationAsync(int id);
    }
}
