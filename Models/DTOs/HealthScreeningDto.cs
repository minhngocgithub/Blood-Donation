namespace Blood_Donation_Website.Models.DTOs
{
    public class HealthScreeningDto
    {
        public int ScreeningId { get; set; }
        public int RegistrationId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Hemoglobin { get; set; }
        public bool IsEligible { get; set; }
        public string? DisqualifyReason { get; set; }
        public int? ScreenedBy { get; set; }
        public DateTime ScreeningDate { get; set; }
        public string? RegistrationStatus { get; set; }
        public DateTime? CheckInTime { get; set; }
        
        // Navigation properties
        public string? UserName { get; set; }
        public string? EventName { get; set; }
        public string? ScreenedByUserName { get; set; }
    }

    public class HealthScreeningCreateDto
    {
        public int RegistrationId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Hemoglobin { get; set; }
        public bool IsEligible { get; set; } = true;
        public string? DisqualifyReason { get; set; }
        public int? ScreenedBy { get; set; }
    }

    public class HealthScreeningUpdateDto
    {
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Hemoglobin { get; set; }
        public bool IsEligible { get; set; }
        public string? DisqualifyReason { get; set; }
    }
} 