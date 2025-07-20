using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.DTOs
{
    public class BloodDonationEventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string? EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? LocationId { get; set; }
        public int MaxDonors { get; set; }
        public int CurrentDonors { get; set; }
        public EventStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? RequiredBloodTypes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation properties
        public string? LocationName { get; set; }
        public string? LocationAddress { get; set; }
        public string? CreatorName { get; set; }
    }

    public class BloodDonationEventCreateDto
    {
        public string EventName { get; set; } = string.Empty;
        public string? EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? LocationId { get; set; }
        public int MaxDonors { get; set; } = 100;
        public EventStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? RequiredBloodTypes { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class BloodDonationEventUpdateDto
    {
        public string EventName { get; set; } = string.Empty;
        public string? EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? LocationId { get; set; }
        public int MaxDonors { get; set; }
        public EventStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public string? RequiredBloodTypes { get; set; }
    }
} 