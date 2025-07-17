# Tính Năng Theo Role - Blood Donation Website

Tài liệu này mô tả các tính năng và chức năng được thêm vào cho từng role trong hệ thống hiến máu.

## 1. Role Admin

### Quản lý tin nhắn liên hệ
- **Controller**: `ContactMessageController`
- **Route**: `/admin/contact-messages`
- **Chức năng**:
  - Xem danh sách tất cả tin nhắn liên hệ
  - Xem tin nhắn chưa đọc
  - Xem chi tiết tin nhắn
  - Trả lời tin nhắn
  - Đánh dấu đã đọc/chưa đọc
  - Cập nhật trạng thái tin nhắn
  - Xóa tin nhắn
  - Xem thống kê tin nhắn

### Quản trị hệ thống
- **Controller**: `DataExportController`
- **Route**: `/admin/data-export`
- **Chức năng**:
  - Xuất dữ liệu hệ thống
  - Backup database
  - Quản lý cài đặt hệ thống

## 2. Role Hospital

### Quản lý sự kiện hiến máu
- **Controller**: `EventManagementController`
- **Route**: `/admin/events`
- **Chức năng**:
  - Xem danh sách sự kiện
  - Tạo sự kiện mới
  - Chỉnh sửa sự kiện
  - Xóa sự kiện
  - Cập nhật trạng thái sự kiện
  - Quản lý sức chứa và đăng ký

### Quản lý địa điểm hiến máu
- **Controller**: `LocationManagementController`
- **Route**: `/admin/locations`
- **Chức năng**:
  - Xem danh sách địa điểm
  - Thêm địa điểm mới
  - Chỉnh sửa địa điểm
  - Xóa địa điểm
  - Kích hoạt/vô hiệu hóa địa điểm
  - Quản lý sức chứa địa điểm

## 3. Role Doctor

### Sàng lọc sức khỏe
- **Controller**: `HealthScreeningController`
- **Route**: `/doctor/screening`
- **Chức năng**:
  - Xem danh sách sàng lọc sức khỏe
  - Xem sàng lọc chờ xử lý
  - Tạo sàng lọc mới cho đăng ký
  - Chỉnh sửa thông tin sàng lọc
  - Xem chi tiết sàng lọc
  - Check-in cho người hiến máu
  - Cập nhật trạng thái sàng lọc
  - Đánh giá điều kiện hiến máu

## 4. Authorization Filters

### HospitalAdminOnlyAttribute
- **File**: `Utilities/Filters/HospitalAdminOnlyAttribute.cs`
- **Chức năng**: Kiểm tra quyền truy cập cho role Hospital và Admin
- **Sử dụng**: Áp dụng cho các controller quản lý sự kiện và địa điểm

### DoctorOnlyAttribute
- **File**: `Utilities/Filters/DoctorOnlyAttribute.cs`
- **Chức năng**: Kiểm tra quyền truy cập cho role Doctor
- **Sử dụng**: Áp dụng cho controller sàng lọc sức khỏe

### AdminOnlyAttribute
- **File**: `Utilities/Filters/AdminOnlyAttribute.cs`
- **Chức năng**: Kiểm tra quyền truy cập cho role Admin
- **Sử dụng**: Áp dụng cho controller quản lý tin nhắn liên hệ

## 5. Services Implementation

### HealthScreeningService
- **File**: `Services/Implementations/HealthScreeningService.cs`
- **Interface**: `IHealthScreeningService`
- **Chức năng**:
  - CRUD operations cho sàng lọc sức khỏe
  - Quản lý trạng thái sàng lọc
  - Check-in functionality
  - Truy vấn theo trạng thái, người dùng, sự kiện
  - Thống kê sàng lọc
  - Kiểm tra điều kiện hiến máu

