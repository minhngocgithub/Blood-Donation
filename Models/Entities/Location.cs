using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Location")]
    public class Location : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [StringLength(15)]
        public string? ContactPhone { get; set; }

        public int Capacity { get; set; } = 50;

        public virtual ICollection<BloodDonationEvent> Events { get; set; } = new List<BloodDonationEvent>();
    }
}
