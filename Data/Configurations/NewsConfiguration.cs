using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            // Primary Key
            builder.HasKey(n => n.NewsId);

            // Properties Configuration
            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(n => n.Summary)
                .HasMaxLength(500);

            builder.Property(n => n.ImageUrl)
                .HasMaxLength(255);

            builder.Property(n => n.ViewCount)
                .HasDefaultValue(0);

            builder.Property(n => n.IsPublished)
                .HasDefaultValue(false);

            builder.Property(n => n.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(n => n.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(n => n.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            

            // Indexes
            builder.HasIndex(n => n.IsPublished)
                .HasDatabaseName("IX_News_IsPublished");

            builder.HasIndex(n => n.PublishedDate)
                .HasDatabaseName("IX_News_PublishedDate");

            builder.HasIndex(n => n.CategoryId)
                .HasDatabaseName("IX_News_CategoryId");

            // Table Configuration
            builder.ToTable("News");
        }
    }
}
