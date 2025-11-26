using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public RoleType RoleName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class RoleCreateDto
    {
        public RoleType RoleName { get; set; }
        public string? Description { get; set; }
    }

    public class RoleUpdateDto
    {
        public RoleType RoleName { get; set; }
        public string? Description { get; set; }
    }
} 