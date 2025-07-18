using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;

namespace Blood_Donation_Website.Controllers
{
    public class EventsController : Controller
    {
        private readonly IBloodDonationEventService _eventService;
        private readonly IDonationRegistrationService _registrationService;

        public EventsController(IBloodDonationEventService eventService, IDonationRegistrationService registrationService)
        {
            _eventService = eventService;
            _registrationService = registrationService;
        }

        public async Task<IActionResult> Index(string searchTerm, string location)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchTerm) || !string.IsNullOrWhiteSpace(location))
                {
                    var results = await _eventService.SearchEventsByNameDescLocationAsync(searchTerm, location);
                    return View(results);
                }
                else
                {
                    var searchDto = new EventSearchDto
                    {
                        Page = 1,
                        PageSize = 6, // Load 6 events initially
                        SortBy = "EventDate",
                        SortOrder = "asc"
                    };
                    var pagedResult = await _eventService.GetEventsPagedAsync(searchDto);
                    return View(pagedResult.Items);
                }
            }
            catch
            {
                return View(new List<BloodDonationEventDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreEvents(int page = 1, int pageSize = 6)
        {
            try
            {
                var searchDto = new EventSearchDto
                {
                    Page = page,
                    PageSize = pageSize,
                    SortBy = "EventDate",
                    SortOrder = "asc"
                };

                var pagedResult = await _eventService.GetEventsPagedAsync(searchDto);
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_EventCards", pagedResult.Items);
                }
                
                return Json(pagedResult.Items);
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
                bool userHasRegistered = false;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        var reg = await _registrationService.GetUserRegistrationForEventAsync(userId, id);
                        userHasRegistered = reg != null && (reg.Status == "Registered" || reg.Status == "CheckedIn");
                    }
                }
                ViewBag.UserHasRegistered = userHasRegistered;
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
                if (result != null && result.RegistrationId > 0)
                {
                    TempData["Success"] = "Đăng ký sự kiện thành công!";
                }
                else
                {
                    TempData["Error"] = "Đăng ký thất bại hoặc bạn đã đăng ký sự kiện này.";
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
