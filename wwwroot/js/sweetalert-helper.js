/**
 * SweetAlert Helper Functions
 * Cung cấp các function tiện ích để sử dụng SweetAlert trong toàn bộ ứng dụng
 */

// Cấu hình mặc định cho SweetAlert
const defaultSwalConfig = {
    customClass: {
        confirmButton: 'btn btn-primary me-2',
        cancelButton: 'btn btn-secondary',
        denyButton: 'btn btn-danger',
        popup: 'swal2-popup',
        title: 'swal2-title',
        'html-container': 'swal2-html-container'
    },
    buttonsStyling: false,
    showCloseButton: true,
    allowOutsideClick: false,
    allowEscapeKey: true
};

/**
 * Hiển thị thông báo thành công
 * @param {string} title - Tiêu đề thông báo
 * @param {string} text - Nội dung thông báo (tùy chọn)
 * @param {number} timer - Thời gian tự động đóng (ms, mặc định 3000)
 */
function showSuccess(title, text = '', timer = 3000) {
    return Swal.fire({
        icon: 'success',
        title: title,
        text: text,
        timer: timer,
        timerProgressBar: true,
        showConfirmButton: false,
        toast: false,
        position: 'center',
        ...defaultSwalConfig
    });
}

/**
 * Hiển thị thông báo lỗi
 * @param {string} title - Tiêu đề thông báo
 * @param {string} text - Nội dung thông báo (tùy chọn)
 */
function showError(title, text = '') {
    return Swal.fire({
        icon: 'error',
        title: title,
        text: text,
        confirmButtonText: 'Đồng ý',
        ...defaultSwalConfig
    });
}

/**
 * Hiển thị thông báo cảnh báo
 * @param {string} title - Tiêu đề thông báo
 * @param {string} text - Nội dung thông báo (tùy chọn)
 */
function showWarning(title, text = '') {
    return Swal.fire({
        icon: 'warning',
        title: title,
        text: text,
        confirmButtonText: 'Đồng ý',
        ...defaultSwalConfig
    });
}

/**
 * Hiển thị thông báo thông tin
 * @param {string} title - Tiêu đề thông báo
 * @param {string} text - Nội dung thông báo (tùy chọn)
 */
function showInfo(title, text = '') {
    return Swal.fire({
        icon: 'info',
        title: title,
        text: text,
        confirmButtonText: 'Đồng ý',
        ...defaultSwalConfig
    });
}

/**
 * Hiển thị hộp thoại xác nhận
 * @param {string} title - Tiêu đề xác nhận
 * @param {string} text - Nội dung xác nhận
 * @param {string} confirmText - Text nút xác nhận (mặc định: "Có, tiếp tục!")
 * @param {string} cancelText - Text nút hủy (mặc định: "Hủy bỏ")
 * @param {string} icon - Icon hiển thị (mặc định: "question")
 */
function showConfirm(title, text = '', confirmText = 'Có, tiếp tục!', cancelText = 'Hủy bỏ', icon = 'question') {
    return Swal.fire({
        icon: icon,
        title: title,
        text: text,
        showCancelButton: true,
        confirmButtonText: confirmText,
        cancelButtonText: cancelText,
        reverseButtons: true,
        ...defaultSwalConfig
    });
}

/**
 * Hiển thị hộp thoại xác nhận xóa
 * @param {string} itemName - Tên item cần xóa
 * @param {string} additionalInfo - Thông tin bổ sung (tùy chọn)
 */
