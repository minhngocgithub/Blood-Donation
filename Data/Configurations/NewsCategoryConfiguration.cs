using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data.Configurations
{
    public class NewsCategoryConfiguration : BaseEntityConfiguration<NewsCategory>
    {
        public override void Configure(EntityTypeBuilder<NewsCategory> builder)
        {
            base.Configure(builder);

            builder.ToTable("NewsCategories");

            builder.Property(nc => nc.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(nc => nc.Description)
                .HasMaxLength(200);

            // Indexes
            builder.HasIndex(nc => nc.CategoryName)
                .IsUnique()
                .HasDatabaseName("UQ_NewsCategories_CategoryName");
        }
    }
}
