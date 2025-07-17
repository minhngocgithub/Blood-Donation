using Blood_Donation_Website.Data;
using Blood_Donation_Website.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Blood_Donation_Website.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("admin/data-export")]
    public class DataExportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DataExporter _dataExporter;
        private readonly ILogger<DataExportController> _logger;

        public DataExportController(ApplicationDbContext context, DataExporter dataExporter, ILogger<DataExportController> logger)
        {
            _context = context;
            _dataExporter = dataExporter;
            _logger = logger;
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
                _logger.LogInformation("Starting export all data with format: {Format}", format);
                
                // Ensure DatabaseExport directory exists
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                    _logger.LogInformation("Created DatabaseExport directory: {Path}", dataExportPath);
                }

                // Perform export
                if (format.ToLower() == "csv")
                {
                    _logger.LogInformation("Exporting to CSV...");
                    await _dataExporter.ExportAllDataToCsvAsync();
                }
                else
                {
                    _logger.LogInformation("Exporting to JSON...");
                    await _dataExporter.ExportAllDataAsync();
                }

                _logger.LogInformation("Export completed, checking for output files...");

                // Find the latest export folder
                var directories = Directory.GetDirectories(dataExportPath);
                _logger.LogInformation("Found {Count} directories in export path", directories.Length);
                
                var latestFolder = directories
                    .OrderByDescending(d => Directory.GetCreationTime(d))
                    .FirstOrDefault();

                if (latestFolder == null)
                {
                    _logger.LogWarning("No export folder found after export operation");
                    TempData["ErrorMessage"] = "No export data found. The export may have failed. Check logs for details.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogInformation("Latest export folder: {Folder}", latestFolder);
                var files = Directory.GetFiles(latestFolder);
                _logger.LogInformation("Found {Count} files in latest folder", files.Length);

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
                _logger.LogError(ex, "Error during data export");
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
                    _logger.LogInformation("Created DatabaseExport directory: {Path}", dataExportPath);
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
                _logger.LogError(ex, "Error during table export for {TableName}", tableName);
                TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file {FileName}", fileName);
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
                _logger.LogError(ex, "Error getting export status");
                return Json(new { hasExports = false, exports = new object[0], error = ex.Message });
            }
        }

        [HttpPost("run-tests")]
        public async Task<IActionResult> RunTests()
        {
            try
            {
                _logger.LogInformation("Starting DataExporter tests...");
                
                // Chạy các test methods
                await Blood_Donation_Website.Tests.DataExporterTest.TestDataExportAsync(_context);
                await Blood_Donation_Website.Tests.DataExporterTest.TestExtensionMethodsAsync(_context);
                await Blood_Donation_Website.Tests.DataExporterTest.TestTablesWithDataAsync(_context);
                
                TempData["SuccessMessage"] = "All tests completed successfully! Check the DatabaseExport folder and console output.";
                _logger.LogInformation("DataExporter tests completed successfully");
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during DataExporter tests");
                TempData["ErrorMessage"] = $"Tests failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-export")]
        public async Task<IActionResult> TestExport()
        {
            try
            {
                _logger.LogInformation("Starting export test...");
                await Blood_Donation_Website.Tests.DataExporterTest.TestDataExportAsync(_context);
                TempData["SuccessMessage"] = "Export test completed successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during export test");
                TempData["ErrorMessage"] = $"Export test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-extensions")]
        public async Task<IActionResult> TestExtensions()
        {
            try
            {
                _logger.LogInformation("Starting extension methods test...");
                await Blood_Donation_Website.Tests.DataExporterTest.TestExtensionMethodsAsync(_context);
                TempData["SuccessMessage"] = "Extension methods test completed successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during extension test");
                TempData["ErrorMessage"] = $"Extension test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-simple")]
        public async Task<IActionResult> TestSimple()
        {
            try
            {
                _logger.LogInformation("Starting simple test...");
                
                // Test database connection
                var userCount = await _context.Users.CountAsync();
                var roleCount = await _context.Roles.CountAsync();
                
                _logger.LogInformation("Database test - Users: {UserCount}, Roles: {RoleCount}", userCount, roleCount);
                
                // Test directory creation
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    Directory.CreateDirectory(dataExportPath);
                }
                
                _logger.LogInformation("Export directory: {Path}", dataExportPath);
                
                // Simple test - try to export roles table
                if (roleCount > 0)
                {
                    _logger.LogInformation("Attempting to export Roles table...");
                    await _dataExporter.ExportTableAsync("Roles");
                    
                    // Check if file was created
                    var directories = Directory.GetDirectories(dataExportPath);
                    var latestFolder = directories.OrderByDescending(d => Directory.GetCreationTime(d)).FirstOrDefault();
                    
                    if (latestFolder != null)
                    {
                        var files = Directory.GetFiles(latestFolder);
                        _logger.LogInformation("Export successful! Created {Count} files in {Folder}", files.Length, latestFolder);
                        TempData["SuccessMessage"] = $"Simple test passed! Created {files.Length} files. Check console for details.";
                    }
                    else
                    {
                        _logger.LogWarning("No export folder created");
                        TempData["ErrorMessage"] = "Export folder was not created. Check DataExporter implementation.";
                    }
                }
                else
                {
                    _logger.LogWarning("No roles found in database");
                    TempData["ErrorMessage"] = "No roles found in database. Please seed data first.";
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during simple test");
                TempData["ErrorMessage"] = $"Simple test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("test-tables")]
        public async Task<IActionResult> TestTables()
        {
            try
            {
                _logger.LogInformation("Starting tables test...");
                await Blood_Donation_Website.Tests.DataExporterTest.TestTablesWithDataAsync(_context);
                TempData["SuccessMessage"] = "Tables test completed successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during tables test");
                TempData["ErrorMessage"] = $"Tables test failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
