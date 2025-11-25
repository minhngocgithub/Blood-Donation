# Kiến trúc dự án Blood Donation Website

Tài liệu này mô tả kiến trúc của dự án Blood Donation Website, giải thích chi tiết vai trò và trách nhiệm của từng thành phần.

## Tổng quan dự án

Blood Donation Website là một ứng dụng web toàn diện hỗ trợ quy trình hiến máu, bao gồm đăng ký người dùng, quản lý sự kiện, theo dõi hiến máu và các chức năng quản trị. Ứng dụng tuân theo mô hình kiến trúc phân lớp rõ ràng với sự phân tách rõ ràng về trách nhiệm.

## Các lớp kiến trúc

Ứng dụng được tổ chức theo kiến trúc đa lớp:

1. **Lớp trình bày (Presentation Layer)** - Controllers và Views (MVC)
2. **Lớp dịch vụ (Service Layer)** - Logic nghiệp vụ và xử lý
3. **Lớp truy cập dữ liệu (Data Access Layer)** - Entity Framework Core và tương tác cơ sở dữ liệu
4. **Lớp mô hình (Domain Model)** - Entities và DTOs

## Vai trò của các thành phần

### 1. Controllers

Nằm trong thư mục `/Controllers/`, controllers xử lý các request HTTP và responses, đóng vai trò là điểm vào cho tương tác người dùng.

* **Vai trò:** 
  * Xử lý các request HTTP đến
  * Phối hợp với services để thực thi logic nghiệp vụ
  * Trả về views hoặc responses phù hợp
  * Xử lý xác thực đầu vào từ người dùng

* **Các loại:**
  * **HomeController** - Quản lý các trang công khai
  * **AccountController** - Xử lý xác thực và các hoạt động tài khoản người dùng
  * **AdminController** - Cung cấp các chức năng quản trị
  * **ProfileController** - Quản lý các hoạt động hồ sơ người dùng

* **Trách nhiệm:**
  * Điều hướng requests đến các phương thức service phù hợp
  * Truyền dữ liệu đến views
  * Xử lý gửi form
  * Quản lý chuyển hướng và luồng người dùng

### 2. Models

Nằm trong thư mục `/Models/`, models được chia thành ba loại chính:

#### 2.1 Entities

Nằm trong `/Models/Entities/`, các lớp này đại diện cho các bảng cơ sở dữ liệu và đối tượng miền.

* **Vai trò:** 
  * Định nghĩa cấu trúc dữ liệu và mối quan hệ
  * Ánh xạ tới các bảng cơ sở dữ liệu
  * Bao gồm các data annotations cho xác thực và định nghĩa schema

* **Ví dụ:**
  * `User.cs` - Đại diện cho tài khoản người dùng
  * `BloodType.cs` - Đại diện cho các nhóm máu
  * `BloodDonationEvent.cs` - Đại diện cho các sự kiện hiến máu
  * `DonationHistory.cs` - Theo dõi lịch sử hiến máu

#### 2.2 DTOs (Data Transfer Objects)

Nằm trong `/Models/DTOs/`, các lớp này được sử dụng để truyền dữ liệu giữa các lớp.

* **Vai trò:**
  * Cung cấp sự tách biệt rõ ràng giữa lớp dữ liệu và lớp trình bày
  * Lọc dữ liệu nhạy cảm khi truyền thông tin
  * Cấu trúc dữ liệu cụ thể cho phía client
  * Xác định quy tắc xác thực cho dữ liệu đầu vào

* **Các loại:**
  * **Entity DTOs** - Ánh xạ trực tiếp tới đối tượng entity (ví dụ: `UserDto`)
  * **Create/Update DTOs** - Dùng cho các thao tác nhập dữ liệu (ví dụ: `UserCreateDto`, `UserUpdateDto`)
  * **Specialized DTOs** - Cho các thao tác cụ thể như tìm kiếm hoặc thống kê

