using Blood_Donation_Website.Models.Entities;
using System.Text;

namespace Blood_Donation_Website.Data.Seeders
{
    public class AdminUserSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Kiểm tra xem đã có admin chưa
            if (context.Users.Any(u => u.Email == "admin@blooddonation.com")) return;

            // Lấy role Admin (RoleId = 1)
            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            if (adminRole == null) return;

            var adminUser = new User
            {
                Username = "admin",
                FullName = "Administrator",
                Email = "admin@blooddonation.com",
                PasswordHash = HashPassword("Admin@123"), // Mật khẩu mặc định
                Phone = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Nam",
                Address = "Hà Nội, Việt Nam",
                BloodType = "O+",
                RoleId = adminRole.Id,
                IsActive = true,
                EmailVerified = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
