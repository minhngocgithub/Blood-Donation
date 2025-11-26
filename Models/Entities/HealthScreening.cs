using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Blood_Donation_Website.Utilities.EnumMapper;

namespace Blood_Donation_Website.Models.Entities
{
    /// <summary>
    /// Entity quản lý kết quả sàng lọc sức khỏe trước khi hiến máu
    /// Kiểm tra các chỉ số sức khỏe để đánh giá người hiến có đủ điều kiện hay không
    /// Table: HealthScreening
    /// </summary>
    [Table("HealthScreening")]
    public class HealthScreening
    {
        /// <summary>ID duy nhất của phiếu sàng lọc - Primary Key</summary>
        [Key]
        public int ScreeningId { get; set; }

        /// <summary>ID đăng ký hiến máu tương ứng - Foreign Key đến bảng DonationRegistrations</summary>
        [Required]
        public int RegistrationId { get; set; }

        /// <summary>Cân nặng (kg) - chính xác đến 2 chữ số thập phân</summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        /// <summary>Chiều cao (cm) - chính xác đến 2 chữ số thập phân</summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; }

        /// <summary>Huyết áp (VD: "120/80" mmHg)</summary>
        [StringLength(20)]
        public string? BloodPressure { get; set; }

        /// <summary>Nhịp tim (lần/phút)</summary>
        public int? HeartRate { get; set; }

        /// <summary>Nhiệt độ cơ thể (°C) - chính xác đến 2 chữ số thập phân</summary>
        [Column(TypeName = "decimal(4,2)")]
        public decimal? Temperature { get; set; }

        /// <summary>Nồng độ hemoglobin trong máu (g/dL) - chỉ số đo thiếu máu</summary>
        [Column(TypeName = "decimal(4,2)")]
        public decimal? Hemoglobin { get; set; }

        /// <summary>Kết quả sàng lọc: Có đủ điều kiện hiến máu hay không</summary>
        public bool IsEligible { get; set; } = true;

        /// <summary>Lý do loại nếu không đủ điều kiện (Underweight, HighBloodPressure...)</summary>
        public DisqualificationReason? DisqualifyReason { get; set; }

        /// <summary>ID người thực hiện sàng lọc (Doctor/Staff) - Foreign Key đến bảng Users</summary>
        public int? ScreenedBy { get; set; }
        
        /// <summary>Ngày giờ thực hiện sàng lọc</summary>
        public DateTime ScreeningDate { get; set; } = DateTime.Now;

        // === NAVIGATION PROPERTIES - Mối quan hệ với các bảng khác ===
        
        /// <summary>Thông tin đăng ký hiến máu tương ứng</summary>
        [ForeignKey("RegistrationId")]
        public virtual DonationRegistration Registration { get; set; } = null!;

        /// <summary>Người thực hiện sàng lọc (Bác sỹ/Nhân viên y tế)</summary>
        [ForeignKey("ScreenedBy")]
        public virtual User? ScreenedByUser { get; set; }
    }
}