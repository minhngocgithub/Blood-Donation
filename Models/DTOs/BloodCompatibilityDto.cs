namespace Blood_Donation_Website.Models.DTOs
{
    public class BloodCompatibilityDto
    {
        public int Id { get; set; }
        public int FromBloodTypeId { get; set; }
        public int ToBloodTypeId { get; set; }
        
        // Navigation properties
        public string? FromBloodTypeName { get; set; }
        public string? ToBloodTypeName { get; set; }
    }

    public class BloodCompatibilityCreateDto
    {
        public int FromBloodTypeId { get; set; }
        public int ToBloodTypeId { get; set; }
    }

    public class BloodCompatibilityUpdateDto
    {
        public int FromBloodTypeId { get; set; }
        public int ToBloodTypeId { get; set; }
    }
} 