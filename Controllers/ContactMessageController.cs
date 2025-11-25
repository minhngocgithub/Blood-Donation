using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using static Blood_Donation_Website.Utilities.EnumMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blood_Donation_Website.Controllers
{
    [Authorize(Roles = "Admin,Hospital")]
    [Route("admin/contact-messages")]
    public class ContactMessageController : Controller
    {
        private readonly IContactMessageService _contactMessageService;
        private readonly IUserService _userService;

        public ContactMessageController(
            IContactMessageService contactMessageService,
            IUserService userService)
        {
            _contactMessageService = contactMessageService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string status = "")
        {   
            IEnumerable<ContactMessageDto> messages;
            
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

        [HttpGet("unread")]
        public async Task<IActionResult> UnreadMessages()
        {
            var unreadMessages = await _contactMessageService.GetUnreadMessagesAsync();
            return View(unreadMessages);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var message = await _contactMessageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            if (message.Status == MessageStatus.New)
            {
                await _contactMessageService.MarkAsReadAsync(id);
            }

            return View(message);
        }

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

        [HttpPost("reply/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, ContactMessageDto replyDto)
        {
            if (ModelState.IsValid)
            {
                try
                    {
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

            var message = await _contactMessageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tin nhắn cần trả lời.";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.OriginalMessage = message;
            return View(replyDto);
        }

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

            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/details/"))
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

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

            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/details/"))
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("status/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
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

            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("/details/"))
            {
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Index));
        }

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

        [HttpGet("statistics")]
        public async Task<IActionResult> Statistics()
        {
            var statistics = await _contactMessageService.GetMessageStatisticsAsync();
            return View(statistics);
        }
    }
} 