function showDeleteConfirm(itemName = 'mục này', additionalInfo = '') {
    const text = additionalInfo ? 
        `Bạn có chắc chắn muốn xóa ${itemName}? ${additionalInfo}` : 
        `Bạn có chắc chắn muốn xóa ${itemName}?`;
    
    return Swal.fire({
        icon: 'warning',
        title: 'Xóa dữ liệu',
        text: text,
        html: `
            <div class="text-start">
                <p class="mb-2">${text}</p>
                <div class="alert alert-warning small mb-0">
                    <i class="fas fa-exclamation-triangle me-1"></i>
                    Hành động này không thể hoàn tác!
                </div>
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: '<i class="fas fa-trash me-1"></i>Xóa',
        cancelButtonText: '<i class="fas fa-times me-1"></i>Hủy bỏ',
        reverseButtons: true,
        customClass: {
            ...defaultSwalConfig.customClass,
            confirmButton: 'btn btn-danger me-2',
            cancelButton: 'btn btn-secondary'
        },
        buttonsStyling: false
    });
}

/**
 * Hiển thị toast notification (góc màn hình)
 * @param {string} type - Loại toast (success, error, warning, info)
 * @param {string} title - Tiêu đề toast
 * @param {number} timer - Thời gian hiển thị (ms, mặc định 3000)
 * @param {string} position - Vị trí hiển thị (mặc định: 'top-end')
 */
function showToast(type, title, timer = 3000, position = 'top-end') {
    const Toast = Swal.mixin({
        toast: true,
        position: position,
        showConfirmButton: false,
        timer: timer,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer);
            toast.addEventListener('mouseleave', Swal.resumeTimer);
        }
    });

    return Toast.fire({
        icon: type,
        title: title
    });
}

/**
 * Hiển thị loading overlay
 * @param {string} title - Tiêu đề loading (mặc định: "Đang xử lý...")
 * @param {string} text - Text loading (tùy chọn)
 */
function showLoading(title = 'Đang xử lý...', text = '') {
    return Swal.fire({
        title: title,
        text: text,
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

/**
 * Đóng loading overlay
 */
function hideLoading() {
    Swal.close();
}

/**
 * Hiển thị form input đơn giản
 * @param {string} title - Tiêu đề form
 * @param {string} inputLabel - Label của input
 * @param {string} inputPlaceholder - Placeholder của input
 * @param {string} inputValue - Giá trị mặc định (tùy chọn)
 * @param {string} inputType - Loại input (text, email, password, etc.)
 */
function showInputForm(title, inputLabel, inputPlaceholder = '', inputValue = '', inputType = 'text') {
    return Swal.fire({
        title: title,
        html: `
            <div class="mb-3 text-start">
                <label class="form-label">${inputLabel}</label>
                <input type="${inputType}" class="form-control" id="swal-input" 
                       placeholder="${inputPlaceholder}" value="${inputValue}">
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'Xác nhận',
        cancelButtonText: 'Hủy bỏ',
        preConfirm: () => {
            const input = document.getElementById('swal-input');
            if (!input.value.trim()) {
                Swal.showValidationMessage('Vui lòng nhập thông tin!');
                return false;
            }
            return input.value.trim();
        },
        ...defaultSwalConfig
    });
}

/**
 * Hiển thị thông báo với custom HTML
 * @param {string} title - Tiêu đề
 * @param {string} html - Nội dung HTML
 * @param {string} icon - Icon (tùy chọn)
 */
function showCustomHTML(title, html, icon = null) {
    const config = {
        title: title,
        html: html,
        confirmButtonText: 'Đóng',
        ...defaultSwalConfig
    };
    
    if (icon) {
        config.icon = icon;
    }
    
    return Swal.fire(config);
}

/**
 * Hiển thị progress bar
 * @param {string} title - Tiêu đề
 */
function showProgress(title) {
    Swal.fire({
        title: title,
        html: `
            <div class="progress mb-3">
                <div class="progress-bar bg-primary" id="progress-bar"
                     role="progressbar" style="width: 0%" aria-valuemin="0" aria-valuemax="100">
                    0%
                </div>
            </div>
        `,
        showConfirmButton: false,
        allowOutsideClick: false,
        allowEscapeKey: false
    });
}

function updateProgress(value) {
    const bar = document.getElementById('progress-bar');
    if (bar) {
        bar.style.width = `${value}%`;
        bar.setAttribute('aria-valuenow', value);
        bar.innerText = `${value}%`;
    }
}

// Xuất các functions để sử dụng global
window.showSuccess = showSuccess;
window.showError = showError;
window.showWarning = showWarning;
window.showInfo = showInfo;
window.showConfirm = showConfirm;
window.showDeleteConfirm = showDeleteConfirm;
window.showToast = showToast;
window.showLoading = showLoading;
window.hideLoading = hideLoading;
window.showInputForm = showInputForm;
window.showCustomHTML = showCustomHTML;
window.showProgress = showProgress;
window.updateProgress = updateProgress;