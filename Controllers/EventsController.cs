using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;

namespace Blood_Donation_Website.Controllers
{
    public class EventsController : Controller
    {
        private readonly IBloodDonationEventService _eventService;

        public EventsController(IBloodDonationEventService eventService)
        {
            _eventService = eventService;
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
            catch (Exception ex)
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
            catch (Exception ex)
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
                return View(eventDetails);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(int eventId)
        {
            try
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để đăng ký sự kiện" });
                }


                return Json(new { success = true, message = "Đăng ký sự kiện thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi đăng ký sự kiện" });
            }
        }
    }
}
