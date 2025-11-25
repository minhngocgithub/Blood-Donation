# Các bảng trong database

Table Roles {
  RoleId int [pk, increment]
  RoleName nvarchar(50) [not null, unique] // Enum: Admin, User, Hospital, Doctor, Staff
  Description nvarchar(200)
  CreatedDate datetime [default: `getdate()`]
}

Table Users {
  UserId int [pk, increment]
  Username nvarchar(50) [not null, unique]
  Email nvarchar(100) [not null, unique]
  PasswordHash nvarchar(255) [not null]
  FullName nvarchar(100) [not null]
  Phone nvarchar(15)
  Address nvarchar(255)
  DateOfBirth date
  Gender nvarchar(10) // Enum: "Male", "Female", "Other"
  BloodTypeId int
  RoleId int [default: 2]
  IsActive bit [default: 1]
  EmailVerified bit [default: 0]
  LastDonationDate date
  CreatedDate datetime [default: `getdate()`]
  UpdatedDate datetime [default: `getdate()`]
}

Table BloodTypes {
  BloodTypeId int [pk, increment]
  BloodTypeName nvarchar(5) [not null, unique]
  Description nvarchar(100)
}

Table BloodCompatibility {
  Id int [pk, increment]
  FromBloodTypeId int [not null]
  ToBloodTypeId int [not null]
}

Table Locations {
  LocationId int [pk, increment]
  LocationName nvarchar(200) [not null]
  Address nvarchar(500) [not null]
  ContactPhone nvarchar(15)
  Capacity int [default: 50]
  IsActive bit [default: 1]
  CreatedDate datetime [default: `getdate()`]
}

Table BloodDonationEvents {
  EventId int [pk, increment]
  EventName nvarchar(200) [not null]
  EventDescription nvarchar(max)
  EventDate date [not null]
  StartTime time [not null]
  EndTime time [not null]
  LocationId int
  MaxDonors int [default: 100]
  CurrentDonors int [default: 0]
  Status nvarchar(20) [default: 'Active'] // Enum: "Draft", "Published", "Active", "Completed", "Cancelled", "Postponed", "Full", "Closed"
  ImageUrl nvarchar(255)
  RequiredBloodTypes nvarchar(100)
  CreatedBy int
  CreatedDate datetime [default: `getdate()`]
  UpdatedDate datetime [default: `getdate()`]
}

Table DonationRegistrations {
  RegistrationId int [pk, increment]
  UserId int [not null]
  EventId int [not null]
  RegistrationDate datetime [default: `getdate()`]
  Status nvarchar(20) [default: 'Registered'] // Enum: "Registered", "Confirmed", "CheckedIn", "Screening", "Eligible", "Ineligible", "Donating", "Completed", "Cancelled", "NoShow", "Failed"
  Notes nvarchar(500)
  IsEligible bit [default: 0]
  CheckInTime datetime
  CompletionTime datetime
  CancellationReason nvarchar(200)
}

Table HealthScreening {
  ScreeningId int [pk, increment]
  RegistrationId int [not null]
  Weight decimal(5,2)
  Height decimal(5,2)
  BloodPressure nvarchar(20)
  HeartRate int
  Temperature decimal(4,2)
  Hemoglobin decimal(4,2)
  IsEligible bit [default: 1]
  DisqualifyReason nvarchar(500) // Enum: "LowHemoglobin", "HighBloodPressure", "LowBloodPressure", "Fever", "LowWeight", "RecentDonation", "MedicalHistory", "CurrentMedication", "RecentVaccination", "Pregnancy", "Breastfeeding", "RecentSurgery", "InfectionRisk", "Other"
  ScreenedBy int
  ScreeningDate datetime [default: `getdate()`]
}

