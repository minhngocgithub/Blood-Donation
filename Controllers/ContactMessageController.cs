using Blood_Donation_Website.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [AdminOnly]
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
        public async Task<IActionResult> Index()
        {   
            var messages = await _contactMessageService.GetAllMessagesAsync();
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

            // Mark as read if it's unread
            if (message.Status == "New")
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
                    var success = await _contactMessageService.ReplyToMessageAsync(id, replyDto);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Phản hồi đã được gửi thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể gửi phản hồi.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var message = await _contactMessageService.GetMessageByIdAsync(id);
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("status/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                var success = await _contactMessageService.UpdateMessageStatusAsync(id, status);
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