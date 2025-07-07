using System;
using System.Collections.Generic;
using System.Linq;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class DonationHistorySeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.DonationHistories.Any())
            {
                var histories = new List<DonationHistory>
                {
                    new DonationHistory { UserId = 1, DonationDate = DateTime.Now.AddMonths(-1), Volume = 450, BloodTypeId = 1, EventId = 1 },
                    // Add more as needed
                };
                context.DonationHistories.AddRange(histories);
                context.SaveChanges();
            }
        }
    }
} 