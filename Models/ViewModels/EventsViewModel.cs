namespace Blood_Donation_Website.Models.ViewModels
{
    public class EventsViewModel
    {
        public IEnumerable<Blood_Donation_Website.Models.DTOs.BloodDonationEventDto> CurrentAndUpcomingEvents { get; set; } = new List<Blood_Donation_Website.Models.DTOs.BloodDonationEventDto>();
        public IEnumerable<Blood_Donation_Website.Models.DTOs.BloodDonationEventDto> PastEvents { get; set; } = new List<Blood_Donation_Website.Models.DTOs.BloodDonationEventDto>();
        
        // Filter properties
        public string? SearchTerm { get; set; }
        public int? LocationId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SelectedBloodType { get; set; }
        
        // Statistics
        public int TotalCurrentEvents { get; set; }
        public int TotalPastEvents { get; set; }
        
        // Helper properties
        public bool HasCurrentEvents => CurrentAndUpcomingEvents.Any();
        public bool HasPastEvents => PastEvents.Any();
        public bool HasAnyEvents => HasCurrentEvents || HasPastEvents;
    }
} 