using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blood_Donation_Website.Utilities
{
    /// <summary>
    /// Utility class để map 2 chiều giữa enum và string
    /// </summary>
    public static class EnumMapper
    {
        #region Gender Enum
        public enum Gender
        {
            [Display(Name = "Nam")]
            Male,
            [Display(Name = "Nữ")]
            Female,
            [Display(Name = "Khác")]
            Other
        }
        #endregion

        #region RegistrationStatus Enum
        public enum RegistrationStatus
        {
            [Display(Name = "Đã đăng ký")]
            Registered,
            [Display(Name = "Đã xác nhận")]
            Confirmed,
            [Display(Name = "Đã đến")]
            CheckedIn,
            [Display(Name = "Đang sàng lọc")]
            Screening,
            [Display(Name = "Đủ điều kiện")]
            Eligible,
            [Display(Name = "Không đủ điều kiện")]
            Ineligible,
            [Display(Name = "Đang hiến máu")]
            Donating,
            [Display(Name = "Hoàn tất")]
            Completed,
            [Display(Name = "Đã hủy")]
            Cancelled,
            [Display(Name = "Không đến")]
            NoShow,
            [Display(Name = "Thất bại")]
            Failed
        }
        #endregion

        #region EventStatus Enum
        public enum EventStatus
        {
            [Display(Name = "Bản nháp")]
            Draft,
            [Display(Name = "Đã công bố")]
            Published,
            [Display(Name = "Đang diễn ra")]
            Active,
            [Display(Name = "Hoàn thành")]
            Completed,
            [Display(Name = "Đã hủy")]
            Cancelled,
            [Display(Name = "Hoãn lại")]
            Postponed,
            [Display(Name = "Đã đầy")]
            Full,
            [Display(Name = "Đóng đăng ký")]
            Closed
        }
        #endregion

        #region DisqualificationReason Enum
        public enum DisqualificationReason
        {
            [Display(Name = "Hb thấp")]
            LowHemoglobin,
            [Display(Name = "Huyết áp cao")]
            HighBloodPressure,
            [Display(Name = "Huyết áp thấp")]
            LowBloodPressure,
            [Display(Name = "Sốt")]
            Fever,
            [Display(Name = "Cân nặng không đủ")]
            LowWeight,
            [Display(Name = "Mới hiến gần đây")]
            RecentDonation,
            [Display(Name = "Tiền sử bệnh lý")]
            MedicalHistory,
            [Display(Name = "Đang dùng thuốc")]
            CurrentMedication,
            [Display(Name = "Mới tiêm vaccine")]
            RecentVaccination,
            [Display(Name = "Đang mang thai")]
            Pregnancy,
            [Display(Name = "Đang cho con bú")]
            Breastfeeding,
            [Display(Name = "Mới phẫu thuật")]
            RecentSurgery,
            [Display(Name = "Nguy cơ nhiễm trùng")]
            InfectionRisk,
            [Display(Name = "Lý do khác")]
            Other
        }
        #endregion

        #region DonationStatus Enum
        public enum DonationStatus
        {
            [Display(Name = "Bắt đầu")]
            Started,
            [Display(Name = "Đang thực hiện")]
            InProgress,
            [Display(Name = "Hoàn thành")]
            Completed,
            [Display(Name = "Dừng giữa chừng")]
            Stopped,
            [Display(Name = "Thất bại")]
            Failed
        }
        #endregion

        #region NotificationType Enum
        public enum NotificationType
        {
            [Display(Name = "Đăng ký")]
            Registration,
            [Display(Name = "Xác nhận")]
            Confirmation,
            [Display(Name = "Nhắc nhở")]
            Reminder,
            [Display(Name = "Hủy bỏ")]
            Cancellation,
            [Display(Name = "Hoàn thành")]
            Completion,
            [Display(Name = "Kết quả")]
            Result,
            [Display(Name = "Sự kiện")]
            Event,
            [Display(Name = "Hệ thống")]
            System,
            [Display(Name = "Y tế")]
            Medical,
            [Display(Name = "Cảnh báo")]
            Warning,
            [Display(Name = "Thông tin")]
            Info
        }
        #endregion

        #region MessageStatus Enum
        public enum MessageStatus
        {
            [Display(Name = "Mới")]
            New,
            [Display(Name = "Đã đọc")]
            Read,
            [Display(Name = "Đang xử lý")]
            InProgress,
            [Display(Name = "Đã giải quyết")]
            Resolved,
            [Display(Name = "Đã đóng")]
            Closed
        }
        #endregion

        #region RoleType Enum
        public enum RoleType
        {
            [Display(Name = "Quản trị viên")]
            Admin,
            [Display(Name = "Người dùng")]
            User,
            [Display(Name = "Bệnh viện")]
            Hospital,
            [Display(Name = "Bác sĩ")]
            Doctor,
            [Display(Name = "Nhân viên y tế")]
            Staff
        }
        #endregion

        #region Generic Enum Mapping Methods
        /// <summary>
        /// Lấy Display Name từ enum value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Display name hoặc enum name nếu không có Display attribute</returns>
        public static string GetDisplayName<T>(T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var displayAttribute = field?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Lấy enum value từ Display Name
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="displayName">Display name</param>
        /// <returns>Enum value hoặc default nếu không tìm thấy</returns>
        public static T GetEnumFromDisplayName<T>(string displayName) where T : Enum
        {
            var enumType = typeof(T);
            foreach (var field in enumType.GetFields())
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute?.Name == displayName)
                {
                    return (T)field.GetValue(null);
                }
            }
            return default(T);
        }

        /// <summary>
        /// Lấy tất cả Display Names của enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>Dictionary với key là enum value, value là display name</returns>
        public static Dictionary<T, string> GetAllDisplayNames<T>() where T : Enum
        {
            var result = new Dictionary<T, string>();
            var enumType = typeof(T);
            
            foreach (var field in enumType.GetFields())
            {
                if (field.FieldType == enumType)
                {
                    var enumValue = (T)field.GetValue(null);
                    var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                    var displayName = displayAttribute?.Name ?? field.Name;
                    result[enumValue] = displayName;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Lấy tất cả Display Names dưới dạng SelectList
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>List of SelectListItem</returns>
        public static List<SelectListItem> GetSelectList<T>() where T : Enum
        {
            var result = new List<SelectListItem>();
            var displayNames = GetAllDisplayNames<T>();
            
            foreach (var item in displayNames)
            {
                result.Add(new SelectListItem
                {
                    Value = item.Key.ToString(),
                    Text = item.Value
                });
            }
            
            return result;
        }

        /// <summary>
        /// Kiểm tra xem string có phải là Display Name hợp lệ không
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="displayName">Display name cần kiểm tra</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool IsValidDisplayName<T>(string displayName) where T : Enum
        {
            var enumType = typeof(T);
            foreach (var field in enumType.GetFields())
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute?.Name == displayName)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Specific Enum Methods
        /// <summary>
        /// Lấy tất cả Gender options
        /// </summary>
        public static List<SelectListItem> GetGenderOptions()
        {
            return GetSelectList<Gender>();
        }

        /// <summary>
        /// Lấy tất cả RegistrationStatus options
        /// </summary>
        public static List<SelectListItem> GetRegistrationStatusOptions()
        {
            return GetSelectList<RegistrationStatus>();
        }

        /// <summary>
        /// Lấy tất cả EventStatus options
        /// </summary>
        public static List<SelectListItem> GetEventStatusOptions()
        {
            return GetSelectList<EventStatus>();
        }

        /// <summary>
        /// Lấy tất cả DisqualificationReason options
        /// </summary>
        public static List<SelectListItem> GetDisqualificationReasonOptions()
        {
            return GetSelectList<DisqualificationReason>();
        }

        /// <summary>
        /// Lấy tất cả DonationStatus options
        /// </summary>
        public static List<SelectListItem> GetDonationStatusOptions()
        {
            return GetSelectList<DonationStatus>();
        }

        /// <summary>
        /// Lấy tất cả NotificationType options
        /// </summary>
        public static List<SelectListItem> GetNotificationTypeOptions()
        {
            return GetSelectList<NotificationType>();
        }

        /// <summary>
        /// Lấy tất cả MessageStatus options
        /// </summary>
        public static List<SelectListItem> GetMessageStatusOptions()
        {
            return GetSelectList<MessageStatus>();
        }

        /// <summary>
        /// Lấy tất cả RoleType options
        /// </summary>
        public static List<SelectListItem> GetRoleTypeOptions()
        {
            return GetSelectList<RoleType>();
        }
        #endregion
    }
} 