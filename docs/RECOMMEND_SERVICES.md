# Services

Danh sách các dịch vụ trong hệ thống quản lý hiến máu, mỗi dịch vụ đảm nhiệm các chức năng cụ thể.

## Dịch vụ xác thực

### AccountService.cs
**Mô tả:** Quản lý đăng ký, đăng nhập và đăng xuất người dùng.  
**Phương thức:**  
- `RegisterAsync`  
- `LoginAsync`  
- `LogoutAsync`

### PasswordService.cs
**Mô tả:** Quản lý các thao tác liên quan đến mật khẩu như quên mật khẩu, đặt lại và thay đổi mật khẩu.  
**Phương thức:**  
- `ForgotPasswordAsync`  
- `ResetPasswordAsync`  
- `ChangePasswordAsync`  
- `HashPasswordAsync`

## Dịch vụ người dùng

### UserManagementService.cs
**Mô tả:** Quản lý các thao tác cốt lõi của người dùng như tạo, lấy, cập nhật và xóa tài khoản.  
**Phương thức:**  
- `GetUserByIdAsync`  
- `GetAllUsersAsync`  
- `CreateUserAsync`  
- `UpdateUserAsync`  
- `DeleteUserAsync`  
- `ActivateUserAsync`  
- `DeactivateUserAsync`  
- `LockUserAsync`  
- `UnlockUserAsync`  
- `VerifyEmailAsync`  
- `IsEmailExistsAsync`  
- `IsUsernameExistsAsync`

### UserBloodTypeService.cs
**Mô tả:** Quản lý cập nhật và lấy thông tin nhóm máu của người dùng.  
**Phương thức:**  
- `UpdateBloodTypeAsync`  
- `GetUserBloodTypeAsync`  
- `GetUsersByBloodTypeAsync`

### UserDonationEligibilityService.cs
**Mô tả:** Kiểm tra tính đủ điều kiện hiến máu và lấy ngày liên quan.  
**Phương thức:**  
- `IsUserEligibleForDonationAsync`  
- `GetUserNextEligibleDateAsync`  
- `GetUserLastDonationDateAsync`

### UserDonationHistoryService.cs
**Mô tả:** Lấy lịch sử hiến máu và tổng số lần hiến của người dùng.  
**Phương thức:**  
- `GetUserDonationHistoryAsync`  
- `GetUserTotalDonationsAsync`

## Dịch vụ vai trò

### RoleManagementService.cs
**Mô tả:** Quản lý tạo, cập nhật và xóa vai trò.  
**Phương thức:**  
- `CreateRoleAsync`  
- `UpdateRoleAsync`  
- `DeleteRoleAsync`  
- `GetRoleByIdAsync`  
- `GetAllRolesAsync`  
- `GetRoleByNameAsync`

### RoleAssignmentService.cs
**Mô tả:** Quản lý gán và xóa vai trò cho người dùng.  
**Phương thức:**  
- `AssignRoleToUserAsync`  
- `RemoveRoleFromUserAsync`  
- `IsUserInRoleAsync`  
- `IsUserInRoleByNameAsync`  
- `GetUserRolesAsync`  
- `GetUsersByRoleAsync`  
- `GetUsersByRoleNameAsync`

## Dịch vụ hồ sơ

### ProfileService.cs
**Mô tả:** Cho phép người dùng xem và chỉnh sửa hồ sơ.  
**Phương thức:**  
- `GetProfileAsync`  
- `UpdateProfileAsync`

## Dịch vụ nhóm máu

### BloodTypeService.cs
**Mô tả:** Quản lý dữ liệu và thống kê nhóm máu.  
**Phương thức:**  
- `GetAllBloodTypesAsync`  
- `GetBloodTypeByIdAsync`  
- `CreateBloodTypeAsync`  
- `UpdateBloodTypeAsync`  
- `DeleteBloodTypeAsync`  
- `GetBloodTypeStatisticsAsync`

