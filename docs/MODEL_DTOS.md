# Tài liệu DTOs (Đối tượng Truyền Dữ liệu)

Thư mục này chứa tất cả các DTOs cho ứng dụng Website Hiến Máu. DTO được sử dụng để truyền dữ liệu giữa các tầng của ứng dụng, đảm bảo phân tách rõ ràng giữa tầng dữ liệu và tầng trình bày.

## DTO Thực Thể

### Thực Thể Cốt Lõi

1. **UserDto.cs**
   - `UserDto` - Thông tin người dùng đầy đủ với thuộc tính liên kết
   - `UserCreateDto` - Tạo người dùng mới
   - `UserUpdateDto` - Cập nhật người dùng hiện tại

2. **BloodTypeDto.cs**
   - `BloodTypeDto` - Thông tin nhóm máu
   - `BloodTypeCreateDto` - Tạo nhóm máu mới
   - `BloodTypeUpdateDto` - Cập nhật nhóm máu

3. **RoleDto.cs**
   - `RoleDto` - Thông tin vai trò
   - `RoleCreateDto` - Tạo vai trò mới
   - `RoleUpdateDto` - Cập nhật vai trò

4. **LocationDto.cs**
   - `LocationDto` - Thông tin địa điểm
   - `LocationCreateDto` - Tạo địa điểm mới
   - `LocationUpdateDto` - Cập nhật địa điểm

### Quản Lý Sự Kiện

5. **BloodDonationEventDto.cs**
   - `BloodDonationEventDto` - Thông tin sự kiện hiến máu đầy đủ
   - `BloodDonationEventCreateDto` - Tạo sự kiện
   - `BloodDonationEventUpdateDto` - Cập nhật sự kiện

6. **DonationRegistrationDto.cs**
   - `DonationRegistrationDto` - Thông tin đăng ký với chi tiết người dùng và sự kiện
   - `DonationRegistrationCreateDto` - Tạo đăng ký mới
   - `DonationRegistrationUpdateDto` - Cập nhật đăng ký

7. **DonationHistoryDto.cs**
   - `DonationHistoryDto` - Lịch sử hiến máu
   - `DonationHistoryCreateDto` - Tạo bản ghi mới
   - `DonationHistoryUpdateDto` - Cập nhật bản ghi

8. **HealthScreeningDto.cs**
   - `HealthScreeningDto` - Thông tin khám sàng lọc
   - `HealthScreeningCreateDto` - Tạo bản ghi khám
   - `HealthScreeningUpdateDto` - Cập nhật khám

### Quản Lý Nội Dung

9. **NewsDto.cs**
   - `NewsDto` - Thông tin bài viết tin tức
   - `NewsCreateDto` - Tạo bài viết
   - `NewsUpdateDto` - Cập nhật bài viết

10. **NewsCategoryDto.cs**
    - `NewsCategoryDto` - Thông tin danh mục tin tức
    - `NewsCategoryCreateDto` - Tạo danh mục
    - `NewsCategoryUpdateDto` - Cập nhật danh mục

### Giao Tiếp

11. **NotificationDto.cs**
    - `NotificationDto` - Thông báo
    - `NotificationCreateDto` - Tạo thông báo
    - `NotificationUpdateDto` - Cập nhật thông báo

12. **ContactMessageDto.cs**
    - `ContactMessageDto` - Tin nhắn liên hệ
    - `ContactMessageCreateDto` - Tạo tin nhắn
    - `ContactMessageUpdateDto` - Cập nhật trạng thái

### Hệ Thống

13. **BloodCompatibilityDto.cs**
    - `BloodCompatibilityDto` - Quy tắc tương thích nhóm máu
    - `BloodCompatibilityCreateDto` - Tạo quy tắc
    - `BloodCompatibilityUpdateDto` - Cập nhật quy tắc

14. **SettingDto.cs**
    - `SettingDto` - Cài đặt hệ thống
    - `SettingCreateDto` - Tạo cài đặt
    - `SettingUpdateDto` - Cập nhật cài đặt

## DTO Chuyên Biệt

### Thống Kê và Báo Cáo

15. **StatisticsDto.cs**
    - `DashboardStatisticsDto` - Thống kê tổng quan
    - `BloodTypeStatisticsDto` - Thống kê theo nhóm máu
    - `EventStatisticsDto` - Thống kê sự kiện
    - `UserDonationHistoryDto` - Tổng hợp lịch sử hiến máu người dùng

### Tìm Kiếm và Lọc

16. **SearchDto.cs**
    - `SearchParametersDto` - Tham số tìm kiếm cơ bản
    - `EventSearchDto` - Tìm kiếm sự kiện
    - `UserSearchDto` - Tìm kiếm người dùng
    - `DonationSearchDto` - Tìm kiếm hiến máu
    - `NewsSearchDto` - Tìm kiếm tin tức

### Phản Hồi API

17. **ResponseDto.cs**
    - `ApiResponseDto<T>` - Gói phản hồi API chung
    - `PagedResponseDto<T>` - Gói phản hồi có phân trang
    - `LoginResponseDto` - Phản hồi xác thực
    - `EmailResponseDto` - Phản hồi thao tác email

## Hướng Dẫn Sử Dụng

### Quy Ước Đặt Tên
- **Dto** - Đầy đủ thông tin để đọc
- **CreateDto** - Dùng để tạo mới (không bao gồm ID tự sinh)
- **UpdateDto** - Dùng để cập nhật (không bao gồm trường không thay đổi)

### Thuộc Tính Liên Kết
DTO có các thuộc tính liên kết như `UserName`, `BloodTypeName` để cung cấp dữ liệu liên quan mà không cần truy vấn thêm.

### Xác Thực
Các DTO dùng cho tạo và cập nhật nên có các thuộc tính xác thực trong controller hoặc service.

### Bảo Mật
- Không bao giờ đưa thông tin nhạy cảm như mật khẩu vào DTO
- Dùng DTO riêng cho vai trò người dùng khác nhau
- Luôn xác thực dữ liệu đầu vào

## Ví Dụ Sử Dụng

```csharp
// Trong controller
[HttpPost]
public async Task<ActionResult<ApiResponseDto<UserDto>>> CreateUser([FromBody] UserCreateDto createDto)
{
    var user = await _userService.CreateUserAsync(createDto);
    var userDto = _mapper.Map<UserDto>(user);
    
    return Ok(new ApiResponseDto<UserDto>
    {
        Success = true,
        Message = "Tạo người dùng thành công",
        Data = userDto
    });
}
```

## Mapping

Nên dùng AutoMapper để chuyển đổi giữa Entity và DTO:

```csharp
CreateMap<User, UserDto>()
    .ForMember(dest => dest.BloodTypeName, opt => opt.MapFrom(src => src.BloodType.BloodTypeName))
    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

CreateMap<UserCreateDto, User>();
CreateMap<UserUpdateDto, User>();
``` 
