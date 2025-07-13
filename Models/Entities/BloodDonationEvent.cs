using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("BloodDonationEvents")]
    public class BloodDonationEvent
    {
        [Key]
        public int EventId { get; set; }

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

        public int? LocationId { get; set; }
        public int MaxDonors { get; set; } = 100;
        public int CurrentDonors { get; set; } = 0;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? RequiredBloodTypes { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User? Creator { get; set; }

        public virtual ICollection<DonationRegistration> Registrations { get; set; } = new List<DonationRegistration>();
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
    }
}