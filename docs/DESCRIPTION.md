# Hệ thống đăng ký hiến máu nhân đạo – Quy trình hoạt động

## 1. Người dùng (Người hiến máu)

### 1.1. Đăng ký tài khoản
- Chọn chức năng Đăng ký, điền thông tin cá nhân (họ tên, email, số điện thoại, mật khẩu, …).
- Hệ thống kiểm tra dữ liệu, lưu hồ sơ vào bảng Users và gửi email xác nhận.
- Sau khi đăng ký thành công, hệ thống cấp mã định danh người hiến máu (donor ID) gửi qua email.
- Nhận thông báo đăng ký thành công, dùng thông tin này để đăng nhập.

### 1.2. Đăng nhập
- Nhập email và mật khẩu.
- Hệ thống đối chiếu với bảng Users.
- Nếu xác thực đúng, tạo phiên làm việc (session) và chuyển hướng đến trang cá nhân.
- Trên trang cá nhân: cập nhật thông tin, đổi mật khẩu, xem sự kiện, lịch sử hiến máu, nhận thông báo.

### 1.3. Cập nhật thông tin cá nhân
- Sửa đổi các trường thông tin cá nhân (họ tên, số điện thoại, địa chỉ, nhóm máu, ...).
- Lưu và cập nhật vào bảng Users, báo lại kết quả thành công.
- Đổi mật khẩu: kiểm tra mật khẩu cũ, cập nhật mật khẩu mới.

### 1.4. Xem sự kiện hiến máu
- Xem danh sách các sự kiện hiến máu sắp diễn ra (truy vấn bảng BloodDonationEvents).
- Thông tin: tên sự kiện, ngày/giờ, địa điểm, mô tả.
- Có thể tìm kiếm theo địa điểm hoặc bộ lọc khác.

### 1.5. Đăng ký tham gia sự kiện
- Tại chi tiết sự kiện, nhấn Đăng ký tham gia.
- Hệ thống kiểm tra điều kiện (đối tượng, khoảng cách lần hiến trước, kết quả sức khỏe trước đó).
- Nếu đủ điều kiện: tạo bản ghi mới trong DonationRegistrations (trạng thái “Đang chờ”), gửi thông báo xác nhận đăng ký thành công.
- Nếu chưa đủ điều kiện: thông báo lý do, không tạo đăng ký.

### 1.6. Hủy đăng ký sự kiện
- Có thể hủy đăng ký trước ngày sự kiện.
- Khi hủy, cập nhật bản ghi trong DonationRegistrations (trạng thái “Đã hủy”) hoặc xóa bản ghi.
- Gửi thông báo xác nhận hủy.

### 1.7. Kiểm tra điều kiện hiến máu
- Kiểm tra lịch sử hiến máu để biết khi nào đủ điều kiện hiến tiếp.
- Hệ thống tính ngày đủ điều kiện dựa trên lần hiến gần nhất và quy định (ví dụ: 3 tháng với nam, 4 tháng với nữ).
- Thông tin lưu trong DonationHistory và hiển thị cho người dùng.

### 1.8. Check-in tại sự kiện
- Khi đến địa điểm, người dùng check-in (có thể dùng mã QR).
- Hệ thống đối chiếu đăng ký trong DonationRegistrations, nếu hợp lệ cập nhật trạng thái “Đã đến” và lưu thời gian check-in.
- Được đưa đến khu vực sàng lọc sức khỏe.

### 1.9. Sàng lọc sức khỏe
- Được bác sĩ khám, tư vấn, khai thác tiền sử bệnh lý, khám lâm sàng.
- Nếu cần, lấy mẫu xét nghiệm (huyết sắc tố, viêm gan B, ...).
- Kết quả lưu vào bảng HealthScreening (liên kết user và event).
- Nếu không đủ điều kiện: bác sĩ chọn nguyên nhân loại, hệ thống đánh dấu “Không đủ điều kiện” trong DonationRegistrations, tạo thông báo cho người dùng.
- Nếu đủ điều kiện: cập nhật trạng thái “Đủ điều kiện”, tiếp tục bước tiếp theo.

