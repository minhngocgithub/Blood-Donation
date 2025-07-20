/**
 * Event Management JavaScript Functions
 * Handles event management operations like reminders, exports, and status updates
 */

// Global variables
let currentEventId = null;
let isLoading = false;

/**
 * Initialize event management functionality
 */
document.addEventListener('DOMContentLoaded', function() {
    initializeEventManagement();
});

/**
 * Initialize all event management features
 */
function initializeEventManagement() {
    // Add loading overlay
    addLoadingOverlay();
    
    // Initialize tooltips
    initializeTooltips();
    
    // Initialize event listeners
    initializeEventListeners();
    
    // Show page load animation
    showPageLoadAnimation();
}

/**
 * Add loading overlay to the page
 */
function addLoadingOverlay() {
    const overlay = document.createElement('div');
    overlay.className = 'loading-overlay';
    overlay.id = 'loadingOverlay';
    overlay.style.display = 'none';
    
    const spinner = document.createElement('div');
    spinner.className = 'loading-spinner';
    
    overlay.appendChild(spinner);
    document.body.appendChild(overlay);
}

/**
 * Show loading overlay
 */
function showLoading() {
    const overlay = document.getElementById('loadingOverlay');
    if (overlay) {
        overlay.style.display = 'flex';
        isLoading = true;
    }
}

/**
 * Hide loading overlay
 */
function hideLoading() {
    const overlay = document.getElementById('loadingOverlay');
    if (overlay) {
        overlay.style.display = 'none';
        isLoading = false;
    }
}

/**
 * Initialize Bootstrap tooltips
 */
function initializeTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

/**
 * Initialize event listeners
 */
function initializeEventListeners() {
    // Quick action buttons
    const quickActionButtons = document.querySelectorAll('.quick-actions .btn');
    quickActionButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            if (isLoading) {
                e.preventDefault();
                return;
            }
            
            const action = this.getAttribute('data-action');
            const eventId = this.getAttribute('data-event-id');
            
            if (action && eventId) {
                handleQuickAction(action, eventId);
            }
        });
    });
    
    // Export format selector
    const exportFormatSelect = document.getElementById('exportFormat');
    if (exportFormatSelect) {
        exportFormatSelect.addEventListener('change', function() {
            updateExportButton(this.value);
        });
    }
}

/**
 * Handle quick action button clicks
 */
function handleQuickAction(action, eventId) {
    switch (action) {
        case 'complete':
            updateEventStatus('Completed', eventId);
            break;
        case 'postpone':
            updateEventStatus('Postponed', eventId);
            break;
        case 'cancel':
            updateEventStatus('Cancelled', eventId);
            break;
        case 'activate':
            updateEventStatus('Active', eventId);
            break;
        case 'reminder':
            sendReminders(eventId);
            break;
        case 'export':
            exportEventData(eventId);
            break;
        default:
            console.warn('Unknown action:', action);
    }
}

/**
 * Update export button based on format selection
 */
function updateExportButton(format) {
    const exportButton = document.querySelector('[data-action="export"]');
    if (exportButton) {
        const icon = format === 'csv' ? 'fa-file-csv' : 'fa-file-code';
        const text = format === 'csv' ? 'Xuất CSV' : 'Xuất JSON';
        
        exportButton.innerHTML = `<i class="fas ${icon} me-2"></i>${text}`;
    }
}

/**
 * Show page load animation
 */
function showPageLoadAnimation() {
    const mainContent = document.querySelector('.container');
    if (mainContent) {
        mainContent.classList.add('fade-in');
    }
}

/**
 * Update event status
 */
function updateEventStatus(status, eventId) {
    const statusText = {
        'Active': 'kích hoạt',
        'Completed': 'hoàn thành',
        'Postponed': 'tạm hoãn',
        'Cancelled': 'hủy bỏ'
    };
    
    const eventName = document.querySelector('.event-name')?.textContent || 'sự kiện';
    
    showConfirm(
        'Cập nhật trạng thái sự kiện',
        `Bạn có chắc chắn muốn ${statusText[status]} sự kiện "${eventName}"?`,
        'Có, cập nhật',
        'Hủy bỏ',
        'question'
    ).then((result) => {
        if (result.isConfirmed) {
            showLoading();
            
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = `/admin/events/status/${eventId}`;
            
            const statusInput = document.createElement('input');
            statusInput.type = 'hidden';
            statusInput.name = 'status';
            statusInput.value = status;
            
            const token = document.createElement('input');
            token.type = 'hidden';
            token.name = '__RequestVerificationToken';
            token.value = document.querySelector('input[name="__RequestVerificationToken"]').value;
            
            form.appendChild(statusInput);
            form.appendChild(token);
            document.body.appendChild(form);
            form.submit();
        }
    });
}

/**
 * Send reminders to event registrants
 */
function sendReminders(eventId) {
    showConfirm(
        'Gửi nhắc nhở',
        'Bạn có chắc chắn muốn gửi nhắc nhở cho tất cả người đăng ký?',
        'Có, gửi nhắc nhở',
        'Hủy bỏ',
        'question'
    ).then((result) => {
        if (result.isConfirmed) {
            showLoading();
            
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = `/admin/events/reminders/${eventId}`;
            
            const token = document.createElement('input');
            token.type = 'hidden';
            token.name = '__RequestVerificationToken';
            token.value = document.querySelector('input[name="__RequestVerificationToken"]').value;
            
            form.appendChild(token);
            document.body.appendChild(form);
            form.submit();
        }
    });
}

