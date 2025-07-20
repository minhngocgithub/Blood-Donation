using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using static Blood_Donation_Website.Utilities.EnumMapper;
using Blood_Donation_Website.Models.ViewModels;

namespace Blood_Donation_Website.Controllers
{
    public class EventsController : Controller
    {
        private readonly IBloodDonationEventService _eventService;
        private readonly IDonationRegistrationService _registrationService;
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IBloodTypeService _bloodTypeService;

        public EventsController(IBloodDonationEventService eventService, IDonationRegistrationService registrationService, IDonationHistoryService donationHistoryService, IBloodTypeService bloodTypeService)
        {
            _eventService = eventService;
            _registrationService = registrationService;
            _donationHistoryService = donationHistoryService;
            _bloodTypeService = bloodTypeService;
        }

        public async Task<IActionResult> Index(string searchTerm, string location, DateTime? fromDate, DateTime? toDate, string? bloodType)
        {
            try
            {
                // Get all blood types for filter dropdown
                var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
                ViewBag.BloodTypes = bloodTypes;

                // Create search DTO with filters
                var searchDto = new EventSearchDto
                {
                    SearchTerm = searchTerm,
                    FromDate = fromDate,
                    ToDate = toDate,
                    RequiredBloodTypes = bloodType,
                    Page = 1,
                    PageSize = 50, // Get more events to group them
                    SortBy = "EventDate",
                    SortOrder = "asc"
                };

                var allEvents = await _eventService.GetEventsPagedAsync(searchDto);
                var currentDate = DateTime.Now.Date;

                // Group events into current/upcoming and past
                var currentAndUpcomingEvents = allEvents.Items
                    .Where(e => e.EventDate >= currentDate)
                    .OrderBy(e => e.EventDate)
                    .ToList();

                var pastEvents = allEvents.Items
                    .Where(e => e.EventDate < currentDate)
                    .OrderByDescending(e => e.EventDate)
                    .ToList();

                // Create view model
                var viewModel = new EventsViewModel
                {
                    CurrentAndUpcomingEvents = currentAndUpcomingEvents,
                    PastEvents = pastEvents,
                    SearchTerm = searchTerm,
                    Location = location,
                    FromDate = fromDate,
                    ToDate = toDate,
                    SelectedBloodType = bloodType,
                    TotalCurrentEvents = currentAndUpcomingEvents.Count,
                    TotalPastEvents = pastEvents.Count
                };

                return View(viewModel);
            }
            catch
            {
                return View(new EventsViewModel
                {
                    CurrentAndUpcomingEvents = new List<BloodDonationEventDto>(),
                    PastEvents = new List<BloodDonationEventDto>()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreEvents(int page = 1, int pageSize = 6, string? eventType = "current")
        {
            try
            {
                var searchDto = new EventSearchDto
                {
                    Page = page,
                    PageSize = pageSize,
                    SortBy = "EventDate",
                    SortOrder = eventType == "past" ? "desc" : "asc"
                };

                var pagedResult = await _eventService.GetEventsPagedAsync(searchDto);
                var currentDate = DateTime.Now.Date;

                // Filter based on event type
                var filteredEvents = eventType == "past" 
                    ? pagedResult.Items.Where(e => e.EventDate < currentDate).ToList()
                    : pagedResult.Items.Where(e => e.EventDate >= currentDate).ToList();
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_EventCards", filteredEvents);
                }
                
                return Json(filteredEvents);
            }
            catch
            {
                return Json(new List<BloodDonationEventDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var eventDetails = await _eventService.GetEventByIdAsync(id);
                if (eventDetails == null)
                {
                    return NotFound();
                }
                DonationRegistrationDto? userRegistration = null;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        userRegistration = await _registrationService.GetUserRegistrationForEventAsync(userId, id);
                        var nextEligibleDate = await _donationHistoryService.GetUserNextEligibleDateAsync(userId);
                        int? daysLeft = null;
                        string eligibleMessage = string.Empty;
                        if (nextEligibleDate.HasValue)
                        {
                            var now = DateTime.Now.Date;
                            if (now >= nextEligibleDate.Value.Date)
                            {
                                eligibleMessage = "Bạn đã có thể hiến máu.";
                            }
                            else
                            {
                                daysLeft = (nextEligibleDate.Value.Date - now).Days;
                                eligibleMessage = $"Bạn cần chờ {daysLeft} ngày nữa để hiến máu.";
                            }
                        }
                        else
                        {
                            eligibleMessage = "Bạn đã có thể hiến máu.";
                        }
                        ViewBag.NextEligibleDate = nextEligibleDate;
                        ViewBag.DaysLeft = daysLeft;
                        ViewBag.EligibleMessage = eligibleMessage;
                    }
                }
                ViewBag.UserRegistration = userRegistration;
                return View(eventDetails);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đăng ký sự kiện.";
                return RedirectToAction("Details", new { id = eventId });
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                TempData["Error"] = "Không xác định được người dùng.";
                return RedirectToAction("Details", new { id = eventId });
            }

            try
            {
                var createDto = new DonationRegistrationCreateDto
                {
                    UserId = userId,
                    EventId = eventId,
                    Notes = null
                };
                var result = await _registrationService.CreateRegistrationAsync(createDto);
                if (result != null && result.Status == RegistrationStatus.Confirmed)
                {
                    TempData["Success"] = "Đăng ký sự kiện thành công!";
                }
                else
                {
                    TempData["Error"] = result?.CancellationReason ?? "Đăng ký thất bại hoặc bạn đã đăng ký sự kiện này.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Details", new { id = eventId });
        }
    }
}
