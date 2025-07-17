Table Roles {
  RoleId int [pk, increment]
  RoleName nvarchar(50) [not null, unique]
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
<<<<<<< HEAD
  Gender nvarchar(10)
=======
  Gender nvarchar(10) // Enum: "Male", "Female", "Other"
>>>>>>> develop
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
<<<<<<< HEAD
  Status nvarchar(20) [default: 'Active']
=======
  Status nvarchar(20) [default: 'Active'] // Enum: "Draft", "Published", "Active", "Completed", "Cancelled", "Postponed", "Full", "Closed"
>>>>>>> develop
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
<<<<<<< HEAD
  Status nvarchar(20) [default: 'Registered']
=======
  Status nvarchar(20) [default: 'Registered'] // Enum: "Registered", "Confirmed", "CheckedIn", "Screening", "Eligible", "Ineligible", "Donating", "Completed", "Cancelled", "NoShow", "Failed"
>>>>>>> develop
  Notes nvarchar(500)
  IsEligible bit [default: 1]
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
<<<<<<< HEAD
  DisqualifyReason nvarchar(500)
=======
  DisqualifyReason nvarchar(500) // Enum: "LowHemoglobin", "HighBloodPressure", "LowBloodPressure", "Fever", "LowWeight", "RecentDonation", "MedicalHistory", "CurrentMedication", "RecentVaccination", "Pregnancy", "Breastfeeding", "RecentSurgery", "InfectionRisk", "Other"
>>>>>>> develop
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
<<<<<<< HEAD
  Status nvarchar(20) [default: 'Completed']
=======
  Status nvarchar(20) [default: 'Completed'] // Enum: "Started", "InProgress", "Completed", "Stopped", "Failed"
>>>>>>> develop
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
<<<<<<< HEAD
  IsPublished bit [default: 0]
=======
  IsPublished bit [default: 0] // Alternative: Status nvarchar(20) - Enum: "Draft", "Pending", "Published", "Archived", "Rejected"
>>>>>>> develop
  PublishedDate datetime
  CreatedDate datetime [default: `getdate()`]
  UpdatedDate datetime [default: `getdate()`]
}

Table Notifications {
  NotificationId int [pk, increment]
  UserId int
  Title nvarchar(200) [not null]
  Message nvarchar(500) [not null]
<<<<<<< HEAD
  Type nvarchar(50)
=======
  Type nvarchar(50) // Enum: "Registration", "Confirmation", "Reminder", "Cancellation", "Completion", "Result", "Event", "System", "Medical", "Warning", "Info"
>>>>>>> develop
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
<<<<<<< HEAD
  Status nvarchar(20) [default: 'New']
=======
  Status nvarchar(20) [default: 'New'] // Enum: "New", "Read", "InProgress", "Resolved", "Closed"
>>>>>>> develop
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
<<<<<<< HEAD
Ref: BloodCompatibility.ToBloodTypeId > BloodTypes.BloodTypeId
=======
Ref: BloodCompatibility.ToBloodTypeId > BloodTypes.BloodTypeId

// Enum Values Documentation
// =========================
// Users.Gender: "Nam", "Nữ", "Khác"
// 
// BloodDonationEvents.Status: 
//   - "Draft" (Bản nháp), "Published" (Đã công bố), "Active" (Đang diễn ra)
//   - "Completed" (Hoàn thành), "Cancelled" (Đã hủy), "Postponed" (Hoãn lại)
//   - "Full" (Đã đầy), "Closed" (Đóng đăng ký)
//
// DonationRegistrations.Status:
//   - "Registered" (Đã đăng ký), "Confirmed" (Đã xác nhận), "CheckedIn" (Đã đến)
//   - "Screening" (Đang sàng lọc), "Eligible" (Đủ điều kiện), "Ineligible" (Không đủ điều kiện)
//   - "Donating" (Đang hiến máu), "Completed" (Hoàn tất), "Cancelled" (Đã hủy)
//   - "NoShow" (Không đến), "Failed" (Thất bại)
//
// HealthScreening.DisqualifyReason:
//   - Medical: "LowHemoglobin", "HighBloodPressure", "LowBloodPressure", "Fever", "LowWeight"
//   - History: "RecentDonation", "MedicalHistory", "CurrentMedication", "RecentVaccination"
//   - Condition: "Pregnancy", "Breastfeeding", "RecentSurgery", "InfectionRisk", "Other"
//
// DonationHistory.Status: "Started", "InProgress", "Completed", "Stopped", "Failed"
//
// Notifications.Type:
//   - Process: "Registration", "Confirmation", "Reminder", "Cancellation", "Completion"
//   - Information: "Result", "Event", "System", "Medical", "Warning", "Info"
//
// ContactMessages.Status: "New", "Read", "InProgress", "Resolved", "Closed"
//
// Note: See TABLE_ENUMS.md for complete enum definitions and usage examples
>>>>>>> develop
