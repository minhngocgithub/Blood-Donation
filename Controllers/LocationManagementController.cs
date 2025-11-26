using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý địa điểm hiến máu (chỉ dành cho Admin và Hospital)
    /// Xử lý: CRUD địa điểm, Kích hoạt/Vô hiệu hóa địa điểm
    /// Route: /admin/locations/*
    /// </summary>
    [HospitalAdminOnly] // Chỉ Admin hoặc Hospital được truy cập
    [Route("admin/locations")]
    public class LocationManagementController : Controller
    {
        private readonly ILocationService _locationService; // Service quản lý địa điểm
        
        /// <summary>
        /// Constructor - Inject LocationService
        /// </summary>
        public LocationManagementController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// GET: /admin/locations
        /// Hiển thị danh sách tất cả địa điểm hiến máu
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            return View(locations);
        }

        /// <summary>
        /// GET: /admin/locations/create
        /// Hiển thị form tạo địa điểm mới
        /// </summary>
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: /admin/locations/create
        /// Xử lý tạo địa điểm mới
        /// </summary>
        /// <param name="locationDto">Thông tin địa điểm (tên, địa chỉ, SĐT, sức chứa)</param>
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
        public async Task<IActionResult> Edit(int id, LocationDto locationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateDto = new LocationUpdateDto
                    {
                        LocationName = locationDto.LocationName,
                        Address = locationDto.Address,
                        ContactPhone = locationDto.ContactPhone,
                        Capacity = locationDto.Capacity,
                        IsActive = locationDto.IsActive
                    };

                    var success = await _locationService.UpdateLocationAsync(id, updateDto);
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

        /// <summary>
        /// POST: /admin/locations/toggle-status/{id}
        /// Chuyển đổi trạng thái hoạt động của địa điểm (Active ↔ Inactive)
        /// </summary>
        /// <param name="id">ID địa điểm</param>
        [HttpPost("toggle-status/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                // Lấy thông tin địa điểm hiện tại
                var location = await _locationService.GetLocationByIdAsync(id);
                if (location == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy địa điểm.";
                    return RedirectToAction(nameof(Index));
                }

                bool success;
                // Chuyển đổi trạng thái dựa trên trạng thái hiện tại
                if (location.IsActive)
                {
                    // Đang Active → Chuyển sang Inactive
                    success = await _locationService.DeactivateLocationAsync(id);
                }
                else
                {
                    // Đang Inactive → Chuyển sang Active
                    success = await _locationService.ActivateLocationAsync(id);
                }

                if (success)
                {
                    TempData["SuccessMessage"] = "Trạng thái địa điểm đã được cập nhật!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật trạng thái địa điểm.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 