using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.Entities
{
    /// <summary>
    /// Entity đại diện cho người dùng trong hệ thống
    /// Bao gồm: Người hiến máu, Quản trị viên, Bác sỹ, Nhân viên, Bệnh viện
    /// Table: Users
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>ID duy nhất của người dùng - Primary Key</summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>Tên đăng nhập (thường là email)</summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>Địa chỉ email - dùng để đăng nhập và liên lạc</summary>
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>Mật khẩu đã mã hóa (hash) - không lưu mật khẩu thô (plain text)</summary>
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>Họ và tên đầy đủ</summary>
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>Số điện thoại liên lạc</summary>
        [StringLength(15)]
        public string? Phone { get; set; }

        /// <summary>Địa chỉ cư trú</summary>
        [StringLength(255)]
        public string? Address { get; set; }

        /// <summary>Ngày sinh</summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>Giới tính (Male/Female/Other)</summary>
        public Gender? Gender { get; set; }

        /// <summary>ID nhóm máu - Foreign Key đến bảng BloodTypes</summary>
        public int? BloodTypeId { get; set; }

        /// <summary>ID vai trò - Foreign Key đến bảng Roles (Admin=1, User=2, Doctor=3, Staff=4, Hospital=5)</summary>
        public int RoleId { get; set; } = 2; // Mặc định là User

        /// <summary>Tài khoản có đang hoạt động không (dùng để vô hiệu hóa tài khoản)</summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>Email đã được xác thực chưa</summary>
        public bool EmailVerified { get; set; } = false;
        
        /// <summary>Ngày hiến máu lần cuối - dùng để tính toán khi nào có thể hiến lại</summary>
        public DateTime? LastDonationDate { get; set; }
        
        /// <summary>Ngày tạo tài khoản</summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        /// <summary>Ngày cập nhật thông tin lần cuối</summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // === NAVIGATION PROPERTIES - Mối quan hệ với các bảng khác ===
        
        /// <summary>Vai trò của người dùng (Admin, User, Doctor, Staff, Hospital)</summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
        
        /// <summary>Nhóm máu của người dùng (A, B, AB, O với Rh+/-)</summary>
        [ForeignKey("BloodTypeId")]
        public virtual BloodType? BloodType { get; set; }

        /// <summary>Danh sách sự kiện hiến máu do người dùng tạo (chỉ dành cho Admin/Hospital)</summary>
        public virtual ICollection<BloodDonationEvent> CreatedEvents { get; set; } = new List<BloodDonationEvent>();
        
        /// <summary>Danh sách đăng ký hiến máu của người dùng</summary>
        public virtual ICollection<DonationRegistration> DonationRegistrations { get; set; } = new List<DonationRegistration>();
        
        /// <summary>Lịch sử hiến máu của người dùng</summary>
        public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
        
        /// <summary>Kết quả sàng lọc sức khỏe của người dùng</summary>
        public virtual ICollection<HealthScreening> HealthScreenings { get; set; } = new List<HealthScreening>();
        
        /// <summary>Thông báo gửi đến người dùng</summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        
        /// <summary>Tin nhắn liên hệ đã được giải quyết bởi người dùng (dành cho Staff/Admin)</summary>
        public virtual ICollection<ContactMessage> ResolvedContactMessages { get; set; } = new List<ContactMessage>();
        
        /// <summary>Bài viết tin tức được viết bởi người dùng (dành cho Admin/Hospital)</summary>
        public virtual ICollection<News> AuthoredNews { get; set; } = new List<News>();
    }
}
