# Hướng dẫn sử dụng SweetAlert trong Blood Donation Website

## Giới thiệu

SweetAlert2 đã được tích hợp vào hệ thống với các tính năng tùy chỉnh phù hợp với giao diện ứng dụng hiến máu. Bạn có thể sử dụng các thông báo đẹp và thân thiện với người dùng thay vì `alert()` mặc định của browser.

## Cấu trúc Files

```
wwwroot/
├── js/
│   ├── sweetalert-helper.js      # Các function helper
│   └── logout.js                 # Đã cập nhật sử dụng SweetAlert
├── css/
│   └── sweetalert-custom.css     # CSS tùy chỉnh
Views/
├── Shared/
│   ├── _Layout.cshtml            # Layout chính (đã tích hợp SweetAlert)
│   ├── _AuthLayout.cshtml        # Layout xác thực (đã tích hợp SweetAlert)
│   └── _SweetAlertNotifications.cshtml  # Partial xử lý TempData
├── Home/
│   └── SweetAlertDemo.cshtml     # Trang demo các tính năng
Extensions/
└── SweetAlertExtensions.cs       # Extension methods cho Controllers
Examples/
└── SweetAlert_Usage_Examples.cs  # Ví dụ sử dụng chi tiết
```

## Sử dụng cơ bản

### 1. Trong JavaScript (Frontend)

```javascript
// Thông báo thành công
showSuccess('Đăng ký thành công!', 'Cảm ơn bạn đã tham gia hiến máu.');

// Thông báo lỗi
showError('Có lỗi xảy ra', 'Không thể kết nối đến máy chủ.');

// Thông báo cảnh báo
showWarning('Cảnh báo', 'Bạn cần nghỉ ít nhất 3 tháng trước khi hiến máu tiếp.');

// Thông báo thông tin
showInfo('Thông tin', 'Lần hiến máu tiếp theo: 15/10/2024');

// Hộp thoại xác nhận
showConfirm('Xác nhận đăng ký', 'Bạn có chắc chắn muốn đăng ký hiến máu?')
    .then((result) => {
        if (result.isConfirmed) {
            // Thực hiện action
        }
    });

// Xác nhận xóa
showDeleteConfirm('người dùng này', 'Tất cả dữ liệu liên quan cũng sẽ bị xóa.')
    .then((result) => {
        if (result.isConfirmed) {
            // Thực hiện xóa
        }
    });

// Toast notifications
showToast('success', 'Đã lưu thành công!');
showToast('error', 'Không thể kết nối!', 5000, 'bottom-end');

// Loading overlay
showLoading('Đang xử lý...', 'Vui lòng đợi');
// Sau khi hoàn thành:
hideLoading();

// Form input
showInputForm('Thêm ghi chú', 'Ghi chú:', 'Nhập ghi chú...')
    .then((result) => {
        if (result.isConfirmed) {
            console.log('Input value:', result.value);
        }
    });
```

### 2. Trong Controller (Backend)

#### Sử dụng Extension Methods:

```csharp
using Blood_Donation_Website.Extensions;

public class ExampleController : Controller
{
    public IActionResult Register()
    {
        try
        {
            // Logic đăng ký...
            
            this.Success("Đăng ký thành công!", "Chúng tôi sẽ liên hệ với bạn sớm nhất.");
            return RedirectToAction("Index");
        }
        catch
        {
            this.Error("Có lỗi xảy ra", ex.Message);
            return View();
        }
    }

    public IActionResult DeleteUser(int id)
    {
        // Logic xóa...
        
        this.ToastSuccess("Đã xóa thành công!");
        return RedirectToAction("Index");
    }

    public IActionResult ShowInfo()
    {
        var html = @"
            <div class='alert alert-info'>
                <i class='fas fa-info-circle me-2'></i>
                Thông tin chi tiết về hiến máu
            </div>";
            
        this.CustomHTML("Thông tin hiến máu", html, "info");
        return RedirectToAction("Index");
    }
}
```

#### Sử dụng TempData trực tiếp:

```csharp
public IActionResult SimpleExample()
{
    // Thông báo đơn giản
    TempData["SuccessMessage"] = "Thao tác thành công!";
    TempData["ErrorMessage"] = "Có lỗi xảy ra!";
    TempData["WarningMessage"] = "Cảnh báo!";
    TempData["InfoMessage"] = "Thông tin!";
    
    return RedirectToAction("Index");
}
```

### 3. Templates chuyên biệt cho Hiến máu