### 1.10. Thực hiện hiến máu
- Qua sàng lọc, tiến hành hiến máu.
- Nhân viên y tế theo dõi, ghi chép thời gian, lượng máu.
- Nhập chi tiết lần hiến vào DonationHistory (mã sự kiện, user_id, ngày giờ, thể tích, nhóm máu).
- Hệ thống tính ngày đủ điều kiện hiến tiếp theo, lưu vào hồ sơ.
- Gửi thông báo xác nhận hoàn tất hiến máu, có thể kèm giấy chứng nhận.

### 1.11. Xem lịch sử hiến máu và thông báo
- Xem lại lịch sử hiến máu (truy vấn DonationHistory).
- Xem danh sách thông báo cá nhân (Notifications): xác nhận đăng ký, nhắc nhở, thông báo sự kiện mới.

---

## 2. Quản trị viên (Admin)

### 2.1. Đăng nhập hệ thống quản trị
- Đăng nhập bằng tài khoản admin (bảng Users có vai trò admin).
- Kiểm tra thông tin, phân quyền.
- Sau đăng nhập, mở giao diện quản trị với các menu: quản lý thành viên, sự kiện, tin tức/thông báo, báo cáo, cấu hình.

### 2.2. Quản lý người dùng và bệnh viện
- Thêm/sửa/xóa tài khoản người dùng, thiết lập quyền hạn (người hiến, bác sĩ, y tá, bệnh viện).
- Cập nhật trực tiếp trên bảng Users.
- Thêm mới/ngắt kết nối các cơ sở hiến máu/ngân hàng máu (bảng liên quan “Bệnh viện”).
- Phân quyền, chỉ định bác sĩ/nhân viên cho từng bệnh viện.

### 2.3. Quản lý sự kiện hiến máu
- Tạo sự kiện mới: nhập tên, mô tả, địa điểm, thời gian, quy trình, số suất hiến dự kiến.
- Lưu sự kiện vào BloodDonationEvents.
- Sửa, hủy, đóng đăng ký sự kiện (cập nhật trạng thái, thông tin trong BloodDonationEvents).
- Mọi thay đổi gửi thông báo cho người đã đăng ký (Notifications).

### 2.4. Quản lý tin tức và thông báo
- Soạn, đăng bài viết/tin tức về hiến máu.
- Lưu nội dung, tạo thông báo tới người dùng liên quan (Notifications).
- Chỉnh sửa/gỡ bỏ tin đã đăng, cập nhật trạng thái hiển thị.

### 2.5. Kiểm duyệt người hiến và sàng lọc
- Xem danh sách đăng ký, kết quả sàng lọc để kiểm duyệt.
- Duyệt thông tin người đăng ký hoặc kết quả xét nghiệm.
- Phê duyệt/loại bỏ kết quả, cập nhật trạng thái trong DonationRegistrations hoặc HealthScreening.

### 2.6. Thống kê và báo cáo
- Tổng hợp dữ liệu: số người đăng ký, đã hiến theo sự kiện (DonationRegistrations, DonationHistory).
- Báo cáo phân bố nhóm máu, so sánh số lượng hiến máu giữa các khoảng thời gian.
- Xuất báo cáo hoặc xem biểu đồ trực tiếp trên giao diện.

### 2.7. Cấu hình hệ thống
- Cấu hình các tham số hệ thống: quy tắc khoảng cách hiến lại, hạn mức sự kiện, cấu hình email/SMS, ...
- Giá trị lưu trong bảng Settings, tự động áp dụng vào các chức năng liên quan.

---

## 3. Bệnh viện (Đơn vị tổ chức hiến máu)

### 3.1. Tạo/sửa sự kiện
- Đại diện bệnh viện đăng nhập, chọn tạo sự kiện hiến máu tại cơ sở.
- Nhập thông tin (tên chiến dịch, số lượng suất hiến, trang thiết bị, cán bộ tham gia, ...).
- Lưu sự kiện vào BloodDonationEvents, liên kết với mã bệnh viện.
- Có thể sửa đổi thông tin hoặc hủy sự kiện.

### 3.2. Phân công nhân viên y tế và bác sĩ
- Phân công bác sĩ, y tá tham gia mỗi sự kiện.
- Hệ thống quản lý nhóm nhân sự liên quan đến event.
- Chỉ nhân sự được phân công mới có quyền cập nhật thông tin sự kiện.

