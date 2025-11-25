using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly IContactMessageService _contactService;
        public ContactController(IContactMessageService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View(new ContactMessageDto());
        }

        [HttpPost("")]
        [HttpPost("index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactMessageDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin hợp lệ.";
                return View(model);
            }
            try
            {
                var result = await _contactService.CreateMessageAsync(model);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm nhất.";
                    return RedirectToAction("Index");
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