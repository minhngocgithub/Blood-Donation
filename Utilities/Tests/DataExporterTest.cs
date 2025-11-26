using Blood_Donation_Website.Data;
using Blood_Donation_Website.Utilities;
using Blood_Donation_Website.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Blood_Donation_Website.Tests
{
    /// <summary>
    /// Test class để kiểm tra DataExporter functionality
    /// Chạy trong Program.cs hoặc Controller để test
    /// </summary>
    public static class DataExporterTest
    {
        public static async Task TestDataExportAsync(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("=== DataExporter Test ===");
                Console.WriteLine("Starting test...");

                var exporter = new DataExporter(context);

                // Test 1: Export single table
                Console.WriteLine("\n1. Testing single table export (Roles)...");
                await exporter.ExportTableAsync("Roles");
                Console.WriteLine("✅ Single table export completed");

                // Test 2: Export all data
                Console.WriteLine("\n2. Testing full database export...");
                await exporter.ExportAllDataAsync();
                Console.WriteLine("✅ Full database export completed");

                // Test 3: Export to CSV
                Console.WriteLine("\n3. Testing CSV export...");
                await exporter.ExportAllDataToCsvAsync();
                Console.WriteLine("✅ CSV export completed");

                Console.WriteLine("\n=== All tests passed! ===");
                Console.WriteLine("Check the DatabaseExport folder for output files.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Test extension methods
        /// </summary>
        public static async Task TestExtensionMethodsAsync(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("\n=== Extension Methods Test ===");
                
                // Test extension methods
                Console.WriteLine("Testing extension methods...");
                await context.ExportTableAsync("Users");
                Console.WriteLine("✅ Extension method test completed");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Extension method test failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Test specific tables với data có sẵn
        /// </summary>
        public static async Task TestTablesWithDataAsync(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("\n=== Tables with Data Test ===");

                // Check which tables have data
                var tableStats = new
                {
                    Roles = await context.Roles.CountAsync(),
                    Users = await context.Users.CountAsync(),
                    BloodTypes = await context.BloodTypes.CountAsync(),
                    Locations = await context.Locations.CountAsync(),
                    Settings = await context.Settings.CountAsync()
                };

                Console.WriteLine("Table statistics:");
                Console.WriteLine($"- Roles: {tableStats.Roles} records");
                Console.WriteLine($"- Users: {tableStats.Users} records");
                Console.WriteLine($"- BloodTypes: {tableStats.BloodTypes} records");
                Console.WriteLine($"- Locations: {tableStats.Locations} records");
                Console.WriteLine($"- Settings: {tableStats.Settings} records");

                // Export tables that have data
                if (tableStats.Roles > 0)
                {
                    await context.ExportTableAsync("Roles");
                    Console.WriteLine("✅ Roles exported");
                }

                if (tableStats.Users > 0)
                {
                    await context.ExportTableAsync("Users");
                    Console.WriteLine("✅ Users exported");
                }

                Console.WriteLine("✅ Tables with data export completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Tables test failed: {ex.Message}");
            }
        }
    }
}
