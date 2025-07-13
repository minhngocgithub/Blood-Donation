using BloodDonationAPI.Models.DTOs;

namespace BloodDonationAPI.Services.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetAllNewsAsync();
        Task<NewsDto?> GetNewsByIdAsync(int id);
        Task<List<NewsDto>> GetNewsByCategoryAsync(int categoryId);
        Task<List<NewsDto>> GetLatestNewsAsync(int count);
        Task<List<NewsDto>> GetPublishedNewsAsync();
        Task<(bool succeeded, string message)> CreateNewsAsync(NewsDto newsDto);
        Task<(bool succeeded, string message)> UpdateNewsAsync(NewsDto newsDto);
        Task<(bool succeeded, string message)> DeleteNewsAsync(int id);
        Task<(bool succeeded, string message)> PublishNewsAsync(int id);
        Task<List<NewsCategoryDto>> GetAllCategoriesAsync();
        Task<List<NewsDto>> SearchNewsAsync(string query);
    }
}
