using Blood_Donation_Website.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers.Doctor
{
    [Authorize(Roles = "Doctor")]
    [DoctorOnly]
    [Route("doctor/screening")]
    public class HealthScreeningController : Controller
    {
        private readonly IHealthScreeningService _screeningService;
        private readonly IDonationRegistrationService _registrationService;
        private readonly IUserService _userService;

        public HealthScreeningController(
            IHealthScreeningService screeningService,
            IDonationRegistrationService registrationService,
            IUserService userService)
        {
            _screeningService = screeningService;
            _registrationService = registrationService;
            _userService = userService;
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
            ViewBag.Registration = registration;
            ViewBag.User = user;

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
            var user = await _userService.GetUserByIdAsync(registration.UserId);
            ViewBag.Registration = registration;
            ViewBag.User = user;

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
    }
} 