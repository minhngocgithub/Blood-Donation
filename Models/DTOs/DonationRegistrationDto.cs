namespace Blood_Donation_Website.Models.DTOs
{
    public class DonationRegistrationDto
    {
        public int RegistrationId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } = string.Empty;
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
        public string? LocationName { get; set; }

        // Thêm property cho check-in
        public string? RegistrationCode { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class DonationRegistrationCreateDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string? Notes { get; set; }
    }

    public class DonationRegistrationUpdateDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public bool IsEligible { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string? CancellationReason { get; set; }
    }
} 