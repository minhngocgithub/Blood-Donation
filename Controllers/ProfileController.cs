using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.ViewModels.Profile;
using Blood_Donation_Website.Utilities.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý hồ sơ cá nhân người dùng
    /// Xử lý: Xem thông tin cá nhân, Cập nhật thông tin
    /// Yêu cầu: Phải đăng nhập (AuthenticatedUser filter)
    /// Route: /profile
    /// </summary>
    [AuthenticatedUser] // Filter kiểm tra người dùng đã đăng nhập
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService; // Service quản lý hồ sơ người dùng
        
        /// <summary>
        /// Constructor - Inject ProfileService
        /// </summary>
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        /// <summary>
        /// Helper method: Load danh sách nhóm máu từ database
        /// Dùng để hiển thị dropdown select nhóm máu trong form
        /// </summary>
        private async Task LoadBloodTypesAsync()
        {
            try
            {
                // Lấy danh sách tất cả nhóm máu
                var bloodTypes = await _profileService.GetBloodTypesAsync();
                
                if (bloodTypes == null || !bloodTypes.Any())
                {
                    // Nếu không có dữ liệu, hiển thị thông báo
                    ViewBag.BloodTypes = new List<dynamic> { new { BloodTypeId = "", BloodTypeName = "Không có dữ liệu" } };
                }
                else
                {
                    ViewBag.BloodTypes = bloodTypes;
                }
            }
            catch
            {
                // Nếu có lỗi, hiển thị thông báo lỗi
                ViewBag.BloodTypes = new SelectList(new[] { new { Id = "", Name = "Không thể tải dữ liệu" } }, "Id", "Name");
            }
        }

        /// <summary>
        /// GET: /profile hoặc /profile/index
        /// Hiển thị trang hồ sơ cá nhân của người dùng đang đăng nhập
        /// </summary>
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy UserId từ Claims của người dùng đã đăng nhập
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }

                // Lấy thông tin hồ sơ từ database
                var profile = await _profileService.GetProfileAsync(userId);
                if (profile == null)
                {
                    TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }

                // Load danh sách nhóm máu để hiển thị trong dropdown
                await LoadBloodTypesAsync();
                
                // Trả về view với dữ liệu hồ sơ
                return View("~/Views/Profile/Index.cshtml", profile);
            }
            catch (FormatException)
            {
                // Lỗi format UserId (không thể parse thành int)
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch
            {
                // Lỗi không xác định
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
        }

        /// <summary>
        /// POST: /profile/update
        /// Cập nhật thông tin hồ sơ cá nhân
        /// </summary>
        /// <param name="model">Dữ liệu hồ sơ mới (họ tên, ngày sinh, giới tính, địa chỉ, nhóm máu...)</param>
        [HttpPost]
        [Route("update")]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF attack
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            try
            {
                // Kiểm tra validation
                if (!ModelState.IsValid)
                {
                    await LoadBloodTypesAsync();
                    return View("~/Views/Profile/Index.cshtml", model);
                }

                // Lấy UserId từ Claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Account");
                }

                // Gọi service để cập nhật hồ sơ
                var result = await _profileService.UpdateProfileAsync(userId, model);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật thông tin. Vui lòng thử lại sau.";
                    await LoadBloodTypesAsync();
                    return View("~/Views/Profile/Index.cshtml", model);
                }

                // Cập nhật thành công
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (FormatException)
            {
                // Lỗi format dữ liệu
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch (ArgumentException)
            {
                // Lỗi tham số không hợp lệ
                ModelState.AddModelError("", $"Thông tin không hợp lệ.");
                await LoadBloodTypesAsync();
            }
            catch (InvalidOperationException)
            {
                // Lỗi thao tác không hợp lệ
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            catch
            {
                // Lỗi không xác định
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }

            return View("~/Views/Profile/Index.cshtml", model);
        }
    }
}
