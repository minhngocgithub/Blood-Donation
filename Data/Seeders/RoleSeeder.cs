using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class RoleSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new List<Role>
            {
                new Role { RoleName = "Admin", Description = "Quản trị viên", CreatedDate = DateTime.Now },
                new Role { RoleName = "User", Description = "Người dùng", CreatedDate = DateTime.Now },
                new Role { RoleName = "Hospital", Description = "Bệnh viện", CreatedDate = DateTime.Now },
                new Role { RoleName = "Doctor", Description = "Bác sĩ", CreatedDate = DateTime.Now },
                new Role { RoleName = "Staff", Description = "Nhân viên", CreatedDate = DateTime.Now }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
