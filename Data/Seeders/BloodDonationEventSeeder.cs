using Blood_Donation_Website.Models.Entities;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class BloodDonationEventSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.BloodDonationEvents.Any())
            {
                // Get the locations by name
                var centralHospital = context.Locations.FirstOrDefault(l => l.LocationName == "Bệnh viện Trung ương");
                var district3Center = context.Locations.FirstOrDefault(l => l.LocationName == "Trung tâm Y tế Quận 3");
                var district7Hospital = context.Locations.FirstOrDefault(l => l.LocationName == "Bệnh viện Đa khoa Quận 7");
                var universityScienceCenter = context.Locations.FirstOrDefault(l => l.LocationName == "Trung tâm Y tế Đại học Khoa học Tự nhiên");
var cityHallCenter = context.Locations.FirstOrDefault(l => l.LocationName == "Trung tâm Y tế Thành phố");
var giaDinhPark = context.Locations.FirstOrDefault(l => l.LocationName == "Công viên Gia Định - Điểm hiến máu");
var youthClubCenter = context.Locations.FirstOrDefault(l => l.LocationName == "CLB Người Trẻ Sống Đẹp");
var district5CultureCenter = context.Locations.FirstOrDefault(l => l.LocationName == "Trung tâm Văn hóa Quận 5");
var familyCenter = context.Locations.FirstOrDefault(l => l.LocationName == "Trung tâm Sinh hoạt Gia đình");
var hiTechParkCenter = context.Locations.FirstOrDefault(l => l.LocationName == "Khu Công nghệ Cao - Trung tâm Y tế");
var youthCultureHouse = context.Locations.FirstOrDefault(l => l.LocationName == "Nhà văn hóa Thanh niên");
var aeonMall = context.Locations.FirstOrDefault(l => l.LocationName == "Aeon Mall - Điểm Hiến máu Cộng đồng");
var district10Hospital = context.Locations.FirstOrDefault(l => l.LocationName == "Bệnh viện Quận 10");

                var events = new List<BloodDonationEvent>
                {
                    new BloodDonationEvent 
                    { 
                        EventName = "Chương trình Hiến máu Mùa xuân", 
                        LocationId = centralHospital?.LocationId ?? 1, 
                        EventDate = DateTime.Now.AddDays(30), 
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0),
                        EventDescription = "Chương trình hiến máu mùa xuân thường niên nhằm giúp đỡ cộng đồng.",
                        MaxDonors = 100,
                        Status = EventStatus.Active,
                        RequiredBloodTypes = "All"
                    },
                    new BloodDonationEvent 
                    { 
                        EventName = "Hiến máu tại Hội chợ Sức khỏe Mùa hè", 
                        LocationId = district3Center?.LocationId ?? 2, 
                        EventDate = DateTime.Now.AddDays(60), 
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(16, 0, 0),
                        EventDescription = "Sự kiện hiến máu nằm trong khuôn khổ hội chợ sức khỏe mùa hè.",
                        MaxDonors = 75,
                        Status = EventStatus.Active,
                        RequiredBloodTypes = "O-, A-, B-, AB-"
                    },
                    new BloodDonationEvent 
                    { 
                        EventName = "Hiến máu Doanh nghiệp", 
                        LocationId = district7Hospital?.LocationId ?? 3, 
                        EventDate = DateTime.Now.AddDays(90), 
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(15, 0, 0),
                        EventDescription = "Sự kiện hiến máu do doanh nghiệp tài trợ.",
                        MaxDonors = 50,
                        Status = EventStatus.Active,
                        RequiredBloodTypes = "All"
                    },
                    new BloodDonationEvent 
    {
        EventName = "Ngày hội Hiến máu Toàn trường Đại học Khoa học Tự nhiên",
        LocationId = universityScienceCenter?.LocationId ?? 4,
        EventDate = DateTime.Now.AddDays(15),
        StartTime = new TimeSpan(8, 30, 0),
        EndTime = new TimeSpan(16, 30, 0),
        EventDescription = "Chương trình phối hợp cùng Đoàn trường nhằm nâng cao ý thức cộng đồng.",
        MaxDonors = 120,
        Status = EventStatus.Active,
        RequiredBloodTypes = "O+, A+"
    },
    new BloodDonationEvent 
    {
        EventName = "Tuần lễ Đỏ – Hiến máu cứu người",
        LocationId = cityHallCenter?.LocationId ?? 5,
        EventDate = DateTime.Now.AddDays(45),
        StartTime = new TimeSpan(9, 0, 0),
        EndTime = new TimeSpan(17, 0, 0),
        EventDescription = "Tuần lễ vận động hiến máu trên toàn thành phố.",
        MaxDonors = 200,
        Status = EventStatus.Active,
        RequiredBloodTypes = "All"
    },
    new BloodDonationEvent 
    {
        EventName = "Hiến máu Tình nguyện tại Công viên Gia Định",
        LocationId = giaDinhPark?.LocationId ?? 6,
        EventDate = DateTime.Now.AddDays(20),
        StartTime = new TimeSpan(7, 30, 0),
        EndTime = new TimeSpan(12, 0, 0),
        EventDescription = "Sự kiện ngoài trời thu hút đông đảo người dân tham gia.",
        MaxDonors = 80,
        Status = EventStatus.Active,
        RequiredBloodTypes = "A-, O-"
    },
    new BloodDonationEvent 
    {
        EventName = "Ngày hội Hiến máu của CLB Người Trẻ Sống Đẹp",
        LocationId = youthClubCenter?.LocationId ?? 7,
        EventDate = DateTime.Now.AddDays(10),
        StartTime = new TimeSpan(8, 0, 0),
        EndTime = new TimeSpan(14, 0, 0),
        EventDescription = "CLB phát động chương trình nhân văn vào đầu năm.",
        MaxDonors = 60,
        Status = EventStatus.Active,
        RequiredBloodTypes = "B+, O+"
    },
    new BloodDonationEvent 
    {
        EventName = "Hiến máu tại Trung tâm Văn hóa Quận 5",
        LocationId = district5CultureCenter?.LocationId ?? 8,
        EventDate = DateTime.Now.AddDays(75),
        StartTime = new TimeSpan(9, 0, 0),
        EndTime = new TimeSpan(13, 0, 0),
        EventDescription = "Chương trình phối hợp giữa UBND và Hội Chữ Thập Đỏ.",
        MaxDonors = 90,
        Status = EventStatus.Active,
        RequiredBloodTypes = "AB+, AB-"
    },
    new BloodDonationEvent 
    {
        EventName = "Ngày hội Giọt Hồng Gia Đình",
        LocationId = familyCenter?.LocationId ?? 9,
        EventDate = DateTime.Now.AddDays(120),
        StartTime = new TimeSpan(8, 0, 0),
        EndTime = new TimeSpan(17, 0, 0),
        EventDescription = "Chương trình nhằm tôn vinh nghĩa cử hiến máu giữa các thành viên gia đình.",
        MaxDonors = 150,
        Status = EventStatus.Active,
        RequiredBloodTypes = "All"
    },
    new BloodDonationEvent 
    {
        EventName = "Hiến máu tại Khu Công nghệ Cao",
        LocationId = hiTechParkCenter?.LocationId ?? 10,
        EventDate = DateTime.Now.AddDays(35),
        StartTime = new TimeSpan(8, 0, 0),
        EndTime = new TimeSpan(15, 30, 0),
        EventDescription = "Sự kiện dành cho kỹ sư và nhân viên các doanh nghiệp công nghệ.",
        MaxDonors = 130,
        Status = EventStatus.Active,
        RequiredBloodTypes = "O+, O-, A+"
    },
    new BloodDonationEvent 
    {
        EventName = "Hiến máu tại Nhà văn hóa Thanh niên",
        LocationId = youthCultureHouse?.LocationId ?? 11,
        EventDate = DateTime.Now.AddDays(50),
        StartTime = new TimeSpan(8, 0, 0),
        EndTime = new TimeSpan(14, 0, 0),
        EventDescription = "Sự kiện đông đảo sinh viên và người trẻ tham gia.",
        MaxDonors = 110,
        Status = EventStatus.Active,
        RequiredBloodTypes = "All"
    },
    new BloodDonationEvent 
    {
        EventName = "Hiến máu Cộng đồng tại Siêu thị Aeon Mall",
        LocationId = aeonMall?.LocationId ?? 12,
        EventDate = DateTime.Now.AddDays(95),
        StartTime = new TimeSpan(9, 0, 0),
        EndTime = new TimeSpan(16, 0, 0),
        EventDescription = "Sự kiện tổ chức trong khuôn khổ chương trình trách nhiệm xã hội doanh nghiệp.",
        MaxDonors = 140,
        Status = EventStatus.Active,
        RequiredBloodTypes = "B-, AB-"
    },
    new BloodDonationEvent 
    {
        EventName = "Chủ nhật Đỏ – Hiến máu vì bệnh nhân",
        LocationId = district10Hospital?.LocationId ?? 13,
        EventDate = DateTime.Now.AddDays(5),
        StartTime = new TimeSpan(7, 0, 0),
        EndTime = new TimeSpan(12, 0, 0),
        EventDescription = "Sự kiện thường niên hưởng ứng phong trào Chủ nhật Đỏ.",
        MaxDonors = 70,
        Status = EventStatus.Active,
        RequiredBloodTypes = "All"
    }
                };
                context.BloodDonationEvents.AddRange(events);
                context.SaveChanges();
            }
        }
    }
} 