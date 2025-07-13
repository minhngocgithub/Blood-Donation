using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Configurations
{
    public class BloodDonationEventConfiguration : IEntityTypeConfiguration<BloodDonationEvent>
    {
        public void Configure(EntityTypeBuilder<BloodDonationEvent> builder)
        {
            builder.HasKey(e => e.EventId);
            builder.Property(e => e.EventName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.EventDescription);
            builder.Property(e => e.EventDate).IsRequired();
            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.EndTime).IsRequired();
            builder.Property(e => e.LocationId);
            builder.Property(e => e.MaxDonors).HasDefaultValue(100);
            builder.Property(e => e.CurrentDonors).HasDefaultValue(0);
            builder.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Active");
            builder.Property(e => e.ImageUrl).HasMaxLength(255);
            builder.Property(e => e.RequiredBloodTypes).HasMaxLength(100);
            builder.Property(e => e.CreatedBy);
            builder.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(e => e.UpdatedDate).HasDefaultValueSql("getdate()");
            builder.HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId);
            builder.HasOne(e => e.Creator)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedBy);
        }
    }
}
