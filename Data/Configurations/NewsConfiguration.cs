using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(n => n.NewsId);
            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(n => n.Content)
                .IsRequired();
            builder.Property(n => n.Summary)
                .HasMaxLength(500);
            builder.Property(n => n.ImageUrl)
                .HasMaxLength(255);
            builder.Property(n => n.CategoryId);
            builder.Property(n => n.AuthorId);
            builder.Property(n => n.ViewCount).HasDefaultValue(0);
            builder.Property(n => n.IsPublished).HasDefaultValue(false);
            builder.Property(n => n.PublishedDate);
            builder.Property(n => n.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(n => n.UpdatedDate).HasDefaultValueSql("getdate()");
            builder.HasOne(n => n.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(n => n.CategoryId);
            builder.HasOne(n => n.Author)
                .WithMany(u => u.AuthoredNews)
                .HasForeignKey(n => n.AuthorId);
        }
    }
}
