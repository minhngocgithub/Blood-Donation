using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.HasKey(c => c.MessageId);
            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Phone)
                .HasMaxLength(15);
            builder.Property(c => c.Subject)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(c => c.Message)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(c => c.Status)
                .HasMaxLength(20)
                .HasDefaultValue("New");
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(c => c.ResolvedDate);
            builder.Property(c => c.ResolvedBy);
            builder.HasOne(c => c.ResolvedByUser)
                .WithMany(u => u.ResolvedContactMessages)
                .HasForeignKey(c => c.ResolvedBy);

            builder.HasIndex(c => c.Status)
                .HasDatabaseName("IX_ContactMessages_Status");

            builder.HasIndex(c => c.Email)
                .HasDatabaseName("IX_ContactMessages_Email");
        }
    }
}
