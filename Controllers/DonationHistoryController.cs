using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Services.Interfaces;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    [Authorize]
    public class DonationHistoryController : Controller
    {
        private readonly IDonationHistoryService _donationHistoryService;

        public DonationHistoryController(IDonationHistoryService donationHistoryService)
        {
            _donationHistoryService = donationHistoryService;
        }

        // GET: /DonationHistory/MyHistory
        public async Task<IActionResult> MyHistory()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var donations = await _donationHistoryService.GetDonationsByUserAsync(userId);
            return View(donations);
        }

        // GET: /DonationHistory/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var donation = await _donationHistoryService.GetDonationByIdAsync(id);
            if (donation == null || donation.UserId != userId)
            {
                TempData["Error"] = "Không tìm thấy lịch sử hiến máu này.";
                return RedirectToAction(nameof(MyHistory));
            }
            return View(donation);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return 0;
        }
    }
} 