### ContactMessageService
- **File**: `Services/Implementations/ContactMessageService.cs`
- **Interface**: `IContactMessageService`
- **Chức năng**:
  - CRUD operations cho tin nhắn liên hệ
  - Quản lý trạng thái tin nhắn
  - Đánh dấu đã đọc/chưa đọc
  - Trả lời tin nhắn
  - Truy vấn theo trạng thái, danh mục, độ ưu tiên
  - Thống kê tin nhắn
  - Tìm kiếm tin nhắn

## 6. Views

### EventManagement Views
- `Views/EventManagement/Index.cshtml` - Danh sách sự kiện
- `Views/EventManagement/Create.cshtml` - Tạo sự kiện mới

### LocationManagement Views
- `Views/LocationManagement/Index.cshtml` - Danh sách địa điểm
- `Views/LocationManagement/Create.cshtml` - Thêm địa điểm mới

### HealthScreening Views
- `Views/HealthScreening/Index.cshtml` - Danh sách sàng lọc sức khỏe

### ContactMessage Views
- `Views/ContactMessage/Index.cshtml` - Danh sách tin nhắn liên hệ

## 7. Navigation Menu

### Cập nhật Layout
- **File**: `Views/Shared/_Layout.cshtml`
- **Thay đổi**:
  - Thêm menu "Quản lý sự kiện" cho Hospital và Admin
  - Thêm menu "Quản lý địa điểm" cho Hospital và Admin
  - Thêm menu "Tin nhắn liên hệ" cho Admin
  - Thêm menu "Sàng lọc sức khỏe" cho Doctor

## 8. Database Updates

### Role Seeder
- **File**: `Data/Seeders/RoleSeeder.cs`
- **Thêm roles**:
  - `Doctor` - Medical Doctor
  - `Hospital` - Hospital Representative

## 9. Program.cs Updates

### Service Registration
- Đăng ký `IHealthScreeningService`
- Đăng ký `IContactMessageService`

## 10. Tính Năng Theo Mô Tả SERVICES_INTERFACES.md

### Đã Triển Khai
1. ✅ **Tạo sự kiện** (Hospital, Admin)
2. ✅ **Thêm địa điểm** (Hospital, Admin)
3. ✅ **Sàng lọc sức khỏe** (Doctor)
4. ✅ **Trả lời tin nhắn** (Admin)

### Các Service Đã Triển Khai
1. ✅ `IHealthScreeningService` - Đầy đủ chức năng theo mô tả
2. ✅ `IContactMessageService` - Đầy đủ chức năng theo mô tả
3. ✅ `IBloodDonationEventService` - Đã có sẵn
4. ✅ `ILocationService` - Đã có sẵn

### Authorization
- ✅ Role-based access control
- ✅ Custom authorization filters
- ✅ Secure routing

### UI/UX
- ✅ Responsive design
- ✅ Modern Bootstrap interface
- ✅ SweetAlert notifications
- ✅ Confirmation modals
- ✅ Breadcrumb navigation

## 11. Hướng Dẫn Sử Dụng

### Cho Admin
1. Truy cập menu "Tin nhắn liên hệ" để quản lý tin nhắn
2. Sử dụng menu "Quản lý sự kiện" và "Quản lý địa điểm" để quản lý hệ thống

### Cho Hospital
1. Sử dụng menu "Quản lý sự kiện" để tạo và quản lý sự kiện hiến máu
2. Sử dụng menu "Quản lý địa điểm" để quản lý các địa điểm hiến máu

### Cho Doctor
1. Truy cập menu "Sàng lọc sức khỏe" để quản lý quá trình sàng lọc
2. Thực hiện check-in và đánh giá điều kiện hiến máu

## 12. Bảo Mật

- Tất cả controller đều có authorization attributes
- CSRF protection cho tất cả form
- Input validation
- Secure routing patterns
- Role-based menu visibility

## 13. Mở Rộng Tương Lai

- Thêm dashboard cho từng role
- Báo cáo và thống kê chi tiết
- Notification system
- Email integration
- Mobile responsive improvements
- Advanced search và filtering 