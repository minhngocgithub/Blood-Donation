using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using static Blood_Donation_Website.Utilities.EnumMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý tin nhắn liên hệ (dành cho Admin và Hospital)
    /// Xử lý: Xem danh sách tin nhắn, Chi tiết, Phản hồi, Đánh dấu đã đọc/chưa đọc, Cập nhật trạng thái, Xóa
    /// Route: /admin/contact-messages/*
    /// </summary>
    [Authorize(Roles = "Admin,Hospital")]
    [Route("admin/contact-messages")]
    public class ContactMessageController : Controller
    {
        // Dependencies
        private readonly IContactMessageService _contactMessageService; // Service quản lý tin nhắn liên hệ
        private readonly IUserService _userService; // Service người dùng

        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public ContactMessageController(
            IContactMessageService contactMessageService,
            IUserService userService)
        {
            _contactMessageService = contactMessageService;
            _userService = userService;
        }

        /// <summary>
        /// GET: /admin/contact-messages
        /// Hiển thị danh sách tất cả tin nhắn liên hệ
        /// Có thể lọc theo trạng thái: New, Read, Replied, Resolved
        /// </summary>
        /// <param name="status">Trạng thái tin nhắn cần lọc (tùy chọn)</param>
        [HttpGet]
        public async Task<IActionResult> Index(string status = "")
        {   
            IEnumerable<ContactMessageDto> messages;
            
            // Lọc theo trạng thái nếu có
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<MessageStatus>(status, out var statusEnum))
            {
                messages = await _contactMessageService.GetMessagesByStatusAsync(statusEnum);
            }
            else
            {
                messages = await _contactMessageService.GetAllMessagesAsync();
            }
            
            return View(messages);
        }

        /// <summary>
        /// GET: /admin/contact-messages/unread
        /// Hiển thị danh sách tin nhắn chưa đọc
        /// </summary>
        [HttpGet("unread")]
        public async Task<IActionResult> UnreadMessages()
        {
            var unreadMessages = await _contactMessageService.GetUnreadMessagesAsync();
            return View(unreadMessages);
        }

        /// <summary>
        /// GET: /admin/contact-messages/details/{id}
        /// Xem chi tiết một tin nhắn liên hệ
        /// Tự động đánh dấu là đã đọc nếu trạng thái là New
        /// </summary>
        /// <param name="id">ID tin nhắn</param>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var message = await _contactMessageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            // Tự động đánh dấu là đã đọc nếu tin nhắn mới
            if (message.Status == MessageStatus.New)
            {
                await _contactMessageService.MarkAsReadAsync(id);
            }

            return View(message);
        }

        /// <summary>
        /// GET: /admin/contact-messages/reply/{id}
        /// Hiển thị form phản hồi tin nhắn
        /// </summary>
        /// <param name="id">ID tin nhắn cần phản hồi</param>
        [HttpGet("reply/{id}")]
        public async Task<IActionResult> Reply(int id)
        {
            var message = await _contactMessageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            ViewBag.OriginalMessage = message;
            return View();
        }

        /// <summary>
        /// POST: /admin/contact-messages/reply/{id}
        /// Xử lý gửi phản hồi cho tin nhắn
        /// Validate: Tiêu đề và nội dung phản hồi không được để trống
        /// Sau khi gửi thành công, tin nhắn được đánh dấu là Resolved
        /// </summary>
        /// <param name="id">ID tin nhắn cần phản hồi</param>
        /// <param name="replyDto">Dữ liệu phản hồi</param>
        [HttpPost("reply/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, ContactMessageDto replyDto)
        {
            if (ModelState.IsValid)
            {
                try
                    {
                    // Validate tiêu đề và nội dung
                    if (string.IsNullOrWhiteSpace(replyDto.Subject))
                    {
                        ModelState.AddModelError("Subject", "Tiêu đề phản hồi không được để trống.");
                    }
                    
                    if (string.IsNullOrWhiteSpace(replyDto.Message))
                    {
                        ModelState.AddModelError("Message", "Nội dung phản hồi không được để trống.");
                    }
                    
                    if (ModelState.IsValid)
                    {
                        var success = await _contactMessageService.ReplyToMessageAsync(id, replyDto);
                        if (success)
                        {
                            TempData["SuccessMessage"] = "Phản hồi đã được gửi thành công! Tin nhắn đã được đánh dấu là đã giải quyết.";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không thể gửi phản hồi. Vui lòng thử lại sau.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }

            // Nếu có lỗi, load lại form
            var message = await _contactMessageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tin nhắn cần trả lời.";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.OriginalMessage = message;
            return View(replyDto);
        }

        /// <summary>
        /// POST: /admin/contact-messages/mark-read/{id}
        /// Đánh dấu tin nhắn là đã đọc
        /// Tự động quay lại trang gốc (Details hoặc Index)
        /// </summary>
        /// <param name="id">ID tin nhắn</param>
        [HttpPost("mark-read/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var success = await _contactMessageService.MarkAsReadAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin nhắn đã được đánh dấu là đã đọc!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể đánh dấu tin nhắn.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Quay lại trang trước đó
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/details/"))
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: /admin/contact-messages/mark-unread/{id}
        /// Đánh dấu tin nhắn là chưa đọc
        /// Tự động quay lại trang gốc (Details hoặc Index)
        /// </summary>
        /// <param name="id">ID tin nhắn</param>
        [HttpPost("mark-unread/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            try
            {
                var success = await _contactMessageService.MarkAsUnreadAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin nhắn đã được đánh dấu là chưa đọc!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể đánh dấu tin nhắn.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Quay lại trang trước đó
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/details/"))
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: /admin/contact-messages/status/{id}
        /// Cập nhật trạng thái tin nhắn
        /// Trạng thái hợp lệ: New, Read, Replied, Resolved
        /// </summary>
        /// <param name="id">ID tin nhắn</param>
        /// <param name="status">Trạng thái mới (string)</param>
        [HttpPost("status/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            // Validate và parse trạng thái
            if (Enum.TryParse<MessageStatus>(status, out var statusEnum))
            {
                try
                {
                    var success = await _contactMessageService.UpdateMessageStatusAsync(id, statusEnum);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Trạng thái tin nhắn đã được cập nhật!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không thể cập nhật trạng thái tin nhắn.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Trạng thái không hợp lệ.";
            }

            // Quay lại trang trước đó
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/details/"))
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// POST: /admin/contact-messages/delete/{id}
        /// Xóa tin nhắn liên hệ
        /// </summary>
        /// <param name="id">ID tin nhắn cần xóa</param>
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _contactMessageService.DeleteMessageAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Tin nhắn đã được xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xóa tin nhắn.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /admin/contact-messages/statistics
        /// Xem thống kê tin nhắn liên hệ
        /// Bao gồm: Tổng số tin nhắn, Phân bố theo trạng thái, Xu hướng theo thời gian
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu lọc (tùy chọn)</param>
        /// <param name="endDate">Ngày kết thúc lọc (tùy chọn)</param>
        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            var statistics = await _contactMessageService.GetMessageStatisticsAsync();
            
            // Lưu giá trị lọc để hiển thị trên view
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            return View(statistics);
        }
    }
} 