# Database Export Tool

Công cụ xuất dữ liệu database cho hệ thống Blood Donation Website.

## Tính năng

- ✅ Export toàn bộ database ra JSON/CSV
- ✅ Export từng bảng riêng lẻ  
- ✅ Export qua Web Interface (Admin)
- ✅ Export qua Command Line
- ✅ Export qua Extension Methods
- ✅ Tự động tạo folder với timestamp
- ✅ Include relationships trong JSON export
- ✅ Tạo file summary với thống kê

## Cách sử dụng

### 1. Qua Web Interface (Admin Panel)

```
URL: /admin/data-export
```

**Yêu cầu:** Đăng nhập với quyền Admin

**Chức năng:**
- Export toàn bộ database
- Export từng bảng riêng lẻ
- Download file ZIP
- Xem trạng thái export

### 2. Qua Extension Methods

```csharp
// Trong Controller hoặc Service
using Blood_Donation_Website.Utilities.Extensions;

// Export toàn bộ database
await _context.ExportAllDataAsync();

// Export bảng cụ thể
await _context.ExportTableAsync("Users");

// Export ra CSV
await _context.ExportAllDataToCsvAsync();
```

### 3. Qua Dependency Injection

```csharp
// Trong Controller
public class SomeController : Controller
{
    private readonly DataExporter _dataExporter;
    
    public SomeController(DataExporter dataExporter)
    {
        _dataExporter = dataExporter;
    }
    
    public async Task<IActionResult> Export()
    {
        await _dataExporter.ExportAllDataAsync();
        return Ok("Export completed");
    }
}
```

### 4. Qua Command Line

```bash
# Export toàn bộ database ra JSON
DataExportCommand all

# Export toàn bộ database ra CSV  
DataExportCommand all csv

# Export bảng cụ thể
DataExportCommand table Users
DataExportCommand table BloodTypes

# Hiển thị help
DataExportCommand help
```

## Cấu trúc Output

### JSON Export
```
DatabaseExport/
└── Export_20250716_143022/
    ├── ExportSummary.json          # Thống kê tổng quan
    ├── Roles.json                  # Dữ liệu roles
    ├── Users.json                  # Dữ liệu users (bao gồm role, blood type)
    ├── BloodTypes.json             # Dữ liệu nhóm máu
    ├── BloodCompatibility.json     # Tương thích nhóm máu
    ├── Locations.json              # Địa điểm
    ├── BloodDonationEvents.json    # Sự kiện hiến máu
    ├── DonationRegistrations.json  # Đăng ký hiến máu
    ├── HealthScreening.json        # Sàng lọc sức khỏe
    ├── DonationHistory.json        # Lịch sử hiến máu
    ├── NewsCategories.json         # Danh mục tin tức
    ├── News.json                   # Tin tức
    ├── Notifications.json          # Thông báo
    ├── Settings.json               # Cài đặt hệ thống
    └── ContactMessages.json        # Tin nhắn liên hệ
```

### CSV Export
```
DatabaseExport/
└── CSV_Export_20250716_143022/
    ├── Roles.csv
    ├── Users.csv
    ├── BloodTypes.csv
    ├── Locations.csv
    └── Settings.csv
```

## Đặc điểm JSON Export

- **Include Relationships**: Tự động join với related tables
- **User-friendly names**: Hiển thị tên thay vì ID
- **UTF-8 Encoding**: Hỗ trợ tiếng Việt
- **Pretty Format**: JSON được format đẹp và dễ đọc

### Ví dụ JSON User Record:
```json
{
  "userId": 1,
  "username": "admin",
  "email": "admin@blooddonation.com",
  "fullName": "System Administrator",
  "phone": "0123456789",
  "address": "Hanoi, Vietnam",
  "dateOfBirth": "1990-01-01",
  "gender": "Male",
  "bloodType": "O+",
  "role": "Admin",
  "isActive": true,
  "emailVerified": true,
  "lastDonationDate": null,
  "createdDate": "2025-07-16T14:30:22.123",
  "updatedDate": "2025-07-16T14:30:22.123"
}
```

## Bảng được hỗ trợ

| Tên Bảng | Mô tả | Có Relationships |
|-----------|-------|------------------|
| Roles | Vai trò người dùng | ❌ |
| Users | Người dùng | ✅ (Role, BloodType) |
| BloodTypes | Nhóm máu | ❌ |
| BloodCompatibility | Tương thích nhóm máu | ✅ (BloodTypes) |
| Locations | Địa điểm | ❌ |
| BloodDonationEvents | Sự kiện hiến máu | ✅ (Location, User) |
| DonationRegistrations | Đăng ký hiến máu | ✅ (User, Event) |
| HealthScreening | Sàng lọc sức khỏe | ✅ (Registration, User) |
| DonationHistory | Lịch sử hiến máu | ✅ (User, Event, BloodType) |
| NewsCategories | Danh mục tin tức | ❌ |
| News | Tin tức | ✅ (Category, Author) |
| Notifications | Thông báo | ✅ (User) |
| Settings | Cài đặt hệ thống | ❌ |
| ContactMessages | Tin nhắn liên hệ | ✅ (ResolvedBy) |

## Lưu ý

### Performance
- ⚠️ Export toàn bộ database có thể mất thời gian với data lớn
- ⚠️ Sử dụng `Include()` cho relationships có thể tăng memory usage
- ✅ Có thể export từng bảng riêng để giảm tải

### Security  
- 🔒 **Passwords**: PasswordHash trong Users table được export (cần cẩn thận)
- 🔒 **Admin Only**: Web interface chỉ cho phép Admin truy cập
- 🔒 **File Location**: Export files được tạo trong thư mục có thể truy cập được

### Backup & Storage
- 📁 Files được tạo với timestamp tự động
- 📁 Cần định kỳ xóa các export cũ để tiết kiệm dung lượng
- 📁 Nên backup export files ở nơi an toàn

## Troubleshooting

### Lỗi thường gặp:

**1. "Connection string not found"**
```
Solution: Kiểm tra appsettings.json có DefaultConnection string
```

**2. "Export folder not found"**  
```
Solution: Folder sẽ được tự động tạo, kiểm tra quyền write
```

**3. "Table 'X' is not supported"**
```
Solution: Xem danh sách bảng được hỗ trợ ở trên
```

**4. "Memory issues with large data"**
```
Solution: Export từng bảng riêng thay vì export all
```

## Development

### Thêm bảng mới:
1. Thêm method `ExportNewTableAsync()` trong `DataExporter.cs`
2. Thêm case trong `ExportTableAsync()` 
3. Cập nhật `CreateExportSummaryAsync()`
4. Cập nhật documentation

### Thêm format mới:
1. Thêm method export cho format mới
2. Cập nhật `DataExportController`
3. Cập nhật command line arguments
4. Test thoroughly

## Version History

- **v1.0** - Initial release với JSON/CSV export
- **v1.1** - Thêm Web Interface
- **v1.2** - Thêm Command Line tools
- **v1.3** - Thêm Extension Methods
