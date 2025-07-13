using BloodDonationAPI.Data;
using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Models.Entities;
using BloodDonationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Services.Implementations
{
    public class BloodDonationEventService : IBloodDonationEventService
    {
        private readonly ApplicationDbContext _context;

        public BloodDonationEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BloodDonationEventDto>> GetAllEventsAsync()
        {
            var events = await _context.BloodDonationEvents
                .Include(e => e.Location)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return events.Select(MapToDto).ToList();
        }

        public async Task<BloodDonationEventDto?> GetEventByIdAsync(int id)
        {
            var bloodEvent = await _context.BloodDonationEvents
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.EventId == id);

            return bloodEvent == null ? null : MapToDto(bloodEvent);
        }

        public async Task<List<BloodDonationEventDto>> GetUpcomingEventsAsync()
        {
            var now = DateTime.UtcNow.Date;
            var events = await _context.BloodDonationEvents
                .Include(e => e.Location)
                .Where(e => e.EventDate > now && e.Status == "Active")
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return events.Select(MapToDto).ToList();
        }

        public async Task<List<BloodDonationEventDto>> GetEventsByLocationAsync(int locationId)
        {
            var events = await _context.BloodDonationEvents
                .Include(e => e.Location)
                .Where(e => e.LocationId == locationId)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return events.Select(MapToDto).ToList();
        }

        public async Task<(bool succeeded, string message)> CreateEventAsync(BloodDonationEventDto eventDto)
        {
            try
            {
                var bloodEvent = new BloodDonationEvent
                {
                    EventName = eventDto.Title,
                    EventDescription = eventDto.Description,
                    EventDate = eventDto.StartDate.Date,
                    StartTime = eventDto.StartDate.TimeOfDay,
                    EndTime = eventDto.EndDate.TimeOfDay,
                    LocationId = eventDto.LocationId,
                    MaxDonors = eventDto.TargetQuantity,
                    Status = "Pending",
                    ImageUrl = eventDto.ImageUrl,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.BloodDonationEvents.Add(bloodEvent);
                await _context.SaveChangesAsync();

                return (true, "Tạo sự kiện hiến máu thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi tạo sự kiện: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> UpdateEventAsync(BloodDonationEventDto eventDto)
        {
            try
            {
                var bloodEvent = await _context.BloodDonationEvents.FindAsync(eventDto.Id);
                if (bloodEvent == null)
                {
                    return (false, "Không tìm thấy sự kiện");
                }

                bloodEvent.EventName = eventDto.Title;
                bloodEvent.EventDescription = eventDto.Description;
                bloodEvent.EventDate = eventDto.StartDate.Date;
                bloodEvent.StartTime = eventDto.StartDate.TimeOfDay;
                bloodEvent.EndTime = eventDto.EndDate.TimeOfDay;
                bloodEvent.LocationId = eventDto.LocationId;
                bloodEvent.MaxDonors = eventDto.TargetQuantity;
                bloodEvent.ImageUrl = eventDto.ImageUrl;
                bloodEvent.Status = eventDto.IsActive ? "Active" : "Inactive";
                bloodEvent.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return (true, "Cập nhật sự kiện thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi cập nhật sự kiện: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> DeleteEventAsync(int id)
        {
            try
            {
                var bloodEvent = await _context.BloodDonationEvents.FindAsync(id);
                if (bloodEvent == null)
                {
                    return (false, "Không tìm thấy sự kiện");
                }

                bloodEvent.Status = "Inactive";
                bloodEvent.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return (true, "Xóa sự kiện thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi xóa sự kiện: " + ex.Message);
            }
        }

        public async Task<(bool succeeded, string message)> RegisterForEventAsync(int userId, int eventId, DateTime preferredDate)
        {
            try
            {
                if (await HasUserRegisteredForEventAsync(userId, eventId))
                {
                    return (false, "Bạn đã đăng ký sự kiện này");
                }

                var bloodEvent = await _context.BloodDonationEvents.FindAsync(eventId);
                if (bloodEvent == null)
                {
                    return (false, "Không tìm thấy sự kiện");
                }

                if (preferredDate.Date != bloodEvent.EventDate)
                {
                    return (false, "Ngày đăng ký không hợp lệ");
                }

                var registration = new DonationRegistration
                {
                    UserId = userId,
                    EventId = eventId,
                    RegistrationDate = DateTime.UtcNow,
                    Status = "Pending",
                    Notes = null,
                    IsEligible = true
                };

                _context.DonationRegistrations.Add(registration);
                await _context.SaveChangesAsync();

                return (true, "Đăng ký hiến máu thành công");
            }
            catch (Exception ex)
            {
                return (false, "Đã xảy ra lỗi khi đăng ký hiến máu: " + ex.Message);
            }
        }

        public async Task<List<DonationRegistrationDto>> GetUserRegistrationsAsync(int userId)
        {
            var registrations = await _context.DonationRegistrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();

            return registrations.Select(MapToRegistrationDto).ToList();
        }

        public async Task<bool> HasUserRegisteredForEventAsync(int userId, int eventId)
        {
            return await _context.DonationRegistrations
                .AnyAsync(r => r.UserId == userId && r.EventId == eventId);
        }

        public async Task<int> GetEventRegistrationCountAsync(int eventId)
        {
            return await _context.DonationRegistrations
                .CountAsync(r => r.EventId == eventId);
        }

        private BloodDonationEventDto MapToDto(BloodDonationEvent bloodEvent)
        {
            return new BloodDonationEventDto
            {
                Id = bloodEvent.EventId,
                Title = bloodEvent.EventName,
                Description = bloodEvent.EventDescription ?? string.Empty,
                StartDate = bloodEvent.EventDate.Date + bloodEvent.StartTime,
                EndDate = bloodEvent.EventDate.Date + bloodEvent.EndTime,
                LocationId = bloodEvent.LocationId ?? 0,
                LocationName = bloodEvent.Location?.LocationName ?? string.Empty,
                Address = bloodEvent.Location?.Address ?? string.Empty,
                TargetQuantity = bloodEvent.MaxDonors,
                CurrentQuantity = bloodEvent.CurrentDonors,
                Status = bloodEvent.Status,
                ImageUrl = bloodEvent.ImageUrl ?? string.Empty,
                IsActive = bloodEvent.Status == "Active",
                CreatedDate = bloodEvent.CreatedDate,
                UpdatedDate = bloodEvent.UpdatedDate
            };
        }

        private DonationRegistrationDto MapToRegistrationDto(DonationRegistration registration)
        {
            return new DonationRegistrationDto
            {
                Id = registration.RegistrationId,
                UserId = registration.UserId,
                UserFullName = registration.User?.FullName ?? string.Empty,
                EventId = registration.EventId,
                EventTitle = registration.Event?.EventName ?? string.Empty,
                RegisterDate = registration.RegistrationDate,
                PreferredDate = registration.RegistrationDate,
                PreferredTime = string.Empty,
                Status = registration.Status,
                Notes = registration.Notes,
                CreatedDate = registration.RegistrationDate,
                UpdatedDate = registration.CheckInTime
            };
        }
    }
}
