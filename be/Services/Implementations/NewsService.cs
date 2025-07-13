using BloodDonationAPI.Data;
using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;

        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NewsDto>> GetAllNewsAsync()
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return news.Select(MapToDto).ToList();
        }

        public async Task<NewsDto?> GetNewsByIdAsync(int id)
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .FirstOrDefaultAsync(n => n.NewsId == id);

            return news == null ? null : MapToDto(news);
        }

        public async Task<List<NewsDto>> GetNewsByCategoryAsync(int categoryId)
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Where(n => n.CategoryId == categoryId && n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return news.Select(MapToDto).ToList();
        }

        public async Task<List<NewsDto>> GetLatestNewsAsync(int count)
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .Take(count)
                .ToListAsync();

            return news.Select(MapToDto).ToList();
        }

        public async Task<List<NewsDto>> GetPublishedNewsAsync()
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return news.Select(MapToDto).ToList();
        }

        public async Task<(bool succeeded, string message)> CreateNewsAsync(NewsDto newsDto)
        {
            try
            {
                var news = new News
                {
                    Title = newsDto.Title,
                    Content = newsDto.Content,
                    Summary = newsDto.Summary,
                    ImageUrl = newsDto.ImageUrl,
                    CategoryId = newsDto.CategoryId,
                    AuthorId = newsDto.AuthorId,
                    IsPublished = newsDto.IsPublished,
                    PublishedDate = newsDto.IsPublished ? DateTime.UtcNow : null,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.News.Add(news);
                await _context.SaveChangesAsync();

                return (true, "Tạo tin tức thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi tạo tin tức: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> UpdateNewsAsync(NewsDto newsDto)
        {
            try
            {
                var news = await _context.News.FindAsync(newsDto.Id);
                if (news == null)
                {
                    return (false, "Không tìm thấy tin tức");
                }

                news.Title = newsDto.Title;
                news.Content = newsDto.Content;
                news.Summary = newsDto.Summary;
                news.ImageUrl = newsDto.ImageUrl;
                news.CategoryId = newsDto.CategoryId;
                news.IsPublished = newsDto.IsPublished;
                if (!news.IsPublished && newsDto.IsPublished)
                {
                    news.PublishedDate = DateTime.UtcNow;
                }
                news.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return (true, "Cập nhật tin tức thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi cập nhật tin tức: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> DeleteNewsAsync(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null)
                {
                    return (false, "Không tìm thấy tin tức");
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();

                return (true, "Xóa tin tức thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi xóa tin tức: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> PublishNewsAsync(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null)
                {
                    return (false, "Không tìm thấy tin tức");
                }

                news.IsPublished = true;
                news.PublishedDate = DateTime.UtcNow;
                news.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return (true, "Xuất bản tin tức thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi xuất bản tin tức: " + ex.Message);
            }
        }

        public async Task<List<NewsCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.NewsCategories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return categories.Select(MapToCategoryDto).ToList();
        }

        public async Task<List<NewsDto>> SearchNewsAsync(string query)
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Where(n => n.IsPublished && 
                    (n.Title.Contains(query) || 
                     n.Content.Contains(query) || 
                     (n.Summary != null && n.Summary.Contains(query))))
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return news.Select(MapToDto).ToList();
        }

        private NewsDto MapToDto(News news)
        {
            return new NewsDto
            {
                Id = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary ?? string.Empty,
                ImageUrl = news.ImageUrl ?? string.Empty,
                CategoryId = news.CategoryId ?? 0,
                CategoryName = news.Category?.CategoryName ?? string.Empty,
                AuthorId = news.AuthorId ?? 0,
                AuthorName = news.Author?.FullName ?? string.Empty,
                IsPublished = news.IsPublished,
                PublishDate = news.PublishedDate,
                CreatedDate = news.CreatedDate,
                UpdatedDate = news.UpdatedDate
            };
        }

        private NewsCategoryDto MapToCategoryDto(NewsCategory category)
        {
            return new NewsCategoryDto
            {
                Id = category.CategoryId,
                Name = category.CategoryName,
                Description = category.Description ?? string.Empty,
                IsActive = category.IsActive,
                NewsCount = _context.News.Count(n => n.CategoryId == category.CategoryId),
                CreatedDate = DateTime.Now, // NewsCategory entity does not have CreatedDate
                UpdatedDate = null // NewsCategory entity does not have UpdatedDate
            };
        }
    }
}
