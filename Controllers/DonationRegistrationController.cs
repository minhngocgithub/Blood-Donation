using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.ViewModels;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    [Authorize]
    public class DonationRegistrationController : Controller
    {
        private readonly IDonationRegistrationService _registrationService;

        public DonationRegistrationController(IDonationRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // GET: /DonationRegistration/Checkin
        [HttpGet]
        [Authorize(Roles = "Admin,Hospital,Staff,Doctor")]
        public IActionResult Checkin()
        {
            // Trang check-in ban đầu, không có dữ liệu
            return View(new List<DonationRegistrationDto>());
        }

        // POST: /DonationRegistration/Checkin
        [HttpPost]
        [Authorize(Roles = "Admin,Hospital,Staff,Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkin(string RegistrationCode)
        {
            var showAll = Request.Form["showAll"].ToString();
            IEnumerable<DonationRegistrationDto> results;
            if (!string.IsNullOrEmpty(showAll) && showAll == "true")
            {
                // Hiển thị tất cả đăng ký chưa check-in
                results = await _registrationService.GetRegistrationsByStatusAsync("Registered");
            }
            else if (!string.IsNullOrWhiteSpace(RegistrationCode))
            {
                results = await _registrationService.SearchRegistrationsForCheckinAsync(RegistrationCode);
                if (results == null || !results.Any())
                {
                    TempData["Error"] = "Không tìm thấy đăng ký phù hợp.";
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng nhập mã đăng ký hoặc số điện thoại hoặc chọn 'Hiển thị tất cả'.";
                results = new List<DonationRegistrationDto>();
            }
            return View(results);
        }

        // POST: /DonationRegistration/ConfirmCheckin
        [HttpPost]
        [Authorize(Roles = "Admin,Hospital,Staff,Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckin(int id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                TempData["Error"] = "Không tìm thấy đăng ký.";
                return RedirectToAction("Checkin");
            }
            if (registration.Status == "CheckedIn")
            {
                TempData["Error"] = "Người này đã được check-in.";
                return RedirectToAction("Checkin");
            }
            var success = await _registrationService.CheckinRegistrationAsync(id);
            if (success)
            {
                TempData["Success"] = "Check-in thành công.";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi check-in.";
            }
            return RedirectToAction("Checkin");
        }

        // GET: /DonationRegistration/MyRegistrations
        public async Task<IActionResult> MyRegistrations(string status = "active")
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var allRegs = await _registrationService.GetRegistrationsByUserAsync(userId);
                IEnumerable<DonationRegistrationDto> filtered = allRegs;

                if (string.IsNullOrEmpty(status) || status == "active")
                {
                    var activeStatuses = new[] { "Registered", "Approved", "CheckedIn", "Screening", "Eligible", "Ineligible", "Donating" };
                    filtered = allRegs.Where(r => activeStatuses.Contains(r.Status));
                }
                else if (status == "completed")
                {
                    filtered = allRegs.Where(r => r.Status == "Completed");
                }
                else if (status == "cancelled")
                {
                    var cancelledStatuses = new[] { "Cancelled", "Rejected", "Failed", "NoShow" };
                    filtered = allRegs.Where(r => cancelledStatuses.Contains(r.Status));
                }
                // else: show all

                return View(filtered);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách đăng ký.";
                return View(new List<DonationRegistrationDto>());
            }
        }

        // GET: /DonationRegistration/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var registration = await _registrationService.GetRegistrationByIdAsync(id);
                if (registration == null)
                {
                    TempData["Error"] = "Không tìm thấy đăng ký.";
                    return RedirectToAction(nameof(MyRegistrations));
                }

                // Kiểm tra xem đăng ký có thuộc về user hiện tại không
                if (registration.UserId != userId)
                {
                    TempData["Error"] = "Bạn không có quyền xem đăng ký này.";
                    return RedirectToAction(nameof(MyRegistrations));
                }

                return View(registration);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải chi tiết đăng ký.";
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        // POST: /DonationRegistration/Cancel/{id}
        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string? reason = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện thao tác này." });
                    }
                    return RedirectToAction("Login", "Account");
                }

                var registration = await _registrationService.GetRegistrationByIdAsync(id);
                if (registration == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Không tìm thấy đăng ký." });
                    }
                    TempData["Error"] = "Không tìm thấy đăng ký.";
                    return RedirectToAction(nameof(MyRegistrations));
                }

                // Kiểm tra xem đăng ký có thuộc về user hiện tại không
                if (registration.UserId != userId)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Bạn không có quyền hủy đăng ký này." });
                    }
                    TempData["Error"] = "Bạn không có quyền hủy đăng ký này.";
                    return RedirectToAction(nameof(MyRegistrations));
                }

                // Kiểm tra xem đăng ký có thể hủy không
                if (registration.Status != "Registered" && registration.Status != "Approved")
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Đăng ký này không thể hủy." });
                    }
                    TempData["Error"] = "Đăng ký này không thể hủy.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var success = await _registrationService.CancelRegistrationAsync(id, reason ?? string.Empty);
                if (success)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Đã hủy đăng ký thành công." });
                    }
                    TempData["Success"] = "Đã hủy đăng ký thành công.";
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đăng ký." });
                    }
                    TempData["Error"] = "Có lỗi xảy ra khi hủy đăng ký.";
                }

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đăng ký." });
                }
                return RedirectToAction(nameof(MyRegistrations));
            }
            catch
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đăng ký." });
                }
                TempData["Error"] = "Có lỗi xảy ra khi hủy đăng ký.";
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        // POST: /DonationRegistration/CancelCheckin
        [HttpPost]
        [Authorize(Roles = "Admin,Hospital,Staff,Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelCheckin(int id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                TempData["Error"] = "Không tìm thấy đăng ký.";
                return RedirectToAction("Checkin");
            }
            if (registration.Status != "CheckedIn")
            {
                TempData["Error"] = "Chỉ có thể hủy check-in cho đăng ký đã check-in.";
                return RedirectToAction("Checkin");
            }
            var success = await _registrationService.CancelCheckinAsync(id);
            if (success)
            {
                TempData["Success"] = "Đã hủy check-in thành công.";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi hủy check-in.";
            }
            return RedirectToAction("Checkin");
        }

        // GET: /DonationRegistration/Statistics
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var statistics = new RegistrationStatisticsViewModel
                {
                    TotalRegistrations = await _registrationService.GetRegistrationCountByUserAsync(userId),
                    CompletedRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, "Completed"),
                    PendingRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, "Registered"),
                    ApprovedRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, "Approved"),
                    CancelledRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, "Cancelled"),
                    RejectedRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, "Rejected"),
                    CheckedInRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, "CheckedIn")
                };

                return View(statistics);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thống kê.";
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
} 