using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blood_Donation_Website.Controllers.Account
{
    [Authorize]
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        private async Task LoadBloodTypesAsync()
        {
            try
            {
                var bloodTypes = await _profileService.GetBloodTypesAsync();
                if (bloodTypes == null || !bloodTypes.Any())
                {
                    _logger.LogWarning("Không có dữ liệu nhóm máu trong hệ thống");
                    ViewBag.BloodTypes = new List<dynamic> { new { BloodTypeId = "", BloodTypeName = "Không có dữ liệu" } };
                }
                else
                {
                    ViewBag.BloodTypes = bloodTypes;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách nhóm máu: {Message}", ex.Message);
                ViewBag.BloodTypes = new SelectList(new[] { new { Id = "", Name = "Không thể tải dữ liệu" } }, "Id", "Name");
            }
        }

        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }

                var profile = await _profileService.GetProfileAsync(userId);
                if (profile == null)
                {
                    TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }

                await LoadBloodTypesAsync();
                return View("~/Views/Account/Profile/Index.cshtml", profile);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Lỗi định dạng userId không hợp lệ: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải thông tin cá nhân: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [Route("update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            try
            {
                _logger.LogInformation("Address data received: Province={Province}, District={District}, Ward={Ward}, AddressDetail={AddressDetail}",
                    model.Province, model.District, model.Ward, model.AddressDetail);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model validation failed: {Errors}", 
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    await LoadBloodTypesAsync();
                    return View("~/Views/Account/Profile/Index.cshtml", model);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }

                var result = await _profileService.UpdateProfileAsync(userId, model);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật thông tin. Vui lòng thử lại sau.";
                    await LoadBloodTypesAsync();
                    return View("~/Views/Account/Profile/Index.cshtml", model);
                }

                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Lỗi định dạng userId không hợp lệ: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Dữ liệu không hợp lệ: {Message}", ex.Message);
                ModelState.AddModelError("", $"Thông tin không hợp lệ: {ex.Message}");
                await LoadBloodTypesAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Lỗi thao tác cập nhật: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            return View("~/Views/Account/Profile/Index.cshtml", model);
        }
    }
}
