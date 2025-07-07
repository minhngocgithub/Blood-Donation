using System.Collections.Generic;
using System.Linq;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class SettingSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Settings.Any())
            {
                var settings = new List<Setting>
                {
                    new Setting { SettingKey = "SiteName", SettingValue = "BloodLife" },
                    // Add more as needed
                };
                context.Settings.AddRange(settings);
                context.SaveChanges();
            }
        }
    }
} 