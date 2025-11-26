using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller xử lý trang liên hệ công khai
    /// Cho phép người dùng gửi tin nhắn liên hệ đến hệ thống
    /// Route: /contact
    /// </summary>
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly IContactMessageService _contactService; // Service quản lý tin nhắn liên hệ
        
        /// <summary>
        /// Constructor - Inject ContactMessageService
        /// </summary>
        public ContactController(IContactMessageService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// GET: /contact hoặc /contact/index
        /// Hiển thị form liên hệ
        /// </summary>
        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            // Trả về view với model rỗng
            return View(new ContactMessageDto());
        }

        /// <summary>
        /// POST: /contact hoặc /contact/index
        /// Xử lý gửi tin nhắn liên hệ
        /// </summary>
        /// <param name="model">Thông tin liên hệ (tên, email, tiêu đề, nội dung)</param>
        [HttpPost("")]
        [HttpPost("index")]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF attack
        public async Task<IActionResult> Index(ContactMessageDto model)
        {
            // Kiểm tra validation
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin hợp lệ.";
                return View(model);
            }
            
            try
            {
                // Gọi service để lưu tin nhắn vào database
                var result = await _contactService.CreateMessageAsync(model);
                
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm nhất.";
                    return RedirectToAction("Index"); // Redirect để tránh submit lại form
                }
                else
                {
                    TempData["ErrorMessage"] = "Gửi liên hệ thất bại. Vui lòng thử lại sau.";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            }
            
            return View(model);
        }
    }
}