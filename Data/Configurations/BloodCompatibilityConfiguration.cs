using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class BloodCompatibilityConfiguration : IEntityTypeConfiguration<BloodCompatibility>
    {
        public void Configure(EntityTypeBuilder<BloodCompatibility> builder)
        {
            builder.HasKey(bc => bc.Id);
            builder.Property(bc => bc.FromBloodTypeId).IsRequired();
            builder.Property(bc => bc.ToBloodTypeId).IsRequired();
            builder.HasOne(bc => bc.FromBloodType)
                .WithMany()
                .HasForeignKey(bc => bc.FromBloodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bc => bc.ToBloodType)
                .WithMany()
                .HasForeignKey(bc => bc.ToBloodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 