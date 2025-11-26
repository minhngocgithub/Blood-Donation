using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý người dùng (dành cho Admin, Hospital, Doctor)
    /// Xử lý: Xem danh sách người hiến máu, Thống kê số lần hiến máu
    /// Route: /user/*
    /// </summary>
    [Authorize(Roles = "Admin,Hospital,Doctor")] // Chỉ Admin, Hospital, Doctor được truy cập
    public class UserController : Controller
    {
        private readonly IUserService _userService; // Service quản lý người dùng
        
        /// <summary>
        /// Constructor - Inject UserService
        /// </summary>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GET: /User/Index
        /// Hiển thị danh sách người hiến máu (đã từng hiến máu ít nhất 1 lần)
        /// Bao gồm thông tin: Tổng số lần hiến, Ngày hiến gần nhất
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả người dùng từ hệ thống
            var allUsers = await _userService.GetAllUsersAsync();
            
            // Danh sách người hiến máu (có ít nhất 1 lần hiến)
            var donors = new List<Blood_Donation_Website.Models.DTOs.UserDto>();
            
            // Lọc chỉ những người đã từng hiến máu
            foreach (var user in allUsers)
            {
                // Lấy tổng số lần hiến máu của user
                var total = await _userService.GetUserTotalDonationsAsync(user.UserId);
                
                if (total > 0) // Chỉ thêm vào danh sách nếu đã hiến máu
                {
                    user.TotalDonations = total;
                    // Lấy ngày hiến máu gần nhất
                    user.LastDonationDate = await _userService.GetUserLastDonationDateAsync(user.UserId);
                    donors.Add(user);
                }
            }
            
            // Trả về view với danh sách người hiến máu
            return View(donors);
        }
    }
}