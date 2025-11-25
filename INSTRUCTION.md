# Quy trình hiến máu tình nguyện

## 1. Đăng ký
- Người dùng đăng ký sự kiện hiến máu (`BloodDonationEvents`)
- Tạo bản ghi trong bảng `DonationRegistrations`:
  - `Status = 'Registered'`
  - `IsEligible = 0`

## 2. Xác nhận (tùy chọn)
- Hệ thống hoặc ban tổ chức xác nhận đăng ký
- Cập nhật:
  - `Status = 'Confirmed'`
  - `IsEligible = 0`

### Có thể bị huỷ xác nhận nếu:
- Sự kiện đã đủ người (`CurrentDonors ≥ MaxDonors`)
- Nhóm máu không phù hợp (`RequiredBloodTypes`)
- Gần đây đã hiến máu (`LastDonationDate` quá gần)
- Tài khoản bị khoá (`IsActive = 0`)

## 3. Check-in
- Người dùng đến địa điểm và thực hiện check-in
- Cập nhật:
  - `Status = 'CheckedIn'`
  - `IsEligible = 0`
  - `CheckInTime = [thời gian hiện tại]`

## 4. Sàng lọc sức khỏe
- Tạo bản ghi trong bảng `HealthScreening`:
  - Bao gồm các thông số: `Weight`, `Height`, `BloodPressure`, `HeartRate`, `Temperature`, `Hemoglobin`, `ScreeningDate`
  - Gắn với `RegistrationId`

### Kết quả sàng lọc:
#### Đạt:
- `IsEligible = 1`
- Cập nhật `DonationRegistrations`:
  - `Status = 'Eligible'`
  - `IsEligible = 1`

#### Không đạt:
- `IsEligible = 0`
- `DisqualifyReason = [lý do loại]`
- Cập nhật `DonationRegistrations`:
  - `Status = 'Ineligible'`
  - `IsEligible = 0`

## 5. Hiến máu
- Người dùng bắt đầu hiến máu
- Cập nhật `DonationRegistrations`:
  - `Status = 'Donating'`

## 6. Hoàn tất
- Sau khi hiến máu thành công:
  - Tạo bản ghi trong bảng `DonationHistory`:
    - `DonationDate`, `BloodTypeId`, `Volume = 350`, `Status = 'Completed'`
    - `NextEligibleDate = [tính theo ngày hiến + thời gian nghỉ]`
  - Cập nhật `DonationRegistrations`:
    - `Status = 'Completed'`
    - `CompletionTime = [thời gian hiện tại]`

---

## Trường hợp đặc biệt

### Huỷ đăng ký
- Người dùng hoặc hệ thống huỷ:
  - `Status = 'Cancelled'`
  - `CancellationReason = [lý do]`

### Không đến
- Người dùng không check-in:
  - `Status = 'NoShow'`

### Thất bại
- Vấn đề trong quá trình hiến máu:
  - `Status = 'Failed'` trong `DonationRegistrations`
  - Tạo bản ghi trong `DonationHistory` nếu cần: `Status = 'Failed'`

---

## Trang "Chờ sàng lọc"
Hiển thị các bản ghi:
- `Status = 'CheckedIn' AND IsEligible = 0`
- hoặc `Status = 'Ineligible' AND IsEligible = 0`

---

## Tổng hợp giá trị ENUM liên quan

### DonationRegistrations.Status:
- Registered
- Confirmed
- CheckedIn
- Screening *(không có trong quy trình, nên có thể bỏ hoặc thêm vào luồng rõ ràng)*
- Eligible
- Ineligible
- Donating
- Completed
- Cancelled
- NoShow
- Failed

### HealthScreening.DisqualifyReason:
- LowHemoglobin
- HighBloodPressure
- LowBloodPressure
- Fever
- LowWeight
- RecentDonation
- MedicalHistory
- CurrentMedication
- RecentVaccination
- Pregnancy
- Breastfeeding
- RecentSurgery
- InfectionRisk
- Other

### BloodDonationEvents.Status:
- Draft
- Published
- Active
- Completed
- Cancelled
- Postponed
- Full
- Closed

### Users.Gender:
- Male
- Female
- Other

### DonationHistory.Status:
- Started
- InProgress
- Completed
- Stopped
- Failed

### Notifications.Type:
- Registration
- Confirmation
- Reminder
- Cancellation
- Completion
- Result
- Event
- System
- Medical
- Warning
- Info

### ContactMessages.Status:
- New
- Read
- InProgress
- Resolved
- Closed

---

## Ghi chú
- Giá trị `Screening` trong `DonationRegistrations.Status` không có trong quy trình, nên cần cân nhắc thêm rõ bước này hoặc loại bỏ.
- Các bước được mapping rõ ràng với `Status`, `IsEligible` và các mốc thời gian quan trọng.
