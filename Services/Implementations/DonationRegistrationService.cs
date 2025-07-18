using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                .Where(r => r.Status != "Cancelled" && r.Status != "Rejected" && r.Status != "Completed");

            // Tìm theo mã đăng ký hoặc số điện thoại
            query = query.Where(r => r.RegistrationId.ToString() == code || r.User.Phone == code);

            var registrations = await query.OrderByDescending(r => r.RegistrationDate).ToListAsync();

            return registrations.Select(r => new DonationRegistrationDto
            {
                RegistrationId = r.RegistrationId,
                FullName = r.User?.FullName,
                RegistrationCode = r.RegistrationId.ToString(),
                PhoneNumber = r.User?.Phone,
                RegistrationDate = r.RegistrationDate,
                Status = r.Status
            });
        }

        public async Task<bool> CheckinRegistrationAsync(int registrationId)

        {
            var registration = await _context.DonationRegistrations.FindAsync(registrationId);
            if (registration == null) return false;
            if (registration.Status == "CheckedIn") return false;
            registration.Status = "CheckedIn";
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
                    LocationName = r.Event?.Location?.LocationName
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
                // Validate user eligibility
                if (!await IsUserEligibleForEventAsync(createDto.UserId, createDto.EventId))
                {
                    throw new InvalidOperationException("User is not eligible for this event");
                }

                // Check if user is already registered
                if (await IsUserRegisteredForEventAsync(createDto.UserId, createDto.EventId))
                {
                    throw new InvalidOperationException("User is already registered for this event");
                }

                // Check if event is full
                if (await IsEventFullAsync(createDto.EventId))
                {
                    throw new InvalidOperationException("Event is full");
                }

                var registration = new DonationRegistration
                {
                    UserId = createDto.UserId,
                    EventId = createDto.EventId,
                    RegistrationDate = DateTime.Now,
                    Status = "Registered",
                    Notes = createDto.Notes,
                    IsEligible = true
                };

                _context.DonationRegistrations.Add(registration);
                await _context.SaveChangesAsync();

                // Increment current donors count
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

                registration.Status = "Approved";
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectRegistrationAsync(int registrationId, string reason)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = "Rejected";
                registration.CancellationReason = reason;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelRegistrationAsync(int registrationId, string reason)
        {
            try
            {
                var registration = await _context.DonationRegistrations.FindAsync(registrationId);
                if (registration == null) return false;

                registration.Status = "Cancelled";
                registration.CancellationReason = reason;
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

                registration.Status = "CheckedIn";
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

                registration.Status = "Completed";
                registration.CompletionTime = DateTime.Now;
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
                return registration?.Status ?? "Unknown";
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

        public async Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByStatusAsync(string status)
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
                var registration = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Include(r => r.Event)
                    .ThenInclude(e => e.Location)
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId);

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
                return await _context.DonationRegistrations
                    .AnyAsync(r => r.UserId == userId && r.EventId == eventId);
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
                if (eventEntity == null || eventEntity.Status != "Active") return false;

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

        public async Task<int> GetRegistrationCountByStatusAsync(string status)
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

        public async Task<int> GetRegistrationCountByUserAndStatusAsync(int userId, string status)
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

        public async Task<int> GetRegistrationCountByUserAndStatusAsync(int userId, string status)
        {
            try
            {
                return await _context.DonationRegistrations
                    .Where(r => r.UserId == userId && r.Status == status)
                    .CountAsync();
            }
            catch (Exception ex)
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
                    .Where(r => r.Status == "Registered")
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
                    .Where(r => r.Status == "Approved")
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
    }
}