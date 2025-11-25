namespace Blood_Donation_Website.Models.DTOs
{
    public class HealthScreeningStatisticsDto
    {
        public int TotalScreenings { get; set; }
        public int EligibleCount { get; set; }
        public int IneligibleCount { get; set; }
        public List<ReasonStat> TopDisqualifyReasons { get; set; } = new();
        public List<BloodTypeStat> BloodTypeStats { get; set; } = new();
    }

    public class ReasonStat
    {
        public string Reason { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class BloodTypeStat
    {
        public string BloodType { get; set; } = string.Empty;
        public int Count { get; set; }
    }
} 