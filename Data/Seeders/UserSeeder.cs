using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Services.Utilities;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class UserSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.RoleId == 2))
            {
                var users = new List<User>
                {
                    new User 
                    { 
                        Username = "john_doe", 
                        Email = "user@example.com", 
                        PasswordHash = PasswordHelper.HashPassword("Admin@123"), 
                        FullName = "John Doe",
                        Phone = "555-0101",
                        Address = "123 Main St, City, State 12345",
                        DateOfBirth = new DateTime(1990, 5, 15),
                        Gender = Gender.Male,
                        BloodTypeId = 1,
                        RoleId = 2,
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = null
                    },
                    new User 
                    { 
                        Username = "jane_smith", 
                        Email = "hospital@example.com", 
                        PasswordHash = PasswordHelper.HashPassword("Admin@123"), 
                        FullName = "Jane Smith",
                        Phone = "555-0102",
                        Address = "456 Oak Ave, City, State 12346",
                        DateOfBirth = new DateTime(1985, 8, 22),
                        Gender = Gender.Female,
                        BloodTypeId = 8,
                        RoleId = 3,
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = null
                    },
                    new User 
                    { 
                        Username = "mike_wilson", 
                        Email = "doctor@example.com", 
                        PasswordHash = PasswordHelper.HashPassword("Admin@123"), 
                        FullName = "Michael Wilson",
                        Phone = "555-0103",
                        Address = "789 Pine Rd, City, State 12347",
                        DateOfBirth = new DateTime(1992, 12, 3),
                        Gender = Gender.Male,
                        BloodTypeId = 3,
                        RoleId = 4,
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = null
                    },
                    new User 
                    { 
                        Username = "sarah_johnson", 
                        Email = "staff@example.com", 
                        PasswordHash = PasswordHelper.HashPassword("Admin@123"), 
                        FullName = "Sarah Johnson",
                        Phone = "555-0104",
                        Address = "321 Elm St, City, State 12348",
                        DateOfBirth = new DateTime(1988, 3, 17),
                        Gender = Gender.Female,
                        BloodTypeId = 5,
                        RoleId = 5,
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = null
                    }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
} 