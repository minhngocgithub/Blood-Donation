using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public class RoleSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new List<Role>
            {
                new Role { RoleName = "Admin", Description = "Quản trị viên hệ thống", CreatedDate = DateTime.Now },
                new Role { RoleName = "User", Description = "Người dùng thông thường", CreatedDate = DateTime.Now },
                new Role { RoleName = "Volunteer", Description = "Tình nguyện viên", CreatedDate = DateTime.Now }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
