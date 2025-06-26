using Blood_Donation_Website.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            ConfigureUserRelationships(modelBuilder);
            ConfigureEventRelationships(modelBuilder);
            ConfigureRegistrationRelationships(modelBuilder);
            ConfigureNewsRelationships(modelBuilder);
            ConfigureUniqueConstraints(modelBuilder);
            ConfigureIndexes(modelBuilder);
        }
        private void ConfigureUserRelationships(ModelBuilder modelBuilder)
        {
            // User - Role relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void ConfigureEventRelationships(ModelBuilder modelBuilder)
        {
            // Event - Location relationship
            modelBuilder.Entity<BloodDonationEvent>()
                .HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event - Creator relationship
            modelBuilder.Entity<BloodDonationEvent>()
                .HasOne(e => e.Creator)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void ConfigureRegistrationRelationships(ModelBuilder modelBuilder)
        {
            // Registration - User relationship
            modelBuilder.Entity<DonationRegistration>()
                .HasOne(r => r.User)
                .WithMany(u => u.DonationRegistrations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Registration - Event relationship
            modelBuilder.Entity<DonationRegistration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthScreening - Registration relationship (One-to-One)
            modelBuilder.Entity<HealthScreening>()
                .HasOne(h => h.Registration)
                .WithOne(r => r.HealthScreening)
                .HasForeignKey<HealthScreening>(h => h.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            // DonationHistory relationships
            modelBuilder.Entity<DonationHistory>()
                .HasOne(d => d.User)
                .WithMany(u => u.DonationHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonationHistory>()
                .HasOne(d => d.Event)
                .WithMany(e => e.DonationHistories)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonationHistory>()
                .HasOne(d => d.Registration)
                .WithMany(r => r.DonationHistories)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        private void ConfigureNewsRelationships(ModelBuilder modelBuilder)
        {
            // News - Category relationship
            modelBuilder.Entity<News>()
                .HasOne(n => n.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // News - Author relationship
            modelBuilder.Entity<News>()
                .HasOne(n => n.Author)
                .WithMany(u => u.NewsArticles)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        private void ConfigureUniqueConstraints(ModelBuilder modelBuilder)
        {
            // Unique constraints
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<BloodType>()
                .HasIndex(b => b.BloodTypeName)
                .IsUnique();

            modelBuilder.Entity<Setting>()
                .HasIndex(s => s.SettingKey)
                .IsUnique();
        }
        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Performance indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email);

            modelBuilder.Entity<BloodDonationEvent>()
                .HasIndex(e => e.EventDate);

            modelBuilder.Entity<DonationRegistration>()
                .HasIndex(r => new { r.UserId, r.EventId });

            modelBuilder.Entity<News>()
                .HasIndex(n => n.IsPublished);
        }
    }
}
