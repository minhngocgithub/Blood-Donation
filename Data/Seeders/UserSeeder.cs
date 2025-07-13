using System;
using System.Collections.Generic;
using System.Linq;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Services.Utilities;

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
                        Email = "john.doe@example.com", 
                        PasswordHash = PasswordHelper.HashPassword("Password123!"), 
                        FullName = "John Doe",
                        Phone = "555-0101",
                        Address = "123 Main St, City, State 12345",
                        DateOfBirth = new DateTime(1990, 5, 15),
                        Gender = "Male",
                        BloodTypeId = 1, // A+
                        RoleId = 2, // Regular user
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = DateTime.Now.AddDays(-90)
                    },
                    new User 
                    { 
                        Username = "jane_smith", 
                        Email = "jane.smith@example.com", 
                        PasswordHash = PasswordHelper.HashPassword("Password123!"), 
                        FullName = "Jane Smith",
                        Phone = "555-0102",
                        Address = "456 Oak Ave, City, State 12346",
                        DateOfBirth = new DateTime(1985, 8, 22),
                        Gender = "Female",
                        BloodTypeId = 8, // O-
                        RoleId = 2, // Regular user
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = DateTime.Now.AddDays(-120)
                    },
                    new User 
                    { 
                        Username = "mike_wilson", 
                        Email = "mike.wilson@example.com", 
                        PasswordHash = "AQAAAAEAACcQAAAAEHashed_Password_Here_789", 
                        FullName = "Michael Wilson",
                        Phone = "555-0103",
                        Address = "789 Pine Rd, City, State 12347",
                        DateOfBirth = new DateTime(1992, 12, 3),
                        Gender = "Male",
                        BloodTypeId = 3, // B+
                        RoleId = 2, // Regular user
                        IsActive = true,
                        EmailVerified = true,
                        LastDonationDate = null // First-time donor
                    },
                    new User 
                    { 
                        Username = "sarah_johnson", 
                        Email = "sarah.johnson@example.com", 
                        PasswordHash = "AQAAAAEAACcQAAAAEHashed_Password_Here_101", 
                        FullName = "Sarah Johnson",
                        Phone = "555-0104",
                        Address = "321 Elm St, City, State 12348",
                        DateOfBirth = new DateTime(1988, 3, 17),
                        Gender = "Female",
                        BloodTypeId = 5, // AB+
                        RoleId = 2, // Regular user
                        IsActive = true,
                        EmailVerified = false, // Email not verified yet
                        LastDonationDate = DateTime.Now.AddDays(-45)
                    },
                    new User 
                    { 
                        Username = "david_brown", 
                        Email = "david.brown@example.com", 
                        PasswordHash = "AQAAAAEAACcQAAAAEHashed_Password_Here_202", 
                        FullName = "David Brown",
                        Phone = "555-0105",
                        Address = "654 Maple Dr, City, State 12349",
                        DateOfBirth = new DateTime(1995, 11, 28),
                        Gender = "Male",
                        BloodTypeId = 7, // O+
                        RoleId = 2, // Regular user
                        IsActive = false, // Temporarily inactive
                        EmailVerified = true,
                        LastDonationDate = DateTime.Now.AddDays(-200)
                    }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
} 