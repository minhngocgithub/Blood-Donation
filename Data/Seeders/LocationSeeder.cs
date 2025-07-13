using System;
using System.Collections.Generic;
using System.Linq;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Seeders
{
    public static class LocationSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Locations.Any()) return;

            var locations = new List<Location>
            {
               new Location
               {
                   LocationName = "Bach Mai Hospital",
                   Address = "78 Giai Phong, Phuong Mai, Dong Da, Hanoi",
                   ContactPhone = "024-3869-3731",
                   Capacity = 100,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               },
               new Location
               {
                   LocationName = "Viet Duc Hospital",
                   Address = "40 Trang Thi, Hoan Kiem, Hanoi",
                   ContactPhone = "024-3825-3531",
                   Capacity = 80,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               },
               new Location
               {
                   LocationName = "National Institute of Hematology and Blood Transfusion",
                   Address = "26 Nguyen Trai, Thanh Xuan, Hanoi",
                   ContactPhone = "024-3553-9206",
                   Capacity = 150,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               },
               new Location
               {
                   LocationName = "Hanoi Medical University",
                   Address = "1 Ton That Tung, Dong Da, Hanoi",
                   ContactPhone = "024-3852-3798",
                   Capacity = 60,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               },
               new Location
               {
                   LocationName = "Community Health Center - District 1",
                   Address = "123 Main Street, District 1, Ho Chi Minh City",
                   ContactPhone = "028-3822-1234",
                   Capacity = 75,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               },
               new Location
               {
                   LocationName = "Red Cross Blood Donation Center",
                   Address = "456 Le Loi Boulevard, District 3, Ho Chi Minh City",
                   ContactPhone = "028-3933-5678",
                   Capacity = 120,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               }
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();
        }
    }
}
