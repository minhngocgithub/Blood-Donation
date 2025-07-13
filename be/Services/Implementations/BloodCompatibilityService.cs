using BloodDonationAPI.Models.Entities;
using BloodDonationAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodDonationAPI.Services.Implementations
{
    public class BloodCompatibilityService : IBloodCompatibilityService
    {
        public Task<IEnumerable<BloodCompatibility>> GetAllBloodCompatibilitiesAsync()
            => throw new NotImplementedException();
        public Task<BloodCompatibility> GetBloodCompatibilityByIdAsync(int id)
            => throw new NotImplementedException();
        public Task<IEnumerable<BloodCompatibility>> GetCompatibleTypesForBloodTypeAsync(int bloodTypeId)
            => throw new NotImplementedException();
        public Task<BloodCompatibility> CreateBloodCompatibilityAsync(BloodCompatibility bloodCompatibility)
            => throw new NotImplementedException();
        public Task<BloodCompatibility> UpdateBloodCompatibilityAsync(int id, BloodCompatibility bloodCompatibility)
            => throw new NotImplementedException();
        public Task<bool> DeleteBloodCompatibilityAsync(int id)
            => throw new NotImplementedException();
    }
} 