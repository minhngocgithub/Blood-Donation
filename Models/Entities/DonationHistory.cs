using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("DonationHistory")]
    public class DonationHistory : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        public int? RegistrationId { get; set; }

        [Required]
        public DateTime DonationDate { get; set; }

        [Required]
        [StringLength(5)]
        public string BloodType { get; set; } = string.Empty;

        public int Volume { get; set; } = 350;

        [StringLength(20)]
        public string Status { get; set; } = "Completed";

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime? NextEligibleDate { get; set; }
        public bool CertificateIssued { get; set; } = false;

        public virtual User User { get; set; } = null!;
        public virtual BloodDonationEvent Event { get; set; } = null!;
        public virtual DonationRegistration? Registration { get; set; }
    }
}