using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Utilities;
using Blood_Donation_Website.Models.DTOs;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    [AuthenticatedUser]
    public class DonationHistoryController : Controller
    {
        private readonly IDonationHistoryService _donationHistoryService;

        public DonationHistoryController(IDonationHistoryService donationHistoryService)
        {
            _donationHistoryService = donationHistoryService;
        }

        // GET: /DonationHistory/MyHistory
        [AuthenticatedUser]
        public async Task<IActionResult> MyHistory()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var donations = await _donationHistoryService.GetDonationsByUserAsync(userId);
            return View(donations);
        }

        // GET: /DonationHistory/Details/{id}
        [AuthenticatedUser]
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            var donation = await _donationHistoryService.GetDonationByIdAsync(id);
            
            // Check access: Allow if owner OR if user has staff roles
            bool isStaff = User.IsInRole("Admin") || User.IsInRole("Hospital") || User.IsInRole("Doctor");
            if (donation == null || (!isStaff && donation.UserId != userId))
            {
                TempData["Error"] = "Không tìm thấy lịch sử hiến máu này hoặc bạn không có quyền truy cập.";
                return RedirectToAction(nameof(MyHistory));
            }

            // Luôn cập nhật NextEligibleDate = DonationDate + 90 ngày
            var newNextEligibleDate = donation.DonationDate.AddDays(90);
            await _donationHistoryService.UpdateDonationAsync(donation.DonationId, new Blood_Donation_Website.Models.DTOs.DonationHistoryUpdateDto {
                Status = donation.Status,
                Notes = donation.Notes,
                NextEligibleDate = newNextEligibleDate,
                CertificateIssued = donation.CertificateIssued
            });
            donation.NextEligibleDate = newNextEligibleDate;

            // Lấy ngày đủ điều kiện hiến máu tiếp theo của NGƯỜI HIẾN MÁU (donation.UserId)
            var nextEligibleDate = await _donationHistoryService.GetUserNextEligibleDateAsync(donation.UserId);
            int? daysLeft = null;
            string eligibleMessage = string.Empty;
            if (nextEligibleDate.HasValue)
            {
                var now = DateTime.Now.Date;
                if (now >= nextEligibleDate.Value.Date)
                {
                    eligibleMessage = "Người hiến máu đã có thể hiến máu lại.";
                }
                else
                {
                    daysLeft = (nextEligibleDate.Value.Date - now).Days;
                    eligibleMessage = $"Người hiến máu cần chờ {daysLeft} ngày nữa để hiến máu.";
                }
            }
            else
            {
                eligibleMessage = "Người hiến máu đã có thể hiến máu.";
            }
            
            // Customize message for self-viewing
            if (donation.UserId == userId)
            {
                if (nextEligibleDate.HasValue && DateTime.Now.Date < nextEligibleDate.Value.Date)
                {
                    eligibleMessage = $"Bạn cần chờ {daysLeft} ngày nữa để hiến máu.";
                }
                else
                {
                    eligibleMessage = "Bạn đã có thể hiến máu.";
                }
            }

            ViewBag.NextEligibleDate = nextEligibleDate;
            ViewBag.DaysLeft = daysLeft;
            ViewBag.EligibleMessage = eligibleMessage;

            return View(donation);
        }

        // GET: /DonationHistory/Statistics
        [AuthenticatedUser]
        public async Task<IActionResult> Statistics()
        {
            // Check if user has admin, hospital, or doctor role
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang thống kê này.";
                return RedirectToAction("MyHistory");
            }

            // Get all donations for statistics
            var allDonations = await _donationHistoryService.GetAllDonationsAsync();
            var totalDonations = await _donationHistoryService.GetTotalDonationsAsync();
            var totalVolume = await _donationHistoryService.GetTotalVolumeAsync();
            
            // Calculate statistics
            var completedDonations = allDonations.Count(d => d.Status == EnumMapper.DonationStatus.Completed);
            var cancelledDonations = allDonations.Count(d => d.Status == EnumMapper.DonationStatus.Cancelled);
            var certificatesIssued = allDonations.Count(d => d.CertificateIssued);
            var totalUsers = allDonations.Select(d => d.UserId).Distinct().Count();
            
            // Blood type distribution
            var bloodTypeStats = allDonations
                .Where(d => !string.IsNullOrEmpty(d.BloodTypeName))
                .GroupBy(d => d.BloodTypeName)
                .Select(g => new BloodTypeStatDto { 
                    BloodType = g.Key!, 
                    Count = g.Count(),
                    TotalVolume = g.Sum(d => d.Volume)
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            
            // Monthly donation trend (last 12 months)
            var monthlyStats = new Dictionary<string, int>();
            for (int i = 11; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var monthKey = month.ToString("MM/yyyy");
                var monthDonations = allDonations.Count(d => 
                    d.DonationDate.Month == month.Month && 
                    d.DonationDate.Year == month.Year);
                monthlyStats[monthKey] = monthDonations;
            }
            
            // Recent donations (last 10)
            var recentDonations = allDonations
                .OrderByDescending(d => d.DonationDate)
                .Take(10)
                .ToList();
            
            // Top donors (users with most donations)
            var topDonors = allDonations
                .GroupBy(d => new { d.UserId, d.UserName, d.UserEmail })
                .Select(g => new TopDonorDto { 
                    UserName = g.Key.UserName ?? "Không xác định", 
                    UserEmail = g.Key.UserEmail ?? "N/A",
                    TotalDonations = g.Count(),
                    TotalVolume = g.Sum(d => d.Volume),
                    LastDonation = g.Max(d => d.DonationDate)
                })
                .OrderByDescending(x => x.TotalDonations)
                .Take(10)
                .ToList();
            
            // Event statistics
            var eventStats = allDonations
                .Where(d => !string.IsNullOrEmpty(d.EventName))
                .GroupBy(d => new { d.EventId, d.EventName, d.EventDate })
                .Select(g => new EventStatDto {
                    EventName = g.Key.EventName!,
                    EventDate = g.Key.EventDate,
                    TotalDonations = g.Count(),
                    TotalVolume = g.Sum(d => d.Volume),
                    CompletedDonations = g.Count(d => d.Status == EnumMapper.DonationStatus.Completed)
                })
                .OrderByDescending(x => x.TotalDonations)
                .Take(10)
                .ToList();
            
            ViewBag.TotalDonations = totalDonations;
            ViewBag.TotalVolume = totalVolume;
            ViewBag.CompletedDonations = completedDonations;
            ViewBag.CancelledDonations = cancelledDonations;
            ViewBag.CertificatesIssued = certificatesIssued;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.BloodTypeStats = bloodTypeStats;
            ViewBag.MonthlyStats = monthlyStats;
            ViewBag.RecentDonations = recentDonations;
            ViewBag.TopDonors = topDonors;
            ViewBag.EventStats = eventStats;
            ViewBag.AllDonations = allDonations;
            
            return View();
        }

        // GET: /DonationHistory/Reports
        [AuthenticatedUser]
        public async Task<IActionResult> Reports()
        {
            // Chỉ cho phép Admin, Hospital, Doctor truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang báo cáo này.";
                return RedirectToAction("MyHistory");
            }

            // Lấy dữ liệu như action Statistics
            var allDonations = await _donationHistoryService.GetAllDonationsAsync();
            var totalDonations = await _donationHistoryService.GetTotalDonationsAsync();
            var totalVolume = await _donationHistoryService.GetTotalVolumeAsync();
            var completedDonations = allDonations.Count(d => d.Status == EnumMapper.DonationStatus.Completed);
            var cancelledDonations = allDonations.Count(d => d.Status == EnumMapper.DonationStatus.Cancelled);
            var certificatesIssued = allDonations.Count(d => d.CertificateIssued);
            var totalUsers = allDonations.Select(d => d.UserId).Distinct().Count();
            var bloodTypeStats = allDonations
                .Where(d => !string.IsNullOrEmpty(d.BloodTypeName))
                .GroupBy(d => d.BloodTypeName)
                .Select(g => new BloodTypeStatDto { 
                    BloodType = g.Key!, 
                    Count = g.Count(),
                    TotalVolume = g.Sum(d => d.Volume)
                })
                .OrderByDescending(x => x.Count)
                .ToList();
            var monthlyStats = new Dictionary<string, int>();
            for (int i = 11; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var monthKey = month.ToString("MM/yyyy");
                var monthDonations = allDonations.Count(d => d.DonationDate.Month == month.Month && d.DonationDate.Year == month.Year);
                monthlyStats[monthKey] = monthDonations;
            }
            var recentDonations = allDonations.OrderByDescending(d => d.DonationDate).Take(10).ToList();
            var topDonors = allDonations
                .GroupBy(d => new { d.UserId, d.UserName, d.UserEmail })
                .Select(g => new TopDonorDto {
                    UserName = g.Key.UserName ?? "Không xác định",
                    UserEmail = g.Key.UserEmail ?? "N/A",
                    TotalDonations = g.Count(),
                    TotalVolume = g.Sum(d => d.Volume),
                    LastDonation = g.Max(d => d.DonationDate)
                })
                .OrderByDescending(x => x.TotalDonations)
                .Take(10)
                .ToList();
            var eventStats = allDonations
                .Where(d => !string.IsNullOrEmpty(d.EventName))
                .GroupBy(d => new { d.EventId, d.EventName, d.EventDate })
                .Select(g => new EventStatDto {
                    EventName = g.Key.EventName!,
                    EventDate = g.Key.EventDate,
                    TotalDonations = g.Count(),
                    TotalVolume = g.Sum(d => d.Volume),
                    CompletedDonations = g.Count(d => d.Status == EnumMapper.DonationStatus.Completed)
                })
                .OrderByDescending(x => x.TotalDonations)
                .Take(10)
                .ToList();
            ViewBag.TotalDonations = totalDonations;
            ViewBag.TotalVolume = totalVolume;
            ViewBag.CompletedDonations = completedDonations;
            ViewBag.CancelledDonations = cancelledDonations;
            ViewBag.CertificatesIssued = certificatesIssued;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.BloodTypeStats = bloodTypeStats;
            ViewBag.MonthlyStats = monthlyStats;
            ViewBag.RecentDonations = recentDonations;
            ViewBag.TopDonors = topDonors;
            ViewBag.EventStats = eventStats;
            ViewBag.AllDonations = allDonations;
            return View();
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