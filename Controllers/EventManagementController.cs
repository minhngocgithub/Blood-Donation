using Blood_Donation_Website.Utilities.Filters;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Utilities;
using Microsoft.AspNetCore.Mvc;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Controllers
{
    [HospitalAdminOnly]
    [Route("admin/events")]
    public class EventManagementController : Controller
    {
        private readonly IBloodDonationEventService _eventService;
        private readonly ILocationService _locationService;
        private readonly IBloodTypeService _bloodTypeService;
        private readonly IDonationRegistrationService _registrationService;
        private readonly IEmailService _emailService;
        private readonly DataExporter _dataExporter;

        public EventManagementController(
            IBloodDonationEventService eventService,
            ILocationService locationService,
            IBloodTypeService bloodTypeService,
            IDonationRegistrationService registrationService,
            IEmailService emailService,
            DataExporter dataExporter)
        {
            _eventService = eventService;
            _locationService = locationService;
            _bloodTypeService = bloodTypeService;
            _registrationService = registrationService;
            _emailService = emailService;
            _dataExporter = dataExporter;
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
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            ViewBag.Locations = locations;
            ViewBag.BloodTypes = bloodTypes;
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
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            ViewBag.Locations = locations;
            ViewBag.BloodTypes = bloodTypes;
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
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            ViewBag.Locations = locations;
            ViewBag.BloodTypes = bloodTypes;
            ViewBag.EventId = eventItem.EventId;
            ViewBag.CreatedBy = eventItem.CreatedBy;
            ViewBag.CreatedDate = eventItem.CreatedDate;
            ViewBag.CurrentDonors = eventItem.CurrentDonors;

            var updateDto = new BloodDonationEventUpdateDto
            {
                EventName = eventItem.EventName,
                EventDescription = eventItem.EventDescription,
                EventDate = eventItem.EventDate,
                StartTime = eventItem.StartTime,
                EndTime = eventItem.EndTime,
                LocationId = eventItem.LocationId,
                MaxDonors = eventItem.MaxDonors,
                Status = eventItem.Status,
                ImageUrl = eventItem.ImageUrl,
                RequiredBloodTypes = eventItem.RequiredBloodTypes
            };

            return View(updateDto);
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
            var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
            ViewBag.Locations = locations;
            ViewBag.BloodTypes = bloodTypes;
            return View(eventDto);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID sự kiện không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                TempData["ErrorMessage"] = $"Không tìm thấy sự kiện với ID: {id}";
                return RedirectToAction(nameof(Index));
            }

            var statistics = await _eventService.GetEventStatisticsAsync(id);
            ViewBag.Statistics = statistics;

            return View(eventItem);
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

        [HttpPost("status/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, EventStatus status)
        {
            try
            {
                bool success = false;
                switch (status)
                {
                    case EventStatus.Active:
                        success = await _eventService.ActivateEventAsync(id);
                        break;
                    case EventStatus.Completed:
                        success = await _eventService.CompleteEventAsync(id);
                        break;
                    case EventStatus.Cancelled:
                        success = await _eventService.CancelEventAsync(id);
                        break;
                    case EventStatus.Postponed:
                        success = await _eventService.DeactivateEventAsync(id);
                        break;
                    default:
                        TempData["ErrorMessage"] = "Trạng thái không hợp lệ.";
                        return RedirectToAction(nameof(Details), new { id });
                }

                if (success)
                {
                    TempData["SuccessMessage"] = "Trạng thái sự kiện đã được cập nhật thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật trạng thái sự kiện.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("reminders/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReminders(int id)
        {
            try
            {
                var eventItem = await _eventService.GetEventByIdAsync(id);
                if (eventItem == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sự kiện.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                // Lấy danh sách đăng ký cho sự kiện
                var registrations = await _registrationService.GetRegistrationsByEventAsync(id);
                var activeRegistrations = registrations.Where(r => 
                    r.Status == RegistrationStatus.Registered || 
                    r.Status == RegistrationStatus.Confirmed);

                if (!activeRegistrations.Any())
                {
                    TempData["WarningMessage"] = "Không có người đăng ký nào để gửi nhắc nhở.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                int sentCount = 0;
                int failedCount = 0;

                foreach (var registration in activeRegistrations)
                {
                    if (!string.IsNullOrEmpty(registration.UserEmail))
                    {
                        var success = await _emailService.SendEventReminderEmailAsync(
                            registration.UserEmail,
                            eventItem.EventName,
                            eventItem.EventDate
                        );

                        if (success)
                            sentCount++;
                        else
                            failedCount++;
                    }
                }

                if (sentCount > 0)
                {
                    TempData["SuccessMessage"] = $"Đã gửi nhắc nhở thành công cho {sentCount} người đăng ký.";
                    if (failedCount > 0)
                    {
                        TempData["WarningMessage"] = $"Không thể gửi nhắc nhở cho {failedCount} người đăng ký.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể gửi nhắc nhở cho bất kỳ ai.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi gửi nhắc nhở: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("export/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportEventData(int id, string format = "json")
        {
            try
            {
                var eventItem = await _eventService.GetEventByIdAsync(id);
                if (eventItem == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sự kiện.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                // Tạo thư mục xuất dữ liệu
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                // Tạo thư mục cho sự kiện cụ thể
                var eventExportPath = Path.Combine(dataExportPath, $"Event_{id}_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(eventExportPath);

                // Xuất thông tin sự kiện
                var eventData = new
                {
                    exportInfo = new
                    {
                        exportDate = DateTime.Now,
                        eventId = eventItem.EventId,
                        eventName = eventItem.EventName,
                        format = format
                    },
                    eventDetails = eventItem,
                    registrations = await _registrationService.GetRegistrationsByEventAsync(id),
                    statistics = await _eventService.GetEventStatisticsAsync(id)
                };

                string fileName;
                string filePath;
                string contentType;

                if (format.ToLower() == "csv")
                {
                    fileName = $"Event_{id}_Data_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                    filePath = Path.Combine(eventExportPath, fileName);
                    contentType = "text/csv";

                    // Tạo CSV content
                    var csvContent = new System.Text.StringBuilder();
                    csvContent.AppendLine("Event Information");
                    csvContent.AppendLine($"Event ID,{eventItem.EventId}");
                    csvContent.AppendLine($"Event Name,{eventItem.EventName}");
                    csvContent.AppendLine($"Event Date,{eventItem.EventDate:yyyy-MM-dd}");
                    csvContent.AppendLine($"Location,{eventItem.LocationName}");
                    csvContent.AppendLine($"Status,{eventItem.Status}");
                    csvContent.AppendLine($"Max Donors,{eventItem.MaxDonors}");
                    csvContent.AppendLine($"Current Donors,{eventItem.CurrentDonors}");
                    csvContent.AppendLine();

                    // Thêm thông tin đăng ký
                    var registrations = await _registrationService.GetRegistrationsByEventAsync(id);
                    csvContent.AppendLine("Registrations");
                    csvContent.AppendLine("Registration ID,User Name,User Email,Registration Date,Status,Check-in Time,Completion Time");
                    
                    foreach (var reg in registrations)
                    {
                        csvContent.AppendLine($"{reg.RegistrationId},{reg.UserName},{reg.UserEmail},{reg.RegistrationDate:yyyy-MM-dd HH:mm},{reg.Status},{reg.CheckInTime?.ToString("yyyy-MM-dd HH:mm") ?? ""},{reg.CompletionTime?.ToString("yyyy-MM-dd HH:mm") ?? ""}");
                    }

                    await System.IO.File.WriteAllTextAsync(filePath, csvContent.ToString());
                }
                else
                {
                    fileName = $"Event_{id}_Data_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    filePath = Path.Combine(eventExportPath, fileName);
                    contentType = "application/json";

                    var jsonContent = System.Text.Json.JsonSerializer.Serialize(eventData, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    await System.IO.File.WriteAllTextAsync(filePath, jsonContent);
                }

                // Đọc file và trả về
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xuất dữ liệu: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
} 