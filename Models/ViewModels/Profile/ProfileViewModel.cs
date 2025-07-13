using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.ViewModels.Profile
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ và tên phải từ 2 đến 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Phone { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string? Province { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string? District { get; set; }

        [Display(Name = "Phường/Xã")]
        public string? Ward { get; set; }

        [Display(Name = "Số nhà, tên đường")]
        public string? AddressDetail { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        [Display(Name = "Nhóm Máu")]
        public string? BloodType { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? LastDonationDate { get; set; }

        public int TotalDonations { get; set; }
    }
}
