using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Blood_Donation_Website.Models.Entities;
namespace Blood_Donation_Website.Data.Configurations
{
    public class LocationConfiguration : BaseEntityConfiguration<Location>
    {
        public override void Configure(EntityTypeBuilder<Location> builder)
        {
            base.Configure(builder);

            builder.ToTable("Locations");

            builder.Property(l => l.LocationName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(l => l.ContactPhone)
                .HasMaxLength(15);

            builder.Property(l => l.Capacity)
                .HasDefaultValue(50);

            // Indexes
            builder.HasIndex(l => l.LocationName)
                .HasDatabaseName("IX_Locations_LocationName");
        }
    }
}