```csharp
using Blood_Donation_Website.Extensions;

// Đăng ký hiến máu thành công
this.BloodDonationRegistrationSuccess("Ngày hiến máu nhân đạo", DateTime.Now.AddDays(7));

// Hiến máu thành công
this.BloodDonationSuccess(450, DateTime.Now.AddMonths(3));

// Cần kiểm tra sức khỏe
var tests = new List<string> { "Xét nghiệm máu", "Đo huyết áp", "Kiểm tra tim mạch" };
this.HealthCheckRequired(tests);

// Nhắc nhở hiến máu
this.DonationReminder("Sự kiện hiến máu", DateTime.Now.AddDays(3), "Trung tâm Y tế Quận 1");
```

## Tính năng nâng cao

### 1. AJAX với SweetAlert

```javascript
function submitFormWithConfirm(formId) {
    showConfirm('Xác nhận gửi', 'Bạn có chắc chắn muốn gửi form này?')
        .then((result) => {
            if (result.isConfirmed) {
                showLoading('Đang xử lý...', 'Vui lòng đợi');
                
                const form = document.getElementById(formId);
                const formData = new FormData(form);
                
                fetch(form.action, {
                    method: 'POST',
                    body: formData
                })
                .then(response => response.json())
                .then(data => {
                    hideLoading();
                    if (data.success) {
                        showSuccess('Thành công', data.message);
                    } else {
                        showError('Có lỗi xảy ra', data.message);
                    }
                })
                .catch(error => {
                    hideLoading();
                    showError('Lỗi kết nối', 'Không thể kết nối đến máy chủ.');
                });
            }
        });
}
```

### 2. Progress Bar cho tác vụ dài

```javascript
function processLongTask() {
    let progress = 0;
    showProgress('Đang xử lý dữ liệu', progress);
    
    const interval = setInterval(() => {
        progress += 10;
        showProgress('Đang xử lý dữ liệu', progress);
        
        if (progress >= 100) {
            clearInterval(interval);
            setTimeout(() => {
                hideLoading();
                showSuccess('Hoàn thành', 'Tác vụ đã được thực hiện thành công!');
            }, 500);
        }
    }, 300);
}
```

### 3. Validation với SweetAlert

```javascript
function validateAndSubmit() {
    const email = document.getElementById('email').value;
    const phone = document.getElementById('phone').value;
    
    if (!email || !phone) {
        showWarning('Thiếu thông tin', 'Vui lòng điền đầy đủ email và số điện thoại.');
        return false;
    }
    
    if (!isValidEmail(email)) {
        showError('Email không hợp lệ', 'Vui lòng nhập đúng định dạng email.');
        return false;
    }
    
    // Submit form...
    showLoading('Đang gửi...', 'Vui lòng đợi');
    return true;
}
```

## Tùy chỉnh giao diện

### 1. Sử dụng theme tùy chỉnh

```javascript
// Blood theme (đỏ)
Swal.fire({
    title: 'Hiến máu thành công',
    text: 'Cảm ơn bạn đã cứu sống 3 người!',
    icon: 'success',
    customClass: {
        popup: 'swal-blood-theme'
    }
});

// Success theme (xanh)
Swal.fire({
    title: 'Thành công',
    icon: 'success',
    customClass: {
        popup: 'swal-success-theme'
    }
});
```

### 2. Responsive design

CSS đã được tối ưu cho mobile:
- Font size tự động điều chỉnh
- Button size phù hợp với màn hình nhỏ
- Popup width responsive

## Testing

Truy cập `/Home/SweetAlertDemo` để xem demo đầy đủ các tính năng và test các thông báo.

## Lưu ý quan trọng

1. **Encoding**: Luôn encode text để tránh XSS attacks
2. **Performance**: Không gọi quá nhiều SweetAlert cùng lúc
3. **UX**: Sử dụng timer phù hợp cho từng loại thông báo
4. **Accessibility**: SweetAlert hỗ trợ keyboard navigation
5. **Mobile**: Test kỹ trên mobile devices

## Troubleshooting

### 1. SweetAlert không hiển thị
- Kiểm tra CDN links trong Layout
- Kiểm tra console errors
- Đảm bảo `sweetalert-helper.js` đã load

### 2. Extension methods không hoạt động
- Thêm `using Blood_Donation_Website.Extensions;`
- Kiểm tra namespace

### 3. TempData không hiển thị
- Đảm bảo `_SweetAlertNotifications.cshtml` đã được include
- Kiểm tra JSON serialization

## Liên hệ

Nếu có vấn đề gì, vui lòng tạo issue hoặc liên hệ team phát triển.
