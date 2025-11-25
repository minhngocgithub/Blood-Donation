# Tài liệu Giao Diện Dịch Vụ (Service Interfaces)

Thư mục này chứa tất cả các giao diện dịch vụ cho ứng dụng Website Hiến Máu. Các giao diện này định nghĩa hợp đồng cho các thao tác nghiệp vụ và cung cấp sự tách biệt rõ ràng giữa tầng trình bày và tầng nghiệp vụ.

## Giao Diện Dịch Vụ Cốt Lõi

### 1. **AccountService.cs**:
Quản lý tài khoản, bao gồm:
- **Register/Login/LogoutAsync**
- **Forget/Reset/ChangePasswordAsync**
- **HashPasswordAsync**
- **VerifyPasswordAsync**
- **GetUserByEmailAsync**
- **GetUserByIdAsync**
- **GetAllUsersAsync**
- **DeleteUserAsync**
- **IsEmailExistsAsync**
- **VerifyEmailAsync**
- **IsUserInRoleAsync**
- **GetUserProfileAsync**
- **UpdateUserProfileAsync**
- **LockUserAsync**
- **UnlockUserAsync**
- **UpdateLastLoginAsync**

### 2. **BloodDonationEventService.cs**:
- **GetEventByIdAsync**
- **GetEventByNameAsync**
- **GetAllEventsAsync**
- **GetEventsPagedAsync**
- **SearchEventsByNameDescLocationAsync**
- **CreateEventAsync**
- **UpdateEventAsync**
- **DeleteEventAsync**
- **ActivateEventAsync**
- **DeactivateEventAsync**
- **CancelEventAsync**
- **CompleteEventAsync**
- **GetEventStatusAsync**
- **UpdateEventCapacityAsync**
- **GetEventAvailableSlotsAsync**
- **IsEventFullAsync**
- **IncrementCurrentDonorsAsync**
- **DecrementCurrentDonorsAsync**
- **GetUpcomingEventsAsync**
- **GetPastEventsAsync**
- **GetEventsByDateRangeAsync**
- **GetEventsByLocationAsync**
- **GetEventsByCreatorAsync**
- **SearchEventsAsync**
- **GetEventsByStatusAsync**
- **GetEventsByBloodTypeAsync**
- **GetEventStatisticsAsync**
- **GetAllEventStatisticsAsync**
- **GetEventRegistrationCountAsync**
- **GetEventDonationCountAsync**
- **IsEventExistsAsync**
- **IsEventNameExistsAsync**
- **IsEventDateValidAsync**
- **SendEventRemindersAsync**
- **SendEventUpdatesAsync**
- **UpdateAllPastEventsStatusAsync**
- **GetEventsNeedingStatusUpdateAsync**
- **AutoUpdateEventStatusAsync**

### 3. **BloodTypeService.cs**:
- **GetBloodTypeByNameAsync**
- **GetAllBloodTypesAsync**
- **GetBloodTypeStatisticsAsync**
- **GetAllBloodTypeStatisticsAsync**
- **GetTotalDonationsByBloodTypeAsync**
- **GetTotalVolumeByBloodTypeAsync**
- **GetUserCountByBloodTypeAsync**
- **IsBloodTypeExistsAsync**: Kiểm tra bằng tên nhóm máu thay vì Id
- **SearchBloodTypesAsync**

### 4. **ContactMessageService.cs**:
- **GetMessageByIdAsync**
- **GetAllMessagesAsync**
- **CreateMessageAsync**
- **UpdateMessageAsync**
- **DeleteMessageAsync**
- **UpdateMessageStatusAsync**
- **MarkAsReadAsync**
- **MarkAsUnreadAsync**
- **GetUnreadMessagesAsync**
- **GetMessagesByStatusAsync**
- **GetMessagesByCategoryAsync**
- **GetMessagesByPriorityAsync**
- **ReplyToMessageAsync**
- **GetMessageStatisticsAsync**
- **SearchMessagesAsync**


