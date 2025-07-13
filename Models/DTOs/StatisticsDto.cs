namespace Blood_Donation_Website.Models.DTOs
{
    public class DashboardStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalEvents { get; set; }
        public int TotalDonations { get; set; }
        public int TotalRegistrations { get; set; }
        public int ActiveEvents { get; set; }
        public int PendingMessages { get; set; }
        public int UnreadNotifications { get; set; }
    }

    public class BloodTypeStatisticsDto
    {
        public int BloodTypeId { get; set; }
        public string BloodTypeName { get; set; } = string.Empty;
        public int TotalDonations { get; set; }
        public int TotalVolume { get; set; }
        public int UserCount { get; set; }
    }

    public class EventStatisticsDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int MaxDonors { get; set; }
        public int CurrentDonors { get; set; }
        public int CompletedDonations { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? LocationName { get; set; }
    }

    public class UserDonationHistoryDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalDonations { get; set; }
        public int TotalVolume { get; set; }
        public DateTime? LastDonationDate { get; set; }
        public DateTime? NextEligibleDate { get; set; }
        public string? BloodTypeName { get; set; }
    }
} 