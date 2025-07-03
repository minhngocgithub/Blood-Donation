using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("BloodDonationEvent")]
    public class BloodDonationEvent : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string EventName { get; set; } = string.Empty;

        public string? EventDescription { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public int LocationId { get; set; }
        public int MaxDonors { get; set; } = 100;
        public int CurrentDonors { get; set; } = 0;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? RequiredBloodTypes { get; set; }

        public int CreatedBy { get; set; }

        
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; } = null!;

        public virtual ICollection<DonationRegistration> Registrations { get; set; } = new List<DonationRegistration>();
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
    }
}