### 5. **DonationHistoryService**:
- **GetDonationByIdAsync**
- **GetAllDonationsAsync**
- **GetDonationsPagedAsync**
- **CreateDonationAsync**
- **UpdateDonationAsync**
- **DeleteDonationAsync**
- **CompleteDonationAsync**
- **CancelDonationAsync**
- **IssueCertificateAsync**
- **GetDonationStatusAsync**
- **GetDonationsByUserAsync**
- **GetDonationsByEventAsync**
- **GetDonationsByBloodTypeAsync**
- **GetDonationsByStatusAsync**
- **GetDonationsByDateRangeAsync**
- **GetDonationsByRegistrationAsync**
- **GetDonationsByDisqualificationReasonAsync**
- **GetTotalDonationsAsync**
- **GetTotalDonationsByUserAsync**
- **GetTotalDonationsByEventAsync**
- **GetTotalDonationsByBloodTypeAsync**
- **GetTotalVolumeAsync**
- **GetTotalVolumeByUserAsync**
- **GetTotalVolumeByEventAsync**
- **GetTotalVolumeByBloodTypeAsync**
- **GetUserNextEligibleDateAsync**
- **IsUserEligibleForDonationAsync**
- **CanUserDonateAsync**
- **SearchDonationsAsync**
- **GetCompletedDonationsAsync**
- **GetCancelledDonationsAsync**
- **GetDonationsWithCertificatesAsync**
- **IsDonationExistsAsync**
- **IsDonationDateValidAsync**
- **GetDonationsByMonthAsync**
- **GetDonationsByYearAsync**
- **GetDonationsByBloodTypeChartAsync**
- **GetDonationsByMonthChartAsync**
- **GenerateDonationCertificateAsync**
- **SendDonationCertificateAsync**
- **GetCertificateCountAsync**
- **SendDonationConfirmationAsync**
- **SendDonationReminderAsync**
- **SendEligibilityNotificationAsync**

### 6. **DonationRegistrationService.cs**:
- **SearchRegistrationsForCheckinAsync**
- **CheckinRegistrationAsync**
- **GetRegistrationByIdAsync**
- **GetAllRegistrationsAsync**
- **GetRegistrationsPagedAsync**
- **CreateRegistrationAsync**
- **UpdateRegistrationAsync**
- **DeleteRegistrationAsync**
- **ApproveRegistrationAsync**
- **ConfirmRegistrationAsync**
- **RejectRegistrationAsync**
- **CancelRegistrationAsync**
- **CompleteRegistrationAsync**
- **StartDonatingAsync**
- **MarkAsFailedAsync**
- **MarkAsNoShowAsync**
- **GetRegistrationStatusAsync**
- **GetRegistrationsByUserAsync**
- **GetRegistrationsByEventAsync**
- **GetRegistrationsByStatusAsync**
- **GetRegistrationsByDateRangeAsync**
- **GetUserRegistrationForEventAsync**
- **IsRegistrationExistsAsync**
- **IsUserRegisteredForEventAsync**
- **IsUserEligibleForEventAsync**
- **IsEventFullAsync**
- **IsRegistrationDateValidAsync**
- **GetRegistrationCountByEventAsync**
- **GetRegistrationCountByUserAsync**
- **GetRegistrationCountByStatusAsync**
- **GetRegistrationCountByUserAndStatusAsync**
- **GetRegistrationCountByDateRangeAsync**
- **SearchRegistrationsAsync**
- **GetRegistrationsByBloodTypeAsync**
- **GetPendingRegistrationsAsync**
- **GetApprovedRegistrationsAsync**
- **SendRegistrationConfirmationAsync**
- **SendRegistrationReminderAsync**
- **SendRegistrationStatusUpdateAsync**
- **HasHealthScreeningAsync**
- **IsHealthScreeningPassedAsync**
- **IncrementEventCurrentDonorsAsync**
- **DecrementEventCurrentDonorsAsync**
- **CancelCheckinAsync**
- **RegisterUserForEventAsync**
- **GetPendingHealthScreeningsAsync**
- **CancelAllActiveRegistrationsExceptAsync**
- **GetRegistrationCountAsync**

### 7. **EmailService.cs**:
- **SendEmailAsync**
- **SendWelcomeEmailAsync**
- **SendPasswordResetEmailAsync**
- **SendEmailVerificationAsync**
- **SendEventReminderEmailAsync**
- **SendDonationConfirmationEmailAsync**

