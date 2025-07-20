using Blood_Donation_Website.Models.Entities;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class RoleSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new List<Role>
            {
                new Role { RoleName = RoleType.Admin, Description = "Quản trị viên", CreatedDate = DateTime.Now },
                new Role { RoleName = RoleType.User, Description = "Người dùng", CreatedDate = DateTime.Now },
                new Role { RoleName = RoleType.Hospital, Description = "Bệnh viện", CreatedDate = DateTime.Now },
                new Role { RoleName = RoleType.Doctor, Description = "Bác sĩ", CreatedDate = DateTime.Now },
                new Role { RoleName = RoleType.Staff, Description = "Nhân viên", CreatedDate = DateTime.Now }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
