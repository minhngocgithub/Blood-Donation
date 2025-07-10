using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Services.Interfaces
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
