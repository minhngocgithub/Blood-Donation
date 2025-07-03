using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.ToTable("ContactMessages");

            builder.HasKey(cm => cm.MessageId);

            builder.Property(cm => cm.MessageId)
                .ValueGeneratedOnAdd();

            builder.Property(cm => cm.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cm => cm.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cm => cm.Phone)
                .HasMaxLength(15);

            builder.Property(cm => cm.Subject)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(cm => cm.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(cm => cm.Status)
                .HasMaxLength(20)
                .HasDefaultValue("New");

            builder.Property(cm => cm.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Relationship
            
        }
    }
}
