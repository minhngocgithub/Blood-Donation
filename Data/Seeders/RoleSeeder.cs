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
                new Role { RoleName = "Admin", Description = "System Administrator", CreatedDate = DateTime.Now },
                new Role { RoleName = "User", Description = "Regular User", CreatedDate = DateTime.Now },
                new Role { RoleName = "Volunteer", Description = "Volunteer", CreatedDate = DateTime.Now },
                new Role { RoleName = "Doctor", Description = "Medical Doctor", CreatedDate = DateTime.Now },
                new Role { RoleName = "Hospital", Description = "Hospital Representative", CreatedDate = DateTime.Now }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