Table DonationHistory {
  DonationId int [pk, increment]
  UserId int [not null]
  EventId int [not null]
  RegistrationId int
  DonationDate datetime [not null]
  BloodTypeId int [not null]
  Volume int [default: 350]
  Status nvarchar(20) [default: 'Completed'] // Enum: "Started", "InProgress", "Completed", "Stopped", "Failed"
  Notes nvarchar(500)
  NextEligibleDate date
  CertificateIssued bit [default: 0]
}

Table NewsCategories {
  CategoryId int [pk, increment]
  CategoryName nvarchar(100) [not null]
  Description nvarchar(200)
  IsActive bit [default: 1]
}

Table News {
  NewsId int [pk, increment]
  Title nvarchar(200) [not null]
  Content nvarchar(max) [not null]
  Summary nvarchar(500)
  ImageUrl nvarchar(255)
  CategoryId int
  AuthorId int
  ViewCount int [default: 0]
  IsPublished bit [default: 0]
  PublishedDate datetime
  CreatedDate datetime [default: `getdate()`]
  UpdatedDate datetime [default: `getdate()`]
}

Table Notifications {
  NotificationId int [pk, increment]
  UserId int
  Title nvarchar(200) [not null]
  Message nvarchar(500) [not null]
  Type nvarchar(50) // Enum: "Registration", "Confirmation", "Reminder", "Cancellation", "Completion", "Result", "Event", "System", "Medical", "Warning", "Info"
  IsRead bit [default: 0]
  CreatedDate datetime [default: `getdate()`]
}

Table Settings {
  SettingId int [pk, increment]
  SettingKey nvarchar(50) [not null, unique]
  SettingValue nvarchar(255) [not null]
  Description nvarchar(200)
  UpdatedDate datetime [default: `getdate()`]
}

Table ContactMessages {
  MessageId int [pk, increment]
  FullName nvarchar(100) [not null]
  Email nvarchar(100) [not null]
  Phone nvarchar(15)
  Subject nvarchar(200) [not null]
  Message nvarchar(1000) [not null]
  Status nvarchar(20) [default: 'New'] // Enum: "New", "Read", "InProgress", "Resolved", "Closed"
  CreatedDate datetime [default: `getdate()`]
  ResolvedDate datetime
  ResolvedBy int
}

// Relationships
Ref: Users.RoleId > Roles.RoleId
Ref: Users.BloodTypeId > BloodTypes.BloodTypeId
Ref: BloodDonationEvents.LocationId > Locations.LocationId
Ref: BloodDonationEvents.CreatedBy > Users.UserId
Ref: DonationRegistrations.UserId > Users.UserId
Ref: DonationRegistrations.EventId > BloodDonationEvents.EventId
Ref: DonationHistory.BloodTypeId > BloodTypes.BloodTypeId
Ref: HealthScreening.RegistrationId > DonationRegistrations.RegistrationId
Ref: HealthScreening.ScreenedBy > Users.UserId
Ref: DonationHistory.UserId > Users.UserId
Ref: DonationHistory.EventId > BloodDonationEvents.EventId
Ref: DonationHistory.RegistrationId > DonationRegistrations.RegistrationId
Ref: News.CategoryId > NewsCategories.CategoryId
Ref: News.AuthorId > Users.UserId
Ref: Notifications.UserId > Users.UserId
Ref: ContactMessages.ResolvedBy > Users.UserId
Ref: BloodCompatibility.FromBloodTypeId > BloodTypes.BloodTypeId
Ref: BloodCompatibility.ToBloodTypeId > BloodTypes.BloodTypeId


# Giải thích các thuộc tính trong database

## Bảng Roles
| Tên thuộc tính | Kiểu dữ liệu      | Vai trò/Ý nghĩa                                                                 |
|----------------|-------------------|-------------------------------------------------------------------------------|
| RoleId         | int (PK, tăng tự động) | Khóa chính, định danh duy nhất cho vai trò                                    |
| RoleName       | nvarchar(50)      | Tên vai trò (Admin, User, Hospital, Doctor, Staff), dùng cho phân quyền       |
| Description    | nvarchar(200)     | Mô tả chi tiết về vai trò                                                     |
| CreatedDate    | datetime          | Ngày tạo vai trò                                                              |