* **Mục đích:**
  * Ngăn chặn việc lộ quá nhiều thông tin của domain entities
  * Cho phép quy tắc xác thực cụ thể cho các thao tác khác nhau
  * Tối ưu hóa việc truyền dữ liệu bằng cách chỉ bao gồm các thuộc tính cần thiết

#### 2.3 ViewModels

Nằm trong `/Models/ViewModels/`, các lớp này được thiết kế đặc biệt cho views.

* **Vai trò:**
  * Cung cấp dữ liệu được định dạng cụ thể cho việc render view
  * Kết hợp nhiều DTOs hoặc entities khi cần thiết cho một view
  * Bao gồm các thuộc tính và hành vi dành riêng cho view

* **Ví dụ:**
  * `ErrorViewModel.cs` - Cho trang lỗi
  * ViewModels cho dashboard
  * Form ViewModels kết hợp nhiều entities

### 3. Services

Nằm trong thư mục `/Services/`, services triển khai logic nghiệp vụ của ứng dụng.

#### 3.1 Service Interfaces

Nằm trong `/Services/Interfaces/`, các interfaces này định nghĩa hợp đồng cho các triển khai service.

* **Vai trò:**
  * Định nghĩa các thao tác có sẵn và chữ ký của chúng
  * Cung cấp sự trừu tượng hóa cho dependency injection
  * Thiết lập hợp đồng rõ ràng giữa controllers và các triển khai service

* **Ví dụ:**
  * `IUserService.cs` - Các thao tác quản lý người dùng
  * `IBloodDonationEventService.cs` - Các thao tác quản lý sự kiện
  * `IDonationHistoryService.cs` - Các thao tác theo dõi hiến máu

#### 3.2 Service Implementations

Nằm trong `/Services/Implementations/`, các lớp này triển khai các service interfaces.

* **Vai trò:**
  * Triển khai logic nghiệp vụ và quy tắc
  * Tương tác với data context để thực hiện các thao tác CRUD
  * Xử lý xác thực, tính toán và các thao tác phức tạp
  * Triển khai các quy tắc và quy trình làm việc cụ thể theo miền

* **Trách nhiệm:**
  * Thực thi các thao tác nghiệp vụ được định nghĩa trong interfaces
  * Thực hiện xác thực dữ liệu và thực thi quy tắc nghiệp vụ
  * Phối hợp giữa nhiều repositories khi cần thiết
  * Xử lý ngoại lệ và điều kiện lỗi

#### 3.3 Utility Services

Nằm trong `/Services/Utilities/`, các dịch vụ này cung cấp chức năng hỗ trợ.

* **Vai trò:**
  * Triển khai các vấn đề cắt ngang như email, logging hoặc xử lý file
  * Cung cấp các phương thức hỗ trợ được sử dụng bởi nhiều service implementations
  * Triển khai các thao tác kỹ thuật chuyên biệt

* **Ví dụ:**
  * Dịch vụ email
  * Xử lý file
  * Tạo PDF

### 4. Lớp truy cập dữ liệu

Nằm trong thư mục `/Data/`, lớp này xử lý tương tác cơ sở dữ liệu.

#### 4.1 DbContext

`ApplicationDbContext.cs` là Entity Framework Core DbContext.

* **Vai trò:**
  * Định nghĩa DbSets cho tất cả entities
  * Cấu hình mối quan hệ và ràng buộc entity
  * Đóng vai trò là giao diện chính đến cơ sở dữ liệu

#### 4.2 Configurations

Nằm trong `/Data/Configurations/`, các lớp này cấu hình ánh xạ entity.

* **Vai trò:**
  * Định nghĩa cấu hình entity sử dụng Fluent API
  * Thiết lập mối quan hệ giữa các entities
  * Cấu hình indexes, keys và constraints
  * Đặt giá trị mặc định và cấu hình thuộc tính

* **Ví dụ:**
  * `UserConfiguration.cs`
  * `BloodTypeConfiguration.cs`

#### 4.3 Seeders

Nằm trong `/Data/Seeders/`, các lớp này cung cấp dữ liệu ban đầu cho cơ sở dữ liệu.

