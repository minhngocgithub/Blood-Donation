using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class NewsCategorySeeder
    {
        public static void Seed(ApplicationDbContext context)
            {
                // Kiểm tra xem đã có dữ liệu chưa
                if (context.NewsCategories.Any()) return;

                var categories = new List<NewsCategory>
            {
                new NewsCategory
                {
                    CategoryName = "Tin tức sự kiện",
                    Description = "Thông tin về các sự kiện hiến máu sắp tới",
                    IsActive = true
                },
                new NewsCategory
                {
                    CategoryName = "Kiến thức y tế",
                    Description = "Các bài viết về kiến thức y tế liên quan đến hiến máu",
                    IsActive = true
                },
                new NewsCategory
                {
                    CategoryName = "Câu chuyện cảm động",
                    Description = "Những câu chuyện cảm động về hiến máu",
                    IsActive = true
                },
                new NewsCategory
                {
                    CategoryName = "Thông báo quan trọng",
                    Description = "Các thông báo quan trọng từ hệ thống",
                    IsActive = true
                },
                new NewsCategory
                {
                    CategoryName = "Hướng dẫn",
                    Description = "Hướng dẫn các quy trình hiến máu",
                    IsActive = true
                }
            };

                context.NewsCategories.AddRange(categories);
                context.SaveChanges();
            }
        }
}
