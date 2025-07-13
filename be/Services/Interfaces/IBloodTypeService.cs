using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface IBloodTypeService
    {
        Task<IEnumerable<BloodTypeDto>> GetAllBloodTypesAsync();
        Task<BloodTypeDto> GetBloodTypeByIdAsync(int id);
        Task<BloodTypeDto> CreateBloodTypeAsync(BloodTypeDto bloodTypeDto);
        Task<BloodTypeDto> UpdateBloodTypeAsync(int id, BloodTypeDto bloodTypeDto);
        Task<bool> DeleteBloodTypeAsync(int id);
    }
}
