using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blood_Donation_Website.Services.Implementations
{
    public class LocationService : ILocationService
    {
        public Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
            => throw new NotImplementedException();
        public Task<LocationDto> GetLocationByIdAsync(int id)
            => throw new NotImplementedException();
        public Task<LocationDto> CreateLocationAsync(LocationDto locationDto)
            => throw new NotImplementedException();
        public Task<LocationDto> UpdateLocationAsync(int id, LocationDto locationDto)
            => throw new NotImplementedException();
        public Task<bool> DeleteLocationAsync(int id)
            => throw new NotImplementedException();
    }
} 