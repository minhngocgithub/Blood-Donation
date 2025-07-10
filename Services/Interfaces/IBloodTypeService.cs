using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
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
