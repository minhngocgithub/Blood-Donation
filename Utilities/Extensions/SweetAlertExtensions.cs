using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Blood_Donation_Website.Extensions
{
    /// <summary>
    /// Extension methods để dễ dàng sử dụng SweetAlert trong Controllers
    /// </summary>
    public static class SweetAlertExtensions
    {
        /// <summary>
        /// Hiển thị thông báo thành công
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề thông báo</param>
        /// <param name="message">Nội dung thông báo (tùy chọn)</param>
        /// <param name="timer">Thời gian tự động đóng (ms, mặc định 3000)</param>
        public static void Success(this Controller controller, string title, string message = "", int timer = 3000)
        {
            var alertData = new
            {
                type = "success",
                title = title,
                text = message,
                timer = timer
            };
            controller.TempData["SweetAlert"] = JsonSerializer.Serialize(alertData);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề thông báo</param>
        /// <param name="message">Nội dung thông báo (tùy chọn)</param>
        public static void Error(this Controller controller, string title, string message = "")
        {
            var alertData = new
            {
                type = "error",
                title = title,
                text = message
            };
            controller.TempData["SweetAlert"] = JsonSerializer.Serialize(alertData);
        }

        /// <summary>
        /// Hiển thị thông báo cảnh báo
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề thông báo</param>
        /// <param name="message">Nội dung thông báo (tùy chọn)</param>
        public static void Warning(this Controller controller, string title, string message = "")
        {
            var alertData = new
            {
                type = "warning",
                title = title,
                text = message
            };
            controller.TempData["SweetAlert"] = JsonSerializer.Serialize(alertData);
        }

        /// <summary>
        /// Hiển thị thông báo thông tin
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề thông báo</param>
        /// <param name="message">Nội dung thông báo (tùy chọn)</param>
        public static void Info(this Controller controller, string title, string message = "")
        {
            var alertData = new
            {
                type = "info",
                title = title,
                text = message
            };
            controller.TempData["SweetAlert"] = JsonSerializer.Serialize(alertData);
        }

        /// <summary>
        /// Hiển thị thông báo với HTML tùy chỉnh
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề thông báo</param>
        /// <param name="html">Nội dung HTML</param>
        /// <param name="icon">Icon hiển thị (tùy chọn)</param>
        public static void CustomHTML(this Controller controller, string title, string html, string? icon = null)
        {
            var alertData = new
            {
                type = "custom",
                title = title,
                html = html,
                icon = icon
            };
            controller.TempData["SweetAlert"] = JsonSerializer.Serialize(alertData);
        }

        /// <summary>
        /// Hiển thị toast notification
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="type">Loại toast (success, error, warning, info)</param>
        /// <param name="title">Tiêu đề toast</param>
        /// <param name="timer">Thời gian hiển thị (ms, mặc định 3000)</param>
        /// <param name="position">Vị trí hiển thị (mặc định top-end)</param>
        public static void Toast(this Controller controller, string type, string title, int timer = 3000, string position = "top-end")
        {
            var toastData = new
            {
                type = type,
                title = title,
                timer = timer,
                position = position
            };
            controller.TempData["Toast"] = JsonSerializer.Serialize(toastData);
        }

        /// <summary>
        /// Hiển thị toast thành công
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề toast</param>
        /// <param name="timer">Thời gian hiển thị (ms, mặc định 3000)</param>
        /// <param name="position">Vị trí hiển thị (mặc định top-end)</param>
        public static void ToastSuccess(this Controller controller, string title, int timer = 3000, string position = "top-end")
        {
            controller.Toast("success", title, timer, position);
        }

        /// <summary>
        /// Hiển thị toast lỗi
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề toast</param>
        /// <param name="timer">Thời gian hiển thị (ms, mặc định 3000)</param>
        /// <param name="position">Vị trí hiển thị (mặc định top-end)</param>
        public static void ToastError(this Controller controller, string title, int timer = 3000, string position = "top-end")
        {
            controller.Toast("error", title, timer, position);
        }

        /// <summary>
        /// Hiển thị toast cảnh báo
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề toast</param>
        /// <param name="timer">Thời gian hiển thị (ms, mặc định 3000)</param>
        /// <param name="position">Vị trí hiển thị (mặc định top-end)</param>
        public static void ToastWarning(this Controller controller, string title, int timer = 3000, string position = "top-end")
        {
            controller.Toast("warning", title, timer, position);
        }

        /// <summary>
        /// Hiển thị toast thông tin
        /// </summary>
        /// <param name="controller">Controller hiện tại</param>
        /// <param name="title">Tiêu đề toast</param>
        /// <param name="timer">Thời gian hiển thị (ms, mặc định 3000)</param>
        /// <param name="position">Vị trí hiển thị (mặc định top-end)</param>
        public static void ToastInfo(this Controller controller, string title, int timer = 3000, string position = "top-end")
        {
            controller.Toast("info", title, timer, position);
        }

        // Extension methods để sử dụng đơn giản hơn với TempData
        /// <summary>
        /// Thông báo thành công đơn giản (sử dụng TempData["SuccessMessage"])
        /// </summary>
        public static void SetSuccessMessage(this Controller controller, string message)
        {
            controller.TempData["SuccessMessage"] = message;
        }

        /// <summary>
        /// Thông báo lỗi đơn giản (sử dụng TempData["ErrorMessage"])
        /// </summary>
        public static void SetErrorMessage(this Controller controller, string message)
        {
            controller.TempData["ErrorMessage"] = message;
        }

        /// <summary>
        /// Thông báo cảnh báo đơn giản (sử dụng TempData["WarningMessage"])
        /// </summary>
        public static void SetWarningMessage(this Controller controller, string message)
        {
            controller.TempData["WarningMessage"] = message;
        }

        /// <summary>
        /// Thông báo thông tin đơn giản (sử dụng TempData["InfoMessage"])
        /// </summary>
        public static void SetInfoMessage(this Controller controller, string message)
        {
            controller.TempData["InfoMessage"] = message;
        }
    }

    /// <summary>
    /// Các template thông báo chuyên biệt cho ứng dụng hiến máu
    /// </summary>
    public static class BloodDonationAlerts
    {
        /// <summary>
        /// Thông báo đăng ký hiến máu thành công
        /// </summary>
        public static void BloodDonationRegistrationSuccess(this Controller controller, string eventName, DateTime eventDate)
        {
            var html = $@"
                <div class='text-center'>
                    <div class='mb-3'>
                        <i class='fas fa-heart text-danger' style='font-size: 3rem;'></i>
                    </div>
                    <p class='mb-2'>Bạn đã đăng ký thành công cho sự kiện:</p>
                    <h6 class='text-primary'>{eventName}</h6>
                    <p class='mb-3'><i class='fas fa-calendar me-1'></i>{eventDate:dd/MM/yyyy HH:mm}</p>
                    <div class='alert alert-info small'>
                        <i class='fas fa-info-circle me-1'></i>
                        Chúng tôi sẽ gửi thông tin chi tiết qua email của bạn.
                    </div>
                </div>";

            controller.CustomHTML("Đăng ký thành công!", html, "success");
        }

        /// <summary>
        /// Thông báo hiến máu thành công
        /// </summary>
        public static void BloodDonationSuccess(this Controller controller, int volumeDonated, DateTime nextEligibleDate)
        {
            var html = $@"
                <div class='text-center'>
                    <div class='mb-3'>
                        <i class='fas fa-heart text-danger' style='font-size: 3rem;'></i>
                    </div>
                    <p class='mb-2'>Cảm ơn sự chia sẻ của bạn!</p>
                    <p class='mb-2'>Bạn đã hiến <strong>{volumeDonated}ml máu</strong></p>
                    <p class='mb-3'>Có thể cứu sống <strong>3 người</strong></p>
                    <div class='alert alert-success small'>
                        <i class='fas fa-calendar-alt me-1'></i>
                        Lần hiến tiếp theo: <strong>{nextEligibleDate:dd/MM/yyyy}</strong>
                    </div>
                </div>";

            controller.CustomHTML("Cảm ơn sự chia sẻ của bạn!", html, "success");
        }

        /// <summary>
        /// Thông báo cần kiểm tra sức khỏe
        /// </summary>
        public static void HealthCheckRequired(this Controller controller, List<string> requiredTests)
        {
            var testsHtml = string.Join("", requiredTests.Select(test => 
                $"<li class='mb-2'><i class='fas fa-check-circle text-warning me-2'></i>{test}</li>"));

            var html = $@"
                <div class='text-start'>
                    <p class='mb-3'>Theo kết quả sàng lọc ban đầu, bạn cần thực hiện các kiểm tra bổ sung:</p>
                    <ul class='list-unstyled'>{testsHtml}</ul>
                    <div class='alert alert-info small mt-3'>
                        <i class='fas fa-info-circle me-1'></i>
                        Vui lòng liên hệ bác sĩ để được tư vấn chi tiết.
                    </div>
                </div>";

            controller.CustomHTML("Cần kiểm tra sức khỏe", html, "warning");
        }

        /// <summary>
        /// Nhắc nhở hiến máu
        /// </summary>
        public static void DonationReminder(this Controller controller, string eventName, DateTime eventDate, string location)
        {
            var html = $@"
                <div class='text-center'>
                    <div class='mb-3'>
                        <i class='fas fa-calendar-check text-primary' style='font-size: 2.5rem;'></i>
                    </div>
                    <p class='mb-2'>Đã đến lúc bạn có thể hiến máu trở lại!</p>
                    <div class='card bg-light'>
                        <div class='card-body'>
                            <h6 class='card-title'>{eventName}</h6>
                            <p class='card-text mb-1'>
                                <i class='fas fa-calendar me-1'></i>{eventDate:dd/MM/yyyy HH:mm}<br>
                                <i class='fas fa-map-marker-alt me-1'></i>{location}
                            </p>
                        </div>
                    </div>
                </div>";

            controller.CustomHTML("Nhắc nhở hiến máu", html, "info");
        }
    }
}
