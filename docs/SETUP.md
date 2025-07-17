<<<<<<< HEAD
## Setting up the DefaultConnection (SQL Server)

To run this project, you need to configure the database connection string so the application can connect to your local SQL Server instance.

### 1. Locate the Connection String

Open either `appsettings.json` or `appsettings.Development.json` in the project root. Find the section:
=======
## Cấu hình DefaultConnection (SQL Server)

Để chạy dự án này, bạn cần cấu hình chuỗi kết nối cơ sở dữ liệu để ứng dụng có thể kết nối đến SQL Server cục bộ của bạn.

### 1. Xác định chuỗi kết nối

Mở tệp `appsettings.json` hoặc `appsettings.Development.json` trong thư mục gốc dự án. Tìm đoạn:
>>>>>>> develop

```
"ConnectionStrings": {
  "DefaultConnection": ""
}
```

<<<<<<< HEAD
### 2. Update the DefaultConnection

Replace the value of `DefaultConnection` with your local SQL Server connection string. For example:
=======
### 2. Cập nhật DefaultConnection

Thay giá trị của `DefaultConnection` bằng chuỗi kết nối đến SQL Server của bạn. Ví dụ:
>>>>>>> develop

```
"DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=Blood_Donation;Integrated Security=True;Trust Server Certificate=True"
```

<<<<<<< HEAD
- `YOUR_SERVER_NAME` is usually `localhost`, `.` (dot), or `MACHINE_NAME\\SQLEXPRESS` for SQL Server Express.
- `Initial Catalog` should match your database name (e.g., `Blood_Donation`).
- `Integrated Security=True` uses Windows Authentication. If you use SQL Authentication, replace with `User ID=your_user;Password=your_password`.

**Example for SQL Server Express:**
```
"DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Blood_Donation;Integrated Security=True;Trust Server Certificate=True"
```

### 3. Save and Run

After updating, save the file and run the project. The application will use the specified connection string to connect to your local database.

If you have any issues connecting, ensure:
- SQL Server is running.
- The database `Blood_Donation` exists.
- Your user has access rights.

## Running in Development Mode

For local development, it is recommended to run the project in **Development** mode. When running in this mode, the application will automatically use the settings from `appsettings.Development.json` (overriding values in `appsettings.json`).

### How to Run in Development Mode

- **Visual Studio:** By default, running or debugging the project will use the `Development` environment.
- **Command Line:** You can set the environment variable before running the project:
  - On Windows (PowerShell):
=======
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
>>>>>>> develop
    ```powershell
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    dotnet run
    ```
<<<<<<< HEAD
  - On Windows (CMD):
=======
  - Trên Windows (CMD):
>>>>>>> develop
    ```cmd
    set ASPNETCORE_ENVIRONMENT=Development
    dotnet run
    ```
<<<<<<< HEAD
  - On Linux/macOS:
=======
  - Trên Linux/macOS:
>>>>>>> develop
    ```bash
    export ASPNETCORE_ENVIRONMENT=Development
    dotnet run
    ```

<<<<<<< HEAD
When running in `Development` mode, the application will use the connection string and other settings from `appsettings.Development.json`. This is the recommended approach for local development and testing.
=======
Khi chạy ở chế độ `Development`, ứng dụng sẽ dùng chuỗi kết nối và các cấu hình khác từ `appsettings.Development.json`. Đây là cách khuyến nghị để phát triển và kiểm thử cục bộ.
>>>>>>> develop
