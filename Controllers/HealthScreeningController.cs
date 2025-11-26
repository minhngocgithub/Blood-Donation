using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Utilities.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý sàng lọc sức khỏe (dành cho Hospital và Doctor)
    /// Xử lý: Sàng lọc sức khỏe người hiến máu, Cập nhật nhóm máu, Thống kê
    /// Quy trình: CheckedIn → Screening → Eligible/Ineligible
    /// Route: /screening/*
    /// </summary>
    [Route("screening")]
    public class HealthScreeningController : Controller
    {
        // Dependencies
        private readonly IHealthScreeningService _screeningService; // Service sàng lọc sức khỏe
        private readonly IDonationRegistrationService _registrationService; // Service đăng ký
        private readonly IUserService _userService; // Service người dùng
        private readonly IBloodTypeService _bloodTypeService; // Service nhóm máu
        
        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public HealthScreeningController(
            IHealthScreeningService screeningService,
            IDonationRegistrationService registrationService,
            IUserService userService,
            IBloodTypeService bloodTypeService)
        {
            _screeningService = screeningService;
            _registrationService = registrationService;
            _userService = userService;
            _bloodTypeService = bloodTypeService;
        }

        /// <summary>
        /// GET: /screening
        /// Hiển thị danh sách tất cả hồ sơ sàng lọc sức khỏe
        /// </summary>
        [HttpGet]
        [HospitalOrDoctor] // Chỉ Hospital hoặc Doctor được truy cập
        public async Task<IActionResult> Index()
        {
            var screenings = await _screeningService.GetAllScreeningsAsync();
            return View(screenings);
        }

        /// <summary>
        /// GET: /screening/pending
        /// Hiển thị danh sách đăng ký chờ sàng lọc sức khỏe
        /// Các đăng ký đã check-in nhưng chưa sàng lọc
        /// </summary>
        [HttpGet("pending")]
        [HospitalOrDoctor]
        public async Task<IActionResult> PendingScreenings()
        {
            var pendingRegistrations = await _screeningService.GetPendingScreeningsAsync();
            return View(pendingRegistrations);
        }

        /// <summary>
        /// GET: /screening/create/{registrationId}
        /// Hiển thị form tạo hồ sơ sàng lọc sức khỏe mới
        /// </summary>
        /// <param name="registrationId">ID đăng ký cần sàng lọc</param>
        [HttpGet("create/{registrationId}")]
        [HospitalOrDoctor]
        public async Task<IActionResult> Create(int registrationId)
        {
            // Lấy thông tin đăng ký
            var registration = await _registrationService.GetRegistrationByIdAsync(registrationId);
            if (registration == null)
            {
                return NotFound();
            }

            // Lấy thông tin người hiến máu
            var user = await _userService.GetUserByIdAsync(registration.UserId);
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            
            // Truyền dữ liệu sang View
            ViewBag.Registration = registration;
            ViewBag.User = user;
            ViewBag.BloodTypes = bloodTypes;

            return View();
        }

        /// <summary>
        /// POST: /screening/create/{registrationId}
        /// Xử lý tạo hồ sơ sàng lọc sức khỏe
        /// Bao gồm: Huyết áp, Nhịp tim, Nhiệt độ, Hemoglobin, Cân nặng
        /// </summary>
        /// <param name="registrationId">ID đăng ký</param>
        /// <param name="screeningDto">Dữ liệu sàng lọc sức khỏe</param>
        [HttpPost("create/{registrationId}")]
        [HospitalOrDoctor]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int registrationId, HealthScreeningDto screeningDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    screeningDto.RegistrationId = registrationId;
                    var createdScreening = await _screeningService.CreateScreeningAsync(screeningDto);
                    TempData["SuccessMessage"] = "Sàng lọc sức khỏe đã được tạo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // Nếu có lỗi, load lại dữ liệu
            var registration = await _registrationService.GetRegistrationByIdAsync(registrationId);
            if (registration == null)
            {
                return NotFound();
            }
            var user = await _userService.GetUserByIdAsync(registration.UserId);
            ViewBag.Registration = registration;
            ViewBag.User = user;

            return View(screeningDto);
        }

        [HttpGet("edit/{id}")]
        [HospitalOrDoctor]
        public async Task<IActionResult> Edit(int id)
        {
            var screening = await _screeningService.GetScreeningByIdAsync(id);
            if (screening == null)
            {
                return NotFound();
            }

            var registration = await _registrationService.GetRegistrationByIdAsync(screening.RegistrationId);
            if (registration == null)
            {
                return NotFound();
            }
            var user = await _userService.GetUserByIdAsync(registration.UserId);
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            
            ViewBag.Registration = registration;
            ViewBag.User = user;
            ViewBag.BloodTypes = bloodTypes;

            return View(screening);
        }

        [HttpPost("edit/{id}")]
        [HospitalOrDoctor]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HealthScreeningDto screeningDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _screeningService.UpdateScreeningAsync(id, screeningDto);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Sàng lọc sức khỏe đã được cập nhật thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể cập nhật sàng lọc sức khỏe.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var registration = await _registrationService.GetRegistrationByIdAsync(screeningDto.RegistrationId);
            if (registration == null)
            {
                return NotFound();
            }
            var user = await _userService.GetUserByIdAsync(registration.UserId);
            ViewBag.Registration = registration;
            ViewBag.User = user;

            return View(screeningDto);
        }

        [HttpGet("details/{id}")]
        [HospitalOrDoctor]
        public async Task<IActionResult> Details(int id)
        {
            var screening = await _screeningService.GetScreeningByIdAsync(id);
            if (screening == null)
            {
                return NotFound();
            }

            var registration = await _registrationService.GetRegistrationByIdAsync(screening.RegistrationId);
            if (registration == null)
            {
                return NotFound();
            }
            var user = await _userService.GetUserByIdAsync(registration.UserId);
            ViewBag.Registration = registration;
            ViewBag.User = user;

            return View(screening);
        }

        [HttpPost("checkin/{id}")]
        [HospitalOrDoctor]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id)
        {
            try
            {
                var success = await _screeningService.CheckInScreeningAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Check-in thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể thực hiện check-in.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("status/{id}")]
        [HospitalOrDoctor]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, bool isEligible)
        {
            try
            {
                var success = await _screeningService.UpdateScreeningStatusAsync(id, isEligible);
                if (success)
                {
                    TempData["SuccessMessage"] = "Trạng thái sàng lọc đã được cập nhật!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật trạng thái sàng lọc.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("update-blood-type/{userId}")]
        [HospitalOrDoctor]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBloodType(int userId, int bloodTypeId)
        {
            try
            {
                var success = await _userService.UpdateBloodTypeAsync(userId, bloodTypeId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Nhóm máu đã được cập nhật thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật nhóm máu.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /screening/update-blood-type/{userId}
        /// Form cập nhật nhóm máu cho người dùng
        /// Dùng khi phát hiện nhóm máu thực tế khác với thông tin đã lưu
        /// </summary>
        /// <param name="userId">ID người dùng cần cập nhật (0 = chưa chọn)</param>
        [HttpGet("update-blood-type/{userId?}")]
        [HospitalOrDoctor]
        public async Task<IActionResult> UpdateBloodTypeForm(int userId = 0)
        {
            // Nếu chưa chọn user, hiển thị trang tìm kiếm
            if (userId == 0)
            {
                return View("SelectUser");
            }

            try
            {
                Console.WriteLine($"Looking for user with ID: {userId}");
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {userId} not found");
                    TempData["ErrorMessage"] = $"Không tìm thấy người dùng với ID: {userId}";
                    return RedirectToAction("Index");
                }
                
                Console.WriteLine($"Found user: {user.FullName} (ID: {user.UserId})");

                // Lấy danh sách nhóm máu
                var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
                
                ViewBag.User = user;
                ViewBag.BloodTypes = bloodTypes;

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải thông tin người dùng: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// GET: /screening/search-user (AJAX)
        /// Tìm kiếm người dùng theo email hoặc số điện thoại
        /// Trả về JSON để hiển thị trong dropdown/autocomplete
        /// </summary>
        /// <param name="email">Email tìm kiếm</param>
        /// <param name="phone">Số điện thoại tìm kiếm</param>
        [HttpGet("search-user")]
        [HospitalOrDoctor]
        public async Task<IActionResult> SearchUser(string? email = null, string? phone = null)
        {
            try
            {
                var users = new List<UserDto>();
                
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userService.GetUserByEmailAsync(email);
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }
                else if (!string.IsNullOrEmpty(phone))
                {
                    var allUsers = await _userService.GetAllUsersAsync();
                    users = allUsers.Where(u => u.Phone == phone).ToList();
                }

                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// GET: /screening/get-all-users (AJAX)
        /// Lấy tất cả người dùng trong hệ thống
        /// Dùng cho trang SelectUser để hiển thị danh sách đầy đủ
        /// </summary>
        [HttpGet("get-all-users")]
        [HospitalOrDoctor]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        /// <summary>
        /// GET: /screening/statistics
        /// Thống kê sàng lọc sức khỏe
        /// Bao gồm: Tổng số, Đạt/Không đạt, Tỷ lệ...
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu lọc (tùy chọn)</param>
        /// <param name="endDate">Ngày kết thúc lọc (tùy chọn)</param>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin,Hospital,Doctor")]
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            var stats = await _screeningService.GetScreeningStatisticsAsync();
            
            // Lưu giá trị lọc để hiển thị trên view
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            
            return View(stats);
        }
    }
}