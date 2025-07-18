
using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class BloodDonationEventService : IBloodDonationEventService
    {
        private readonly ApplicationDbContext _context;

        public BloodDonationEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<BloodDonationEventDto?> GetEventByIdAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity == null) return null;

                return new BloodDonationEventDto
                {
                    EventId = eventEntity.EventId,
                    EventName = eventEntity.EventName,
                    EventDescription = eventEntity.EventDescription,
                    EventDate = eventEntity.EventDate,
                    StartTime = eventEntity.StartTime,
                    EndTime = eventEntity.EndTime,
                    LocationId = eventEntity.LocationId,
                    MaxDonors = eventEntity.MaxDonors,
                    CurrentDonors = eventEntity.CurrentDonors,
                    Status = eventEntity.Status,
                    ImageUrl = eventEntity.ImageUrl,
                    RequiredBloodTypes = eventEntity.RequiredBloodTypes,
                    CreatedBy = eventEntity.CreatedBy,
                    CreatedDate = eventEntity.CreatedDate,
                    UpdatedDate = eventEntity.UpdatedDate,
                    LocationName = eventEntity.Location?.LocationName,
                    LocationAddress = eventEntity.Location?.Address,
                    CreatorName = eventEntity.Creator?.FullName
                };
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<BloodDonationEventDto>> SearchEventsByNameDescLocationAsync(string searchTerm, string location)
        {
            try
            {
                var query = _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(e =>
                        e.EventName.Contains(searchTerm) ||
                        (e.EventDescription != null && e.EventDescription.Contains(searchTerm))
                    );
                }

                if (!string.IsNullOrWhiteSpace(location))
                {
                    query = query.Where(e => e.Location != null && e.Location.LocationName.Contains(location));
                }

                var events = await query.OrderByDescending(e => e.EventDate).ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<BloodDonationEventDto?> GetEventByNameAsync(string eventName)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .FirstOrDefaultAsync(e => e.EventName == eventName);

                if (eventEntity == null) return null;

                return new BloodDonationEventDto
                {
                    EventId = eventEntity.EventId,
                    EventName = eventEntity.EventName,
                    EventDescription = eventEntity.EventDescription,
                    EventDate = eventEntity.EventDate,
                    StartTime = eventEntity.StartTime,
                    EndTime = eventEntity.EndTime,
                    LocationId = eventEntity.LocationId,
                    MaxDonors = eventEntity.MaxDonors,
                    CurrentDonors = eventEntity.CurrentDonors,
                    Status = eventEntity.Status,
                    ImageUrl = eventEntity.ImageUrl,
                    RequiredBloodTypes = eventEntity.RequiredBloodTypes,
                    CreatedBy = eventEntity.CreatedBy,
                    CreatedDate = eventEntity.CreatedDate,
                    UpdatedDate = eventEntity.UpdatedDate,
                    LocationName = eventEntity.Location?.LocationName,
                    LocationAddress = eventEntity.Location?.Address,
                    CreatorName = eventEntity.Creator?.FullName
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetAllEventsAsync()
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<PagedResponseDto<BloodDonationEventDto>> GetEventsPagedAsync(EventSearchDto searchDto)
        {
            try
            {
                var query = _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                {
                    query = query.Where(e => 
                        e.EventName.Contains(searchDto.SearchTerm) ||
                        (e.EventDescription != null && e.EventDescription.Contains(searchDto.SearchTerm)));
                }

                if (searchDto.FromDate.HasValue)
                {
                    query = query.Where(e => e.EventDate >= searchDto.FromDate.Value);
                }

                if (searchDto.ToDate.HasValue)
                {
                    query = query.Where(e => e.EventDate <= searchDto.ToDate.Value);
                }

                if (searchDto.LocationId.HasValue)
                {
                    query = query.Where(e => e.LocationId == searchDto.LocationId);
                }

                if (!string.IsNullOrEmpty(searchDto.Status))
                {
                    query = query.Where(e => e.Status == searchDto.Status);
                }

                if (!string.IsNullOrEmpty(searchDto.RequiredBloodTypes))
                {
                    query = query.Where(e => e.RequiredBloodTypes != null && e.RequiredBloodTypes.Contains(searchDto.RequiredBloodTypes));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(searchDto.SortBy))
                {
                    query = searchDto.SortBy.ToLower() switch
                    {
                        "eventname" => searchDto.SortOrder == "desc" ? query.OrderByDescending(e => e.EventName) : query.OrderBy(e => e.EventName),
                        "eventdate" => searchDto.SortOrder == "desc" ? query.OrderByDescending(e => e.EventDate) : query.OrderBy(e => e.EventDate),
                        "status" => searchDto.SortOrder == "desc" ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
                        _ => query.OrderByDescending(e => e.EventDate)
                    };
                }
                else
                {
                    query = query.OrderByDescending(e => e.EventDate);
                }

                var totalCount = await query.CountAsync();
                var pageSize = searchDto.PageSize ?? 10;
                var pageNumber = searchDto.Page ?? 1;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var events = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var eventDtos = events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                }).ToList();

                return new PagedResponseDto<BloodDonationEventDto>
                {
                    Items = eventDtos,
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
                return new PagedResponseDto<BloodDonationEventDto>
                {
                    Items = new List<BloodDonationEventDto>(),
                    TotalCount = 0,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalPages = 0,
                    HasPreviousPage = false,
                    HasNextPage = false
                };
            }
        }

        public async Task<BloodDonationEventDto> CreateEventAsync(BloodDonationEventCreateDto createDto)
        {
            try
            {
                if (await IsEventNameExistsAsync(createDto.EventName))
                {
                    throw new InvalidOperationException("Event name already exists");
                }

                if (!await IsEventDateValidAsync(createDto.EventDate))
                {
                    throw new InvalidOperationException("Event date must be in the future");
                }

                if (!await IsEventTimeValidAsync(createDto.StartTime, createDto.EndTime))
                {
                    throw new InvalidOperationException("End time must be after start time");
                }

                var eventEntity = new BloodDonationEvent
                {
                    EventName = createDto.EventName,
                    EventDescription = createDto.EventDescription,
                    EventDate = createDto.EventDate,
                    StartTime = createDto.StartTime,
                    EndTime = createDto.EndTime,
                    LocationId = createDto.LocationId,
                    MaxDonors = createDto.MaxDonors,
                    CurrentDonors = 0,
                    Status = "Active",
                    ImageUrl = createDto.ImageUrl,
                    RequiredBloodTypes = createDto.RequiredBloodTypes,
                    CreatedBy = createDto.CreatedBy,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _context.BloodDonationEvents.Add(eventEntity);
                await _context.SaveChangesAsync();

                return await GetEventByIdAsync(eventEntity.EventId) ?? new BloodDonationEventDto();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateEventAsync(int eventId, BloodDonationEventUpdateDto updateDto)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                eventEntity.EventName = updateDto.EventName;
                eventEntity.EventDescription = updateDto.EventDescription;
                eventEntity.EventDate = updateDto.EventDate;
                eventEntity.StartTime = updateDto.StartTime;
                eventEntity.EndTime = updateDto.EndTime;
                eventEntity.LocationId = updateDto.LocationId;
                eventEntity.MaxDonors = updateDto.MaxDonors;
                eventEntity.Status = updateDto.Status;
                eventEntity.ImageUrl = updateDto.ImageUrl;
                eventEntity.RequiredBloodTypes = updateDto.RequiredBloodTypes;
                eventEntity.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                // Check if event has registrations
                var hasRegistrations = await _context.DonationRegistrations
                    .AnyAsync(r => r.EventId == eventId);

                if (hasRegistrations)
                {
                    throw new InvalidOperationException("Cannot delete event that has registrations");
                }

                _context.BloodDonationEvents.Remove(eventEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Event status operations
        public async Task<bool> ActivateEventAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                eventEntity.Status = "Active";
                eventEntity.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateEventAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                eventEntity.Status = "Inactive";
                eventEntity.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelEventAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                eventEntity.Status = "Cancelled";
                eventEntity.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CompleteEventAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                eventEntity.Status = "Completed";
                eventEntity.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetEventStatusAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                return eventEntity?.Status ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        // Event capacity management
        public async Task<bool> UpdateEventCapacityAsync(int eventId, int maxDonors)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                if (maxDonors < eventEntity.CurrentDonors)
                {
                    throw new InvalidOperationException("New capacity cannot be less than current donors");
                }

                eventEntity.MaxDonors = maxDonors;
                eventEntity.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetEventAvailableSlotsAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return 0;

                return Math.Max(0, eventEntity.MaxDonors - eventEntity.CurrentDonors);
            }
            catch
            {
                return 0;
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

        public async Task<bool> IncrementCurrentDonorsAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                if (eventEntity.CurrentDonors >= eventEntity.MaxDonors)
                {
                    return false; // Event is full
                }

                eventEntity.CurrentDonors++;
                eventEntity.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DecrementCurrentDonorsAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents.FindAsync(eventId);
                if (eventEntity == null) return false;

                if (eventEntity.CurrentDonors > 0)
                {
                    eventEntity.CurrentDonors--;
                    eventEntity.UpdatedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Event scheduling
        public async Task<IEnumerable<BloodDonationEventDto>> GetUpcomingEventsAsync()
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.EventDate >= DateTime.Now && e.Status == "Active")
                    .OrderBy(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetPastEventsAsync()
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.EventDate < DateTime.Now)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.EventDate >= startDate && e.EventDate <= endDate)
                    .OrderBy(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetEventsByLocationAsync(int locationId)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.LocationId == locationId)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetEventsByCreatorAsync(int creatorId)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.CreatedBy == creatorId)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        // Event search and filtering
        public async Task<IEnumerable<BloodDonationEventDto>> SearchEventsAsync(string searchTerm)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.EventName.Contains(searchTerm) || 
                               (e.EventDescription != null && e.EventDescription.Contains(searchTerm)))
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetEventsByStatusAsync(string status)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.Status == status)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        public async Task<IEnumerable<BloodDonationEventDto>> GetEventsByBloodTypeAsync(string requiredBloodTypes)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.RequiredBloodTypes != null && e.RequiredBloodTypes.Contains(requiredBloodTypes))
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();

                return events.Select(e => new BloodDonationEventDto
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    EventDate = e.EventDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    LocationId = e.LocationId,
                    MaxDonors = e.MaxDonors,
                    CurrentDonors = e.CurrentDonors,
                    Status = e.Status,
                    ImageUrl = e.ImageUrl,
                    RequiredBloodTypes = e.RequiredBloodTypes,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    LocationName = e.Location?.LocationName,
                    LocationAddress = e.Location?.Address,
                    CreatorName = e.Creator?.FullName
                });
            }
            catch
            {
                return new List<BloodDonationEventDto>();
            }
        }

        // Event statistics
        public async Task<EventStatisticsDto> GetEventStatisticsAsync(int eventId)
        {
            try
            {
                var eventEntity = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity == null) return new EventStatisticsDto();

                var completedDonations = await _context.DonationHistories
                    .Where(d => d.EventId == eventId && d.Status == "Completed")
                    .CountAsync();

                return new EventStatisticsDto
                {
                    EventId = eventEntity.EventId,
                    EventName = eventEntity.EventName,
                    EventDate = eventEntity.EventDate,
                    MaxDonors = eventEntity.MaxDonors,
                    CurrentDonors = eventEntity.CurrentDonors,
                    CompletedDonations = completedDonations,
                    Status = eventEntity.Status,
                    LocationName = eventEntity.Location?.LocationName
                };
            }
            catch
            {
                return new EventStatisticsDto();
            }
        }

        public async Task<IEnumerable<EventStatisticsDto>> GetAllEventStatisticsAsync()
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .ToListAsync();

                var statistics = new List<EventStatisticsDto>();

                foreach (var eventEntity in events)
                {
                    var stat = await GetEventStatisticsAsync(eventEntity.EventId);
                    statistics.Add(stat);
                }

                return statistics.OrderByDescending(s => s.EventDate);
            }
            catch
            {
                return new List<EventStatisticsDto>();
            }
        }

        public async Task<int> GetEventRegistrationCountAsync(int eventId)
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

        public async Task<int> GetEventDonationCountAsync(int eventId)
        {
            try
            {
                return await _context.DonationHistories
                    .Where(d => d.EventId == eventId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        // Event validation
        public async Task<bool> IsEventExistsAsync(int eventId)
        {
            try
            {
                return await _context.BloodDonationEvents.AnyAsync(e => e.EventId == eventId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsEventNameExistsAsync(string eventName)
        {
            try
            {
                return await _context.BloodDonationEvents.AnyAsync(e => e.EventName == eventName);
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> IsEventDateValidAsync(DateTime eventDate)
        {
            return Task.FromResult(eventDate > DateTime.Now);
        }

        public Task<bool> IsEventTimeValidAsync(TimeSpan startTime, TimeSpan endTime)
        {
            return Task.FromResult(endTime > startTime);
        }

        // Event notifications
        public async Task<bool> SendEventRemindersAsync(int eventId)
        {
            try
            {
                // Get event details and registered users for reminders
                var eventEntity = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity == null)
                {
                    return false;
                }

                var registeredUsers = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Where(r => r.EventId == eventId && r.Status == "Approved")
                    .ToListAsync();

                if (!registeredUsers.Any())
                {
                    return true;
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

        public async Task<bool> SendEventUpdatesAsync(int eventId, string updateMessage)
        {
            try
            {
                // Get event details and registered users for updates
                var eventEntity = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity == null)
                {
                    return false;
                }

                var registeredUsers = await _context.DonationRegistrations
                    .Include(r => r.User)
                    .Where(r => r.EventId == eventId && 
                           (r.Status == "Approved" || r.Status == "Registered"))
                    .ToListAsync();

                if (!registeredUsers.Any())
                {
                    return true;
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
    }
} 