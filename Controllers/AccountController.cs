using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Models.ViewModels.Account;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    /// <summ
    /// ary>
    /// Controller quản lý tài khoản người dùng
    /// Xử lý: Đăng nhập, Đăng ký, Đăng xuất, Đổi mật khẩu
    /// Route: /account/*
    /// </summary>
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        
        /// <summary>
        /// Constructor - Inject AccountService để xử lý logic nghiệp vụ
        /// </summary>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// GET: /account/login
        /// Hiển thị trang đăng nhập
        /// </summary>
        /// <param name="returnUrl">URL để chuyển hướng sau khi đăng nhập thành công</param>
        [HttpGet("login")]
        [AllowAnonymous] // Cho phép truy cập không cần đăng nhập
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập rồi thì chuyển về trang chủ
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            // Lưu returnUrl để chuyển hướng sau khi đăng nhập
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// POST: /account/login
        /// Xử lý đăng nhập - xác thực thông tin và tạo cookie
        /// </summary>
        /// <param name="model">Thông tin đăng nhập (Email, Password, RememberMe)</param>
        /// <param name="returnUrl">URL chuyển hướng sau khi đăng nhập</param>
        [HttpPost("login")]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF attack
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Kiểm tra validation
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Gọi service để xác thực thông tin đăng nhập
                var loginResult = await _accountService.LoginAsync(model);
                if (!loginResult)
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
                    return View(model);
                }

                // Lấy thông tin user sau khi đăng nhập thành công
                var user = await _accountService.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng nhập.");
                    return View(model);
                }

                // Tạo các Claims - thông tin định danh người dùng được lưu trong cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // ID người dùng
                    new Claim(ClaimTypes.Name, user.FullName), // Tên đầy đủ
                    new Claim(ClaimTypes.Email, user.Email), // Email
                    new Claim(ClaimTypes.Role, user.RoleName.ToString() ?? "User") // Vai trò (Admin, Doctor, Staff, User...)
                };

                // Tạo Identity từ Claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                // Cấu hình thuộc tính xác thực
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe, // Lưu cookie lâu dài nếu chọn "Nhớ đăng nhập"
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30) // 30 ngày hoặc 30 phút
                };

                // Tạo cookie xác thực và lưu vào HttpContext
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.");
                return View(model);
            }
        }
        
        /// <summary>
        /// GET: /account/register
        /// Hiển thị trang đăng ký tài khoản mới
        /// </summary>
        [HttpGet("register")]
        public IActionResult Register()
        {
            // Nếu đã đăng nhập rồi thì không cho đăng ký thêm
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        /// <summary>
        /// POST: /account/register
        /// Xử lý đăng ký tài khoản mới - tạo user trong database
        /// </summary>
        /// <param name="model">Thông tin đăng ký (Email, Password, FullName, Phone...)</param>
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Kiểm tra checkbox đồng ý điều khoản
            if (!model.AgreeToTerms)
            {
                ModelState.AddModelError("AgreeToTerms", "Bạn phải đồng ý với điều khoản sử dụng");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Kiểm tra email đã tồn tại chưa
                if (await _accountService.IsEmailExistsAsync(model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                var result = await _accountService.RegisterAsync(model);
                
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Đăng ký thất bại. Vui lòng thử lại.");
                    return View(model);
                }
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại.");
                return View(model);
            }
        }

        [HttpGet("logout")]
        public IActionResult LogoutConfirmation()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = User.Identity?.Name;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    await _accountService.LogoutAsync(userId);
                }
                
                HttpContext.Session.Clear();
                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                foreach (var cookie in Request.Cookies.Keys)
                {
                    if (cookie.StartsWith(".AspNetCore") || cookie.StartsWith("AspNetCore"))
                    {
                        Response.Cookies.Delete(cookie, new CookieOptions
                        {
                            Path = "/",
                            Secure = Request.IsHttps,
                            SameSite = SameSiteMode.Lax
                        });
                    }
                }
                
                Response.Cookies.Delete("BloodDonationAuth", new CookieOptions
                {
                    Path = "/",
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax
                });
                                
                TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công!";
                
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi đăng xuất. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("quick-logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickLogout()
        {
            try
            {
                var userEmail = User.Identity?.Name;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    await _accountService.LogoutAsync(userId);
                }
                
                HttpContext.Session.Clear();
                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                foreach (var cookie in Request.Cookies.Keys)
                {
                    if (cookie.StartsWith(".AspNetCore") || cookie.StartsWith("AspNetCore"))
                    {
                        Response.Cookies.Delete(cookie, new CookieOptions
                        {
                            Path = "/",
                            Secure = Request.IsHttps,
                            SameSite = SameSiteMode.Lax
                        });
                    }
                }
                
                Response.Cookies.Delete("BloodDonationAuth", new CookieOptions
                {
                    Path = "/",
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax
                });
                                
                if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Đăng xuất thành công!" });
                }
                
                TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công!";
                return RedirectToAction("Index", "Home");
            }
            catch
            {                
                if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi đăng xuất." });
                }
                
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi đăng xuất. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("ping")]
        public IActionResult Ping()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Json(new { success = true, timestamp = DateTime.UtcNow });
            }
            return Json(new { success = false });
        }

        [HttpPost("CheckEmailExists")]
        public async Task<IActionResult> CheckEmailExists([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Json(new { exists = false });
                }

                var exists = await _accountService.IsEmailExistsAsync(email);
                return Json(new { exists = exists });
            }
            catch
            {
                return Json(new { exists = false });
            }
        }

        /// <summary>
        /// GET: /account/changepassword
        /// Hiển thị trang đổi mật khẩu (yêu cầu đã đăng nhập)
        /// </summary>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// POST: /account/changepassword
        /// Xử lý đổi mật khẩu - xác thực mật khẩu cũ và cập nhật mật khẩu mới
        /// </summary>
        /// <param name="model">Mật khẩu hiện tại và mật khẩu mới</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Lấy UserId từ Claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Có lỗi xác thực người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Account");
            }
            
            // Gọi service để đổi mật khẩu
            var result = await _accountService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (result)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }
        }

        /// <summary>
        /// GET: /account/accessdenied
        /// Trang thông báo không có quyền truy cập
        /// </summary>
        /// <param name="returnUrl">URL người dùng đã cố truy cập</param>
        [HttpGet("AccessDenied")]
        [AllowAnonymous]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Helper method: Chuyển hướng an toàn đến URL local
        /// Tránh Open Redirect vulnerability
        /// </summary>
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}