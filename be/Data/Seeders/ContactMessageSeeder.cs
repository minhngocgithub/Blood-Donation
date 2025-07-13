using System;
using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
{
    public static class ContactMessageSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.ContactMessages.Any())
            {
                var messages = new List<ContactMessage>
                {
                    new ContactMessage { FullName = "John Doe", Email = "john@example.com", Message = "How can I donate?", CreatedDate = DateTime.Now },                    
                };
                context.ContactMessages.AddRange(messages);
                context.SaveChanges();
            }
        }
    }
} 