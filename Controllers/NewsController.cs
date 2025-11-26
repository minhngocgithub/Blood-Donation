using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý tin tức (dành cho Admin và Hospital)
    /// Xử lý: CRUD tin tức, Xuất bản/Gỡ xuất bản, Xem công khai
    /// Route: /news/*
    /// </summary>
    public class NewsController : Controller
    {
        private readonly INewsService _newsService; // Service quản lý tin tức
        
        /// <summary>
        /// Constructor - Inject NewsService
        /// </summary>
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// GET: /News/Index (Admin/Hospital only)
        /// Hiển thị danh sách tất cả tin tức với tìm kiếm và lọc
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tiêu đề, tóm tắt, nội dung)</param>
        /// <param name="categoryId">Lọc theo danh mục</param>
        /// <param name="isPublished">Lọc theo trạng thái xuất bản</param>
        /// <param name="sortBy">Sắp xếp: date, title, views, published</param>
        [HospitalAdminOnly] // Chỉ Admin và Hospital được truy cập
        public async Task<IActionResult> Index(string searchTerm = "", int? categoryId = null, bool? isPublished = null, string sortBy = "date")
        {
            // Lấy tất cả tin tức
            var allNews = await _newsService.GetAllNewsAsync();
            
            // Áp dụng bộ lọc tìm kiếm theo từ khóa
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                allNews = allNews.Where(n => 
                    n.Title.ToLower().Contains(searchTerm) ||
                    (n.Summary != null && n.Summary.ToLower().Contains(searchTerm)) ||
                    (n.Content != null && n.Content.ToLower().Contains(searchTerm))
                ).ToList();
            }

            // Lọc theo danh mục
            if (categoryId.HasValue)
            {
                allNews = allNews.Where(n => n.CategoryId == categoryId.Value).ToList();
            }

            // Lọc theo trạng thái xuất bản
            if (isPublished.HasValue)
            {
                allNews = allNews.Where(n => n.IsPublished == isPublished.Value).ToList();
            }

            // Sắp xếp theo tiêu chí
            allNews = sortBy switch
            {
                "title" => allNews.OrderBy(n => n.Title).ToList(),
                "views" => allNews.OrderByDescending(n => n.ViewCount).ToList(),
                "published" => allNews.OrderByDescending(n => n.PublishedDate).ToList(),
                _ => allNews.OrderByDescending(n => n.CreatedDate).ToList() // Mặc định: mới nhất trước
            };

            // Lấy danh sách danh mục để hiển thị dropdown filter
            var categories = await _newsService.GetNewsCategoriesAsync();
            
            // Truyền dữ liệu sang View
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.IsPublished = isPublished;
            ViewBag.SortBy = sortBy;
            ViewBag.Categories = categories;
            ViewBag.TotalNews = allNews.Count();
            ViewBag.PublishedNews = allNews.Count(n => n.IsPublished);
            ViewBag.UnpublishedNews = allNews.Count(n => !n.IsPublished);

            return View(allNews);
        }

        /// <summary>
        /// GET: /News/Create (Admin/Hospital only)
        /// Hiển thị form tạo tin tức mới
        /// </summary>
        [HospitalAdminOnly]
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách danh mục để hiển thị dropdown
            var categories = await _newsService.GetNewsCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        /// <summary>
        /// POST: /News/Create (Admin/Hospital only)
        /// Xử lý tạo tin tức mới
        /// </summary>
        /// <param name="model">Thông tin tin tức (tiêu đề, nội dung, danh mục...)</param>
        [HospitalAdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors để debug
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {string.Join(", ", errors)}";
                
                var categories = await _newsService.GetNewsCategoriesAsync();
                ViewBag.Categories = categories;
                return View(model);
            }

            try
            {
                // Lấy ID người dùng hiện tại làm tác giả
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    model.AuthorId = userId;
                }

                // Gọi service để tạo tin tức
                var createdNews = await _newsService.CreateNewsAsync(model);
                
                if (createdNews == null || createdNews.NewsId == 0)
                {
                    TempData["ErrorMessage"] = "Không thể tạo tin tức. Vui lòng kiểm tra lại dữ liệu.";
                    var categories = await _newsService.GetNewsCategoriesAsync();
                    ViewBag.Categories = categories;
                    return View(model);
                }
                
                TempData["SuccessMessage"] = "Tin tức đã được tạo thành công!";
                return RedirectToAction(nameof(Details), new { id = createdNews.NewsId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                var categories = await _newsService.GetNewsCategoriesAsync();
                ViewBag.Categories = categories;
                return View(model);
            }
        }

        // GET: /News/Edit/{id}
        [HospitalAdminOnly]
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tin tức!";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _newsService.GetNewsCategoriesAsync();
            ViewBag.Categories = categories;

            var updateDto = new NewsUpdateDto
            {
                Title = news.Title,
                Content = news.Content,
                Summary = news.Summary,
                ImageUrl = news.ImageUrl,
                CategoryId = news.CategoryId,
                IsPublished = news.IsPublished,
                PublishedDate = news.PublishedDate
            };

            ViewBag.NewsId = id;
            return View(updateDto);
        }

        // POST: /News/Edit/{id}
        [HospitalAdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _newsService.GetNewsCategoriesAsync();
                ViewBag.Categories = categories;
                ViewBag.NewsId = id;
                return View(model);
            }

            try
            {
                var success = await _newsService.UpdateNewsAsync(id, model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin tức đã được cập nhật thành công!";
                    return RedirectToAction(nameof(Details), new { id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật tin tức!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                var categories = await _newsService.GetNewsCategoriesAsync();
                ViewBag.Categories = categories;
                ViewBag.NewsId = id;
                return View(model);
            }
        }

        // GET: /News/Details/{id}
        [HospitalAdminOnly]
        public async Task<IActionResult> Details(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tin tức!";
                return RedirectToAction(nameof(Index));
            }

            // Increment view count
            await _newsService.IncrementViewCountAsync(id);

            return View(news);
        }

        // POST: /News/Delete/{id}
        [HospitalAdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _newsService.DeleteNewsAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin tức đã được xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xóa tin tức!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /News/Publish/{id}
        [HospitalAdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            try
            {
                var success = await _newsService.PublishNewsAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin tức đã được xuất bản!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xuất bản tin tức!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /News/Unpublish/{id}
        [HospitalAdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpublish(int id)
        {
            try
            {
                var success = await _newsService.UnpublishNewsAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin tức đã được gỡ xuất bản!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể gỡ xuất bản tin tức!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// GET: /News/List (Public)
        /// Hiển thị danh sách tin tức công khai (chỉ tin đã xuất bản)
        /// Hỗ trợ lọc theo danh mục và phân trang
        /// </summary>
        /// <param name="categoryId">Lọc theo danh mục (tùy chọn)</param>
        /// <param name="page">Số trang (mặc định = 1)</param>
        [AllowAnonymous] // Cho phép truy cập không cần đăng nhập
        public async Task<IActionResult> List(int? categoryId = null, int page = 1)
        {
            var pageSize = 12; // 12 tin tức mỗi trang
            
            // Tạo DTO tìm kiếm
            var searchDto = new NewsSearchDto
            {
                CategoryId = categoryId,
                IsPublished = true, // Chỉ lấy tin đã xuất bản
                Page = page,
                PageSize = pageSize,
                SortBy = "published",
                SortOrder = "desc" // Mới nhất trước
            };

            // Lấy tin tức có phân trang
            var pagedNews = await _newsService.GetNewsPagedAsync(searchDto);
            var categories = await _newsService.GetNewsCategoriesAsync();

            // Truyền dữ liệu sang View
            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentPage = page;

            return View(pagedNews);
        }

        /// <summary>
        /// GET: /News/Read/{id} (Public)
        /// Đọc chi tiết một tin tức (tự động tăng lượt xem)
        /// Hiển thị các tin liên quan cùng danh mục
        /// </summary>
        /// <param name="id">ID tin tức</param>
        [AllowAnonymous]
        public async Task<IActionResult> Read(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            
            // Kiểm tra tin tức có tồn tại và đã xuất bản chưa
            if (news == null || !news.IsPublished)
            {
                TempData["ErrorMessage"] = "Tin tức không tồn tại hoặc chưa được xuất bản!";
                return RedirectToAction(nameof(List));
            }

            // Tăng số lượt xem
            await _newsService.IncrementViewCountAsync(id);

            // Lấy tin liên quan (cùng danh mục, đã xuất bản, tối đa 4 tin)
            var relatedNews = new List<NewsDto>();
            if (news.CategoryId.HasValue)
            {
                var categoryNews = await _newsService.GetNewsByCategoryAsync(news.CategoryId.Value);
                relatedNews = categoryNews
                    .Where(n => n.NewsId != id && n.IsPublished) // Loại bỏ tin hiện tại
                    .Take(4)
                    .ToList();
            }

            ViewBag.RelatedNews = relatedNews;

            return View(news);
        }
    }
}
