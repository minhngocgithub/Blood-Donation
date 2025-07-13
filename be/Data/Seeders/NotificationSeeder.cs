using System;
using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
{
    public static class NotificationSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Notifications.Any())
            {
                var notifications = new List<Notification>
                {
                    new Notification { Message = "Welcome to BloodLife!", UserId = 1, CreatedDate = DateTime.Now },
                };
                context.Notifications.AddRange(notifications);
                context.SaveChanges();
            }
        }
    }
} 