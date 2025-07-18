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

            // Luôn cập nhật NextEligibleDate = DonationDate + 90 ngày
            if (donation.DonationDate != null)
            {
                var newNextEligibleDate = donation.DonationDate.AddDays(90);
                await _donationHistoryService.UpdateDonationAsync(donation.DonationId, new Blood_Donation_Website.Models.DTOs.DonationHistoryUpdateDto {
                    Status = donation.Status,
                    Notes = donation.Notes,
                    NextEligibleDate = newNextEligibleDate,
                    CertificateIssued = donation.CertificateIssued
                });
                donation.NextEligibleDate = newNextEligibleDate;
            }

            // Lấy ngày đủ điều kiện hiến máu tiếp theo (90 ngày sau lần hiến gần nhất)
            var nextEligibleDate = await _donationHistoryService.GetUserNextEligibleDateAsync(userId);
            int? daysLeft = null;
            string eligibleMessage = string.Empty;
            if (nextEligibleDate.HasValue)
            {
                var now = DateTime.Now.Date;
                if (now >= nextEligibleDate.Value.Date)
                {
                    eligibleMessage = "Bạn đã có thể hiến máu.";
                }
                else
                {
                    daysLeft = (nextEligibleDate.Value.Date - now).Days;
                    eligibleMessage = $"Bạn cần chờ {daysLeft} ngày nữa để hiến máu.";
                }
            }
            else
            {
                eligibleMessage = "Bạn đã có thể hiến máu.";
            }
            ViewBag.NextEligibleDate = nextEligibleDate;
            ViewBag.DaysLeft = daysLeft;
            ViewBag.EligibleMessage = eligibleMessage;

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