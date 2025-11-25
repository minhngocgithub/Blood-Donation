using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class LocationSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    new Location 
                    { 
                        LocationName = "Bệnh viện Trung ương",
                        Address = "123 Đường ABC, Quận 1, TP.HCM",
                        ContactPhone = "028-12345678",
                        Capacity = 100,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    },
                    new Location 
                    { 
                        LocationName = "Trung tâm Y tế Quận 3",
                        Address = "456 Đường XYZ, Quận 3, TP.HCM",
                        ContactPhone = "028-87654321",
                        Capacity = 75,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    },
                    new Location 
                    { 
                        LocationName = "Bệnh viện Đa khoa Quận 7",
                        Address = "789 Đường DEF, Quận 7, TP.HCM",
                        ContactPhone = "028-11223344",
                        Capacity = 50,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    },
                    new Location
        {
            LocationName = "Trung tâm Y tế Đại học Khoa học Tự nhiên",
            Address = "227 Nguyễn Văn Cừ, Quận 5, TP.HCM",
            ContactPhone = "028-33445566",
            Capacity = 120,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "Trung tâm Y tế Thành phố",
            Address = "86 Lý Thường Kiệt, Quận 10, TP.HCM",
            ContactPhone = "028-55667788",
            Capacity = 200,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "Công viên Gia Định - Điểm hiến máu",
            Address = "Công viên Gia Định, Gò Vấp, TP.HCM",
            ContactPhone = "028-99887766",
            Capacity = 80,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "CLB Người Trẻ Sống Đẹp",
            Address = "4 Phạm Ngọc Thạch, Quận 1, TP.HCM",
            ContactPhone = "028-44556677",
            Capacity = 60,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "Trung tâm Văn hóa Quận 5",
            Address = "105 Trần Hưng Đạo, Quận 5, TP.HCM",
            ContactPhone = "028-77889900",
            Capacity = 90,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "Nhà văn hóa Thanh niên",
            Address = "4A Phạm Ngọc Thạch, Quận 1, TP.HCM",
            ContactPhone = "028-55668899",
            Capacity = 110,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "Aeon Mall - Điểm Hiến máu Cộng đồng",
            Address = "30 Bờ Bao Tân Thắng, Tân Phú, TP.HCM",
            ContactPhone = "028-66778899",
            Capacity = 140,
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Location
        {
            LocationName = "Bệnh viện Quận 10",
            Address = "571 Sư Vạn Hạnh, Quận 10, TP.HCM",
            ContactPhone = "028-22334455",
            Capacity = 70,
            IsActive = true,
            CreatedDate = DateTime.Now
        }
                };
                context.Locations.AddRange(locations);
                context.SaveChanges();
            }
        }
    }
}
