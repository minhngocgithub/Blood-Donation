using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blood_Donation_Website.Utilities
{
    /// <summary>
    /// Class tiện ích chứa tất cả các Enum của hệ thống
    /// Bao gồm: Giới tính, Trạng thái đăng ký, Trạng thái sự kiện, Lý do loại, Trạng thái hiến máu...
    /// Cung cấp các phương thức map 2 chiều giữa enum và string
    /// </summary>
    public static class EnumMapper
    {
        #region Gender Enum
        /// <summary>
        /// Enum giới tính - dùng cho User.Gender
        /// </summary>
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
        /// <summary>
        /// Enum trạng thái đăng ký hiến máu - dùng cho DonationRegistration.Status
        /// Chu trình: Registered → Confirmed → CheckedIn → Screening → Eligible/Ineligible → Donating → Completed
        /// Có thể Cancelled, NoShow, Failed, Deferred ở bất kỳ giai đoạn nào
        /// </summary>
        public enum RegistrationStatus
        {
            [Display(Name = "Đã đăng ký")] // Bước 1: Vừa đăng ký tham gia sự kiện
            Registered,
            [Display(Name = "Đã xác nhận")] // Bước 2: Admin/Hospital xác nhận đăng ký
            Confirmed,
            [Display(Name = "Đã đến")] // Bước 3: Check-in tại sự kiện
            CheckedIn,
            [Display(Name = "Đang sàng lọc")] // Bước 4: Đang sàng lọc sức khỏe
            Screening,
            [Display(Name = "Đủ điều kiện")] // Bước 5a: Pass sàng lọc, được hiến máu
            Eligible,
            [Display(Name = "Không đủ điều kiện")] // Bước 5b: Không pass sàng lọc
            Ineligible,
            [Display(Name = "Đang hiến máu")] // Bước 6: Đang trong quá trình lấy máu
            Donating,
            [Display(Name = "Hoàn tất")] // Bước 7: Đã hoàn thành hiến máu
            Completed,
            [Display(Name = "Đã hủy")] // Người dùng hủy đăng ký
            Cancelled,
            [Display(Name = "Không đến")] // Đăng ký nhưng không đến
            NoShow,
            [Display(Name = "Thất bại")] // Có vấn đề trong quá trình hiến máu
            Failed,
            [Display(Name = "Tạm hoãn")] // Tạm thời không hiến được, sẽ hiến lần sau
            Deferred
        }
        #endregion

        #region EventStatus Enum
        /// <summary>
        /// Enum trạng thái sự kiện hiến máu - dùng cho BloodDonationEvent.Status
        /// Chu trình: Draft → Published → Active → Completed
        /// </summary>
        public enum EventStatus
        {
            [Display(Name = "Bản nháp")] // Chưa công bố, đang soạn thảo
            Draft,
            [Display(Name = "Đã công bố")] // Đã công bố, người dùng có thể thấy và đăng ký
            Published,
            [Display(Name = "Đang diễn ra")] // Sự kiện đang diễn ra
            Active,
            [Display(Name = "Hoàn thành")] // Sự kiện đã kết thúc
            Completed,
            [Display(Name = "Đã hủy")] // Sự kiện bị hủy bỏ
            Cancelled,
            [Display(Name = "Hoãn lại")] // Tạm hoãn, sẽ tổ chức lần khác
            Postponed,
            [Display(Name = "Đã đầy")] // Đã đủ số lượng người đăng ký
            Full,
            [Display(Name = "Đóng đăng ký")] // Không nhận đăng ký mới nữa
            Closed
        }
        #endregion

        #region DisqualificationReason Enum
        /// <summary>
        /// Enum lý do loại/không đủ điều kiện hiến máu - dùng cho HealthScreening.DisqualifyReason
        /// Dựa trên các tiêu chuẩn y tế để đánh giá sức khỏe người hiến máu
        /// </summary>
        public enum DisqualificationReason
        {
            [Display(Name = "Thiếu cân (BMI < 18.5)")] // Chỉ số khối cơ thể thấp
            Underweight,
            [Display(Name = "Thừa cân (BMI > 30)")] // Chỉ số khối cơ thể cao
            Overweight,
            [Display(Name = "Huyết áp cao")] // Huyết áp > 140/90 mmHg
            HighBloodPressure,
            [Display(Name = "Huyết áp thấp")] // Huyết áp < 90/60 mmHg
            LowBloodPressure,
            [Display(Name = "Nhịp tim cao")] // Nhịp tim > 100 lần/phút
            HighHeartRate,
            [Display(Name = "Nhịp tim thấp")] // Nhịp tim < 50 lần/phút
            LowHeartRate,
            [Display(Name = "Sốt")] // Nhiệt độ > 37.5°C
            HighTemperature,
            [Display(Name = "Thiếu máu (Hb thấp)")] // Hemoglobin thấp
            LowHemoglobin,
            [Display(Name = "Cân nặng không đủ (< 45kg)")] // Cân nặng tối thiểu
            LowWeight,
            [Display(Name = "Chiều cao không đủ (< 150cm)")] // Chiều cao tối thiểu
            LowHeight,
            [Display(Name = "Mới hiến máu gần đây")] // Chưa đủ 90 ngày kể từ lần hiến cuối
            RecentDonation,
            [Display(Name = "Tiền sử bệnh lý")] // Có bệnh mạn tính
            MedicalHistory,
            [Display(Name = "Đang dùng thuốc")] // Đang sử dụng thuốc kê đơn
            CurrentMedication,
            [Display(Name = "Mới tiêm vaccine")] // Tiêm vaccine trong vòng 2 tuần
            RecentVaccination,
            [Display(Name = "Đang mang thai")] // Phụ nữ mang thai
            Pregnancy,
            [Display(Name = "Đang cho con bú")] // Phụ nữ đang cho con bú
            Breastfeeding,
            [Display(Name = "Mới phẫu thuật")] // Phẫu thuật trong vòng 6 tháng
            RecentSurgery,
            [Display(Name = "Nguy cơ nhiễm trùng")] // Tiếp xúc với nguồn lây nhiễm
            InfectionRisk,
            [Display(Name = "Lý do khác")] // Các lý do khác không liệt kê
            Other
        }
        #endregion

        #region DonationStatus Enum
        /// <summary>
        /// Enum trạng thái quá trình hiến máu - dùng cho DonationHistory.Status
        /// Chu trình: Started → InProgress → Completed
        /// </summary>
        public enum DonationStatus
        {
            [Display(Name = "Bắt đầu")] // Bắt đầu quá trình lấy máu
            Started,
            [Display(Name = "Đang thực hiện")] // Đang trong quá trình lấy máu
            InProgress,
            [Display(Name = "Hoàn thành")] // Đã lấy xong máu thành công
            Completed,
            [Display(Name = "Dừng giữa chừng")] // Dừng lại giữa chừng (người hiến không chịu được)
            Stopped,
            [Display(Name = "Thất bại")] // Không thể lấy máu (vấn đề kỹ thuật)
            Failed,
            [Display(Name = "Đã hủy")] // Hủy bỏ không tiến hành
            Cancelled
        }
        #endregion

        #region NotificationType Enum
        /// <summary>
        /// Enum phân loại thông báo - dùng cho Notification.Type
        /// Giúp người dùng phân biệt các loại thông báo khác nhau trong hệ thống
        /// </summary>
        public enum NotificationType
        {
            [Display(Name = "Đăng ký")] // Thông báo về đăng ký hiến máu mới
            Registration,
            [Display(Name = "Xác nhận")] // Xác nhận đã được chấp nhận đăng ký
            Confirmation,
            [Display(Name = "Nhắc nhở")] // Nhắc nhở về lịch hiến máu sắp tới
            Reminder,
            [Display(Name = "Hủy bỏ")] // Thông báo hủy đăng ký hoặc sự kiện
            Cancellation,
            [Display(Name = "Hoàn thành")] // Thông báo đã hoàn thành hiến máu
            Completion,
            [Display(Name = "Kết quả")] // Kết quả khám sàng lọc hoặc hiến máu
            Result,
            [Display(Name = "Sự kiện")] // Thông báo về sự kiện hiến máu mới
            Event,
            [Display(Name = "Hệ thống")] // Thông báo từ hệ thống (bảo trì, cập nhật...)
            System,
            [Display(Name = "Y tế")] // Thông báo về thông tin y tế (điều kiện hiến máu...)
            Medical,
            [Display(Name = "Cảnh báo")] // Cảnh báo quan trọng cần chú ý
            Warning,
            [Display(Name = "Thông tin")] // Thông tin chung, tin tức
            Info
        }
        #endregion

        #region MessageStatus Enum
        /// <summary>
        /// Enum trạng thái tin nhắn liên hệ - dùng cho ContactMessage.Status
        /// Chu trình xử lý: New → Read → InProgress → Resolved → Closed
        /// </summary>
        public enum MessageStatus
        {
            [Display(Name = "Mới")] // Bước 1: Tin nhắn mới gửi đến, chưa được đọc
            New,
            [Display(Name = "Đã đọc")] // Bước 2: Admin đã đọc tin nhắn
            Read,
            [Display(Name = "Đang xử lý")] // Bước 3: Đang tiến hành xử lý/trả lời
            InProgress,
            [Display(Name = "Đã giải quyết")] // Bước 4: Đã giải quyết xong vấn đề
            Resolved,
            [Display(Name = "Đã đóng")] // Bước 5: Đóng ticket, không cần xử lý thêm
            Closed
        }
        #endregion

        #region RoleType Enum
        /// <summary>
        /// Enum vai trò người dùng - dùng cho Role.RoleName
        /// Xác định quyền hạn và chức năng mà mỗi user có thể truy cập
        /// </summary>
        public enum RoleType
        {
            [Display(Name = "Quản trị viên")] // Quyền cao nhất: quản lý toàn bộ hệ thống, user, sự kiện, dữ liệu
            Admin,
            [Display(Name = "Người dùng")] // Người hiến máu: đăng ký sự kiện, xem lịch sử hiến máu
            User,
            [Display(Name = "Bệnh viện")] // Tổ chức y tế: tạo sự kiện, quản lý đăng ký
            Hospital,
            [Display(Name = "Bác sĩ")] // Bác sỹ: thực hiện khám sàng lọc, đánh giá sức khỏe
            Doctor,
            [Display(Name = "Nhân viên")] // Nhân viên: hỗ trợ check-in, quản lý sự kiện
            Staff
        }
        #endregion

        #region Generic Enum Mapping Methods - Các phương thức helper chuyển đổi enum
        /// <summary>
        /// Lấy Display Name từ enum value
        /// VD: GetDisplayName(Gender.Male) => "Nam"
        /// </summary>
        /// <typeparam name="T">Loại enum (Gender, RegistrationStatus...)</typeparam>
        /// <param name="enumValue">Giá trị enum cần lấy tên</param>
        /// <returns>Tên hiển thị tiếng Việt từ [Display(Name)] hoặc tên enum nếu không có attribute</returns>
        public static string GetDisplayName<T>(T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var displayAttribute = field?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Lấy enum value từ Display Name (chuyển đổi ngược lại)
        /// VD: GetEnumFromDisplayName<Gender>("Nam") => Gender.Male
        /// </summary>
        /// <typeparam name="T">Loại enum cần chuyển đổi</typeparam>
        /// <param name="displayName">Tên hiển thị tiếng Việt cần tìm</param>
        /// <returns>Giá trị enum tương ứng, hoặc giá trị mặc định nếu không tìm thấy</returns>
        public static T GetEnumFromDisplayName<T>(string displayName) where T : Enum
        {
            var enumType = typeof(T);
            foreach (var field in enumType.GetFields())
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute?.Name == displayName)
                {
                    var valueObj = field.GetValue(null);
                    if (valueObj != null)
                    {
                        return (T)valueObj;
                    }
                }
            }
            return default!;
        }

        /// <summary>
        /// Lấy tất cả Display Names của một enum
        /// VD: GetAllDisplayNames<Gender>() => {Male: "Nam", Female: "Nữ", Other: "Khác"}
        /// </summary>
        /// <typeparam name="T">Loại enum cần lấy danh sách</typeparam>
        /// <returns>Dictionary với key là giá trị enum, value là tên hiển thị tiếng Việt</returns>
        public static Dictionary<T, string> GetAllDisplayNames<T>() where T : Enum
        {
            var result = new Dictionary<T, string>();
            var enumType = typeof(T);
            
            foreach (var field in enumType.GetFields())
            {
                if (field.FieldType == enumType)
                {
                    var valueObj = field.GetValue(null);
                    if (valueObj != null)
                    {
                        var enumValue = (T)valueObj;
                        var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                        var displayName = displayAttribute?.Name ?? field.Name;
                        result[enumValue] = displayName;
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// Lấy tất cả Display Names dưới dạng SelectList (dùng cho dropdown trong View)
        /// Sử dụng trong Razor: @Html.DropDownListFor(m => m.Status, EnumMapper.GetSelectList<RegistrationStatus>())
        /// </summary>
        /// <typeparam name="T">Loại enum cần chuyển thành SelectList</typeparam>
        /// <returns>Danh sách SelectListItem với Value là enum name, Text là tên tiếng Việt</returns>
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
        /// Kiểm tra xem một chuỗi có phải là Display Name hợp lệ của enum không
        /// VD: IsValidDisplayName<Gender>("Nam") => true, IsValidDisplayName<Gender>("Abc") => false
        /// </summary>
        /// <typeparam name="T">Loại enum cần kiểm tra</typeparam>
        /// <param name="displayName">Tên hiển thị cần kiểm tra tính hợp lệ</param>
        /// <returns>True nếu displayName là tên hợp lệ, False nếu không tồn tại</returns>
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

        #region Specific Enum Methods - Các phương thức lấy SelectList cho từng enum cụ thể
        /// <summary>Lấy danh sách lựa chọn Giới tính (Nam/Nữ/Khác)</summary>
        public static List<SelectListItem> GetGenderOptions()
        {
            return GetSelectList<Gender>();
        }

        /// <summary>Lấy danh sách lựa chọn Trạng thái đăng ký (Registered, Confirmed, CheckedIn...)</summary>
        public static List<SelectListItem> GetRegistrationStatusOptions()
        {
            return GetSelectList<RegistrationStatus>();
        }

        /// <summary>Lấy danh sách lựa chọn Trạng thái sự kiện (Draft, Published, Active...)</summary>
        public static List<SelectListItem> GetEventStatusOptions()
        {
            return GetSelectList<EventStatus>();
        }

        /// <summary>Lấy danh sách lựa chọn Lý do loại không đủ điều kiện (Underweight, HighBloodPressure...)</summary>
        public static List<SelectListItem> GetDisqualificationReasonOptions()
        {
            return GetSelectList<DisqualificationReason>();
        }

        /// <summary>Lấy danh sách lựa chọn Trạng thái hiến máu (Started, InProgress, Completed...)</summary>
        public static List<SelectListItem> GetDonationStatusOptions()
        {
            return GetSelectList<DonationStatus>();
        }

        /// <summary>Lấy danh sách lựa chọn Loại thông báo (Registration, Confirmation, Reminder...)</summary>
        public static List<SelectListItem> GetNotificationTypeOptions()
        {
            return GetSelectList<NotificationType>();
        }

        /// <summary>Lấy danh sách lựa chọn Trạng thái tin nhắn (New, Read, InProgress...)</summary>
        public static List<SelectListItem> GetMessageStatusOptions()
        {
            return GetSelectList<MessageStatus>();
        }

        /// <summary>Lấy danh sách lựa chọn Vai trò người dùng (Admin, User, Hospital, Doctor, Staff)</summary>
        public static List<SelectListItem> GetRoleTypeOptions()
        {
            return GetSelectList<RoleType>();
        }
        #endregion

        #region Utility Methods - Các phương thức kiểm tra logic nghiệp vụ
        /// <summary>
        /// Kiểm tra xem trạng thái đăng ký có thể check-in không
        /// Chỉ cho phép check-in khi ở trạng thái Registered hoặc Confirmed
        /// </summary>
        /// <param name="status">Trạng thái đăng ký hiện tại</param>
        /// <returns>True nếu có thể check-in (Registered/Confirmed), False nếu không</returns>
        public static bool CanCheckIn(RegistrationStatus status)
        {
            return status == RegistrationStatus.Registered || status == RegistrationStatus.Confirmed;
        }

        /// <summary>
        /// Kiểm tra xem trạng thái đăng ký có thể sàng lọc không
        /// Chỉ cho phép sàng lọc khi đã check-in thành công
        /// </summary>
        /// <param name="status">Trạng thái đăng ký hiện tại</param>
        /// <returns>True nếu có thể sàng lọc (CheckedIn), False nếu không</returns>
        public static bool CanScreen(RegistrationStatus status)
        {
            return status == RegistrationStatus.CheckedIn;
        }

        /// <summary>
        /// Kiểm tra xem trạng thái đăng ký có thể hiến máu không
        /// Chỉ cho phép hiến máu khi đã được xác nhận đủ điều kiện (Eligible)
        /// </summary>
        /// <param name="status">Trạng thái đăng ký hiện tại</param>
        /// <param name="isEligible">Kết quả sàng lọc: Có đủ điều kiện sức khỏe không</param>
        /// <returns>True nếu có thể hiến máu (status=Eligible và isEligible=true), False nếu không</returns>
        public static bool CanDonate(RegistrationStatus status, bool isEligible)
        {
            return status == RegistrationStatus.Eligible && isEligible;
        }

        /// <summary>
        /// Lấy trạng thái tiếp theo sau khi sàng lọc sức khỏe
        /// Dựa vào kết quả sàng lọc để chuyển sang Eligible (có thể hiến) hoặc Ineligible (không đủ điều kiện)
        /// </summary>
        /// <param name="isEligible">Kết quả sàng lọc: Có đủ điều kiện sức khỏe không</param>
        /// <returns>RegistrationStatus.Eligible nếu isEligible=true, RegistrationStatus.Ineligible nếu isEligible=false</returns>
        public static RegistrationStatus GetNextStatusAfterScreening(bool isEligible)
        {
            return isEligible ? RegistrationStatus.Eligible : RegistrationStatus.Ineligible;
        }

        /// <summary>
        /// Lấy màu badge hiển thị cho trạng thái đăng ký (dùng trong View)
        /// Các màu: primary (xanh dương), info (xanh), success (xanh lá), warning (vàng), danger (đỏ), secondary (xám)
        /// </summary>
        /// <param name="status">Trạng thái đăng ký hiện tại</param>
        /// <returns>CSS class Bootstrap cho badge (VD: "bg-success", "bg-danger"...)</returns>
        public static string GetRegistrationStatusBadgeClass(RegistrationStatus status)
        {
            return status switch
            {
                RegistrationStatus.Registered => "bg-primary",
                RegistrationStatus.Confirmed => "bg-info",
                RegistrationStatus.CheckedIn => "bg-warning",
                RegistrationStatus.Screening => "bg-warning",
                RegistrationStatus.Eligible => "bg-success",
                RegistrationStatus.Ineligible => "bg-danger",
                RegistrationStatus.Donating => "bg-info",
                RegistrationStatus.Completed => "bg-success",
                RegistrationStatus.Cancelled => "bg-danger",
                RegistrationStatus.NoShow => "bg-secondary",
                RegistrationStatus.Failed => "bg-danger",
                RegistrationStatus.Deferred => "bg-warning",
                _ => "bg-secondary"
            };
        }

        /// <summary>
        /// Lấy màu badge hiển thị cho trạng thái sự kiện (dùng trong View)
        /// Các màu: success (xanh lá - đang diễn ra), warning (vàng - cảnh báo), danger (đỏ - hủy/kết thúc), primary (xanh dương - đã xuất bản)
        /// </summary>
        /// <param name="status">Trạng thái sự kiện hiện tại</param>
        /// <returns>CSS class Bootstrap cho text color (VD: "text-success", "text-danger"...)</returns>
        public static string GetEventStatusBadgeClass(EventStatus status)
        {
            return status switch
            {
                EventStatus.Active => "text-success",
                EventStatus.Full => "text-warning",
                EventStatus.Completed => "text-danger",
                EventStatus.Cancelled => "text-danger",
                EventStatus.Closed => "text-danger",
                EventStatus.Draft => "text-secondary",
                EventStatus.Published => "text-primary",
                EventStatus.Postponed => "text-warning",
                _ => "text-secondary"
            };
        }
        #endregion
    }
} 