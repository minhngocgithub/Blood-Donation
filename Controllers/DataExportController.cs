using Blood_Donation_Website.Data;
using Blood_Donation_Website.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text.Json;
using Blood_Donation_Website.Models.Entities;

namespace Blood_Donation_Website.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("admin/data-export")]
    public class DataExportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DataExporter _dataExporter;

        public DataExportController(ApplicationDbContext context, DataExporter dataExporter)
        {
            _context = context;
            _dataExporter = dataExporter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("export-all")]
        public async Task<IActionResult> ExportAll(string format = "json")
        {
            try
            {                
                // Ensure DatabaseExport directory exists
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                // Perform export
                if (format.ToLower() == "csv")
                {
                    await _dataExporter.ExportAllDataToCsvAsync();
                }
                else
                {
                    await _dataExporter.ExportAllDataAsync();
                }

                // Find the latest export folder
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

                // Create ZIP file
                var tempPath = Path.GetTempPath();
                var zipPath = Path.Combine(tempPath, $"DatabaseExport_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                ZipFile.CreateFromDirectory(latestFolder, zipPath);

                var fileBytes = await System.IO.File.ReadAllBytesAsync(zipPath);
                
                // Cleanup
                System.IO.File.Delete(zipPath);
                
                return File(fileBytes, "application/zip", $"DatabaseExport_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

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

                // Ensure DatabaseExport directory exists
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }

                await _dataExporter.ExportTableAsync(tableName);

                // Find the latest export folder
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

                // Extract ZIP file
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

                // Import all JSON files
                var jsonFiles = Directory.GetFiles(extractPath, "*.json", SearchOption.AllDirectories);
                var importResults = new List<string>();

                foreach (var jsonFile in jsonFiles)
                {
                    var tableName = Path.GetFileNameWithoutExtension(jsonFile);
                    var result = await ImportTableFromFile(jsonFile, tableName);
                    importResults.Add($"{tableName}: {result}");
                }

                // Cleanup
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

                // Save uploaded file temporarily
                var tempPath = Path.GetTempPath();
                var tempFile = Path.Combine(tempPath, $"{tableName}_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                
                using (var stream = new FileStream(tempFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var result = await ImportTableFromFile(tempFile, tableName);

                // Cleanup
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

                        case "bloodcompatibilities":
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
                // Chạy các test methods
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
                // Test database connection
                var userCount = await _context.Users.CountAsync();
                var roleCount = await _context.Roles.CountAsync();
                                
                // Test directory creation
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }
                
                // Simple test - try to export roles table
                if (roleCount > 0)
                {
                    await _dataExporter.ExportTableAsync("Roles");
                    
                    // Check if file was created
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
