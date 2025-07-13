using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BloodDonationAPI.Models.Entities;

namespace BloodDonationAPI.Data.Configurations
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
