namespace Blood_Donation_Website.Models.DTOs
{
    public class LocationDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LocationCreateDto
    {
        public string LocationName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public int Capacity { get; set; } = 50;
        public bool IsActive { get; set; } = true;
    }

    public class LocationUpdateDto
    {
        public string LocationName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ContactPhone { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
} 