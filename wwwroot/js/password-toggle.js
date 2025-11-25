/**
 * Password Visibility Toggle
 * 
 * This script adds password visibility toggle functionality to password input fields.
 * It adds an eye icon button next to password fields that allows users to show/hide
 * the password text.
 */

document.addEventListener('DOMContentLoaded', function () {
    // Find all password input fields
    const passwordFields = document.querySelectorAll('input[type="password"]');
    
    passwordFields.forEach(function (passwordField) {
        // Create the container for positioning
        const container = document.createElement('div');
        container.classList.add('password-field-container');
        
        // Get the parent element
        const parent = passwordField.parentNode;
        
        // Insert the container before the password field
        parent.insertBefore(container, passwordField);
        
        // Move the password field inside the container
        container.appendChild(passwordField);
        
        // Create the toggle button
        const toggleButton = document.createElement('button');
        toggleButton.type = 'button';
        toggleButton.classList.add('password-toggle-btn');
        toggleButton.innerHTML = '<i class="fa fa-eye-slash"></i>';
        toggleButton.setAttribute('aria-label', 'Hiện/ẩn mật khẩu');
        
        // Add the toggle button to the container
        container.appendChild(toggleButton);
        
        // Add event listener to toggle button
        toggleButton.addEventListener('click', function () {
            // Toggle the password field type
            if (passwordField.type === 'password') {
                passwordField.type = 'text';
                toggleButton.innerHTML = '<i class="fa fa-eye"></i>';
            } else {
                passwordField.type = 'password';
                toggleButton.innerHTML = '<i class="fa fa-eye-slash"></i>';
            }
        });
    });
});
