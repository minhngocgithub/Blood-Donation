using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("BloodTypes")]
    public class BloodType
    {
        [Key]
        public int BloodTypeId { get; set; }

        [Required]
        [StringLength(5)]
        public string BloodTypeName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Description { get; set; }
    }
}
