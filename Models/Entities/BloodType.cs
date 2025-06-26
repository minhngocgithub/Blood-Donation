using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    public class BloodType : BaseEntity
    {
        [Required]
        [StringLength(5)]
        public string BloodTypeName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? CanDonateTo { get; set; }

        [StringLength(50)]
        public string? CanReceiveFrom { get; set; }
    }
}
