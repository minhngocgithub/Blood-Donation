using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Interfaces
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
