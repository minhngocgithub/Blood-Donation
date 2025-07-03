using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class ContactMessageConfiguration : BaseEntityConfiguration<ContactMessage>
    {
        public override void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            base.Configure(builder);

            builder.ToTable("ContactMessages");

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

            builder.HasOne(cm => cm.ResolvedByUser)
                .WithMany(u => u.ResolvedContactMessages)
                .HasForeignKey(cm => cm.ResolvedBy)
                .OnDelete(DeleteBehavior.SetNull);


            // Indexes
            builder.HasIndex(cm => cm.Status)
                .HasDatabaseName("IX_ContactMessages_Status");

            builder.HasIndex(cm => cm.Email)
                .HasDatabaseName("IX_ContactMessages_Email");
        }
    }
}
