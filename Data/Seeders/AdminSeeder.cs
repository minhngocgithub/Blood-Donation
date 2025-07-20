using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Services.Utilities;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class AdminSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Users.Any(u => u.Email == "admin@blooddonation.com")) return;

            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == RoleType.Admin);
            if (adminRole == null) return;

            var bloodType = context.BloodTypes.FirstOrDefault(bt => bt.BloodTypeName == "O+");
            if (bloodType == null)
            {
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
                PasswordHash = PasswordHelper.HashPassword("Admin@123"),
                Phone = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Address = "Hanoi, Vietnam",
                BloodTypeId = bloodType.BloodTypeId,
                RoleId = adminRole.RoleId,
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
