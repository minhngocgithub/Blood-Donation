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
                    }
                };
                context.BloodDonationEvents.AddRange(events);
                context.SaveChanges();
            }
        }
    }
} 