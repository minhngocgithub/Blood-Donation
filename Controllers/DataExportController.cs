using Blood_Donation_Website.Data;
using Blood_Donation_Website.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Blood_Donation_Website.Controllers.Admin
{
    [Authorize(Roles = "Quản trị viên")]
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
