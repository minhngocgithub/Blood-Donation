using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IStatisticsService
    {
        // Dashboard statistics
        Task<DashboardStatisticsDto> GetDashboardStatisticsAsync();
        Task<DashboardStatisticsDto> GetDashboardStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Blood type statistics
        Task<IEnumerable<BloodTypeStatisticsDto>> GetAllBloodTypeStatisticsAsync();
        Task<BloodTypeStatisticsDto> GetBloodTypeStatisticsAsync(int bloodTypeId);
        Task<Dictionary<string, int>> GetBloodTypeDonationChartAsync();
        Task<Dictionary<string, int>> GetBloodTypeUserChartAsync();
        
        // Event statistics
        Task<IEnumerable<EventStatisticsDto>> GetAllEventStatisticsAsync();
        Task<EventStatisticsDto> GetEventStatisticsAsync(int eventId);
        Task<Dictionary<string, int>> GetEventPerformanceChartAsync();
        Task<Dictionary<string, int>> GetEventRegistrationChartAsync();
        
        // User statistics
        Task<IEnumerable<UserDonationHistoryDto>> GetTopDonorsAsync(int count = 10);
        Task<IEnumerable<UserDonationHistoryDto>> GetRecentDonorsAsync(int count = 10);
        Task<Dictionary<string, int>> GetUserRegistrationChartAsync();
        Task<Dictionary<string, int>> GetUserDonationChartAsync();
        
        // Donation statistics
        Task<int> GetTotalDonationsAsync();
        Task<int> GetTotalDonationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetTotalVolumeAsync();
        Task<int> GetTotalVolumeByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, int>> GetDonationTrendChartAsync();
        Task<Dictionary<string, int>> GetDonationVolumeChartAsync();
        
        // Registration statistics
        Task<int> GetTotalRegistrationsAsync();
        Task<int> GetTotalRegistrationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, int>> GetRegistrationTrendChartAsync();
        Task<Dictionary<string, int>> GetRegistrationStatusChartAsync();
        
        // Health screening statistics
        Task<int> GetTotalScreeningsAsync();
        Task<int> GetEligibleScreeningsCountAsync();
        Task<int> GetDisqualifiedScreeningsCountAsync();
        Task<Dictionary<string, int>> GetScreeningResultsChartAsync();
        Task<Dictionary<string, decimal>> GetAverageVitalsChartAsync();
        
        // Location statistics
        Task<Dictionary<string, int>> GetLocationEventChartAsync();
        Task<Dictionary<string, int>> GetLocationDonationChartAsync();
        Task<Dictionary<string, int>> GetLocationCapacityChartAsync();
        
        // Time-based statistics
        Task<Dictionary<string, int>> GetStatisticsByMonthAsync(int year);
        Task<Dictionary<string, int>> GetStatisticsByYearAsync(int startYear, int endYear);
        Task<Dictionary<string, int>> GetStatisticsByWeekAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<string, int>> GetStatisticsByDayAsync(DateTime startDate, DateTime endDate);
        
        // Comparative statistics
        Task<Dictionary<string, object>> ComparePeriodsAsync(DateTime period1Start, DateTime period1End, DateTime period2Start, DateTime period2End);
        Task<Dictionary<string, object>> CompareLocationsAsync(List<int> locationIds);
        Task<Dictionary<string, object>> CompareBloodTypesAsync(List<int> bloodTypeIds);
        
        // Growth statistics
        Task<Dictionary<string, double>> GetGrowthRatesAsync();
        Task<Dictionary<string, double>> GetGrowthRatesByBloodTypeAsync();
        Task<Dictionary<string, double>> GetGrowthRatesByLocationAsync();
        
        // Efficiency statistics
        Task<Dictionary<string, double>> GetRegistrationToDonationRateAsync();
        Task<Dictionary<string, double>> GetScreeningPassRateAsync();
        Task<Dictionary<string, double>> GetEventCapacityUtilizationAsync();
        
        // Predictive statistics
        Task<Dictionary<string, int>> PredictDonationsAsync(int monthsAhead);
        Task<Dictionary<string, int>> PredictRegistrationsAsync(int monthsAhead);
        Task<Dictionary<string, int>> PredictBloodTypeNeedsAsync(int monthsAhead);
        
        // Export statistics
        Task<byte[]> ExportStatisticsToExcelAsync(DateTime startDate, DateTime endDate);
        Task<byte[]> ExportStatisticsToPdfAsync(DateTime startDate, DateTime endDate);
        Task<string> ExportStatisticsToCsvAsync(DateTime startDate, DateTime endDate);
        
        // Real-time statistics
        Task<Dictionary<string, int>> GetRealTimeStatisticsAsync();
        Task<Dictionary<string, int>> GetTodayStatisticsAsync();
        Task<Dictionary<string, int>> GetThisWeekStatisticsAsync();
        Task<Dictionary<string, int>> GetThisMonthStatisticsAsync();
        
        // Custom statistics
        Task<Dictionary<string, object>> GetCustomStatisticsAsync(Dictionary<string, object> parameters);
        Task<Dictionary<string, object>> GetFilteredStatisticsAsync(SearchParametersDto searchParams);
    }
} 