using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class DonationHistoryConfiguration : BaseEntityConfiguration<DonationHistory>
    {
        public override void Configure(EntityTypeBuilder<DonationHistory> builder)
        {
            base.Configure(builder);

            builder.ToTable("DonationHistory");

            // Properties
            builder.Property(dh => dh.DonationDate)
                .IsRequired();

            builder.Property(dh => dh.BloodType)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(dh => dh.Volume)
                .HasDefaultValue(350);

            builder.Property(dh => dh.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Completed");

            builder.Property(dh => dh.Notes)
                .HasMaxLength(500);

            builder.Property(dh => dh.CertificateIssued)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(dh => dh.User)
                .WithMany(u => u.DonationHistories)
                .HasForeignKey(dh => dh.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dh => dh.Event)
                .WithMany(e => e.DonationHistories)
                .HasForeignKey(dh => dh.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dh => dh.Registration)
                .WithMany(dr => dr.DonationHistories)
                .HasForeignKey(dh => dh.RegistrationId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(dh => dh.UserId)
                .HasDatabaseName("IX_DonationHistory_UserId");

            builder.HasIndex(dh => dh.DonationDate)
                .HasDatabaseName("IX_DonationHistory_DonationDate");
        }
    }

}
