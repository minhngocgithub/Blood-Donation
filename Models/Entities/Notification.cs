using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int? UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Type { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}