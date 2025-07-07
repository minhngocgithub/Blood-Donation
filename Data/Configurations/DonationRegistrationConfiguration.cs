using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class DonationRegistrationConfiguration : IEntityTypeConfiguration<DonationRegistration>
    {
        public void Configure(EntityTypeBuilder<DonationRegistration> builder)
        {
            builder.HasKey(r => r.RegistrationId);
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.EventId).IsRequired();
            builder.Property(r => r.RegistrationDate).HasDefaultValueSql("getdate()");
            builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("Registered");
            builder.Property(r => r.Notes).HasMaxLength(500);
            builder.Property(r => r.IsEligible).HasDefaultValue(true);
            builder.Property(r => r.CheckInTime);
            builder.Property(r => r.CompletionTime);
            builder.Property(r => r.CancellationReason).HasMaxLength(200);
            builder.HasOne(r => r.User)
                .WithMany(u => u.DonationRegistrations)
                .HasForeignKey(r => r.UserId);
            builder.HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId);
            builder.HasOne(r => r.HealthScreening)
                .WithOne(h => h.Registration)
                .HasForeignKey<HealthScreening>(h => h.RegistrationId);
        }
    }
}
