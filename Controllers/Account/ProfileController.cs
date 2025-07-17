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

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        private async Task LoadBloodTypesAsync()
        {
            try
            {
                var bloodTypes = await _profileService.GetBloodTypesAsync();
                if (bloodTypes == null || !bloodTypes.Any())
                {
                    ViewBag.BloodTypes = new List<dynamic> { new { BloodTypeId = "", BloodTypeName = "Không có dữ liệu" } };
                }
                else
                {
                    ViewBag.BloodTypes = bloodTypes;
                }
            }
            catch (Exception ex)
            {
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
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
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
                if (!ModelState.IsValid)
                {
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
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", $"Thông tin không hợp lệ: {ex.Message}");
                await LoadBloodTypesAsync();
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            return View("~/Views/Account/Profile/Index.cshtml", model);
        }
    }
}
