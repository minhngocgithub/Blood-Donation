using BloodDonationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BloodDonationAPI.Models.ViewModels.Account;
using System.Security.Claims;
using System.Text.Json;

namespace BloodDonationAPI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAccountService accountService, ILogger<AuthController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new { 
                        success = false, 
                        message = "Dữ liệu không hợp lệ", 
                        errors = errors 
                    });
                }

                var loginResult = await _accountService.LoginAsync(model);
                if (!loginResult)
                {
                    return Unauthorized(new { 
                        success = false, 
                        message = "Email hoặc mật khẩu không chính xác." 
                    });
                }

                var user = await _accountService.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized(new { 
                        success = false, 
                        message = "Có lỗi xảy ra khi đăng nhập." 
                    });
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                _logger.LogInformation("User {Email} logged in via API.", user.Email);

                return Ok(new { 
                    success = true, 
                    message = "Đăng nhập thành công!",
                    user = new {
                        id = user.UserId,
                        fullName = user.FullName,
                        email = user.Email,
                        role = user.Role?.RoleName ?? "User"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during API login for user {Email}", model.Email);
                return StatusCode(500, new { 
                    success = false, 
                    message = "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại." 
                });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                // Manual validation for AgreeToTerms
                if (!model.AgreeToTerms)
                {
                    ModelState.AddModelError("AgreeToTerms", "Bạn phải đồng ý với điều khoản sử dụng");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new { 
                        success = false, 
                        message = "Dữ liệu không hợp lệ", 
                        errors = errors 
                    });
                }

                // Check if email already exists
                if (await _accountService.IsEmailExistsAsync(model.Email))
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "Email này đã được sử dụng." 
                    });
                }

                var result = await _accountService.RegisterAsync(model);
                
                if (!result)
                {
                    _logger.LogWarning("API Registration failed for email: {Email}", model.Email);
                    return BadRequest(new { 
                        success = false, 
                        message = "Đăng ký thất bại. Vui lòng thử lại." 
                    });
                }

                _logger.LogInformation("User registered successfully via API: {Email}", model.Email);
                return Ok(new { 
                    success = true, 
                    message = "Đăng ký thành công! Vui lòng đăng nhập." 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during API registration for user {Email}", model.Email);
                return StatusCode(500, new { 
                    success = false, 
                    message = "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại." 
                });
            }
        }

        [HttpPost("logout")]
        [Authorize]
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
                
                _logger.LogInformation("User {Email} (ID: {UserId}) logged out via API. Cookies cleared.", userEmail, userId);
                
                return Ok(new { 
                    success = true, 
                    message = "Đăng xuất thành công!" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during API logout for user {Email}", User.Identity?.Name);
                return StatusCode(500, new { 
                    success = false, 
                    message = "Có lỗi xảy ra khi đăng xuất. Vui lòng thử lại." 
                });
            }
        }

        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                return Ok(new { 
                    success = true, 
                    isAuthenticated = true,
                    user = new {
                        id = userId,
                        email = userEmail,
                        fullName = userName,
                        role = userRole
                    }
                });
            }

            return Ok(new { 
                success = true, 
                isAuthenticated = false 
            });
        }

        [HttpPost("check-email")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailExists([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Ok(new { exists = false });
                }

                var exists = await _accountService.IsEmailExistsAsync(email);
                return Ok(new { exists = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists via API: {Email}", email);
                return StatusCode(500, new { 
                    success = false, 
                    message = "Có lỗi xảy ra khi kiểm tra email." 
                });
            }
        }
    }
} 