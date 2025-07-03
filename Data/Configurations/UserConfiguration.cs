using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blood_Donation_Website.Data.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("Users");

            // Properties
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
                .HasMaxLength(20);

            builder.Property(u => u.Address)
                .HasMaxLength(200);

            builder.Property(u => u.Gender)
                .HasMaxLength(10);

            builder.Property(u => u.RoleId)
                .HasDefaultValue(2);

            builder.Property(u => u.EmailVerified)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.BloodType)
                .WithMany()
                .HasForeignKey(u => u.BloodTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("UQ_Users_Email");

            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("UQ_Users_Username");

            builder.HasIndex(u => u.RoleId)
                .HasDatabaseName("IX_Users_RoleId");
        }
    }
}
