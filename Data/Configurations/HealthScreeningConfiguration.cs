using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class HealthScreeningConfiguration : IEntityTypeConfiguration<HealthScreening>
    {
        public void Configure(EntityTypeBuilder<HealthScreening> builder)
        {
            // Primary Key
            builder.HasKey(h => h.ScreeningId);

            // Properties Configuration
            builder.Property(h => h.Weight)
                .HasColumnType("decimal(5,2)");

            builder.Property(h => h.Height)
                .HasColumnType("decimal(5,2)");

            builder.Property(h => h.BloodPressure)
                .HasMaxLength(20);

            builder.Property(h => h.Temperature)
                .HasColumnType("decimal(4,2)");

            builder.Property(h => h.Hemoglobin)
                .HasColumnType("decimal(4,2)");

            builder.Property(h => h.IsEligible)
                .HasDefaultValue(true);

            builder.Property(h => h.DisqualifyReason)
                .HasMaxLength(500);

            builder.Property(h => h.ScreeningDate)
                .HasDefaultValueSql("GETDATE()");

            // Relationships (One-to-One with DonationRegistration)
            builder.HasOne(h => h.Registration)
                .WithOne(r => r.HealthScreening)
                .HasForeignKey<HealthScreening>(h => h.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.ScreenedByUser)
                .WithMany()
                .HasForeignKey(h => h.ScreenedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(h => h.RegistrationId)
                .IsUnique()
                .HasDatabaseName("UQ_HealthScreening_RegistrationId");

            builder.HasIndex(h => h.ScreeningDate)
                .HasDatabaseName("IX_HealthScreening_ScreeningDate");

            // Table Configuration
            builder.ToTable("HealthScreenings");
        }
    }
}