### 3.3. Theo dõi tiến độ sự kiện
- Xem số lượt đăng ký, check-in, hiến thành công tại từng sự kiện.
- Truy vấn DonationRegistrations, DonationHistory theo mã sự kiện để thống kê.
- Dùng thông tin để điều phối tài nguyên.

### 3.4. Nhận thông báo và báo cáo
- Nhận thông báo khi có sự kiện mới hoặc các tình huống quan trọng.
- Sau sự kiện, xem báo cáo chi tiết (số lượng máu thu được, nhóm máu, người tham gia).

### 3.5. Cập nhật thông tin cơ sở
- Cập nhật thông tin chung của bệnh viện (địa chỉ, số điện thoại).
- Thao tác này ảnh hưởng đến bảng Hospital và thông tin hiển thị cho người dùng.

---

## 4. Bác sĩ

### 4.1. Đăng nhập
- Sử dụng tài khoản chuyên dụng (vai trò “Bác sĩ”) để đăng nhập.
- Hệ thống xác nhận quyền hạn, mở trang quản lý sàng lọc sức khỏe.

### 4.2. Xem danh sách người đến sự kiện
- Vào mục “Sàng lọc sức khỏe”, chọn sự kiện.
- Truy vấn DonationRegistrations lấy danh sách người đã check-in (“Đã đến”).
- Thông tin: tên, tuổi, nhóm máu, các lần hiến trước, ...

### 4.3. Khám và tư vấn
- Khám sơ bộ cho từng người đến: khai thác bệnh lý, hỏi sức khỏe, kiểm tra huyết áp, nhịp tim, ...
- Nhập kết quả khám lâm sàng vào hệ thống (HealthScreening).

### 4.4. Xét nghiệm máu trước hiến
- Chỉ định xét nghiệm cần thiết (Hb, viêm gan B, giang mai, ...).
- Khi có kết quả, nhập vào hệ thống (HealthScreening).

### 4.5. Đánh giá điều kiện hiến
- Dựa trên khám và xét nghiệm, đánh giá đủ điều kiện hay không.
- Nếu đủ: đánh dấu “Đủ điều kiện” trong HealthScreening, cập nhật trạng thái “Đủ điều kiện” trong DonationRegistrations.
- Nếu không đủ: chọn lý do loại, đánh dấu “Không đủ điều kiện”, ghi nhận lý do, cập nhật DonationRegistrations, gửi thông báo lý do từ chối.

### 4.6. Hoàn tất sàng lọc
- Sau khi sàng lọc xong, xác nhận hoàn tất.
- Chuyển các trường hợp “Đủ điều kiện” sang bước hiến máu.
- Lưu thông tin sàng lọc cuối cùng trong HealthScreening.

---

## 5. Nhân viên y tế

### 5.1. Đăng nhập
- Dùng tài khoản chuyên biệt đăng nhập, vào khu vực quản lý sự kiện/phòng hiến máu.

### 5.2. Check-in người hiến
- Khi người hiến được bác sĩ đồng ý, nhân viên y tế check-in cuối cùng.
- Xác nhận danh tính, đánh dấu bắt đầu hiến máu trên hệ thống.
- Cập nhật DonationRegistrations (trạng thái “Đang hiến” hoặc “Đã bắt đầu hiến”), lưu thời gian check-in.

### 5.3. Thực hiện hiến máu
- Hướng dẫn người hiến vào phòng lấy máu, theo dõi quy trình.
- Sau khi lấy máu, nhập dữ liệu chi tiết lần hiến (ngày giờ, thể tích, nhóm máu) vào DonationHistory.
- Hệ thống tính và lưu “ngày đủ điều kiện lần hiến tiếp theo”.

### 5.4. Quan sát sau hiến
- Sau hiến máu, người hiến nghỉ ngơi 10–15 phút.
- Nhân viên y tế theo dõi, cung cấp ăn uống nhẹ, trao giấy chứng nhận hiến máu.
- Hệ thống có thể sinh chứng nhận này và gửi qua email hoặc in từ giao diện.

### 5.5. Kết thúc phiên hiến máu
- Đánh dấu kết thúc phiên hiến cho từng người.
- Cập nhật trạng thái trong DonationRegistrations thành “Hoàn tất hiến máu”, ghi nhận vào DonationHistory.
- Gửi thông báo xác nhận hiến thành công tới người dùng.
- Dữ liệu sau hiến dùng cho thống kê và lịch sử hiến của người dùng.
