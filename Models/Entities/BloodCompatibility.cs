using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("BloodCompatibility")]
    public class BloodCompatibility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FromBloodTypeId { get; set; }

        [Required]
        public int ToBloodTypeId { get; set; }

        [ForeignKey("FromBloodTypeId")]
        public virtual BloodType FromBloodType { get; set; } = null!;

        [ForeignKey("ToBloodTypeId")]
        public virtual BloodType ToBloodType { get; set; } = null!;
    }
} 