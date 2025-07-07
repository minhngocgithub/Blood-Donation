using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Settings")]
    public class Setting
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

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
