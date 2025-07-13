using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
    {
        public void Configure(EntityTypeBuilder<NewsCategory> builder)
        {
            builder.HasKey(nc => nc.CategoryId);
            builder.Property(nc => nc.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(nc => nc.Description)
                .HasMaxLength(200);
            builder.Property(nc => nc.IsActive).HasDefaultValue(true);
        }
    }
}
