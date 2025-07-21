using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Blood_Donation_Website.Data;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly ApplicationDbContext _context;
        public AdminController(IUserService userService, IAccountService accountService, ApplicationDbContext context)
        {
            _userService = userService;
            _accountService = accountService;
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var users = await _userService.GetAllUsersAsync();
            var totalUsers = users.Count();
            var totalAdmins = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Admin);
            var totalDoctors = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Doctor);
            var totalStaff = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Staff);
            var totalHospitals = users.Count(u => u.RoleName == Blood_Donation_Website.Utilities.EnumMapper.RoleType.Hospital);
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalDoctors = totalDoctors;
            ViewBag.TotalStaff = totalStaff;
            ViewBag.TotalHospitals = totalHospitals;
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = roles;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(int userId, int roleId)
        {
            var success = await _userService.AssignRoleAsync(userId, roleId);
            if (success)
                return Json(new { success = true, message = "Cập nhật vai trò thành công!" });
            return Json(new { success = false, message = "Cập nhật vai trò thất bại!" });
        }
    }
}
