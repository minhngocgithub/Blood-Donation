using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IBloodDonationEventService
    {
        /// <summary>
        /// Lấy thông tin sự kiện hiến máu theo ID.
        /// </summary>
        Task<BloodDonationEventDto?> GetEventByIdAsync(int eventId);
        /// <summary>
        /// Lấy thông tin sự kiện hiến máu theo tên.
        /// </summary>
        Task<BloodDonationEventDto?> GetEventByNameAsync(string eventName);
        /// <summary>
        /// Lấy danh sách tất cả sự kiện hiến máu.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetAllEventsAsync();
        /// <summary>
        /// Lấy danh sách sự kiện hiến máu có phân trang theo điều kiện tìm kiếm.
        /// </summary>
        Task<PagedResponseDto<BloodDonationEventDto>> GetEventsPagedAsync(EventSearchDto searchDto);
        /// <summary>
        /// Tạo mới một sự kiện hiến máu.
        /// </summary>
        Task<BloodDonationEventDto> CreateEventAsync(BloodDonationEventCreateDto createDto);
        /// <summary>
        /// Cập nhật thông tin sự kiện hiến máu.
        /// </summary>
        Task<bool> UpdateEventAsync(int eventId, BloodDonationEventUpdateDto updateDto);
        /// <summary>
        /// Xóa sự kiện hiến máu theo ID.
        /// </summary>
        Task<bool> DeleteEventAsync(int eventId);
        /// <summary>
        /// Kích hoạt sự kiện hiến máu.
        /// </summary>
        Task<bool> ActivateEventAsync(int eventId);
        /// <summary>
        /// Hủy kích hoạt sự kiện hiến máu.
        /// </summary>
        Task<bool> DeactivateEventAsync(int eventId);
        /// <summary>
        /// Hủy sự kiện hiến máu.
        /// </summary>
        Task<bool> CancelEventAsync(int eventId);
        /// <summary>
        /// Đánh dấu sự kiện hiến máu đã hoàn thành.
        /// </summary>
        Task<bool> CompleteEventAsync(int eventId);
        /// <summary>
        /// Lấy trạng thái hiện tại của sự kiện hiến máu.
        /// </summary>
        Task<string> GetEventStatusAsync(int eventId);
        /// <summary>
        /// Cập nhật số lượng người hiến máu tối đa cho sự kiện.
        /// </summary>
        Task<bool> UpdateEventCapacityAsync(int eventId, int maxDonors);
        /// <summary>
        /// Lấy số lượng chỗ còn trống của sự kiện.
        /// </summary>
        Task<int> GetEventAvailableSlotsAsync(int eventId);
        /// <summary>
        /// Kiểm tra sự kiện đã đủ số lượng người hiến máu chưa.
        /// </summary>
        Task<bool> IsEventFullAsync(int eventId);
        /// <summary>
        /// Tăng số lượng người hiến máu hiện tại của sự kiện.
        /// </summary>
        Task<bool> IncrementCurrentDonorsAsync(int eventId);
        /// <summary>
        /// Giảm số lượng người hiến máu hiện tại của sự kiện.
        /// </summary>
        Task<bool> DecrementCurrentDonorsAsync(int eventId);
        /// <summary>
        /// Lấy danh sách các sự kiện hiến máu sắp tới.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetUpcomingEventsAsync();
        /// <summary>
        /// Lấy danh sách các sự kiện hiến máu đã diễn ra.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetPastEventsAsync();
        /// <summary>
        /// Lấy danh sách sự kiện hiến máu theo khoảng ngày.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Lấy danh sách sự kiện hiến máu theo địa điểm.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByLocationAsync(int locationId);
        /// <summary>
        /// Lấy danh sách sự kiện hiến máu do người tạo chỉ định.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByCreatorAsync(int creatorId);
        /// <summary>
        /// Tìm kiếm sự kiện hiến máu theo từ khóa.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> SearchEventsAsync(string searchTerm);
        /// <summary>
        /// Lấy danh sách sự kiện hiến máu theo trạng thái.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByStatusAsync(string status);
        /// <summary>
        /// Lấy danh sách sự kiện hiến máu theo nhóm máu cần thiết.
        /// </summary>
        Task<IEnumerable<BloodDonationEventDto>> GetEventsByBloodTypeAsync(string requiredBloodTypes);
        /// <summary>
        /// Lấy thống kê cho một sự kiện hiến máu.
        /// </summary>
        Task<EventStatisticsDto> GetEventStatisticsAsync(int eventId);
        /// <summary>
        /// Lấy thống kê cho tất cả sự kiện hiến máu.
        /// </summary>
        Task<IEnumerable<EventStatisticsDto>> GetAllEventStatisticsAsync();
        /// <summary>
        /// Lấy số lượng đăng ký tham gia sự kiện.
        /// </summary>
        Task<int> GetEventRegistrationCountAsync(int eventId);
        /// <summary>
        /// Lấy số lượng người hiến máu thực tế của sự kiện.
        /// </summary>
        Task<int> GetEventDonationCountAsync(int eventId);
        /// <summary>
        /// Kiểm tra sự kiện có tồn tại theo ID không.
        /// </summary>
        Task<bool> IsEventExistsAsync(int eventId);
        /// <summary>
        /// Kiểm tra tên sự kiện đã tồn tại chưa.
        /// </summary>
        Task<bool> IsEventNameExistsAsync(string eventName);
        /// <summary>
        /// Kiểm tra ngày tổ chức sự kiện có hợp lệ không.
        /// </summary>
        Task<bool> IsEventDateValidAsync(DateTime eventDate);
        /// <summary>
        /// Kiểm tra thời gian bắt đầu và kết thúc sự kiện có hợp lệ không.
        /// </summary>
        Task<bool> IsEventTimeValidAsync(TimeSpan startTime, TimeSpan endTime);
        /// <summary>
        /// Gửi thông báo nhắc nhở cho sự kiện hiến máu.
        /// </summary>
        Task<bool> SendEventRemindersAsync(int eventId);
        /// <summary>
        /// Gửi thông báo cập nhật cho sự kiện hiến máu.
        /// </summary>
        Task<bool> SendEventUpdatesAsync(int eventId, string updateMessage);
    }
} 