using Blood_Donation_Website.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    [Authorize(Roles = "Hospital,Admin")]
    [HospitalAdminOnly]
    [Route("admin/locations")]
    public class LocationManagementController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationManagementController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            return View(locations);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationCreateDto locationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdLocation = await _locationService.CreateLocationAsync(locationDto);
                    TempData["SuccessMessage"] = "Địa điểm đã được tạo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(locationDto);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LocationUpdateDto locationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _locationService.UpdateLocationAsync(id, locationDto);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Địa điểm đã được cập nhật thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể cập nhật địa điểm.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(locationDto);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _locationService.DeleteLocationAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Địa điểm đã được xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xóa địa điểm.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // [HttpPost("toggle-status/{id}")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> ToggleStatus(int id)
        // {
        //     try
        //     {
        //         var success = await _locationService.ToggleLocationStatusAsync(id);
        //         if (success)
        //         {
        //             TempData["SuccessMessage"] = "Trạng thái địa điểm đã được cập nhật!";
        //         }
        //         else
        //         {
        //             TempData["ErrorMessage"] = "Không thể cập nhật trạng thái địa điểm.";
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         TempData["ErrorMessage"] = ex.Message;
        //     }
        //
        //     return RedirectToAction(nameof(Index));
        // }
    }
} 