using Blood_Donation_Website.Models;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller chính của website - Trang chủ và các trang thông tin
    /// Xử lý: Trang chủ, Giới thiệu, FAQ, Hướng dẫn, Điều khoản
    /// Route: / (trang chủ)
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IBloodDonationEventService _eventService; // Service lấy thông tin sự kiện
        
        /// <summary>
        /// Constructor - Inject EventService để hiển thị sự kiện sắp tới trên trang chủ
        /// </summary>
        public HomeController(IBloodDonationEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// GET: /
        /// Trang chủ website - Hiển thị 3 sự kiện sắp tới
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Lấy 3 sự kiện sắp diễn ra gần nhất
            var upcomingEvents = (await _eventService.GetUpcomingEventsAsync()).Take(3).ToList();
            return View(upcomingEvents);
        }

        /// <summary>
        /// GET: /Home/About
        /// Trang giới thiệu về hệ thống hiến máu
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/FAQ
        /// Trang câu hỏi thường gặp (Frequently Asked Questions)
        /// </summary>
        public IActionResult FAQ()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Guide
        /// Trang hướng dẫn sử dụng hệ thống và quy trình hiến máu
        /// </summary>
        public IActionResult Guide()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Privacy
        /// Trang chính sách bảo mật thông tin người dùng
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Terms
        /// Trang điều khoản sử dụng dịch vụ
        /// </summary>
        public IActionResult Terms()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/SweetAlertDemo
        /// Trang demo thư viện SweetAlert (dùng cho development/testing)
        /// </summary>
        public IActionResult SweetAlertDemo()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Error
        /// Trang hiển thị lỗi hệ thống
        /// Không cache để luôn hiển thị thông tin lỗi mới nhất
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Tạo ErrorViewModel với RequestId để tracking
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}