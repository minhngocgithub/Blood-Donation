using Blood_Donation_Website.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    [Authorize(Roles = "Bệnh viện, Quản trị viên")]
    [HospitalAdminOnly]
    [Route("admin/events")]
    public class EventManagementController : Controller
    {
        private readonly IBloodDonationEventService _eventService;
        private readonly ILocationService _locationService;

        public EventManagementController(
            IBloodDonationEventService eventService,
            ILocationService locationService)
        {
            _eventService = eventService;
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllEventsAsync();
            return View(events);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            ViewBag.Locations = locations;
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BloodDonationEventCreateDto eventDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdEvent = await _eventService.CreateEventAsync(eventDto);
                    TempData["SuccessMessage"] = "Sự kiện đã được tạo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var locations = await _locationService.GetAllLocationsAsync();
            ViewBag.Locations = locations;
            return View(eventDto);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var locations = await _locationService.GetAllLocationsAsync();
            ViewBag.Locations = locations;
            return View(eventItem);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BloodDonationEventUpdateDto eventDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _eventService.UpdateEventAsync(id, eventDto);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Sự kiện đã được cập nhật thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể cập nhật sự kiện.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var locations = await _locationService.GetAllLocationsAsync();
            ViewBag.Locations = locations;
            return View(eventDto);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _eventService.DeleteEventAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Sự kiện đã được xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xóa sự kiện.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        //HttpPost("status/{id}")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> UpdateStatus(int id, string status)
        // {
        //     try
        //     {
        //         var success = await _eventService.UpdateEventStatusAsync(id, status);
        //         if (success)
        //         {
        //             TempData["SuccessMessage] =Trạng thái sự kiện đã được cập nhật!";
        //         }
        //         else
        //         {
        //             TempData["ErrorMessage"] =Không thể cập nhật trạng thái sự kiện.";
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //             TempData["ErrorMessage] = ex.Message;
        //     }
        //
        //     return RedirectToAction(nameof(Index));
        // }
    }
} 