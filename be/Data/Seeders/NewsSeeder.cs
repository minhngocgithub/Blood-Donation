using System;
using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
{
    public static class NewsSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.News.Any())
            {
                var news = new List<News>
                {
                    new News { Title = "Blood Drive Success", Content = "Our recent blood drive was a huge success!", CategoryId = 1, CreatedDate = DateTime.Now },
                };
                context.News.AddRange(news);
                context.SaveChanges();
            }
        }
    }
} 