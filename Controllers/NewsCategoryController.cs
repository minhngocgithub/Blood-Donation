using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Data;
using Blood_Donation_Website.Data.Seeders;
using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý danh mục tin tức (chỉ dành cho Admin và Hospital)
    /// Xử lý: CRUD danh mục tin tức, Seed dữ liệu mẫu
    /// Route: /NewsCategory/*
    /// </summary>
    [HospitalAdminOnly] // Filter: Chỉ Admin hoặc Hospital được truy cập
    public class NewsCategoryController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context để truy vấn trực tiếp
        
        /// <summary>
        /// Constructor - Inject ApplicationDbContext
        /// </summary>
        public NewsCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: /NewsCategory/Index
        /// Hiển thị danh sách tất cả danh mục tin tức
        /// Bao gồm số lượng bài viết trong mỗi danh mục
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Lấy danh mục kèm theo danh sách bài viết (để đếm số lượng)
            var categories = await _context.NewsCategories
                .Include(c => c.NewsArticles) // Eager loading bài viết
                .OrderBy(c => c.CategoryName) // Sắp xếp theo tên danh mục
                .ToListAsync();
            
            return View(categories);
        }

        /// <summary>
        /// GET: /NewsCategory/Create
        /// Hiển thị form tạo danh mục mới
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: /NewsCategory/Create
        /// Xử lý tạo danh mục tin tức mới
        /// </summary>
        /// <param name="category">Thông tin danh mục (tên, mô tả)</param>
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF attack
        public async Task<IActionResult> Create(NewsCategory category)
        {
            if (ModelState.IsValid)
            {
                // Thêm danh mục mới vào database
                _context.NewsCategories.Add(category);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Danh mục đã được tạo thành công!";
                return RedirectToAction(nameof(Index));
            }
            
            return View(category);
        }

        /// <summary>
        /// GET: /NewsCategory/Edit/{id}
        /// Hiển thị form chỉnh sửa danh mục
        /// </summary>
        /// <param name="id">ID danh mục cần sửa</param>
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.NewsCategories.FindAsync(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }
            
            return View(category);
        }

        /// <summary>
        /// POST: /NewsCategory/Edit/{id}
        /// Xử lý cập nhật thông tin danh mục
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <param name="category">Thông tin danh mục mới</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsCategory category)
        {
            // Kiểm tra ID có khớp không
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật danh mục trong database
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Danh mục đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý lỗi concurrency (2 người cùng sửa 1 record)
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(category);
        }

        /// <summary>
        /// POST: /NewsCategory/Delete/{id}
        /// Xóa danh mục tin tức (chỉ xóa được nếu không có bài viết nào)
        /// </summary>
        /// <param name="id">ID danh mục cần xóa</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Lấy danh mục kèm danh sách bài viết
            var category = await _context.NewsCategories
                .Include(c => c.NewsArticles)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            
            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem danh mục có bài viết nào không
            if (category.NewsArticles != null && category.NewsArticles.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa danh mục vì còn {category.NewsArticles.Count} tin tức liên quan!";
                return RedirectToAction(nameof(Index));
            }

            // Xóa danh mục khỏi database
            _context.NewsCategories.Remove(category);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Danh mục đã được xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Helper method: Kiểm tra danh mục có tồn tại không
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        private bool CategoryExists(int id)
        {
            return _context.NewsCategories.Any(e => e.CategoryId == id);
        }

        /// <summary>
        /// GET: /NewsCategory/Seed
        /// Khởi tạo dữ liệu mẫu cho danh mục tin tức (dùng cho development/testing)
        /// Sử dụng NewsCategorySeeder để tạo các danh mục chuẩn
        /// </summary>
        public IActionResult Seed()
        {
            // Gọi seeder để khởi tạo dữ liệu mẫu
            NewsCategorySeeder.Seed(_context);
            
            TempData["SuccessMessage"] = "Đã khởi tạo danh mục tin tức thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
