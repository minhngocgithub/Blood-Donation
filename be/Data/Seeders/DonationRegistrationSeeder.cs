using System;
using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
{
    public static class DonationRegistrationSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.DonationRegistrations.Any())
            {
                var registrations = new List<DonationRegistration>
                {
                    new DonationRegistration 
                    { 
                        UserId = 1, 
                        EventId = 1, 
                        RegistrationDate = DateTime.Now.AddDays(-5),
                        Status = "Completed",
                        IsEligible = true
                    },
                    new DonationRegistration 
                    { 
                        UserId = 2, 
                        EventId = 1, 
                        RegistrationDate = DateTime.Now.AddDays(-3),
                        Status = "Completed",
                        IsEligible = true
                    },
                    new DonationRegistration 
                    { 
                        UserId = 3, 
                        EventId = 2, 
                        RegistrationDate = DateTime.Now.AddDays(-2),
                        Status = "Cancelled",
                        IsEligible = false,
                        CancellationReason = "Failed health screening"
                    },
                    new DonationRegistration 
                    { 
                        UserId = 4, 
                        EventId = 2, 
                        RegistrationDate = DateTime.Now.AddDays(-1),
                        Status = "Completed",
                        IsEligible = true
                    },
                    new DonationRegistration 
                    { 
                        UserId = 5, 
                        EventId = 3, 
                        RegistrationDate = DateTime.Now,
                        Status = "Cancelled",
                        IsEligible = false,
                        CancellationReason = "Failed health screening"
                    }
                };
                context.DonationRegistrations.AddRange(registrations);
                context.SaveChanges();
            }
        }
    }
} 