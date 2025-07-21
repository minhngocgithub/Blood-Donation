using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Models.ViewModels;
using Blood_Donation_Website.Utilities.Filters;
using System.Security.Claims;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Controllers
{
    [Authorize]
    public class DonationRegistrationController : Controller
    {
        private readonly IDonationRegistrationService _registrationService;
        private readonly IHealthScreeningService _healthScreeningService;

        public DonationRegistrationController(IDonationRegistrationService registrationService, IHealthScreeningService healthScreeningService)
        {
            _registrationService = registrationService;
            _healthScreeningService = healthScreeningService;
        }

        // GET: /DonationRegistration/Checkin
        [HttpGet]
        [DoctorOrStaff]
        public IActionResult Checkin()
        {
            // Trang check-in ban đầu, không có dữ liệu
            return View(new List<DonationRegistrationDto>());
        }

        // POST: /DonationRegistration/Checkin
        [HttpPost]
        [DoctorOrStaff]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkin(string RegistrationCode)
        {
            var showAll = Request.Form["showAll"].ToString();
            IEnumerable<DonationRegistrationDto> results;
            
            // Kiểm tra showAll trước
            if (!string.IsNullOrEmpty(showAll) && showAll == "true")
            {
                // Hiển thị tất cả đăng ký
                results = await _registrationService.GetAllRegistrationsAsync();
                if (results == null || !results.Any())
                {
                    TempData["Info"] = "Chưa có đăng ký nào trong hệ thống.";
                }
                else
                {
                    TempData["Success"] = $"Đã tải {results.Count()} đăng ký.";
                }
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
        [DoctorOrStaff]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckin(int id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                TempData["Error"] = "Không tìm thấy đăng ký.";
                return RedirectToAction("Checkin");
            }
            
            // Kiểm tra trạng thái có thể check-in
            var allowedStatuses = new RegistrationStatus[] { RegistrationStatus.Registered, RegistrationStatus.Confirmed };
            if (!allowedStatuses.Contains(registration.Status))
            {
                TempData["Error"] = $"Không thể check-in với trạng thái '{registration.Status}'.";
                return RedirectToAction("Checkin");
            }
            
            if (registration.Status == RegistrationStatus.CheckedIn)
            {
                TempData["Error"] = "Người này đã được check-in.";
                return RedirectToAction("Checkin");
            }
            
            // Kiểm tra thời gian check-in
            var currentDateTime = DateTime.Now;
            var eventDate = registration.EventDate?.Date ?? DateTime.MinValue;
            var eventStartTime = registration.EventStartTime ?? TimeSpan.Zero;
            var eventEndTime = registration.EventEndTime ?? TimeSpan.Zero;
            
            // Kiểm tra ngày
            if (currentDateTime.Date != eventDate)
            {
                TempData["Error"] = $"Chỉ có thể check-in vào ngày {eventDate:dd/MM/yyyy}.";
                return RedirectToAction("Checkin");
            }
            
            // Kiểm tra giờ (cho phép check-in từ 30 phút trước giờ bắt đầu đến giờ kết thúc)
            var eventStartDateTime = eventDate.Add(eventStartTime);
            var checkinStartTime = eventStartDateTime.AddMinutes(-30); // Cho phép check-in sớm 30 phút
            var eventEndDateTime = eventDate.Add(eventEndTime);
            
            if (currentDateTime < checkinStartTime)
            {
                var timeUntilStart = checkinStartTime - currentDateTime;
                var hours = (int)timeUntilStart.TotalHours;
                var minutes = timeUntilStart.Minutes;
                var timeMessage = hours > 0 ? $"{hours} giờ {minutes} phút" : $"{minutes} phút";
                TempData["Error"] = $"Chưa đến giờ check-in. Vui lòng quay lại sau {timeMessage} nữa.";
                return RedirectToAction("Checkin");
            }
            
            if (currentDateTime > eventEndDateTime)
            {
                TempData["Error"] = $"Đã quá giờ check-in. Sự kiện kết thúc lúc {eventEndTime:hh\\:mm}.";
                return RedirectToAction("Checkin");
            }
            
            var success = await _registrationService.CheckinRegistrationAsync(id);
            if (success)
            {
                TempData["Success"] = "Check-in thành công. Vui lòng đưa người hiến đến khu vực sàng lọc sức khỏe.";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi check-in.";
            }
            return RedirectToAction("Checkin");
        }

        // GET: /DonationRegistration/MyRegistrations
        [AuthenticatedUser]
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
                    var activeStatuses = new RegistrationStatus[] { RegistrationStatus.Registered, RegistrationStatus.CheckedIn, RegistrationStatus.Screening, RegistrationStatus.Eligible, RegistrationStatus.Ineligible, RegistrationStatus.Donating };
                    filtered = allRegs.Where(r => activeStatuses.Contains(r.Status)).ToList();
                }
                else if (status == "completed")
                {
                    filtered = allRegs.Where(r => r.Status == RegistrationStatus.Completed).ToList();
                }
                else if (status == "cancelled")
                {
                    var cancelledStatuses = new RegistrationStatus[] { RegistrationStatus.Cancelled, RegistrationStatus.Failed, RegistrationStatus.NoShow };
                    filtered = allRegs.Where(r => cancelledStatuses.Contains(r.Status)).ToList();
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
        [AuthenticatedUser]
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

                // Lấy screening mới nhất nếu có
                var screening = await _healthScreeningService.GetLatestScreeningByRegistrationIdAsync(id);
                ViewBag.HealthScreening = screening;

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
        [AuthenticatedUser]
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
                if (registration.Status != RegistrationStatus.Registered)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Đăng ký này không thể hủy." });
                    }
                    TempData["Error"] = "Đăng ký này không thể hủy.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                if (Enum.TryParse<DisqualificationReason>(reason, out var reasonEnum))
                {
                    var success = await _registrationService.CancelRegistrationAsync(id, reasonEnum);
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
                }
                else
                {
                    TempData["Error"] = "Lý do hủy không hợp lệ.";
                    return RedirectToAction(nameof(MyRegistrations));
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
        [DoctorOrStaff]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelCheckin(int id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                TempData["Error"] = "Không tìm thấy đăng ký.";
                return RedirectToAction("Checkin");
            }
            if (registration.Status != RegistrationStatus.CheckedIn)
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

        // GET: /DonationRegistration/MyStatistics
        [AuthenticatedUser]
        public async Task<IActionResult> MyStatistics()
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
                    CompletedRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, RegistrationStatus.Completed),
                    PendingRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, RegistrationStatus.Registered),
                    CancelledRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, RegistrationStatus.Cancelled),
                    CheckedInRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, RegistrationStatus.CheckedIn)
                };

                return View("MyStatistics", statistics);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thống kê.";
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        // GET: /DonationRegistration/Statistics
        [Authorize(Roles = "Admin, Staff, Doctor, Hospital")]
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var statistics = new RegistrationStatisticsViewModel
                {
                    TotalRegistrations = await _registrationService.GetRegistrationCountAsync(),
                    CompletedRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Completed),
                    PendingRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Registered),
                    CancelledRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Cancelled),
                    CheckedInRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.CheckedIn)
                };
                return View("Statistics", statistics);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thống kê.";
                return RedirectToAction("MyRegistrations");
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