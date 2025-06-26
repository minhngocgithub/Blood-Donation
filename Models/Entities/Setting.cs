using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    public class Setting : BaseEntity
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        [StringLength(50)]
        public string SettingKey { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string SettingValue { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }
    }
}
