using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class DonationRegistrationConfiguration : IEntityTypeConfiguration<DonationRegistration>
    {
        public void Configure(EntityTypeBuilder<DonationRegistration> builder)
        {
            // Primary Key
            builder.HasKey(r => r.RegistrationId);

            // Properties Configuration
            builder.Property(r => r.RegistrationDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(r => r.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Registered");

            builder.Property(r => r.Notes)
                .HasMaxLength(500);

            builder.Property(r => r.IsEligible)
                .HasDefaultValue(true);

            builder.Property(r => r.CancellationReason)
                .HasMaxLength(200);

            // Relationships
            builder.HasOne(r => r.User)
                .WithMany(u => u.DonationRegistrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(r => new { r.UserId, r.EventId })
                .IsUnique()
                .HasDatabaseName("UQ_DonationRegistrations_UserId_EventId");

            builder.HasIndex(r => r.Status)
                .HasDatabaseName("IX_DonationRegistrations_Status");

            builder.HasIndex(r => r.RegistrationDate)
                .HasDatabaseName("IX_DonationRegistrations_RegistrationDate");

            // Table Configuration
            builder.ToTable("DonationRegistrations");
        }
    }
}
