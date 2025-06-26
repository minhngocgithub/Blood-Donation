using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Blood_Donation_Website.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(5)]
        public string? BloodType { get; set; }

        public int RoleId { get; set; } = 2; 

        public bool EmailVerified { get; set; } = false;
        public DateTime? LastDonationDate { get; set; }

        
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<BloodDonationEvent> CreatedEvents { get; set; } = new List<BloodDonationEvent>();
        public virtual ICollection<DonationRegistration> DonationRegistrations { get; set; } = new List<DonationRegistration>();
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
        public virtual ICollection<News> NewsArticles { get; set; } = new List<News>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    
    }
}
