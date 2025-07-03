using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
    {
        public void Configure(EntityTypeBuilder<NewsCategory> builder)
        {
            builder.ToTable("NewsCategories");

            builder.HasKey(nc => nc.CategoryId);

            builder.Property(nc => nc.CategoryId)
                .ValueGeneratedOnAdd();

            builder.Property(nc => nc.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(nc => nc.Description)
                .HasMaxLength(200);

            builder.Property(nc => nc.IsActive)
                .HasDefaultValue(true);
        }
    }
}
