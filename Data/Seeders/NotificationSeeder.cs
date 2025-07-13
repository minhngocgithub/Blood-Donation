using System;
using System.Collections.Generic;
using System.Linq;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
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