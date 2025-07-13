namespace Blood_Donation_Website.Models.DTOs
{
    public class SettingDto
    {
        public int SettingId { get; set; }
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class SettingCreateDto
    {
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class SettingUpdateDto
    {
        public string SettingValue { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
} 