using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Data.Configurations;
using Blood_Donation_Website.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region DbSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BloodType> BloodTypes { get; set; }
        public DbSet<BloodCompatibility> BloodCompatibilities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BloodDonationEvent> BloodDonationEvents { get; set; }
        public DbSet<DonationRegistration> DonationRegistrations { get; set; }
        public DbSet<HealthScreening> HealthScreenings { get; set; }
        public DbSet<DonationHistory> DonationHistories { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BloodTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BloodCompatibilityConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new BloodDonationEventConfiguration());
            modelBuilder.ApplyConfiguration(new DonationRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new HealthScreeningConfiguration());
            modelBuilder.ApplyConfiguration(new DonationHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new NewsCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new ContactMessageConfiguration());

            // Enum to string conversions
            modelBuilder.Entity<BloodDonationEvent>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<DonationRegistration>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<HealthScreening>().Property(e => e.DisqualifyReason).HasConversion<string>();
            modelBuilder.Entity<DonationHistory>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<Notification>().Property(e => e.Type).HasConversion<string>();
            modelBuilder.Entity<ContactMessage>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<User>().Property(e => e.Gender).HasConversion<string>();
            modelBuilder.Entity<Role>().Property(e => e.RoleName).HasConversion<string>();
        }

        /// <summary>
        /// Seeds the database with initial data
        /// </summary>
        public void SeedData()
        {
            RoleSeeder.Seed(this);
            BloodTypeSeeder.Seed(this);
            BloodCompatibilitySeeder.Seed(this);
            LocationSeeder.Seed(this);
            AdminSeeder.Seed(this);
            UserSeeder.Seed(this);
            BloodDonationEventSeeder.Seed(this);
            DonationRegistrationSeeder.Seed(this);
            HealthScreeningSeeder.Seed(this);
            DonationHistorySeeder.Seed(this);
            NewsCategorySeeder.Seed(this);
            NewsSeeder.Seed(this);
            NotificationSeeder.Seed(this);
            SettingSeeder.Seed(this);
            ContactMessageSeeder.Seed(this);
        }
    }
}