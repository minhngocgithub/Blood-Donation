using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Data.Configurations;
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
            modelBuilder.ApplyConfiguration(new (BloodType));
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
        }
    }
}