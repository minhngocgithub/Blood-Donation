# Blood Donation – Hệ Thống Quản Lý Hiến Máu Nhân Đạo

## Giới thiệu

Blood Donation là hệ thống quản lý và đăng ký hiến máu nhân đạo, hỗ trợ người hiến máu, bác sĩ, nhân viên y tế, bệnh viện và quản trị viên trong việc tổ chức, theo dõi, và báo cáo các hoạt động hiến máu. Hệ thống cung cấp quy trình khép kín từ đăng ký, sàng lọc sức khỏe, thực hiện hiến máu, lưu trữ lịch sử, đến thống kê và thông báo.

---

## Tính năng chính

### 1. Quản lý người dùng
- Đăng ký, đăng nhập, đăng xuất
- Cập nhật thông tin cá nhân, đổi mật khẩu
- Xem lịch sử hiến máu, nhận thông báo

### 2. Quản lý sự kiện hiến máu
- Tạo, cập nhật, xoá sự kiện
- Quản lý số lượng người đăng ký, trạng thái sự kiện

### 3. Đăng ký hiến máu
- Đăng ký/hủy đăng ký tham gia sự kiện
- Kiểm tra điều kiện tham gia, check-in tại sự kiện

### 4. Sàng lọc sức khỏe
- Nhập kết quả kiểm tra sức khỏe, đánh giá điều kiện hiến máu
- Lưu lý do loại (nếu không đủ điều kiện)

### 5. Lưu trữ lịch sử hiến máu
- Ghi nhận lịch sử hiến máu (thể tích, loại máu, thời gian)
- Tính ngày đủ điều kiện lần hiến tiếp theo

### 6. Quản lý tin tức & thông báo
- Đăng, cập nhật bài viết/tin tức
- Quản lý trạng thái xuất bản, gửi thông báo

### 7. Cấu hình hệ thống
- Quản lý, cập nhật các giá trị cấu hình hệ thống

### 8. Báo cáo & thống kê
- Thống kê số lượng người hiến máu, đăng ký từng sự kiện
- Phân tích nhóm máu, xuất báo cáo

---

## Quy trình hoạt động

### Người dùng (Người hiến máu)
1. **Đăng ký tài khoản:** Điền thông tin cá nhân, xác thực email, nhận mã định danh.
2. **Đăng nhập:** Xác thực thông tin, truy cập trang cá nhân.
3. **Cập nhật thông tin:** Sửa đổi thông tin cá nhân, đổi mật khẩu.
4. **Xem sự kiện:** Xem, tìm kiếm các sự kiện hiến máu sắp diễn ra.
5. **Đăng ký tham gia:** Kiểm tra điều kiện, đăng ký sự kiện, nhận thông báo xác nhận.
6. **Hủy đăng ký:** Hủy trước ngày sự kiện, nhận thông báo xác nhận hủy.
7. **Kiểm tra điều kiện hiến máu:** Xem lịch sử, biết ngày đủ điều kiện hiến tiếp.
8. **Check-in tại sự kiện:** Đối chiếu đăng ký, cập nhật trạng thái “Đã đến”.
9. **Sàng lọc sức khỏe:** Khám, xét nghiệm, lưu kết quả, đánh giá điều kiện.
10. **Thực hiện hiến máu:** Ghi nhận chi tiết lần hiến, tính ngày đủ điều kiện tiếp theo.
11. **Xem lịch sử & thông báo:** Theo dõi lịch sử hiến máu, nhận thông báo cá nhân.

### Quản trị viên (Admin)
- Đăng nhập hệ thống quản trị, phân quyền
- Quản lý người dùng, bệnh viện, sự kiện, tin tức/thông báo
- Kiểm duyệt đăng ký, kết quả sàng lọc
- Thống kê, xuất báo cáo, cấu hình hệ thống

### Bệnh viện (Đơn vị tổ chức)
- Tạo/sửa sự kiện, phân công nhân sự
- Theo dõi tiến độ, nhận thông báo, xem báo cáo
- Cập nhật thông tin cơ sở

### Bác sĩ
- Đăng nhập, xem danh sách người đến sự kiện
- Khám, tư vấn, xét nghiệm, đánh giá điều kiện hiến máu
- Hoàn tất sàng lọc, cập nhật trạng thái

### Nhân viên y tế
- Đăng nhập, check-in người hiến
- Thực hiện hiến máu, nhập dữ liệu, quan sát sau hiến
- Kết thúc phiên hiến máu, cập nhật trạng thái

---

## Kiến trúc dữ liệu (Cơ sở dữ liệu)

### Các bảng chính

