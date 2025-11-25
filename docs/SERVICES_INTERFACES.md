# Tài liệu Giao Diện Dịch Vụ (Service Interfaces)

Thư mục này chứa tất cả các giao diện dịch vụ cho ứng dụng Website Hiến Máu. Các giao diện này định nghĩa hợp đồng cho các thao tác nghiệp vụ và cung cấp sự tách biệt rõ ràng giữa tầng trình bày và tầng nghiệp vụ.

## Giao Diện Dịch Vụ Cốt Lõi

### 1. **IUserService.cs**
Quản lý người dùng toàn diện bao gồm:
- **CRUD**: Tạo, đọc, cập nhật, xoá người dùng
- **Quản lý trạng thái**: Kích hoạt, vô hiệu hoá, khoá, mở khoá
- **Quản lý vai trò**: Gán / huỷ vai trò, kiểm tra vai trò
- **Nhóm máu**: Cập nhật và lấy thông tin nhóm máu
- **Thống kê**: Lịch sử hiến máu, theo dõi điều kiện đủ
- **Tìm kiếm / Lọc nâng cao**
- **Xác thực**: Kiểm tra email / username, điều kiện hiến máu

### 2. **IBloodTypeService.cs**
Quản lý nhóm máu:
- **Thống kê**: Số lượt hiến, thể tích, số người
- **Tìm kiếm**

### 3. **IRoleService.cs**
Quản lý truy cập theo vai trò:
- **CRUD**: Quản lý vai trò
- **Gán vai trò**: Gán / huỷ vai trò cho người dùng
- **Lọc theo vai trò**
- **Xác thực**: Kiểm tra tên và tồn tại vai trò
- **Thống kê**: Số người dùng theo vai trò

### 4. **ILocationService.cs**
Quản lý địa điểm hiến máu:
- **CRUD**
- **Trạng thái**: Kích hoạt / vô hiệu hoá
- **Sức chứa**
- **Sự kiện**: Lấy sự kiện theo địa điểm
- **Thống kê**: Số sự kiện và số lượt hiến máu

## Giao Diện Quản Lý Sự Kiện

### 5. **IBloodDonationEventService.cs**
Quản lý sự kiện hiến máu:
- **CRUD**
- **Trạng thái**: Kích hoạt, huỷ, hoàn tất
- **Sức chứa**
- **Lịch**: Tương lai, quá khứ, theo khoảng ngày
- **Thống kê**
- **Xác thực**
- **Thông báo**: Thông báo cho người dùng gần địa điểm

### 6. **IDonationRegistrationService.cs**
Quản lý đăng ký hiến máu:
- **CRUD**
- **Trạng thái**: (Các trạng thái xem trong TABLE_ENUM)
- **Truy vấn**: Theo người, theo sự kiện
- **Xác thực**: Kiểm tra điều kiện và số lượng
- **Thống kê**
- **Thông báo**

### 7. **IDonationHistoryService.cs**
Theo dõi lịch sử hiến máu:
- **Trạng thái**: (Các trạng thái xem trong TABLE_ENUM)
- **Truy vấn**
- **Thống kê**
- **Điều kiện đủ**
- **Báo cáo**
- **Giấy chứng nhận**

### 8. **IHealthScreeningService.cs**
Quản lý sàng lọc sức khoẻ:
- **CRUD**
- **Truy vấn**
- **Trạng thái**: (Các trạng thái xem trong TABLE_ENUM)
- **Check-in**
- **Thống kê**
- **Báo cáo**

## Giao Diện Quản Lý Nội Dung

### 9. **INewsService.cs**
Quản lý tin tức:
- **CRUD**
- **Trạng thái**: (Các trạng thái xem trong TABLE_ENUM)
- **Truy vấn**
- **Thống kê**
- **Theo dõi lượt xem**
- **Danh mục**, **Tác giả**
- **Quy trình phát hành**
- **SEO**

### 10. **INewsCategoryService.cs**
Quản lý danh mục tin tức:
- **CRUD**
- **Trạng thái**
- **Thống kê**
- **Quản lý bài viết**

## Giao Diện Giao Tiếp

### 11. **INotificationService.cs**
Quản lý thông báo:
- **CRUD**
- **Truy vấn**
- **Trạng thái đọc / chưa đọc**
- **Loại**
- **Thống kê**
- **Gửi**
- **Mẫu**
- **Xoá cũ**
- **Tuỳ chọn người dùng**

### 12. **IContactMessageService.cs**
Quản lý liên hệ:
- **CRUD**
- **Trạng thái**
- **Truy vấn**
- **Thống kê**
- **Xử lý**
- **Phản hồi**
- **Danh mục**, **Độ ưu tiên**
- **Báo cáo**
- **Mẫu phản hồi**

## Giao Diện Hệ Thống

### 13. **IBloodCompatibilityService.cs**
Quản lý tương thích nhóm máu:
- **Truy vấn**
- **Ma trận tương thích**
- **Thống kê**
- **Gợi ý**

### 14. **ISettingService.cs**
Quản lý thiết lập hệ thống:
- **CRUD**
- **Lấy / cập nhật giá trị**
- **Phân loại**
- **Thống kê**
- **Khởi tạo**, **Sao lưu**
- **Xác thực**
- **Cache**

## Giao Diện Chuyên Biệt

### 15. **IStatisticsService.cs**
Thống kê toàn hệ thống:
- **Dashboard**
- **Theo nhóm máu**, **Sự kiện**, **Người dùng**
- **Hiến máu**, **Đăng ký**, **Sàng lọc**
- **Theo địa điểm**, **Theo thời gian**
- **So sánh**, **Tăng trưởng**, **Hiệu quả**
- **Dự đoán**
- **Xuất dữ liệu**
- **Thống kê theo thời gian thực**
- **Thống kê tuỳ chọn**

## Giao Diện Sẵn Có

### 16. **IAccountService.cs**
Quản lý tài khoản:
- **Xác thực**
- **Quản lý mật khẩu**
- **Quản lý người dùng**
- **Xác thực email**
- **Vai trò**
- **Trạng thái**

### 17. **IProfileService.cs**
Quản lý hồ sơ người dùng:
- **Xem / cập nhật**
- **Cập nhật nhóm máu**: (Quyền của Bệnh viện, Bác sĩ và Admin)

### 18. **IEmailService.cs**
Gửi email:
- **Chung**
- **Mẫu có sẵn**
- **Sự kiện**, **Hiến máu**

## Hướng Dẫn Sử Dụng

### Dependency Injection
```csharp
services.AddScoped<IUserService, UserService>();
// ... đăng ký các service khác
```

### Sử dụng trong Controller
```csharp
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }
}
```

### Cài đặt Service
```csharp
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UserService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Triển khai các phương thức
}
```

## Thực Hành Tốt

1. **Async**
2. **DTO**
3. **Xử lý lỗi**
4. **Xác thực**
5. **Ghi log**
6. **Cache**
7. **Bảo mật**
8. **Test**

## Nguyên Tắc Thiết Kế

1. **Single Responsibility**
2. **Đầy đủ chức năng**
3. **Đặt tên nhất quán**
4. **Async đầu tiên**
5. **DTO tách biệt**
6. **Hỗ trợ xác thực**
7. **Mở rộng dễ dàng**

Thiết kế này cung cấp nền tảng vững chắc cho tầng nghiệp vụ của hệ thống hiến máu.
