using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Implementations
{
    public class DonationRegistrationService : IDonationRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public DonationRegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DonationRegistrationDto>> SearchRegistrationsForCheckinAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return new List<DonationRegistrationDto>();

            var query = _context.DonationRegistrations
                .Include(r => r.User)
                .Include(r => r.Event)
                .ThenInclude(e => e.Location)
                .Where(r => r.RegistrationId.ToString() == code || r.User.Phone == code);

            var registrations = await query.OrderByDescending(r => r.RegistrationDate).ToListAsync();

            return registrations.Select(r => new DonationRegistrationDto
            {
                RegistrationId = r.RegistrationId,
                FullName = r.User?.FullName,
                RegistrationCode = r.RegistrationId.ToString(),
                PhoneNumber = r.User?.Phone,
                RegistrationDate = r.RegistrationDate,
                Status = r.Status,
                EventName = r.Event?.EventName,
                EventDate = r.Event?.EventDate,
                EventStartTime = r.Event?.StartTime,
                EventEndTime = r.Event?.EndTime
            });
        }

        public async Task<bool> CheckinRegistrationAsync(int registrationId)
        {
            var registration = await _context.DonationRegistrations.FindAsync(registrationId);
            if (registration == null) return false;
            if (registration.Status == RegistrationStatus.CheckedIn) return false;
            
            // Check-in được thực hiện trước khi sàng lọc sức khỏe
            // Theo quy trình: Status = 'CheckedIn', IsEligible = 0, CheckInTime = [thời gian]
            registration.Status = RegistrationStatus.CheckedIn;
            registration.IsEligible = false; // Đảm bảo IsEligible = 0 (chưa sàng lọc sức khỏe)
            registration.CheckInTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // Basic CRUD operations
        public async Task<DonationRegistrationDto?> GetRegistrationByIdAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

                if (registration == null) return null;

                return new DonationRegistrationDto
                {
                    RegistrationId = registration.RegistrationId,
                    UserId = registration.UserId,
                    EventId = registration.EventId,
                    RegistrationDate = registration.RegistrationDate,
                    Status = registration.Status,
                    Notes = registration.Notes,
                    IsEligible = registration.IsEligible,
                    CheckInTime = registration.CheckInTime,
                    CompletionTime = registration.CompletionTime,
                    CancellationReason = registration.CancellationReason,
                    UserName = registration.User?.FullName,
                    UserEmail = registration.User?.Email,
                    EventName = registration.Event?.EventName,
                    EventDate = registration.Event?.EventDate,
                    EventStartTime = registration.Event?.StartTime,
                    EventEndTime = registration.Event?.EndTime,
                    LocationName = registration.Event?.Location?.LocationName
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetAllRegistrationsAsync()
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    EventStartTime = r.Event?.StartTime,
                    EventEndTime = r.Event?.EndTime,
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

        public async Task<PagedResponseDto<DonationRegistrationDto>> GetRegistrationsPagedAsync(SearchParametersDto searchDto)
        {
            try
            {
                var query = _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                {
                    query = query.Where(r =>
                        r.User.FullName.Contains(searchDto.SearchTerm) ||
                        r.User.Email.Contains(searchDto.SearchTerm) ||
                        r.Event.EventName.Contains(searchDto.SearchTerm) ||
                        (r.Notes != null && r.Notes.Contains(searchDto.SearchTerm)));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(searchDto.SortBy))
                {
                    query = searchDto.SortBy.ToLower() switch
                    {
                        "registrationdate" => searchDto.SortOrder == "desc" ? query.OrderByDescending(r => r.RegistrationDate) : query.OrderBy(r => r.RegistrationDate),
                        "status" => searchDto.SortOrder == "desc" ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status),
                        "username" => searchDto.SortOrder == "desc" ? query.OrderByDescending(r => r.User.FullName) : query.OrderBy(r => r.User.FullName),
                        _ => query.OrderByDescending(r => r.RegistrationDate)
                    };
                }
                else
                {
                    query = query.OrderByDescending(r => r.RegistrationDate);
                }

                var totalCount = await query.CountAsync();
                var pageSize = searchDto.PageSize ?? 10;
                var pageNumber = searchDto.Page ?? 1;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var registrations = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var registrationDtos = registrations.Select(r => new DonationRegistrationDto
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
                    LocationName = r.Event?.Location?.LocationName
                }).ToList();

                return new PagedResponseDto<DonationRegistrationDto>
                {
                    Items = registrationDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages
                };
            }
            catch
            {
                return new PagedResponseDto<DonationRegistrationDto>
                {
                    Items = new List<DonationRegistrationDto>(),
                    TotalCount = 0,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };
            }
        }

        public async Task<DonationRegistrationDto> CreateRegistrationAsync(DonationRegistrationCreateDto createDto)
        {
            try
            {
                // Lấy thông tin sự kiện
                var eventEntity = await _context.BloodDonationEvents.FindAsync(createDto.EventId);
                if (eventEntity == null)
                    throw new InvalidOperationException("Event does not exist");

                var now = DateTime.Now;
                var eventDate = eventEntity.EventDate.Date;
                var eventStart = eventDate + eventEntity.StartTime;
                var eventEnd = eventDate + eventEntity.EndTime;

                // Chỉ cho phép đăng ký nếu sự kiện ở trạng thái Published hoặc Active và chưa bắt đầu
                if (!(eventEntity.Status == EventStatus.Published || eventEntity.Status == EventStatus.Active))
                    throw new InvalidOperationException("Sự kiện không được phép đăng ký");
                if (now >= eventStart)
                    throw new InvalidOperationException("Đăng ký sự kiện đã đóng (sự kiện đã bắt đầu)");
                if (eventEntity.EventDate < now.Date)
                    throw new InvalidOperationException("Ngày sự kiện đã qua");

                // 0. Đã đăng ký sự kiện này trước đó
                if (await IsUserRegisteredForEventAsync(createDto.UserId, createDto.EventId))
                {
                    return new DonationRegistrationDto
                    {
                        Status = RegistrationStatus.Cancelled,
                        CancellationReason = "Bạn đã đăng ký sự kiện này trước đó"
                    };
                }

                // Sau đó mới tạo bản ghi mới nếu không trùng
                var registration = new DonationRegistration
                {
                    UserId = createDto.UserId,
                    EventId = createDto.EventId,
                    RegistrationDate = DateTime.Now,
                    Status = RegistrationStatus.Registered,
                    Notes = createDto.Notes,
                    IsEligible = false
                };
                _context.DonationRegistrations.Add(registration);
                await _context.SaveChangesAsync();

                // Kiểm tra các điều kiện sau khi đăng ký
                var user = await _context.Users.Include(u => u.BloodType).FirstOrDefaultAsync(u => u.UserId == createDto.UserId);
                if (user == null)
                    throw new InvalidOperationException("Người dùng không tồn tại");

                // 1. Sự kiện đã đầy
                if (eventEntity.CurrentDonors >= eventEntity.MaxDonors)
                {
                    registration.Status = RegistrationStatus.Cancelled;
                    registration.CancellationReason = "Sự kiện đã đầy";
                    await _context.SaveChangesAsync();
                    return await GetRegistrationByIdAsync(registration.RegistrationId) ?? new DonationRegistrationDto();
                }

                // 2. Nhóm máu không phù hợp
                if (!string.IsNullOrEmpty(eventEntity.RequiredBloodTypes) &&
                    !string.IsNullOrEmpty(user.BloodType?.BloodTypeName) &&
                    !eventEntity.RequiredBloodTypes.Contains(user.BloodType.BloodTypeName))
                {
                    registration.Status = RegistrationStatus.Cancelled;
                    registration.CancellationReason = "Nhóm máu không phù hợp";
                    await _context.SaveChangesAsync();
                    return await GetRegistrationByIdAsync(registration.RegistrationId) ?? new DonationRegistrationDto();
                }

                // 3. Hiến máu gần đây (<56 ngày)
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == user.UserId)
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();
                if (lastDonation != null)
                {
                    var daysSinceLastDonation = (DateTime.Now - lastDonation.DonationDate).Days;
                    if (daysSinceLastDonation < 56)
                    {
                        registration.Status = RegistrationStatus.Deferred;
                        registration.CancellationReason = "Bạn đã hiến máu gần đây";
                        await _context.SaveChangesAsync();
                        return await GetRegistrationByIdAsync(registration.RegistrationId) ?? new DonationRegistrationDto();
                    }
                }

                // 4. Tài khoản bị khoá
                if (!user.IsActive)
                {
                    registration.Status = RegistrationStatus.Cancelled;
                    registration.CancellationReason = "Tài khoản bị khóa";
                    await _context.SaveChangesAsync();
                    return await GetRegistrationByIdAsync(registration.RegistrationId) ?? new DonationRegistrationDto();
                }

                // 5. Nếu thỏa mãn hết, chuyển sang Confirmed
                registration.Status = RegistrationStatus.Confirmed;
                await _context.SaveChangesAsync();

                // Tăng số lượng CurrentDonors nếu đăng ký thành công
                await IncrementEventCurrentDonorsAsync(createDto.EventId);

                return await GetRegistrationByIdAsync(registration.RegistrationId) ?? new DonationRegistrationDto();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateRegistrationAsync(int registrationId, DonationRegistrationUpdateDto updateDto)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = updateDto.Status;
                registration.Notes = updateDto.Notes;
                registration.IsEligible = updateDto.IsEligible;
                registration.CheckInTime = updateDto.CheckInTime;
                registration.CompletionTime = updateDto.CompletionTime;
                registration.CancellationReason = updateDto.CancellationReason;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRegistrationAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                _context.DonationRegistrations.Remove(registration);
                await _context.SaveChangesAsync();

                // Decrement current donors count
                await DecrementEventCurrentDonorsAsync(registration.EventId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Registration status operations
        public async Task<bool> ApproveRegistrationAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.Confirmed;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Xác nhận đăng ký (Confirm) - theo quy trình
        public async Task<bool> ConfirmRegistrationAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);
                
                if (registration == null) return false;
                if (registration.Status != RegistrationStatus.Registered) return false;

                // Kiểm tra các điều kiện theo quy trình
                if (registration.Event.CurrentDonors >= registration.Event.MaxDonors)
                {
                    return false; // Sự kiện đầy
                }

                if (registration.Event.RequiredBloodTypes != null && 
                    !string.IsNullOrEmpty(registration.User.BloodType?.BloodTypeName) &&
                    !registration.Event.RequiredBloodTypes.Contains(registration.User.BloodType.BloodTypeName))
                {
                    return false; // Nhóm máu không phù hợp
                }

                // Kiểm tra lần hiến máu gần đây (56 ngày)
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == registration.UserId)
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                if (lastDonation != null)
                {
                    var daysSinceLastDonation = (DateTime.Now - lastDonation.DonationDate).Days;
                    if (daysSinceLastDonation < 56) return false; // Gần đây hiến máu
                }

                if (!registration.User.IsActive) return false; // Tài khoản bị khóa

                // Xác nhận đăng ký
                registration.Status = RegistrationStatus.Confirmed;
                registration.IsEligible = false; // Theo quy trình: IsEligible = 0 (chưa sàng lọc sức khỏe)
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectRegistrationAsync(int registrationId, DisqualificationReason reason)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.Cancelled;
                registration.CancellationReason = reason.ToString();
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelRegistrationAsync(int registrationId, DisqualificationReason reason)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.Cancelled;
                registration.CancellationReason = reason.ToString();
                await _context.SaveChangesAsync();

                // Decrement current donors count
                await DecrementEventCurrentDonorsAsync(registration.EventId);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckInRegistrationAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.CheckedIn;
                registration.CheckInTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CompleteRegistrationAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.Completed;
                registration.CompletionTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật trạng thái bắt đầu hiến máu
        public async Task<bool> StartDonatingAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;
                if (registration.Status != RegistrationStatus.Eligible) return false; // Chỉ có thể bắt đầu hiến khi đã đủ điều kiện

                registration.Status = RegistrationStatus.Donating;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật trạng thái thất bại
        public async Task<bool> MarkAsFailedAsync(int registrationId, DisqualificationReason? reason = null)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.Failed;
                registration.Notes = reason?.ToString() ?? string.Empty;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật trạng thái không đến
        public async Task<bool> MarkAsNoShowAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = RegistrationStatus.NoShow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetRegistrationStatusAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                return registration?.Status.ToString() ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        // Registration queries
        public async Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByUserAsync(int userId)
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByEventAsync(int eventId)
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.EventId == eventId)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByStatusAsync(RegistrationStatus status)
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.Status == status)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    RegistrationCode = r.RegistrationId.ToString(),
                    FullName = r.User?.FullName,
                    PhoneNumber = r.User?.Phone
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.RegistrationDate >= startDate && r.RegistrationDate <= endDate)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<DonationRegistrationDto?> GetUserRegistrationForEventAsync(int userId, int eventId)
        {
            try
            {
                // Chỉ lấy bản ghi có trạng thái còn hiệu lực, mới nhất
                var validStatuses = new[] {
                    RegistrationStatus.Registered,
                    RegistrationStatus.Confirmed,
                    RegistrationStatus.CheckedIn,
                    RegistrationStatus.Screening,
                    RegistrationStatus.Eligible,
                    RegistrationStatus.Donating,
                    RegistrationStatus.Completed
                };
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.UserId == userId && r.EventId == eventId && validStatuses.Contains(r.Status))
                    .OrderByDescending(r => r.RegistrationDate)
                    .FirstOrDefaultAsync();

                if (registration == null) return null;

                return new DonationRegistrationDto
                {
                    RegistrationId = registration.RegistrationId,
                    UserId = registration.UserId,
                    EventId = registration.EventId,
                    RegistrationDate = registration.RegistrationDate,
                    Status = registration.Status,
                    Notes = registration.Notes,
                    IsEligible = registration.IsEligible,
                    CheckInTime = registration.CheckInTime,
                    CompletionTime = registration.CompletionTime,
                    CancellationReason = registration.CancellationReason,
                    UserName = registration.User?.FullName,
                    UserEmail = registration.User?.Email,
                    EventName = registration.Event?.EventName,
                    EventDate = registration.Event?.EventDate,
                    LocationName = registration.Event?.Location?.LocationName
                };
            }
            catch
            {
                return null;
            }
        }

        // Registration validation
        public async Task<bool> IsRegistrationExistsAsync(int registrationId)
        {
            try
            {
                return await _context.DonationRegistrations.AnyAsync(r => r.RegistrationId == registrationId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserRegisteredForEventAsync(int userId, int eventId)
        {
            try
            {
                // Chỉ tính các trạng thái còn hiệu lực
                var validStatuses = new[] { RegistrationStatus.Registered, RegistrationStatus.Confirmed, RegistrationStatus.CheckedIn, RegistrationStatus.Completed };
                return await _context.DonationRegistrations
                    .AnyAsync(r => r.UserId == userId && r.EventId == eventId && validStatuses.Contains(r.Status));
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserEligibleForEventAsync(int userId, int eventId)
        {
            try
            {
                // Check if user exists and is active
                var user = await _context.Users.FindAsync(userId);
                if (user == null || !user.IsActive) return false;

                // Check if event exists and is active
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null || eventEntity.Status != EventStatus.Active) return false;

                // Check if event date is in the future
                if (eventEntity.EventDate <= DateTime.Now) return false;

                // Check if user has donated recently (56 days rule)
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                if (lastDonation != null)
                {
                    var daysSinceLastDonation = (DateTime.Now - lastDonation.DonationDate).Days;
                    if (daysSinceLastDonation < 56) return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsEventFullAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                return eventEntity?.CurrentDonors >= eventEntity?.MaxDonors;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsRegistrationDateValidAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                return eventEntity?.EventDate > DateTime.Now;
            }
            catch
            {
                return false;
            }
        }

        // Registration statistics
        public async Task<int> GetRegistrationCountByEventAsync(int eventId)
        {
            try
            {
                return await _context.DonationRegistrations
                    .Where(r => r.EventId == eventId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetRegistrationCountByUserAsync(int userId)
        {
            try
            {
                return await _context.DonationRegistrations
                    .Where(r => r.UserId == userId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetRegistrationCountByStatusAsync(RegistrationStatus status)
        {
            try
            {
                return await _context.DonationRegistrations
                    .Where(r => r.Status == status)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetRegistrationCountByUserAndStatusAsync(int userId, RegistrationStatus status)
        {
            try
            {
                return await _context.DonationRegistrations
                    .Where(r => r.UserId == userId && r.Status == status)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetRegistrationCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.DonationRegistrations
                    .Where(r => r.RegistrationDate >= startDate && r.RegistrationDate <= endDate)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        // Registration search and filtering
        public async Task<IEnumerable<DonationRegistrationDto>> SearchRegistrationsAsync(string searchTerm)
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.User.FullName.Contains(searchTerm) ||
                               r.User.Email.Contains(searchTerm) ||
                               r.Event.EventName.Contains(searchTerm) ||
                               (r.Notes != null && r.Notes.Contains(searchTerm)))
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.User.BloodTypeId == bloodTypeId)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetPendingRegistrationsAsync()
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.Status == RegistrationStatus.Registered)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        public async Task<IEnumerable<DonationRegistrationDto>> GetApprovedRegistrationsAsync()
        {
            try
            {
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => r.Status == RegistrationStatus.Confirmed)
                    .OrderByDescending(r => r.RegistrationDate)
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
                    LocationName = r.Event?.Location?.LocationName
                });
            }
            catch
            {
                return new List<DonationRegistrationDto>();
            }
        }

        // Registration notifications
        public async Task<bool> SendRegistrationConfirmationAsync(int registrationId)
        {
            try
            {
                // Get registration details for email content
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

                if (registration == null)
                {
                    return false;
                }

                // Simulate async operation
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendRegistrationReminderAsync(int registrationId)
        {
            try
            {
                // Get registration details for reminder content
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

                if (registration == null)
                {
                    return false;
                }

                // Simulate async operation
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendRegistrationStatusUpdateAsync(int registrationId)
        {
            try
            {
                // Get registration details for status update content
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

                if (registration == null)
                {
                    return false;
                }

                // Simulate async operation
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Registration health screening
        public async Task<bool> HasHealthScreeningAsync(int registrationId)
        {
            try
            {
                return await _context.HealthScreenings
                    .AnyAsync(h => h.RegistrationId == registrationId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsHealthScreeningPassedAsync(int registrationId)
        {
            try
            {
                var screening = await _context.HealthScreenings
                    .FirstOrDefaultAsync(h => h.RegistrationId == registrationId);

                return screening?.IsEligible ?? false;
            }
            catch
            {
                return false;
            }
        }

        // Helper methods
        private async Task IncrementEventCurrentDonorsAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity != null && eventEntity.CurrentDonors < eventEntity.MaxDonors)
                {
                    eventEntity.CurrentDonors++;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }

        private async Task DecrementEventCurrentDonorsAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity != null && eventEntity.CurrentDonors > 0)
                {
                    eventEntity.CurrentDonors--;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }

        public async Task<bool> CancelCheckinAsync(int registrationId)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;
                if (registration.Status != RegistrationStatus.CheckedIn) return false;
                registration.Status = RegistrationStatus.Registered;
                registration.CheckInTime = null;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DonationRegistrationDto> RegisterUserForEventAsync(int userId, int eventId, string? notes = null)
        {
            try
            {
                var createDto = new DonationRegistrationCreateDto
                {
                    UserId = userId,
                    EventId = eventId,
                    Notes = notes
                };
                
                return await CreateRegistrationAsync(createDto);
            }
            catch
            {
                throw;
            }
        }

        // Lấy danh sách đăng ký chờ sàng lọc sức khỏe
        public async Task<IEnumerable<DonationRegistrationDto>> GetPendingHealthScreeningsAsync()
        {
            try
            {
                // Theo quy trình: Status = 'CheckedIn' AND IsEligible = 0
                // Hoặc: Status = 'Ineligible' AND IsEligible = 0
                var registrations = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .Where(r => (r.Status == RegistrationStatus.CheckedIn && !r.IsEligible) || 
                               (r.Status == RegistrationStatus.Ineligible && !r.IsEligible))
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

        /// <summary>
        /// Huỷ tất cả đăng ký còn hiệu lực của user (trừ registrationId vừa hoàn thành)
        /// </summary>
        public async Task<int> CancelAllActiveRegistrationsExceptAsync(int userId, int exceptRegistrationId, DisqualificationReason reason)
        {
            // Các trạng thái còn hiệu lực (chưa hoàn thành, chưa huỷ, chưa thất bại, chưa tạm hoãn, chưa no-show)
            var activeStatuses = new[]
            {
                RegistrationStatus.Registered,
                RegistrationStatus.Confirmed,
                RegistrationStatus.CheckedIn,
                RegistrationStatus.Screening,
                RegistrationStatus.Eligible,
                RegistrationStatus.Donating
            };
            var registrations = await _context.DonationRegistrations
                .Where(r => r.UserId == userId && r.RegistrationId != exceptRegistrationId && activeStatuses.Contains(r.Status))
                .ToListAsync();
            foreach (var reg in registrations)
            {
                reg.Status = RegistrationStatus.Cancelled;
                reg.CancellationReason = reason.ToString();
            }
            await _context.SaveChangesAsync();
            return registrations.Count;
        }
    }
}