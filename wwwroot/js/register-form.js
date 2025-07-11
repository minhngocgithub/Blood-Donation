document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('form');
    const submitBtn = document.querySelector('button[type="submit"]');
    if (!form || !submitBtn) return;

    form.addEventListener('submit', function(e) {
        const requiredFields = ['FullName', 'Email', 'Phone', 'Password', 'ConfirmPassword'];
        const missingFields = [];
        requiredFields.forEach(field => {
            const input = document.querySelector(`[name="${field}"]`);
            if (input && !input.value.trim()) {
                missingFields.push(field);
            }
        });
        const agreeToTermsCheckbox = document.querySelector('#agreeToTerms');
        if (agreeToTermsCheckbox && !agreeToTermsCheckbox.checked) {
            missingFields.push('AgreeToTerms');
        }
        if (missingFields.length > 0) {
            e.preventDefault();
            const fieldNames = {
                'FullName': 'Họ và tên',
                'Email': 'Email',
                'Phone': 'Số điện thoại',
                'Password': 'Mật khẩu',
                'ConfirmPassword': 'Xác nhận mật khẩu',
                'AgreeToTerms': 'Đồng ý điều khoản'
            };
            const missingFieldsVi = missingFields.map(field => fieldNames[field] || field);
            alert('Vui lòng điền đầy đủ các trường bắt buộc: ' + missingFieldsVi.join(', '));
            return false;
        }
        submitBtn.disabled = true;
        submitBtn.innerHTML = 'Đang đăng ký...';
    });
    const phoneInput = document.querySelector('[name="Phone"]');
    if (phoneInput) {
        phoneInput.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 11) {
                value = value.slice(0, 11);
            }
            e.target.value = value;
        });
    }
}); 