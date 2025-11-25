using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.LocationId);
            builder.Property(l => l.LocationName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(l => l.Address)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(l => l.ContactPhone)
                .HasMaxLength(15);
            builder.Property(l => l.Capacity).HasDefaultValue(50);
            builder.Property(l => l.IsActive).HasDefaultValue(true);
            builder.Property(l => l.CreatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
