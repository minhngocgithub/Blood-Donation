using Blood_Donation_Website.Data;
using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD operations
        public async Task<LocationDto?> GetLocationByIdAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return null;

                return new LocationDto
                {
                    LocationId = location.LocationId,
                    LocationName = location.LocationName,
                    Address = location.Address,
                    ContactPhone = location.ContactPhone,
                    Capacity = location.Capacity,
                    IsActive = location.IsActive,
                    CreatedDate = location.CreatedDate
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<LocationDto?> GetLocationByNameAsync(string locationName)
        {
            try
            {
                var location = await _context.Locations
                    .FirstOrDefaultAsync(l => l.LocationName == locationName);

                if (location == null) return null;

                return new LocationDto
                {
                    LocationId = location.LocationId,
                    LocationName = location.LocationName,
                    Address = location.Address,
                    ContactPhone = location.ContactPhone,
                    Capacity = location.Capacity,
                    IsActive = location.IsActive,
                    CreatedDate = location.CreatedDate
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
        {
            try
            {
                var locations = await _context.Locations
                    .OrderBy(l => l.LocationName)
                    .ToListAsync();

                return locations.Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName,
                    Address = l.Address,
                    ContactPhone = l.ContactPhone,
                    Capacity = l.Capacity,
                    IsActive = l.IsActive,
                    CreatedDate = l.CreatedDate
                });
            }
            catch
            {
                return new List<LocationDto>();
            }
        }

        public async Task<IEnumerable<LocationDto>> GetActiveLocationsAsync()
        {
            try
            {
                var locations = await _context.Locations
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.LocationName)
                    .ToListAsync();

                return locations.Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName,
                    Address = l.Address,
                    ContactPhone = l.ContactPhone,
                    Capacity = l.Capacity,
                    IsActive = l.IsActive,
                    CreatedDate = l.CreatedDate
                });
            }
            catch
            {
                return new List<LocationDto>();
            }
        }

        public async Task<LocationDto> CreateLocationAsync(LocationCreateDto createDto)
        {
            try
            {
                if (await IsLocationNameExistsAsync(createDto.LocationName))
                {
                    throw new InvalidOperationException("Location name already exists");
                }

                var location = new Location
                {
                    LocationName = createDto.LocationName,
                    Address = createDto.Address,
                    ContactPhone = createDto.ContactPhone,
                    Capacity = createDto.Capacity,
                    IsActive = createDto.IsActive,
                    CreatedDate = DateTime.Now
                };

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                return new LocationDto
                {
                    LocationId = location.LocationId,
                    LocationName = location.LocationName,
                    Address = location.Address,
                    ContactPhone = location.ContactPhone,
                    Capacity = location.Capacity,
                    IsActive = location.IsActive,
                    CreatedDate = location.CreatedDate
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateLocationAsync(int locationId, LocationUpdateDto updateDto)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return false;

                location.LocationName = updateDto.LocationName;
                location.Address = updateDto.Address;
                location.ContactPhone = updateDto.ContactPhone;
                location.Capacity = updateDto.Capacity;
                location.IsActive = updateDto.IsActive;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLocationAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return false;

                // Check if location is being used by any events
                var eventsWithLocation = await _context.BloodDonationEvents
                    .AnyAsync(e => e.LocationId == locationId);

                if (eventsWithLocation)
                {
                    throw new InvalidOperationException("Cannot delete location that has associated events");
                }

                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Location status operations
        public async Task<bool> ActivateLocationAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return false;

                location.IsActive = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateLocationAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return false;

                location.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsLocationActiveAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                return location?.IsActive ?? false;
            }
            catch
            {
                return false;
            }
        }

        // Location capacity operations
        public async Task<bool> UpdateLocationCapacityAsync(int locationId, int capacity)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return false;

                location.Capacity = capacity;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetLocationCapacityAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                return location?.Capacity ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetAvailableCapacityAsync(int locationId)
        {
            try
            {
                var location = await _context.Locations.FindAsync(locationId);
                if (location == null) return 0;

                var currentEvents = await _context.BloodDonationEvents
                    .Where(e => e.LocationId == locationId && e.Status == "Active")
                    .SumAsync(e => e.CurrentDonors);

                return Math.Max(0, location.Capacity - currentEvents);
            }
            catch
            {
                return 0;
            }
        }

        // Location events
        public async Task<IEnumerable<BloodDonationEventDto>> GetLocationEventsAsync(int locationId)
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

        public async Task<IEnumerable<BloodDonationEventDto>> GetLocationUpcomingEventsAsync(int locationId)
        {
            try
            {
                var events = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .Include(e => e.Creator)
                    .Where(e => e.LocationId == locationId && e.EventDate >= DateTime.Now && e.Status == "Active")
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

        public async Task<int> GetLocationEventCountAsync(int locationId)
        {
            try
            {
                return await _context.BloodDonationEvents
                    .Where(e => e.LocationId == locationId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        // Location search and filtering
        public async Task<IEnumerable<LocationDto>> SearchLocationsAsync(string searchTerm)
        {
            try
            {
                var locations = await _context.Locations
                    .Where(l => l.LocationName.Contains(searchTerm) || 
                               l.Address.Contains(searchTerm) ||
                               (l.ContactPhone != null && l.ContactPhone.Contains(searchTerm)))
                    .OrderBy(l => l.LocationName)
                    .ToListAsync();

                return locations.Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName,
                    Address = l.Address,
                    ContactPhone = l.ContactPhone,
                    Capacity = l.Capacity,
                    IsActive = l.IsActive,
                    CreatedDate = l.CreatedDate
                });
            }
            catch
            {
                return new List<LocationDto>();
            }
        }

        public async Task<IEnumerable<LocationDto>> GetLocationsByCapacityAsync(int minCapacity)
        {
            try
            {
                var locations = await _context.Locations
                    .Where(l => l.Capacity >= minCapacity)
                    .OrderBy(l => l.LocationName)
                    .ToListAsync();

                return locations.Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName,
                    Address = l.Address,
                    ContactPhone = l.ContactPhone,
                    Capacity = l.Capacity,
                    IsActive = l.IsActive,
                    CreatedDate = l.CreatedDate
                });
            }
            catch
            {
                return new List<LocationDto>();
            }
        }

        public async Task<IEnumerable<LocationDto>> GetLocationsByAddressAsync(string address)
        {
            try
            {
                var locations = await _context.Locations
                    .Where(l => l.Address.Contains(address))
                    .OrderBy(l => l.LocationName)
                    .ToListAsync();

                return locations.Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName,
                    Address = l.Address,
                    ContactPhone = l.ContactPhone,
                    Capacity = l.Capacity,
                    IsActive = l.IsActive,
                    CreatedDate = l.CreatedDate
                });
            }
            catch
            {
                return new List<LocationDto>();
            }
        }

        // Location validation
        public async Task<bool> IsLocationExistsAsync(int locationId)
        {
            try
            {
                return await _context.Locations.AnyAsync(l => l.LocationId == locationId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsLocationNameExistsAsync(string locationName)
        {
            try
            {
                return await _context.Locations.AnyAsync(l => l.LocationName == locationName);
            }
            catch
            {
                return false;
            }
        }

        // Location statistics
        public async Task<int> GetTotalEventsAtLocationAsync(int locationId)
        {
            try
            {
                return await _context.BloodDonationEvents
                    .Where(e => e.LocationId == locationId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetTotalDonationsAtLocationAsync(int locationId)
        {
            try
            {
                return await _context.DonationHistories
                    .Include(d => d.Event)
                    .Where(d => d.Event.LocationId == locationId)
                    .CountAsync();
            }
            catch
            {
                return 0;
            }
        }
    }
} 