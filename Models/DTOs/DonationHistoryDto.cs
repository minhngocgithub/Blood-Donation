using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.DTOs
{
    public class DonationHistoryDto
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int? RegistrationId { get; set; }
        public DateTime DonationDate { get; set; }
        public int BloodTypeId { get; set; }
        public int Volume { get; set; }
        public DonationStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? NextEligibleDate { get; set; }
        public bool CertificateIssued { get; set; }
        
        // Navigation properties
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? EventName { get; set; }
        public DateTime? EventDate { get; set; }
        public string? BloodTypeName { get; set; }
        public string? RegistrationCode { get; set; }
    }

    public class DonationHistoryCreateDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int? RegistrationId { get; set; }
        public DateTime DonationDate { get; set; }
        public int BloodTypeId { get; set; }
        public int Volume { get; set; } = 350;
        public string? Notes { get; set; }
        public DateTime? NextEligibleDate { get; set; }
    }

    public class DonationHistoryUpdateDto
    {
        public DonationStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? NextEligibleDate { get; set; }
        public bool CertificateIssued { get; set; }
    }
} 