using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class BloodTypeSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.BloodTypes.Any()) return;

            var bloodTypes = new List<BloodType>
            {
                new BloodType 
                { 
                    BloodTypeName = "A+", 
                    Description = "Nhóm máu A dương tính - Có thể hiến cho A+ và AB+, nhận từ A+, A-, O+, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "A-", 
                    Description = "Nhóm máu A âm tính - Có thể hiến cho A+, A-, AB+, AB-, nhận từ A-, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "B+", 
                    Description = "Nhóm máu B dương tính - Có thể hiến cho B+ và AB+, nhận từ B+, B-, O+, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "B-", 
                    Description = "Nhóm máu B âm tính - Có thể hiến cho B+, B-, AB+, AB-, nhận từ B-, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "AB+", 
                    Description = "Nhóm máu AB dương tính - Hiến huyết tương toàn cầu, nhận từ tất cả các nhóm máu" 
                },
                new BloodType 
                { 
                    BloodTypeName = "AB-", 
                    Description = "Nhóm máu AB âm tính - Có thể hiến cho AB+, AB-, nhận từ A-, B-, AB-, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "O+", 
                    Description = "Nhóm máu O dương tính - Có thể hiến cho A+, B+, AB+, O+, nhận từ O+, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "O-", 
                    Description = "Nhóm máu O âm tính - Hiến máu toàn cầu, có thể hiến cho tất cả các nhóm máu" 
                }
            };

            context.BloodTypes.AddRange(bloodTypes);
            context.SaveChanges();
        }
    }
}
