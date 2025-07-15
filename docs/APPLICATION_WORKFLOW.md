# Luồng hoạt động của ứng dụng Blood Donation Website

Tài liệu này mô tả các luồng hoạt động và quy trình chính trong ứng dụng Blood Donation Website, cho thấy cách các thành phần khác nhau tương tác để cung cấp chức năng của ứng dụng.

## Các luồng hoạt động dành cho người dùng

### 1. Đăng ký và xác thực người dùng

1. **Quy trình đăng ký**
   - Người dùng điền form đăng ký trên frontend
   - Action `AccountController.Register()` xác thực đầu vào
   - `IAccountService.RegisterAsync()` tạo tài khoản người dùng
   - `IEmailService.SendVerificationEmailAsync()` gửi email xác thực
   - Người dùng nhận xác nhận và được chuyển hướng đến trang đăng nhập

2. **Quy trình đăng nhập**
   - Người dùng gửi thông tin đăng nhập
   - Action `AccountController.Login()` xử lý thông tin đăng nhập
   - `IAccountService.LoginAsync()` xác thực người dùng
   - Phiên làm việc được tạo và người dùng được chuyển hướng đến dashboard

3. **Quản lý hồ sơ**
   - Người dùng truy cập trang hồ sơ
   - `ProfileController.Index()` lấy dữ liệu hồ sơ người dùng qua `IProfileService`
   - Người dùng cập nhật thông tin hồ sơ
   - `ProfileController.Update()` xác thực và xử lý các thay đổi
   - `IProfileService.UpdateProfileAsync()` cập nhật dữ liệu người dùng
   - Người dùng nhận xác nhận thay đổi

### 2. Chu trình sự kiện hiến máu

1. **Tạo sự kiện (Admin)**
   - Admin tạo sự kiện mới qua giao diện quản trị
   - `AdminController.CreateEvent()` xác thực đầu vào
   - `IBloodDonationEventService.CreateEventAsync()` tạo bản ghi sự kiện
   - `INotificationService.NotifyEligibleDonorsAsync()` thông báo cho người hiến tiềm năng

2. **Khám phá sự kiện (Người dùng)**
   - Người dùng duyệt các sự kiện sắp tới
   - `HomeController.Events()` lấy các sự kiện qua `IBloodDonationEventService`
   - Các sự kiện được hiển thị với các tùy chọn lọc
   - Người dùng chọn một sự kiện để xem chi tiết
   - `HomeController.EventDetails()` lấy thông tin chi tiết

3. **Đăng ký sự kiện**
   - Người dùng đăng ký cho một sự kiện
   - `HomeController.RegisterForEvent()` xử lý yêu cầu đăng ký
   - `IDonationRegistrationService.RegisterUserForEventAsync()` tạo đăng ký
   - `INotificationService.SendRegistrationConfirmationAsync()` xác nhận đăng ký
   - Người dùng nhận xác nhận và lời mời lịch

4. **Check-in sự kiện**
   - Người dùng đến sự kiện và check-in
   - `IDonationRegistrationService.CheckInUserAsync()` ghi lại việc tham dự
   - Người dùng được hướng dẫn đến khu vực sàng lọc sức khỏe

5. **Sàng lọc sức khỏe**
   - Nhân viên y tế thực hiện sàng lọc sức khỏe
   - `IHealthScreeningService.RecordScreeningAsync()` lưu kết quả sàng lọc
   - Hệ thống xác định tính đủ điều kiện dựa trên kết quả sàng lọc
   - Người dùng được phê duyệt cho hiến máu hoặc bị từ chối

6. **Quy trình hiến máu**
   - Hiến máu thành công được ghi lại
   - `IDonationHistoryService.RecordDonationAsync()` tạo bản ghi hiến máu
   - `IUserService.UpdateLastDonationDateAsync()` cập nhật ngày hiến máu của người dùng
   - `INotificationService.SendDonationCertificateAsync()` cấp chứng chỉ
   - Người dùng nhận thông báo cảm ơn và chứng chỉ

### 3. Dashboard người dùng và lịch sử

1. **Xem lịch sử hiến máu**
   - Người dùng truy cập trang lịch sử hiến máu
   - `ProfileController.DonationHistory()` lấy lịch sử qua `IDonationHistoryService`
   - Lịch sử được hiển thị với thống kê và ngày đủ điều kiện tiếp theo

2. **Quản lý thông báo**
   - Người dùng nhận thông báo về sự kiện, xác nhận, v.v.
   - `INotificationService` gửi thông báo
   - Người dùng có thể đánh dấu thông báo là đã đọc, xóa chúng hoặc thực hiện hành động

