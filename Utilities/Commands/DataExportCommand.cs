using Blood_Donation_Website.Data;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Utilities.Commands
{
    /// <summary>
    /// Console command for exporting database data
    /// Usage: DataExportCommand.RunAsync(args)
    /// </summary>
    public static class DataExportCommand
    {
        public static async Task RunAsync(string[] args)
        {
            try
            {
                // Parse command line arguments
                var command = args.Length > 0 ? args[0].ToLower() : "help";
                var tableName = args.Length > 1 ? args[1] : "";
                var format = args.Length > 2 ? args[2].ToLower() : "json";

                // Setup configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .Build();

                // Setup database context
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Error: Connection string not found in configuration.");
                    return;
                }

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new ApplicationDbContext(optionsBuilder.Options);
                var exporter = new DataExporter(context);

                // Execute command
                switch (command)
                {
                    case "all":
                        Console.WriteLine("Starting full database export...");
                        if (format == "csv")
                        {
                            await exporter.ExportAllDataToCsvAsync();
                        }
                        else
                        {
                            await exporter.ExportAllDataAsync();
                        }
                        break;

                    case "table":
                        if (string.IsNullOrEmpty(tableName))
                        {
                            Console.WriteLine("Error: Table name is required for table export.");
                            ShowUsage();
                            return;
                        }
                        Console.WriteLine($"Starting export for table: {tableName}...");
                        await exporter.ExportTableAsync(tableName);
                        break;

                    case "help":
                    default:
                        ShowUsage();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Database Export Tool");
            Console.WriteLine("===================");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  DataExportCommand all [format]           - Export all tables (format: json/csv, default: json)");
            Console.WriteLine("  DataExportCommand table <tablename>      - Export specific table");
            Console.WriteLine("  DataExportCommand help                   - Show this help");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  DataExportCommand all                    - Export all data to JSON");
            Console.WriteLine("  DataExportCommand all csv                - Export all data to CSV");
            Console.WriteLine("  DataExportCommand table Users            - Export Users table only");
            Console.WriteLine("  DataExportCommand table BloodTypes       - Export BloodTypes table only");
            Console.WriteLine();
            Console.WriteLine("Supported tables:");
            Console.WriteLine("  Roles, Users, BloodTypes, BloodCompatibility, Locations,");
            Console.WriteLine("  BloodDonationEvents, DonationRegistrations, HealthScreening,");
            Console.WriteLine("  DonationHistory, NewsCategories, News, Notifications,");
            Console.WriteLine("  Settings, ContactMessages");
            Console.WriteLine();
            Console.WriteLine("Output: Files will be created in DatabaseExport folder with timestamp.");
        }
    }
}
