using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
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
                };
                context.Settings.AddRange(settings);
                context.SaveChanges();
            }
        }
    }
} 