using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Utilities;
using Blood_Donation_Website.Models.DTOs;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý lịch sử hiến máu (dành cho Donor và Staff)
    /// Xử lý: Xem lịch sử hiến máu cá nhân, Tìm kiếm, Thống kê, Báo cáo
    /// Tính năng: Lọc theo ngày/nhóm máu/trạng thái, Sắp xếp, Tìm kiếm người dùng
    /// </summary>
    [AuthenticatedUser]
    public class DonationHistoryController : Controller
    {
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IProfileService _profileService;

        public DonationHistoryController(IDonationHistoryService donationHistoryService, IProfileService profileService)
        {
            _donationHistoryService = donationHistoryService;
            _profileService = profileService;
        }

        /// <summary>
        /// GET: /DonationHistory/MyHistory
        /// Xem lịch sử hiến máu của người dùng hiện tại
        /// Manager (Admin/Hospital/Doctor) có thể xem tất cả lịch sử khi có bộ lọc
        /// User thường chỉ xem lịch sử của mình
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tên, email, ID donation, tên sự kiện)</param>
        /// <param name="bloodType">Lọc theo nhóm máu</param>
        /// <param name="status">Lọc theo trạng thái (Completed, Cancelled...)</param>
        /// <param name="sortBy">Sắp xếp: date_asc/desc, name_asc/desc, volume_asc/desc, bloodtype_asc/desc</param>
        /// <param name="fromDate">Lọc từ ngày</param>
        /// <param name="toDate">Lọc đến ngày</param>
        [AuthenticatedUser]
        public async Task<IActionResult> MyHistory(string searchTerm = "", string bloodType = "", string status = "", string sortBy = "date_desc", DateTime? fromDate = null, DateTime? toDate = null)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            // Kiểm tra xem có phải là vai trò quản lý không
            bool isManager = User.IsInRole("Admin") || User.IsInRole("Hospital") || User.IsInRole("Doctor");
            
            IEnumerable<DonationHistoryDto> donations;
            
            // Nếu là manager và có tìm kiếm, lấy tất cả
            if (isManager && (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(bloodType) || !string.IsNullOrEmpty(status) || fromDate.HasValue || toDate.HasValue))
            {
                donations = await _donationHistoryService.GetAllDonationsAsync();
                ViewBag.IsManagerView = true;
            }
            else
            {
                // Nếu là user thường, chỉ lấy của mình
                donations = await _donationHistoryService.GetDonationsByUserAsync(userId);
                ViewBag.IsManagerView = false;
            }

            // Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                donations = donations.Where(d => 
                    (d.UserName != null && d.UserName.ToLower().Contains(searchTerm)) ||
                    (d.UserEmail != null && d.UserEmail.ToLower().Contains(searchTerm)) ||
                    (d.EventName != null && d.EventName.ToLower().Contains(searchTerm)) ||
                    d.DonationId.ToString().Contains(searchTerm)
                ).ToList();
            }

            // Lọc theo nhóm máu
            if (!string.IsNullOrWhiteSpace(bloodType))
            {
                donations = donations.Where(d => d.BloodTypeName == bloodType).ToList();
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<EnumMapper.DonationStatus>(status, out var donationStatus))
            {
                donations = donations.Where(d => d.Status == donationStatus).ToList();
            }

            // Lọc theo khoảng thời gian
            if (fromDate.HasValue)
            {
                donations = donations.Where(d => d.DonationDate.Date >= fromDate.Value.Date).ToList();
            }
            if (toDate.HasValue)
            {
                donations = donations.Where(d => d.DonationDate.Date <= toDate.Value.Date).ToList();
            }

            // Sắp xếp
            donations = sortBy switch
            {
                "date_asc" => donations.OrderBy(d => d.DonationDate).ToList(),
                "date_desc" => donations.OrderByDescending(d => d.DonationDate).ToList(),
                "name_asc" => donations.OrderBy(d => d.UserName).ToList(),
                "name_desc" => donations.OrderByDescending(d => d.UserName).ToList(),
                "volume_asc" => donations.OrderBy(d => d.Volume).ToList(),
                "volume_desc" => donations.OrderByDescending(d => d.Volume).ToList(),
                "bloodtype_asc" => donations.OrderBy(d => d.BloodTypeName).ToList(),
                "bloodtype_desc" => donations.OrderByDescending(d => d.BloodTypeName).ToList(),
                _ => donations.OrderByDescending(d => d.DonationDate).ToList()
            };

            // Truyền dữ liệu cho view
            ViewBag.SearchTerm = searchTerm;
            ViewBag.BloodType = bloodType;
            ViewBag.Status = status;
            ViewBag.SortBy = sortBy;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            
            // Lấy tất cả nhóm máu từ database
            var allBloodTypes = await _profileService.GetBloodTypesAsync();
            ViewBag.BloodTypes = allBloodTypes.Select(bt => bt.BloodTypeName).OrderBy(n => n).ToList();

            return View(donations);
        }

        /// <summary>
        /// GET: /DonationHistory/Details/{id}
        /// Xem chi tiết một lần hiến máu
        /// Bao gồm: Thông tin hiến máu, Ngày đủ điều kiện hiến lại (DonationDate + 90 ngày)
        /// Chỉ chủ sở hữu hoặc Staff mới được xem
        /// Tự động cập nhật NextEligibleDate khi xem
        /// </summary>
        /// <param name="id">ID lịch sử hiến máu</param>
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

        /// <summary>
        /// GET: /DonationHistory/Statistics
        /// Xem thống kê tổng quan lịch sử hiến máu (chỉ dành cho Admin/Hospital/Doctor)
        /// Bao gồm:
        /// - Tổng số lần hiến máu, tổng lượng máu, số người hiến
        /// - Phân bố theo nhóm máu
        /// - Xu hướng theo tháng (12 tháng gần nhất)
        /// - Top 10 người hiến nhiều nhất
        /// - Top 10 sự kiện hiến máu nhiều nhất
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu lọc (tùy chọn)</param>
        /// <param name="endDate">Ngày kết thúc lọc (tùy chọn)</param>
        [AuthenticatedUser]
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            // Check if user has admin, hospital, or doctor role
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang thống kê này.";
                return RedirectToAction("MyHistory");
            }

            // Get all donations for statistics
            var allDonationsQuery = await _donationHistoryService.GetAllDonationsAsync();
            var allDonations = allDonationsQuery.ToList();
            
            // Áp dụng bộ lọc theo ngày nếu có
            if (startDate.HasValue)
            {
                allDonations = allDonations.Where(d => d.DonationDate.Date >= startDate.Value.Date).ToList();
            }
            if (endDate.HasValue)
            {
                allDonations = allDonations.Where(d => d.DonationDate.Date <= endDate.Value.Date).ToList();
            }
            
            // Lưu giá trị lọc để hiển thị trên view
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            // Tính toán dựa trên dữ liệu đã lọc
            var totalDonations = allDonations.Count;
            var totalVolume = allDonations.Any() ? allDonations.Sum(d => (int)d.Volume) : 0;
            
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
                    UserName = g.Key.UserName ?? "Kh\u00f4ng x\u00e1c \u0111\u1ecbnh", 
                    UserEmail = g.Key.UserEmail ?? "N/A",
                    TotalDonations = g.Count(),
                    TotalVolume = g.Sum(d => (int)d.Volume),
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
                    TotalVolume = g.Sum(d => (int)d.Volume),
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

        /// <summary>
        /// GET: /DonationHistory/Reports
        /// Xem báo cáo chi tiết lịch sử hiến máu (dành cho Admin/Hospital/Doctor)
        /// Tương tự Statistics nhưng có thể xuất báo cáo
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu lọc (tùy chọn)</param>
        /// <param name="endDate">Ngày kết thúc lọc (tùy chọn)</param>
        [AuthenticatedUser]
        public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
        {
            // Chỉ cho phép Admin, Hospital, Doctor truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor"))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập trang báo cáo này.";
                return RedirectToAction("MyHistory");
            }

            // Lấy dữ liệu như action Statistics
            var allDonationsQuery = await _donationHistoryService.GetAllDonationsAsync();
            var allDonations = allDonationsQuery.ToList();
            
            // Áp dụng bộ lọc theo ngày nếu có
            if (startDate.HasValue)
            {
                allDonations = allDonations.Where(d => d.DonationDate.Date >= startDate.Value.Date).ToList();
            }
            if (endDate.HasValue)
            {
                allDonations = allDonations.Where(d => d.DonationDate.Date <= endDate.Value.Date).ToList();
            }
            
            // Lưu giá trị lọc để hiển thị trên view
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            // Tính toán dựa trên dữ liệu đã lọc
            var totalDonations = allDonations.Count;
            var totalVolume = allDonations.Any() ? allDonations.Sum(d => (int)d.Volume) : 0;
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
                    TotalVolume = g.Sum(d => (int)d.Volume)
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
                    UserName = g.Key.UserName ?? "Kh\u00f4ng x\u00e1c \u0111\u1ecbnh",
                    UserEmail = g.Key.UserEmail ?? "N/A",
                    TotalDonations = g.Count(),
                    TotalVolume = g.Sum(d => (int)d.Volume),
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
                    TotalVolume = g.Sum(d => (int)d.Volume),
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

        // GET: /DonationHistory/SearchUser
        [AuthenticatedUser]
        public async Task<IActionResult> SearchUser(string searchTerm)
        {
            // Chỉ cho phép Admin, Hospital, Doctor, Staff truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor") && !User.IsInRole("Staff"))
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này." });
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new { success = false, message = "Vui lòng nhập email hoặc số điện thoại." });
            }

            try
            {
                // Tìm kiếm người dùng qua UserService
                var userService = HttpContext.RequestServices.GetService<IUserService>();
                if (userService == null)
                {
                    return Json(new { success = false, message = "Lỗi hệ thống." });
                }

                var allUsers = await userService.GetAllUsersAsync();
                var user = allUsers.FirstOrDefault(u => 
                    u.Email.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                    u.Phone == searchTerm);

                if (user != null)
                {
                    return Json(new { success = true, userId = user.UserId, userName = user.FullName, userEmail = user.Email });
                }

                return Json(new { success = false, message = "Không tìm thấy người dùng với thông tin này." });
            }
            catch
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tìm kiếm." });
            }
        }

        // GET: /DonationHistory/UserHistory
        [AuthenticatedUser]
        public async Task<IActionResult> UserHistory(int userId)
        {
            // Chỉ cho phép Admin, Hospital, Doctor, Staff truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor") && !User.IsInRole("Staff"))
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("MyHistory");
            }

            if (userId == 0)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("MyHistory");
            }

            try
            {
                var donations = await _donationHistoryService.GetDonationsByUserAsync(userId);
                
                // Lấy thông tin người dùng
                var userService = HttpContext.RequestServices.GetService<IUserService>();
                if (userService != null)
                {
                    var user = await userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        ViewBag.SearchedUser = user;
                        ViewBag.SearchedUserName = user.FullName;
                        ViewBag.SearchedUserEmail = user.Email;
                    }
                }
                
                return View("MyHistory", donations);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải lịch sử hiến máu.";
                return RedirectToAction("MyHistory");
            }
        }

        // GET: /DonationHistory/SearchUser
        [AuthenticatedUser]
        public async Task<IActionResult> SearchUser(string searchTerm)
        {
            // Chỉ cho phép Admin, Hospital, Doctor, Staff truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor") && !User.IsInRole("Staff"))
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này." });
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new { success = false, message = "Vui lòng nhập email hoặc số điện thoại." });
            }

            try
            {
                // Tìm kiếm người dùng qua UserService
                var userService = HttpContext.RequestServices.GetService<IUserService>();
                if (userService == null)
                {
                    return Json(new { success = false, message = "Lỗi hệ thống." });
                }

                var allUsers = await userService.GetAllUsersAsync();
                var user = allUsers.FirstOrDefault(u => 
                    u.Email.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                    u.Phone == searchTerm);

                if (user != null)
                {
                    return Json(new { success = true, userId = user.UserId, userName = user.FullName, userEmail = user.Email });
                }

                return Json(new { success = false, message = "Không tìm thấy người dùng với thông tin này." });
            }
            catch
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tìm kiếm." });
            }
        }

        // GET: /DonationHistory/UserHistory
        [AuthenticatedUser]
        public async Task<IActionResult> UserHistory(int userId)
        {
            // Chỉ cho phép Admin, Hospital, Doctor, Staff truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor") && !User.IsInRole("Staff"))
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("MyHistory");
            }

            if (userId == 0)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("MyHistory");
            }

            try
            {
                var donations = await _donationHistoryService.GetDonationsByUserAsync(userId);
                
                // Lấy thông tin người dùng
                var userService = HttpContext.RequestServices.GetService<IUserService>();
                if (userService != null)
                {
                    var user = await userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        ViewBag.SearchedUser = user;
                        ViewBag.SearchedUserName = user.FullName;
                        ViewBag.SearchedUserEmail = user.Email;
                    }
                }
                
                return View("MyHistory", donations);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải lịch sử hiến máu.";
                return RedirectToAction("MyHistory");
            }
        }

        /// <summary>
        /// GET: /DonationHistory/SearchUser (AJAX)
        /// Tìm kiếm người dùng theo email hoặc số điện thoại
        /// Trả về JSON với thông tin người dùng (userId, userName, userEmail)
        /// Dành cho Staff để tra cứu lịch sử hiến máu của người khác
        /// </summary>
        /// <param name="searchTerm">Email hoặc số điện thoại</param>
        [AuthenticatedUser]
        public async Task<IActionResult> SearchUser(string searchTerm)
        {
            // Chỉ cho phép Admin, Hospital, Doctor, Staff truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor") && !User.IsInRole("Staff"))
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này." });
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new { success = false, message = "Vui lòng nhập email hoặc số điện thoại." });
            }

            try
            {
                // Tìm kiếm người dùng qua UserService
                var userService = HttpContext.RequestServices.GetService<IUserService>();
                if (userService == null)
                {
                    return Json(new { success = false, message = "Lỗi hệ thống." });
                }

                var allUsers = await userService.GetAllUsersAsync();
                var user = allUsers.FirstOrDefault(u => 
                    u.Email.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                    u.Phone == searchTerm);

                if (user != null)
                {
                    return Json(new { success = true, userId = user.UserId, userName = user.FullName, userEmail = user.Email });
                }

                return Json(new { success = false, message = "Không tìm thấy người dùng với thông tin này." });
            }
            catch
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tìm kiếm." });
            }
        }

        /// <summary>
        /// GET: /DonationHistory/UserHistory
        /// Xem lịch sử hiến máu của một người dùng cụ thể (dành cho Staff)
        /// Hiển thị view MyHistory với dữ liệu của người dùng được chỉ định
        /// </summary>
        /// <param name="userId">ID người dùng cần xem lịch sử</param>
        [AuthenticatedUser]
        public async Task<IActionResult> UserHistory(int userId)
        {
            // Chỉ cho phép Admin, Hospital, Doctor, Staff truy cập
            if (!User.IsInRole("Admin") && !User.IsInRole("Hospital") && !User.IsInRole("Doctor") && !User.IsInRole("Staff"))
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("MyHistory");
            }

            if (userId == 0)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("MyHistory");
            }

            try
            {
                var donations = await _donationHistoryService.GetDonationsByUserAsync(userId);
                
                // Lấy thông tin người dùng
                var userService = HttpContext.RequestServices.GetService<IUserService>();
                if (userService != null)
                {
                    var user = await userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        ViewBag.SearchedUser = user;
                        ViewBag.SearchedUserName = user.FullName;
                        ViewBag.SearchedUserEmail = user.Email;
                    }
                }
                
                return View("MyHistory", donations);
            }
            catch
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải lịch sử hiến máu.";
                return RedirectToAction("MyHistory");
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
                return userId;
            return 0;
        }
    }
}