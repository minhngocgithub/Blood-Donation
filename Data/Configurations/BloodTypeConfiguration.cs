using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class BloodTypeConfiguration : IEntityTypeConfiguration<BloodType>
    {
        public void Configure(EntityTypeBuilder<BloodType> builder)
        {
            builder.ToTable("BloodTypes");

            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.Id)
                .HasColumnName("BloodTypeId")
                .ValueGeneratedOnAdd();

            builder.Property(bt => bt.BloodTypeName)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(bt => bt.Description)
                .HasMaxLength(100);

            builder.Property(bt => bt.CanDonateTo)
                .HasMaxLength(50);

            builder.Property(bt => bt.CanReceiveFrom)
                .HasMaxLength(50);

            builder.Property(bt => bt.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(bt => bt.BloodTypeName)
                .IsUnique();
        }
    }
}
