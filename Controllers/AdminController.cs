using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Data;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản trị hệ thống (chỉ dành cho Admin)
    /// Xử lý: Dashboard thống kê, Quản lý người dùng, Phân quyền
    /// Route: /admin/*
    /// </summary>
    [Authorize(Roles = "Admin")] // Chỉ Admin mới truy cập được
    public class AdminController : Controller
    {
        // Dependencies
        private readonly IUserService _userService; // Service quản lý người dùng
        private readonly IAccountService _accountService; // Service quản lý tài khoản
        private readonly ApplicationDbContext _context; // Database context để truy vấn trực tiếp
        
        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public AdminController(IUserService userService, IAccountService accountService, ApplicationDbContext context)
        {
            _userService = userService;
            _accountService = accountService;
            _context = context;
        }

        /// <summary>
        /// GET: /Admin/Dashboard
        /// Trang dashboard hiển thị thống kê tổng quan hệ thống
        /// </summary>
        public async Task<IActionResult> Dashboard()
        {
            // Lấy tất cả người dùng từ database
            var users = await _userService.GetAllUsersAsync();
            
            // Thống kê số lượng theo từng vai trò
            var totalUsers = users.Count();
            var totalAdmins = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Admin);
            var totalDoctors = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Doctor);
            var totalStaff = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Staff);
            var totalHospitals = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Hospital);
            
            // Truyền dữ liệu thống kê sang View qua ViewBag
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalDoctors = totalDoctors;
            ViewBag.TotalStaff = totalStaff;
            ViewBag.TotalHospitals = totalHospitals;
            
            return View();
        }

        /// <summary>
        /// GET: /Admin/Users
        /// Hiển thị danh sách tất cả người dùng trong hệ thống
        /// </summary>
        public async Task<IActionResult> Users()
        {
            // Lấy danh sách người dùng và vai trò
            var users = await _userService.GetAllUsersAsync();
            var roles = await _context.Roles.ToListAsync();
            
            // Truyền danh sách vai trò để hiển thị dropdown
            ViewBag.Roles = roles;
            
            return View(users);
        }

        /// <summary>
        /// POST: /Admin/UpdateUserRole
        /// Cập nhật vai trò cho người dùng (AJAX request)
        /// </summary>
        /// <param name="userId">ID người dùng cần cập nhật</param>
        /// <param name="roleId">ID vai trò mới</param>
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(int userId, int roleId)
        {
            // Gọi service để gán vai trò mới
            var success = await _userService.AssignRoleAsync(userId, roleId);
            
            // Trả về JSON response cho AJAX
            if (success)
                return Json(new { success = true, message = "Cập nhật vai trò thành công!" });
            return Json(new { success = false, message = "Cập nhật vai trò thất bại!" });
        }
    }
}
