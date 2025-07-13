using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(s => s.SettingId);
            builder.Property(s => s.SettingKey)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(s => s.SettingKey).IsUnique();
            builder.Property(s => s.SettingValue)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(s => s.Description)
                .HasMaxLength(200);
            builder.Property(s => s.UpdatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
