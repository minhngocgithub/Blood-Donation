using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Configurations
{
    public class BloodTypeConfiguration : IEntityTypeConfiguration<BloodType>
    {
        public void Configure(EntityTypeBuilder<BloodType> builder)
        {
            builder.HasKey(b => b.BloodTypeId);
            builder.Property(b => b.BloodTypeName)
                .IsRequired()
                .HasMaxLength(5);
            builder.HasIndex(b => b.BloodTypeName).IsUnique();
            builder.Property(b => b.Description)
                .HasMaxLength(100);
        }
    }
}
