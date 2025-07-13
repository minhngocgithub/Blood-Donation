using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.RoleId);
            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(r => r.RoleName).IsUnique();
            builder.Property(r => r.Description)
                .HasMaxLength(200);
            builder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("getdate()");
            builder.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
