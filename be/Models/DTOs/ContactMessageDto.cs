namespace BloodDonationAPI.Models.DTOs
{
    public class ContactMessageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? Response { get; set; }
        public int? RespondedByUserId { get; set; }
        public string? RespondedByUserName { get; set; }
    }
}
