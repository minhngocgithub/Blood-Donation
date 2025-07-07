using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [StringLength(200)]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [StringLength(15)]
        public string? ContactPhone { get; set; }

        public int Capacity { get; set; } = 50;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<BloodDonationEvent> Events { get; set; } = new List<BloodDonationEvent>();
    }
}
