using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Settings");

            builder.HasKey(s => s.SettingId);

            builder.Property(s => s.SettingId)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.SettingKey)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SettingValue)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.Description)
                .HasMaxLength(200);

            builder.Property(s => s.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasIndex(s => s.SettingKey)
                .IsUnique();
        }
    }
}
