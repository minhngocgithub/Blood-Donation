using System;
using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
{
    public static class BloodDonationEventSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.BloodDonationEvents.Any())
            {
                var events = new List<BloodDonationEvent>
                {
                    new BloodDonationEvent 
                    { 
                        EventName = "Spring Blood Drive", 
                        LocationId = 1, 
                        EventDate = DateTime.Now.AddDays(30), 
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0),
                        EventDescription = "Annual spring blood donation drive to help our community.",
                        MaxDonors = 100,
                        Status = "Active",
                        RequiredBloodTypes = "All Types Welcome"
                    },
                    new BloodDonationEvent 
                    { 
                        EventName = "Summer Health Fair Blood Drive", 
                        LocationId = 2, 
                        EventDate = DateTime.Now.AddDays(60), 
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(16, 0, 0),
                        EventDescription = "Blood donation event as part of the summer health fair.",
                        MaxDonors = 75,
                        Status = "Active",
                        RequiredBloodTypes = "O-, A-, B-, AB-"
                    },
                    new BloodDonationEvent 
                    { 
                        EventName = "Corporate Blood Drive", 
                        LocationId = 3, 
                        EventDate = DateTime.Now.AddDays(90), 
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(15, 0, 0),
                        EventDescription = "Corporate sponsored blood donation event.",
                        MaxDonors = 50,
                        Status = "Active",
                        RequiredBloodTypes = "All Types Welcome"
                    }
                };
                context.BloodDonationEvents.AddRange(events);
                context.SaveChanges();
            }
        }
    }
} 