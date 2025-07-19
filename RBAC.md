## Bảng phân quyền
# Dựa trên cơ sở dữ liệu và quy trình hiến máu, bảng phân quyền dưới đây phân định quyền truy cập và thao tác cho các vai trò (Roles: Admin, User, Hospital, Doctor, Staff).

| Chức năng / Trang | Admin | User | Hospital | Doctor | Staff |
|-------------------|:-----:|:----:|:--------:|:------:|:-----:|
| **Lịch sử hiến máu** ||||||
| Xem lịch sử cá nhân (DonationHistory) | ✓ | ✓ |   |   |   |
| Xem tất cả lịch sử hiến máu           | ✓ |   | ✓ | ✓ | ✓ |
| **Đăng ký hiến máu** ||||||
| Xem sự kiện (BloodDonationEvents)      | ✓ | ✓ | ✓ | ✓ | ✓ |
| Đăng ký sự kiện (DonationRegistrations)|   | ✓ |   |   |   |
| Hủy đăng ký (Status = 'Cancelled')     | ✓ | ✓ | ✓ | ✓ | ✓ |
| Xác nhận đăng ký (Status = 'Confirmed')| ✓ |   | ✓ | ✓ | ✓ |
| Check-in (Status = 'CheckedIn')        | ✓ |   | ✓ | ✓ | ✓ |
| **Sàng lọc sức khỏe** ||||||
| Xem danh sách chờ sàng lọc             | ✓ |   | ✓ | ✓ | ✓ |
| Nhập dữ liệu sàng lọc (HealthScreening)| ✓ |   | ✓ | ✓ | ✓ |
| Cập nhật kết quả sàng lọc (Eligible/Ineligible) | ✓ |   | ✓ | ✓ | ✓ |
| **Quản lý sự kiện hiến máu** ||||||
| Tạo/sửa/xóa sự kiện (BloodDonationEvents) | ✓ |   | ✓ |   |   |
| Cập nhật trạng thái sự kiện            | ✓ |   | ✓ |   | ✓ |
| **Thông báo** ||||||
| Xem thông báo cá nhân (Notifications)  | ✓ | ✓ | ✓ | ✓ | ✓ |
| Gửi thông báo                          | ✓ |   | ✓ | ✓ | ✓ |
| **Hồ sơ người dùng** ||||||
| Xem/cập nhật hồ sơ cá nhân (Users)     | ✓ | ✓ |   |   |   |
| Xem hồ sơ người dùng khác              | ✓ |   | ✓ | ✓ | ✓ |
| Xác minh email (EmailVerified)         | ✓ | ✓ |   |   |   |
| **Báo cáo và thống kê** ||||||
| Xem báo cáo (DonationHistory, HealthScreening) | ✓ |   | ✓ | ✓ | ✓ |
| Xuất báo cáo                           | ✓ |   | ✓ |   |   |
| **Quản lý danh mục tin tức** ||||||
| Tạo/sửa/xóa danh mục (NewsCategories)  | ✓ |   |   |   |   |
| **Quản lý tin tức** ||||||
| Tạo/sửa/xóa tin tức (News)             | ✓ |   | ✓ |   | ✓ |
| Phê duyệt tin tức (IsPublished)        | ✓ |   | ✓ |   |   |
| **Quản lý liên hệ** ||||||
| Xem/xử lý tin nhắn (ContactMessages)   | ✓ |   | ✓ |   | ✓ |
| **Quản lý cài đặt hệ thống** ||||||
| Cập nhật cài đặt (Settings)            | ✓ |   |   |   |   |

## Ghi chú:

Admin: Quyền toàn diện, quản lý toàn bộ hệ thống.
User: Người dùng thông thường, chỉ thao tác trên dữ liệu cá nhân (đăng ký, xem lịch sử, hồ sơ).
Hospital: Quản lý sự kiện, báo cáo, và xử lý dữ liệu liên quan đến hiến máu.
Doctor: Thực hiện sàng lọc sức khỏe, xem dữ liệu người dùng và báo cáo.
Staff: Hỗ trợ check-in, nhập dữ liệu sàng lọc, quản lý sự kiện cơ bản.
