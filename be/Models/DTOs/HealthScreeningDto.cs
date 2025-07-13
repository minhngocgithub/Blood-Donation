namespace BloodDonationAPI.Models.DTOs
{
    public class HealthScreeningDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public DateTime ScreeningDate { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int BloodPressure { get; set; }
        public int PulseRate { get; set; }
        public double Hemoglobin { get; set; }
        public string Result { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int ExaminedByUserId { get; set; }
        public string ExaminedByUserName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
