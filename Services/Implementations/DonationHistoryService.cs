using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class DonationHistoryService : IDonationHistoryService
    {
        private readonly ApplicationDbContext _context;

        public DonationHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<DonationHistoryDto?> GetDonationByIdAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .FirstOrDefaultAsync(d => d.DonationId == donationId);

                if (donation == null) return null;

                return new DonationHistoryDto
                {
                    DonationId = donation.DonationId,
                    UserId = donation.UserId,
                    EventId = donation.EventId,
                    RegistrationId = donation.RegistrationId,
                    DonationDate = donation.DonationDate,
                    BloodTypeId = donation.BloodTypeId,
                    Volume = donation.Volume,
                    Status = donation.Status,
                    Notes = donation.Notes,
                    NextEligibleDate = donation.NextEligibleDate,
                    CertificateIssued = donation.CertificateIssued,
                    UserName = donation.User?.FullName,
                    UserEmail = donation.User?.Email,
                    EventName = donation.Event?.EventName,
                    EventDate = donation.Event?.EventDate,
                    BloodTypeName = donation.BloodType?.BloodTypeName
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetAllDonationsAsync()
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<PagedResponseDto<DonationHistoryDto>> GetDonationsPagedAsync(DonationSearchDto searchDto)
        {
            try
            {
                var query = _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .AsQueryable();

                // Apply filters
                if (searchDto.UserId.HasValue)
                {
                    query = query.Where(d => d.UserId == searchDto.UserId.Value);
                }

                if (searchDto.EventId.HasValue)
                {
                    query = query.Where(d => d.EventId == searchDto.EventId.Value);
                }

                if (searchDto.BloodTypeId.HasValue)
                {
                    query = query.Where(d => d.BloodTypeId == searchDto.BloodTypeId.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.Status))
                {
                    query = query.Where(d => d.Status == searchDto.Status);
                }

                if (searchDto.StartDate.HasValue)
                {
                    query = query.Where(d => d.DonationDate >= searchDto.StartDate.Value);
                }

                if (searchDto.EndDate.HasValue)
                {
                    query = query.Where(d => d.DonationDate <= searchDto.EndDate.Value);
                }

                if (searchDto.CertificateIssued.HasValue)
                {
                    query = query.Where(d => d.CertificateIssued == searchDto.CertificateIssued.Value);
                }

                // Apply search term
                if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                {
                    query = query.Where(d => 
                        d.User.FullName.Contains(searchDto.SearchTerm) ||
                        d.User.Email.Contains(searchDto.SearchTerm) ||
                        d.Event.EventName.Contains(searchDto.SearchTerm) ||
                        d.BloodType.BloodTypeName.Contains(searchDto.SearchTerm) ||
                        (d.Notes != null && d.Notes.Contains(searchDto.SearchTerm)));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(searchDto.SortBy))
                {
                    query = searchDto.SortBy.ToLower() switch
                    {
                        "donationdate" => searchDto.SortOrder == "desc" ? query.OrderByDescending(d => d.DonationDate) : query.OrderBy(d => d.DonationDate),
                        "status" => searchDto.SortOrder == "desc" ? query.OrderByDescending(d => d.Status) : query.OrderBy(d => d.Status),
                        "username" => searchDto.SortOrder == "desc" ? query.OrderByDescending(d => d.User.FullName) : query.OrderBy(d => d.User.FullName),
                        "volume" => searchDto.SortOrder == "desc" ? query.OrderByDescending(d => d.Volume) : query.OrderBy(d => d.Volume),
                        _ => query.OrderByDescending(d => d.DonationDate)
                    };
                }
                else
                {
                    query = query.OrderByDescending(d => d.DonationDate);
                }

                var totalCount = await query.CountAsync();
                var pageSize = searchDto.PageSize ?? 10;
                var pageNumber = searchDto.Page ?? 1;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var donations = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var donationDtos = donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                }).ToList();

                return new PagedResponseDto<DonationHistoryDto>
                {
                    Items = donationDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages
                };
            }
            catch (Exception ex)
            {
                return new PagedResponseDto<DonationHistoryDto>
                {
                    Items = new List<DonationHistoryDto>(),
                    TotalCount = 0,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };
            }
        }

        public async Task<DonationHistoryDto> CreateDonationAsync(DonationHistoryCreateDto createDto)
        {
            try
            {
                // Calculate next eligible date (56 days from donation date)
                var nextEligibleDate = createDto.DonationDate.AddDays(56);

                var donation = new DonationHistory
                {
                    UserId = createDto.UserId,
                    EventId = createDto.EventId,
                    RegistrationId = createDto.RegistrationId,
                    DonationDate = createDto.DonationDate,
                    BloodTypeId = createDto.BloodTypeId,
                    Volume = createDto.Volume,
                    Status = "Completed",
                    Notes = createDto.Notes,
                    NextEligibleDate = nextEligibleDate,
                    CertificateIssued = false
                };

                _context.DonationHistories.Add(donation);
                await _context.SaveChangesAsync();

                return await GetDonationByIdAsync(donation.DonationId) ?? new DonationHistoryDto();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateDonationAsync(int donationId, DonationHistoryUpdateDto updateDto)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                if (donation == null) return false;

                donation.Status = updateDto.Status;
                donation.Notes = updateDto.Notes;
                donation.NextEligibleDate = updateDto.NextEligibleDate;
                donation.CertificateIssued = updateDto.CertificateIssued;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteDonationAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                if (donation == null) return false;

                _context.DonationHistories.Remove(donation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Donation status operations
        public async Task<bool> CompleteDonationAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                if (donation == null) return false;

                donation.Status = "Completed";
                donation.NextEligibleDate = donation.DonationDate.AddDays(56);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CancelDonationAsync(int donationId, string reason)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                if (donation == null) return false;

                donation.Status = "Cancelled";
                donation.Notes = reason;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IssueCertificateAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                if (donation == null) return false;

                donation.CertificateIssued = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> GetDonationStatusAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                return donation?.Status ?? "Unknown";
            }
            catch (Exception ex)
            {
                return "Unknown";
            }
        }

        // Donation queries
        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByUserAsync(int userId)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByEventAsync(int eventId)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.EventId == eventId)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.BloodTypeId == bloodTypeId)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByStatusAsync(string status)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.Status == status)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.DonationDate >= startDate && d.DonationDate <= endDate)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByRegistrationAsync(int registrationId)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.RegistrationId == registrationId)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        // Donation statistics
        public async Task<int> GetTotalDonationsAsync()
        {
            try
            {
                return await _context.DonationHistories.CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalDonationsByUserAsync(int userId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.UserId == userId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalDonationsByEventAsync(int eventId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.EventId == eventId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalDonationsByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.BloodTypeId == bloodTypeId)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalVolumeAsync()
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.Status == "Completed")
                    .SumAsync(d => d.Volume);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalVolumeByUserAsync(int userId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.UserId == userId && d.Status == "Completed")
                    .SumAsync(d => d.Volume);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalVolumeByEventAsync(int eventId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.EventId == eventId && d.Status == "Completed")
                    .SumAsync(d => d.Volume);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetTotalVolumeByBloodTypeAsync(int bloodTypeId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.BloodTypeId == bloodTypeId && d.Status == "Completed")
                    .SumAsync(d => d.Volume);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // Donation eligibility
        public async Task<DateTime?> GetUserNextEligibleDateAsync(int userId)
        {
            try
            {
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == userId && d.Status == "Completed")
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                return lastDonation?.NextEligibleDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> IsUserEligibleForDonationAsync(int userId)
        {
            try
            {
                var nextEligibleDate = await GetUserNextEligibleDateAsync(userId);
                return !nextEligibleDate.HasValue || nextEligibleDate.Value <= DateTime.Now;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CanUserDonateAsync(int userId, DateTime donationDate)
        {
            try
            {
                var lastDonation = await _context.DonationHistories
                    .Where(d => d.UserId == userId && d.Status == "Completed")
                    .OrderByDescending(d => d.DonationDate)
                    .FirstOrDefaultAsync();

                if (lastDonation == null) return true;

                var daysSinceLastDonation = (donationDate - lastDonation.DonationDate).Days;
                return daysSinceLastDonation >= 56;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Donation search and filtering
        public async Task<IEnumerable<DonationHistoryDto>> SearchDonationsAsync(string searchTerm)
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.User.FullName.Contains(searchTerm) ||
                               d.User.Email.Contains(searchTerm) ||
                               d.Event.EventName.Contains(searchTerm) ||
                               d.BloodType.BloodTypeName.Contains(searchTerm) ||
                               (d.Notes != null && d.Notes.Contains(searchTerm)))
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetCompletedDonationsAsync()
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.Status == "Completed")
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetCancelledDonationsAsync()
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.Status == "Cancelled")
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsWithCertificatesAsync()
        {
            try
            {
                var donations = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .Include(d => d.BloodType)
                    .Include(d => d.Registration)
                    .Where(d => d.CertificateIssued)
                    .OrderByDescending(d => d.DonationDate)
                    .ToListAsync();

                return donations.Select(d => new DonationHistoryDto
                {
                    DonationId = d.DonationId,
                    UserId = d.UserId,
                    EventId = d.EventId,
                    RegistrationId = d.RegistrationId,
                    DonationDate = d.DonationDate,
                    BloodTypeId = d.BloodTypeId,
                    Volume = d.Volume,
                    Status = d.Status,
                    Notes = d.Notes,
                    NextEligibleDate = d.NextEligibleDate,
                    CertificateIssued = d.CertificateIssued,
                    UserName = d.User?.FullName,
                    UserEmail = d.User?.Email,
                    EventName = d.Event?.EventName,
                    EventDate = d.Event?.EventDate,
                    BloodTypeName = d.BloodType?.BloodTypeName
                });
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        // Donation validation
        public async Task<bool> IsDonationExistsAsync(int donationId)
        {
            try
            {
                return await _context.DonationHistories.AnyAsync(d => d.DonationId == donationId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsDonationDateValidAsync(DateTime donationDate)
        {
            try
            {
                return donationDate <= DateTime.Now && donationDate >= DateTime.Now.AddYears(-10);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsDonationVolumeValidAsync(int volume)
        {
            try
            {
                return volume >= 200 && volume <= 500;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Donation reporting
        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByMonthAsync(int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                return await GetDonationsByDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<IEnumerable<DonationHistoryDto>> GetDonationsByYearAsync(int year)
        {
            try
            {
                var startDate = new DateTime(year, 1, 1);
                var endDate = new DateTime(year, 12, 31);

                return await GetDonationsByDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                return new List<DonationHistoryDto>();
            }
        }

        public async Task<Dictionary<string, int>> GetDonationsByBloodTypeChartAsync()
        {
            try
            {
                var result = await _context.DonationHistories
                    .Include(d => d.BloodType)
                    .Where(d => d.Status == "Completed")
                    .GroupBy(d => d.BloodType.BloodTypeName)
                    .Select(g => new { BloodType = g.Key, Count = g.Count() })
                    .ToListAsync();

                return result.ToDictionary(x => x.BloodType, x => x.Count);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, int>();
            }
        }

        public async Task<Dictionary<string, int>> GetDonationsByMonthChartAsync(int year)
        {
            try
            {
                var result = await _context.DonationHistories
                    .Where(d => d.DonationDate.Year == year && d.Status == "Completed")
                    .GroupBy(d => d.DonationDate.Month)
                    .Select(g => new { Month = g.Key, Count = g.Count() })
                    .ToListAsync();

                var monthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                return result.ToDictionary(x => monthNames[x.Month - 1], x => x.Count);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, int>();
            }
        }

        // Donation certificates
        public async Task<bool> GenerateDonationCertificateAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories.FindAsync(donationId);
                if (donation == null) return false;

                donation.CertificateIssued = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendDonationCertificateAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.DonationId == donationId);

                if (donation == null) return false;

                // Simulate sending certificate email
                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> GetCertificateCountAsync()
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.CertificateIssued)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // Donation notifications
        public async Task<bool> SendDonationConfirmationAsync(int donationId)
        {
            try
            {
                var donation = await _context.DonationHistories
                    .Include(d => d.User)
                    .Include(d => d.Event)
                    .FirstOrDefaultAsync(d => d.DonationId == donationId);

                if (donation == null) return false;

                // Simulate sending confirmation email
                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendDonationReminderAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                // Simulate sending reminder email
                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendEligibilityNotificationAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                // Simulate sending eligibility notification
                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
} 