3. **Thông tin sức khỏe**
   - Người dùng xem lịch sử sàng lọc sức khỏe
   - `ProfileController.HealthScreenings()` lấy dữ liệu qua `IHealthScreeningService`
   - Hệ thống hiển thị xu hướng sức khỏe và các khuyến nghị

## Các luồng hoạt động quản trị

### 1. Quản lý người dùng

1. **Quản trị người dùng**
   - Admin truy cập phần quản lý người dùng
   - `AdminController.Users()` lấy người dùng qua `IUserService`
   - Admin có thể tìm kiếm, lọc, sắp xếp và quản lý người dùng
   - Các hành động bao gồm kích hoạt/vô hiệu hóa, gán vai trò, v.v.

2. **Quản lý vai trò**
   - Admin quản lý vai trò và quyền
   - `AdminController.Roles()` làm việc với `IRoleService`
   - Admin có thể tạo, chỉnh sửa và gán vai trò cho người dùng

### 2. Quản lý sự kiện

1. **Quản trị sự kiện**
   - Admin quản lý các sự kiện sắp tới và đã qua
   - `AdminController.Events()` sử dụng `IBloodDonationEventService`
   - Admin có thể tạo, chỉnh sửa, hủy sự kiện và quản lý đăng ký

2. **Quản lý địa điểm**
   - Admin quản lý các địa điểm hiến máu
   - `AdminController.Locations()` sử dụng `ILocationService`
   - Admin có thể thêm, chỉnh sửa hoặc vô hiệu hóa địa điểm

### 3. Báo cáo và phân tích

1. **Báo cáo thống kê**
   - Admin truy cập dashboard báo cáo
   - `AdminController.Statistics()` sử dụng `IStatisticsService`
   - Hệ thống tạo báo cáo về hiến máu, sự kiện, người dùng, v.v.
   - Dữ liệu có thể được lọc theo khoảng thời gian, địa điểm, nhóm máu, v.v.

2. **Xu hướng hiến máu**
   - Admin xem xu hướng và mẫu hiến máu
   - `IStatisticsService.GetDonationTrendsAsync()` phân tích dữ liệu lịch sử
   - Hệ thống hiển thị biểu đồ và đồ thị về hoạt động hiến máu

## Luồng hoạt động hệ thống

### 1. Bảo trì dữ liệu

1. **Tạo dữ liệu seed**
   - Khi thiết lập ban đầu, seeders tạo dữ liệu tham chiếu
   - `AdminSeeder`, `BloodTypeSeeder`, v.v. tạo các bản ghi ban đầu
   - Hệ thống được khởi tạo với dữ liệu cơ sở cần thiết

2. **Quản lý cấu hình**
   - Admin quản lý cài đặt hệ thống
   - `AdminController.Settings()` sử dụng `ISettingService`
   - Cài đặt ảnh hưởng đến hành vi hệ thống, mẫu thông báo, v.v.

### 2. Giao tiếp

1. **Thông báo email**
   - Hệ thống gửi email giao dịch
   - `IEmailService` xử lý định dạng và gửi email
   - Mẫu cho các loại thông báo khác nhau

2. **Thông báo trong ứng dụng**
   - Hệ thống tạo thông báo cho người dùng và admin
   - `INotificationService` tạo và gửi thông báo
   - Người dùng nhận thông báo theo thời gian thực hoặc được lưu trữ

## Điểm tích hợp

### 1. Hệ thống bên ngoài

1. **Tích hợp dịch vụ email**
   - Hệ thống tích hợp với các nhà cung cấp dịch vụ email
   - Triển khai `IEmailService` xử lý việc tích hợp

2. **Tích hợp lịch**
   - Đăng ký sự kiện có thể tạo lời mời lịch
   - Hỗ trợ các định dạng lịch tiêu chuẩn (iCal, v.v.)

### 2. Dịch vụ xác thực

1. **Nhà cung cấp định danh bên ngoài**
   - Tích hợp tùy chọn với các nhà cung cấp định danh bên ngoài
   - Hỗ trợ đăng nhập xã hội hoặc xác thực tổ chức

## Kết luận

Blood Donation Website triển khai các luồng hoạt động toàn diện để quản lý quy trình hiến máu từ đầu đến cuối. Kiến trúc hỗ trợ cả quy trình dành cho người dùng và chức năng quản trị, với sự phân tách rõ ràng về mối quan tâm giữa các lớp. Mỗi luồng hoạt động được triển khai thông qua sự hợp tác giữa controllers, services và lớp dữ liệu, đảm bảo khả năng bảo trì và mở rộng.
