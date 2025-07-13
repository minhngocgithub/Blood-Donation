using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface INewsCategoryService
    {
        Task<IEnumerable<NewsCategoryDto>> GetAllNewsCategoriesAsync();
        Task<NewsCategoryDto> GetNewsCategoryByIdAsync(int id);
        Task<NewsCategoryDto> CreateNewsCategoryAsync(NewsCategoryDto categoryDto);
        Task<NewsCategoryDto> UpdateNewsCategoryAsync(int id, NewsCategoryDto categoryDto);
        Task<bool> DeleteNewsCategoryAsync(int id);
    }
}
