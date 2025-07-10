using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blood_Donation_Website.Services.Implementations
{
    public class BloodTypeService : IBloodTypeService
    {
        public Task<IEnumerable<BloodTypeDto>> GetAllBloodTypesAsync()
            => throw new NotImplementedException();
        public Task<BloodTypeDto> GetBloodTypeByIdAsync(int id)
            => throw new NotImplementedException();
        public Task<BloodTypeDto> CreateBloodTypeAsync(BloodTypeDto bloodTypeDto)
            => throw new NotImplementedException();
        public Task<BloodTypeDto> UpdateBloodTypeAsync(int id, BloodTypeDto bloodTypeDto)
            => throw new NotImplementedException();
        public Task<bool> DeleteBloodTypeAsync(int id)
            => throw new NotImplementedException();
    }
} 