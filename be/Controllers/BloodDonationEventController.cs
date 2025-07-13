using Microsoft.AspNetCore.Mvc;
using BloodDonationAPI.Models.DTOs;
using BloodDonationAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.Controllers
{
    public class BloodDonationEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BloodDonationEventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var events = _context.BloodDonationEvents
                .Include(e => e.Location)
                .Select(e => new BloodDonationEventDto
                {
                    Id = e.EventId,
                    Title = e.EventName,
                    Description = e.EventDescription ?? string.Empty,
                    StartDate = e.EventDate.Add(e.StartTime),
                    EndDate = e.EventDate.Add(e.EndTime),
                    LocationName = e.Location != null ? e.Location.LocationName : string.Empty,
                    Address = e.Location != null ? e.Location.Address : string.Empty,
                    TargetQuantity = e.MaxDonors,
                    CurrentQuantity = e.CurrentDonors,
                    Status = e.Status,
                    // Nếu muốn truyền thêm RequiredBloodTypes:
                    // RequiredBloodTypes = e.RequiredBloodTypes ?? string.Empty
                })
                .ToList();
            return View(events);
        }

        // Action mới để lấy 3 sự kiện gần nhất cho trang Home
        [HttpGet]
        public IActionResult GetTop3()
        {
            var top3Events = _context.BloodDonationEvents
                .Include(e => e.Location)
                .AsEnumerable()
                .OrderBy(e => e.EventDate.Add(e.StartTime))
                .Take(3)
                .Select(e => new BloodDonationEventDto
                {
                    Id = e.EventId,
                    Title = e.EventName,
                    Description = e.EventDescription ?? string.Empty,
                    StartDate = e.EventDate.Add(e.StartTime),
                    EndDate = e.EventDate.Add(e.EndTime),
                    LocationName = e.Location != null ? e.Location.LocationName : string.Empty,
                    Address = e.Location != null ? e.Location.Address : string.Empty,
                    TargetQuantity = e.MaxDonors,
                    CurrentQuantity = e.CurrentDonors,
                    Status = e.Status,
                })
                .ToList();
            return Json(top3Events);
        }
    }
} 