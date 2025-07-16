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

        // GET: /DonationRegistration/MyRegistrations
        public async Task<IActionResult> MyRegistrations()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var registrations = await _registrationService.GetRegistrationsByUserAsync(userId);
                return View(registrations);
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải chi tiết đăng ký.";
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        // POST: /DonationRegistration/Cancel/{id}
        [HttpPost]
        public async Task<IActionResult> Cancel(int id, string reason)
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
                    TempData["Error"] = "Bạn không có quyền hủy đăng ký này.";
                    return RedirectToAction(nameof(MyRegistrations));
                }

                // Kiểm tra xem đăng ký có thể hủy không
                if (registration.Status != "Registered" && registration.Status != "Approved")
                {
                    TempData["Error"] = "Đăng ký này không thể hủy.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var success = await _registrationService.CancelRegistrationAsync(id, reason);
                if (success)
                {
                    TempData["Success"] = "Đã hủy đăng ký thành công.";
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra khi hủy đăng ký.";
                }

                return RedirectToAction(nameof(MyRegistrations));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi hủy đăng ký.";
                return RedirectToAction(nameof(MyRegistrations));
            }
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
            catch (Exception ex)
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