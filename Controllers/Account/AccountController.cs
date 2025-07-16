using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Models.ViewModels.Account;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginResult = await _accountService.LoginAsync(model);
                if (!loginResult)
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
                    return View(model);
                }

                var user = await _accountService.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng nhập.");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleName ?? "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.");
                return View(model);
            }
        }
        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Manual validation for AgreeToTerms
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
                // Check if email already exists
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return Json(new { exists = false });
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Lấy userId đúng từ Claims
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _accountService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (result)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return View(model);
            }
            else
            {
                // Thông báo lỗi rõ ràng cho SweetAlert
                ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }
        }

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