### BloodTypeCompatibilityService.cs
**Mô tả:** Quản lý quy tắc tương thích nhóm máu.  
**Phương thức:**  
- `GetCompatibleDonorsAsync`  
- `GetCompatibleRecipientsAsync`  
- `IsCompatibleAsync`

## Dịch vụ đăng ký hiến máu

### RegistrationManagementService.cs
**Mô tả:** Quản lý tạo, cập nhật và xóa đăng ký hiến máu.  
**Phương thức:**  
- `CreateRegistrationAsync`  
- `UpdateRegistrationAsync`  
- `DeleteRegistrationAsync`  
- `GetRegistrationByIdAsync`  
- `GetAllRegistrationsAsync`  
- `GetRegistrationsByUserAsync`  
- `GetRegistrationsByEventAsync`

### RegistrationStatusService.cs
**Mô tả:** Quản lý thay đổi trạng thái đăng ký như phê duyệt, xác nhận và hủy.  
**Phương thức:**  
- `ApproveRegistrationAsync`  
- `ConfirmRegistrationAsync`  
- `RejectRegistrationAsync`  
- `CancelRegistrationAsync`  
- `CompleteRegistrationAsync`  
- `MarkAsFailedAsync`  
- `MarkAsNoShowAsync`  
- `GetRegistrationStatusAsync`

### RegistrationCheckinService.cs
**Mô tả:** Quản lý quy trình check-in và check-out cho đăng ký.  
**Phương thức:**  
- `CheckinRegistrationAsync`  
- `CancelCheckinAsync`  
- `IncrementEventCurrentDonorsAsync`  
- `DecrementEventCurrentDonorsAsync`

### RegistrationEligibilityService.cs
**Mô tả:** Kiểm tra tính đủ điều kiện và xác thực điều kiện đăng ký.  
**Phương thức:**  
- `IsUserEligibleForEventAsync`  
- `IsEventFullAsync`  
- `IsRegistrationDateValidAsync`  
- `IsUserRegisteredForEventAsync`

### RegistrationStatisticsService.cs
**Mô tả:** Cung cấp thống kê và số liệu liên quan đến đăng ký.  
**Phương thức:**  
- `GetRegistrationCountByEventAsync`  
- `GetRegistrationCountByUserAsync`  
- `GetRegistrationCountByStatusAsync`  
- `GetRegistrationCountByDateRangeAsync`
- `GetRegistrationCountAsync`

## Dịch vụ lịch sử hiến máu

### DonationManagementService.cs
**Mô tả:** Quản lý hồ sơ hiến máu, bao gồm tạo, cập nhật và xóa.  
**Phương thức:**  
- `GetDonationByIdAsync`  
- `GetAllDonationsAsync`  
- `CreateDonationAsync`  
- `UpdateDonationAsync`  
- `DeleteDonationAsync`  
- `CompleteDonationAsync`  
- `CancelDonationAsync`

### DonationCertificateService.cs
**Mô tả:** Quản lý phát hành và quản lý chứng nhận hiến máu.  
**Phương thức:**  
- `IssueCertificateAsync`  
- `GenerateDonationCertificateAsync`  
- `SendDonationCertificateAsync`  
- `GetDonationsWithCertificatesAsync`  
- `GetCertificateCountAsync`

### DonationStatisticsService.cs
**Mô tả:** Cung cấp thống kê và biểu đồ liên quan đến hiến máu.  
**Phương thức:**  
- `GetTotalDonationsAsync`  
- `GetTotalVolumeAsync`  
- `GetDonationsByBloodTypeChartAsync`  
- `GetDonationsByMonthChartAsync`  
- `GetTotalDonationsByUserAsync`  
- `GetTotalVolumeByBloodTypeAsync`

### DonationEligibilityService.cs
**Mô tả:** Kiểm tra tính đủ điều kiện hiến máu của người dùng.  
**Phương thức:**  
- `IsUserEligibleForDonationAsync`  
- `CanUserDonateAsync`  
- `GetUserNextEligibleDateAsync`

