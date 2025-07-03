using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public class BloodTypeSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Kiểm tra xem đã có dữ liệu chưa
            if (context.BloodTypes.Any()) return;

            var bloodTypes = new List<BloodType>
            {
                new BloodType { BloodTypeName = "A+", Description = "Nhóm máu A Rh dương", CanDonateTo = "A+, AB+", CanReceiveFrom = "A+, A-, O+, O-" },
                new BloodType { BloodTypeName = "A-", Description = "Nhóm máu A Rh âm", CanDonateTo = "A+, A-, AB+, AB-", CanReceiveFrom = "A-, O-" },
                new BloodType { BloodTypeName = "B+", Description = "Nhóm máu B Rh dương", CanDonateTo = "B+, AB+", CanReceiveFrom = "B+, B-, O+, O-" },
                new BloodType { BloodTypeName = "B-", Description = "Nhóm máu B Rh âm", CanDonateTo = "B+, B-, AB+, AB-", CanReceiveFrom = "B-, O-" },
                new BloodType { BloodTypeName = "AB+", Description = "Nhóm máu AB Rh dương", CanDonateTo = "AB+", CanReceiveFrom = "Tất cả" },
                new BloodType { BloodTypeName = "AB-", Description = "Nhóm máu AB Rh âm", CanDonateTo = "AB+, AB-", CanReceiveFrom = "A-, B-, AB-, O-" },
                new BloodType { BloodTypeName = "O+", Description = "Nhóm máu O Rh dương", CanDonateTo = "A+, B+, AB+, O+", CanReceiveFrom = "O+, O-" },
                new BloodType { BloodTypeName = "O-", Description = "Nhóm máu O Rh âm", CanDonateTo = "Tất cả", CanReceiveFrom = "O-" }
            };

            context.BloodTypes.AddRange(bloodTypes);
            context.SaveChanges();
        }
    }
}
