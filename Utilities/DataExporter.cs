using Blood_Donation_Website.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Blood_Donation_Website.Utilities
{
    public class DataExporter
    {
        private readonly ApplicationDbContext _context;
        private readonly string _exportDirectory;

        public DataExporter(ApplicationDbContext context)
        {
            _context = context;
            // Use project root directory instead of assembly location
            _exportDirectory = GetProjectRootDirectory();
        }

        private static string GetProjectRootDirectory()
        {
            // Start from current directory and walk up to find project root
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = currentDir;
            
            // If we're in bin/Debug/net8.0, go up 3 levels to project root
            if (currentDir.Contains("bin"))
            {
                var binIndex = currentDir.LastIndexOf("bin");
                projectRoot = currentDir.Substring(0, binIndex).TrimEnd(Path.DirectorySeparatorChar);
            }
            
            return projectRoot;
        }

        /// <summary>
        /// Export all data from database to JSON files
        /// </summary>
        public async Task ExportAllDataAsync()
        {
            try
            {
                var exportPath = Path.Combine(_exportDirectory, "DatabaseExport");
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var exportFolder = Path.Combine(exportPath, $"Export_{timestamp}");
                Directory.CreateDirectory(exportFolder);

                // Export each table
                await ExportRolesAsync(exportFolder);
                await ExportUsersAsync(exportFolder);
                await ExportBloodTypesAsync(exportFolder);
                await ExportBloodCompatibilityAsync(exportFolder);
                await ExportLocationsAsync(exportFolder);
                await ExportBloodDonationEventsAsync(exportFolder);
                await ExportDonationRegistrationsAsync(exportFolder);
                await ExportHealthScreeningAsync(exportFolder);
                await ExportDonationHistoryAsync(exportFolder);
                await ExportNewsCategoriesAsync(exportFolder);
                await ExportNewsAsync(exportFolder);
                await ExportNotificationsAsync(exportFolder);
                await ExportSettingsAsync(exportFolder);
                await ExportContactMessagesAsync(exportFolder);

                // Create summary file
                await CreateExportSummaryAsync(exportFolder);

                Console.WriteLine($"Data export completed successfully to: {exportFolder}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during data export: {ex.Message}");
                throw;
            }
        }

        private async Task ExportRolesAsync(string exportFolder)
        {
            var roles = await _context.Roles.ToListAsync();
            var filePath = Path.Combine(exportFolder, "Roles.json");
            await WriteJsonFileAsync(filePath, roles);
        }

        private async Task ExportUsersAsync(string exportFolder)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.BloodType)
                .Select(u => new
                {
                    u.UserId,
                    u.Username,
                    u.Email,
                    u.FullName,
                    u.Phone,
                    u.Address,
                    u.DateOfBirth,
                    u.Gender,
                    BloodType = u.BloodType != null ? u.BloodType.BloodTypeName : null,
                    Role = u.Role != null ? u.Role.RoleName : null,
                    u.IsActive,
                    u.EmailVerified,
                    u.LastDonationDate,
                    u.CreatedDate,
                    u.UpdatedDate
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "Users.json");
            await WriteJsonFileAsync(filePath, users);
        }

        private async Task ExportBloodTypesAsync(string exportFolder)
        {
            var bloodTypes = await _context.BloodTypes.ToListAsync();
            var filePath = Path.Combine(exportFolder, "BloodTypes.json");
            await WriteJsonFileAsync(filePath, bloodTypes);
        }

        private async Task ExportBloodCompatibilityAsync(string exportFolder)
        {
            var compatibility = await _context.BloodCompatibilities
                .Include(bc => bc.FromBloodType)
                .Include(bc => bc.ToBloodType)
                .Select(bc => new
                {
                    bc.Id,
                    FromBloodType = bc.FromBloodType.BloodTypeName,
                    ToBloodType = bc.ToBloodType.BloodTypeName
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "BloodCompatibility.json");
            await WriteJsonFileAsync(filePath, compatibility);
        }

        private async Task ExportLocationsAsync(string exportFolder)
        {
            var locations = await _context.Locations.ToListAsync();
            var filePath = Path.Combine(exportFolder, "Locations.json");
            await WriteJsonFileAsync(filePath, locations);
        }

        private async Task ExportBloodDonationEventsAsync(string exportFolder)
        {
            var events = await _context.BloodDonationEvents
                .Include(e => e.Location)
                .Include(e => e.Creator)
                .Select(e => new
                {
                    e.EventId,
                    e.EventName,
                    e.EventDescription,
                    e.EventDate,
                    e.StartTime,
                    e.EndTime,
                    Location = e.Location != null ? e.Location.LocationName : null,
                    e.MaxDonors,
                    e.CurrentDonors,
                    e.Status,
                    e.ImageUrl,
                    e.RequiredBloodTypes,
                    CreatedBy = e.Creator != null ? e.Creator.FullName : null,
                    e.CreatedDate,
                    e.UpdatedDate
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "BloodDonationEvents.json");
            await WriteJsonFileAsync(filePath, events);
        }

        private async Task ExportDonationRegistrationsAsync(string exportFolder)
        {
            var registrations = await _context.DonationRegistrations
                .Include(dr => dr.User)
                .Include(dr => dr.Event)
                .Select(dr => new
                {
                    dr.RegistrationId,
                    User = dr.User.FullName,
                    Event = dr.Event.EventName,
                    dr.RegistrationDate,
                    dr.Status,
                    dr.Notes,
                    dr.IsEligible,
                    dr.CheckInTime,
                    dr.CompletionTime,
                    dr.CancellationReason
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "DonationRegistrations.json");
            await WriteJsonFileAsync(filePath, registrations);
        }

        private async Task ExportHealthScreeningAsync(string exportFolder)
        {
            var screenings = await _context.HealthScreenings
                .Include(hs => hs.Registration)
                .ThenInclude(r => r.User)
                .Include(hs => hs.ScreenedByUser)
                .Select(hs => new
                {
                    hs.ScreeningId,
                    User = hs.Registration.User.FullName,
                    hs.Weight,
                    hs.Height,
                    hs.BloodPressure,
                    hs.HeartRate,
                    hs.Temperature,
                    hs.Hemoglobin,
                    hs.IsEligible,
                    hs.DisqualifyReason,
                    ScreenedBy = hs.ScreenedByUser != null ? hs.ScreenedByUser.FullName : null,
                    hs.ScreeningDate
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "HealthScreening.json");
            await WriteJsonFileAsync(filePath, screenings);
        }

        private async Task ExportDonationHistoryAsync(string exportFolder)
        {
            var history = await _context.DonationHistories
                .Include(dh => dh.User)
                .Include(dh => dh.Event)
                .Include(dh => dh.BloodType)
                .Include(dh => dh.Registration)
                .Select(dh => new
                {
                    dh.DonationId,
                    User = dh.User.FullName,
                    Event = dh.Event.EventName,
                    dh.DonationDate,
                    BloodType = dh.BloodType.BloodTypeName,
                    dh.Volume,
                    dh.Status,
                    dh.Notes,
                    dh.NextEligibleDate,
                    dh.CertificateIssued
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "DonationHistory.json");
            await WriteJsonFileAsync(filePath, history);
        }

        private async Task ExportNewsCategoriesAsync(string exportFolder)
        {
            var categories = await _context.NewsCategories.ToListAsync();
            var filePath = Path.Combine(exportFolder, "NewsCategories.json");
            await WriteJsonFileAsync(filePath, categories);
        }

        private async Task ExportNewsAsync(string exportFolder)
        {
            var news = await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Select(n => new
                {
                    n.NewsId,
                    n.Title,
                    n.Content,
                    n.Summary,
                    n.ImageUrl,
                    Category = n.Category != null ? n.Category.CategoryName : null,
                    Author = n.Author != null ? n.Author.FullName : null,
                    n.ViewCount,
                    n.IsPublished,
                    n.PublishedDate,
                    n.CreatedDate,
                    n.UpdatedDate
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "News.json");
            await WriteJsonFileAsync(filePath, news);
        }

        private async Task ExportNotificationsAsync(string exportFolder)
        {
            var notifications = await _context.Notifications
                .Include(n => n.User)
                .Select(n => new
                {
                    n.NotificationId,
                    User = n.User != null ? n.User.FullName : "System",
                    n.Title,
                    n.Message,
                    n.Type,
                    n.IsRead,
                    n.CreatedDate
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "Notifications.json");
            await WriteJsonFileAsync(filePath, notifications);
        }

        private async Task ExportSettingsAsync(string exportFolder)
        {
            var settings = await _context.Settings.ToListAsync();
            var filePath = Path.Combine(exportFolder, "Settings.json");
            await WriteJsonFileAsync(filePath, settings);
        }

        private async Task ExportContactMessagesAsync(string exportFolder)
        {
            var messages = await _context.ContactMessages
                .Include(cm => cm.ResolvedByUser)
                .Select(cm => new
                {
                    cm.MessageId,
                    cm.FullName,
                    cm.Email,
                    cm.Phone,
                    cm.Subject,
                    cm.Message,
                    cm.Status,
                    cm.CreatedDate,
                    cm.ResolvedDate,
                    ResolvedBy = cm.ResolvedByUser != null ? cm.ResolvedByUser.FullName : null
                })
                .ToListAsync();

            var filePath = Path.Combine(exportFolder, "ContactMessages.json");
            await WriteJsonFileAsync(filePath, messages);
        }

        private async Task CreateExportSummaryAsync(string exportFolder)
        {
            var summary = new
            {
                ExportDate = DateTime.Now,
                ExportedTables = new
                {
                    Roles = await _context.Roles.CountAsync(),
                    Users = await _context.Users.CountAsync(),
                    BloodTypes = await _context.BloodTypes.CountAsync(),
                    BloodCompatibility = await _context.BloodCompatibilities.CountAsync(),
                    Locations = await _context.Locations.CountAsync(),
                    BloodDonationEvents = await _context.BloodDonationEvents.CountAsync(),
                    DonationRegistrations = await _context.DonationRegistrations.CountAsync(),
                    HealthScreening = await _context.HealthScreenings.CountAsync(),
                    DonationHistory = await _context.DonationHistories.CountAsync(),
                    NewsCategories = await _context.NewsCategories.CountAsync(),
                    News = await _context.News.CountAsync(),
                    Notifications = await _context.Notifications.CountAsync(),
                    Settings = await _context.Settings.CountAsync(),
                    ContactMessages = await _context.ContactMessages.CountAsync()
                },
                TotalRecords = await GetTotalRecordsAsync()
            };

            var filePath = Path.Combine(exportFolder, "ExportSummary.json");
            await WriteJsonFileAsync(filePath, summary);
        }

        private async Task<int> GetTotalRecordsAsync()
        {
            var total = 0;
            total += await _context.Roles.CountAsync();
            total += await _context.Users.CountAsync();
            total += await _context.BloodTypes.CountAsync();
            total += await _context.BloodCompatibilities.CountAsync();
            total += await _context.Locations.CountAsync();
            total += await _context.BloodDonationEvents.CountAsync();
            total += await _context.DonationRegistrations.CountAsync();
            total += await _context.HealthScreenings.CountAsync();
            total += await _context.DonationHistories.CountAsync();
            total += await _context.NewsCategories.CountAsync();
            total += await _context.News.CountAsync();
            total += await _context.Notifications.CountAsync();
            total += await _context.Settings.CountAsync();
            total += await _context.ContactMessages.CountAsync();
            return total;
        }

        private async Task WriteJsonFileAsync<T>(string filePath, T data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var jsonString = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(filePath, jsonString, Encoding.UTF8);
        }

        /// <summary>
        /// Export specific table data
        /// </summary>
        /// <param name="tableName">Name of the table to export</param>
        public async Task ExportTableAsync(string tableName)
        {
            var exportPath = Path.Combine(_exportDirectory, "DatabaseExport");
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var exportFolder = Path.Combine(exportPath, $"SingleTable_{timestamp}");
            Directory.CreateDirectory(exportFolder);

            switch (tableName.ToLower())
            {
                case "roles":
                    await ExportRolesAsync(exportFolder);
                    break;
                case "users":
                    await ExportUsersAsync(exportFolder);
                    break;
                case "bloodtypes":
                    await ExportBloodTypesAsync(exportFolder);
                    break;
                case "bloodcompatibility":
                    await ExportBloodCompatibilityAsync(exportFolder);
                    break;
                case "locations":
                    await ExportLocationsAsync(exportFolder);
                    break;
                case "blooddonationevents":
                    await ExportBloodDonationEventsAsync(exportFolder);
                    break;
                case "donationregistrations":
                    await ExportDonationRegistrationsAsync(exportFolder);
                    break;
                case "healthscreening":
                    await ExportHealthScreeningAsync(exportFolder);
                    break;
                case "donationhistory":
                    await ExportDonationHistoryAsync(exportFolder);
                    break;
                case "newscategories":
                    await ExportNewsCategoriesAsync(exportFolder);
                    break;
                case "news":
                    await ExportNewsAsync(exportFolder);
                    break;
                case "notifications":
                    await ExportNotificationsAsync(exportFolder);
                    break;
                case "settings":
                    await ExportSettingsAsync(exportFolder);
                    break;
                case "contactmessages":
                    await ExportContactMessagesAsync(exportFolder);
                    break;
                default:
                    throw new ArgumentException($"Table '{tableName}' is not supported for export.");
            }

            Console.WriteLine($"Table '{tableName}' exported successfully to: {exportFolder}");
        }

        /// <summary>
        /// Export data to CSV format
        /// </summary>
        public async Task ExportAllDataToCsvAsync()
        {
            var exportPath = Path.Combine(_exportDirectory, "DatabaseExport");
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var exportFolder = Path.Combine(exportPath, $"CSV_Export_{timestamp}");
            Directory.CreateDirectory(exportFolder);

            // Export each table to CSV
            await ExportTableToCsvAsync("Roles", exportFolder);
            await ExportTableToCsvAsync("Users", exportFolder);
            await ExportTableToCsvAsync("BloodTypes", exportFolder);
            await ExportTableToCsvAsync("Locations", exportFolder);
            await ExportTableToCsvAsync("Settings", exportFolder);

            Console.WriteLine($"CSV export completed successfully to: {exportFolder}");
        }

        private async Task ExportTableToCsvAsync(string tableName, string exportFolder)
        {
            var csvContent = new StringBuilder();
            var filePath = Path.Combine(exportFolder, $"{tableName}.csv");

            switch (tableName.ToLower())
            {
                case "roles":
                    var roles = await _context.Roles.ToListAsync();
                    csvContent.AppendLine("RoleId,RoleName,Description,CreatedDate");
                    foreach (var role in roles)
                    {
                        csvContent.AppendLine($"{role.RoleId},\"{role.RoleName}\",\"{role.Description}\",{role.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                    }
                    break;

                case "users":
                    var users = await _context.Users.Include(u => u.Role).Include(u => u.BloodType).ToListAsync();
                    csvContent.AppendLine("UserId,Username,Email,FullName,Phone,Gender,Role,BloodType,IsActive,EmailVerified,CreatedDate");
                    foreach (var user in users)
                    {
                        csvContent.AppendLine($"{user.UserId},\"{user.Username}\",\"{user.Email}\",\"{user.FullName}\",\"{user.Phone}\",\"{user.Gender}\",\"{user.Role?.RoleName}\",\"{user.BloodType?.BloodTypeName}\",{user.IsActive},{user.EmailVerified},{user.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                    }
                    break;

                case "bloodtypes":
                    var bloodTypes = await _context.BloodTypes.ToListAsync();
                    csvContent.AppendLine("BloodTypeId,BloodTypeName,Description");
                    foreach (var bt in bloodTypes)
                    {
                        csvContent.AppendLine($"{bt.BloodTypeId},\"{bt.BloodTypeName}\",\"{bt.Description}\"");
                    }
                    break;

                case "locations":
                    var locations = await _context.Locations.ToListAsync();
                    csvContent.AppendLine("LocationId,LocationName,Address,ContactPhone,Capacity,IsActive,CreatedDate");
                    foreach (var location in locations)
                    {
                        csvContent.AppendLine($"{location.LocationId},\"{location.LocationName}\",\"{location.Address}\",\"{location.ContactPhone}\",{location.Capacity},{location.IsActive},{location.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                    }
                    break;

                case "settings":
                    var settings = await _context.Settings.ToListAsync();
                    csvContent.AppendLine("SettingId,SettingKey,SettingValue,Description,UpdatedDate");
                    foreach (var setting in settings)
                    {
                        csvContent.AppendLine($"{setting.SettingId},\"{setting.SettingKey}\",\"{setting.SettingValue}\",\"{setting.Description}\",{setting.UpdatedDate:yyyy-MM-dd HH:mm:ss}");
                    }
                    break;
            }

            await File.WriteAllTextAsync(filePath, csvContent.ToString(), Encoding.UTF8);
        }
    }
}
