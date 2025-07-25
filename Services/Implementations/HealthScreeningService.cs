using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Implementations
{
    public class HealthScreeningService : IHealthScreeningService
    {
        private readonly ApplicationDbContext _context;

        public HealthScreeningService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<HealthScreeningDto?> GetScreeningByIdAsync(int screeningId)
        {
            try
            {
                var screening = await _context.HealthScreenings
                    .Include(h => h.Registration)
                    .Include(h => h.Registration.User)
                    .Include(h => h.Registration.Event)
                    .Include(h => h.ScreenedByUser)
                    .FirstOrDefaultAsync(h => h.ScreeningId == screeningId);

                if (screening == null) return null;

                return new HealthScreeningDto
                {
                    ScreeningId = screening.ScreeningId,
                    RegistrationId = screening.RegistrationId,
                    Weight = screening.Weight,
                    Height = screening.Height,
                    BloodPressure = screening.BloodPressure,
                    HeartRate = screening.HeartRate,
                    Temperature = screening.Temperature,
                    Hemoglobin = screening.Hemoglobin,
                    IsEligible = screening.IsEligible,
                    DisqualifyReason = screening.DisqualifyReason,
                    ScreenedBy = screening.ScreenedBy,
                    ScreeningDate = screening.ScreeningDate,
                    UserName = screening.Registration.User.FullName,
                    EventName = screening.Registration.Event.EventName,
                    ScreenedByUserName = screening.ScreenedByUser?.FullName,
                    RegistrationStatus = screening.Registration.Status.ToString(),
                    CheckInTime = screening.Registration.CheckInTime
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<HealthScreeningDto>> GetAllScreeningsAsync()
        {
            try
            {
                var screenings = await _context.HealthScreenings
                    .Include(h => h.Registration)
                    .Include(h => h.Registration.User)
                    .Include(h => h.Registration.Event)
                    .Include(h => h.ScreenedByUser)
                    .OrderByDescending(h => h.ScreeningDate)
                    .ToListAsync();

                return screenings.Select(h => new HealthScreeningDto
                {
                    ScreeningId = h.ScreeningId,
                    RegistrationId = h.RegistrationId,
                    Weight = h.Weight,
                    Height = h.Height,
                    BloodPressure = h.BloodPressure,
                    HeartRate = h.HeartRate,
                    Temperature = h.Temperature,
                    Hemoglobin = h.Hemoglobin,
                    IsEligible = h.IsEligible,
                    DisqualifyReason = h.DisqualifyReason,
                    ScreenedBy = h.ScreenedBy,
                    ScreeningDate = h.ScreeningDate,
                    UserName = h.Registration.User.FullName,
                    EventName = h.Registration.Event.EventName,
                    ScreenedByUserName = h.ScreenedByUser?.FullName,
                    RegistrationStatus = h.Registration.Status.ToString(),
                    CheckInTime = h.Registration.CheckInTime
                });
            }
            catch
            {
                return new List<HealthScreeningDto>();
            }
        }

        public async Task<HealthScreeningDto> CreateScreeningAsync(HealthScreeningDto screeningDto)
        {
            try
            {
                var screening = new HealthScreening
                {
                    RegistrationId = screeningDto.RegistrationId,
                    Weight = screeningDto.Weight,
                    Height = screeningDto.Height,
                    BloodPressure = screeningDto.BloodPressure,
                    HeartRate = screeningDto.HeartRate,
                    Temperature = screeningDto.Temperature,
                    Hemoglobin = screeningDto.Hemoglobin,
                    IsEligible = screeningDto.IsEligible,
                    DisqualifyReason = screeningDto.DisqualifyReason,
                    ScreenedBy = screeningDto.ScreenedBy,
                    ScreeningDate = screeningDto.ScreeningDate
                };

                _context.HealthScreenings.Add(screening);
                await _context.SaveChangesAsync();

                // Cập nhật trạng thái DonationRegistration theo kết quả sàng lọc
                await UpdateRegistrationStatusAfterScreeningAsync(screeningDto.RegistrationId, screeningDto.IsEligible, screeningDto.DisqualifyReason);

                return await GetScreeningByIdAsync(screening.ScreeningId) ?? screeningDto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateScreeningAsync(int screeningId, HealthScreeningDto screeningDto)
        {
            try
            {
                var screening = await _context.HealthScreenings.FindAsync(screeningId);
                if (screening == null) return false;

                screening.Weight = screeningDto.Weight;
                screening.Height = screeningDto.Height;
                screening.BloodPressure = screeningDto.BloodPressure;
                screening.HeartRate = screeningDto.HeartRate;
                screening.Temperature = screeningDto.Temperature;
                screening.Hemoglobin = screeningDto.Hemoglobin;
                screening.IsEligible = screeningDto.IsEligible;
                screening.DisqualifyReason = screeningDto.DisqualifyReason;

                await _context.SaveChangesAsync();

                // Cập nhật trạng thái DonationRegistration theo kết quả sàng lọc
                await UpdateRegistrationStatusAfterScreeningAsync(screening.RegistrationId, screeningDto.IsEligible, screeningDto.DisqualifyReason);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteScreeningAsync(int screeningId)
        {
            try
            {
                var screening = await _context.HealthScreenings.FindAsync(screeningId);
                if (screening == null) return false;

                _context.HealthScreenings.Remove(screening);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Status management - Since HealthScreening doesn't have Status property,
        // well use IsEligible to track status
        public async Task<bool> UpdateScreeningStatusAsync(int screeningId, bool isEligible)
        {
            try
            {
                var screening = await _context.HealthScreenings.FindAsync(screeningId);
                if (screening == null) return false;

                screening.IsEligible = isEligible;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Check-in functionality - Since HealthScreening doesn't have check-in properties,
        // we'll just return true for now
        public async Task<bool> CheckInScreeningAsync(int screeningId)
        {
            try
            {
                var screening = await _context.HealthScreenings.FindAsync(screeningId);
                if (screening == null) return false;

                // Since there's no check-in property, we'll just mark as eligible
                screening.IsEligible = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Query operations
        public async Task<IEnumerable<HealthScreeningDto>> GetScreeningsByStatusAsync(bool isEligible)
        {
            try
            {
                var screenings = await _context.HealthScreenings
                    .Include(h => h.Registration)
                    .Include(h => h.Registration.User)
                    .Include(h => h.Registration.Event)
                    .Include(h => h.ScreenedByUser)
                    .Where(h => h.IsEligible == isEligible)
                    .OrderByDescending(h => h.ScreeningDate)
                    .ToListAsync();

                return screenings.Select(h => new HealthScreeningDto
                {
                    ScreeningId = h.ScreeningId,
                    RegistrationId = h.RegistrationId,
                    Weight = h.Weight,
                    Height = h.Height,
                    BloodPressure = h.BloodPressure,
                    HeartRate = h.HeartRate,
                    Temperature = h.Temperature,
                    Hemoglobin = h.Hemoglobin,
                    IsEligible = h.IsEligible,
                    DisqualifyReason = h.DisqualifyReason,
                    ScreenedBy = h.ScreenedBy,
                    ScreeningDate = h.ScreeningDate,
                    UserName = h.Registration.User.FullName,
                    EventName = h.Registration.Event.EventName,
                    ScreenedByUserName = h.ScreenedByUser?.FullName,
                    RegistrationStatus = h.Registration.Status.ToString(),
                    CheckInTime = h.Registration.CheckInTime
                });
            }
            catch
            {
                return new List<HealthScreeningDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetPendingScreeningsAsync()
        {
            try
            {
                // Lấy các đăng ký đã check-in nhưng chưa có sàng lọc sức khỏe
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.Status == RegistrationStatus.CheckedIn && !r.IsEligible)
                    .OrderByDescending(r => r.CheckInTime)
                    .ToListAsync();

                return registrations.Select(r => new DonationRegistrationDto
                {
                    RegistrationId = r.RegistrationId,
                    UserId = r.UserId,
                    EventId = r.EventId,
                    RegistrationDate = r.RegistrationDate,
                    Status = r.Status,
                    Notes = r.Notes,
                    IsEligible = r.IsEligible,
                    CheckInTime = r.CheckInTime,
                    CompletionTime = r.CompletionTime,
                    CancellationReason = r.CancellationReason,
                    UserName = r.User?.FullName,
                    UserEmail = r.User?.Email,
                    EventName = r.Event?.EventName,
                    EventDate = r.Event?.EventDate,
                    LocationName = r.Event?.Location?.LocationName,
                    FullName = r.User?.FullName,
                    RegistrationCode = r.RegistrationId.ToString(),
                    PhoneNumber = r.User?.Phone
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<HealthScreeningDto>> GetScreeningsByUserAsync(int userId)
        {
            try
            {
                var screenings = await _context.HealthScreenings
                    .Include(h => h.Registration)
                    .Include(h => h.Registration.User)
                    .Include(h => h.Registration.Event)
                    .Include(h => h.ScreenedByUser)
                    .Where(h => h.Registration.UserId == userId)
                    .OrderByDescending(h => h.ScreeningDate)
                    .ToListAsync();

                return screenings.Select(h => new HealthScreeningDto
                {
                    ScreeningId = h.ScreeningId,
                    RegistrationId = h.RegistrationId,
                    Weight = h.Weight,
                    Height = h.Height,
                    BloodPressure = h.BloodPressure,
                    HeartRate = h.HeartRate,
                    Temperature = h.Temperature,
                    Hemoglobin = h.Hemoglobin,
                    IsEligible = h.IsEligible,
                    DisqualifyReason = h.DisqualifyReason,
                    ScreenedBy = h.ScreenedBy,
                    ScreeningDate = h.ScreeningDate,
                    UserName = h.Registration.User.FullName,
                    EventName = h.Registration.Event.EventName,
                    ScreenedByUserName = h.ScreenedByUser?.FullName,
                    RegistrationStatus = h.Registration.Status.ToString(),
                    CheckInTime = h.Registration.CheckInTime
                });
            }
            catch
            {
                return new List<HealthScreeningDto>();
            }
        }

        public async Task<IEnumerable<HealthScreeningDto>> GetScreeningsByEventAsync(int eventId)
        {
            try
            {
                var screenings = await _context.HealthScreenings
                    .Include(h => h.Registration)
                    .Include(h => h.Registration.User)
                    .Include(h => h.Registration.Event)
                    .Include(h => h.ScreenedByUser)
                    .Where(h => h.Registration.EventId == eventId)
                    .OrderByDescending(h => h.ScreeningDate)
                    .ToListAsync();

                return screenings.Select(h => new HealthScreeningDto
                {
                    ScreeningId = h.ScreeningId,
                    RegistrationId = h.RegistrationId,
                    Weight = h.Weight,
                    Height = h.Height,
                    BloodPressure = h.BloodPressure,
                    HeartRate = h.HeartRate,
                    Temperature = h.Temperature,
                    Hemoglobin = h.Hemoglobin,
                    IsEligible = h.IsEligible,
                    DisqualifyReason = h.DisqualifyReason,
                    ScreenedBy = h.ScreenedBy,
                    ScreeningDate = h.ScreeningDate,
                    UserName = h.Registration.User.FullName,
                    EventName = h.Registration.Event.EventName,
                    ScreenedByUserName = h.ScreenedByUser?.FullName,
                    RegistrationStatus = h.Registration.Status.ToString(),
                    CheckInTime = h.Registration.CheckInTime
                });
            }
            catch
            {
                return new List<HealthScreeningDto>();
            }
        }

        public async Task<HealthScreeningDto?> GetLatestScreeningByRegistrationIdAsync(int registrationId)
        {
            try
            {
                var screening = await _context.HealthScreenings
                    .Include(h => h.Registration)
                    .Include(h => h.Registration.User)
                    .Include(h => h.Registration.Event)
                    .Include(h => h.ScreenedByUser)
                    .Where(h => h.RegistrationId == registrationId)
                    .OrderByDescending(h => h.ScreeningDate)
                    .FirstOrDefaultAsync();

                if (screening == null) return null;

                return new HealthScreeningDto
                {
                    ScreeningId = screening.ScreeningId,
                    RegistrationId = screening.RegistrationId,
                    Weight = screening.Weight,
                    Height = screening.Height,
                    BloodPressure = screening.BloodPressure,
                    HeartRate = screening.HeartRate,
                    Temperature = screening.Temperature,
                    Hemoglobin = screening.Hemoglobin,
                    IsEligible = screening.IsEligible,
                    DisqualifyReason = screening.DisqualifyReason,
                    ScreenedBy = screening.ScreenedBy,
                    ScreeningDate = screening.ScreeningDate,
                    UserName = screening.Registration.User.FullName,
                    EventName = screening.Registration.Event.EventName,
                    ScreenedByUserName = screening.ScreenedByUser?.FullName,
                    RegistrationStatus = screening.Registration.Status.ToString(),
                    CheckInTime = screening.Registration.CheckInTime
                };
            }
            catch
            {
                return null;
            }
        }

        // Statistics
        public async Task<HealthScreeningStatisticsDto> GetScreeningStatisticsAsync()
        {
            var all = await _context.HealthScreenings.ToListAsync();
            var total = all.Count;
            var eligible = all.Count(x => x.IsEligible);
            var ineligible = all.Count(x => !x.IsEligible);
            var topReasons = all.Where(x => !x.IsEligible && !string.IsNullOrEmpty(x.DisqualifyReason.ToString()))
                .GroupBy(x => x.DisqualifyReason)
                .Select(g => new ReasonStat { Reason = g.Key.ToString()!, Count = g.Count() })
                .OrderByDescending(x => x.Count).Take(5).ToList();
            var bloodTypeStats = all
                .Where(x => x.Registration != null && x.Registration.User != null && x.Registration.User.BloodType != null && !string.IsNullOrEmpty(x.Registration.User.BloodType.BloodTypeName))
                .GroupBy(x => x.Registration.User.BloodType!.BloodTypeName)
                .Select(g => new BloodTypeStat { BloodType = g.Key!, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();
            return new HealthScreeningStatisticsDto
            {
                TotalScreenings = total,
                EligibleCount = eligible,
                IneligibleCount = ineligible,
                TopDisqualifyReasons = topReasons,
                BloodTypeStats = bloodTypeStats
            };
        }

        // Validation
        public async Task<bool> IsEligibleForDonationAsync(int screeningId)
        {
            try
            {
                var screening = await _context.HealthScreenings.FindAsync(screeningId);
                if (screening == null) return false;

                // Basic eligibility checks
                var isEligible = screening.IsEligible &&
                               screening.Weight >= 45m && // Minimum weight requirement
                               screening.Height > 0m && // Must have height recorded
                               !string.IsNullOrEmpty(screening.BloodPressure) &&
                               screening.HeartRate >= 50 && screening.HeartRate <= 100 &&
                               screening.Temperature >= 36.5m && screening.Temperature <= 37.5m &&
                               screening.Hemoglobin >= 12.5m;

                return isEligible;
            }
            catch
            {
                return false;
            }
        }

        // Helper method để cập nhật trạng thái DonationRegistration sau sàng lọc
        private async Task UpdateRegistrationStatusAfterScreeningAsync(int registrationId, bool isEligible, DisqualificationReason? disqualifyReason)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return;

                if (isEligible)
                {
                    // Đạt -> Status = 'Eligible', IsEligible = 1
                    registration.Status = RegistrationStatus.Eligible;
                    registration.IsEligible = true;
                }
                else
                {
                    // Không đạt -> Status = 'Ineligible', IsEligible = 0
                    registration.Status = RegistrationStatus.Ineligible;
                    registration.IsEligible = false;
                    // Có thể lưu lý do không đạt vào Notes nếu cần
                    registration.Notes = disqualifyReason?.ToString() ?? string.Empty;
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                // Log error if needed
            }
        }
    }
} 