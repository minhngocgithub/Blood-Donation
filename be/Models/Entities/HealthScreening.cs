using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodDonationAPI.Models.Entities
{
    [Table("HealthScreening")]
    public class HealthScreening
    {
        [Key]
        public int ScreeningId { get; set; }

        [Required]
        public int RegistrationId { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; }

        [StringLength(20)]
        public string? BloodPressure { get; set; }

        public int? HeartRate { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? Temperature { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? Hemoglobin { get; set; }

        public bool IsEligible { get; set; } = true;

        [StringLength(500)]
        public string? DisqualifyReason { get; set; }

        public int? ScreenedBy { get; set; }
        public DateTime ScreeningDate { get; set; } = DateTime.Now;

        [ForeignKey("RegistrationId")]
        public virtual DonationRegistration Registration { get; set; } = null!;

        [ForeignKey("ScreenedBy")]
        public virtual User? ScreenedByUser { get; set; }
    }
}