using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blood_Donation_Website.Services.Implementations
{
    public class DonationRegistrationService : IDonationRegistrationService
    {
        public Task<IEnumerable<DonationRegistrationDto>> GetAllRegistrationsAsync()
            => throw new NotImplementedException();
        public Task<DonationRegistrationDto> GetRegistrationByIdAsync(int id)
            => throw new NotImplementedException();
        public Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByUserIdAsync(int userId)
            => throw new NotImplementedException();
        public Task<DonationRegistrationDto> CreateRegistrationAsync(DonationRegistrationDto registrationDto)
            => throw new NotImplementedException();
        public Task<DonationRegistrationDto> UpdateRegistrationAsync(int id, DonationRegistrationDto registrationDto)
            => throw new NotImplementedException();
        public Task<bool> DeleteRegistrationAsync(int id)
            => throw new NotImplementedException();
        public Task<bool> ApproveRegistrationAsync(int id)
            => throw new NotImplementedException();
        public Task<bool> RejectRegistrationAsync(int id, string reason)
            => throw new NotImplementedException();
    }
} 