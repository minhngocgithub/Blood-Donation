using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class SettingConfiguration : BaseEntityConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);

            builder.ToTable("Settings");

            builder.Property(s => s.SettingKey)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.SettingValue)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.Description)
                .HasMaxLength(200);

            // Indexes
            builder.HasIndex(s => s.SettingKey)
                .IsUnique()
                .HasDatabaseName("UQ_Settings_SettingKey");
        }
    }
}
