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
    /// <summary>
    /// Controller quản lý đăng ký hiến máu (dành cho Donor và Staff)
    /// Xử lý: Đăng ký tham gia sự kiện, Check-in, Hủy đăng ký, Xem lịch sử
    /// Quy trình: Registered → CheckedIn → Screening → Eligible → InProgress → Completed
    /// </summary>
    [Authorize]
    public class DonationRegistrationController : Controller
    {
        // Dependencies
        private readonly IDonationRegistrationService _registrationService; // Service đăng ký hiến máu
        private readonly IHealthScreeningService _healthScreeningService; // Service sàng lọc sức khỏe

        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public DonationRegistrationController(IDonationRegistrationService registrationService, IHealthScreeningService healthScreeningService)
        {
            _registrationService = registrationService;
            _healthScreeningService = healthScreeningService;
        }

        /// <summary>
        /// GET: /DonationRegistration/Checkin
        /// Trang check-in cho người đăng ký khi đến sự kiện (dành cho Staff)
        /// Ban đầu hiển thị trang trống, chờ nhập mã đăng ký hoặc số điện thoại
        /// </summary>
        [HttpGet]
        [DoctorOrStaff]
        public IActionResult Checkin()
        {
            // Trang check-in ban đầu, không có dữ liệu
            return View(new List<DonationRegistrationDto>());
        }

        /// <summary>
        /// POST: /DonationRegistration/Checkin
        /// Tìm kiếm đăng ký để check-in theo mã đăng ký hoặc số điện thoại
        /// Hoặc hiển thị tất cả đăng ký trong hệ thống
        /// </summary>
        /// <param name="RegistrationCode">Mã đăng ký hoặc số điện thoại cần tìm</param>
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
                // Tìm kiếm theo mã đăng ký hoặc số điện thoại
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

        /// <summary>
        /// POST: /DonationRegistration/ConfirmCheckin
        /// Xác nhận check-in cho một đăng ký
        /// Kiểm tra: Trạng thái phù hợp, Thời gian sự kiện (chỉ check-in vào ngày và giờ sự kiện)
        /// Chuyển trạng thái từ Registered/Confirmed → CheckedIn
        /// </summary>
        /// <param name="id">ID đăng ký cần check-in</param>
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
            
            // Thực hiện check-in
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

        /// <summary>
        /// GET: /DonationRegistration/MyRegistrations
        /// Xem danh sách đăng ký hiến máu của người dùng hiện tại
        /// Có thể lọc theo trạng thái: all (tất cả), active (đang hoạt động), confirmed, completed, cancelled, deferred
        /// </summary>
        /// <param name="status">Trạng thái cần lọc (mặc định: all)</param>
        [AuthenticatedUser]
        public async Task<IActionResult> MyRegistrations(string status = "all")
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

                // Lọc theo trạng thái
                if (status == "active")
                {
                    var activeStatuses = new RegistrationStatus[] { RegistrationStatus.Registered, RegistrationStatus.Confirmed, RegistrationStatus.CheckedIn, RegistrationStatus.Screening, RegistrationStatus.Eligible, RegistrationStatus.Ineligible, RegistrationStatus.Donating };
                    filtered = allRegs.Where(r => activeStatuses.Contains(r.Status)).ToList();
                }
                else if (status == "confirmed")
                {
                    filtered = allRegs.Where(r => r.Status == RegistrationStatus.Confirmed).ToList();
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
                else if (status == "deferred")
                {
                    filtered = allRegs.Where(r => r.Status == RegistrationStatus.Deferred).ToList();
                }
                // else: show all (mặc định)

                return View(filtered);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách đăng ký.";
                return View(new List<DonationRegistrationDto>());
            }
        }

        /// <summary>
        /// GET: /DonationRegistration/Details/{id}
        /// Xem chi tiết một đăng ký hiến máu
        /// Bao gồm: Thông tin đăng ký, Sự kiện, Trạng thái, Kết quả sàng lọc sức khỏe (nếu có)
        /// Chỉ chủ đăng ký mới được xem
        /// </summary>
        /// <param name="id">ID đăng ký</param>
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

        /// <summary>
        /// POST: /DonationRegistration/Cancel/{id}
        /// Hủy đăng ký hiến máu
        /// Điều kiện: Chỉ hủy được khi trạng thái là Registered hoặc Confirmed
        /// Kiểm tra thời gian: Không cho hủy nếu sự kiện đã bắt đầu
        /// Hỗ trợ cả AJAX và form thường
        /// </summary>
        /// <param name="id">ID đăng ký cần hủy</param>
        /// <param name="reason">Lý do hủy (tùy chọn)</param>
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

                // Kiểm tra xem đăng ký có thể hủy không - Cho phép hủy khi đang ở trạng thái Registered hoặc Confirmed
                var cancellableStatuses = new[] { RegistrationStatus.Registered, RegistrationStatus.Confirmed };
                if (!cancellableStatuses.Contains(registration.Status))
                {
                    var statusMessage = registration.Status switch
                    {
                        RegistrationStatus.CheckedIn => "Bạn đã check-in, không thể hủy đăng ký.",
                        RegistrationStatus.Screening => "Bạn đang trong quá trình sàng lọc sức khỏe, không thể hủy đăng ký.",
                        RegistrationStatus.Donating => "Bạn đang trong quá trình hiến máu, không thể hủy đăng ký.",
                        RegistrationStatus.Completed => "Đăng ký đã hoàn thành, không thể hủy.",
                        RegistrationStatus.Cancelled => "Đăng ký đã được hủy trước đó.",
                        _ => "Không thể hủy đăng ký ở trạng thái hiện tại."
                    };
                    
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = statusMessage });
                    }
                    TempData["Error"] = statusMessage;
                    return RedirectToAction(nameof(Details), new { id });
                }

                // Kiểm tra thời gian - không cho hủy nếu sự kiện đã bắt đầu hoặc đã qua
                if (registration.EventDate.HasValue)
                {
                    var eventDateTime = registration.EventDate.Value;
                    if (registration.EventStartTime.HasValue)
                    {
                        eventDateTime = eventDateTime.Date.Add(registration.EventStartTime.Value);
                    }

                    if (DateTime.Now >= eventDateTime)
                    {
                        var message = "Sự kiện đã bắt đầu hoặc đã qua, không thể hủy đăng ký.";
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message });
                        }
                        TempData["Error"] = message;
                        return RedirectToAction(nameof(Details), new { id });
                    }
                }

                // Xử lý lý do hủy - nếu không có lý do thì dùng "Other"
                DisqualificationReason cancelReason = DisqualificationReason.Other;
                if (!string.IsNullOrEmpty(reason))
                {
                    // Nếu reason là text tự do, lưu vào CancellationReason
                    if (!Enum.TryParse<DisqualificationReason>(reason, out cancelReason))
                    {
                        cancelReason = DisqualificationReason.Other;
                    }
                }

                var success = await _registrationService.CancelRegistrationAsync(id, cancelReason);
                if (success)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Đã hủy đăng ký thành công." });
                    }
                    TempData["Success"] = "Đã hủy đăng ký thành công.";
                    return RedirectToAction(nameof(MyRegistrations));
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đăng ký." });
                    }
                    TempData["Error"] = "Có lỗi xảy ra khi hủy đăng ký.";
                    return RedirectToAction(nameof(MyRegistrations));
                }
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

        /// <summary>
        /// POST: /DonationRegistration/CancelCheckin
        /// Hủy check-in cho một đăng ký (dành cho Staff)
        /// Chuyển trạng thái từ CheckedIn → Registered/Confirmed
        /// </summary>
        /// <param name="id">ID đăng ký cần hủy check-in</param>
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

        /// <summary>
        /// GET: /DonationRegistration/MyStatistics
        /// Xem thống kê đăng ký của người dùng hiện tại
        /// Bao gồm: Tổng số đăng ký, Hoàn thành, Đang chờ, Đã hủy, Đã check-in, Hoãn lại
        /// </summary>
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
                    CheckedInRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, RegistrationStatus.CheckedIn),
                    DeferredRegistrations = await _registrationService.GetRegistrationCountByUserAndStatusAsync(userId, RegistrationStatus.Deferred)
                };

                return View("MyStatistics", statistics);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thống kê.";
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        /// <summary>
        /// GET: /DonationRegistration/Statistics
        /// Xem thống kê tổng quan tất cả đăng ký trong hệ thống (dành cho Admin/Staff)
        /// Bao gồm: Tổng số, Hoàn thành, Đã duyệt, Đã hủy, Check-in, Hoãn lại
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu lọc (tùy chọn)</param>
        /// <param name="endDate">Ngày kết thúc lọc (tùy chọn)</param>
        [Authorize(Roles = "Admin, Staff, Doctor, Hospital")]
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var statistics = new RegistrationStatisticsViewModel
                {
                    TotalRegistrations = await _registrationService.GetRegistrationCountAsync(),
                    CompletedRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Completed),
                    ApprovedRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Confirmed),
                    CancelledRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Cancelled),
                    CheckedInRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.CheckedIn),
                    DeferredRegistrations = await _registrationService.GetRegistrationCountByStatusAsync(RegistrationStatus.Deferred)
                };
                
                // Lưu giá trị lọc để hiển thị trên view
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
                
                return View("Statistics", statistics);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải thống kê.";
                return RedirectToAction("MyRegistrations");
            }
        }

        /// <summary>
        /// Helper method: Lấy UserId của người dùng hiện tại từ Claims
        /// </summary>
        /// <returns>UserId (0 nếu không tìm thấy)</returns>
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