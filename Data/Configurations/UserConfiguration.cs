using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blood_Donation_Website.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(u => u.Phone)
                .HasMaxLength(20);
                
            builder.Property(u => u.Address)
                .HasMaxLength(200);
                
            builder.Property(u => u.Gender)
                .HasMaxLength(10);
            
            // Configure relationships
            builder.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(u => u.BloodType)
                .WithMany()
                .HasForeignKey(u => u.BloodTypeId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure indexes
            builder.HasIndex(u => u.Email)
                .IsUnique();
                
            builder.HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
