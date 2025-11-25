using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Utilities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class DonationHistoryConfiguration : IEntityTypeConfiguration<DonationHistory>
    {
        public void Configure(EntityTypeBuilder<DonationHistory> builder)
        {
            builder.HasKey(d => d.DonationId);
            builder.Property(d => d.UserId).IsRequired();
            builder.Property(d => d.EventId).IsRequired();
            builder.Property(d => d.RegistrationId);
            builder.Property(d => d.DonationDate).IsRequired();
            builder.Property(d => d.BloodTypeId).IsRequired();
            builder.Property(d => d.Volume).IsRequired();
            builder.Property(d => d.Status).HasDefaultValue(EnumMapper.DonationStatus.Completed);
            builder.Property(d => d.Notes).HasMaxLength(500);
            builder.Property(d => d.NextEligibleDate);
            builder.Property(d => d.CertificateIssued).HasDefaultValue(false);
            builder.HasOne(d => d.User)
                .WithMany(u => u.DonationHistories)
                .HasForeignKey(d => d.UserId);
            builder.HasOne(d => d.Event)
                .WithMany(e => e.DonationHistories)
                .HasForeignKey(d => d.EventId);
            builder.HasOne(d => d.Registration)
                .WithMany(r => r.DonationHistories)
                .HasForeignKey(d => d.RegistrationId);
            builder.HasOne(d => d.BloodType)
                .WithMany()
                .HasForeignKey(d => d.BloodTypeId);
        }
    }
}
