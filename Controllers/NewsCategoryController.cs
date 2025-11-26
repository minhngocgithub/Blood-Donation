using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Data;
using Blood_Donation_Website.Data.Seeders;
using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Controllers
{
    [HospitalAdminOnly]
    public class NewsCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.NewsCategories
                .Include(c => c.NewsArticles)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
            return View(categories);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.NewsCategories.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Danh mục đã được tạo thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Edit/5
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

        // POST: Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsCategory category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Danh mục đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
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

        // POST: Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.NewsCategories
                .Include(c => c.NewsArticles)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            
            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục!";
                return RedirectToAction(nameof(Index));
            }

            if (category.NewsArticles != null && category.NewsArticles.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa danh mục vì còn {category.NewsArticles.Count} tin tức liên quan!";
                return RedirectToAction(nameof(Index));
            }

            _context.NewsCategories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Danh mục đã được xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.NewsCategories.Any(e => e.CategoryId == id);
        }

        // Seed action để khởi tạo dữ liệu nhanh
        public IActionResult Seed()
        {
            NewsCategorySeeder.Seed(_context);
            TempData["SuccessMessage"] = "Đã khởi tạo danh mục tin tức thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
