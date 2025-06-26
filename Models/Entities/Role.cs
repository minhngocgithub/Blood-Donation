using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        // Navigation Properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}