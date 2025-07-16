using Blood_Donation_Website.Models.Entities;
using Blood_Donation_Website.Models.DTOs;

namespace Blood_Donation_Website.Services.Interfaces
{
    public interface IBloodCompatibilityService
    {
        // Compatibility queries

        /// <summary>
        /// Lấy thông tin chi tiết của một bản ghi tương thích nhóm máu theo ID.
        /// </summary>
        Task<BloodCompatibilityDto?> GetCompatibilityByIdAsync(int id);
        /// <summary>
        /// Lấy tất cả các bản ghi tương thích trong cơ sở dữ liệu.
        /// </summary>
        Task<IEnumerable<BloodCompatibilityDto>> GetAllCompatibilitiesAsync();
        /// <summary>
        /// Lấy danh sách các nhóm máu mà một nhóm máu (người hiến) có thể hiến.
        /// </summary>
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibleBloodTypesAsync(int fromBloodTypeId);
        /// <summary>
        /// Lấy danh sách các nhóm máu có thể nhận từ một nhóm máu (người nhận).
        /// </summary>
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibleRecipientsAsync(int toBloodTypeId);
        /// <summary>
        /// Lấy thông tin tương thích cụ thể giữa hai nhóm máu
        /// </summary>
        Task<BloodCompatibilityDto?> GetCompatibilityAsync(int fromBloodTypeId, int toBloodTypeId);
        
        // Compatibility validation

        /// <summary>
        /// Kiểm tra xem một nhóm máu người hiến có tương thích với nhóm máu người nhận không.
        /// </summary>
        Task<bool> IsCompatibleAsync(int fromBloodTypeId, int toBloodTypeId);
        /// <summary>
        /// Kiểm tra xem một quy tắc tương thích có tồn tại trong cơ sở dữ liệu không.
        /// </summary>
        Task<bool> IsCompatibilityExistsAsync(int fromBloodTypeId, int toBloodTypeId);
        /// <summary>
        /// Kiểm tra xem một ID nhóm máu có hợp lệ không.
        /// </summary>
        Task<bool> IsValidBloodTypeAsync(int bloodTypeId);
        
        // Compatibility matrix
        
        /// <summary>
        /// Từ điển ánh xạ các nhóm máu đến danh sách nhóm máu tương thích.
        /// </summary>
        Task<Dictionary<string, List<string>>> GetCompatibilityMatrixAsync();
        /// <summary>
        /// Lấy danh sách tên nhóm máu mà một người hiến có thể hiến.
        /// </summary>
        Task<List<string>> GetCompatibleBloodTypeNamesAsync(int fromBloodTypeId);
        /// <summary>
        /// Lấy danh sách tên nhóm máu mà một người hiến có thể hiến
        /// </summary>
        Task<List<string>> GetCompatibleRecipientNamesAsync(int toBloodTypeId);
        
        // Compatibility search

        /// <summary>
        /// Tìm kiếm các bản ghi tương thích dựa trên một chuỗi tìm kiếm.
        /// </summary>
        Task<IEnumerable<BloodCompatibilityDto>> SearchCompatibilitiesAsync(string searchTerm);
        /// <summary>
        /// Lấy các bản ghi tương thích cho một nhóm máu người hiến.
        /// </summary>
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibilitiesByFromTypeAsync(int fromBloodTypeId);
        /// <summary>
        /// Lấy các bản ghi tương thích cho một nhóm máu người nhận.
        /// </summary>
        Task<IEnumerable<BloodCompatibilityDto>> GetCompatibilitiesByToTypeAsync(int toBloodTypeId);
        
        // Compatibility statistics
        
        /// <summary>
        /// Đếm tổng số bản ghi tương thích trong cơ sở dữ liệu.
        /// </summary>
        Task<int> GetTotalCompatibilitiesAsync();
        /// <summary>
        /// Đếm số nhóm máu mà một người hiến có thể hiến.
        /// </summary>
        Task<int> GetCompatibleTypesCountAsync(int fromBloodTypeId);
        /// <summary>
        /// Đếm số nhóm máu có thể nhận từ một người hiến.
        /// </summary>
        Task<int> GetCompatibleRecipientsCountAsync(int toBloodTypeId);
        
        // Compatibility recommendations
        
        /// <summary>
        /// Gợi ý các nhóm máu người hiến cho một nhóm máu người nhận.
        /// </summary>
        Task<IEnumerable<BloodTypeDto>> GetRecommendedDonorsAsync(int requiredBloodTypeId);
        /// <summary>
        /// Gợi ý các nhóm máu người nhận cho một nhóm máu người hiến.
        /// </summary>
        Task<IEnumerable<BloodTypeDto>> GetRecommendedRecipientsAsync(int availableBloodTypeId);
        /// <summary>
        /// Cung cấp giải thích về tính tương thích giữa hai nhóm máu.
        /// </summary>
        Task<string> GetCompatibilityExplanationAsync(int fromBloodTypeId, int toBloodTypeId);
        
        // Compatibility validation for donations
        
        /// <summary>
        /// Kiểm tra xem một ca hiến máu có tương thích không.
        /// </summary>
        Task<bool> IsDonationCompatibleAsync(int donorBloodTypeId, int recipientBloodTypeId);
        /// <summary>
        /// Xác thực tính tương thích của một bản ghi hiến máu.
        /// </summary>
        Task<bool> ValidateDonationCompatibilityAsync(int donationId);
        /// <summary>
        /// Tìm người hiến tương thích cho một sự kiện hiến máu.
        /// </summary>
        Task<IEnumerable<BloodTypeDto>> GetCompatibleDonorsForEventAsync(int eventId, int requiredBloodTypeId);
        
        // Compatibility reporting
        
        /// <summary>
        /// Cung cấp thống kê về tần suất sử dụng các quy tắc tương thích.
        /// </summary>
        Task<Dictionary<string, int>> GetCompatibilityUsageAsync();
        /// <summary>
        /// Tạo dữ liệu để hiển thị biểu đồ tương thích.
        /// </summary>
        Task<Dictionary<string, List<string>>> GetCompatibilityChartAsync();
        
        // Compatibility education
        
        /// <summary>
        /// Cung cấp hướng dẫn chung về tương thích nhóm máu.
        /// </summary>
        Task<string> GetCompatibilityGuidelinesAsync();
        /// <summary>
        /// Cung cấp thông tin chi tiết về một nhóm máu cụ thể.
        /// </summary>
        Task<string> GetBloodTypeInformationAsync(int bloodTypeId);
        /// <summary>
        /// Cung cấp thông tin về tất cả các nhóm máu.
        /// </summary>
        Task<Dictionary<string, string>> GetAllBloodTypeInformationAsync();
    }
} 