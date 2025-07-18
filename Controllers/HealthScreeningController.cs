using Blood_Donation_Website.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blood_Donation_Website.Controllers.Doctor
{
    [Authorize(Roles = "Doctor,Hospital")]
    [Route("screening")]
    public class HealthScreeningController : Controller
    {
        private readonly IHealthScreeningService _screeningService;
        private readonly IDonationRegistrationService _registrationService;
        private readonly IUserService _userService;
        private readonly IBloodTypeService _bloodTypeService;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var screenings = await _screeningService.GetAllScreeningsAsync();
            return View(screenings);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> PendingScreenings()
        {
            var pendingScreenings = await _screeningService.GetScreeningsByStatusAsync("Pending");
            return View(pendingScreenings);
        }

        [HttpGet("create/{registrationId}")]
        public async Task<IActionResult> Create(int registrationId)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(registrationId);
            if (registration == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(registration.UserId);
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            
            ViewBag.Registration = registration;
            ViewBag.User = user;
            ViewBag.BloodTypes = bloodTypes;

            return View();
        }

        [HttpPost("create/{registrationId}")]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                var success = await _screeningService.UpdateScreeningStatusAsync(id, status);
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

        [HttpGet("update-blood-type/{userId?}")]
        public async Task<IActionResult> UpdateBloodTypeForm(int userId = 0)
        {
            if (userId == 0)
            {
                // Hiển thị form tìm kiếm user
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

        [HttpGet("search-user")]
        public async Task<IActionResult> SearchUser(string email = null, string phone = null)
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
                    // Tìm theo phone - cần implement trong UserService
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

        [HttpGet("debug-users")]
        public async Task<IActionResult> DebugUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var userList = users.Select(u => new { 
                    u.UserId, 
                    u.FullName, 
                    u.Email, 
                    u.Phone,
                    BloodType = u.BloodTypeName ?? "Chưa cập nhật"
                }).ToList();
                
                return Json(new { 
                    totalUsers = userList.Count,
                    users = userList
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                Console.WriteLine($"Found {users.Count()} users in database");
                foreach (var user in users.Take(5))
                {
                    Console.WriteLine($"User ID: {user.UserId}, Name: {user.FullName}, Email: {user.Email}");
                }
                return Json(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }
    }
}