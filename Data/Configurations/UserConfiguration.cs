using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blood_Donation_Website.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Properties Configuration
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

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

            builder.Property(u => u.Gender)
                .HasMaxLength(10);

            builder.Property(u => u.BloodType)
                .HasMaxLength(5);

            builder.Property(u => u.RoleId)
                .HasDefaultValue(2);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.EmailVerified)
                .HasDefaultValue(false);

            builder.Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("UQ_Users_Username");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("UQ_Users_Email");

            builder.HasIndex(u => u.BloodType)
                .HasDatabaseName("IX_Users_BloodType");

            // Table Configuration
            builder.ToTable("Users");
        }
    }
}
