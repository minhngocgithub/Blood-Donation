using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.DTOs
{
    public class ContactMessageStatisticsDto
    {
        public int Total { get; set; }
        public int Unread { get; set; }
        public int Read { get; set; }
        public int Resolved { get; set; }
        public int InProgress { get; set; }
        public int Closed { get; set; }
        public double ResolutionRate { get; set; }
        public List<StatusStatDto> StatusStats { get; set; } = new List<StatusStatDto>();
    }

    public class StatusStatDto
    {
        public MessageStatus Status { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
} 