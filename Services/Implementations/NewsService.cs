using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;

        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<NewsDto?> GetNewsByIdAsync(int newsId)
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .FirstOrDefaultAsync(n => n.NewsId == newsId);

                if (news == null) return null;

                return MapToDto(news);
            }
            catch
            {
                return null;
            }
        }

        public async Task<NewsDto?> GetNewsByTitleAsync(string title)
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .FirstOrDefaultAsync(n => n.Title == title);

                if (news == null) return null;

                return MapToDto(news);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<PagedResponseDto<NewsDto>> GetNewsPagedAsync(NewsSearchDto searchDto)
        {
            try
            {
                var query = _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .AsQueryable();

                // Apply filters
                if (searchDto.CategoryId.HasValue)
                {
                    query = query.Where(n => n.CategoryId == searchDto.CategoryId.Value);
                }

                if (searchDto.AuthorId.HasValue)
                {
                    query = query.Where(n => n.AuthorId == searchDto.AuthorId.Value);
                }

                if (searchDto.IsPublished.HasValue)
                {
                    query = query.Where(n => n.IsPublished == searchDto.IsPublished.Value);
                }

                if (searchDto.FromDate.HasValue)
                {
                    query = query.Where(n => n.CreatedDate >= searchDto.FromDate.Value);
                }

                if (searchDto.ToDate.HasValue)
                {
                    query = query.Where(n => n.CreatedDate <= searchDto.ToDate.Value);
                }

                // Apply search term
                if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                {
                    query = query.Where(n =>
                        n.Title.Contains(searchDto.SearchTerm) ||
                        (n.Content != null && n.Content.Contains(searchDto.SearchTerm)) ||
                        (n.Summary != null && n.Summary.Contains(searchDto.SearchTerm)));
                }

                // Apply sorting
                query = (searchDto.SortBy?.ToLower()) switch
                {
                    "title" => searchDto.SortOrder == "desc" ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title),
                    "views" => searchDto.SortOrder == "desc" ? query.OrderByDescending(n => n.ViewCount) : query.OrderBy(n => n.ViewCount),
                    "published" => searchDto.SortOrder == "desc" ? query.OrderByDescending(n => n.PublishedDate) : query.OrderBy(n => n.PublishedDate),
                    _ => query.OrderByDescending(n => n.CreatedDate)
                };

                var totalCount = await query.CountAsync();
                var pageSize = searchDto.PageSize ?? 10;
                var pageNumber = searchDto.Page ?? 1;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var newsList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var newsDtos = newsList.Select(MapToDto).ToList();

                return new PagedResponseDto<NewsDto>
                {
                    Items = newsDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages
                };
            }
            catch
            {
                return new PagedResponseDto<NewsDto>
                {
                    Items = new List<NewsDto>(),
                    TotalCount = 0,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };
            }
        }

        public async Task<NewsDto> CreateNewsAsync(NewsCreateDto createDto)
        {
            var news = new News
            {
                Title = createDto.Title,
                Content = createDto.Content,
                Summary = createDto.Summary,
                ImageUrl = createDto.ImageUrl,
                CategoryId = createDto.CategoryId,
                AuthorId = createDto.AuthorId,
                IsPublished = createDto.IsPublished,
                PublishedDate = createDto.IsPublished ? DateTime.Now : null,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                ViewCount = 0
            };

            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();

            return await GetNewsByIdAsync(news.NewsId) ?? new NewsDto();
        }

        public async Task<bool> UpdateNewsAsync(int newsId, NewsUpdateDto updateDto)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.Title = updateDto.Title;
                news.Content = updateDto.Content;
                news.Summary = updateDto.Summary;
                news.ImageUrl = updateDto.ImageUrl;
                news.CategoryId = updateDto.CategoryId;
                news.IsPublished = updateDto.IsPublished;
                news.PublishedDate = updateDto.PublishedDate;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteNewsAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                _context.News.Remove(news);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // News status operations
        public async Task<bool> PublishNewsAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.IsPublished = true;
                news.PublishedDate = DateTime.Now;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnpublishNewsAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.IsPublished = false;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsNewsPublishedAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                return news?.IsPublished ?? false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DateTime?> GetPublishedDateAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                return news?.PublishedDate;
            }
            catch
            {
                return null;
            }
        }

        // News queries
        public async Task<IEnumerable<NewsDto>> GetPublishedNewsAsync()
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.IsPublished)
                    .OrderByDescending(n => n.PublishedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetUnpublishedNewsAsync()
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => !n.IsPublished)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByCategoryAsync(int categoryId)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.CategoryId == categoryId)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByAuthorAsync(int authorId)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.AuthorId == authorId)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetRecentNewsAsync(int count = 10)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.IsPublished)
                    .OrderByDescending(n => n.PublishedDate)
                    .Take(count)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetPopularNewsAsync(int count = 10)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.IsPublished)
                    .OrderByDescending(n => n.ViewCount)
                    .Take(count)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        // News search and filtering
        public async Task<IEnumerable<NewsDto>> SearchNewsAsync(string searchTerm)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n =>
                        n.Title.Contains(searchTerm) ||
                        (n.Content != null && n.Content.Contains(searchTerm)) ||
                        (n.Summary != null && n.Summary.Contains(searchTerm)))
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByKeywordsAsync(string keywords)
        {
            return await SearchNewsAsync(keywords);
        }

        public async Task<IEnumerable<NewsDto>> GetNewsByContentAsync(string content)
        {
            try
            {
                var newsList = await _context.News
                    .Include(n => n.Category)
                    .Include(n => n.Author)
                    .Where(n => n.Content != null && n.Content.Contains(content))
                    .OrderByDescending(n => n.CreatedDate)
                    .ToListAsync();

                return newsList.Select(MapToDto);
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        // News statistics
        public async Task<int> GetTotalNewsAsync()
        {
            try
            {
                return await _context.News.CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetPublishedNewsCountAsync()
        {
            try
            {
                return await _context.News.CountAsync(n => n.IsPublished);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetUnpublishedNewsCountAsync()
        {
            try
            {
                return await _context.News.CountAsync(n => !n.IsPublished);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetNewsCountByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.News.CountAsync(n => n.CategoryId == categoryId);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetNewsCountByAuthorAsync(int authorId)
        {
            try
            {
                return await _context.News.CountAsync(n => n.AuthorId == authorId);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetNewsViewCountAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                return news?.ViewCount ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        // News view tracking
        public async Task<bool> IncrementViewCountAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.ViewCount++;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetViewCountAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.ViewCount = 0;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetTotalViewsAsync()
        {
            try
            {
                return await _context.News.SumAsync(n => n.ViewCount);
            }
            catch
            {
                return 0;
            }
        }

        // News validation
        public async Task<bool> IsNewsExistsAsync(int newsId)
        {
            try
            {
                return await _context.News.AnyAsync(n => n.NewsId == newsId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsNewsTitleExistsAsync(string title)
        {
            try
            {
                return await _context.News.AnyAsync(n => n.Title == title);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsNewsSlugExistsAsync(string slug)
        {
            // Not implemented - would need slug field
            return await Task.FromResult(false);
        }

        // News categories
        public async Task<IEnumerable<NewsCategoryDto>> GetNewsCategoriesAsync()
        {
            try
            {
                var categories = await _context.NewsCategories.ToListAsync();
                return categories.Select(c => new NewsCategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                });
            }
            catch
            {
                return new List<NewsCategoryDto>();
            }
        }

        public async Task<NewsCategoryDto?> GetNewsCategoryAsync(int newsId)
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.Category)
                    .FirstOrDefaultAsync(n => n.NewsId == newsId);

                if (news?.Category == null) return null;

                return new NewsCategoryDto
                {
                    CategoryId = news.Category.CategoryId,
                    CategoryName = news.Category.CategoryName,
                    Description = news.Category.Description
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AssignCategoryAsync(int newsId, int categoryId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.CategoryId = categoryId;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveCategoryAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.CategoryId = null;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // News authors
        public async Task<UserDto?> GetNewsAuthorAsync(int newsId)
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.Author)
                    .FirstOrDefaultAsync(n => n.NewsId == newsId);

                if (news?.Author == null) return null;

                return new UserDto
                {
                    UserId = news.Author.UserId,
                    FullName = news.Author.FullName,
                    Email = news.Author.Email
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AssignAuthorAsync(int newsId, int authorId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.AuthorId = authorId;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAuthorAsync(int newsId)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.AuthorId = null;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // News publishing workflow
        public async Task<bool> SubmitForReviewAsync(int newsId)
        {
            // Not fully implemented - would need status field
            return await Task.FromResult(true);
        }

        public async Task<bool> ApproveNewsAsync(int newsId, int reviewerId)
        {
            return await PublishNewsAsync(newsId);
        }

        public async Task<bool> RejectNewsAsync(int newsId, int reviewerId, string reason)
        {
            return await UnpublishNewsAsync(newsId);
        }

        public async Task<bool> SchedulePublishAsync(int newsId, DateTime publishDate)
        {
            try
            {
                var news = await _context.News.FindAsync(newsId);
                if (news == null) return false;

                news.PublishedDate = publishDate;
                news.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // News notifications (stub implementations)
        public async Task<bool> SendNewsPublishedNotificationAsync(int newsId)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> SendNewsUpdateNotificationAsync(int newsId)
        {
            return await Task.FromResult(true);
        }

        // News SEO
        public async Task<string> GenerateNewsSlugAsync(string title)
        {
            var slug = title.ToLower()
                .Replace(" ", "-")
                .Replace("đ", "d")
                .Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
                .Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
                .Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
                .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
                .Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
                .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
                .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
                .Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
                .Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
                .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
                .Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
                .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y");

            return await Task.FromResult(slug);
        }

        public async Task<string> GenerateNewsSummaryAsync(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var summary = content.Length > 200 ? content.Substring(0, 200) + "..." : content;
            return await Task.FromResult(summary);
        }

        public async Task<bool> UpdateNewsMetaDataAsync(int newsId, string metaTitle, string metaDescription)
        {
            // Not implemented - would need meta fields
            return await Task.FromResult(true);
        }

        // Helper methods
        private NewsDto MapToDto(News news)
        {
            return new NewsDto
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary,
                ImageUrl = news.ImageUrl,
                CategoryId = news.CategoryId,
                AuthorId = news.AuthorId,
                ViewCount = news.ViewCount,
                IsPublished = news.IsPublished,
                PublishedDate = news.PublishedDate,
                CreatedDate = news.CreatedDate,
                UpdatedDate = news.UpdatedDate,
                CategoryName = news.Category?.CategoryName,
                AuthorName = news.Author?.FullName
            };
        }
    }
}