/**
 * Export event data
 */
function exportEventData(eventId) {
    const format = document.getElementById('exportFormat')?.value || 'json';
    
    showConfirm(
        'Xuất dữ liệu sự kiện',
        `Bạn có muốn xuất dữ liệu chi tiết của sự kiện này? (Định dạng: ${format.toUpperCase()})`,
        'Có, xuất dữ liệu',
        'Hủy bỏ',
        'question'
    ).then((result) => {
        if (result.isConfirmed) {
            showLoading();
            
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = `/admin/events/export/${eventId}`;
            
            const formatInput = document.createElement('input');
            formatInput.type = 'hidden';
            formatInput.name = 'format';
            formatInput.value = format;
            
            const token = document.createElement('input');
            token.type = 'hidden';
            token.name = '__RequestVerificationToken';
            token.value = document.querySelector('input[name="__RequestVerificationToken"]').value;
            
            form.appendChild(formatInput);
            form.appendChild(token);
            document.body.appendChild(form);
            form.submit();
        }
    });
}

/**
 * Show confirmation dialog using SweetAlert
 */
function showConfirm(title, text, confirmText, cancelText, icon) {
    return Swal.fire({
        title: title,
        text: text,
        icon: icon || 'question',
        showCancelButton: true,
        confirmButtonColor: '#007bff',
        cancelButtonColor: '#6c757d',
        confirmButtonText: confirmText || 'Có',
        cancelButtonText: cancelText || 'Không',
        reverseButtons: true
    });
}

/**
 * Show success message
 */
function showSuccess(title, message) {
    Swal.fire({
        title: title,
        text: message,
        icon: 'success',
        confirmButtonColor: '#28a745'
    });
}

/**
 * Show error message
 */
function showError(title, message) {
    Swal.fire({
        title: title,
        text: message,
        icon: 'error',
        confirmButtonColor: '#dc3545'
    });
}

/**
 * Show warning message
 */
function showWarning(title, message) {
    Swal.fire({
        title: title,
        text: message,
        icon: 'warning',
        confirmButtonColor: '#ffc107'
    });
}

/**
 * Show info message
 */
function showInfo(title, message) {
    Swal.fire({
        title: title,
        text: message,
        icon: 'info',
        confirmButtonColor: '#17a2b8'
    });
}

/**
 * Show loading message
 */
function showLoadingMessage(title, message) {
    Swal.fire({
        title: title,
        text: message,
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

/**
 * Close SweetAlert
 */
function closeSweetAlert() {
    Swal.close();
}

/**
 * Refresh page after successful operation
 */
function refreshPage() {
    setTimeout(() => {
        window.location.reload();
    }, 1500);
}

/**
 * Format date for display
 */
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

/**
 * Format time for display
 */
function formatTime(timeString) {
    if (!timeString) return '--';
    
    const time = new Date(`2000-01-01T${timeString}`);
    return time.toLocaleTimeString('vi-VN', {
        hour: '2-digit',
        minute: '2-digit'
    });
}

/**
 * Calculate percentage
 */
function calculatePercentage(current, total) {
    if (total === 0) return 0;
    return Math.round((current / total) * 100);
}

/**
 * Get status class for badges
 */
function getStatusClass(status) {
    const statusClasses = {
        'Active': 'bg-success',
        'Draft': 'bg-secondary',
        'Published': 'bg-primary',
        'Completed': 'bg-info',
        'Cancelled': 'bg-danger',
        'Postponed': 'bg-warning',
        'Full': 'bg-dark',
        'Closed': 'bg-secondary'
    };
    
    return statusClasses[status] || 'bg-secondary';
}

/**
 * Update progress bar
 */
function updateProgressBar(percentage, elementId) {
    const progressBar = document.getElementById(elementId);
    if (progressBar) {
        progressBar.style.width = `${Math.min(percentage, 100)}%`;
        
        // Update color based on percentage
        if (percentage >= 100) {
            progressBar.className = 'progress-bar bg-danger';
        } else if (percentage >= 80) {
            progressBar.className = 'progress-bar bg-warning';
        } else {
            progressBar.className = 'progress-bar bg-success';
        }
    }
}

/**
 * Animate counter
 */
function animateCounter(element, target, duration = 1000) {
    const start = 0;
    const increment = target / (duration / 16);
    let current = start;
    
    const timer = setInterval(() => {
        current += increment;
        if (current >= target) {
            current = target;
            clearInterval(timer);
        }
        element.textContent = Math.floor(current);
    }, 16);
}

/**
 * Initialize counter animations
 */
function initializeCounters() {
    const counters = document.querySelectorAll('.counter');
    counters.forEach(counter => {
        const target = parseInt(counter.getAttribute('data-target'));
        if (target) {
            animateCounter(counter, target);
        }
    });
}

// Export functions for global use
window.EventManagement = {
    updateEventStatus,
    sendReminders,
    exportEventData,
    showConfirm,
    showSuccess,
    showError,
    showWarning,
    showInfo,
    showLoadingMessage,
    closeSweetAlert,
    refreshPage,
    formatDate,
    formatTime,
    calculatePercentage,
    getStatusClass,
    updateProgressBar,
    animateCounter,
    initializeCounters
}; 