using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.Entities
{
    /// <summary>
    /// Entity quản lý sự kiện hiến máu
    /// Chứa thông tin về thời gian, địa điểm, số lượng người hiến máu của từng sự kiện
    /// Table: BloodDonationEvents
    /// </summary>
    [Table("BloodDonationEvents")]
    public class BloodDonationEvent
    {
        /// <summary>ID duy nhất của sự kiện - Primary Key</summary>
        [Key]
        public int EventId { get; set; }

        /// <summary>Tên sự kiện hiến máu</summary>
        [Required]
        [StringLength(200)]
        public string EventName { get; set; } = string.Empty;

        /// <summary>Mô tả chi tiết về sự kiện</summary>
        public string? EventDescription { get; set; }

        /// <summary>Ngày diễn ra sự kiện</summary>
        [Required]
        public DateTime EventDate { get; set; }

        /// <summary>Giờ bắt đầu sự kiện</summary>
        [Required]
        public TimeSpan StartTime { get; set; }

        /// <summary>Giờ kết thúc sự kiện</summary>
        [Required]
        public TimeSpan EndTime { get; set; }

        /// <summary>ID địa điểm tổ chức - Foreign Key đến bảng Locations</summary>
        public int? LocationId { get; set; }
        
        /// <summary>Số lượng người hiến máu tối đa mà sự kiện có thể tiếp nhận</summary>
        public int MaxDonors { get; set; } = 100;
        
        /// <summary>Số lượng người đã đăng ký/hiến máu hiện tại</summary>
        public int CurrentDonors { get; set; } = 0;

        /// <summary>Trạng thái sự kiện (Draft/Published/Active/Completed/Cancelled...)</summary>
        public EventStatus? Status { get; set; }

        /// <summary>Đường dẫn hình ảnh minh họa cho sự kiện</summary>
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        /// <summary>Danh sách nhóm máu cần thiết (VD: "A+, O+, AB-")</summary>
        [StringLength(100)]
        public string? RequiredBloodTypes { get; set; }

        /// <summary>ID người tạo sự kiện - Foreign Key đến bảng Users</summary>
        public int? CreatedBy { get; set; }
        
        /// <summary>Ngày tạo sự kiện</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        /// <summary>Ngày cập nhật thông tin sự kiện lần cuối</summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // === NAVIGATION PROPERTIES - Mối quan hệ với các bảng khác ===
        
        /// <summary>Địa điểm tổ chức sự kiện</summary>
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }

        /// <summary>Người tạo sự kiện (thường là Admin hoặc Hospital)</summary>
        [ForeignKey("CreatedBy")]
        public virtual User? Creator { get; set; }

        /// <summary>Danh sách đăng ký tham gia sự kiện</summary>
        public virtual ICollection<DonationRegistration> Registrations { get; set; } = new List<DonationRegistration>();
        
        /// <summary>Danh sách lịch sử hiến máu tại sự kiện này</summary>
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
    }
}