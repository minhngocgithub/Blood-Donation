# Hướng dẫn sử dụng DataExporter

## Cách sử dụng nhanh

### 1. Trong Controller (Web Interface)

```csharp
public class AdminController : Controller
{
    private readonly DataExporter _dataExporter;
    
    public AdminController(DataExporter dataExporter)
    {
        _dataExporter = dataExporter;
    }
    
    [HttpGet]
    public async Task<IActionResult> ExportDatabase()
    {
        try
        {
            await _dataExporter.ExportAllDataAsync();
            TempData["SuccessMessage"] = "Export completed successfully!";
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["ErrorMessage"] = $"Export failed: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
```

### 2. Sử dụng Extension Methods

```csharp
// Trong bất kỳ service nào có ApplicationDbContext
public class SomeService
{
    private readonly ApplicationDbContext _context;
    
    public SomeService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task BackupData()
    {
        // Export toàn bộ database
        await _context.ExportAllDataAsync();
        
        // Export bảng cụ thể
        await _context.ExportTableAsync("Users");
        
        // Export ra CSV
        await _context.ExportAllDataToCsvAsync();
    }
}
```

### 3. Test trong Program.cs

Thêm vào Program.cs để test:

```csharp
// Sau khi setup context và seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Test export
    await DataExporterTest.TestDataExportAsync(context);
}
```

### 4. Command Line Usage

```bash
# Chạy từ project directory
dotnet run -- export all           # Export all tables
dotnet run -- export table Users   # Export Users table only
dotnet run -- export all csv       # Export all to CSV
```

## Cấu trúc Output

Files sẽ được tạo trong:
```
bin/Debug/net8.0/DatabaseExport/
├── Export_20250716_143022/
│   ├── ExportSummary.json
│   ├── Users.json
│   ├── Roles.json
│   └── [other tables...]
└── CSV_Export_20250716_143500/
    ├── Users.csv
    ├── Roles.csv
    └── [other tables...]
```

## Troubleshooting

**Q: Export files không được tạo?**
A: Kiểm tra quyền write trong thư mục bin, hoặc thay đổi `_exportDirectory` trong DataExporter.

**Q: Lỗi "Table not supported"?**
A: Kiểm tra tên bảng trong method `ExportTableAsync()`, case-sensitive.

**Q: Memory issues?**
A: Sử dụng export từng bảng thay vì export all với database lớn.

**Q: Navigation property null?**
A: Kiểm tra navigation property names trong Entity models và Include statements.
