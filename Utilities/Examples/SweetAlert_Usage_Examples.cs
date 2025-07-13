/**
 * Hướng dẫn sử dụng SweetAlert trong ASP.NET Core MVC
 * File: Controllers/ExampleController.cs
 */

using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    public class ExampleController : Controller
    {
        // 1. Hiển thị thông báo thành công sau khi thực hiện action
        public IActionResult RegisterSuccess()
        {
            // Thực hiện logic đăng ký...
            
            // Cách 1: Sử dụng TempData để truyền thông báo đến View
            TempData["SweetAlert"] = new
            {
                Type = "success",
                Title = "Đăng ký thành công!",
                Text = "Cảm ơn bạn đã đăng ký hiến máu. Chúng tôi sẽ liên hệ với bạn sớm nhất.",
                Timer = 3000
            };

            return RedirectToAction("Index", "Home");
        }

        // 2. Hiển thị thông báo lỗi
        public IActionResult Error()
        {
            TempData["SweetAlert"] = new
            {
                Type = "error",
                Title = "Có lỗi xảy ra",
                Text = "Không thể thực hiện yêu cầu. Vui lòng thử lại sau."
            };

            return RedirectToAction("Index");
        }

        // 3. Hiển thị toast notification
        public IActionResult QuickAction()
        {
            // Thực hiện action nhanh...
            
            TempData["Toast"] = new
            {
                Type = "success",
                Title = "Đã lưu thành công!",
                Timer = 2000,
                Position = "top-end"
            };

            return RedirectToAction("Index");
        }

        // 4. Yêu cầu xác nhận trước khi thực hiện action nguy hiểm
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                // Thực hiện xóa...
                // userService.Delete(id);

                TempData["SweetAlert"] = new
                {
                    Type = "success",
                    Title = "Đã xóa thành công",
                    Text = "Người dùng đã được xóa khỏi hệ thống."
                };
            }
            catch (Exception ex)
            {
                TempData["SweetAlert"] = new
                {
                    Type = "error",
                    Title = "Không thể xóa",
                    Text = "Có lỗi xảy ra trong quá trình xóa: " + ex.Message
                };
            }

            return RedirectToAction("Index");
        }

        // 5. Hiển thị progress trong quá trình xử lý dài
        public async Task<IActionResult> ProcessLongTask()
        {
            // Có thể sử dụng SignalR để cập nhật progress real-time
            // Hoặc sử dụng AJAX để check progress

            // Simulate long process
            await Task.Delay(5000);

            TempData["SweetAlert"] = new
            {
                Type = "success",
                Title = "Xử lý hoàn tất",
                Text = "Tác vụ đã được thực hiện thành công."
            };

            return RedirectToAction("Index");
        }

        // 6. Hiển thị thông tin chi tiết với HTML
        public IActionResult ShowBloodDonationInfo(int userId)
        {
            // Lấy thông tin từ database...
            var bloodInfo = new
            {
                BloodType = "O+",
                LastDonation = "15/07/2024",
                TotalDonations = 5,
                NextEligibleDate = "15/01/2025"
            };

            var htmlContent = $@"
                <div class='text-start'>
                    <div class='alert alert-info mb-3'>
                        <i class='fas fa-tint me-2'></i>
                        <strong>Thông tin hiến máu</strong>
                    </div>
                    <table class='table table-sm'>
                        <tr><td><strong>Nhóm máu:</strong></td><td>{bloodInfo.BloodType}</td></tr>
                        <tr><td><strong>Lần cuối hiến:</strong></td><td>{bloodInfo.LastDonation}</td></tr>
                        <tr><td><strong>Tổng lần hiến:</strong></td><td>{bloodInfo.TotalDonations} lần</td></tr>
                        <tr><td><strong>Lần hiến tiếp theo:</strong></td><td>{bloodInfo.NextEligibleDate}</td></tr>
                    </table>
                </div>";

            TempData["SweetAlert"] = new
            {
                Type = "custom",
                Title = "Hồ sơ hiến máu",
                Html = htmlContent,
                Icon = "info"
            };

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

/* 
 * Trong View Layout hoặc View cụ thể, thêm script để xử lý TempData:
 */

/*
@if (TempData["SweetAlert"] != null)
{
    var alert = TempData["SweetAlert"] as dynamic;
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            @if (alert.Type == "success")
            {
                <text>showSuccess('@alert.Title', '@alert.Text', @(alert.Timer ?? 3000));</text>
            }
            else if (alert.Type == "error")
            {
                <text>showError('@alert.Title', '@alert.Text');</text>
            }
            else if (alert.Type == "warning")
            {
                <text>showWarning('@alert.Title', '@alert.Text');</text>
            }
            else if (alert.Type == "info")
            {
                <text>showInfo('@alert.Title', '@alert.Text');</text>
            }
            else if (alert.Type == "custom")
            {
                <text>showCustomHTML('@alert.Title', `@Html.Raw(alert.Html)`, '@alert.Icon');</text>
            }
        });
    </script>
}

@if (TempData["Toast"] != null)
{
    var toast = TempData["Toast"] as dynamic;
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            showToast('@toast.Type', '@toast.Title', @(toast.Timer ?? 3000), '@(toast.Position ?? "top-end")');
        });
    </script>
}
*/

/*
 * Ví dụ sử dụng AJAX với SweetAlert:
 */

/*
function deleteUserWithConfirm(userId) {
    showDeleteConfirm('người dùng này', 'Tất cả dữ liệu liên quan cũng sẽ bị xóa.')
        .then((result) => {
            if (result.isConfirmed) {
                showLoading('Đang xóa...', 'Vui lòng đợi');
                
                $.ajax({
                    url: '/Example/DeleteUser',
                    type: 'POST',
                    data: { id: userId },
                    success: function(response) {
                        hideLoading();
                        showSuccess('Đã xóa thành công', 'Người dùng đã được xóa khỏi hệ thống.');
                        // Refresh page or remove element
                        location.reload();
                    },
                    error: function() {
                        hideLoading();
                        showError('Có lỗi xảy ra', 'Không thể xóa người dùng. Vui lòng thử lại.');
                    }
                });
            }
        });
}

function submitFormWithProgress(formId) {
    const form = document.getElementById(formId);
    const formData = new FormData(form);
    
    showLoading('Đang xử lý...', 'Vui lòng đợi');
    
    $.ajax({
        url: form.action,
        type: form.method,
        data: formData,
        processData: false,
        contentType: false,
        success: function(response) {
            hideLoading();
            if (response.success) {
                showSuccess('Thành công', response.message || 'Dữ liệu đã được lưu.');
            } else {
                showError('Có lỗi xảy ra', response.message || 'Vui lòng kiểm tra lại thông tin.');
            }
        },
        error: function() {
            hideLoading();
            showError('Lỗi kết nối', 'Không thể kết nối đến máy chủ. Vui lòng thử lại.');
        }
    });
}
*/
