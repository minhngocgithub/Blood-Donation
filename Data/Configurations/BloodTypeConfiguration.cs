using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class BloodTypeConfiguration : BaseEntityConfiguration<BloodType>
    {
        public override void Configure(EntityTypeBuilder<BloodType> builder)
        {
            base.Configure(builder);

            builder.ToTable("BloodTypes");

            builder.Property(bt => bt.BloodTypeName)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(bt => bt.Description)
                .HasMaxLength(100);

            builder.Property(bt => bt.CanDonateTo)
                .HasMaxLength(50);

            builder.Property(bt => bt.CanReceiveFrom)
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(bt => bt.BloodTypeName)
                .IsUnique()
                .HasDatabaseName("UQ_BloodTypes_BloodTypeName");
        }
    }
}
