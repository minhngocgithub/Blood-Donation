using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Utilities;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý quy trình hiến máu (dành cho Hospital, Doctor, Staff)
    /// Xử lý: Bắt đầu hiến máu, Theo dõi quá trình, Hoàn thành/Dừng hiến máu
    /// Quy trình: Eligible → Donating → Completed
    /// Route: /DonationManagement/*
    /// </summary>
    [Authorize(Roles = "Hospital,Doctor,Staff")] // Chỉ nhân viên y tế được truy cập
    public class DonationManagementController : Controller
    {
        // Dependencies - Các service cần thiết
        private readonly IDonationRegistrationService _donationRegistrationService; // Quản lý đăng ký
        private readonly IDonationHistoryService _donationHistoryService; // Quản lý lịch sử hiến máu
        private readonly IHealthScreeningService _healthScreeningService; // Quản lý sàng lọc sức khỏe
        private readonly IUserService _userService; // Quản lý người dùng
        private readonly ApplicationDbContext _context; // Database context để truy vấn trực tiếp

        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public DonationManagementController(
            IDonationRegistrationService donationRegistrationService,
            IDonationHistoryService donationHistoryService,
            IHealthScreeningService healthScreeningService,
            IUserService userService,
            ApplicationDbContext context)
        {
            _donationRegistrationService = donationRegistrationService;
            _donationHistoryService = donationHistoryService;
            _healthScreeningService = healthScreeningService;
            _userService = userService;
            _context = context;
        }

        /// <summary>
        /// GET: /DonationManagement/Index
        /// Hiển thị danh sách người đủ điều kiện để hiến máu
        /// Bao gồm các trạng thái: Eligible, Donating, Completed
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy danh sách đăng ký đủ điều kiện hiến máu
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User) // Thông tin người hiến
                    .Include(r => r.Event) // Thông tin sự kiện
                    .Include(r => r.Event.Location) // Thông tin địa điểm
                    .Where(r => r.Status == EnumMapper.RegistrationStatus.Eligible || 
                               r.Status == EnumMapper.RegistrationStatus.Donating ||
                               r.Status == EnumMapper.RegistrationStatus.Completed)
                    .OrderByDescending(r => r.RegistrationDate)
                    .Select(r => new DonationRegistrationDto
                    {
                        RegistrationId = r.RegistrationId,
                        RegistrationCode = $"REG{r.RegistrationId:D6}", // Mã đăng ký dạng REG000001
                        UserId = r.UserId,
                        EventId = r.EventId,
                        RegistrationDate = r.RegistrationDate,
                        Status = r.Status,
                        Notes = r.Notes,
                        FullName = r.User.FullName,
                        UserEmail = r.User.Email,
                        PhoneNumber = r.User.Phone,
                        EventName = r.Event.EventName,
                        EventDate = r.Event.EventDate,
                        LocationName = r.Event.Location!.LocationName,
                        IsEligible = r.Status == EnumMapper.RegistrationStatus.Eligible || 
                                   r.Status == EnumMapper.RegistrationStatus.Donating ||
                                   r.Status == EnumMapper.RegistrationStatus.Completed
                    })
                    .ToListAsync();

                return View(registrations);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách hiến máu: " + ex.Message;
                return View(new List<DonationRegistrationDto>());
            }
        }

        // GET: DonationManagement/Start/{id}
        public async Task<IActionResult> Start(int id)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .Include(r => r.Event.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == id);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đăng ký hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                if (registration.Status != EnumMapper.RegistrationStatus.Eligible)
                {
                    TempData["ErrorMessage"] = "Đăng ký này không đủ điều kiện để bắt đầu hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new DonationRegistrationDto
                {
                    RegistrationId = registration.RegistrationId,
                    RegistrationCode = $"REG{registration.RegistrationId:D6}",
                    UserId = registration.UserId,
                    EventId = registration.EventId,
                    RegistrationDate = registration.RegistrationDate,
                    Status = registration.Status,
                    Notes = registration.Notes,
                    FullName = registration.User.FullName,
                    UserEmail = registration.User.Email,
                    PhoneNumber = registration.User.Phone,
                    EventName = registration.Event.EventName,
                    EventDate = registration.Event.EventDate,
                    LocationName = registration.Event.Location!.LocationName,
                    IsEligible = true
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DonationManagement/Start
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartDonation(int id)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.RegistrationId == id);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đăng ký hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                if (registration.Status != EnumMapper.RegistrationStatus.Eligible)
                {
                    TempData["ErrorMessage"] = "Đăng ký này không đủ điều kiện để bắt đầu hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                // Cập nhật trạng thái thành "Đang hiến máu"
                registration.Status = EnumMapper.RegistrationStatus.Donating;
                registration.Notes = $"Bắt đầu hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã bắt đầu hiến máu cho {registration.User.FullName}.";
                return RedirectToAction(nameof(InProgress), new { id = registration.RegistrationId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi bắt đầu hiến máu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DonationManagement/InProgress/{id}
        public async Task<IActionResult> InProgress(int id)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .Include(r => r.Event.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == id);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đăng ký hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                if (registration.Status != EnumMapper.RegistrationStatus.Donating)
                {
                    TempData["ErrorMessage"] = "Đăng ký này không đang trong quá trình hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new DonationRegistrationDto
                {
                    RegistrationId = registration.RegistrationId,
                    RegistrationCode = $"REG{registration.RegistrationId:D6}",
                    UserId = registration.UserId,
                    EventId = registration.EventId,
                    RegistrationDate = registration.RegistrationDate,
                    Status = registration.Status,
                    Notes = registration.Notes,
                    FullName = registration.User.FullName,
                    UserEmail = registration.User.Email,
                    PhoneNumber = registration.User.Phone,
                    EventName = registration.Event.EventName,
                    EventDate = registration.Event.EventDate,
                    LocationName = registration.Event.Location!.LocationName,
                    IsEligible = true
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /DonationManagement/Complete
        /// Hoàn thành quy trình hiến máu - Tạo bản ghi lịch sử và hủy các đăng ký khác
        /// </summary>
        /// <param name="id">ID đăng ký</param>
        /// <param name="notes">Ghi chú (tùy chọn)</param>
        /// <param name="volume">Thể tích máu hiến (ml) - Mặc định: 350ml, Khoảng: 200-500ml</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, string notes = "", int volume = 350)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .Include(r => r.Event.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == id);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đăng ký hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                // Kiểm tra trạng thái phải là "Đang hiến máu"
                if (registration.Status != EnumMapper.RegistrationStatus.Donating)
                {
                    TempData["ErrorMessage"] = "Đăng ký này không đang trong quá trình hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                // Validate thể tích máu (200-500ml)
                if (volume < 200 || volume > 500)
                {
                    TempData["ErrorMessage"] = "Thể tích máu không hợp lệ. Vui lòng nhập giá trị từ 200ml đến 500ml.";
                    return RedirectToAction(nameof(InProgress), new { id });
                }

                // Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Bước 1: Cập nhật trạng thái đăng ký thành "Hoàn thành"
                    registration.Status = EnumMapper.RegistrationStatus.Completed;
                    registration.Notes = string.IsNullOrEmpty(notes) 
                        ? $"Hoàn tất hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
                        : $"Hoàn tất hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss}. Ghi chú: {notes}";

                    // Bước 2: Tạo record trong DonationHistory (lưu lịch sử hiến máu)
                    var donationHistory = new DonationHistory
                    {
                        UserId = registration.UserId,
                        EventId = registration.EventId,
                        RegistrationId = registration.RegistrationId,
                        DonationDate = DateTime.Now,
                        Status = EnumMapper.DonationStatus.Completed,
                        Notes = registration.Notes,
                        BloodTypeId = registration.User.BloodTypeId ?? 1, // Mặc định BloodTypeId = 1 nếu null
                        Volume = volume // Thể tích máu hiến
                    };

                    _context.DonationHistories.Add(donationHistory);
                    await _context.SaveChangesAsync();

                    // Bước 3: Hủy các đăng ký còn lại của user (trừ đăng ký này)
                    // Lý do: Người hiến máu cần nghỉ 90 ngày trước khi hiến lần tiếp theo
                    await _donationRegistrationService.CancelAllActiveRegistrationsExceptAsync(
                        registration.UserId,
                        registration.RegistrationId,
                        EnumMapper.DisqualificationReason.RecentDonation
                    );

                    // Commit transaction
                    await transaction.CommitAsync();

                    // Lưu DonationHistoryId để hiển thị trong View
                    ViewBag.DonationHistoryId = donationHistory.DonationId;
                    ViewBag.DonationNotes = notes;

                    var dto = new DonationRegistrationDto
                    {
                        RegistrationId = registration.RegistrationId,
                        RegistrationCode = $"REG{registration.RegistrationId:D6}",
                        UserId = registration.UserId,
                        EventId = registration.EventId,
                        RegistrationDate = registration.RegistrationDate,
                        Status = registration.Status,
                        Notes = registration.Notes,
                        FullName = registration.User.FullName,
                        UserEmail = registration.User.Email,
                        PhoneNumber = registration.User.Phone,
                        EventName = registration.Event.EventName,
                        EventDate = registration.Event.EventDate,
                        LocationName = registration.Event.Location!.LocationName,
                        IsEligible = true
                    };

                    TempData["SuccessMessage"] = $"Hoàn tất hiến máu thành công cho {registration.User.FullName}.";
                    return View("Complete", dto);
                }
                catch
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi hoàn tất hiến máu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DonationManagement/Stop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stop(int id, string notes = "")
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.RegistrationId == id);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đăng ký hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                if (registration.Status != EnumMapper.RegistrationStatus.Donating)
                {
                    TempData["ErrorMessage"] = "Đăng ký này không đang trong quá trình hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                // Cập nhật trạng thái thành "Đã hủy"
                registration.Status = EnumMapper.RegistrationStatus.Cancelled;
                registration.Notes = string.IsNullOrEmpty(notes)
                    ? $"Dừng hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
                    : $"Dừng hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss}. Lý do: {notes}";

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã dừng hiến máu cho {registration.User.FullName}.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi dừng hiến máu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DonationManagement/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .Include(r => r.Event.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == id);

                if (registration == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đăng ký hiến máu.";
                    return RedirectToAction(nameof(Index));
                }

                // Lấy thông tin sàng lọc sức khỏe
                var healthScreening = await _context.HealthScreenings
                    .Where(h => h.RegistrationId == registration.RegistrationId)
                    .OrderByDescending(h => h.ScreeningDate)
                    .FirstOrDefaultAsync();

                if (healthScreening != null)
                {
                    ViewBag.HealthScreening = healthScreening;
                }

                // Lấy DonationHistoryId nếu đã hoàn thành
                if (registration.Status == EnumMapper.RegistrationStatus.Completed)
                {
                    var donationHistory = await _context.DonationHistories
                        .Where(d => d.UserId == registration.UserId && d.EventId == registration.EventId)
                        .OrderByDescending(d => d.DonationDate)
                        .FirstOrDefaultAsync();

                    if (donationHistory != null)
                    {
                        ViewBag.DonationHistoryId = donationHistory.DonationId;
                    }
                }

                var dto = new DonationRegistrationDto
                {
                    RegistrationId = registration.RegistrationId,
                    RegistrationCode = $"REG{registration.RegistrationId:D6}",
                    UserId = registration.UserId,
                    EventId = registration.EventId,
                    RegistrationDate = registration.RegistrationDate,
                    Status = registration.Status,
                    Notes = registration.Notes,
                    FullName = registration.User.FullName,
                    UserEmail = registration.User.Email,
                    PhoneNumber = registration.User.Phone,
                    EventName = registration.Event.EventName,
                    EventDate = registration.Event.EventDate,
                    LocationName = registration.Event.Location!.LocationName,
                    IsEligible = registration.Status == EnumMapper.RegistrationStatus.Eligible || 
                               registration.Status == EnumMapper.RegistrationStatus.Donating ||
                               registration.Status == EnumMapper.RegistrationStatus.Completed
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method để tạo mã hiến máu
        private string GenerateDonationCode()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var random = new Random();
            var randomPart = random.Next(1000, 9999);
            return $"DM{timestamp}{randomPart}";
        }
    }
} 