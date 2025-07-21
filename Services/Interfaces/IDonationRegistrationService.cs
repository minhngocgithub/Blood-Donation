using Blood_Donation_Website.Models.DTOs;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IDonationRegistrationService
    {
        // Basic CRUD operations
        Task<DonationRegistrationDto?> GetRegistrationByIdAsync(int registrationId);
        Task<IEnumerable<DonationRegistrationDto>> GetAllRegistrationsAsync();
        Task<PagedResponseDto<DonationRegistrationDto>> GetRegistrationsPagedAsync(SearchParametersDto searchDto);
        Task<DonationRegistrationDto> CreateRegistrationAsync(DonationRegistrationCreateDto createDto);
        Task<bool> UpdateRegistrationAsync(int registrationId, DonationRegistrationUpdateDto updateDto);
        Task<bool> DeleteRegistrationAsync(int registrationId);
        
        // Registration status operations
        Task<bool> ApproveRegistrationAsync(int registrationId);
        Task<bool> RejectRegistrationAsync(int registrationId, DisqualificationReason reason);
        Task<bool> CancelRegistrationAsync(int registrationId, DisqualificationReason reason);
        Task<bool> CheckInRegistrationAsync(int registrationId);
        Task<bool> CompleteRegistrationAsync(int registrationId);
        Task<string> GetRegistrationStatusAsync(int registrationId);
        
        // Registration queries
        Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByUserAsync(int userId);
        Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByEventAsync(int eventId);
        Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByStatusAsync(RegistrationStatus status);
        Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<DonationRegistrationDto?> GetUserRegistrationForEventAsync(int userId, int eventId);
        
        // Registration validation
        Task<bool> IsRegistrationExistsAsync(int registrationId);
        Task<bool> IsUserRegisteredForEventAsync(int userId, int eventId);
        Task<bool> IsUserEligibleForEventAsync(int userId, int eventId);
        Task<bool> IsEventFullAsync(int eventId);
        Task<bool> IsRegistrationDateValidAsync(int eventId);
        
        // Registration statistics
        Task<int> GetRegistrationCountByEventAsync(int eventId);
        Task<int> GetRegistrationCountByUserAsync(int userId);
        Task<int> GetRegistrationCountByStatusAsync(RegistrationStatus status);
        Task<int> GetRegistrationCountByUserAndStatusAsync(int userId, RegistrationStatus status);
        Task<int> GetRegistrationCountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetRegistrationCountAsync();
        
        // Registration search and filtering
        Task<IEnumerable<DonationRegistrationDto>> SearchRegistrationsAsync(string searchTerm);
        Task<IEnumerable<DonationRegistrationDto>> GetRegistrationsByBloodTypeAsync(int bloodTypeId);
        Task<IEnumerable<DonationRegistrationDto>> GetPendingRegistrationsAsync();
        Task<IEnumerable<DonationRegistrationDto>> GetApprovedRegistrationsAsync();
        
        // Registration notifications
        Task<bool> SendRegistrationConfirmationAsync(int registrationId);
        Task<bool> SendRegistrationReminderAsync(int registrationId);
        Task<bool> SendRegistrationStatusUpdateAsync(int registrationId);
        
        // Registration health screening
        Task<bool> HasHealthScreeningAsync(int registrationId);
        Task<bool> IsHealthScreeningPassedAsync(int registrationId);
        
        /// <summary>
        /// Tìm các đăng ký theo mã đăng ký hoặc số điện thoại (cho check-in).
        /// </summary>
        /// <param name="code">Mã đăng ký hoặc số điện thoại</param>
        /// <returns>Danh sách đăng ký phù hợp</returns>
        Task<IEnumerable<DonationRegistrationDto>> SearchRegistrationsForCheckinAsync(string code);
        
        /// <summary>
        /// Đăng ký người dùng cho sự kiện hiến máu.
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="eventId">ID sự kiện</param>
        /// <param name="notes">Ghi chú</param>
        /// <returns>Đăng ký đã tạo</returns>
        Task<DonationRegistrationDto> RegisterUserForEventAsync(int userId, int eventId, string? notes = null);

        /// <summary>
        /// Xác nhận check-in cho một đăng ký hiến máu.
        /// </summary>
        /// <param name="registrationId">Id đăng ký</param>
        /// <returns>True nếu thành công</returns>
        Task<bool> CheckinRegistrationAsync(int registrationId);
        Task<bool> CancelCheckinAsync(int registrationId);

        /// <summary>
        /// Huỷ tất cả đăng ký còn hiệu lực của user (trừ registrationId vừa hoàn thành)
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="exceptRegistrationId">ID đăng ký không huỷ (vừa hoàn thành)</param>
        /// <param name="reason">Lý do huỷ</param>
        /// <returns>Số lượng đăng ký bị huỷ</returns>
        Task<int> CancelAllActiveRegistrationsExceptAsync(int userId, int exceptRegistrationId, DisqualificationReason reason);
    }
} 