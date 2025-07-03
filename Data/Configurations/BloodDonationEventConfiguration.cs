using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class BloodDonationEventConfiguration : BaseEntityConfiguration<BloodDonationEvent>
    {
        public override void Configure(EntityTypeBuilder<BloodDonationEvent> builder)
        {
            base.Configure(builder);

            builder.ToTable("BloodDonationEvents");

            // Properties
            builder.Property(e => e.EventName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.EventDescription)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.EventDate)
                .IsRequired();

            builder.Property(e => e.StartTime)
                .IsRequired();

            builder.Property(e => e.EndTime)
                .IsRequired();

            builder.Property(e => e.MaxDonors)
                .HasDefaultValue(100);

            builder.Property(e => e.CurrentDonors)
                .HasDefaultValue(0);

            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(255);

            builder.Property(e => e.RequiredBloodTypes)
                .HasMaxLength(100);

            // Relationships
            builder.HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Creator)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.EventDate)
                .HasDatabaseName("IX_BloodDonationEvents_EventDate");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_BloodDonationEvents_Status");

            builder.HasIndex(e => e.LocationId)
                .HasDatabaseName("IX_BloodDonationEvents_LocationId");

            builder.HasIndex(e => e.CreatedBy)
                .HasDatabaseName("IX_BloodDonationEvents_CreatedBy");
        }
    }

}