- **Users:** Thông tin người dùng, phân quyền (admin, user, hospital, doctor, staff)
- **Roles:** Danh sách vai trò
- **BloodTypes, BloodCompatibility:** Nhóm máu, tương thích truyền máu
- **Locations:** Địa điểm tổ chức sự kiện
- **BloodDonationEvents:** Sự kiện hiến máu
- **DonationRegistrations:** Đăng ký tham gia sự kiện
- **HealthScreening:** Kết quả sàng lọc sức khỏe
- **DonationHistory:** Lịch sử hiến máu
- **News, NewsCategories:** Tin tức, danh mục tin
- **Notifications:** Thông báo cho người dùng
- **Settings:** Cấu hình hệ thống
- **ContactMessages:** Liên hệ, phản hồi

### Quan hệ giữa các bảng
- Users liên kết Roles, BloodTypes
- BloodDonationEvents liên kết Locations, Users (người tạo)
- DonationRegistrations liên kết Users, BloodDonationEvents
- HealthScreening liên kết DonationRegistrations, Users (bác sĩ)
- DonationHistory liên kết Users, BloodDonationEvents, DonationRegistrations
- News liên kết NewsCategories, Users (tác giả)
- Notifications liên kết Users
- ContactMessages liên kết Users (người xử lý)
- BloodCompatibility liên kết BloodTypes

---

## Kiến trúc dự án

Dự án được phát triển theo mô hình kiến trúc MVC (Model-View-Controller) với nhiều lớp rõ ràng:

### Lớp trình bày (Presentation Layer)
- **Controllers**: Xử lý yêu cầu HTTP, điều phối luồng dữ liệu, gọi services
- **Views**: Hiển thị giao diện người dùng bằng Razor Pages

### Lớp dịch vụ (Service Layer)
- **Interfaces**: Định nghĩa hợp đồng cho các dịch vụ
- **Implementations**: Triển khai logic nghiệp vụ của ứng dụng

### Lớp truy cập dữ liệu (Data Access Layer)
- **ApplicationDbContext**: Quản lý kết nối database, định nghĩa DbSets
- **Configurations**: Cấu hình Entity Framework cho các entities
- **Seeders**: Khởi tạo dữ liệu mẫu

### Lớp mô hình (Model Layer)
- **Entities**: Định nghĩa cấu trúc dữ liệu và quan hệ
- **DTOs**: Đối tượng chuyển dữ liệu giữa các lớp
- **ViewModels**: Mô hình dữ liệu dành riêng cho views

### Tiện ích (Utilities)
- **Extensions**: Các phương thức mở rộng
- **Filters**: Bộ lọc cho các controller

Chi tiết kiến trúc dự án có thể xem tại [PROJECT_ARCHITECTURE.md](docs/PROJECT_ARCHITECTURE.md).

---

## Hướng dẫn cài đặt & khởi động

1. **Yêu cầu hệ thống:**
   - .NET Core SDK
   - SQL Server hoặc hệ quản trị CSDL tương thích
   - Node.js (nếu sử dụng frontend riêng)

2. **Cài đặt:**
   - Clone source code về máy
   - Cấu hình chuỗi kết nối CSDL trong `appsettings.json`
   - Chạy lệnh migrate để tạo database (nếu có hỗ trợ)
   - Chạy lệnh build và start dự án:
     ```
     dotnet build
     dotnet run
     ```
   - Truy cập địa chỉ được cung cấp (thường là http://localhost:5000)

3. **Tài khoản mẫu:**
   - Admin: Tài khoản admin sẽ được seed sẵn hoặc tạo thủ công trong database.

---

## Đóng góp & phát triển

- Đọc thêm tài liệu trong thư mục `docs/` để hiểu về cấu trúc CSS, luồng trang chủ, dịch vụ, DTOs, v.v.
- Đóng góp code, báo lỗi hoặc đề xuất tính năng mới qua GitHub Issues/Pull Requests.

---

## Liên hệ

- Mọi thắc mắc, góp ý vui lòng gửi qua chức năng Liên hệ trên hệ thống hoặc email của nhóm phát triển.

---

**Tài liệu tham khảo:**  
- [DATABASE.md](docs/DATABASE.md) – Sơ đồ & mô tả cơ sở dữ liệu  
- [DESCRIPTION.md](docs/DESCRIPTION.md) – Quy trình & nghiệp vụ hệ thống  
- [PROJECT_ARCHITECTURE.md](docs/PROJECT_ARCHITECTURE.md) – Kiến trúc dự án & vai trò các thành phần  
- [APPLICATION_WORKFLOW.md](docs/APPLICATION_WORKFLOW.md) – Luồng hoạt động chi tiết của ứng dụng  
- [docs/](docs/) – Hướng dẫn chi tiết về các thành phần hệ thống

---

Nếu cần bổ sung chi tiết về API, giao diện, hoặc hướng dẫn sử dụng cụ thể, hãy liên hệ nhóm phát triển hoặc xem thêm trong thư mục `docs/`.