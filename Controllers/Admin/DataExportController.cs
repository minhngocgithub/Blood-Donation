using Blood_Donation_Website.Data;
using Blood_Donation_Website.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                var tempPath = Path.GetTempPath();
                var exportId = Guid.NewGuid().ToString();
                var exportPath = Path.Combine(tempPath, $"DataExport_{exportId}");
                Directory.CreateDirectory(exportPath);

                if (format.ToLower() == "csv")
                {
                    await _dataExporter.ExportAllDataToCsvAsync();
                }
                else
                {
                    await _dataExporter.ExportAllDataAsync();
                }

                // Find the latest export folder
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    return NotFound("Export folder not found.");
                }

                var latestFolder = Directory.GetDirectories(dataExportPath)
                    .OrderByDescending(d => Directory.GetCreationTime(d))
                    .FirstOrDefault();

                if (latestFolder == null)
                {
                    return NotFound("No export data found.");
                }

                // Create ZIP file
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

                await _dataExporter.ExportTableAsync(tableName);

                // Find the latest export folder
                var dataExportPath = Path.Combine(Directory.GetCurrentDirectory(), "DatabaseExport");
                if (!Directory.Exists(dataExportPath))
                {
                    return NotFound("Export folder not found.");
                }

                var latestFolder = Directory.GetDirectories(dataExportPath)
                    .OrderByDescending(d => Directory.GetCreationTime(d))
                    .FirstOrDefault();

                if (latestFolder == null)
                {
                    return NotFound("No export data found.");
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
    }
}
