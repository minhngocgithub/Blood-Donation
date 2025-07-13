using System.Collections.Generic;
using System.Linq;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Seeders
{
    public static class BloodCompatibilitySeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.BloodCompatibilities.Any())
            {
                var compatibilities = new List<BloodCompatibility>
                {
                    // O- can donate to everyone (Universal donor)
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 1 }, // O- -> A+
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 2 }, // O- -> A-
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 3 }, // O- -> B+
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 4 }, // O- -> B-
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 5 }, // O- -> AB+
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 6 }, // O- -> AB-
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 7 }, // O- -> O+
                    new BloodCompatibility { FromBloodTypeId = 8, ToBloodTypeId = 8 }, // O- -> O-

                    // O+ can donate to O+, A+, B+, AB+
                    new BloodCompatibility { FromBloodTypeId = 7, ToBloodTypeId = 1 }, // O+ -> A+
                    new BloodCompatibility { FromBloodTypeId = 7, ToBloodTypeId = 3 }, // O+ -> B+
                    new BloodCompatibility { FromBloodTypeId = 7, ToBloodTypeId = 5 }, // O+ -> AB+
                    new BloodCompatibility { FromBloodTypeId = 7, ToBloodTypeId = 7 }, // O+ -> O+

                    // A- can donate to A+, A-, AB+, AB-
                    new BloodCompatibility { FromBloodTypeId = 2, ToBloodTypeId = 1 }, // A- -> A+
                    new BloodCompatibility { FromBloodTypeId = 2, ToBloodTypeId = 2 }, // A- -> A-
                    new BloodCompatibility { FromBloodTypeId = 2, ToBloodTypeId = 5 }, // A- -> AB+
                    new BloodCompatibility { FromBloodTypeId = 2, ToBloodTypeId = 6 }, // A- -> AB-

                    // A+ can donate to A+, AB+
                    new BloodCompatibility { FromBloodTypeId = 1, ToBloodTypeId = 1 }, // A+ -> A+
                    new BloodCompatibility { FromBloodTypeId = 1, ToBloodTypeId = 5 }, // A+ -> AB+

                    // B- can donate to B+, B-, AB+, AB-
                    new BloodCompatibility { FromBloodTypeId = 4, ToBloodTypeId = 3 }, // B- -> B+
                    new BloodCompatibility { FromBloodTypeId = 4, ToBloodTypeId = 4 }, // B- -> B-
                    new BloodCompatibility { FromBloodTypeId = 4, ToBloodTypeId = 5 }, // B- -> AB+
                    new BloodCompatibility { FromBloodTypeId = 4, ToBloodTypeId = 6 }, // B- -> AB-

                    // B+ can donate to B+, AB+
                    new BloodCompatibility { FromBloodTypeId = 3, ToBloodTypeId = 3 }, // B+ -> B+
                    new BloodCompatibility { FromBloodTypeId = 3, ToBloodTypeId = 5 }, // B+ -> AB+

                    // AB- can donate to AB+, AB-
                    new BloodCompatibility { FromBloodTypeId = 6, ToBloodTypeId = 5 }, // AB- -> AB+
                    new BloodCompatibility { FromBloodTypeId = 6, ToBloodTypeId = 6 }, // AB- -> AB-

                    // AB+ can donate to AB+ only
                    new BloodCompatibility { FromBloodTypeId = 5, ToBloodTypeId = 5 }, // AB+ -> AB+
                };

                context.BloodCompatibilities.AddRange(compatibilities);
                context.SaveChanges();
            }
        }
    }
} 