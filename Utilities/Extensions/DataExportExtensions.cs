using Blood_Donation_Website.Data;

namespace Blood_Donation_Website.Utilities.Extensions
{
    public static class DataExportExtensions
    {
        /// <summary>
        /// Add DataExporter service to DI container
        /// </summary>
        public static IServiceCollection AddDataExporter(this IServiceCollection services)
        {
            services.AddScoped<DataExporter>();
            return services;
        }

        /// <summary>
        /// Export all database data using the context
        /// </summary>
        public static async Task ExportAllDataAsync(this ApplicationDbContext context)
        {
            var exporter = new DataExporter(context);
            await exporter.ExportAllDataAsync();
        }

        /// <summary>
        /// Export specific table data using the context
        /// </summary>
        public static async Task ExportTableAsync(this ApplicationDbContext context, string tableName)
        {
            var exporter = new DataExporter(context);
            await exporter.ExportTableAsync(tableName);
        }

        /// <summary>
        /// Export all data to CSV format using the context
        /// </summary>
        public static async Task ExportAllDataToCsvAsync(this ApplicationDbContext context)
        {
            var exporter = new DataExporter(context);
            await exporter.ExportAllDataToCsvAsync();
        }
    }
}