## Bảng Users
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                                                 |
|------------------|----------------------|-----------------------------------------------------------------------------------------------|
| UserId           | int (PK, tăng tự động)| Khóa chính, định danh duy nhất cho người dùng                                                  |
| Username         | nvarchar(50)         | Tên đăng nhập, duy nhất                                                                      |
| Email            | nvarchar(100)        | Email người dùng, duy nhất                                                                    |
| PasswordHash     | nvarchar(255)        | Mã hóa mật khẩu người dùng                                                                    |
| FullName         | nvarchar(100)        | Họ tên đầy đủ của người dùng                                                                  |
| Phone            | nvarchar(15)         | Số điện thoại người dùng                                                                      |
| Address          | nvarchar(255)        | Địa chỉ người dùng                                                                            |
| DateOfBirth      | date                 | Ngày sinh                                                                                    |
| Gender           | nvarchar(10)         | Giới tính (Nam, Nữ, Khác)                                                                    |
| BloodTypeId      | int                  | Nhóm máu, liên kết đến bảng BloodTypes                                                        |
| RoleId           | int                  | Vai trò, liên kết đến bảng Roles                                                              |
| IsActive         | bit                  | Trạng thái hoạt động của tài khoản (1: hoạt động, 0: khóa)                                    |
| EmailVerified    | bit                  | Đã xác thực email hay chưa                                                                    |
| LastDonationDate | date                 | Ngày hiến máu gần nhất                                                                       |
| CreatedDate      | datetime             | Ngày tạo tài khoản                                                                           |
| UpdatedDate      | datetime             | Ngày cập nhật tài khoản gần nhất                                                              |

## Bảng BloodTypes
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                                 |
|------------------|----------------------|-------------------------------------------------------------------------------|
| BloodTypeId      | int (PK, tăng tự động)| Khóa chính, định danh nhóm máu                                                |
| BloodTypeName    | nvarchar(5)          | Tên nhóm máu (A, B, AB, O, v.v.)                                              |
| Description      | nvarchar(100)        | Mô tả về nhóm máu                                                             |

## Bảng BloodCompatibility
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| Id               | int (PK, tăng tự động)| Khóa chính, định danh duy nhất cho bản ghi tương thích nhóm máu      |
| FromBloodTypeId  | int                  | Nhóm máu cho (từ), liên kết đến BloodTypes                          |
| ToBloodTypeId    | int                  | Nhóm máu nhận (đến), liên kết đến BloodTypes                        |

## Bảng Locations
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| LocationId       | int (PK, tăng tự động)| Khóa chính, định danh địa điểm                                       |
| LocationName     | nvarchar(200)        | Tên địa điểm tổ chức hiến máu                                        |
| Address          | nvarchar(500)        | Địa chỉ chi tiết địa điểm                                            |
| ContactPhone     | nvarchar(15)         | Số điện thoại liên hệ                                                |
| Capacity         | int                  | Sức chứa tối đa của địa điểm                                         |
| IsActive         | bit                  | Địa điểm còn hoạt động hay không                                     |
| CreatedDate      | datetime             | Ngày tạo địa điểm                                                    |

