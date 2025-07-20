using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("DonationRegistrations")]
    public class DonationRegistration
    {
        [Key]
        public int RegistrationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;

        [StringLength(500)]
        public string? Notes { get; set; }

        public bool IsEligible { get; set; } = true;
        public DateTime? CheckInTime { get; set; }
        public DateTime? CompletionTime { get; set; }

        [StringLength(200)]
        public string? CancellationReason { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("EventId")]
        public virtual BloodDonationEvent Event { get; set; } = null!;

        public virtual HealthScreening? HealthScreening { get; set; }
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
    }
}