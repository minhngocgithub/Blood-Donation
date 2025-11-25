using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(u => u.Username).IsUnique();
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Phone)
                .HasMaxLength(15);
            builder.Property(u => u.Address)
                .HasMaxLength(255);
            builder.Property(u => u.DateOfBirth);
            builder.Property(u => u.Gender)
                .HasMaxLength(10);
            builder.Property(u => u.BloodTypeId);
            builder.Property(u => u.RoleId).HasDefaultValue(2);
            builder.Property(u => u.IsActive).HasDefaultValue(true);
            builder.Property(u => u.EmailVerified).HasDefaultValue(false);
            builder.Property(u => u.LastDonationDate);
            builder.Property(u => u.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(u => u.UpdatedDate).HasDefaultValueSql("getdate()");
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            builder.HasOne(u => u.BloodType)
                .WithMany()
                .HasForeignKey(u => u.BloodTypeId);
        }
    }
}
