using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.DTOs
{
    public class DonationRegistrationDto
    {
        public int RegistrationId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public RegistrationStatus Status { get; set; }
        public string? Notes { get; set; }
        public bool IsEligible { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string? CancellationReason { get; set; }

        // Navigation properties
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? EventName { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? EventStartTime { get; set; }
        public TimeSpan? EventEndTime { get; set; }
        public string? LocationName { get; set; }

        // Thêm property cho check-in
        public string? RegistrationCode { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BloodTypeName { get; set; }
    }

    public class DonationRegistrationCreateDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string? Notes { get; set; }
    }

    public class DonationRegistrationUpdateDto
    {
        public RegistrationStatus Status { get; set; }
        public string? Notes { get; set; }
        public bool IsEligible { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string? CancellationReason { get; set; }
    }
} 