## Dịch vụ kiểm tra sức khỏe

### HealthScreeningService.cs
**Mô tả:** Quản lý quy trình kiểm tra sức khỏe cho hiến máu.  
**Phương thức:**  
- `CreateHealthScreeningAsync`  
- `UpdateHealthScreeningAsync`  
- `GetHealthScreeningByIdAsync`  
- `GetHealthScreeningsByUserAsync`  
- `IsEligibleForDonationAsync`  
- `UpdateRegistrationStatusAfterScreeningAsync`

## Dịch vụ địa điểm

### LocationManagementService.cs
**Mô tả:** Quản lý dữ liệu địa điểm cho các sự kiện hiến máu.  
**Phương thức:**  
- `CreateLocationAsync`  
- `UpdateLocationAsync`  
- `DeleteLocationAsync`  
- `GetLocationByIdAsync`  
- `GetAllLocationsAsync`

### LocationCapacityService.cs
**Mô tả:** Quản lý sức chứa của địa điểm.  
**Phương thức:**  
- `UpdateLocationCapacityAsync`  
- `GetLocationCapacityAsync`  
- `GetAvailableCapacityAsync`

### LocationEventService.cs
**Mô tả:** Cung cấp thông tin sự kiện liên quan đến địa điểm.  
**Phương thức:**  
- `GetLocationEventsAsync`  
- `GetLocationUpcomingEventsAsync`  
- `GetLocationEventCountAsync`  
- `GetTotalEventsAtLocationAsync`  
- `GetTotalDonationsAtLocationAsync`

## Dịch vụ tin nhắn liên hệ

### ContactMessageService.cs
**Mô tả:** Quản lý tin nhắn liên hệ và trạng thái của chúng.  
**Phương thức:**  
- `CreateContactMessageAsync`  
- `UpdateContactMessageStatusAsync`  
- `GetContactMessageByIdAsync`  
- `GetAllContactMessagesAsync`

## Dịch vụ danh mục tin tức

### NewsCategoryManagementService.cs
**Mô tả:** Quản lý danh mục tin tức.  
**Phương thức:**  
- `CreateCategoryAsync`  
- `UpdateCategoryAsync`  
- `DeleteCategoryAsync`  
- `GetCategoryByIdAsync`  
- `GetAllCategoriesAsync`

### NewsCategoryHierarchyService.cs
**Mô tả:** Quản lý mối quan hệ phân cấp giữa các danh mục tin tức.  
**Phương thức:**  
- `SetParentCategoryAsync`  
- `GetParentCategoryAsync`  
- `GetChildCategoriesAsync`  
- `GetCategoryTreeAsync`

## Dịch vụ tin tức

### NewsManagementService.cs
**Mô tả:** Quản lý bài viết tin tức.  
**Phương thức:**  
- `GetNewsByIdAsync`  
- `GetAllNewsAsync`  
- `CreateNewsAsync`  
- `UpdateNewsAsync`  
- `DeleteNewsAsync`

### NewsPublishingService.cs
**Mô tả:** Quản lý xuất bản và lên lịch bài viết tin tức.  
**Phương thức:**  
- `PublishNewsAsync`  
- `UnpublishNewsAsync`  
- `IsNewsPublishedAsync`  
- `GetPublishedDateAsync`  
- `SchedulePublishAsync`

### NewsCategoryAssignmentService.cs
**Mô tả:** Gán danh mục cho bài viết tin tức.  
**Phương thức:**  
- `GetNewsCategoriesAsync`  
- `AssignCategoryToNewsAsync`  
- `RemoveCategoryFromNewsAsync`

### NewsStatisticsService.cs
**Mô tả:** Theo dõi số lượt xem và thống kê bài viết tin tức.  
**Phương thức:**  
- `GetNewsViewCountAsync`  
- `IncrementViewCountAsync`  
- `ResetViewCountAsync`  
- `GetTotalViewsAsync`

