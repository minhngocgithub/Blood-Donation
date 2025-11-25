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
                    }
                };
                context.Locations.AddRange(locations);
                context.SaveChanges();
            }
        }
    }
}
