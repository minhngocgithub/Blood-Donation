namespace Blood_Donation_Website.Models.DTOs
{
    public class ContactMessageDto
    {
        public int MessageId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public int? ResolvedBy { get; set; }
        
        // Navigation properties
        public string? ResolvedByUserName { get; set; }
    }

    public class ContactMessageCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class ContactMessageUpdateDto
    {
        public string Status { get; set; } = string.Empty;
        public DateTime? ResolvedDate { get; set; }
        public int? ResolvedBy { get; set; }
    }
} 