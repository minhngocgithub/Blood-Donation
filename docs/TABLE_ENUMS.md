# Enums cho các thuộc tính của bảng trong hệ thống hiến máu

## 1. User Table Enums

### 1.1. Gender (Giới tính)
```csharp
public enum Gender
{
    Male = "Nam",
    Female = "Nữ",
    Other = "Khác"
}
```

## 2. DonationRegistration Table Enums

### 2.1. RegistrationStatus (Trạng thái đăng ký)
```csharp
public enum RegistrationStatus
{
    Registered = "Đã đăng ký",           // Mới đăng ký
    Confirmed = "Đã xác nhận",           // Admin xác nhận
    CheckedIn = "Đã đến",                // Đã check-in tại sự kiện
    Screening = "Đang sàng lọc",         // Đang khám sàng lọc
    Eligible = "Đủ điều kiện",           // Qua sàng lọc, đủ điều kiện hiến
    Ineligible = "Không đủ điều kiện",   // Không qua sàng lọc
    Donating = "Đang hiến máu",          // Đang thực hiện hiến máu
    Completed = "Hoàn tất",              // Hoàn tất hiến máu
    Cancelled = "Đã hủy",                // Người dùng hủy đăng ký
    NoShow = "Không đến",                // Đăng ký nhưng không đến
    Failed = "Thất bại"                  // Có vấn đề trong quá trình hiến
}
```

## 3. BloodDonationEvent Table Enums

### 3.1. EventStatus (Trạng thái sự kiện)
```csharp
public enum EventStatus
{
    Draft = "Bản nháp",                  // Đang tạo sự kiện
    Published = "Đã công bố",            // Mở đăng ký
    Active = "Đang diễn ra",             // Sự kiện đang diễn ra
    Completed = "Hoàn thành",            // Sự kiện đã kết thúc
    Cancelled = "Đã hủy",                // Sự kiện bị hủy
    Postponed = "Hoãn lại",              // Sự kiện bị hoãn
    Full = "Đã đầy",                     // Đã đủ số lượng đăng ký
    Closed = "Đóng đăng ký"              // Đóng đăng ký nhưng chưa diễn ra
}
```

## 4. HealthScreening Table Enums

### 4.1. DisqualificationReason (Lý do loại)
```csharp
public enum DisqualificationReason
{
    LowHemoglobin = "Hb thấp",
    HighBloodPressure = "Huyết áp cao", 
    LowBloodPressure = "Huyết áp thấp",
    Fever = "Sốt",
    LowWeight = "Cân nặng không đủ",
    RecentDonation = "Mới hiến gần đây",
    MedicalHistory = "Tiền sử bệnh lý",
    CurrentMedication = "Đang dùng thuốc",
    RecentVaccination = "Mới tiêm vaccine",
    Pregnancy = "Đang mang thai",
    Breastfeeding = "Đang cho con bú",
    RecentSurgery = "Mới phẫu thuật",
    InfectionRisk = "Nguy cơ nhiễm trùng",
    Other = "Lý do khác"
}
```

## 5. DonationHistory Table Enums

### 5.1. DonationStatus (Trạng thái hiến máu)
```csharp
public enum DonationStatus
{
    Started = "Bắt đầu",                 // Bắt đầu lấy máu
    InProgress = "Đang thực hiện",       // Đang lấy máu
    Completed = "Hoàn thành",            // Hoàn thành lấy máu
    Stopped = "Dừng giữa chừng",         // Dừng do vấn đề
    Failed = "Thất bại"                  // Không thể hoàn thành
}
```

## 6. Notification Table Enums

### 6.1. NotificationType (Loại thông báo)
```csharp
public enum NotificationType
{
    Registration = "Đăng ký",            // Thông báo đăng ký sự kiện
    Confirmation = "Xác nhận",           // Xác nhận đăng ký
    Reminder = "Nhắc nhở",               // Nhắc nhở sự kiện sắp tới
    Cancellation = "Hủy bỏ",             // Thông báo hủy
    Completion = "Hoàn thành",           // Hoàn thành hiến máu
    Result = "Kết quả",                  // Kết quả sàng lọc
    Event = "Sự kiện",                   // Thông báo sự kiện mới
    System = "Hệ thống",                 // Thông báo hệ thống
    Medical = "Y tế",                    // Thông báo y tế
    Warning = "Cảnh báo",                // Cảnh báo
    Info = "Thông tin"                   // Thông tin chung
}
```

## 7. ContactMessage Table Enums

### 7.1. MessageStatus (Trạng thái tin nhắn)
```csharp
public enum MessageStatus
{
    New = "Mới",                         // Tin nhắn mới
    Read = "Đã đọc",                     // Đã đọc
    InProgress = "Đang xử lý",           // Đang xử lý
    Resolved = "Đã giải quyết",          // Đã giải quyết
    Closed = "Đã đóng"                   // Đã đóng
}
```

## 8. Role Table Enums

### 8.1. RoleType (Loại vai trò)
```csharp
public enum RoleType
{
    Admin = "Quản trị viên",             // Quản trị toàn hệ thống
    Doctor = "Bác sĩ",                   // Bác sĩ sàng lọc
    Staff = "Nhân viên",                 // Nhân viên tổ chức
    Hospital = "Bệnh viện",              // Đại diện bệnh viện
    User = "Người dùng",                 // Người hiến máu
}
```

## Ghi chú sử dụng

1. **Triển khai**: Các enum này nên được định nghĩa trong namespace `Blood_Donation_Website.Models.Enums`
2. **Validation**: Sử dụng Data Annotations để validate các giá trị enum
3. **Database**: Có thể lưu trữ dưới dạng string hoặc int trong database
4. **Localization**: Có thể mở rộng để hỗ trợ đa ngôn ngữ
5. **Mở rộng**: Dễ dàng thêm giá trị mới khi cần thiết

## Ví dụ sử dụng trong Entity

```csharp
[StringLength(20)]
public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;

[StringLength(50)]
public NotificationType Type { get; set; } = NotificationType.Info;
```