### 8. **HealthScreeningService.cs**:
- **GetScreeningByIdAsync**
- **GetAllScreeningsAsync**
- **CreateScreeningAsync**
- **UpdateScreeningAsync**
- **DeleteScreeningAsync**
- **UpdateScreeningStatusAsync**
- **CheckInScreeningAsync**
- **GetScreeningsByStatusAsync**
- **GetPendingScreeningsAsync**
- **GetScreeningsByUserAsync**
- **GetScreeningsByEventAsync**
- **GetLatestScreeningByRegistrationIdAsync**
- **GetScreeningStatisticsAsync**
- **IsEligibleForDonationAsync**
- **UpdateRegistrationStatusAfterScreeningAsync**

### 9. **LocationService.cs**:
- **GetLocationByIdAsync**
- **GetLocationByNameAsync**
- **GetAllLocationsAsync**
- **GetActiveLocationsAsync**
- **CreateLocationAsync**
- **UpdateLocationAsync**
- **DeleteLocationAsync**
- **ActivateLocationAsync**
- **DeactivateLocationAsync**
- **IsLocationActiveAsync**
- **UpdateLocationCapacityAsync**
- **GetLocationCapacityAsync**
- **GetAvailableCapacityAsync**
- **GetLocationEventsAsync**
- **GetLocationUpcomingEventsAsync**
- **GetLocationEventCountAsync**
- **SearchLocationsAsync**
- **GetLocationsByCapacityAsync**
- **GetLocationsByAddressAsync**
- **IsLocationExistsAsync**
- **IsLocationNameExistsAsync**
- **GetTotalEventsAtLocationAsync**
- **GetTotalDonationsAtLocationAsync**

### 10. **ProfileService.cs**:
- **GetBloodTypesAsync**
- **UpdateProfileAsync**

### 11. **RoleService.cs**:
- **GetRoleByIdAsync**
- **GetRoleByNameAsync**
- **GetAllRolesAsync**
- **CreateRoleAsync**
- **UpdateRoleAsync**
- **DeleteRoleAsync**
- **AssignRoleToUserAsync**
- **RemoveRoleFromUserAsync**
- **IsUserInRoleAsync**
- **IsUserInRoleByNameAsync**
- **GetUsersByRoleAsync**
- **GetUsersByRoleNameAsync**
- **IsRoleExistsAsync**
- **IsRoleNameExistsAsync**
- **SearchRolesAsync**
- **GetUserCountByRoleAsync**
- **GetUserCountByRoleNameAsync**

### 12. **UserService.cs**:
- **GetUserByIdAsync**
- **GetUserByEmailAsync**
- **GetUserByUsernameAsync**
- **GetAllUsersAsync**
- **GetUsersPagedAsync**
- **CreateUserAsync**
- **UpdateUserAsync**
- **DeleteUserAsync**
- **ActivateUserAsync**
- **DeactivateUserAsync**
- **LockUserAsync**
- **UnlockUserAsync**
- **VerifyEmailAsync**
- **AssignRoleAsync**
- **RemoveRoleAsync**
- **IsUserInRoleAsync**
- **GetUserRolesAsync**
- **UpdateBloodTypeAsync**
- **GetUserBloodTypeAsync**
- **GetUserDonationHistoryAsync**
- **GetUserTotalDonationsAsync**
- **GetUserLastDonationDateAsync**
- **GetUserNextEligibleDateAsync**
- **GetUsersByBloodTypeAsync**
- **GetUsersByRoleAsync**
- **GetActiveUsersAsync**
- **GetUsersByGenderAsync**
- **IsEmailExistsAsync**
- **IsUsernameExistsAsync**
- **IsUserEligibleForDonationAsync**
- **HashPasswordAsync**


### 1. **IUserService.cs**
Quản lý người dùng toàn diện bao gồm:
- **CRUD**: Tạo, đọc, cập nhật, xoá người dùng
- **Quản lý trạng thái**: Kích hoạt, vô hiệu hoá, khoá, mở khoá
- **Quản lý vai trò**: Gán / huỷ vai trò, kiểm tra vai trò
- **Nhóm máu**: Cập nhật và lấy thông tin nhóm máu
- **Thống kê**: Lịch sử hiến máu, theo dõi điều kiện đủ
- **Tìm kiếm / Lọc nâng cao**
- **Xác thực**: Kiểm tra email / username, điều kiện hiến máu