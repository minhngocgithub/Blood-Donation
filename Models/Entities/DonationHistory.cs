using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.Entities
{
    /// <summary>
    /// Entity lưu trữ lịch sử hiến máu
    /// Ghi nhận tất cả các lần hiến máu thành công của người dùng
    /// Table: DonationHistory
    /// </summary>
    [Table("DonationHistory")]
    public class DonationHistory
    {
        /// <summary>ID duy nhất của bản ghi hiến máu - Primary Key</summary>
        [Key]
        public int DonationId { get; set; }

        /// <summary>ID người hiến máu - Foreign Key đến bảng Users</summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>ID sự kiện hiến máu - Foreign Key đến bảng BloodDonationEvents</summary>
        [Required]
        public int EventId { get; set; }

        /// <summary>ID đăng ký hiến máu tương ứng - Foreign Key đến bảng DonationRegistrations</summary>
        public int? RegistrationId { get; set; }

        /// <summary>Ngày giờ hiến máu thực tế</summary>
        [Required]
        public DateTime DonationDate { get; set; }

        /// <summary>ID nhóm máu đã hiến - Foreign Key đến bảng BloodTypes</summary>
        [Required]
        public int BloodTypeId { get; set; }

        /// <summary>Thể tích máu hiến (ml) - thường là 250ml, 350ml, 450ml</summary>
        [Required]
        public int Volume { get; set; }

        /// <summary>Trạng thái hiến máu (Completed/Cancelled/Pending...)</summary>
        public DonationStatus? Status { get; set; }

        /// <summary>Ghi chú bổ sung (tình trạng sức khỏe sau hiến, phản ứng...)</summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        /// <summary>Ngày có thể hiến máu tiếp theo - thường là DonationDate + 90 ngày</summary>
        public DateTime? NextEligibleDate { get; set; }
        
        /// <summary>Đã cấp giấy chứng nhận hiến máu chưa</summary>
        public bool CertificateIssued { get; set; } = false;

        // === NAVIGATION PROPERTIES - Mối quan hệ với các bảng khác ===
        
        /// <summary>Thông tin người hiến máu</summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        /// <summary>Thông tin sự kiện hiến máu</summary>
        [ForeignKey("EventId")]
        public virtual BloodDonationEvent Event { get; set; } = null!;
        
        /// <summary>Thông tin đăng ký hiến máu tương ứng</summary>
        [ForeignKey("RegistrationId")]
        public virtual DonationRegistration? Registration { get; set; }
        
        /// <summary>Thông tin nhóm máu đã hiến</summary>
        [ForeignKey("BloodTypeId")]
        public virtual BloodType BloodType { get; set; } = null!;
    }
}