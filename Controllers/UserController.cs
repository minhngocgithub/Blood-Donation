using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blood_Donation_Website.Controllers
{
    [Authorize(Roles = "Admin,Hospital,Doctor")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _userService.GetAllUsersAsync();
            var donors = new List<Blood_Donation_Website.Models.DTOs.UserDto>();
            
            foreach (var user in allUsers)
            {
                var totalDonations = await _userService.GetUserTotalDonationsAsync(user.UserId);
                if (totalDonations > 0)
                {
                    // Cập nhật thông tin hiến máu vào DTO
                    user.TotalDonations = totalDonations;
                    user.LastDonationDate = await _userService.GetUserLastDonationDateAsync(user.UserId);
                    donors.Add(user);
                }
            }
            
            return View(donors);
        }
    }
} 