namespace Blood_Donation_Website.Models.ViewModels
{
    public class RegistrationStatisticsViewModel
    {
        public int TotalRegistrations { get; set; }
        public int CompletedRegistrations { get; set; }
        public int PendingRegistrations { get; set; }
        public int ApprovedRegistrations { get; set; }
        public int CancelledRegistrations { get; set; }
        public int RejectedRegistrations { get; set; }
        public int CheckedInRegistrations { get; set; }
        
        public double CompletionRate => TotalRegistrations > 0 ? Math.Round((double)CompletedRegistrations / TotalRegistrations * 100, 1) : 0;
        public double PendingRate => TotalRegistrations > 0 ? Math.Round((double)PendingRegistrations / TotalRegistrations * 100, 1) : 0;
        public double ApprovedRate => TotalRegistrations > 0 ? Math.Round((double)ApprovedRegistrations / TotalRegistrations * 100, 1) : 0;
        public double CancelledRate => TotalRegistrations > 0 ? Math.Round((double)CancelledRegistrations / TotalRegistrations * 100, 1) : 0;
        public double RejectedRate => TotalRegistrations > 0 ? Math.Round((double)RejectedRegistrations / TotalRegistrations * 100, 1) : 0;
        public double CheckedInRate => TotalRegistrations > 0 ? Math.Round((double)CheckedInRegistrations / TotalRegistrations * 100, 1) : 0;
    }
} 