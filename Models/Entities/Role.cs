using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("Roles")]
    [Index(nameof(RoleName), IsUnique = true)]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public RoleType RoleName { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}