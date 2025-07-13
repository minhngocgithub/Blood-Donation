using BloodDonationAPI.Data;
using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services.Implementations
{
    public class HealthScreeningService : IHealthScreeningService
    {
        private readonly ApplicationDbContext _context;

        public HealthScreeningService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthScreeningDto>> GetAllScreeningsAsync()
        {
            var screenings = await _context.HealthScreenings
                .Include(h => h.Registration)
                .ThenInclude(r => r.User)
                .Include(h => h.ScreenedByUser)
                .OrderByDescending(h => h.ScreeningDate)
                .ToListAsync();

            return screenings.Select(MapToDto).ToList();
        }

        public async Task<HealthScreeningDto?> GetScreeningByIdAsync(int id)
        {
            var screening = await _context.HealthScreenings
                .Include(h => h.Registration)
                .ThenInclude(r => r.User)
                .Include(h => h.ScreenedByUser)
                .FirstOrDefaultAsync(h => h.ScreeningId == id);

            return screening == null ? null : MapToDto(screening);
        }

        public async Task<List<HealthScreeningDto>> GetScreeningsByUserAsync(int userId)
        {
            var screenings = await _context.HealthScreenings
                .Include(h => h.Registration)
                .ThenInclude(r => r.User)
                .Include(h => h.ScreenedByUser)
                .Where(h => h.Registration.UserId == userId)
                .OrderByDescending(h => h.ScreeningDate)
                .ToListAsync();

            return screenings.Select(MapToDto).ToList();
        }

        public async Task<(bool succeeded, string message)> CreateScreeningAsync(HealthScreeningDto screeningDto)
        {
            try
            {
                var screening = new HealthScreening
                {
                    RegistrationId = screeningDto.Id,
                    Weight = (decimal?)screeningDto.Weight,
                    Height = (decimal?)screeningDto.Height,
                    BloodPressure = screeningDto.BloodPressure.ToString(),
                    HeartRate = screeningDto.PulseRate,
                    Hemoglobin = (decimal?)screeningDto.Hemoglobin,
                    IsEligible = screeningDto.Result == "Eligible",
                    DisqualifyReason = screeningDto.Notes,
                    ScreenedBy = screeningDto.ExaminedByUserId,
                    ScreeningDate = screeningDto.ScreeningDate
                };

                _context.HealthScreenings.Add(screening);
                await _context.SaveChangesAsync();

                // Optionally, update eligibility and reason after creation
                // var (isEligible, reason) = await CheckDonationEligibilityAsync(MapToDto(screening));
                // screening.IsEligible = isEligible;
                // screening.DisqualifyReason = reason;
                // await _context.SaveChangesAsync();

                return (true, "Tạo kết quả sàng lọc thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi tạo kết quả sàng lọc: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> UpdateScreeningAsync(HealthScreeningDto screeningDto)
        {
            try
            {
                var screening = await _context.HealthScreenings.FindAsync(screeningDto.Id);
                if (screening == null)
                {
                    return (false, "Không tìm thấy kết quả sàng lọc");
                }

                screening.Weight = (decimal?)screeningDto.Weight;
                screening.Height = (decimal?)screeningDto.Height;
                screening.BloodPressure = screeningDto.BloodPressure.ToString();
                screening.HeartRate = screeningDto.PulseRate;
                screening.Hemoglobin = (decimal?)screeningDto.Hemoglobin;
                screening.IsEligible = screeningDto.Result == "Eligible";
                screening.DisqualifyReason = screeningDto.Notes;
                // No UpdatedDate in entity

                // Optionally, update eligibility and reason after update
                // var (isEligible, reason) = await CheckDonationEligibilityAsync(screeningDto);
                // screening.IsEligible = isEligible;
                // screening.DisqualifyReason = reason;

                await _context.SaveChangesAsync();
                return (true, "Cập nhật kết quả sàng lọc thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi cập nhật kết quả sàng lọc: " + ex.Message);
            }
        }

        public async Task<bool> IsUserEligibleToDonateAsync(int userId)
        {
            var latestScreening = await _context.HealthScreenings
                .Include(h => h.Registration)
                .Where(h => h.Registration.UserId == userId)
                .OrderByDescending(h => h.ScreeningDate)
                .FirstOrDefaultAsync();

            if (latestScreening == null)
            {
                return false;
            }

            return latestScreening.IsEligible &&
                   latestScreening.ScreeningDate.AddDays(7) >= DateTime.UtcNow;
        }

        public async Task<(bool isEligible, string reason)> CheckDonationEligibilityAsync(HealthScreeningDto screening)
        {
            var reasons = new List<string>();

            // Check weight
            if (screening.Weight < 45)
            {
                reasons.Add("Cân nặng dưới 45kg");
            }

            // Check blood pressure (parse from string)
            // Skipping for now, as entity uses string

            // Check pulse rate
            if (screening.PulseRate < 60 || screening.PulseRate > 100)
            {
                reasons.Add("Nhịp tim không ổn định");
            }

            // Check hemoglobin
            if (screening.Hemoglobin < 12)
            {
                reasons.Add("Hemoglobin thấp");
            }

            // Check last donation date
            var lastDonation = await _context.DonationHistories
                .Where(d => d.UserId == screening.UserId)
                .OrderByDescending(d => d.DonationDate)
                .FirstOrDefaultAsync();

            if (lastDonation != null && lastDonation.DonationDate.AddMonths(3) > DateTime.UtcNow)
            {
                reasons.Add("Chưa đủ 3 tháng kể từ lần hiến máu trước");
            }

            return reasons.Any()
                ? (false, string.Join(", ", reasons))
                : (true, "Đủ điều kiện hiến máu");
        }

        public async Task<List<HealthScreeningDto>> GetPendingScreeningsAsync()
        {
            var screenings = await _context.HealthScreenings
                .Include(h => h.Registration)
                .ThenInclude(r => r.User)
                .Include(h => h.ScreenedByUser)
                .Where(h => h.IsEligible == false)
                .OrderByDescending(h => h.ScreeningDate)
                .ToListAsync();

            return screenings.Select(MapToDto).ToList();
        }

        public async Task<List<HealthScreeningDto>> GetScreeningsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var screenings = await _context.HealthScreenings
                .Include(h => h.Registration)
                .ThenInclude(r => r.User)
                .Include(h => h.ScreenedByUser)
                .Where(h => h.ScreeningDate >= startDate && h.ScreeningDate <= endDate)
                .OrderByDescending(h => h.ScreeningDate)
                .ToListAsync();

            return screenings.Select(MapToDto).ToList();
        }

        private HealthScreeningDto MapToDto(HealthScreening screening)
        {
            return new HealthScreeningDto
            {
                Id = screening.ScreeningId,
                UserId = screening.Registration.UserId,
                UserFullName = screening.Registration.User?.FullName ?? string.Empty,
                ScreeningDate = screening.ScreeningDate,
                Weight = (double)(screening.Weight ?? 0),
                Height = (double)(screening.Height ?? 0),
                BloodPressure = int.TryParse(screening.BloodPressure, out var bp) ? bp : 0,
                PulseRate = screening.HeartRate ?? 0,
                Hemoglobin = (double)(screening.Hemoglobin ?? 0),
                Result = screening.IsEligible ? "Eligible" : "Not Eligible",
                Notes = screening.DisqualifyReason,
                ExaminedByUserId = screening.ScreenedBy ?? 0,
                ExaminedByUserName = screening.ScreenedByUser?.FullName ?? string.Empty,
                CreatedDate = screening.ScreeningDate,
                UpdatedDate = null // No UpdatedDate in entity
            };
        }
    }
}