## Bảng BloodDonationEvents
| Tên thuộc tính      | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|---------------------|----------------------|---------------------------------------------------------------------|
| EventId             | int (PK, tăng tự động)| Khóa chính, định danh sự kiện hiến máu                              |
| EventName           | nvarchar(200)        | Tên sự kiện hiến máu                                                |
| EventDescription    | nvarchar(max)        | Mô tả chi tiết về sự kiện                                           |
| EventDate           | date                 | Ngày diễn ra sự kiện                                                |
| StartTime           | time                 | Thời gian bắt đầu                                                   |
| EndTime             | time                 | Thời gian kết thúc                                                  |
| LocationId          | int                  | Địa điểm tổ chức, liên kết đến Locations                            |
| MaxDonors           | int                  | Số lượng người hiến tối đa                                          |
| CurrentDonors       | int                  | Số lượng người đã đăng ký/đã hiến                                   |
| Status              | nvarchar(20)         | Trạng thái sự kiện (Draft, Active, Completed, ...)                  |
| ImageUrl            | nvarchar(255)        | Đường dẫn ảnh minh họa sự kiện                                      |
| RequiredBloodTypes  | nvarchar(100)        | Nhóm máu cần thiết cho sự kiện                                      |
| CreatedBy           | int                  | Người tạo sự kiện, liên kết đến Users                               |
| CreatedDate         | datetime             | Ngày tạo sự kiện                                                    |
| UpdatedDate         | datetime             | Ngày cập nhật sự kiện                                               |

## Bảng DonationRegistrations
| Tên thuộc tính      | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|---------------------|----------------------|---------------------------------------------------------------------|
| RegistrationId      | int (PK, tăng tự động)| Khóa chính, định danh lượt đăng ký                                  |
| UserId              | int                  | Người đăng ký, liên kết đến Users                                    |
| EventId             | int                  | Sự kiện đăng ký, liên kết đến BloodDonationEvents                    |
| RegistrationDate    | datetime             | Ngày giờ đăng ký                                                    |
| Status              | nvarchar(20)         | Trạng thái đăng ký (Registered, Confirmed, CheckedIn, ...)           |
| Notes               | nvarchar(500)        | Ghi chú thêm                                                        |
| IsEligible          | bit                  | Đủ điều kiện hiến máu hay không                                      |
| CheckInTime         | datetime             | Thời gian check-in tại sự kiện                                      |
| CompletionTime      | datetime             | Thời gian hoàn thành hiến máu                                       |
| CancellationReason  | nvarchar(200)        | Lý do hủy đăng ký (nếu có)                                          |

## Bảng HealthScreening
| Tên thuộc tính      | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|---------------------|----------------------|---------------------------------------------------------------------|
| ScreeningId         | int (PK, tăng tự động)| Khóa chính, định danh lượt sàng lọc                                 |
| RegistrationId      | int                  | Liên kết đến lượt đăng ký (DonationRegistrations)                   |
| Weight              | decimal(5,2)         | Cân nặng người hiến                                                 |
| Height              | decimal(5,2)         | Chiều cao người hiến                                                |
| BloodPressure       | nvarchar(20)         | Huyết áp                                                            |
| HeartRate           | int                  | Nhịp tim                                                            |
| Temperature         | decimal(4,2)         | Nhiệt độ cơ thể                                                     |
| Hemoglobin          | decimal(4,2)         | Nồng độ Hemoglobin                                                  |
| IsEligible          | bit                  | Đủ điều kiện hiến máu hay không                                      |
| DisqualifyReason    | nvarchar(500)        | Lý do không đủ điều kiện (nếu có)                                   |
| ScreenedBy          | int                  | Người thực hiện sàng lọc, liên kết đến Users                        |
| ScreeningDate       | datetime             | Ngày thực hiện sàng lọc                                             |

## Bảng DonationHistory
| Tên thuộc tính      | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|---------------------|----------------------|---------------------------------------------------------------------|
| DonationId          | int (PK, tăng tự động)| Khóa chính, định danh lịch sử hiến máu                              |
| UserId              | int                  | Người hiến máu, liên kết đến Users                                   |
| EventId             | int                  | Sự kiện hiến máu, liên kết đến BloodDonationEvents                   |
| RegistrationId      | int                  | Liên kết đến lượt đăng ký (có thể null nếu không đăng ký trước)      |
| DonationDate        | datetime             | Ngày giờ hiến máu                                                   |
| BloodTypeId         | int                  | Nhóm máu của người hiến, liên kết đến BloodTypes                     |
| Volume              | int                  | Lượng máu hiến (ml)                                                 |
| Status              | nvarchar(20)         | Trạng thái hiến máu (Started, Completed, Failed, ...)                |
| Notes               | nvarchar(500)        | Ghi chú thêm                                                        |
| NextEligibleDate    | date                 | Ngày đủ điều kiện hiến máu tiếp theo                                 |
| CertificateIssued   | bit                  | Đã cấp giấy chứng nhận hiến máu hay chưa                             | 

