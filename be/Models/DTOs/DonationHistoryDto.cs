namespace BloodDonationAPI.Models.DTOs
{
    public class DonationHistoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime DonationDate { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int? HealthScreeningId { get; set; }
        public string? HealthScreeningResult { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