* **Vai trò:**
  * Định nghĩa dữ liệu seed cho ứng dụng
  * Tạo dữ liệu tham chiếu ban đầu như các nhóm máu
  * Tạo tài khoản admin mặc định và cài đặt

* **Ví dụ:**
  * `AdminSeeder.cs`
  * `BloodTypeSeeder.cs`

### 5. Utilities

Nằm trong thư mục `/Utilities/`, các thành phần này cung cấp chức năng hỗ trợ.

#### 5.1 Extensions

Nằm trong `/Utilities/Extensions/`, các thành phần này mở rộng các lớp hiện có.

* **Vai trò:**
  * Thêm phương thức mở rộng cho các lớp có sẵn hoặc lớp miền
  * Cung cấp các phương thức hỗ trợ cho các thao tác phổ biến
  * Nâng cao chức năng của các types hiện có

#### 5.2 Filters

Nằm trong `/Utilities/Filters/`, các thành phần này triển khai các MVC filters.

* **Vai trò:**
  * Triển khai các action filters, authorization filters tùy chỉnh
  * Áp dụng các vấn đề cắt ngang cho các controller actions
  * Xử lý xác thực và phân quyền

* **Ví dụ:**
  * `AdminOnlyAttribute.cs` - Giới hạn truy cập cho admin

### 6. Views

Nằm trong thư mục `/Views/`, chứa các Razor views để render HTML.

* **Vai trò:**
  * Định nghĩa UI của ứng dụng
  * Render dữ liệu được cung cấp bởi controllers
  * Triển khai chức năng client-side và xác thực

* **Cấu trúc:**
  * Tổ chức theo controller (ví dụ: `/Views/Home/`, `/Views/Account/`)
  * Shared layouts và partial views trong `/Views/Shared/`

### 7. Static Files

Nằm trong thư mục `/wwwroot/`, bao gồm CSS, JavaScript, hình ảnh và thư viện.

* **Vai trò:**
  * Cung cấp tài nguyên tĩnh cho ứng dụng
  * Triển khai chức năng client-side
  * Xác định phong cách và tương tác

* **Cấu trúc:**
  * CSS trong `/wwwroot/css/`
  * JavaScript trong `/wwwroot/js/`
  * Hình ảnh trong `/wwwroot/image/`
  * Thư viện bên thứ ba trong `/wwwroot/lib/`

## Luồng dữ liệu

1. HTTP request đến action của controller
2. Controller xác thực input và gọi phương thức service phù hợp
3. Service triển khai logic nghiệp vụ, tương tác với DbContext khi cần
4. Dữ liệu được trả về controller thông qua DTOs
5. Controller truyền dữ liệu đến view hoặc trả về response phù hợp
6. View render HTML được trả về client

## Các mẫu thiết kế chính

1. **Repository Pattern** (ngầm định qua EF Core DbContext)
2. **Dependency Injection** - Services được đăng ký và inject khi cần
3. **DTO Pattern** - Tách biệt rõ ràng giữa các lớp
4. **Service Pattern** - Logic nghiệp vụ được đóng gói trong các service classes
5. **MVC Pattern** - Tách biệt models, views, và controllers

## Các vấn đề cắt ngang

1. **Xác thực & Phân quyền** - Quản lý thông qua ASP.NET Core Identity
2. **Xác thực** - Xác thực đầu vào qua Data Annotations và xác thực tùy chỉnh
3. **Logging** - Logging tích hợp thông qua ILogger
4. **Xử lý lỗi** - Xử lý lỗi tập trung và trang lỗi thân thiện với người dùng

## Kết luận

Blood Donation Website tuân theo kiến trúc sạch với sự phân tách rõ ràng về trách nhiệm. Mỗi thành phần có vai trò và trách nhiệm cụ thể, làm cho codebase dễ bảo trì, kiểm thử và mở rộng. Cách tiếp cận phân lớp cho phép phát triển và kiểm thử độc lập các thành phần, và việc sử dụng interfaces thúc đẩy sự kết hợp lỏng lẻo giữa các lớp.
