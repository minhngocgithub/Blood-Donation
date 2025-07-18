using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class HealthScreeningSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.HealthScreenings.Any())
            {
                var screenings = new List<HealthScreening>
                {
                    new HealthScreening 
                    { 
                        RegistrationId = 1,
                        Weight = 70.5m,
                        Height = 175.0m,
                        BloodPressure = "120/80",
                        HeartRate = 72,
                        Temperature = 36.5m,
                        Hemoglobin = 14.2m,
                        IsEligible = true,
                        ScreenedBy = 1,
                        ScreeningDate = DateTime.Now.AddDays(-5)
                    },
                    new HealthScreening 
                    { 
                        RegistrationId = 2,
                        Weight = 65.0m,
                        Height = 168.0m,
                        BloodPressure = "110/70",
                        HeartRate = 68,
                        Temperature = 36.8m,
                        Hemoglobin = 13.8m,
                        IsEligible = true,
                        ScreenedBy = 1,
                        ScreeningDate = DateTime.Now.AddDays(-3)
                    },
                    new HealthScreening 
                    { 
                        RegistrationId = 3,
                        Weight = 58.0m,
                        Height = 160.0m,
                        BloodPressure = "140/90",
                        HeartRate = 85,
                        Temperature = 37.2m,
                        Hemoglobin = 11.5m,
                        IsEligible = false,
                        DisqualifyReason = "Blood pressure too high and hemoglobin below minimum threshold",
                        ScreenedBy = 2,
                        ScreeningDate = DateTime.Now.AddDays(-2)
                    },
                    new HealthScreening 
                    { 
                        RegistrationId = 4,
                        Weight = 80.0m,
                        Height = 182.0m,
                        BloodPressure = "118/75",
                        HeartRate = 70,
                        Temperature = 36.6m,
                        Hemoglobin = 15.1m,
                        IsEligible = true,
                        ScreenedBy = 2,
                        ScreeningDate = DateTime.Now.AddDays(-1)
                    },
                    new HealthScreening 
                    { 
                        RegistrationId = 5,
                        Weight = 62.5m,
                        Height = 165.0m,
                        BloodPressure = "115/78",
                        HeartRate = 74,
                        Temperature = 37.0m,
                        Hemoglobin = 12.8m,
                        IsEligible = false,
                        DisqualifyReason = "Slightly elevated temperature - recommended to return when feeling completely well",
                        ScreenedBy = 1,
                        ScreeningDate = DateTime.Now
                    }
                };
                context.HealthScreenings.AddRange(screenings);
                context.SaveChanges();
            }
        }
    }
} 