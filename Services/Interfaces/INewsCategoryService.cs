using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface INewsCategoryService
    {
        // Basic CRUD operations
        Task<NewsCategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<NewsCategoryDto?> GetCategoryByNameAsync(string categoryName);
        Task<IEnumerable<NewsCategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<NewsCategoryDto>> GetActiveCategoriesAsync();
        Task<NewsCategoryDto> CreateCategoryAsync(NewsCategoryCreateDto createDto);
        Task<bool> UpdateCategoryAsync(int categoryId, NewsCategoryUpdateDto updateDto);
        Task<bool> DeleteCategoryAsync(int categoryId);
        
        // Category status operations
        Task<bool> ActivateCategoryAsync(int categoryId);
        Task<bool> DeactivateCategoryAsync(int categoryId);
        Task<bool> IsCategoryActiveAsync(int categoryId);
        
        // Category queries
        Task<IEnumerable<NewsCategoryDto>> GetCategoriesWithNewsCountAsync();
        Task<IEnumerable<NewsCategoryDto>> GetCategoriesByNewsCountAsync(int minCount);
        Task<IEnumerable<NewsCategoryDto>> GetPopularCategoriesAsync(int count = 10);
        
        // Category search and filtering
        Task<IEnumerable<NewsCategoryDto>> SearchCategoriesAsync(string searchTerm);
        Task<IEnumerable<NewsCategoryDto>> GetCategoriesByDescriptionAsync(string description);
        
        // Category statistics
        Task<int> GetTotalCategoriesAsync();
        Task<int> GetActiveCategoriesCountAsync();
        Task<int> GetNewsCountByCategoryAsync(int categoryId);
        Task<int> GetTotalNewsInCategoriesAsync();
        
        // Category validation
        Task<bool> IsCategoryExistsAsync(int categoryId);
        Task<bool> IsCategoryNameExistsAsync(string categoryName);
        
        // Category news management
        Task<IEnumerable<NewsDto>> GetNewsByCategoryAsync(int categoryId);
        Task<IEnumerable<NewsDto>> GetPublishedNewsByCategoryAsync(int categoryId);
        Task<bool> MoveNewsToCategoryAsync(int newsId, int categoryId);
        Task<bool> RemoveNewsFromCategoryAsync(int newsId);
        
        // Category hierarchy (for future use)
        Task<bool> SetParentCategoryAsync(int categoryId, int? parentCategoryId);
        Task<NewsCategoryDto?> GetParentCategoryAsync(int categoryId);
        Task<IEnumerable<NewsCategoryDto>> GetChildCategoriesAsync(int categoryId);
        Task<IEnumerable<NewsCategoryDto>> GetCategoryTreeAsync();
    }
} 