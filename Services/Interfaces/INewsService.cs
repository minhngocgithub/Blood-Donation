using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface INewsService
    {
        // Basic CRUD operations
        Task<NewsDto?> GetNewsByIdAsync(int newsId);
        Task<NewsDto?> GetNewsByTitleAsync(string title);
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<PagedResponseDto<NewsDto>> GetNewsPagedAsync(NewsSearchDto searchDto);
        Task<NewsDto> CreateNewsAsync(NewsCreateDto createDto);
        Task<bool> UpdateNewsAsync(int newsId, NewsUpdateDto updateDto);
        Task<bool> DeleteNewsAsync(int newsId);
        
        // News status operations
        Task<bool> PublishNewsAsync(int newsId);
        Task<bool> UnpublishNewsAsync(int newsId);
        Task<bool> IsNewsPublishedAsync(int newsId);
        Task<DateTime?> GetPublishedDateAsync(int newsId);
        
        // News queries
        Task<IEnumerable<NewsDto>> GetPublishedNewsAsync();
        Task<IEnumerable<NewsDto>> GetUnpublishedNewsAsync();
        Task<IEnumerable<NewsDto>> GetNewsByCategoryAsync(int categoryId);
        Task<IEnumerable<NewsDto>> GetNewsByAuthorAsync(int authorId);
        Task<IEnumerable<NewsDto>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<NewsDto>> GetRecentNewsAsync(int count = 10);
        Task<IEnumerable<NewsDto>> GetPopularNewsAsync(int count = 10);
        
        // News search and filtering
        Task<IEnumerable<NewsDto>> SearchNewsAsync(string searchTerm);
        Task<IEnumerable<NewsDto>> GetNewsByKeywordsAsync(string keywords);
        Task<IEnumerable<NewsDto>> GetNewsByContentAsync(string content);
        
        // News statistics
        Task<int> GetTotalNewsAsync();
        Task<int> GetPublishedNewsCountAsync();
        Task<int> GetUnpublishedNewsCountAsync();
        Task<int> GetNewsCountByCategoryAsync(int categoryId);
        Task<int> GetNewsCountByAuthorAsync(int authorId);
        Task<int> GetNewsViewCountAsync(int newsId);
        
        // News view tracking
        Task<bool> IncrementViewCountAsync(int newsId);
        Task<bool> ResetViewCountAsync(int newsId);
        Task<int> GetTotalViewsAsync();
        
        // News validation
        Task<bool> IsNewsExistsAsync(int newsId);
        Task<bool> IsNewsTitleExistsAsync(string title);
        Task<bool> IsNewsSlugExistsAsync(string slug);
        
        // News categories
        Task<IEnumerable<NewsCategoryDto>> GetNewsCategoriesAsync();
        Task<NewsCategoryDto?> GetNewsCategoryAsync(int newsId);
        Task<bool> AssignCategoryAsync(int newsId, int categoryId);
        Task<bool> RemoveCategoryAsync(int newsId);
        
        // News authors
        Task<UserDto?> GetNewsAuthorAsync(int newsId);
        Task<bool> AssignAuthorAsync(int newsId, int authorId);
        Task<bool> RemoveAuthorAsync(int newsId);
        
        // News publishing workflow
        Task<bool> SubmitForReviewAsync(int newsId);
        Task<bool> ApproveNewsAsync(int newsId, int reviewerId);
        Task<bool> RejectNewsAsync(int newsId, int reviewerId, string reason);
        Task<bool> SchedulePublishAsync(int newsId, DateTime publishDate);
        
        // News notifications
        Task<bool> SendNewsPublishedNotificationAsync(int newsId);
        Task<bool> SendNewsUpdateNotificationAsync(int newsId);
        
        // News SEO
        Task<string> GenerateNewsSlugAsync(string title);
        Task<string> GenerateNewsSummaryAsync(string content);
        Task<bool> UpdateNewsMetaDataAsync(int newsId, string metaTitle, string metaDescription);
    }
} 