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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.LoginAsync(model);

            // If LoginAsync returns a bool, handle accordingly
            if (result)
            {
                // You may need to retrieve the user info here if needed for claims
                // For now, just redirect on success
                TempData["SuccessMessage"] = "Đăng nhập thành công!";
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin đăng nhập.");
            return View(model);
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
