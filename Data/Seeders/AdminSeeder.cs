using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Services.Utilities;
using System.Text;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class AdminSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Check if admin already exists
            if (context.Users.Any(u => u.Email == "admin@blooddonation.com")) return;

            // Get Admin role (RoleId = 1)
            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            if (adminRole == null) return;

            // Get BloodType O+ (should already exist from BloodTypeSeeder)
            var bloodType = context.BloodTypes.FirstOrDefault(bt => bt.BloodTypeName == "O+");
            if (bloodType == null)
            {
                // This shouldn't happen if BloodTypeSeeder ran first, but just in case
                bloodType = new BloodType
                {
                    BloodTypeName = "O+",
                    Description = "Blood type O positive - Can donate to A+, B+, AB+, O+, can receive from O+, O-"
                };
                context.BloodTypes.Add(bloodType);
                context.SaveChanges();
            }

            var adminUser = new User
            {
                Username = "admin",
                FullName = "System Administrator",
                Email = "admin@blooddonation.com",
                PasswordHash = PasswordHelper.HashPassword("Admin@123"), // Default password
                Phone = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Address = "Hanoi, Vietnam",
                BloodTypeId = bloodType.BloodTypeId,
                RoleId = adminRole.RoleId, // This will be 1 (Admin)
                IsActive = true,
                EmailVerified = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}
