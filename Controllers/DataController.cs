using Blood_Donation_Website.Data;
using Blood_Donation_Website.Utilities;
using Blood_Donation_Website.Utilities.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text.Json;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý xuất/nhập dữ liệu (chỉ dành cho Admin)
    /// Xử lý: Xuất tất cả dữ liệu, Xuất theo bảng, Xuất theo sự kiện, Xuất có lọc
    /// Định dạng: JSON và CSV
    /// Chức năng: Import dữ liệu, Download file, Kiểm tra trạng thái
    /// Route: /admin/*
    /// </summary>
    [AdminOnly]
    [Route("admin")]
    public class DataController : Controller
    {
        // Dependencies
        private readonly ApplicationDbContext _context; // Database context
        private readonly DataExporter _dataExporter; // Service xuất dữ liệu

        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public DataController(ApplicationDbContext context, DataExporter dataExporter)
        {
            _context = context;
            _dataExporter = dataExporter;
        }

        /// <summary>
        /// GET: /admin/data-export
        /// Hiển thị trang quản lý xuất/nhập dữ liệu
        /// </summary>
        [HttpGet("data-export")]
        public IActionResult DataExport()
        {
            return View("DataExport");
        }

        /// <summary>
        /// POST: /admin/export-all
        /// Xuất tất cả dữ liệu trong database ra file ZIP
        /// Bao gồm tất cả các bảng: Users, Events, Registrations, Donations, HealthScreenings...
        /// Trả về file ZIP chứa tất cả các file JSON/CSV
        /// </summary>
        /// <param name="format">Định dạng xuất: json hoặc csv (mặc định: json)</param>
        [HttpPost("export-all")]
        public async Task<IActionResult> ExportAll(string format = "json")
        {
            try
            {                
                // Tạo thư mục xuất dữ liệu
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                // Xuất dữ liệu theo định dạng
                if (format.ToLower() == "csv")
                {
                    await _dataExporter.ExportAllDataToCsvAsync();
                }
                else
                {
                    await _dataExporter.ExportAllDataAsync();
                }

                // Tìm thư mục xuất mới nhất
                var directories = Directory.GetDirectories(dataExportPath);
                
                var latestFolder = directories
                    .OrderByDescending(d => Directory.GetCreationTime(d))
                    .FirstOrDefault();

                if (latestFolder == null)
                {
                    TempData["ErrorMessage"] = "No export data found. The export may have failed. Check logs for details.";
                    return RedirectToAction(nameof(Index));
                }

                var files = Directory.GetFiles(latestFolder);

                // Tạo file ZIP
                var tempPath = Path.GetTempPath();
                var zipPath = Path.Combine(tempPath, $"DatabaseExport_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                ZipFile.CreateFromDirectory(latestFolder, zipPath);

                // Đọc và trả về file
                var fileBytes = await System.IO.File.ReadAllBytesAsync(zipPath);
                
                System.IO.File.Delete(zipPath);
                
                return File(fileBytes, "application/zip", $"DatabaseExport_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /admin/export-table
        /// Xuất dữ liệu của một bảng cụ thể
        /// Tìm file mới nhất trong thư mục export và trả về
        /// </summary>
        /// <param name="tableName">Tên bảng cần xuất (VD: Users, Events, DonationHistories...)</param>
        /// <param name="format">Định dạng: json hoặc csv</param>
        [HttpPost("export-table")]
        public async Task<IActionResult> ExportTable(string tableName, string format = "json")
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    TempData["ErrorMessage"] = "Table name is required.";
                    return RedirectToAction(nameof(Index));
                }

                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                await _dataExporter.ExportTableAsync(tableName);

                var latestFolder = Directory.GetDirectories(dataExportPath)
                    .OrderByDescending(d => Directory.GetCreationTime(d))
                    .FirstOrDefault();

                if (latestFolder == null)
                {
                    return NotFound("No export data found. The export may have failed.");
                }

                var fileName = $"{tableName}.json";
                var filePath = Path.Combine(latestFolder, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound($"Export file for table '{tableName}' not found.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /admin/export-event
        /// Xuất tất cả dữ liệu liên quan đến một sự kiện cụ thể
        /// Bao gồm: Thông tin sự kiện, Đăng ký, Lịch sử hiến máu, Sàng lọc sức khỏe, Thống kê
        /// </summary>
        /// <param name="eventId">ID sự kiện cần xuất</param>
        /// <param name="format">Định dạng xuất: json hoặc csv (mặc định: json)</param>
        [HttpPost("export-event")]
        public async Task<IActionResult> ExportEventData(int eventId, string format = "json")
        {
            try
            {
                // Kiểm tra sự kiện có tồn tại không
                var eventEntity = await _context.BloodDonationEvents
                    .Include(e => e.Location)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity == null)
                {
                    TempData["ErrorMessage"] = "Sự kiện không tồn tại.";
                    return RedirectToAction("DataExport");
                }

                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                // Tạo thư mục cho sự kiện cụ thể
                var eventExportPath = Path.Combine(dataExportPath, $"Event_{eventId}_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(eventExportPath);

                if (format.ToLower() == "csv")
                {
                    await ExportEventToCsvAsync(eventEntity, eventExportPath);
                }
                else
                {
                    await ExportEventToJsonAsync(eventEntity, eventExportPath);
                }

                // Tạo file ZIP
                var tempPath = Path.GetTempPath();
                var zipPath = Path.Combine(tempPath, $"Event_{eventId}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                ZipFile.CreateFromDirectory(eventExportPath, zipPath);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(zipPath);
                
                // Dọn dẹp
                System.IO.File.Delete(zipPath);
                Directory.Delete(eventExportPath, true);
                
                return File(fileBytes, "application/zip", $"Event_{eventId}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction("DataExport");
            }
        }

        /// <summary>
        /// POST: /admin/export-filtered
        /// Xuất dữ liệu có lọc theo các tiêu chí
        /// Hỗ trợ lọc: Khoảng ngày, Nhóm máu, Địa điểm, Trạng thái
        /// Loại dữ liệu: donations, registrations, events, users
        /// </summary>
        /// <param name="fromDate">Lọc từ ngày (tùy chọn)</param>
        /// <param name="toDate">Lọc đến ngày (tùy chọn)</param>
        /// <param name="bloodTypeId">Lọc theo ID nhóm máu (tùy chọn)</param>
        /// <param name="locationId">Lọc theo ID địa điểm (tùy chọn)</param>
        /// <param name="status">Lọc theo trạng thái (tùy chọn)</param>
        /// <param name="dataType">Loại dữ liệu: donations, registrations, events, users (mặc định: donations)</param>
        /// <param name="format">Định dạng: json hoặc csv (mặc định: json)</param>
        [HttpPost("export-filtered")]
        public async Task<IActionResult> ExportFilteredData(
            DateTime? fromDate, 
            DateTime? toDate, 
            int? bloodTypeId, 
            int? locationId,
            string? status,
            string dataType = "donations",
            string format = "json")
        {
            try
            {
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                var exportPath = Path.Combine(dataExportPath, $"Filtered_{dataType}_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(exportPath);

                if (format.ToLower() == "csv")
                {
                    await ExportFilteredToCsvAsync(fromDate, toDate, bloodTypeId, locationId, status, dataType, exportPath);
                }
                else
                {
                    await ExportFilteredToJsonAsync(fromDate, toDate, bloodTypeId, locationId, status, dataType, exportPath);
                }

                var tempPath = Path.GetTempPath();
                var zipPath = Path.Combine(tempPath, $"Filtered_{dataType}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                ZipFile.CreateFromDirectory(exportPath, zipPath);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(zipPath);
                
                System.IO.File.Delete(zipPath);
                Directory.Delete(exportPath, true);
                
                return File(fileBytes, "application/zip", $"Filtered_{dataType}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction("DataExport");
            }
        }

        /// <summary>
        /// Helper method: Xuất dữ liệu có lọc ra file JSON
        /// Hỗ trợ 4 loại dữ liệu: donations, registrations, events, users
        /// Tự động bao gồm thông tin xuất (exportInfo) và dữ liệu đã lọc
        /// </summary>
        private async Task ExportFilteredToJsonAsync(
            DateTime? fromDate, 
            DateTime? toDate, 
            int? bloodTypeId, 
            int? locationId,
            string? status,
            string dataType,
            string exportPath)
        {
            object data = null;
            string fileName = "";

            switch (dataType.ToLower())
            {
                case "donations":
                    // Xuất lịch sử hiến máu
                    var donations = _context.DonationHistories
                        .Include(d => d.User).ThenInclude(u => u.BloodType)
                        .Include(d => d.Registration).ThenInclude(r => r.Event).ThenInclude(e => e.Location)
                        .AsQueryable();

                    // Áp dụng các bộ lọc
                    if (fromDate.HasValue)
                        donations = donations.Where(d => d.DonationDate >= fromDate.Value);
                    if (toDate.HasValue)
                        donations = donations.Where(d => d.DonationDate <= toDate.Value);
                    if (bloodTypeId.HasValue)
                        donations = donations.Where(d => d.User.BloodTypeId == bloodTypeId.Value);
                    if (locationId.HasValue)
                        donations = donations.Where(d => d.Registration.Event.LocationId == locationId.Value);

                    var donationList = await donations.ToListAsync();
                    data = new
                    {
                        exportInfo = new
                        {
                            exportDate = DateTime.Now,
                            dataType = "Donation Histories",
                            filters = new
                            {
                                fromDate,
                                toDate,
                                bloodTypeId,
                                locationId
                            },
                            totalRecords = donationList.Count
                        },
                        data = donationList.Select(d => new
                        {
                            donationId = d.DonationId,
                            donationDate = d.DonationDate,
                            userId = d.UserId,
                            userName = d.User?.FullName,
                            bloodType = d.User?.BloodType?.BloodTypeName,
                            volume = d.Volume,
                            eventName = d.Registration?.Event?.EventName,
                            location = d.Registration?.Event?.Location?.LocationName,
                            notes = d.Notes
                        })
                    };
                    fileName = "DonationHistories.json";
                    break;

                case "registrations":
                    var registrations = _context.DonationRegistrations
                        .Include(r => r.User).ThenInclude(u => u.BloodType)
                        .Include(r => r.Event).ThenInclude(e => e.Location)
                        .AsQueryable();

                    if (fromDate.HasValue)
                        registrations = registrations.Where(r => r.RegistrationDate >= fromDate.Value);
                    if (toDate.HasValue)
                        registrations = registrations.Where(r => r.RegistrationDate <= toDate.Value);
                    if (bloodTypeId.HasValue)
                        registrations = registrations.Where(r => r.User.BloodTypeId == bloodTypeId.Value);
                    if (locationId.HasValue)
                        registrations = registrations.Where(r => r.Event.LocationId == locationId.Value);
                    if (!string.IsNullOrEmpty(status))
                        registrations = registrations.Where(r => r.Status.ToString() == status);

                    var registrationList = await registrations.ToListAsync();
                    data = new
                    {
                        exportInfo = new
                        {
                            exportDate = DateTime.Now,
                            dataType = "Donation Registrations",
                            filters = new
                            {
                                fromDate,
                                toDate,
                                bloodTypeId,
                                locationId,
                                status
                            },
                            totalRecords = registrationList.Count
                        },
                        data = registrationList.Select(r => new
                        {
                            registrationId = r.RegistrationId,
                            registrationDate = r.RegistrationDate,
                            userId = r.UserId,
                            userName = r.User?.FullName,
                            bloodType = r.User?.BloodType?.BloodTypeName,
                            eventName = r.Event?.EventName,
                            location = r.Event?.Location?.LocationName,
                            status = r.Status.ToString(),
                            isEligible = r.IsEligible,
                            checkInTime = r.CheckInTime,
                            notes = r.Notes
                        })
                    };
                    fileName = "DonationRegistrations.json";
                    break;

                case "events":
                    var events = _context.BloodDonationEvents
                        .Include(e => e.Location)
                        .AsQueryable();

                    if (fromDate.HasValue)
                        events = events.Where(e => e.EventDate >= fromDate.Value);
                    if (toDate.HasValue)
                        events = events.Where(e => e.EventDate <= toDate.Value);
                    if (locationId.HasValue)
                        events = events.Where(e => e.LocationId == locationId.Value);
                    if (!string.IsNullOrEmpty(status))
                        events = events.Where(e => e.Status.ToString() == status);

                    var eventList = await events.ToListAsync();
                    data = new
                    {
                        exportInfo = new
                        {
                            exportDate = DateTime.Now,
                            dataType = "Blood Donation Events",
                            filters = new
                            {
                                fromDate,
                                toDate,
                                locationId,
                                status
                            },
                            totalRecords = eventList.Count
                        },
                        data = eventList.Select(e => new
                        {
                            eventId = e.EventId,
                            eventName = e.EventName,
                            eventDate = e.EventDate,
                            startTime = e.StartTime,
                            endTime = e.EndTime,
                            location = e.Location?.LocationName,
                            maxDonors = e.MaxDonors,
                            currentDonors = e.CurrentDonors,
                            status = e.Status.ToString(),
                            requiredBloodTypes = e.RequiredBloodTypes
                        })
                    };
                    fileName = "BloodDonationEvents.json";
                    break;

                case "users":
                    var users = _context.Users
                        .Include(u => u.BloodType)
                        .Include(u => u.Role)
                        .AsQueryable();

                    if (bloodTypeId.HasValue)
                        users = users.Where(u => u.BloodTypeId == bloodTypeId.Value);

                    var userList = await users.ToListAsync();
                    data = new
                    {
                        exportInfo = new
                        {
                            exportDate = DateTime.Now,
                            dataType = "Users",
                            filters = new
                            {
                                bloodTypeId
                            },
                            totalRecords = userList.Count
                        },
                        data = userList.Select(u => new
                        {
                            userId = u.UserId,
                            fullName = u.FullName,
                            email = u.Email,
                            phone = u.Phone,
                            bloodType = u.BloodType?.BloodTypeName,
                            role = u.Role?.RoleName,
                            dateOfBirth = u.DateOfBirth,
                            gender = u.Gender,
                            address = u.Address,
                            createdDate = u.CreatedDate
                        })
                    };
                    fileName = "Users.json";
                    break;
            }

            var filePath = Path.Combine(exportPath, fileName);
            var jsonContent = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(filePath, jsonContent);
        }

        /// <summary>
        /// Helper method: Xuất dữ liệu có lọc ra file CSV
        /// Hỗ trợ 4 loại dữ liệu: donations, registrations, events, users
        /// Tạo file CSV với header và dữ liệu đã lọc
        /// </summary>
        private async Task ExportFilteredToCsvAsync(
            DateTime? fromDate, 
            DateTime? toDate, 
            int? bloodTypeId, 
            int? locationId,
            string? status,
            string dataType,
            string exportPath)
        {
            var csv = new System.Text.StringBuilder();
            string fileName = "";

            switch (dataType.ToLower())
            {
                case "donations":
                    // Xuất lịch sử hiến máu ra CSV
                    var donations = _context.DonationHistories
                        .Include(d => d.User).ThenInclude(u => u.BloodType)
                        .Include(d => d.Registration).ThenInclude(r => r.Event).ThenInclude(e => e.Location)
                        .AsQueryable();

                    // Áp dụng các bộ lọc
                    if (fromDate.HasValue) donations = donations.Where(d => d.DonationDate >= fromDate.Value);
                    if (toDate.HasValue) donations = donations.Where(d => d.DonationDate <= toDate.Value);
                    if (bloodTypeId.HasValue) donations = donations.Where(d => d.User.BloodTypeId == bloodTypeId.Value);
                    if (locationId.HasValue) donations = donations.Where(d => d.Registration.Event.LocationId == locationId.Value);

                    csv.AppendLine("Donation ID,Donation Date,User ID,User Name,Blood Type,Volume,Event Name,Location,Notes");
                    foreach (var d in await donations.ToListAsync())
                    {
                        csv.AppendLine($"{d.DonationId},{d.DonationDate:yyyy-MM-dd HH:mm},{d.UserId},{d.User?.FullName},{d.User?.BloodType?.BloodTypeName},{d.Volume},{d.Registration?.Event?.EventName},{d.Registration?.Event?.Location?.LocationName},{d.Notes ?? ""}");
                    }
                    fileName = "DonationHistories.csv";
                    break;

                case "registrations":
                    var registrations = _context.DonationRegistrations
                        .Include(r => r.User).ThenInclude(u => u.BloodType)
                        .Include(r => r.Event).ThenInclude(e => e.Location)
                        .AsQueryable();

                    if (fromDate.HasValue) registrations = registrations.Where(r => r.RegistrationDate >= fromDate.Value);
                    if (toDate.HasValue) registrations = registrations.Where(r => r.RegistrationDate <= toDate.Value);
                    if (bloodTypeId.HasValue) registrations = registrations.Where(r => r.User.BloodTypeId == bloodTypeId.Value);
                    if (locationId.HasValue) registrations = registrations.Where(r => r.Event.LocationId == locationId.Value);
                    if (!string.IsNullOrEmpty(status)) registrations = registrations.Where(r => r.Status.ToString() == status);

                    csv.AppendLine("Registration ID,Registration Date,User ID,User Name,Blood Type,Event Name,Location,Status,Is Eligible,Check-in Time,Notes");
                    foreach (var r in await registrations.ToListAsync())
                    {
                        csv.AppendLine($"{r.RegistrationId},{r.RegistrationDate:yyyy-MM-dd HH:mm},{r.UserId},{r.User?.FullName},{r.User?.BloodType?.BloodTypeName},{r.Event?.EventName},{r.Event?.Location?.LocationName},{r.Status},{r.IsEligible},{r.CheckInTime?.ToString("yyyy-MM-dd HH:mm") ?? ""},{r.Notes ?? ""}");
                    }
                    fileName = "DonationRegistrations.csv";
                    break;

                case "events":
                    var events = _context.BloodDonationEvents.Include(e => e.Location).AsQueryable();

                    if (fromDate.HasValue) events = events.Where(e => e.EventDate >= fromDate.Value);
                    if (toDate.HasValue) events = events.Where(e => e.EventDate <= toDate.Value);
                    if (locationId.HasValue) events = events.Where(e => e.LocationId == locationId.Value);
                    if (!string.IsNullOrEmpty(status)) events = events.Where(e => e.Status.ToString() == status);

                    csv.AppendLine("Event ID,Event Name,Event Date,Start Time,End Time,Location,Max Donors,Current Donors,Status,Required Blood Types");
                    foreach (var e in await events.ToListAsync())
                    {
                        csv.AppendLine($"{e.EventId},{e.EventName},{e.EventDate:yyyy-MM-dd},{e.StartTime},{e.EndTime},{e.Location?.LocationName},{e.MaxDonors},{e.CurrentDonors},{e.Status},{e.RequiredBloodTypes}");
                    }
                    fileName = "BloodDonationEvents.csv";
                    break;

                case "users":
                    var users = _context.Users.Include(u => u.BloodType).Include(u => u.Role).AsQueryable();

                    if (bloodTypeId.HasValue) users = users.Where(u => u.BloodTypeId == bloodTypeId.Value);

                    csv.AppendLine("User ID,Full Name,Email,Phone,Blood Type,Role,Date of Birth,Gender,Address,Created Date");
                    foreach (var u in await users.ToListAsync())
                    {
                        csv.AppendLine($"{u.UserId},{u.FullName},{u.Email},{u.Phone},{u.BloodType?.BloodTypeName},{u.Role?.RoleName},{u.DateOfBirth:yyyy-MM-dd},{u.Gender},{u.Address},{u.CreatedDate:yyyy-MM-dd}");
                    }
                    fileName = "Users.csv";
                    break;
            }

            var filePath = Path.Combine(exportPath, fileName);
            await System.IO.File.WriteAllTextAsync(filePath, csv.ToString());
        }

        /// <summary>
        /// Helper method: Xuất dữ liệu sự kiện ra file JSON
        /// Bao gồm: Chi tiết sự kiện, Đăng ký, Lịch sử hiến máu, Sàng lọc sức khỏe, Thống kê
        /// </summary>
        private async Task ExportEventToJsonAsync(BloodDonationEvent eventEntity, string exportPath)
        {
            // Lấy dữ liệu đăng ký
            var registrations = await _context.DonationRegistrations
                .Include(r => r.User)
                .Where(r => r.EventId == eventEntity.EventId)
                .ToListAsync();

            // Lấy dữ liệu lịch sử hiến máu
            var donationHistories = await _context.DonationHistories
                .Include(d => d.User)
                .Include(d => d.Registration)
                .Where(d => d.Registration!.EventId == eventEntity.EventId)
                .ToListAsync();

            // Lấy dữ liệu sàng lọc sức khỏe
            var healthScreenings = await _context.HealthScreenings
                .Include(h => h.Registration)
                .Include(h => h.Registration.User)
                .Where(h => h.Registration.EventId == eventEntity.EventId)
                .ToListAsync();

            var eventData = new
            {
                exportInfo = new
                {
                    exportDate = DateTime.Now,
                    eventId = eventEntity.EventId,
                    eventName = eventEntity.EventName,
                    format = "json"
                },
                eventDetails = new
                {
                    eventId = eventEntity.EventId,
                    eventName = eventEntity.EventName,
                    eventDescription = eventEntity.EventDescription,
                    eventDate = eventEntity.EventDate,
                    startTime = eventEntity.StartTime,
                    endTime = eventEntity.EndTime,
                    locationName = eventEntity.Location?.LocationName,
                    locationAddress = eventEntity.Location?.Address,
                    maxDonors = eventEntity.MaxDonors,
                    currentDonors = eventEntity.CurrentDonors,
                    status = eventEntity.Status,
                    requiredBloodTypes = eventEntity.RequiredBloodTypes,
                    imageUrl = eventEntity.ImageUrl,
                    createdDate = eventEntity.CreatedDate,
                    updatedDate = eventEntity.UpdatedDate
                },
                registrations = registrations.Select(r => new
                {
                    registrationId = r.RegistrationId,
                    userId = r.UserId,
                    userName = r.User?.FullName,
                    userEmail = r.User?.Email,
                    userPhone = r.User?.Phone,
                    registrationDate = r.RegistrationDate,
                    status = r.Status,
                    notes = r.Notes,
                    isEligible = r.IsEligible,
                    checkInTime = r.CheckInTime,
                    completionTime = r.CompletionTime,
                    cancellationReason = r.CancellationReason
                }),
                donationHistories = donationHistories.Select(d => new
                {
                    donationId = d.DonationId,
                    userId = d.UserId,
                    userName = d.User?.FullName,
                    donationDate = d.DonationDate,
                    bloodVolume = d.Volume,
                    bloodType = d.User?.BloodType?.BloodTypeName,
                    notes = d.Notes
                }),
                healthScreenings = healthScreenings.Select(h => new
                {
                    screeningId = h.ScreeningId,
                    userId = h.Registration.UserId,
                    userName = h.Registration.User?.FullName,
                    screeningDate = h.ScreeningDate,
                    isEligible = h.IsEligible,
                    bloodPressure = h.BloodPressure,
                    heartRate = h.HeartRate,
                    temperature = h.Temperature,
                    hemoglobin = h.Hemoglobin,
                    weight = h.Weight,
                    disqualifyReason = h.DisqualifyReason,
                    screenedBy = h.ScreenedByUser?.FullName
                }),
                statistics = new
                {
                    totalRegistrations = registrations.Count,
                    completedDonations = donationHistories.Count,
                    eligibleScreenings = healthScreenings.Count(h => h.IsEligible),
                    ineligibleScreenings = healthScreenings.Count(h => !h.IsEligible),
                    completionRate = eventEntity.MaxDonors > 0 ? (double)donationHistories.Count / eventEntity.MaxDonors * 100 : 0
                }
            };

            var fileName = $"Event_{eventEntity.EventId}_Data_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(exportPath, fileName);

            var jsonContent = System.Text.Json.JsonSerializer.Serialize(eventData, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            await System.IO.File.WriteAllTextAsync(filePath, jsonContent);
        }

        /// <summary>
        /// Helper method: Xuất dữ liệu sự kiện ra file CSV
        /// Tạo file CSV với các phần: Event Info, Statistics, Registrations, Donation Histories, Health Screenings
        /// </summary>
        private async Task ExportEventToCsvAsync(BloodDonationEvent eventEntity, string exportPath)
        {
            // Lấy dữ liệu đăng ký
            var registrations = await _context.DonationRegistrations
                .Include(r => r.User)
                .Where(r => r.EventId == eventEntity.EventId)
                .ToListAsync();

            // Lấy dữ liệu lịch sử hiến máu
            var donationHistories = await _context.DonationHistories
                .Include(d => d.User)
                .Include(d => d.Registration)
                .Where(d => d.Registration!.EventId == eventEntity.EventId)
                .ToListAsync();

            // Lấy dữ liệu sàng lọc sức khỏe
            var healthScreenings = await _context.HealthScreenings
                .Include(h => h.Registration)
                .Include(h => h.Registration.User)
                .Where(h => h.Registration.EventId == eventEntity.EventId)
                .ToListAsync();

            var csvContent = new System.Text.StringBuilder();

            // Thông tin sự kiện
            csvContent.AppendLine("=== EVENT INFORMATION ===");
            csvContent.AppendLine($"Event ID,{eventEntity.EventId}");
            csvContent.AppendLine($"Event Name,{eventEntity.EventName}");
            csvContent.AppendLine($"Event Date,{eventEntity.EventDate:yyyy-MM-dd}");
            csvContent.AppendLine($"Start Time,{eventEntity.StartTime}");
            csvContent.AppendLine($"End Time,{eventEntity.EndTime}");
            csvContent.AppendLine($"Location,{eventEntity.Location?.LocationName}");
            csvContent.AppendLine($"Address,{eventEntity.Location?.Address}");
            csvContent.AppendLine($"Max Donors,{eventEntity.MaxDonors}");
            csvContent.AppendLine($"Current Donors,{eventEntity.CurrentDonors}");
            csvContent.AppendLine($"Status,{eventEntity.Status}");
            csvContent.AppendLine($"Required Blood Types,{eventEntity.RequiredBloodTypes}");
            csvContent.AppendLine($"Created Date,{eventEntity.CreatedDate:yyyy-MM-dd HH:mm}");
            csvContent.AppendLine($"Updated Date,{eventEntity.UpdatedDate:yyyy-MM-dd HH:mm}");
            csvContent.AppendLine();

            // Thống kê
            csvContent.AppendLine("=== STATISTICS ===");
            csvContent.AppendLine($"Total Registrations,{registrations.Count}");
            csvContent.AppendLine($"Completed Donations,{donationHistories.Count}");
            csvContent.AppendLine($"Eligible Screenings,{healthScreenings.Count(h => h.IsEligible)}");
            csvContent.AppendLine($"Ineligible Screenings,{healthScreenings.Count(h => !h.IsEligible)}");
            csvContent.AppendLine($"Completion Rate,{(eventEntity.MaxDonors > 0 ? (double)donationHistories.Count / eventEntity.MaxDonors * 100 : 0):F1}%");
            csvContent.AppendLine();

            // Danh sách đăng ký
            csvContent.AppendLine("=== REGISTRATIONS ===");
            csvContent.AppendLine("Registration ID,User ID,User Name,User Email,User Phone,Registration Date,Status,Is Eligible,Check-in Time,Completion Time,Cancellation Reason,Notes");
            
            foreach (var reg in registrations)
            {
                csvContent.AppendLine($"{reg.RegistrationId},{reg.UserId},{reg.User?.FullName},{reg.User?.Email},{reg.User?.Phone},{reg.RegistrationDate:yyyy-MM-dd HH:mm},{reg.Status},{reg.IsEligible},{reg.CheckInTime?.ToString("yyyy-MM-dd HH:mm") ?? ""},{reg.CompletionTime?.ToString("yyyy-MM-dd HH:mm") ?? ""},{reg.CancellationReason ?? ""},{reg.Notes ?? ""}");
            }
            csvContent.AppendLine();

            // Lịch sử hiến máu
            csvContent.AppendLine("=== DONATION HISTORIES ===");
            csvContent.AppendLine("Donation ID,User ID,User Name,Blood Type,Donation Date,Blood Volume,Notes");
            
            foreach (var donation in donationHistories)
            {
                csvContent.AppendLine($"{donation.DonationId},{donation.UserId},{donation.User?.FullName},{donation.User?.BloodType?.BloodTypeName},{donation.DonationDate:yyyy-MM-dd HH:mm},{donation.Volume},{donation.Notes ?? ""}");
            }
            csvContent.AppendLine();

            // Sàng lọc sức khỏe
            csvContent.AppendLine("=== HEALTH SCREENINGS ===");
            csvContent.AppendLine("Screening ID,User ID,User Name,Screening Date,Is Eligible,Blood Pressure,Heart Rate,Temperature,Hemoglobin,Weight,Notes,Screened By");
            
            foreach (var screening in healthScreenings)
            {
                csvContent.AppendLine($"{screening.ScreeningId},{screening.Registration.UserId},{screening.Registration.User?.FullName},{screening.ScreeningDate:yyyy-MM-dd HH:mm},{screening.IsEligible},{screening.BloodPressure},{screening.HeartRate},{screening.Temperature},{screening.Hemoglobin},{screening.Weight},{screening.DisqualifyReason?.ToString() ?? ""},{screening.ScreenedByUser?.FullName ?? ""}");
            }

            var fileName = $"Event_{eventEntity.EventId}_Data_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(exportPath, fileName);
            await System.IO.File.WriteAllTextAsync(filePath, csvContent.ToString());
        }

        /// <summary>
        /// POST: /admin/import-all
        /// Nhập tất cả dữ liệu từ file ZIP
        /// File ZIP phải chứa các file JSON của các bảng
        /// Tự động phát hiện và import từng bảng theo thứ tự
        /// </summary>
        /// <param name="file">File ZIP chứa dữ liệu export</param>
        [HttpPost("import-all")]
        public async Task<IActionResult> ImportAll(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please select a file to import.";
                    return RedirectToAction(nameof(Index));
                }

                if (!file.FileName.EndsWith(".zip"))
                {
                    TempData["ErrorMessage"] = "Please select a ZIP file containing JSON data.";
                    return RedirectToAction(nameof(Index));
                }

                var tempPath = Path.GetTempPath();
                var extractPath = Path.Combine(tempPath, $"Import_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(extractPath);

                using (var stream = file.OpenReadStream())
                {
                    var tempZipPath = Path.Combine(Path.GetTempPath(), $"temp_import_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                    using (var fileStream = new FileStream(tempZipPath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                    
                    ZipFile.ExtractToDirectory(tempZipPath, extractPath);
                    System.IO.File.Delete(tempZipPath);
                }

                var jsonFiles = Directory.GetFiles(extractPath, "*.json", SearchOption.AllDirectories);
                var importResults = new List<string>();

                foreach (var jsonFile in jsonFiles)
                {
                    var tableName = Path.GetFileNameWithoutExtension(jsonFile);
                    var result = await ImportTableFromFile(jsonFile, tableName);
                    importResults.Add($"{tableName}: {result}");
                }

                Directory.Delete(extractPath, true);

                TempData["SuccessMessage"] = $"Import completed! Results: {string.Join(", ", importResults)}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Import failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /admin/import-table
        /// Nhập dữ liệu của một bảng cụ thể từ file JSON
        /// File phải đúng định dạng với cấu trúc "data" array
        /// </summary>
        /// <param name="file">File JSON chứa dữ liệu bảng</param>
        /// <param name="tableName">Tên bảng cần import</param>
        [HttpPost("import-table")]
        public async Task<IActionResult> ImportTable(IFormFile file, string tableName)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please select a file to import.";
                    return RedirectToAction(nameof(Index));
                }

                if (!file.FileName.EndsWith(".json"))
                {
                    TempData["ErrorMessage"] = "Please select a JSON file.";
                    return RedirectToAction(nameof(Index));
                }

                if (string.IsNullOrEmpty(tableName))
                {
                    TempData["ErrorMessage"] = "Table name is required.";
                    return RedirectToAction(nameof(Index));
                }

                var tempPath = Path.GetTempPath();
                var tempFile = Path.Combine(tempPath, $"{tableName}_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                
                using (var stream = new FileStream(tempFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var result = await ImportTableFromFile(tempFile, tableName);

                if (System.IO.File.Exists(tempFile))
                {
                    System.IO.File.Delete(tempFile);
                }

                TempData["SuccessMessage"] = $"Import completed! {result}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Import failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Helper method: Import dữ liệu từ file JSON vào bảng cụ thể
        /// Hỗ trợ nhiều bảng: Users, Roles, BloodTypes, Locations, Events, Registrations...
        /// Tự động parse JSON và map vào các entity tương ứng
        /// </summary>
        /// <param name="filePath">Đường dẫn đến file JSON</param>
        /// <param name="tableName">Tên bảng cần import</param>
        /// <returns>Thông báo kết quả import</returns>
        private async Task<string> ImportTableFromFile(string filePath, string tableName)
        {
            try
            {
                var jsonContent = await System.IO.File.ReadAllTextAsync(filePath);
                var data = JsonSerializer.Deserialize<JsonElement>(jsonContent);

                if (!data.TryGetProperty("data", out var dataArray))
                {
                    return "Invalid JSON format - missing 'data' property";
                }

                var records = dataArray.EnumerateArray().ToList();
                var importedCount = 0;
                var updatedCount = 0;
                var errorCount = 0;

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    switch (tableName.ToLower())
                    {
                        case "users":
                            foreach (var record in records)
                            {
                                var user = JsonSerializer.Deserialize<User>(record.GetRawText());
                                var existingUser = await _context.Users.FindAsync(user!.UserId);
                                
                                if (existingUser == null)
                                {
                                    await _context.Users.AddAsync(user);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingUser).CurrentValues.SetValues(user);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "roles":
                            foreach (var record in records)
                            {
                                var role = JsonSerializer.Deserialize<Role>(record.GetRawText());
                                var existingRole = await _context.Roles.FindAsync(role!.RoleId);
                                
                                if (existingRole == null)
                                {
                                    await _context.Roles.AddAsync(role);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingRole).CurrentValues.SetValues(role);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "bloodtypes":
                            foreach (var record in records)
                            {
                                var bloodType = JsonSerializer.Deserialize<BloodType>(record.GetRawText());
                                var existingBloodType = await _context.BloodTypes.FindAsync(bloodType!.BloodTypeId);
                                
                                if (existingBloodType == null)
                                {
                                    await _context.BloodTypes.AddAsync(bloodType);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingBloodType).CurrentValues.SetValues(bloodType);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "locations":
                            foreach (var record in records)
                            {
                                var location = JsonSerializer.Deserialize<Location>(record.GetRawText());
                                var existingLocation = await _context.Locations.FindAsync(location!.LocationId);
                                
                                if (existingLocation == null)
                                {
                                    await _context.Locations.AddAsync(location);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingLocation).CurrentValues.SetValues(location);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "blooddonationevents":
                            foreach (var record in records)
                            {
                                var event_ = JsonSerializer.Deserialize<BloodDonationEvent>(record.GetRawText());
                                var existingEvent = await _context.BloodDonationEvents.FindAsync(event_!.EventId);
                                
                                if (existingEvent == null)
                                {
                                    await _context.BloodDonationEvents.AddAsync(event_);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingEvent).CurrentValues.SetValues(event_);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "donationregistrations":
                            foreach (var record in records)
                            {
                                var registration = JsonSerializer.Deserialize<DonationRegistration>(record.GetRawText());
                                var existingRegistration = await _context.DonationRegistrations.FindAsync(registration!.RegistrationId);
                                
                                if (existingRegistration == null)
                                {
                                    await _context.DonationRegistrations.AddAsync(registration);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingRegistration).CurrentValues.SetValues(registration);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "donationhistories":
                            foreach (var record in records)
                            {
                                var history = JsonSerializer.Deserialize<DonationHistory>(record.GetRawText());
                                var existingHistory = await _context.DonationHistories.FindAsync(history!.DonationId);
                                
                                if (existingHistory == null)
                                {
                                    await _context.DonationHistories.AddAsync(history);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingHistory).CurrentValues.SetValues(history);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "healthscreenings":
                            foreach (var record in records)
                            {
                                var screening = JsonSerializer.Deserialize<HealthScreening>(record.GetRawText());
                                var existingScreening = await _context.HealthScreenings.FindAsync(screening!.ScreeningId);
                                
                                if (existingScreening == null)
                                {
                                    await _context.HealthScreenings.AddAsync(screening);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingScreening).CurrentValues.SetValues(screening);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "news":
                            foreach (var record in records)
                            {
                                var news = JsonSerializer.Deserialize<News>(record.GetRawText());
                                var existingNews = await _context.News.FindAsync(news!.NewsId);
                                
                                if (existingNews == null)
                                {
                                    await _context.News.AddAsync(news);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingNews).CurrentValues.SetValues(news);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "contactmessages":
                            foreach (var record in records)
                            {
                                var message = JsonSerializer.Deserialize<ContactMessage>(record.GetRawText());
                                var existingMessage = await _context.ContactMessages.FindAsync(message!.MessageId);
                                
                                if (existingMessage == null)
                                {
                                    await _context.ContactMessages.AddAsync(message);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingMessage).CurrentValues.SetValues(message);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "newscategories":
                            foreach (var record in records)
                            {
                                var category = JsonSerializer.Deserialize<NewsCategory>(record.GetRawText());
                                var existingCategory = await _context.NewsCategories.FindAsync(category!.CategoryId);
                                
                                if (existingCategory == null)
                                {
                                    await _context.NewsCategories.AddAsync(category);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingCategory).CurrentValues.SetValues(category);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "notifications":
                            foreach (var record in records)
                            {
                                var notification = JsonSerializer.Deserialize<Notification>(record.GetRawText());
                                var existingNotification = await _context.Notifications.FindAsync(notification!.NotificationId);
                                
                                if (existingNotification == null)
                                {
                                    await _context.Notifications.AddAsync(notification);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingNotification).CurrentValues.SetValues(notification);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "settings":
                            foreach (var record in records)
                            {
                                var setting = JsonSerializer.Deserialize<Setting>(record.GetRawText());
                                var existingSetting = await _context.Settings.FindAsync(setting!.SettingId);
                                
                                if (existingSetting == null)
                                {
                                    await _context.Settings.AddAsync(setting);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingSetting).CurrentValues.SetValues(setting);
                                    updatedCount++;
                                }
                            }
                            break;

                        case "bloodcompatibility":
                            foreach (var record in records)
                            {
                                var compatibility = JsonSerializer.Deserialize<BloodCompatibility>(record.GetRawText());
                                var existingCompatibility = await _context.BloodCompatibilities.FindAsync(compatibility!.Id);
                                
                                if (existingCompatibility == null)
                                {
                                    await _context.BloodCompatibilities.AddAsync(compatibility);
                                    importedCount++;
                                }
                                else
                                {
                                    _context.Entry(existingCompatibility).CurrentValues.SetValues(compatibility);
                                    updatedCount++;
                                }
                            }
                            break;

                        default:
                            return $"Unsupported table: {tableName}";
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return $"Imported: {importedCount}, Updated: {updatedCount}, Errors: {errorCount}";
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// GET: /admin/download/{fileName}
        /// Tải file đã export từ thư mục DatabaseExport
        /// Tìm file mới nhất khớp với tên file
        /// </summary>
        /// <param name="fileName">Tên file cần tải</param>
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                var filePath = Directory.GetFiles(dataExportPath, fileName, SearchOption.AllDirectories).FirstOrDefault();

                if (filePath == null || !System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var contentType = fileName.EndsWith(".json") ? "application/json" : "text/csv";
                
                return File(fileBytes, contentType, fileName);
            }
            catch 
            {
                return NotFound("Error downloading file.");
            }
        }

        /// <summary>
        /// GET: /admin/status
        /// Lấy trạng thái xuất dữ liệu
        /// Trả về thông tin về thư mục export mới nhất và danh sách file
        /// </summary>
        [HttpGet("status")]
        public IActionResult GetExportStatus()
        {
            try
            {
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    return Json(new { hasExports = false, exports = new object[0] });
                }

                var exports = Directory.GetDirectories(dataExportPath)
                    .Select(d => new
                    {
                        name = Path.GetFileName(d),
                        createdDate = Directory.GetCreationTime(d),
                        fileCount = Directory.GetFiles(d).Length,
                        size = Directory.GetFiles(d).Sum(f => new FileInfo(f).Length)
                    })
                    .OrderByDescending(e => e.createdDate)
                    .Take(10)
                    .ToArray();

                return Json(new { hasExports = exports.Length > 0, exports });
            }
            catch (Exception ex)
            {
                return Json(new { hasExports = false, exports = new object[0], error = ex.Message });
            }
        }

        [HttpPost("run-tests")]
        public async Task<IActionResult> RunTests()
        {
            try
            {                
                await Blood_Donation_Website.Tests.DataExporterTest.TestDataExportAsync(_context);
                await Blood_Donation_Website.Tests.DataExporterTest.TestExtensionMethodsAsync(_context);
                await Blood_Donation_Website.Tests.DataExporterTest.TestTablesWithDataAsync(_context);
                
                TempData["SuccessMessage"] = "All tests completed successfully! Check the DatabaseExport folder and console output.";
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Tests failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-export")]
        public async Task<IActionResult> TestExport()
        {
            try
            {
                await Blood_Donation_Website.Tests.DataExporterTest.TestDataExportAsync(_context);
                TempData["SuccessMessage"] = "Export test completed successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-extensions")]
        public async Task<IActionResult> TestExtensions()
        {
            try
            {
                await Blood_Donation_Website.Tests.DataExporterTest.TestExtensionMethodsAsync(_context);
                TempData["SuccessMessage"] = "Extension methods test completed successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Extension test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-simple")]
        public async Task<IActionResult> TestSimple()
        {
            try
            {
                var userCount = await _context.Users.CountAsync();
                var roleCount = await _context.Roles.CountAsync();
 
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }
                
                if (roleCount > 0)
                {
                    await _dataExporter.ExportTableAsync("Roles");
                    
                    var directories = Directory.GetDirectories(dataExportPath);
                    var latestFolder = directories.OrderByDescending(d => Directory.GetCreationTime(d)).FirstOrDefault();
                    
                    if (latestFolder != null)
                    {
                        var files = Directory.GetFiles(latestFolder);
                        TempData["SuccessMessage"] = $"Simple test passed! Created {files.Length} files. Check console for details.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Export folder was not created. Check DataExporter implementation.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "No roles found in database. Please seed data first.";
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Simple test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /admin/test-tables
        /// Test xuất dữ liệu từng bảng
        /// Kiểm tra tính toàn vẹn dữ liệu
        /// </summary>
        [HttpPost("test-tables")]
        public async Task<IActionResult> TestTables()
        {
            try
            {
                await Blood_Donation_Website.Tests.DataExporterTest.TestTablesWithDataAsync(_context);
                TempData["SuccessMessage"] = "Tables test completed successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Tables test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