## Bảng NewsCategories
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| CategoryId       | int (PK, tăng tự động)| Khóa chính, định danh danh mục tin tức                              |
| CategoryName     | nvarchar(100)        | Tên danh mục                                                        |
| Description      | nvarchar(200)        | Mô tả về danh mục                                                   |
| IsActive         | bit                  | Danh mục còn hoạt động hay không                                     |

## Bảng News
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| NewsId           | int (PK, tăng tự động)| Khóa chính, định danh tin tức                                       |
| Title            | nvarchar(200)        | Tiêu đề tin tức                                                     |
| Content          | nvarchar(max)        | Nội dung chi tiết tin tức                                            |
| Summary          | nvarchar(500)        | Tóm tắt nội dung                                                    |
| ImageUrl         | nvarchar(255)        | Đường dẫn ảnh minh họa                                              |
| CategoryId       | int                  | Danh mục tin tức, liên kết đến NewsCategories                        |
| AuthorId         | int                  | Người đăng tin, liên kết đến Users                                   |
| ViewCount        | int                  | Số lượt xem                                                         |
| IsPublished      | bit                  | Đã xuất bản hay chưa                                                |
| PublishedDate    | datetime             | Ngày xuất bản                                                       |
| CreatedDate      | datetime             | Ngày tạo tin                                                        |
| UpdatedDate      | datetime             | Ngày cập nhật tin                                                   |

## Bảng Notifications
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| NotificationId   | int (PK, tăng tự động)| Khóa chính, định danh thông báo                                     |
| UserId           | int                  | Người nhận thông báo, liên kết đến Users                             |
| Title            | nvarchar(200)        | Tiêu đề thông báo                                                   |
| Message          | nvarchar(500)        | Nội dung thông báo                                                  |
| Type             | nvarchar(50)         | Loại thông báo (Registration, Reminder, Info, ...)                  |
| IsRead           | bit                  | Đã đọc hay chưa                                                     |
| CreatedDate      | datetime             | Ngày tạo thông báo                                                  |

## Bảng Settings
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| SettingId        | int (PK, tăng tự động)| Khóa chính, định danh thiết lập                                     |
| SettingKey       | nvarchar(50)         | Khóa thiết lập, duy nhất                                            |
| SettingValue     | nvarchar(255)        | Giá trị thiết lập                                                   |
| Description      | nvarchar(200)        | Mô tả về thiết lập                                                  |
| UpdatedDate      | datetime             | Ngày cập nhật thiết lập                                             |

## Bảng ContactMessages
| Tên thuộc tính   | Kiểu dữ liệu         | Vai trò/Ý nghĩa                                                      |
|------------------|----------------------|---------------------------------------------------------------------|
| MessageId        | int (PK, tăng tự động)| Khóa chính, định danh tin nhắn liên hệ                              |
| FullName         | nvarchar(100)        | Họ tên người gửi                                                    |
| Email            | nvarchar(100)        | Email người gửi                                                     |
| Phone            | nvarchar(15)         | Số điện thoại người gửi                                             |
| Subject          | nvarchar(200)        | Chủ đề tin nhắn                                                     |
| Message          | nvarchar(1000)       | Nội dung tin nhắn                                                   |
| Status           | nvarchar(20)         | Trạng thái xử lý tin nhắn (New, Read, Resolved, ...)                |
| CreatedDate      | datetime             | Ngày gửi tin nhắn                                                   |
| ResolvedDate     | datetime             | Ngày xử lý xong tin nhắn                                            |
| ResolvedBy       | int                  | Người xử lý, liên kết đến Users                                     | 