### NewsReviewService.cs
**Mô tả:** Quản lý quy trình xem xét và phê duyệt bài viết tin tức.  
**Phương thức:**  
- `SubmitForReviewAsync`  
- `ApproveNewsAsync`  
- `RejectNewsAsync`

## Dịch vụ email

### EmailService.cs
**Mô tả:** Gửi các thông báo qua email.  
**Phương thức:**  
- `SendEmailAsync`  
- `SendRegistrationConfirmationEmailAsync`  
- `SendDonationCertificateEmailAsync`  
- `SendPasswordResetEmailAsync`  
- `SendNewsNotificationEmailAsync`

## Dịch vụ thông báo

### NotificationService.cs
**Mô tả:** Gửi thông báo hệ thống và người dùng.  
**Phương thức:**  
- `SendNotificationAsync`  
- `SendSystemNotificationAsync`  
- `SendUserNotificationAsync`

## Dịch vụ thống kê

### DonationStatisticsService.cs
**Mô tả:** Cung cấp thống kê liên quan đến hiến máu.  
**Phương thức:**  
- `GetTotalDonationsAsync`  
- `GetDonationTrendChartAsync`  
- `GetDonationVolumeChartAsync`

### RegistrationStatisticsService.cs
**Mô tả:** Cung cấp thống kê liên quan đến đăng ký.  
**Phương thức:**  
- `GetTotalRegistrationsAsync`  
- `GetRegistrationTrendChartAsync`  
- `GetRegistrationStatusChartAsync`

### EventStatisticsService.cs
**Mô tả:** Cung cấp thống kê liên quan đến sự kiện.  
**Phương thức:**  
- `GetEventStatisticsAsync`  
- `GetEventPerformanceChartAsync`  
- `GetEventRegistrationChartAsync`

### UserStatisticsService.cs
**Mô tả:** Cung cấp thống kê liên quan đến người dùng.  
**Phương thức:**  
- `GetTopDonorsAsync`  
- `GetRecentDonorsAsync`  
- `GetUserRegistrationChartAsync`  
- `GetUserDonationChartAsync`

### BloodTypeStatisticsService.cs
**Mô tả:** Cung cấp thống kê liên quan đến nhóm máu.  
**Phương thức:**  
- `GetAllBloodTypeStatisticsAsync`  
- `GetBloodTypeDonationChartAsync`  
- `GetBloodTypeUserChartAsync`

### GeneralStatisticsService.cs
**Mô tả:** Cung cấp thống kê tổng quan và bảng điều khiển.  
**Phương thức:**  
- `GetDashboardStatisticsAsync`  
- `GetRealTimeStatisticsAsync`  
- `GetTodayStatisticsAsync`

## Dịch vụ xuất dữ liệu

### ExportService.cs
**Mô tả:** Xuất dữ liệu dưới các định dạng khác nhau.  
**Phương thức:**  
- `ExportToExcelAsync`  
- `ExportToPdfAsync`  
- `ExportToCsvAsync`

## Dịch vụ tệp

### FileService.cs
**Mô tả:** Quản lý tải lên và tải xuống tệp.  
**Phương thức:**  
- `UploadFileAsync`  
- `DownloadFileAsync`  
- `DeleteFileAsync`

## Dịch vụ kiểm tra

### AuditService.cs
**Mô tả:** Ghi lại hành động của người dùng và hệ thống.  
**Phương thức:**  
- `LogActionAsync`  
- `GetAuditLogsAsync`

## Dịch vụ xác thực

### DonationValidationService.cs
**Mô tả:** Xác thực logic nghiệp vụ liên quan đến hiến máu.  
**Phương thức:**  
- `ValidateDonationEligibilityAsync`

### RegistrationValidationService.cs
**Mô tả:** Xác thực logic nghiệp vụ liên quan đến đăng ký.  
**Phương thức:**  
- `ValidateRegistrationAsync`

### EventValidationService.cs
**Mô tả:** Xác thực logic nghiệp vụ liên quan đến sự kiện.  
**Phương thức:**  
- `ValidateEventAsync`

