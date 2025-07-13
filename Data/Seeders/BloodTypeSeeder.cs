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
                    Description = "Blood type A positive - Can donate to A+ and AB+, can receive from A+, A-, O+, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "A-", 
                    Description = "Blood type A negative - Can donate to A+, A-, AB+, AB-, can receive from A-, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "B+", 
                    Description = "Blood type B positive - Can donate to B+ and AB+, can receive from B+, B-, O+, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "B-", 
                    Description = "Blood type B negative - Can donate to B+, B-, AB+, AB-, can receive from B-, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "AB+", 
                    Description = "Blood type AB positive - Universal plasma donor, can receive from all blood types" 
                },
                new BloodType 
                { 
                    BloodTypeName = "AB-", 
                    Description = "Blood type AB negative - Can donate to AB+, AB-, can receive from A-, B-, AB-, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "O+", 
                    Description = "Blood type O positive - Can donate to A+, B+, AB+, O+, can receive from O+, O-" 
                },
                new BloodType 
                { 
                    BloodTypeName = "O-", 
                    Description = "Blood type O negative - Universal donor, can donate to all blood types" 
                }
            };

            context.BloodTypes.AddRange(bloodTypes);
            context.SaveChanges();
        }
    }
}
