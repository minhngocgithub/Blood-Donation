using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}