## Cấu hình DefaultConnection (SQL Server)

Để chạy dự án này, bạn cần cấu hình chuỗi kết nối cơ sở dữ liệu để ứng dụng có thể kết nối đến SQL Server cục bộ của bạn.

### 1. Xác định chuỗi kết nối

Mở tệp `appsettings.json` hoặc `appsettings.Development.json` trong thư mục gốc dự án. Tìm đoạn:

```
"ConnectionStrings": {
  "DefaultConnection": ""
}
```

### 2. Cập nhật DefaultConnection

Thay giá trị của `DefaultConnection` bằng chuỗi kết nối đến SQL Server của bạn. Ví dụ:

```
"DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=Blood_Donation;Integrated Security=True;Trust Server Certificate=True"
```

- `YOUR_SERVER_NAME` thường là `localhost`, `.` (dấu chấm), hoặc `MACHINE_NAME\SQLEXPRESS` nếu dùng SQL Server Express.
- `Initial Catalog` phải trùng với tên cơ sở dữ liệu của bạn (ví dụ: `Blood_Donation`).
- `Integrated Security=True` dùng xác thực Windows. Nếu bạn dùng xác thực SQL, thay bằng `User ID=your_user;Password=your_password`.

**Ví dụ với SQL Server Express:**
```
"DefaultConnection": "Data Source=localhost\SQLEXPRESS;Initial Catalog=Blood_Donation;Integrated Security=True;Trust Server Certificate=True"
```

### 3. Lưu và chạy

Sau khi cập nhật, lưu tệp và chạy dự án. Ứng dụng sẽ dùng chuỗi kết nối để kết nối đến cơ sở dữ liệu cục bộ của bạn.

Nếu gặp lỗi kết nối, hãy kiểm tra:
- SQL Server đang chạy.
- Cơ sở dữ liệu `Blood_Donation` đã tồn tại.
- Người dùng của bạn có quyền truy cập.

## Chạy ở chế độ Phát triển (Development Mode)

Để phát triển cục bộ, nên chạy dự án ở chế độ **Development**. Khi chạy ở chế độ này, ứng dụng sẽ tự động dùng cấu hình từ `appsettings.Development.json` (đè lên giá trị trong `appsettings.json`).

### Cách chạy ở chế độ Development

- **Visual Studio:** Mặc định khi chạy hoặc debug dự án sẽ dùng môi trường `Development`.
- **Command Line:** Bạn có thể đặt biến môi trường trước khi chạy dự án:
  - Trên Windows (PowerShell):
    ```powershell
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    dotnet run
    ```
  - Trên Windows (CMD):
    ```cmd
    set ASPNETCORE_ENVIRONMENT=Development
    dotnet run
    ```
  - Trên Linux/macOS:
    ```bash
    export ASPNETCORE_ENVIRONMENT=Development
    dotnet run
    ```

Khi chạy ở chế độ `Development`, ứng dụng sẽ dùng chuỗi kết nối và các cấu hình khác từ `appsettings.Development.json`. Đây là cách khuyến nghị để phát triển và kiểm thử cục bộ.
