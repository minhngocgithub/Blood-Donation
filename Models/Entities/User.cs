using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public int? BloodTypeId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool EmailVerified { get; set; } = false;
        public DateTime? LastDonationDate { get; set; }

 
        public virtual Role Role { get; set; } = null!;
        public virtual BloodType? BloodType { get; set; }

        public virtual ICollection<BloodDonationEvent> CreatedEvents { get; set; } = new List<BloodDonationEvent>();
        public virtual ICollection<DonationRegistration> DonationRegistrations { get; set; } = new List<DonationRegistration>();
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
        public virtual ICollection<HealthScreening> HealthScreenings { get; set; } = new List<HealthScreening>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<ContactMessage> ResolvedContactMessages { get; set; } = new List<ContactMessage>();
        public virtual ICollection<News> AuthoredNews { get; set; } = new List<News>();
    }
}
