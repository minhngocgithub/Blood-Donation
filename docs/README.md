# Tài liệu dự án Blood Donation

Đây là chỉ mục cho tất cả tài liệu của dự án Blood Donation Website. Sử dụng hướng dẫn này để tìm thông tin chi tiết về các thành phần khác nhau của hệ thống.

## Tài liệu kiến trúc và quy trình

1. **[PROJECT_ARCHITECTURE.md](PROJECT_ARCHITECTURE.md)** - Mô tả chi tiết kiến trúc dự án và vai trò của từng thành phần
   - Giải thích về các lớp: Presentation, Service, Data, Model
   - Mô tả về Controllers, Services, Entities, DTOs, và cách chúng tương tác
   - Giải thích về các design patterns được sử dụng

2. **[APPLICATION_WORKFLOW.md](APPLICATION_WORKFLOW.md)** - Mô tả các luồng làm việc chính của ứng dụng
   - Quy trình đăng ký người dùng và xác thực
   - Chu trình sự kiện hiến máu từ tạo đến hoàn thành
   - Luồng quản trị và báo cáo

3. **[DESCRIPTION.md](DESCRIPTION.md)** - Quy trình nghiệp vụ hệ thống hiến máu
   - Mô tả chi tiết các quy trình nghiệp vụ cho người dùng
   - Mô tả chi tiết các quy trình nghiệp vụ cho quản trị viên
   - Trình bày các luồng xử lý từ đăng ký đến hiến máu

## Tài liệu kỹ thuật

4. **[DATABASE.md](DATABASE.md)** - Database schema and structure (Giữ nguyên tiếng Anh)
   - Tables definitions
   - Relationships
   - Entity diagrams

5. **[DATA_INSERT_ORDER.md](DATA_INSERT_ORDER.md)** - Thứ tự chèn dữ liệu vào cơ sở dữ liệu
   - Hướng dẫn về thứ tự tạo dữ liệu để tránh lỗi khóa ngoại
   - Diễn giải các phụ thuộc giữa các bảng

6. **[SETUP.md](SETUP.md)** - Hướng dẫn cài đặt và cấu hình dự án
   - Cài đặt môi trường phát triển
   - Cấu hình kết nối cơ sở dữ liệu
   - Khởi chạy ứng dụng

## Tài liệu thành phần

7. **[MODEL_DTOS.md](MODEL_DTOS.md)** - Tài liệu về các DTOs (Data Transfer Objects)
   - Mô tả các loại DTOs: Entity DTOs, Specialized DTOs
   - Giải thích cấu trúc và mục đích của từng loại DTO
   - Hướng dẫn cách sử dụng DTOs trong dự án

8. **[SERVICES_INTERFACES.md](SERVICES_INTERFACES.md)** - Tài liệu về các Service Interfaces
   - Mô tả các giao diện dịch vụ chính: User, Event, Content, v.v.
   - Giải thích các phương thức và chức năng của từng dịch vụ
   - Hướng dẫn triển khai các dịch vụ

## Tài liệu giao diện người dùng

9. **[SWEETALERT.md](SWEETALERT.md)** - Hướng dẫn sử dụng SweetAlert
    - Tích hợp và cấu hình
    - Các loại thông báo và cách sử dụng
    - Tùy chỉnh giao diện thông báo

## Tài liệu tham khảo khác

- **README.md** - Tài liệu chính của dự án ở thư mục gốc

## Đóng góp và cập nhật tài liệu

Khi thêm tính năng mới hoặc thay đổi đáng kể, vui lòng cập nhật tài liệu liên quan. Nếu cần tạo tài liệu mới, hãy tuân thủ các quy ước sau:

1. Sử dụng định dạng Markdown (.md)
2. Đặt tên file rõ ràng, thống nhất với quy ước hiện có
3. Thêm liên kết đến tài liệu mới trong file chỉ mục này
4. Tổ chức nội dung với các đề mục rõ ràng

## Liên hệ hỗ trợ

Nếu có thắc mắc về tài liệu hoặc cần hỗ trợ, vui lòng liên hệ team phát triển qua email hoặc tạo issue trên GitHub repository.
