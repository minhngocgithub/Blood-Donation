using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class NewsCategorySeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.NewsCategories.Any())
            {
                var categories = new List<NewsCategory>
                {
                    new NewsCategory
                    {
                        CategoryName = "Tin tức sự kiện",
                        Description = "Thông tin về các sự kiện hiến máu sắp diễn ra và đã diễn ra"
                    },
                    new NewsCategory
                    {
                        CategoryName = "Kiến thức hiến máu",
                        Description = "Kiến thức, hướng dẫn về hiến máu và sức khỏe"
                    },
                    new NewsCategory
                    {
                        CategoryName = "Câu chuyện nhân văn",
                        Description = "Những câu chuyện cảm động về hiến máu và cứu người"
                    },
                    new NewsCategory
                    {
                        CategoryName = "Thông báo",
                        Description = "Thông báo chính thức từ hệ thống"
                    },
                    new NewsCategory
                    {
                        CategoryName = "Hỏi đáp",
                        Description = "Giải đáp thắc mắc về hiến máu"
                    }
                };

                context.NewsCategories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}
