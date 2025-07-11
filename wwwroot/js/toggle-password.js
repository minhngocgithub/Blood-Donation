document.addEventListener('DOMContentLoaded', function() {
    document.querySelectorAll('.toggle-password').forEach(function(btn) {
        btn.addEventListener('click', function() {
            var input = document.getElementById(btn.getAttribute('data-target'));
            if (input.type === 'password') {
                input.type = 'text';
                btn.querySelector('i').classList.remove('fa-eye');
                btn.querySelector('i').classList.add('fa-eye-slash');
            } else {
                input.type = 'password';
                btn.querySelector('i').classList.remove('fa-eye-slash');
                btn.querySelector('i').classList.add('fa-eye');
            }
        });
    });
}); 