## Dịch vụ ánh xạ

### MappingService.cs
**Mô tả:** Xử lý chuyển đổi dữ liệu giữa các thực thể, DTO và mô hình xem.  
**Phương thức:**  
- `MapToDtoAsync`  
- `MapToEntityAsync`

## Dịch vụ bộ nhớ đệm

### CacheService.cs
**Mô tả:** Quản lý các thao tác bộ nhớ đệm.  
**Phương thức:**  
- `GetCachedDataAsync`  
- `SetCachedDataAsync`  
- `InvalidateCacheAsync`

## Dịch vụ lập lịch

### SchedulerService.cs
**Mô tả:** Quản lý các tác vụ được lập lịch.  
**Phương thức:**  
- `ScheduleTaskAsync`  
- `CancelScheduledTaskAsync`

## Dịch vụ tìm kiếm

### UserSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho người dùng.  
**Phương thức:**  
- `SearchUsersAsync`  
- `FilterUsersAsync`  
- `PaginateUsersAsync`

### EventSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho sự kiện.  
**Phương thức:**  
- `SearchEventsAsync`  
- `FilterEventsAsync`  
- `PaginateEventsAsync`

### DonationSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho hiến máu.  
**Phương thức:**  
- `SearchDonationsAsync`  
- `FilterDonationsAsync`  
- `PaginateDonationsAsync`

### RegistrationSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho đăng ký.  
**Phương thức:**  
- `SearchRegistrationsAsync`  
- `FilterRegistrationsAsync`  
- `PaginateRegistrationsAsync`

### NewsSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho tin tức.  
**Phương thức:**  
- `SearchNewsAsync`  
- `FilterNewsAsync`  
- `PaginateNewsAsync`

### LocationSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho địa điểm.  
**Phương thức:**  
- `SearchLocationsAsync`  
- `FilterLocationsAsync`  
- `PaginateLocationsAsync`

### ContactMessageSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho tin nhắn liên hệ.  
**Phương thức:**  
- `SearchContactMessagesAsync`  
- `FilterContactMessagesAsync`  
- `PaginateContactMessagesAsync`

### HealthScreeningSearchService.cs
**Mô tả:** Xử lý tìm kiếm, lọc và phân trang cho kiểm tra sức khỏe.  
**Phương thức:**  
- `SearchHealthScreeningsAsync`  
- `FilterHealthScreeningsAsync`  
- `PaginateHealthScreeningsAsync`

## Dịch vụ bổ sung

### GlobalSearchService.cs
**Mô tả:** Cung cấp chức năng tìm kiếm đa miền.  
**Phương thức:**  
- `SearchAllAsync`

### EventManagementService.cs
**Mô tả:** Quản lý các sự kiện hiến máu.  
**Phương thức:**  
- `CreateEventAsync`  
- `UpdateEventAsync`  
- `DeleteEventAsync`  
- `GetEventByIdAsync`  
- `GetAllEventsAsync`  
- `GetUpcomingEventsAsync`

### CertificateService.cs
**Mô tả:** Quản lý tạo và quản lý chứng nhận hiến máu.  
**Phương thức:**  
- `GenerateCertificateAsync`  
- `SendCertificateAsync`  
- `GetCertificateByIdAsync`  
- `GetCertificatesByUserAsync`

### UserNotificationPreferenceService.cs
**Mô tả:** Quản lý tùy chọn thông báo của người dùng.  
**Phương thức:**  
- `GetPreferencesAsync`  
- `UpdatePreferencesAsync`  
- `IsNotificationTypeEnabledAsync`

### EventSchedulingService.cs
**Mô tả:** Xử lý lập lịch và nhắc nhở cho sự kiện.  
**Phương thức:**  
- `ScheduleEventAsync`  
- `CancelScheduledEventAsync`  
- `SendEventRemindersAsync`  
- `GetScheduledEventsAsync`