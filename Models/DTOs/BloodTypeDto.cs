namespace Blood_Donation_Website.Models.DTOs
{
    public class BloodTypeDto
    {
        public int BloodTypeId { get; set; }
        public string BloodTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class BloodTypeCreateDto
    {
        public string BloodTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class BloodTypeUpdateDto
    {
        public string BloodTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
} 