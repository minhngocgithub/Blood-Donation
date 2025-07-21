# Luồng hoạt động hiến máu tình nguyện

## 1. Đăng ký sự kiện hiến máu
- Người dùng chọn sự kiện và đăng ký.
- Hệ thống tạo bản ghi trong bảng `DonationRegistrations`:
  - `Status = 'Registered'`
  - `IsEligible = 0`

## 2. Xác nhận đăng ký (tùy chọn)
- Hệ thống hoặc ban tổ chức xác nhận đăng ký.
- Nếu hợp lệ, cập nhật:
  - `Status = 'Confirmed'`
  - `IsEligible = 0`
- **Có thể bị huỷ xác nhận nếu:**
  - Sự kiện đã đủ người (`CurrentDonors ≥ MaxDonors`)
  - Nhóm máu không phù hợp (`RequiredBloodTypes`)
  - Gần đây đã hiến máu (`LastDonationDate` quá gần)
  - Tài khoản bị khoá (`IsActive = 0`)
  - Khi đó, cập nhật `Status = 'Cancelled'` và ghi rõ `CancellationReason`.

## 3. Check-in tại sự kiện
- Người dùng đến địa điểm tổ chức và thực hiện check-in.
- Hệ thống cập nhật:
  - `Status = 'CheckedIn'`
  - `IsEligible = 0`
  - `CheckInTime = [thời gian hiện tại]`
- **Chỉ có thể check-in vào đúng ngày sự kiện, trong khung giờ cho phép.**

## 4. Sàng lọc sức khỏe
- Nhân viên y tế thực hiện sàng lọc, nhập thông số vào bảng `HealthScreening` (gắn với `RegistrationId`):
  - `Weight`, `Height`, `BloodPressure`, `HeartRate`, `Temperature`, `Hemoglobin`, `ScreeningDate`, ...
- Kết quả sàng lọc:
  - **Đạt:**  
    - `IsEligible = 1` (trong `HealthScreening`)
    - Cập nhật `DonationRegistrations`:
      - `Status = 'Eligible'`
      - `IsEligible = 1`
  - **Không đạt:**  
    - `IsEligible = 0`
    - `DisqualifyReason = [lý do loại]`
    - Cập nhật `DonationRegistrations`:
      - `Status = 'Ineligible'`
      - `IsEligible = 0`

## 5. Hiến máu
- Người dùng đủ điều kiện bắt đầu hiến máu.
- Hệ thống cập nhật `DonationRegistrations`:
  - `Status = 'Donating'`

## 6. Hoàn tất hiến máu
- Sau khi hiến máu thành công:
  - Tạo bản ghi trong bảng `DonationHistory`:
    - `DonationDate`, `BloodTypeId`, `Volume = 350`, `Status = 'Completed'`
    - `NextEligibleDate = [tính theo ngày hiến + thời gian nghỉ]`
  - Cập nhật `DonationRegistrations`:
    - `Status = 'Completed'`
    - `CompletionTime = [thời gian hiện tại]`

---

## Các trường hợp đặc biệt
- **Huỷ đăng ký:**  
  Người dùng hoặc hệ thống huỷ đăng ký:
  - `Status = 'Cancelled'`
  - `CancellationReason = [lý do]`

- **Không đến sự kiện:**  
  Người dùng không check-in:
  - `Status = 'NoShow'`

- **Thất bại:**  
  Có vấn đề trong quá trình hiến máu:
  - `Status = 'Failed'` trong `DonationRegistrations`
  - Có thể tạo bản ghi trong `DonationHistory` với `Status = 'Failed'`

---

## Trang "Chờ sàng lọc"
- Hiển thị các bản ghi đăng ký:
  - `Status = 'CheckedIn' AND IsEligible = 0`
  - hoặc `Status = 'Ineligible' AND IsEligible = 0`

---

## Mapping các giá trị ENUM liên quan

### DonationRegistrations.Status:
- Registered
- Confirmed
- CheckedIn
- Screening
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

**Ghi chú:**  
- Giá trị `Screening` trong `DonationRegistrations.Status` không xuất hiện trong quy trình thực tế, nên có thể loại bỏ hoặc bổ sung rõ ràng nếu cần.
- Các bước được mapping rõ ràng với các trường `Status`, `IsEligible`, các mốc thời gian (`CheckInTime`, `CompletionTime`), và lý do loại (`DisqualifyReason`, `CancellationReason`).

---

**Tóm tắt:**  
Luồng hoạt động của hệ thống bám sát các trạng thái và quy trình thực tế, đảm bảo mọi bước đều được ghi nhận và kiểm soát chặt chẽ qua các trường trạng thái và lịch sử trong database.
