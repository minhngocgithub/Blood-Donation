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
    [Authorize(Roles = "Hospital,Doctor,Staff")]
    public class DonationManagementController : Controller
    {
        private readonly IDonationRegistrationService _donationRegistrationService;
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IHealthScreeningService _healthScreeningService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

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

        // GET: DonationManagement/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy danh sách đăng ký đủ điều kiện hiến máu
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .Include(r => r.Event.Location)
                    .Where(r => r.Status == EnumMapper.RegistrationStatus.Eligible || 
                               r.Status == EnumMapper.RegistrationStatus.Donating ||
                               r.Status == EnumMapper.RegistrationStatus.Completed)
                    .OrderByDescending(r => r.RegistrationDate)
                    .Select(r => new DonationRegistrationDto
                    {
                        RegistrationId = r.RegistrationId,
                        RegistrationCode = $"REG{r.RegistrationId:D6}",
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

        // GET: DonationManagement/Complete
        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .ThenInclude(u => u.BloodType)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
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
                    BloodTypeName = registration.User.BloodType?.BloodTypeName,
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

        // POST: DonationManagement/CompleteDonation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteDonation(int id, int volume, string notes = "")
        {
            try
            {
                // Validate volume
                if (volume < 200 || volume > 500)
                {
                    TempData["ErrorMessage"] = "Lượng máu hiến phải từ 200ml đến 500ml.";
                    return RedirectToAction(nameof(Complete), new { id });
                }

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

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Cập nhật trạng thái đăng ký thành "Hoàn thành"
                    registration.Status = EnumMapper.RegistrationStatus.Completed;
                    registration.Notes = string.IsNullOrEmpty(notes) 
                        ? $"Hoàn tất hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} - Lượng: {volume}ml"
                        : $"Hoàn tất hiến máu lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} - Lượng: {volume}ml. Ghi chú: {notes}";

                    // Tạo record trong DonationHistory
                    var donationHistory = new DonationHistory
                    {
                        UserId = registration.UserId,
                        EventId = registration.EventId,
                        RegistrationId = registration.RegistrationId,
                        DonationDate = DateTime.Now,
                        Volume = volume,
                        Status = EnumMapper.DonationStatus.Completed,
                        Notes = registration.Notes,
                        BloodTypeId = registration.User.BloodTypeId ?? 1,
                        CertificateIssued = false
                    };

                    _context.DonationHistories.Add(donationHistory);
                    await _context.SaveChangesAsync();

                    // Huỷ các đăng ký còn lại của user (trừ đăng ký này)
                    await _donationRegistrationService.CancelAllActiveRegistrationsExceptAsync(
                        registration.UserId,
                        registration.RegistrationId,
                        EnumMapper.DisqualificationReason.RecentDonation
                    );

                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = $"Hoàn tất hiến máu thành công cho {registration.User.FullName}. Lượng máu: {volume}ml.";
                    return RedirectToAction(nameof(Details), new { id = registration.RegistrationId });
                }
                catch
                {
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