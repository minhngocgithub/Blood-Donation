using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Configurations
{
    public class HealthScreeningConfiguration : IEntityTypeConfiguration<HealthScreening>
    {
        public void Configure(EntityTypeBuilder<HealthScreening> builder)
        {
            builder.HasKey(h => h.ScreeningId);
            builder.Property(h => h.RegistrationId).IsRequired();
            builder.Property(h => h.Weight).HasColumnType("decimal(5,2)");
            builder.Property(h => h.Height).HasColumnType("decimal(5,2)");
            builder.Property(h => h.BloodPressure).HasMaxLength(20);
            builder.Property(h => h.HeartRate);
            builder.Property(h => h.Temperature).HasColumnType("decimal(4,2)");
            builder.Property(h => h.Hemoglobin).HasColumnType("decimal(4,2)");
            builder.Property(h => h.IsEligible).HasDefaultValue(true);
            builder.Property(h => h.DisqualifyReason).HasMaxLength(500);
            builder.Property(h => h.ScreenedBy);
            builder.Property(h => h.ScreeningDate).HasDefaultValueSql("getdate()");
            builder.HasOne(h => h.Registration)
                .WithOne(r => r.HealthScreening)
                .HasForeignKey<HealthScreening>(h => h.RegistrationId);
            builder.HasOne(h => h.ScreenedByUser)
                .WithMany(u => u.HealthScreenings)
                .HasForeignKey(h => h.ScreenedBy);
        }
    }
}
